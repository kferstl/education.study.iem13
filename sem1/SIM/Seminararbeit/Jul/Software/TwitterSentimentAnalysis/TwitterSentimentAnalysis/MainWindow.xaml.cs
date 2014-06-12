using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


using TwitterSentimentAnalysis.twitter;
using TwitterSentimentAnalysis.data;
using TwitterSentimentAnalysis.classify;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using De.TorstenMandelkow.MetroChart;
using TwitterSentimentAnalysis.claissify;


namespace TwitterSentimentAnalysis {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	/// 
	public partial class MainWindow : Window, INotifyPropertyChanged {

		private const string Twitter_userAccessToken = "296364810-94ju4YAFSjVUV29JayjKwqIkJwHjJKaw8d4mjmrj";
		private const string Twitter_userAccessSecret = "nXQuvGr9m0BzuJFoSj1LGKxKdup9hK8IsNYYzkAz9rjet";
		private const string Twitter_consumerKey = "eiHvFURg52Yomn0xEIBPJ6inL";
		private const string Twitter_consumerSecret = "areKDP4TrO3CWijEgIbzytJ2GKn4Ax8VpgRwUfLAI3tb8pCPiA";

		private const string DatumBox_apiKey = "a76853008d862cdc98cb1c5bc6973f57";
		private const string UClassify_apiKey = "AkcxXQA4jrxrvpaFMKhAA9KxRzI";

		TwitterWorker _twitterWrk;
		TweetStorage _tweetStorage;
		List<ClassifyWorker> _datumBoxWrks;

		private CancellationTokenSource _appClosingToken = new CancellationTokenSource();

		private List<string> _keywords = new List<string>();

		private IClassifier _clssifier;

		public MainWindow() {

			InitializeComponent();

			_clssifier = new FakeClassify();
			//_clssifier = new UclassifyAPI(UClassify_apiKey);

			_tweetStorage = new TweetStorage();

			_twitterWrk = new TwitterWorker(_tweetStorage);
			_twitterWrk.SetCredentials(	Twitter_userAccessToken,
										Twitter_userAccessSecret,
										Twitter_consumerKey,
										Twitter_consumerSecret);


			_keywords.Add("worldcup");

			foreach (var keyword in _keywords) {
				_twitterWrk.AddTrack(keyword);
			}


			_datumBoxWrks = new List<ClassifyWorker>();

			initViewModel();

			_twitterWrk.StartStreamAsync();

			for (var i = 0; i < 5; i++) {
				var worker = new ClassifyWorker(_clssifier, _tweetStorage);
				worker.StartWorkAsync(_appClosingToken.Token);
				_datumBoxWrks.Add(worker);
			}


			DataContext = this;
			WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
			WindowState = System.Windows.WindowState.Maximized;
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			_twitterWrk.StopWork();
			_appClosingToken.Cancel();
		}



		#region VIEW MODEL
		ObservableCollection<TestClass> _errors;
		public ObservableCollection<TestClass> Errors {
			get {
				return _errors;
			}
			private set {
				_errors = value;
			}
		}

		public ObservableCollection<DisplayItem> Counts { get; private set; }
		DisplayItem[] _tweetCount;

		public ObservableCollection<DisplayItem> Sentiment { get; private set; }
		DisplayItem[] _tweetCountSentiment;

		public ObservableCollection<DisplayItem> TopCountries { get; private set; }
		DisplayItem[] _tweetCountTopCountries;

		public ObservableCollection<DisplayItem> TopLanguages { get; private set; }
		DisplayItem[] _tweetCountTopLanguages;

		public ObservableCollection<DisplayItem> TopLanguagesPositiv { get; private set; }
		DisplayItem[] _tweetCountTopLanguagesPositiv;

		public ObservableCollection<DisplayItem> TopLanguagesNegativ { get; private set; }
		DisplayItem[] _tweetCountTopLanguagesNegativ;

		private void initViewModel() {
			_tweetStorage.DataUpdated += _tweetStorage_DataUpdated;


			Counts = new ObservableCollection<DisplayItem>();
			_tweetCount = new DisplayItem[3];
			_tweetCount[0] = new DisplayItem() { Name = "Analysed" };
			_tweetCount[1] = new DisplayItem() { Name = "ToAnalyse" };
			_tweetCount[2] = new DisplayItem() { Name = "Error" };
			Counts.Add(_tweetCount[0]);
			Counts.Add(_tweetCount[1]);
			Counts.Add(_tweetCount[2]);

			Sentiment = new ObservableCollection<DisplayItem>();
			_tweetCountSentiment = new DisplayItem[2];
			_tweetCountSentiment[0] = new DisplayItem() { Name = "Positive" };
			_tweetCountSentiment[1] = new DisplayItem() { Name = "Beteiligung" };
			Sentiment.Add(_tweetCountSentiment[0]);
			Sentiment.Add(_tweetCountSentiment[1]);

			TopCountries = new ObservableCollection<DisplayItem>();
			_tweetCountTopCountries = new DisplayItem[5];
			for (int i = 0; i < _tweetCountTopCountries.Length; i++) {
				_tweetCountTopCountries[i] = new DisplayItem();
				_tweetCountTopCountries[i].Name = i.ToString();
				TopCountries.Add(_tweetCountTopCountries[i]);
			}

			TopLanguages = new ObservableCollection<DisplayItem>();
			_tweetCountTopLanguages = new DisplayItem[5];
			for (int i = 0; i < _tweetCountTopLanguages.Length; i++) {
				_tweetCountTopLanguages[i] = new DisplayItem();
				_tweetCountTopLanguages[i].Name = i.ToString();
				TopLanguages.Add(_tweetCountTopLanguages[i]);
			}

			TopLanguagesPositiv = new ObservableCollection<DisplayItem>();
			_tweetCountTopLanguagesPositiv = new DisplayItem[5];
			for (int i = 0; i < _tweetCountTopLanguagesPositiv.Length; i++) {
				_tweetCountTopLanguagesPositiv[i] = new DisplayItem();
				_tweetCountTopLanguagesPositiv[i].Name = i.ToString();
				TopLanguagesPositiv.Add(_tweetCountTopLanguagesPositiv[i]);
			}

			TopLanguagesNegativ = new ObservableCollection<DisplayItem>();
			_tweetCountTopLanguagesNegativ = new DisplayItem[5];
			for (int i = 0; i < _tweetCountTopLanguagesNegativ.Length; i++) {
				_tweetCountTopLanguagesNegativ[i] = new DisplayItem();
				_tweetCountTopLanguagesNegativ[i].Name = i.ToString();
				TopLanguagesNegativ.Add(_tweetCountTopLanguagesNegativ[i]);
			}
				



			Errors = new ObservableCollection<TestClass>();

			Errors.Add(new TestClass() { Category = "Globalization", Number = 75 });
			Errors.Add(new TestClass() { Category = "Features", Number = 2 });
			Errors.Add(new TestClass() { Category = "ContentTypes", Number = 12 });
			Errors.Add(new TestClass() { Category = "Correctness", Number = 83 });
			Errors.Add(new TestClass() { Category = "Best Practices!!!!!!!", Number = 29 });


			Update();
		}


		
		void _tweetStorage_DataUpdated(object sender, EventArgs e) {
			Update();
		}

		public void Update() {
			this.Dispatcher.BeginInvoke(new Action(() => {

				_tweetCount[0].Value = _tweetStorage.GetPercentage(TweetStorageSelectBy.Analysed);
				_tweetCount[1].Value = _tweetStorage.GetPercentage(TweetStorageSelectBy.ToAnalyse);
				_tweetCount[2].Value = _tweetStorage.GetPercentage(TweetStorageSelectBy.Error);

				NotifyPropertyChanged("Counts");

				_tweetCountSentiment[0].Value = _tweetStorage.GetPercentage(TweetStorageSelectBy.Positive);
				_tweetCountSentiment[1].Value = _tweetStorage.GetPercentage(TweetStorageSelectBy.Involvement);
				NotifyPropertyChanged("Sentiment");

				int x = 0;
				foreach(var country in _tweetStorage.GetTopCountries()){
					if (x > _tweetCountTopCountries.Length - 1) {
						break;
					}
					ChartTopCounties.ChartLegendItems[x].Caption = country.Name;
					_tweetCountTopCountries[x].Name = country.Name;
					_tweetCountTopCountries[x].Value = country.Value;
					x++;
				}

				x = 0;
				foreach (var country in _tweetStorage.GetTopLanguages()) {
					if (x > _tweetCountTopLanguages.Length - 1) {
						break;
					}
					ChartTopLanguages.ChartLegendItems[x].Caption = country.Name;
					_tweetCountTopLanguages[x].Name = country.Name;
					_tweetCountTopLanguages[x].Value = country.Value;
					x++;
				}

				x = 0;
				foreach (var country in _tweetStorage.GetTopLanguages(TweetStorageSelectBy.Positive)) {
					if (x > _tweetCountTopLanguagesPositiv.Length - 1) {
						break;
					}
					ChartTopLanguagesPositiv.ChartLegendItems[x].Caption = country.Name;
					_tweetCountTopLanguagesPositiv[x].Name = country.Name;
					_tweetCountTopLanguagesPositiv[x].Value = country.Value;
					x++;
				}


				x = 0;
				foreach (var country in _tweetStorage.GetTopLanguages(TweetStorageSelectBy.Negative)) {
					if (x > _tweetCountTopLanguagesNegativ.Length - 1) {
						break;
					}
					ChartTopLanguagesNegativ.ChartLegendItems[x].Caption = country.Name;
					_tweetCountTopLanguagesNegativ[x].Name = country.Name;
					_tweetCountTopLanguagesNegativ[x].Value = country.Value;
					x++;
				}

				//ChartTop10Counties.InvalidateVisual();


				NotifyPropertyChanged("TopCountries");
				NotifyPropertyChanged("TopCountriesPositv");
				NotifyPropertyChanged("TopCountriesNegativ");

				NotifyPropertyChanged("TopLanguages");
				NotifyPropertyChanged("TopLanguagesPositv");
				NotifyPropertyChanged("TopLanguagesNegativ");

				PageTitle = String.Format("Twitter Sentiment Analysis | Keywords: {0} | Tweets recived: {1} | by Julian Nischler 2014", string.Join(" ", _keywords.ToArray()), _tweetStorage.CountTweets(TweetStorageSelectBy.All));
			}));
		}


		private string _title;
		public string PageTitle {
			get { return _title; }
			private set { _title = value; Title = value; NotifyPropertyChanged("PageTitle"); }
		}
		
		#endregion


		#region simple bindings
		bool darkLayout = false;
		public string ChartForeground {
			get {
				if (darkLayout) {
					return "#FFEEEEEE";
				}
				return "#FF666666";
			}
		}
		public string MainForeground {
			get {
				if (darkLayout) {
					return "#FFFFFFFF";
				}
				return "#FF666666";
			}
		}
		public string ChartBackground {
			get {
				if (darkLayout) {
					return "#FF333333";
				}
				return "#FFF9F9F9";
			}
		}
		public string MainBackground {
			get {
				if (darkLayout) {
					return "#FF000000";
				}
				return "#FFEFEFEF";
			}
		}
		private void NotifyPropertyChanged(string property) {
			if (PropertyChanged != null) {
				PropertyChanged.Invoke(this, new PropertyChangedEventArgs(property));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

	}

	// class which represent a data point in the chart
	public class TestClass {
		public string Category { get; set; }

		public int Number { get; set; }
	}
}

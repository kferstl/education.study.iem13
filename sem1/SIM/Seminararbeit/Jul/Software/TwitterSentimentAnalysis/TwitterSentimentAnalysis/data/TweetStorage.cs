using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Enum;
using System.ComponentModel;

namespace TwitterSentimentAnalysis.data {
	public class TweetStorage {
		private List<TweetStorageItem> _items;

		public TweetStorage() {
			_items = new List<TweetStorageItem>();
		}

		public void AddTweet(ITweet tweet) {
			lock (_items) {
				_items.Add(new TweetStorageItem(tweet));
			}
			Update();
		}

		public IEnumerable<DisplayItem> GetTopLanguages(TweetStorageSelectBy countType = TweetStorageSelectBy.All, int max = 10) {
			var res = (from item in _items
					   where SelectPredicate(countType)(item) && item.Language != TweetStorageItem.UnknownLanguage
					   group item by item.Language into grp
					   orderby grp.Count() descending
					   select new DisplayItem { Name = grp.Key.ToString(), Value = grp.Count() }).Take(max);
			return res;
		}

		public IEnumerable<DisplayItem> GetTopCountries(TweetStorageSelectBy countType = TweetStorageSelectBy.All, int max = 10){
			var res = (from item in _items
					  where SelectPredicate(countType)(item) && item.Country != TweetStorageItem.UnknownCountry
					  group item by item.Country into grp
					  orderby grp.Count() descending
					   select new DisplayItem { Name = grp.Key, Value = grp.Count() }).Take(max);
			return res;
		}

		private Func<TweetStorageItem,bool> SelectPredicate(TweetStorageSelectBy type = TweetStorageSelectBy.SkipErrors) {
			switch (type) {
				case TweetStorageSelectBy.All:
					return (p) => true;
				case TweetStorageSelectBy.SkipErrors:
					return (p) => p.Status != TweetStorageItemStatus.Error;
				case TweetStorageSelectBy.Analysed:
					return (p) => p.Status == TweetStorageItemStatus.Analysed;
				case TweetStorageSelectBy.ToAnalyse:
					return (p) => p.Status == TweetStorageItemStatus.New;
				case TweetStorageSelectBy.Error:
					return (p) => p.Status == TweetStorageItemStatus.Error;
				case TweetStorageSelectBy.Positive:
					return (p) => p.Sentiment == AnalysisResultSentiment.Positive;
				case TweetStorageSelectBy.Negative:
					return (p) => p.Sentiment == AnalysisResultSentiment.Negative;
				case TweetStorageSelectBy.Neutral:
					return (p) => p.Sentiment == AnalysisResultSentiment.Neutral;
				case TweetStorageSelectBy.Involvement:
					return (p) => p.Sentiment != AnalysisResultSentiment.Neutral && p.Sentiment != AnalysisResultSentiment.Error;
				case TweetStorageSelectBy.PositiveAndNegative:
					return (p) => p.Sentiment == AnalysisResultSentiment.Positive || p.Sentiment == AnalysisResultSentiment.Negative;
				case TweetStorageSelectBy.PositiveAndNegativeAndNeutral:
					return (p) => p.Sentiment == AnalysisResultSentiment.Positive || p.Sentiment == AnalysisResultSentiment.Negative || p.Sentiment == AnalysisResultSentiment.Neutral;
				default:
					return (p) => false;
			}
		}

		public int CountTweets(TweetStorageSelectBy type = TweetStorageSelectBy.SkipErrors) {
			lock (_items) {
				return _items.Count(SelectPredicate(type));
			}
		}

		public int GetPercentage(TweetStorageSelectBy countType = TweetStorageSelectBy.Positive) {

			int all = 0;

			switch(countType){
				case TweetStorageSelectBy.All:
					return 100;

				case TweetStorageSelectBy.SkipErrors:
				case TweetStorageSelectBy.Analysed:
				case TweetStorageSelectBy.ToAnalyse:
				case TweetStorageSelectBy.Error:
					all = CountTweets(TweetStorageSelectBy.All);
					break;

				case TweetStorageSelectBy.Positive:
				case TweetStorageSelectBy.Negative:
					all = CountTweets(TweetStorageSelectBy.PositiveAndNegative);
					break;

				case TweetStorageSelectBy.Neutral:
				case TweetStorageSelectBy.Involvement:
				case TweetStorageSelectBy.PositiveAndNegative:
					all = CountTweets(TweetStorageSelectBy.PositiveAndNegativeAndNeutral);
					break;
				case TweetStorageSelectBy.PositiveAndNegativeAndNeutral:
					return 100;

				default:
					return 0;
			}

			if (all <= 0) {
				return 0;
			}

			return Convert.ToInt32(Math.Round((Convert.ToDouble(CountTweets(countType)) / all) * 100.0));
		}


		public ITweet GetNextTweetToAnalyse() {
			lock (_items) {
				var query = from item in _items where item.Status == TweetStorageItemStatus.New orderby item.TimeAdded select item.Tweet;
				return query.FirstOrDefault();
			}
		}

		public void AddAnalysis(ITweet tweet, AnalysisResult analysis) {
			var item = _items.FirstOrDefault(p => p.Tweet == tweet);
			if (item != null) {
				item.Analysis = analysis;
				item.Status = TweetStorageItemStatus.Analysed;
			} else {
				item.Analysis = null;
				item.Status = TweetStorageItemStatus.Error;
			}
			Update();
		}

		public event EventHandler DataUpdated;
		public void OnDataUpdated() {
			var tmp = DataUpdated;
			if (tmp != null) {
				DataUpdated(this,null);
			}
		}


		public void Update() {
			OnDataUpdated();
		}

	}

	public enum TweetStorageSelectBy {
		All,
		SkipErrors,
		ToAnalyse,
		Analysed,
		Error,
		Positive,
		Negative,
		Neutral,
		Involvement,
		PositiveAndNegative,
		PositiveAndNegativeAndNeutral,
	}


	public class TweetStorageItem {

		public TweetStorageItem(ITweet tweet) {
			Tweet = tweet;
			Status = TweetStorageItemStatus.New;
			TimeAdded = DateTime.Now;
		}

		public ITweet Tweet;
		public AnalysisResult Analysis;
		public TweetStorageItemStatus Status;
		public DateTime TimeAdded;

		public string Country{
			get {
				if (Tweet != null && Tweet.Place != null && !string.IsNullOrWhiteSpace(Tweet.Place.Country)) {
					return Tweet.Place.Country;
				} else {
					return UnknownCountry;
				}
			}
		}

		public Language Language {
			get {
				if (Tweet != null && Tweet.Language != null) {
					return Tweet.Language;
				} else {
					return UnknownLanguage;
				}
			}
		}

		public AnalysisResultSentiment Sentiment {
			get {
				if (Analysis != null) {
					return Analysis.Sentiment;
				} else {
					return AnalysisResultSentiment.Error;
				}
			}
		}

		public static readonly string UnknownCountry = "unknown";
		public static readonly Language UnknownLanguage = Language.Undefined;
	}

	public class AnalysisResult{
		public AnalysisResult(AnalysisResultSentiment sentiment) {
			Sentiment = sentiment;
		}
		public AnalysisResultSentiment Sentiment;
	}

	public enum AnalysisResultSentiment{
		Neutral,
		Positive,
		Negative,
		Error
	}

	public enum TweetStorageItemStatus{
		New,
		Analysed,
		Error,
	}

	public class DisplayItem : INotifyPropertyChanged {

		private string _name;
		public string Name { get { return _name; } set { _name = value; NotifyPropertyChanged("Name"); } }

		private int _value;
		public int Value { get { return _value; } set { _value = value; NotifyPropertyChanged("Value"); } }

		private void NotifyPropertyChanged(string property) {
			if (PropertyChanged != null) {
				PropertyChanged.Invoke(this, new PropertyChangedEventArgs(property));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterSentimentAnalysis.classify;
using System.IO;
using System.Diagnostics;


using edu.stanford.nlp.pipeline;


namespace TwitterSentimentAnalysis.claissify {
	public class StanfordCoreNLP : IClassifier {

		string _modelsFolder = @"C:\nifdex\tfs\Education.Study.IEM13\sem1\SIM\Seminararbeit\Jul\Software\stanford-corenlp-full-2014-01-04\stanford-corenlp-3.3.1-models";
		edu.stanford.nlp.pipeline.StanfordCoreNLP _nlp;

		public StanfordCoreNLP() {
			var props = new java.util.Properties();
			props.setProperty("annotators", "tokenize, ssplit, parse, sentiment");

			var dictBackup = Environment.CurrentDirectory;
			System.IO.Directory.SetCurrentDirectory(_modelsFolder);
			_nlp = new edu.stanford.nlp.pipeline.StanfordCoreNLP(props);
			System.IO.Directory.SetCurrentDirectory(dictBackup);

		}


		private StafordCoreNlpSentimentClass FindSentiment(string text) {

			var sentimentList = new List<int>();

			if (!String.IsNullOrWhiteSpace(text)) {

				var annotation = _nlp.process(text);

				var SentencesAnnotationClass = new edu.stanford.nlp.ling.CoreAnnotations.SentencesAnnotation().getClass();
				var SentimentAnnotatedTreeClass = new edu.stanford.nlp.sentiment.SentimentCoreAnnotations.AnnotatedTree().getClass();

				java.util.ArrayList bla = (java.util.ArrayList)annotation.get(SentencesAnnotationClass);

				
				foreach (edu.stanford.nlp.util.CoreMap sentence in bla) {
					var tree = (edu.stanford.nlp.trees.Tree)sentence.get(SentimentAnnotatedTreeClass);
					int sentiment = edu.stanford.nlp.neural.rnn.RNNCoreAnnotations.getPredictedClass(tree);
					sentimentList.Add(sentiment);
				}
			}

			return SummarizeSentiment(sentimentList);
		}

		private StafordCoreNlpSentimentClass SummarizeSentiment(List<int> sentimentList) {

			if (sentimentList.Count < 1) {
				return StafordCoreNlpSentimentClass.Neutral;
			}

			double mainSentiment = 0;
			mainSentiment = sentimentList.Sum() / sentimentList.Count;

			/*

			sentimentList.RemoveAll(s => s == (int)Convert.ChangeType(StafordCoreNlpSentimentClass.Neutral, StafordCoreNlpSentimentClass.Neutral.GetTypeCode()));

			double mainSentiment = 0;
			double count = 0;
			foreach (var sentiment in sentimentList) {

				mainSentiment += sentiment;
				count++;
				
				if (sentiment < (int)Convert.ChangeType(StafordCoreNlpSentimentClass.Neutral, StafordCoreNlpSentimentClass.Neutral.GetTypeCode())) {
					mainSentiment += 1;
				}
			}

			if (count < 1) {
				return StafordCoreNlpSentimentClass.Neutral;
			}

			mainSentiment = mainSentiment / count;

			if (mainSentiment > 2.4 && mainSentiment < 2.6) {
				return StafordCoreNlpSentimentClass.Neutral;
			}

			mainSentiment = Math.Round(mainSentiment);
			if (mainSentiment <= 2) {
				mainSentiment--;
			}
			*/

			var sentimentRes = (StafordCoreNlpSentimentClass)Enum.Parse(typeof(StafordCoreNlpSentimentClass), mainSentiment.ToString());
			return sentimentRes;
		}

		public async Task<data.AnalysisResult> SentimentAnalysisAsync(Tweetinvi.Core.Interfaces.ITweet item) {
			return await Task<data.AnalysisResult>.Run(() => {
				if (string.IsNullOrWhiteSpace(item.Text)) {
					return new data.AnalysisResult(data.AnalysisResultSentiment.Error);
				}


				var sentiment = FindSentiment(item.Text);

				switch (sentiment) {
					case StafordCoreNlpSentimentClass.VeryNegative:
					case StafordCoreNlpSentimentClass.Negative:
						return new data.AnalysisResult(data.AnalysisResultSentiment.Negative);
					case StafordCoreNlpSentimentClass.Neutral:
						return new data.AnalysisResult(data.AnalysisResultSentiment.Neutral);
					case StafordCoreNlpSentimentClass.Positive:
					case StafordCoreNlpSentimentClass.VeryPositive:
						return new data.AnalysisResult(data.AnalysisResultSentiment.Positive);
					default:
						return new data.AnalysisResult(data.AnalysisResultSentiment.Error);
				}

			});
		}
	}

	public enum StafordCoreNlpSentimentClass {
		VeryNegative = 0,
		Negative = 1,
		Neutral = 2,
		Positive = 3,
		VeryPositive = 4
	}
}

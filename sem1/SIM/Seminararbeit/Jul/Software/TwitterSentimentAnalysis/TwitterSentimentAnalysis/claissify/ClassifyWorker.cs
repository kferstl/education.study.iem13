using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi.Core.Interfaces;
using TwitterSentimentAnalysis.data;

namespace TwitterSentimentAnalysis.classify {
	public class ClassifyWorker {

		private TweetStorage _storage;
		private IClassifier _classifier;
		public ClassifyWorker(IClassifier classifier, TweetStorage storage) {
			_classifier = classifier;
			_storage = storage;
		}

		public async void StartWorkAsync(CancellationToken token) {
			while (!token.IsCancellationRequested) {
				var item = _storage.GetNextTweetToAnalyse();
				if (item != null) {

					var res = await _classifier.SentimentAnalysisAsync(item);

					//var result = await Task<string>.Run(() => _datumBoxApi.TwitterSentimentAnalysis(item.Text));



					_storage.AddAnalysis(item, res);
				} else {
					await Task.Delay(100);
				}
			}
		}
	}

	public interface IClassifier {
		Task<AnalysisResult> SentimentAnalysisAsync(ITweet item);
	}
}

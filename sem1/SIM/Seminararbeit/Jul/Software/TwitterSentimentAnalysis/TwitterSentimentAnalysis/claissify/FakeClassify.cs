using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi.Core.Interfaces;
using TwitterSentimentAnalysis.classify;
using TwitterSentimentAnalysis.data;

namespace TwitterSentimentAnalysis.claissify {
	public class FakeClassify : IClassifier {

		public async Task<data.AnalysisResult> SentimentAnalysisAsync(ITweet item) {
			var rand = new Random(Thread.CurrentThread.ManagedThreadId * DateTime.Now.Millisecond);
			await Task.Delay(rand.Next(160, 400));
			AnalysisResult res = null;
			if (rand.Next(0, 100) > 10) {

				if (rand.Next(0, 100) < 12) {
					res = new AnalysisResult(AnalysisResultSentiment.Neutral);
				} else {
					if (rand.Next(0, 100) > 75) {
						res = new AnalysisResult(AnalysisResultSentiment.Negative);
					} else {
						res = new AnalysisResult(AnalysisResultSentiment.Positive);
					}
				}

			}
			return res;
		}
	}
}

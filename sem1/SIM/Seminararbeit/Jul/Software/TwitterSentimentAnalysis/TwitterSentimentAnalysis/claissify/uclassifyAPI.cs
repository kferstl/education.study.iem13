using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TwitterSentimentAnalysis.classify;
using System.Net.Http;
using Tweetinvi.Core.Interfaces;
using Newtonsoft.Json;

namespace TwitterSentimentAnalysis.claissify {
	public class UclassifyAPI : IClassifier {

		private string _apiKey;
		private string _apiUrlFormat = "http://uclassify.com/browse/uClassify/Sentiment/ClassifyText?version=1.01&output=json&readkey={0}&text={1}";

		public UclassifyAPI(string apiKey) {
			_apiKey = apiKey;
		}

		public async Task<data.AnalysisResult> SentimentAnalysisAsync(ITweet item)
		{
			var requestUrl = String.Format(_apiUrlFormat, _apiKey, HttpUtility.UrlEncode(item.Text));

			var client = new HttpClient();
			var res = await client.GetStringAsync(requestUrl);

			var ds = JsonHelper.CreateSerializer();
			var resultObject = ds.Deserialize<UclassifyData>(res);

			if (resultObject.Cls1 != null) {

			}

			return null;
		}

		private class UclassifyData {
			public string Version { get; set; }
			public bool Success { get; set; }
			public int StatusCode { get; set; }
			public string ErrorMessage{get; set;}
			public double TextCoverage { get; set; }

			public UclassifyDataCls Cls1 { get; set; }

			public class UclassifyDataCls {
				double Negative { get; set; }
				double Positive { get; set; }
			}
		}
	}
}

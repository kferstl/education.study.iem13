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
using System.Runtime.Serialization;

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

			data.AnalysisResult analysisRes = null;

			if (resultObject != null && resultObject.Cls1 != null) {
				if (resultObject.Cls1.Negative > resultObject.Cls1.Positive) {
					analysisRes = new data.AnalysisResult(data.AnalysisResultSentiment.Negative);
				} else if (resultObject.Cls1.Negative == resultObject.Cls1.Positive) {
					analysisRes = new data.AnalysisResult(data.AnalysisResultSentiment.Neutral);
				} else {
					analysisRes = new data.AnalysisResult(data.AnalysisResultSentiment.Positive);
				}
			}

			return analysisRes;
		}

		[DataContract]
		private class UclassifyData {
			[DataMember]
			public string Version { get; set; }
			[DataMember]
			public bool Success { get; set; }
			[DataMember]
			public int StatusCode { get; set; }
			[DataMember]
			public string ErrorMessage{get; set;}
			[DataMember]
			public double TextCoverage { get; set; }
			[DataMember]
			public UclassifyDataCls Cls1 { get; set; }
		}
		[DataContract]
		public class UclassifyDataCls {
			[DataMember]
			public double Negative { get; set; }
			[DataMember]
			public double Positive { get; set; }
		}
	}
}

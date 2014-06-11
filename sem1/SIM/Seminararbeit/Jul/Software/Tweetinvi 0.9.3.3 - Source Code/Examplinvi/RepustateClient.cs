using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.IO;

namespace RepustateApplication {
	class RepustateClient {
		public static string API_KEY = "446d5ee683e27859a1e62cfa34ae4a9aeae7937a";
		public static string DOMAIN = "http://api.repustate.com/v2/";
		public static string BASE_URL = DOMAIN + API_KEY;

		public static string FORMAT_JSON = ".json";
		public static string FORMAT_XML = ".xml";

		public static string URL_ARGUMENT_STRING = "?url=";

		public static string SENTIMENT_PATH = "/score";
		public static string SENTIMENT_BULK_PATH = "/bulk-score";
		public static string ADJECTIVES_PATH = "/adj";
		public static string NOUNS_PATH = "/noun";
		public static string VERBS_PATH = "/verb";
		public static string CLEAN_HTML_PATH = "/clean-html";
		public static string NGRAMS_PATH = "/ngrams";
		public static string EXTRACT_DATES_PATH = "/extract-dates";
		public static string POWERPOINT_PATH = "/powerpoint/";

		/**
		 * Generic method for calling any other future methods that Repustate may add
		 * @param path
		 * @param format
		 * @param httpRequestType
		 * @param data
		 * @return
		 * @throws MalformedURLException
		 * @throws IOException
		 * @throws RepustateException
		 */
		public static string getRepustateData(string path, string format, string httpRequestType, Dictionary<string, string> data) {
			string output = sendRequest(path + format, "", httpRequestType, getContentFromDictionary(data));
			return output;
		}

		/**
		 * This method takes a map of strings as argument. Each key value pair will represent a block of text. 
		 * Each argument starting with 'text' will be the only ones scored. To help you reconcile scores with blocks of text, 
		 * Repustate requires that you append some sort of ID to the POST argument. For example, if you had 50 blocks of text, 
		 * you could enumerate them from 1 to 50 as text1, text2, ..., text50.
		 *
		 * @param format
		 * @param data
		 * @return
		 * @throws MalformedURLException
		 * @throws IOException
		 * @throws RepustateException
		 */
		public static string getSentimentBulk(string format, Dictionary<string, string> data) {
			//curl -d "text1=first block of text&text2=second block of text"// http://api.repustate.com/v2/demokey/bulk-score.json
			string output = sendRequest(SENTIMENT_BULK_PATH + format, "", "POST", getContentFromDictionary(data));
			return output;
		}

		public static string getSentiment(string format, Dictionary<string, string> data) {
			//curl -d "url=www.twitter.com" http://api.repustate.com/v2/demokey/score.json
			string output = sendRequest(SENTIMENT_PATH + format, "", "POST", getContentFromDictionary(data));
			return output;
		}

		public static string getNouns(string format, Dictionary<string, string> data) {
			//curl -d "text=This is a big block of new text" http://api.repustate.com/v2/demokey/adj.xml
			string output = sendRequest(NOUNS_PATH + format, "", "POST", getContentFromDictionary(data));
			return output;
		}

		public static string getAdjectives(string format, Dictionary<string, string> data) {
			//curl -d "text=This is a big block of new text" http://api.repustate.com/v2/demokey/adj.xml
			string output = sendRequest(ADJECTIVES_PATH + format, "", "POST", getContentFromDictionary(data));
			return output;
		}

		public static string getVerbs(string format, Dictionary<string, string> data) {
			//curl -d "text=This man is buying iPads." http://api.repustate.com/v2/demokey/verb.json
			string output = sendRequest(VERBS_PATH + format, "", "POST", getContentFromDictionary(data));
			return output;
		}

		/**
		 * API notes from Repustate
		 * Note: all of the above API calls use this implicitly so you don't have to
		 * clean your HTML before passing it to Repustate. Only use this API call if
		 * you intend to do your own processing after the fact with the cleaned up
		 * HTML.
		 * 
		 * Note 2: This call doesn't work on home pages e.g. cnn.com, ebay.com,
		 * nytimes.com. Its intended use is for single article pages.
		 * 
		 * @param format
		 * @param data
		 * @return
		 * @throws MalformedURLException
		 * @throws IOException
		 * @throws RepustateException
		 */
		public static string getCleanHTML(string format, Dictionary<string, string> data, string url) {
			//http://api.repustate.com/v2/demokey/clean-html.json?url=http://tcrn.ch/aav9Ty
			string urlStr = CLEAN_HTML_PATH + format;
			if (url != null) {
				urlStr = urlStr + URL_ARGUMENT_STRING + url;
			}
			string output = sendRequest(urlStr, "", "GET", getContentFromDictionary(data));
			return output;
		}

		/**
		 * If the url argument is specified, we will use the HTTP GET method, or if
		 * the text is specified, then we will use the POST method according to the
		 * repustate APIs. if you are using HTTP GET, any optional arguments need to
		 * be URL-encoded. If you are using HTTP POST, any optional arguments should
		 * be part of your POST'ed data.
		 * 
		 * @param format
		 * @param data
		 * @param url
		 * @return
		 * @throws MalformedURLException
		 * @throws IOException
		 * @throws RepustateException
		 */
		public static string getNgrams(string format, Dictionary<string, string> data, string url) {
			//http://api.repustate.com/v2/demokey/ngrams.json?url=http://tcrn.ch/aav9Ty&max=4&min=2&freq=2

			string output = "";
			if (url != null && !url.Equals("")) {
				//In this case, we need to use HTTP GET and all the arguments need to be URL encoded
				string urlStr = NGRAMS_PATH + format;
				urlStr += URL_ARGUMENT_STRING + url + "&";
				urlStr += getContentFromDictionary(data);
				output = sendRequest(urlStr, "", "GET", "");
			} else {
				//In this case we need to use HTTP POST and pass the arguments in the POST data
				output = sendRequest(NGRAMS_PATH + format, "", "POST", getContentFromDictionary(data));
			}

			return output;
		}

		public static string getExtractedDates(string format, Dictionary<string, string> data) {
			//curl -d "text=I can't wait to go to school tomorrow" http://api.repustate.com/v2/demokey/extract-dates.json
			string output = sendRequest(EXTRACT_DATES_PATH + format, "", "POST", getContentFromDictionary(data));
			return output;
		}

		public static void savePowerpointSlides(Dictionary<string, string> data, string fileName) {
			//curl -d "author=Martin
			//&report_id=10
			//&report_title=My first report
			//&slide_1_title=My first slide
			//&slide_1_image=<b64 encoding>
			//&slide_1_notes=A great slide to show people" 
			//http://api.repustate.com/v2/your_api_key_goes_here/powerpoint/

			sendPowerPointRequest(POWERPOINT_PATH, "", "POST", getContentFromDictionary(data), fileName);
		}


		/********************************UTILITY METHODS****************************************/
		private static void sendPowerPointRequest(string path, string contentType, string method, string data, string fileName) {
			HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(BASE_URL + path);
			myHttpWebRequest.Method = method.ToUpper();

			//Write the data to the stream
			if (data != null && !data.Equals("")) {
				byte[] byteArray = Encoding.UTF8.GetBytes(data);
				Stream dataStream = myHttpWebRequest.GetRequestStream();
				dataStream.Write(byteArray, 0, byteArray.Length);
				dataStream.Close();
			}

			HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
			Stream streamResponse = myHttpWebResponse.GetResponseStream();

			BinaryReader binReader = new BinaryReader(streamResponse);

			System.IO.FileStream fileStream = new System.IO.FileStream(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);

			byte[] buff = new byte[2048];
			int bytesRead;
			while (0 != (bytesRead = binReader.Read(buff, 0, buff.Length))) {
				fileStream.Write(buff, 0, bytesRead);
			}
			fileStream.Close();
			binReader.Close();
			streamResponse.Close();
			myHttpWebResponse.Close();
		}

		public static string sendRequest(string path, string contentType, string method, string data) {
			string outputData = "";
			HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(BASE_URL + path);
			myHttpWebRequest.Method = method.ToUpper();

			//Write the data to the stream
			if (data != null && !data.Equals("")) {
				byte[] byteArray = Encoding.UTF8.GetBytes(data);
				Stream dataStream = myHttpWebRequest.GetRequestStream();
				dataStream.Write(byteArray, 0, byteArray.Length);
				dataStream.Close();
			}

			HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
			Stream streamResponse = myHttpWebResponse.GetResponseStream();
			StreamReader streamRead = new StreamReader(streamResponse);

			Char[] readBuff = new Char[256];
			int count = streamRead.Read(readBuff, 0, 256);
			while (count > 0) {
				outputData += new string(readBuff, 0, count);
				count = streamRead.Read(readBuff, 0, 256);
			}

			streamResponse.Close();
			streamRead.Close();
			myHttpWebResponse.Close();

			return outputData;
		}

		public static string sendRequest(String path, String contentType) {
			return sendRequest(path, contentType, "GET", "");
		}

		private static string getContentFromDictionary(Dictionary<string, string> data) {
			String content = "";
			if (data != null) {
				int i = 0;
				foreach (KeyValuePair<string, string> kvp in data) {
					if (i != 0) {
						content += "&";
					}

					content += kvp.Key + "=" + HttpUtility.UrlEncode(kvp.Value, Encoding.UTF8);
					i++;
				}
			} else {
				content = "";
			}
			return content;
		}
	}
}
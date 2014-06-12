using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Web;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace DatumboxAPI
{
  public class DatumboxAPI
    {
        private readonly string api_key;

        public DatumboxAPI(string api_key)
        {
            this.api_key = api_key;
        }

        //Web Request Functions
        private void AddArgument(Dictionary<string,string> arguments, string key, string value)
        {
            arguments.Remove(key); //will return false if not found...
            arguments.Add(key, value);
        }

        private string GetArguments(Dictionary<string,string> arguments)
        {
            StringBuilder parameters = new StringBuilder();
            foreach (KeyValuePair<string, string> kvp in arguments)
            {
                EncodeAndAddItem(parameters, kvp.Key, kvp.Value);
            }

            return parameters.ToString();
        }

        private void EncodeAndAddItem(StringBuilder baseRequest, string key, string dataItem)
        {
            if (baseRequest == null)
            {
                baseRequest = new StringBuilder();
            }
            if (baseRequest.Length != 0)
            {
                baseRequest.Append("&");
            }
            baseRequest.Append(key);
            baseRequest.Append("=");
            baseRequest.Append(HttpUtility.UrlEncode(dataItem));
        }

        private string SendPostRequest(string URL, Dictionary<string,string> arguments)
        {
            HttpWebRequest request;
            string postData = GetArguments(arguments);

            Uri uri = new Uri(URL);
            request = (HttpWebRequest)WebRequest.Create(uri);

            request.Method = "POST";

            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postData.Length;

            using (Stream writeStream = request.GetRequestStream())
            {
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] bytes = encoding.GetBytes(postData);
                writeStream.Write(bytes, 0, bytes.Length);
            }


            request.AllowAutoRedirect = true;

            UTF8Encoding enc = new UTF8Encoding();


            string result = string.Empty;


            HttpWebResponse Response;
            try
            {
                using (Response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream responseStream = Response.GetResponseStream())
                    {
                        using (StreamReader readStream = new StreamReader(responseStream, Encoding.UTF8))
                        {
                            return readStream.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine("Error: " + exc.Message);
                throw exc;
            }
        }

        public string SentimentAnalysis(string text)
        {
            Dictionary<string,string> arguments = new Dictionary<string,string>();

            AddArgument(arguments, "api_key", api_key);
            AddArgument(arguments, "text", text);
			
            return SendPostRequest("http://api.datumbox.com/1.0/SentimentAnalysis.json", arguments);
        }

        public string TwitterSentimentAnalysis(string text)
        {
            Dictionary<string,string> arguments = new Dictionary<string,string>();

            AddArgument(arguments, "api_key", api_key);
            AddArgument(arguments, "text", text);
			
            return SendPostRequest("http://api.datumbox.com/1.0/TwitterSentimentAnalysis.json", arguments);
        }

        public string SubjectivityAnalysis(string text)
        {
            Dictionary<string,string> arguments = new Dictionary<string,string>();

            AddArgument(arguments, "api_key", api_key);
            AddArgument(arguments, "text", text);
			
            return SendPostRequest("http://api.datumbox.com/1.0/SubjectivityAnalysis.json", arguments);
        }

        public string TopicClassification(string text)
        {
            Dictionary<string,string> arguments = new Dictionary<string,string>();

            AddArgument(arguments, "api_key", api_key);
            AddArgument(arguments, "text", text);
			
            return SendPostRequest("http://api.datumbox.com/1.0/TopicClassification.json", arguments);
        }

        public string SpamDetection(string text)
        {
            Dictionary<string,string> arguments = new Dictionary<string,string>();

            AddArgument(arguments, "api_key", api_key);
            AddArgument(arguments, "text", text);
			
            return SendPostRequest("http://api.datumbox.com/1.0/SpamDetection.json", arguments);
        }

        public string AdultContentDetection(string text)
        {
            Dictionary<string,string> arguments = new Dictionary<string,string>();

            AddArgument(arguments, "api_key", api_key);
            AddArgument(arguments, "text", text);
			
            return SendPostRequest("http://api.datumbox.com/1.0/AdultContentDetection.json", arguments);
        }

        public string ReadabilityAssessment(string text)
        {
            Dictionary<string,string> arguments = new Dictionary<string,string>();

            AddArgument(arguments, "api_key", api_key);
            AddArgument(arguments, "text", text);
			
            return SendPostRequest("http://api.datumbox.com/1.0/ReadabilityAssessment.json", arguments);
        }

        public string LanguageDetection(string text)
        {
            Dictionary<string,string> arguments = new Dictionary<string,string>();

            AddArgument(arguments, "api_key", api_key);
            AddArgument(arguments, "text", text);
			
            return SendPostRequest("http://api.datumbox.com/1.0/LanguageDetection.json", arguments);
        }

        public string CommercialDetection(string text)
        {
            Dictionary<string,string> arguments = new Dictionary<string,string>();

            AddArgument(arguments, "api_key", api_key);
            AddArgument(arguments, "text", text);
			
            return SendPostRequest("http://api.datumbox.com/1.0/CommercialDetection.json", arguments);
        }

        public string EducationalDetection(string text)
        {
            Dictionary<string,string> arguments = new Dictionary<string,string>();

            AddArgument(arguments, "api_key", api_key);
            AddArgument(arguments, "text", text);
			
            return SendPostRequest("http://api.datumbox.com/1.0/EducationalDetection.json", arguments);
        }

        public string GenderDetection(string text)
        {
            Dictionary<string,string> arguments = new Dictionary<string,string>();

            AddArgument(arguments, "api_key", api_key);
            AddArgument(arguments, "text", text);
			
            return SendPostRequest("http://api.datumbox.com/1.0/GenderDetection.json", arguments);
        }

        public string TextExtraction(string text)
        {
            Dictionary<string,string> arguments = new Dictionary<string,string>();

            AddArgument(arguments, "api_key", api_key);
            AddArgument(arguments, "text", text);
			
            return SendPostRequest("http://api.datumbox.com/1.0/TextExtraction.json", arguments);
        }
		
        public string KeywordExtraction(string text, int n)
        {
            Dictionary<string,string> arguments = new Dictionary<string,string>();

            AddArgument(arguments, "api_key", api_key);
            AddArgument(arguments, "text", text);
            AddArgument(arguments, "n", n.ToString());
			
            return SendPostRequest("http://api.datumbox.com/1.0/KeywordExtraction.json", arguments);
        }
		
        public string DocumentSimilarity(string original, string copy)
        {
            Dictionary<string,string> arguments = new Dictionary<string,string>();

            AddArgument(arguments, "api_key", api_key);
            AddArgument(arguments, "original", original);
            AddArgument(arguments, "copy", copy);
			
            return SendPostRequest("http://api.datumbox.com/1.0/DocumentSimilarity.json", arguments);
        }
    }
}
/*
 * Example of API Client for Datumbox Machine Learning API.
 * 
 * @version 1.0
 * @author Vasilis Vryniotis
 * @link   http://www.datumbox.com/
 * @copyright Copyright (c) 2013, Datumbox.com
 */


/*
DatumboxAPI DatumboxAPI = new DatumboxAPI("YOUR_API_KEY");
            
string text = "Google Search (or Google Web Search) is a web search engine owned by Google Inc. Google Search is the most-used search engine on the World Wide Web,[4] receiving several hundred million queries each day through its various services.[5] The order of search results on Google's search-results pages is based, in part, on a priority rank called a 'PageRank'. Google Search provides many options for customized search, using Boolean operators such as: exclusion ('-xx'), alternatives ('xx OR yy'), and wildcards ('x * x').[6] The main purpose of Google Search is to hunt for text in publicly accessible documents offered by web servers, as opposed to other data, such as with Google Image Search. Google Search was originally developed by Larry Page and Sergey Brin in 1997.[7] Google Search provides at least 22 special features beyond the original word-search capability.[8] These include synonyms, weather forecasts, time zones, stock quotes, maps, earthquake data, movie showtimes, airports, home listings, and sports scores. There are special features for dates, including ranges,[9] prices, temperatures, money/unit conversions, calculations, package tracking, patents, area codes,[8] and language translation of displayed pages. In June 2011, Google introduced 'Google Voice Search' and 'Search by Image' features for allowing the users to search words by speaking and by giving images.[10] In May 2012, Google introduced a new Knowledge Graph semantic search feature to customers in the U.S";    

Console.WriteLine(DatumboxAPI.SentimentAnalysis(text));
Console.WriteLine(DatumboxAPI.SubjectivityAnalysis(text));
Console.WriteLine(DatumboxAPI.TopicClassification(text));
Console.WriteLine(DatumboxAPI.SpamDetection(text));
Console.WriteLine(DatumboxAPI.AdultContentDetection(text));
Console.WriteLine(DatumboxAPI.ReadabilityAssessment(text));
Console.WriteLine(DatumboxAPI.LanguageDetection(text));
Console.WriteLine(DatumboxAPI.CommercialDetection(text));
Console.WriteLine(DatumboxAPI.EducationalDetection(text));
Console.WriteLine(DatumboxAPI.GenderDetection(text));

string tweet = "I love the new Datumbox API! :)";
Console.WriteLine(DatumboxAPI.TwitterSentimentAnalysis(tweet));

string html = "<html>\n<body>\nI love\n<h1>\nMachine Learning\n</h1>\n</body>\n</html>";    

Console.WriteLine(DatumboxAPI.TextExtraction(html));
Console.WriteLine(DatumboxAPI.KeywordExtraction(text,3));

string original = "This book has been written against a background of both reckless optimism and reckless despair. It holds that Progress and Doom are two sides of the same medal; that both are articles of superstition, not of faith. It was written out of the conviction that it should be possible to discover the hidden mechanics by which all traditional elements of our political and spiritual world were dissolved into a conglomeration where everything seems to have lost specific value, and has become unrecognizable for human comprehension, unusable for human purpose. Hannah Arendt, The Origins of Totalitarianism (New York: Harcourt Brace Jovanovich, Inc., 1973 ed.), p.vii, Preface to the First Edition.";

string copy="Hannah Arendt's book, The Origins of Totalitarianism, was written in the light of both excessive hope and excessive pessimism. Her thesis is that both Advancement and Ruin are merely different sides of the same coin. Her book was produced out of a belief that one can understand the method in which the more conventional aspects of politics and philosophy were mixed together so that they lose their distinctiveness and become worthless for human uses.";

Console.WriteLine(DatumboxAPI.DocumentSimilarity(original,copy));
*/
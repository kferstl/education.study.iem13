using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterSentimentAnalysis.classify;
using System.IO;
using System.Diagnostics;


namespace TwitterSentimentAnalysis.claissify {
	public class StanfordCoreNLP : IClassifier {

		string _coreNlpPath = @"C:\nifdex\tfs\Education.Study.IEM13\sem1\SIM\Seminararbeit\Jul\Software\stanford-corenlp-full-2014-01-04";
		string _coreNlpCommand = "start.bat";
		// java -cp "*" -mx5g edu.stanford.nlp.sentiment.SentimentPipeline -stdin

		Process _javaProcess;

		public StanfordCoreNLP() {

			_javaProcess = new Process();
			_javaProcess.StartInfo.RedirectStandardOutput = true;
			_javaProcess.StartInfo.RedirectStandardInput = true;
			_javaProcess.StartInfo.RedirectStandardError = true;
			_javaProcess.StartInfo.UseShellExecute = false;
			_javaProcess.StartInfo.WorkingDirectory = _coreNlpPath;
			_javaProcess.StartInfo.FileName = _coreNlpPath+"\\"+_coreNlpCommand;
			_javaProcess.Start();



			while (true) {
				Console.WriteLine(_javaProcess.StandardOutput.ReadLine());
			}
			
		}

		public Task<data.AnalysisResult> SentimentAnalysisAsync(Tweetinvi.Core.Interfaces.ITweet item) {
			return null;
		}

	}
}

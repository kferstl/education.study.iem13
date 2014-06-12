using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Events.EventArguments;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Models.Parameters;
using Tweetinvi.Core.Interfaces.oAuth;
using Tweetinvi.Core.Interfaces.Streaminvi;
using Tweetinvi.Json;
using TwitterSentimentAnalysis.data;
using Stream = Tweetinvi.Stream;


namespace TwitterSentimentAnalysis.twitter {
	public class TwitterWorker {

		IFilteredStream _stream;
		TweetStorage _storage;
		Task _workerTask;

		public TwitterWorker(TweetStorage storage) {
			_storage = storage;

			_stream = Stream.CreateFilteredStream();
			_stream.MatchingTweetAndLocationReceived += _stream_MatchingTweetAndLocationReceived;
		}

		public void SetCredentials(string userAccessToken, string userAccessSecret, string consumerKey, string consumerSecret) {
			TwitterCredentials.ApplicationCredentials =
				TwitterCredentials.CreateCredentials(userAccessToken, userAccessSecret, consumerKey, consumerSecret);
		}

		void _stream_MatchingTweetAndLocationReceived(object sender, MatchedTweetAndLocationReceivedEventArgs e) {
			if (e.Tweet != null) {
				_storage.AddTweet(e.Tweet);
			}
		} 

		public void AddTrack(string track) {
			_stream.AddTrack(track);
		}

		public void CelarTracks() {
			_stream.ClearTracks();
		}

		public bool StreamRunning {
			get { return _stream.StreamState == StreamState.Resume; }
		}

		public async void StartStreamAsync() {
			if (!StreamRunning) {
				await _stream.StartStreamMatchingAnyConditionAsync();
			}
		}

		public void StopWork() {
			_stream.StopStream();
		}

	}
}

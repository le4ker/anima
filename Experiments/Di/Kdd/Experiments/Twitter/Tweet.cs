namespace Di.Kdd.Experiments.Twitter
{
	using System;

	public class Tweet
	{
		private const int 	ID_INDEX = 0;
		private const int 	USERNAME_INDEX = 1;
		private const int 	DATETIME_INDEX = 2;
		private const int 	LONG_INDEX = 4;
		private const int 	LAT_INDEX = 5;
		private const int 	TWEET_INDEX = 6;

		private int time;
		private Location location;
		string tweet;

		private int tweetIndex = 0;

		public Tweet(string fromString)
		{
			string[] tokens = fromString.Split ('|');

			string noisedTime = tokens[Tweet.DATETIME_INDEX].Split(':')[0];
			string hour = new String (noisedTime.ToCharArray(), noisedTime.Length - 2, 2);
			this.time = Int32.Parse (hour);

			this.location = new Location (new Coordinate (tokens [Tweet.LONG_INDEX]), new Coordinate (tokens [Tweet.LAT_INDEX]));

			tokens = tokens [Tweet.TWEET_INDEX].Split (' ');

			this.tweet = "";

			foreach (var token in tokens) 
			{
				if (token.StartsWith ("http:") == false && token.StartsWith ("https:") == false) {
					this.tweet += token + " ";
				} 
			}
		}

		public Tweet (string tweet, int time, Location location)
		{
			this.tweet = tweet;
			this.time = time;
			this.location = location;
		}

		public bool HasNext()
		{
			return this.tweetIndex < this.tweet.Length - 1;
		}

		public char PeekNext()
		{
			if (this.HasNext ()) 
			{
				return this.tweet [this.tweetIndex];
			} 
			else {
				return '\0';
			}
		}		

		public char ConsumeNext()
		{
			if (this.HasNext ()) 
			{
				return this.tweet [this.tweetIndex++];
			} 
			else 
			{
				return '\0';
			}
		}

		public void Reset()
		{
			this.tweetIndex = 0;
		}

		public int Size() 
		{
			return this.tweet.Length;
		}

		public int GetTime()
		{
			return this.time;
		}
	}
}

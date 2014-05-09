namespace Di.Kdd.Experiments.Twitter
{
	using System;

	public class Tweet
	{
		private const int 	ID_INDEX = 0;
		private const int 	USERNAME_INDEX = 1;
		private const int 	DATETIME_INDEX = 2;
		private const int 	LONG_INDEX = 3;
		private const int 	LAT_INDEX = 4;
		private const int 	TWEET_INDEX = 5;

		private int time;
		private Location location;
		string tweet;

		public Tweet(string fromString)
		{
			string[] tokens = fromString.Split ('|');

			string noisedTime = tokens[Tweet.DATETIME_INDEX].Split(':')[0];
			string hour = new String (noisedTime.ToCharArray(), noisedTime.Length - 2, 2);
			this.time = Int32.Parse (hour);

			this.location = new Location (new Coordinate (tokens [Tweet.LONG_INDEX]), new Coordinate (tokens [Tweet.LAT_INDEX]));

			this.tweet = tokens [Tweet.TWEET_INDEX];
		}

		public Tweet (string tweet, int time, Location location)
		{
			this.tweet = tweet;
			this.time = time;
			this.location = location;
		}
	}
}


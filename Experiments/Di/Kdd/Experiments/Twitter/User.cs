namespace Di.Kdd.Experiments.Twitter
{
	using System;
	using System.Collections.Generic;
	using System.IO;

	public class User
	{
		private const string DataFolder = "../../Data/";

		private string id;
		private List<Tweet> tweets = new List<Tweet>();

		private int tweetIndex = 0;

		public User (string dataFile)
		{
			if (File.Exists(DataFolder + dataFile) == false)
			{
				throw new IOException(DataFolder + dataFile);
			}

			this.id = dataFile;

			var line = "";

			using (var reader = new StreamReader(File.OpenRead(DataFolder + dataFile)))
			{
				while ((line = reader.ReadLine()) != null)
				{
					this.tweets.Add(new Tweet(line));
				}
			}
		}

		public string GetId()
		{
			return this.id;
		}

		public bool HasNext() 
		{
			if (this.tweetIndex == this.tweets.Count - 1) 
			{
				return this.tweets [this.tweets.Count - 1].HasNext ();
			}
			else 
			{
				return this.tweetIndex < this.tweets.Count - 1;
			}
		}

		public void Reset()
		{
			this.tweetIndex = 0;

			foreach (var tweet in this.tweets) 
			{
				tweet.Reset ();
			}
		}

		public char PeekNext()
		{
			if (this.HasNext())
			{
				return this.tweets[this.tweetIndex].PeekNext ();
			}
			else
			{
				return '\0';
			}
		}

		public char ConsumeNext()
		{
			if (this.HasNext())
			{
				if (this.tweets[this.tweetIndex].HasNext () == false)
				{
					this.tweetIndex++;
				}

				return this.tweets[this.tweetIndex].ConsumeNext ();
			}
			else
			{
				return '\0';
			}
		}
	}
}


namespace Di.Kdd.Experiments.Twitter
{
	using System;
	using System.Collections.Generic;
	using System.IO;

	public class User
	{
		private string id;
		private List<Tweet> tweets = new List<Tweet>();

		private int tweetIndex = 0;

		public User() { }

		public User (string dataFile)
		{
			if (File.Exists(dataFile) == false)
			{
				throw new IOException(dataFile);
			}

			this.id = dataFile;

			var line = "";

			using (var reader = new StreamReader(File.OpenRead(dataFile)))
			{
				while ((line = reader.ReadLine()) != null)
				{
					this.tweets.Add(new Tweet(line));
				}
			}
		}

		private int testSetIndex;

		public User GetTrainSet(float percentage)
		{
			User trainSet = new User ();

			int i;

			for (i = 0; i < this.tweets.Count * percentage; i++) 
			{
				trainSet.AddTweet (this.tweets[i]);
			}

			this.testSetIndex = i;

			return trainSet;
		}

		public User GetTestSet()
		{
			User testSet = new User ();

			for (int i = this.testSetIndex; i < this.tweets.Count; i++) 
			{
				testSet.AddTweet (this.tweets[i]);
			}

			return testSet;
		}

		private void AddTweet(Tweet tweet)
		{
			this.tweets.Add (tweet);
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
			if (this.HasNext ()) 
			{

				if (this.tweets [this.tweetIndex].HasNext ()) {
					return this.tweets [this.tweetIndex].PeekNext ();
				} 
				else 
				{
					return this.tweets [this.tweetIndex + 1].PeekNext ();
				}
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

		public int PeekNextTime()
		{
			if (this.HasNext ()) 
			{
				if (this.tweets [this.tweetIndex].HasNext ()) 
				{
					return this.tweets [this.tweetIndex].GetTime ();
				} 
				else 
				{
					return this.tweets [this.tweetIndex + 1].GetTime ();
				}
			} 
			else
			{
				return '\0';
			}
		}

		public int GetTime()
		{
			return this.tweets [this.tweetIndex].GetTime ();
		}
	}
}

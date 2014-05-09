namespace Di.Kdd.Experiments.Twitter
{
	using System;
	using System.Collections.Generic;
	using System.IO;

	public class User
	{
		private const string DataFolder = "../../Data/";

		private string username;
		private List<Tweet> tweets = new List<Tweet>();

		public User (string dataFile)
		{
			if (File.Exists(DataFolder + dataFile) == false)
			{
				throw new IOException(DataFolder + dataFile);
			}

			var line = "";

			using (var reader = new StreamReader(File.OpenRead(DataFolder + dataFile)))
			{
				while ((line = reader.ReadLine()) != null)
				{
					this.tweets.Add(new Tweet(line));
				}
			}
		}
	}
}


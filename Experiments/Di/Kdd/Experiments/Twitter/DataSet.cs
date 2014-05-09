namespace Di.Kdd.Experiments.Twitter
{
	using System;
	using System.Collections.Generic;

	public class DataSet
	{
		private const string DataFolder = "../../Data/";

		private string[] userFiles = { "1833919200.txt", "2168747747.txt", "2234721587.txt", "271027125.txt", "63251769.txt",
			"214204613.txt", "2179897715.txt", "265441754.txt", "324386374.txt", "941839460.txt"
		};

		private List<User> users = new List<User>();

		public DataSet ()
		{
			foreach (var userFile in this.userFiles) {
				this.users.Add(new User(userFile));
			}
		}
	}
}


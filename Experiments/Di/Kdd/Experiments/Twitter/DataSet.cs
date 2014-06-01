using System.IO;

namespace Di.Kdd.Experiments.Twitter
{
	using System;
	using System.Collections.Generic;
	using System.Resources;

	public class DataSet
	{
		private static DataSet dataset;

		private const string DataFolder = "../../Data/";

		public List<User> Users = new List<User>();

		public static DataSet GetInstance(bool sort = false) 
		{
			if (DataSet.dataset == null) {
				DataSet.dataset = new DataSet (sort);
			}

			DataSet.dataset.Reset ();

			return DataSet.dataset;
		}

		private DataSet (bool sort = false)
		{
			string[] userFiles = Directory.GetFiles (DataSet.DataFolder, "*.txt", SearchOption.TopDirectoryOnly);

			foreach (var userFile in userFiles) {
				if (userFile.EndsWith ("words.txt")) {
					continue;
				}

				this.Users.Add (new User (userFile));
			}

			if (sort) 
			{
				foreach (User user in this.Users) 
				{
					user.Sort ();
					user.Purge ();
					user.Save ();
				}
			} 
		}
			
		public void Reset()
		{
			foreach (var user in this.Users) 
			{
				user.Reset ();
			}
		}
	}
}


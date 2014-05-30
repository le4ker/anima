using System.IO;

namespace Di.Kdd.Experiments
{
	using System;

	using Di.Kdd.Experiments.Twitter;

	using Di.Kdd.WriteRightSimulator;

	class MainClass
	{
		private static int K = 1500;
		private static int timePartitions = 7;

		public static void Main (string[] args)
		{
			memper ();
			dic ();
			per ();
			time ();
		}

		private static void pervstime()
		{
			var dataset = new DataSet ();

			foreach (var user in dataset.Users) 
			{
				var perPrecission = UserPersonalizationExp.run (user);
				user.Reset ();
				var timePrecission = UserTimePerExp.run (user);

				Console.WriteLine (user.GetId () + " per: " + perPrecission + " time: " + timePrecission);
			}
		}

		private static void memper()
		{
			var results = new int[(K / 50) + 1];
			var resultsWRiter = new StreamWriter (File.Create ("memper-precission.csv"));

			for (int k = 0, i = 0; k <= K; k += 50, i++) 
			{
				Personalization.run (k);
				results [i] = Personalization.memory;
				resultsWRiter.WriteLine (results[i]);
				Console.WriteLine (results[i]);
				resultsWRiter.Flush ();
			}

			resultsWRiter.Close ();
		}

		private static void dic()
		{
			var results = new float[(K / 50) + 1];
			var resultsWRiter = new StreamWriter (File.Create ("dic-precission.csv"));

			for (int k = 0, i = 0; k <= K; k += 50, i++) 
			{
				results[i] = DictionaryExperiment.run (k);
				resultsWRiter.WriteLine (results[i]);
				Console.WriteLine (results[i]);
				resultsWRiter.Flush ();
			}

			resultsWRiter.Close ();
		}

		private static void per()
		{
			var results = new float[(K / 50) + 1];
			var resultsWRiter = new StreamWriter (File.Create ("per-precission.csv"));

			for (int k = 0, i = 0; k <= K; k += 50, i++) 
			{
				results[i] = Personalization.run (k);
				resultsWRiter.WriteLine (results[i]);
				Console.WriteLine (results[i]);
				resultsWRiter.Flush ();
			}

			resultsWRiter.Close ();
		}

		private static void time()
		{
			for (int t = 2; t < MainClass.timePartitions; t++) 
			{
				var results = new float[(K / 50) + 1];
				var resultsWRiter = new StreamWriter (File.Create ("time" + t + "-precission.csv"));

				for (int k = 0, i = 0; k <= K; k += 50, i++) 
				{
					results[i] = TimeAwareExperiment.run (k, t);
					resultsWRiter.WriteLine (results[i]);
					Console.WriteLine (results[i]);
					resultsWRiter.Flush ();
				}

				resultsWRiter.Close ();
			}
		}
	}
}

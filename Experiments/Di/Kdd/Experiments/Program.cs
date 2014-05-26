using System.IO;

namespace Di.Kdd.Experiments
{
	using System;

	using Di.Kdd.Experiments.Twitter;

	using Di.Kdd.WriteRightSimulator;

	class MainClass
	{
		public static void Main (string[] args)
		{
			int K = 1500;
			int timePartitions = 6;
			var results = new float[(K / 50) + 1];

			var resultsWRiter = new StreamWriter (File.Create ("ftime6-precission.csv"));

			for (int k = 0, i = 0; k <= K; k += 50, i++) 
			{
				var p = new FaultyTimeExperiment (k);
				results[i] = p.run (k, timePartitions);
				resultsWRiter.WriteLine (results[i]);
				Console.WriteLine (results[i]);
				resultsWRiter.Flush ();
			}

			resultsWRiter.Close ();
		}
	}
}

namespace Di.Kdd.Experiments
{
	using System;

	using Di.Kdd.Experiments.Twitter;

	using Di.Kdd.WriteRightSimulator;

	class MainClass
	{
		public static void Main (string[] args)
		{
			float trainSetPercentage = 0.9f;

			for (int k = 1; k < 6; k++) {
				new DictionaryExperiment (k, trainSetPercentage);
				new DictionaryWithPersonalization (k, trainSetPercentage);
				new TimeAwareExperiment (k, trainSetPercentage);
				new FaultyTimeExperiment (k, trainSetPercentage);
			}
		}
	}
}

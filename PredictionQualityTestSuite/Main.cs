namespace Di.Kdd.PredictionQualityTestSuite
{
	using Di.Kdd.TextPrediction;
	using Di.Kdd.Utilities;

	using System;

	class Sell
	{
		public static void Main (string[] args)
		{
			Logger.LogToConsole();

			var experiment = new ExperimentScenario();

			Scenarios.Execute();
		}
	}
}

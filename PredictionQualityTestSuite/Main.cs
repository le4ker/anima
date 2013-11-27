namespace Di.Kdd.PredictionQualityTestSuite
{
	using Di.Kdd.TextPrediction;
	using Di.Kdd.Utilities;

	using System;

	class MainClass
	{
		public static void Main (string[] args)
		{
			Logger.LogToConsole();

			var basic = new BasicScenario();

			Scenarios.Execute();
		}
	}
}

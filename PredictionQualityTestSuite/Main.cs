namespace Di.Kdd.PredictionQualityTestSuite
{
	using System;

	class MainClass
	{
		public static void Main (string[] args)
		{
			var basic = new BasicScenario();

			Scenarios.Execute();
		}
	}
}

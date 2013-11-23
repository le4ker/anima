namespace Di.Kdd.PredictionQualityTestSuite
{
	using Di.Kdd.TextPrediction;

	using System;
	using System.Collections.Generic;

	public class Scenarios
	{
		protected static Dictionary<string, Scenario> scenarios = new Dictionary<string, Scenario>();

		public static void Execute ()
		{
			foreach (var scenario in Scenarios.scenarios)
			{
				Logger.Log("Executing: " + scenario.Key);

				scenario.Value.Setup();

				if (scenario.Value.Run())
				{
					Logger.Log("Success", ConsoleColor.Green);
				}
				else
				{
					Logger.Log("Fail. Result: " + scenario.Value.GetResult() + " Success Rate: " + scenario.Value.GetSuccessRate(), ConsoleColor.Red);
				}

				scenario.Value.Teardown();
			}
		}
	}
}


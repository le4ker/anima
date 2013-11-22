namespace Di.Kdd.PredictionQualityTestSuite
{
	using System;
	using System.Collections.Generic;

	public class Scenarios
	{
		public static Dictionary<string, Scenario> scenarios = new Dictionary<string, Scenario>();

		public static void Execute ()
		{
			foreach (var scenario in Scenarios.scenarios)
			{
				// todo: use logger

				Console.WriteLine("Executing: " + scenario.Key);

				scenario.Value.Setup();
				scenario.Value.Run();
				scenario.Value.Teardown();
			}
		}
	}
}


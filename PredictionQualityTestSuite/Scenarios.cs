namespace Di.Kdd.PredictionQualityTestSuite
{
	using System;
	using System.Collections.Generic;

	public class Scenarios
	{
		protected static Dictionary<string, Scenario> scenarios = new Dictionary<string, Scenario>();

		public static void Execute ()
		{
			foreach (var scenario in Scenarios.scenarios)
			{
				// todo: use logger

				Console.WriteLine("Executing: " + scenario.Key);

				scenario.Value.Setup();

				var foregroundColor = Console.ForegroundColor;

				if (scenario.Value.Run())
				{
					Console.ForegroundColor = ConsoleColor.Green;

					Console.WriteLine("Success");
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.Red;

					Console.WriteLine("Fail. Result: " + scenario.Value.GetResult() + " Success Rate: " + scenario.Value.GetSuccessRate());
				}

				Console.ForegroundColor = foregroundColor;

				scenario.Value.Teardown();
			}
		}
	}
}


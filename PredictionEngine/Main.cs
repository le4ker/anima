namespace Di.Kdd.TextPrediction
{
	using System;
	using System.Collections.Generic;

	class Shell
	{
		private const char ShellExit = '±';
		private const string EngineDb = "database.txt";

		public static void Main (string[] args)
		{
			var engine = new PredictionEngine<Statistics>();
			engine.LoadDB(EngineDb);

			var buffer = "";
			var letter = '\0';

			var predictions = new Dictionary<char, float>();

			do
			{
				var key = Console.ReadKey();
				letter = key.KeyChar;

				if (letter == ShellExit)
				{
					break;
				}

				if (engine.ValidCharacter(letter) == false)
				{
					continue;
				}

				engine.CharTyped(letter);

				buffer += letter;

				Console.Clear();
				Console.WriteLine(buffer);

				predictions = engine.GetPredictions();

				if (predictions.Count == 0)
				{
					Console.WriteLine("Unknown word\n");
				}

				foreach (var prediction in predictions)
				{
					if (prediction.Value != 0)
					{
						Console.WriteLine("P(" + prediction.Key + ") = " + prediction.Value);
					}
				}

			} while (true);

			engine.SaveDB(EngineDb);
		}
	}
}

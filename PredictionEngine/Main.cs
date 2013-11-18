namespace Di.Kdd.TextPrediction
{
	using System;
	using System.Collections.Generic;

	class Shell
	{
		private const char ShellExit = '±';
		private const string PredictionEngineInstance = "prediction_engine_instance.txt";

		public static void Main (string[] args)
		{
			var engine = new PredictionEngine();
			engine.LoadDB(PredictionEngineInstance);

			var input = "";
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

				input += letter;

				Console.Clear();
				Console.WriteLine(input);

				predictions = engine.GetSortedPredictions();

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

			engine.SaveDB(PredictionEngineInstance);
		}
	}
}

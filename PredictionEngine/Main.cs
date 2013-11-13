using System;
using System.Collections.Generic;

namespace Di.Kdd.PredictionEngine
{
	class Shell
	{
		private const char ShellExit = '±';
		private const string PredictionEngineInstance = "prediction_engine_instance.txt";

		public static void Main (string[] args)
		{
			var engine = new PredictionEngine();
			engine.Load(PredictionEngineInstance);

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

				if (PredictionEngine.ValidLetter(letter) == false)
				{
					continue;
				}

				engine.LetterTyped(letter);

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

			engine.Save(PredictionEngineInstance);
		}
	}
}

using System;
using System.Collections.Generic;

namespace Di.Kdd.WriteRightSimulator
{
	class Shell
	{
		private const char ShellExit = '±';
		private const string PredictionwriteRightInstance = "write_right_instance.txt";

		public static void Main (string[] args)
		{
			var writeRight = new WriteRightPredictionEngine();
			writeRight.Load(PredictionwriteRightInstance);

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

				if (writeRight.ValidLetter(letter) == false)
				{
					continue;
				}

				writeRight.LetterTyped(letter);

				input += letter;

				Console.Clear();
				Console.WriteLine(input);

				predictions = writeRight.GetSortedPredictions();

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

			writeRight.Save(PredictionwriteRightInstance);
		}
	}
}

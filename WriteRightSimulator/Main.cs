namespace Di.Kdd.WriteRightSimulator
{
	using Di.Kdd.TextPrediction;

	using System;
	using System.Collections.Generic;

	class Shell
	{
		private const char ShellExit = '±';
		private const string WriteRightDb = "writerightdb.txt";

		public static void Main (string[] args)
		{
			var writeRight = new WriteRight();
			writeRight.LoadDB(WriteRightDb);

			var buffer = "";
			var character = '\0';
			var foregroundColor = Console.ForegroundColor;

			var topKPredictions = new Dictionary<char, float>();

			do
			{
				var key = Console.ReadKey();
				character = key.KeyChar;

				if (character == ShellExit)
				{
					break;
				}

				if (writeRight.IsValidCharacter(character) == false)
				{
					continue;
				}

				writeRight.CharacterTyped(character);

				buffer += character;

				Console.Clear();
				Console.WriteLine(buffer);

				topKPredictions = writeRight.GetTopKPredictions();

				if (writeRight.IsUnknownWord())
				{
					Console.WriteLine("Unknown word!");

					continue;
				}

				Console.ForegroundColor = ConsoleColor.Green;

				foreach (var prediction in topKPredictions)
				{
					Console.WriteLine("P(" + prediction.Key + ") = " + prediction.Value);
				}

				Console.WriteLine("\n");
				Console.ForegroundColor = ConsoleColor.Red;

				var predictions = writeRight.GetPredictions();

				foreach (var prediction in predictions)
				{
					if (topKPredictions.ContainsKey(prediction.Key) == false)
					{
						Console.WriteLine("P(" + prediction.Key + ") = " + prediction.Value);
					}
				}

				Console.ForegroundColor = foregroundColor;

			} while (true);

			writeRight.SaveDB(WriteRightDb);
		}
	}
}

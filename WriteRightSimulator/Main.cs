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
			var consoleColor = Console.ForegroundColor;

			var topKPredictions = new Dictionary<char, float>();

			do
			{
				var key = Console.ReadKey();
				character = key.KeyChar;

				if (character == ShellExit)
				{
					break;
				}

				if (writeRight.ValidCharacter(character) == false)
				{
					continue;
				}

				writeRight.CharacterTyped(character);

				buffer += character;

				Console.Clear();
				Console.WriteLine(buffer);

				topKPredictions = writeRight.GetTopKPredictions ();

				if (topKPredictions.Count == 0)
				{
					Console.WriteLine("Unknown word!");

					continue;
				}

				Console.ForegroundColor = ConsoleColor.Green;

				foreach (var prediction in topKPredictions)
				{
					Console.WriteLine("P(" + prediction.Key.ToString() + ") = " + prediction.Value.ToString());
				}

				Console.WriteLine("\n");
				Console.ForegroundColor = ConsoleColor.Red;

				var predictions = writeRight.GetPredictions();

				foreach (var latinLetter in Trie.LatinLetters)
				{
					if (topKPredictions.ContainsKey(latinLetter) == false)
					{
						Console.WriteLine("P(" + latinLetter.ToString() + ") = " + predictions[latinLetter].ToString());
					}
				}

				Console.ForegroundColor = consoleColor;

			} while (true);

			writeRight.SaveDB(WriteRightDb);
		}
	}
}

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
			var character = '\0';

			var predictions = new Dictionary<char, float>();

			do
			{
				var key = Console.ReadKey();
				character = key.KeyChar;

				if (character == ShellExit)
				{
					break;
				}

				if (engine.ValidCharacter(character) == false)
				{
					continue;
				}

				engine.CharacterTyped(character);

				buffer += character;

				Console.Clear();
				Console.WriteLine(buffer);

				predictions = engine.GetPredictions();

				if (engine.IsUnknownWord())
				{
					Console.WriteLine("Unknown word!");

					continue;
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

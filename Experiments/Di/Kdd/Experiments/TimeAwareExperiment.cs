namespace Di.Kdd.Experiments
{
	using Di.Kdd.Experiments.Twitter;
	using Di.Kdd.WriteRightSimulator;

	using System;

	public class TimeAwareExperiment
	{
		public TimeAwareExperiment ()
		{
			var dataSet = new DataSet ();

			Console.WriteLine ("Time Aware WriteRight");

			foreach (User user in dataSet.Users) 
			{
				var writeRight = new TimeAwareWriteRight();

				while (user.HasNext ()) 
				{
					var ch = user.ConsumeNext ();

					writeRight.SetTime (user.GetTime ());
					writeRight.CharacterTyped (ch);
				}

				user.Reset ();

				var totalWords = 0;
				var guessedWords = 0;
				var previousWasSeparator = false;

				var totalChars = 0;
				var guessedChars = 0;

				while (user.HasNext ()) 
				{
					var ch = user.ConsumeNext ();

					writeRight.SetTime (user.GetTime ());

					if (writeRight.IsWordSeparator (ch) && previousWasSeparator) 
					{
						continue;
					}

					if (writeRight.IsWordSeparator (ch)) 
					{
						totalWords++;
						previousWasSeparator = true;

						if (writeRight.IsIdle () == false) 
						{
							guessedWords++;
						}
					} 
					else 
					{
						previousWasSeparator = false;
					}

					writeRight.CharacterTyped (ch);

					if (writeRight.IsIdle ()) 
					{
						continue;
					}

					var predictions = writeRight.GetTopKPredictions ();
					var next = user.PeekNext ();

					if (writeRight.IsWordSeparator (next) == false && predictions.ContainsKey (next) == false) 
					{
						writeRight.BadPrediction ();
					} 
					else 
					{
						guessedChars++;
					}

					totalChars++;
				}

				Console.WriteLine (user.GetId() + " [" + guessedChars + " out of " + totalChars + "] " + (float) guessedChars / (float) totalChars);
			}

			dataSet.Reset ();
		}
	}
}


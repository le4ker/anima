namespace Di.Kdd.Experiments
{
	using Di.Kdd.Experiments.Twitter;
	using Di.Kdd.WriteRightSimulator;

	using System;

	public class TimeAwareExperiment
	{
		public TimeAwareExperiment ()
		{
			this.run ();
		}

		void run ()
		{
			var dataSet = new DataSet ();

			Console.WriteLine ("Time Aware WriteRight");

			foreach (User user in dataSet.Users) 
			{
				var timeAwareWriteRight = new TimeAwareWriteRight();

				/* Train the engine */

				while (user.HasNext ()) 
				{
					var ch = user.ConsumeNext ();
					timeAwareWriteRight.SetTime (user.GetTime ());
					timeAwareWriteRight.CharacterTyped (ch);
				}

				user.Reset ();

				var totalChars = 0;
				var guessedChars = 0;

				while (user.HasNext ()) 
				{
					var ch = user.ConsumeNext ();
					timeAwareWriteRight.SetTime (user.GetTime ());
					timeAwareWriteRight.CharacterTyped (ch);

					var next = user.PeekNext ();
					timeAwareWriteRight.SetTime (user.PeekNextTime ());
					var predictions = timeAwareWriteRight.GetTopKPredictions ();

					if (timeAwareWriteRight.IsValidCharacter (next) == false || 
						timeAwareWriteRight.IsWordSeparator (next) || 
						timeAwareWriteRight.IsIdle ()) 
					{
						continue;
					}

					if (predictions.ContainsKey (next) == false)
					{
						timeAwareWriteRight.BadPrediction ();
						totalChars++;
					} 
					else if (timeAwareWriteRight.IsWordSeparator (next) == false) 
					{
						guessedChars++;
						totalChars++;
					}
				}

				Console.WriteLine (user.GetId() + " [" + guessedChars + " out of " + totalChars + "] " + 
																(float) guessedChars / (float) totalChars);
			}

			dataSet.Reset ();
		}
	}
}

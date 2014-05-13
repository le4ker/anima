namespace Di.Kdd.Experiments
{
	using Di.Kdd.Experiments.Twitter;
	using Di.Kdd.WriteRightSimulator;

	using System;

	public class TimeAwareExperiment
	{
		public TimeAwareExperiment (float trainSetPercentage)
		{
			for (int i = 2; i < 6; i++) 
			{
				this.run (i, trainSetPercentage);
			}
		}

		void run (int timePartitions, float trainSetPercentage)
		{
			var dataSet = new DataSet ();

			Console.WriteLine ("Time Aware WriteRight (" + timePartitions + " time partitions");

			foreach (User user in dataSet.Users) 
			{
				var timeAwareWriteRight = new TimeAwareWriteRight(timePartitions);
				timeAwareWriteRight.LoadDB ("dummy");

				var evaluation = new ExperimentEvaluation ();

				/* Train the engine */

				User trainSet = user.GetTrainSet (trainSetPercentage);

				while (trainSet.HasNext ()) 
				{
					var ch = trainSet.ConsumeNext ();
					timeAwareWriteRight.SetTime (trainSet.GetTime ());
					timeAwareWriteRight.CharacterTyped (ch);
				}

				User testSet = user.GetTestSet ();

				var totalChars = 0;
				var guessedChars = 0;

				while (testSet.HasNext ()) 
				{
					var ch = testSet.ConsumeNext ();
					timeAwareWriteRight.SetTime (testSet.GetTime ());
					timeAwareWriteRight.CharacterTyped (ch);

					var next = testSet.PeekNext ();
					timeAwareWriteRight.SetTime (testSet.PeekNextTime ());
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

						evaluation.Miss ();
					} 
					else if (timeAwareWriteRight.IsWordSeparator (next) == false) 
					{
						guessedChars++;
						totalChars++;

						evaluation.Hit (predictions.Count);
					}
				}

				Console.WriteLine (user.GetId() + " [" + guessedChars + " out of " + totalChars + "] " + 
																(float) guessedChars / (float) totalChars);
				Console.WriteLine ("Evaluation score: " + evaluation.GetScore ());
			}

			dataSet.Reset ();
		}
	}
}

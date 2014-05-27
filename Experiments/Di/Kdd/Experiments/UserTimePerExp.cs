using System;
using Di.Kdd.Experiments.Twitter;
using Di.Kdd.WriteRightSimulator;

namespace Di.Kdd.Experiments
{
	public class UserTimePerExp
	{
		private static int trainSetSize = 1500;
		private static int timePartitions = 3;

		public static float run (User user)
		{
			var writeRight = new TimeAwareWriteRight (timePartitions);
			var evaluation = new ExperimentEvaluation ();

			/* Train the engine */

			User trainSet = user.GetTopKTweetsTrainSet (trainSetSize);

			while (trainSet.HasNext ()) 
			{
				var ch = trainSet.ConsumeNext ();
				writeRight.SetTime (trainSet.GetTime ());
				writeRight.CharacterTyped (ch);
			}

			writeRight.CharacterTyped (' ');

			User testSet = user.GetTestSet ();

			var totalChars = 0;
			var guessedChars = 0;

			writeRight.InTestMode ();

			while (testSet.HasNext ()) 
			{
				var ch = testSet.ConsumeNext ();
				writeRight.SetTime (testSet.GetTime ());
				writeRight.CharacterTyped (ch);

				var next = testSet.PeekNext ();
				writeRight.SetTime (testSet.GetTime ());
				var predictions = writeRight.GetTopKPredictions ();

				if (writeRight.IsValidCharacter (next) == false || 
					writeRight.IsWordSeparator (next)) 
				{
					continue;
				}

				if (predictions == null || predictions.ContainsKey (next) == false)
				{
					writeRight.BadPrediction ();
					totalChars++;

					evaluation.Miss ();
				} 
				else if (writeRight.IsWordSeparator (next) == false) 
				{
					guessedChars++;
					totalChars++;

					evaluation.Hit (predictions.Count);
				}
			}

			return evaluation.GetPrecission ();
		}
	}
}


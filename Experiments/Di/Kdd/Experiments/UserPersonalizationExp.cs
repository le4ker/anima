using System;
using Di.Kdd.Experiments.Twitter;
using Di.Kdd.WriteRightSimulator;
using Di.Kdd.Experiments;

namespace Di.Kdd.Experiments
{
	public class UserPersonalizationExp
	{
		private static int trainSetSize = 1500;

		public static float run(User user)
		{
			var writeRight = new WriteRight();
			var evaluation = new ExperimentEvaluation ();

			/* Train the engine */

			User trainSet = user.GetTopKTweetsTrainSet (trainSetSize);

			while (trainSet.HasNext ()) 
			{
				var ch = trainSet.ConsumeNext ();
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
				writeRight.CharacterTyped (ch);

				var next = testSet.PeekNext ();
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


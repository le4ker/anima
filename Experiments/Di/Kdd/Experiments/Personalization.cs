using System.IO;
using System.Globalization;

namespace Di.Kdd.Experiments
{
	using Di.Kdd.Experiments.Twitter;
	using Di.Kdd.WriteRightSimulator;

	using System;

	public class Personalization
	{
		public static int memory;

		public static float run(int k)
		{
			var dataSet = new DataSet ();

			float hitRatio = 0.0f;
			float precission = 0.0f;
			memory = 0;

			foreach (User user in dataSet.Users) 
			{
				var writeRight = new WriteRight();
				var evaluation = new ExperimentEvaluation ();

				/* Train the engine */

				User trainSet = user.GetTopKTweetsTrainSet (k);

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

				hitRatio += (float)guessedChars / (float)totalChars;
				precission += evaluation.GetPrecission ();
				memory += writeRight.GetKnowledgeSize ();
			}
			/*
			this.hitRatioWriter.WriteLine (hitRatio / dataSet.Users.Count);
			this.evalWriter.WriteLine (evaluationScore / dataSet.Users.Count);

			this.hitRatioWriter.Flush ();
			this.evalWriter.Flush ();
*/

			memory = memory / dataSet.Users.Count;

			return (float) precission / dataSet.Users.Count;
		}
	}
}


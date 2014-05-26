using System.IO;
using System.Globalization;

namespace Di.Kdd.Experiments
{
	using Di.Kdd.Experiments.Twitter;
	using Di.Kdd.WriteRightSimulator;

	using System;

	public class FaultyTimeExperiment
	{
		int k;
		Random random = new Random();

		public FaultyTimeExperiment (int k)
		{

			this.k = k;
		}

		public float run(int k, int timePartitions)
		{
			var dataSet = new DataSet ();

			float hitRatio = 0.0f;
			float precission = 0.0f;

			foreach (User user in dataSet.Users) 
			{
				var writeRight = new TimeAwareWriteRight (timePartitions);
				var evaluation = new ExperimentEvaluation ();

				/* Train the engine */

				User trainSet = user.GetTopKTweetsTrainSet (k);

				while (trainSet.HasNext ()) 
				{
					var ch = trainSet.ConsumeNext ();
					writeRight.SetTime (this.getRandomTime ());
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

				hitRatio += (float)guessedChars / (float)totalChars;
				precission += evaluation.GetPrecission ();
			}
			/*
			this.hitRatioWriter.WriteLine (hitRatio / dataSet.Users.Count);
			this.evalWriter.WriteLine (evaluationScore / dataSet.Users.Count);

			this.hitRatioWriter.Flush ();
			this.evalWriter.Flush ();
*/
			return (float) precission / dataSet.Users.Count;
		}

		private int getRandomTime()
		{
			return this.random.Next () % 25;
		}
	}
}


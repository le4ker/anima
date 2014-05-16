using System.Runtime.InteropServices;
using System.IO;

namespace Di.Kdd.Experiments
{
	using Di.Kdd.Experiments.Twitter;
	using Di.Kdd.WriteRightSimulator;

	using System;

	public class FaultyTimeExperiment
	{
		Random random = new Random();

		private  string hitRatioFile = "falsetime_hitratio.txt";
		private  string evalFile = "falsetime_eval.txt";

		private  TextWriter hitRatioWriter;
		private  TextWriter evalWriter;

		public FaultyTimeExperiment (int k, float trainSetPercentage)
		{
			for (int i = 2; i <= 6; i++) 
			{
				this.hitRatioWriter = new StreamWriter(File.Create ("k" + k + "t" + i + hitRatioFile));
				this.evalWriter = new StreamWriter(File.Create ("k" + k + "t" + i + evalFile));

				this.run (k, i, trainSetPercentage);

				this.hitRatioWriter.Close ();
				this.evalWriter.Close ();
			}
		}

		void run (int k, int timePartitions, float trainSetPercentage)
		{
			var dataSet = new DataSet ();

			Console.WriteLine ("(Faulty) Time Aware WriteRight (" + timePartitions + " time partitions");

			float hitRatio = 0.0f;
			float evaluationScore = 0.0f;

			foreach (User user in dataSet.Users) 
			{
				var timeAwareWriteRight = new TimeAwareWriteRight(k, timePartitions);
				timeAwareWriteRight.LoadDB ("dummy");

				var evaluation = new ExperimentEvaluation ();

				/* Train the engine */

				User trainSet = user.GetTrainSet (trainSetPercentage);

				while (trainSet.HasNext ()) 
				{
					var ch = trainSet.ConsumeNext ();	
					timeAwareWriteRight.SetTime (this.random.Next () % 24);
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
					timeAwareWriteRight.SetTime (this.random.Next () % 24);
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

				hitRatio += (float)guessedChars / (float)totalChars;
				evaluationScore += evaluation.GetScore ();

				Console.WriteLine(user.GetId ());
			}

			this.hitRatioWriter.WriteLine (hitRatio / dataSet.Users.Count);
			this.evalWriter.WriteLine (evaluationScore / dataSet.Users.Count);

			this.hitRatioWriter.Flush ();
			this.evalWriter.Flush ();

			dataSet.Reset ();
		}
	}
}


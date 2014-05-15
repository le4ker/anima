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

		public FaultyTimeExperiment (float trainSetPercentage)
		{
			for (int i = 2; i < 6; i++) 
			{
				this.hitRatioWriter = new StreamWriter(File.Create (i + hitRatioFile));
				this.evalWriter = new StreamWriter(File.Create (i + evalFile));

				this.run (i, trainSetPercentage);

				this.hitRatioWriter.Close ();
				this.evalWriter.Close ();
			}
		}

		void run (int timePartitions, float trainSetPercentage)
		{
			var dataSet = new DataSet ();

			Console.WriteLine ("(Faulty) Time Aware WriteRight (" + timePartitions + " time partitions");

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
					timeAwareWriteRight.SetTime (this.random.Next () % timePartitions);
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
					timeAwareWriteRight.SetTime (this.random.Next () % timePartitions);
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

				this.hitRatioWriter.WriteLine ((float) guessedChars / (float) totalChars + " " );
				this.evalWriter.WriteLine (evaluation.GetScore () + " ");

				this.hitRatioWriter.Flush ();
				this.evalWriter.Flush ();

				Console.WriteLine(user.GetId ());
			}

			dataSet.Reset ();
		}
	}
}


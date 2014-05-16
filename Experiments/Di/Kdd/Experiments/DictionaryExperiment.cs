using System.IO;
using System.Configuration;

namespace Di.Kdd.Experiments
{
	using Di.Kdd.Experiments.Twitter;
	using Di.Kdd.WriteRightSimulator;

	using System;

	public class DictionaryExperiment
	{
		private  string hitRatioFile = "dic_hitratio.txt";
		private  string evalFile = "dic_eval.txt";

		private  TextWriter hitRatioWriter;
		private  TextWriter evalWriter;

		public DictionaryExperiment (int k, float trainSetPercentage)
		{
			this.hitRatioWriter = new StreamWriter(File.Create ("k" + k + hitRatioFile));
			this.evalWriter = new StreamWriter(File.Create ("k" + k + evalFile));

			this.run (k, trainSetPercentage);

			this.hitRatioWriter.Close ();
			this.evalWriter.Close ();
		}
			
		public void run(int k, float trainSetPercentage)
		{
			var dataSet = new DataSet ();

			Console.WriteLine ("WriteRight with Dictionary");

			float hitRatio = 0.0f;
			float evaluationScore = 0.0f;

			foreach (User user in dataSet.Users) 
			{
				var writeRight = new WriteRight(k);
				writeRight.DontPersonalize ();
				writeRight.LoadDB ("dummy");

				var evaluation = new ExperimentEvaluation ();

				/* Train the engine */

				User trainSet = user.GetTrainSet (trainSetPercentage);

				while (trainSet.HasNext ()) 
				{
					var ch = trainSet.ConsumeNext ();
					writeRight.CharacterTyped (ch);
				}

				User testSet = user.GetTestSet ();

				var totalChars = 0;
				var guessedChars = 0;

				while (testSet.HasNext ()) 
				{
					var ch = testSet.ConsumeNext ();
					writeRight.CharacterTyped (ch);

					var next = testSet.PeekNext ();
					var predictions = writeRight.GetTopKPredictions ();

					if (writeRight.IsValidCharacter (next) == false || 
						writeRight.IsWordSeparator (next) || 
						writeRight.IsIdle ()) 
					{
						continue;
					}

					if (predictions.ContainsKey (next) == false)
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


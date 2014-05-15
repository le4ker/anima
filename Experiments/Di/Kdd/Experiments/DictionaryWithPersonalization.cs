using System.IO;

namespace Di.Kdd.Experiments
{
	using Di.Kdd.Experiments.Twitter;
	using Di.Kdd.WriteRightSimulator;

	using System;

	public class DictionaryWithPersonalization
	{
		private  string hitRatioFile = "dicper_hitratio.txt";
		private  string evalFile = "dicper_eval.txt";

		private  TextWriter hitRatioWriter;
		private  TextWriter evalWriter;

		public DictionaryWithPersonalization (float trainSetPercentage)
		{
			this.hitRatioWriter = new StreamWriter(File.Create (hitRatioFile));
			this.evalWriter = new StreamWriter(File.Create (evalFile));

			this.run (trainSetPercentage);

			this.hitRatioWriter.Close ();
			this.evalWriter.Close ();
		}

		public void run(float trainSetPercentage)
		{
			var dataSet = new DataSet ();

			Console.WriteLine ("WriteRight with Dictionary and Personalization");

			foreach (User user in dataSet.Users) 
			{
				var writeRight = new WriteRight();
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


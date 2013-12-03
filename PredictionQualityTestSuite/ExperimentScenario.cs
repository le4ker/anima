namespace Di.Kdd.PredictionQualityTestSuite
{
	using Di.Kdd.Utilities;
	using Di.Kdd.WriteRightSimulator;

	using System;

	public class ExperimentScenario : Scenario
	{
		private DataStream stream;
		private WriteRight writeRight;

		public ExperimentScenario () : base("Experiment", 0.8F)
		{
		}

		public override void Setup ()
		{
			this.stream = new DataStream("zdnet-content.txt");
		}

		public override bool Run ()
		{
			for (var wordsSize = 1000; wordsSize <= 64000; wordsSize *= 2 )
			{
				for (var k = 1; k <= 10; k++)
				{
					this.writeRight = new WriteRight();

					this.writeRight.SetWordsSize(wordsSize);
					this.writeRight.SetK(k);

					this.run();

					Logger.Log("Words size: " + wordsSize + " k " + k + " result: " + this.result);
				}
			}

			return true;
		}

		public void run()
		{
			var totalWords = 0;
			var guessedWords = 0;
			var previousWasSeparator = false;

			while (this.stream.HasNext())
			{
				var ch = this.stream.ConsumeNext();

				if (this.writeRight.IsWordSeparator(ch) && previousWasSeparator)
				{
					continue;
				}

				if (this.writeRight.IsWordSeparator(ch))
				{
					totalWords++;
					previousWasSeparator = true;

					if (this.writeRight.IsIdle() == false)
					{
						guessedWords++;
					}
				}
				else
				{
					previousWasSeparator = false;
				}

				this.writeRight.CharacterTyped(ch);

				if (this.writeRight.IsIdle())
				{
					continue;
				}

				var predictions = this.writeRight.GetTopKPredictions();
				var next = this.stream.PeekNext();

				if (this.writeRight.IsWordSeparator(next) == false && predictions.ContainsKey(next) == false)
				{
					this.writeRight.BadPrediction();
				}
			}

			this.result = (float) guessedWords / (float) totalWords;
		}

		public override void Teardown ()
		{
		}
	}
}


namespace Di.Kdd.PredictionQualityTestSuite
{
	using Di.Kdd.WriteRightSimulator;

	using System;

	public class BasicScenario : Scenario
	{
		private DataStream stream;
		private WriteRight writeRight = new WriteRight();

		private const string WriteRightDb = "writerightdb.txt";

		public BasicScenario () :  base("Basic", 0.8F)
		{
			this.stream = new DataStream("zdnet-content.txt");
			this.writeRight.LoadDB(WriteRightDb);
		}

		public override void Setup ()
		{
		}

		public override bool Run ()
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

			return this.result >= this.successRate;
		}

		public override void Teardown ()
		{
		}
	}
}


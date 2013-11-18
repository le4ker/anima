namespace Di.Kdd.TextPrediction
{
	using System;

	public class Statistics
	{
		protected int usageCounter = 1;

		public Statistics() { }

		public Statistics(string usageCounter)
		{
			this.usageCounter = Int32.Parse(usageCounter);
		}

		public virtual void WordTyped()
		{
			this.usageCounter++;
		}

		public int GetPopularity()
		{
			return this.usageCounter;
		}

		public override string ToString ()
		{
			return this.usageCounter.ToString();
		}
	}
}


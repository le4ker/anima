namespace Di.Kdd.TextPrediction
{
	using System;

	public class Statistics
	{
		protected int usageCounter = 1;

		public Statistics() { }

		public virtual void InitFromString (string text)
		{
			this.usageCounter = Int32.Parse(text);
		}
		public virtual void WordTyped()
		{
			this.usageCounter++;
		}

		public int GetPopularity ()
		{
			return this.usageCounter;
		}

		public override string ToString ()
		{
			return this.usageCounter.ToString();
		}
	}
}


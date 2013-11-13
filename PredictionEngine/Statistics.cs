using System;

namespace Di.Kdd.PredictionEngine
{
	public class Statistics
	{
		private int usageCounter = 0;

		public Statistics()
		{

		}

		public Statistics(string usageCounter)
		{
			this.usageCounter = Int32.Parse(usageCounter);
		}

		public void WordTyped()
		{
			usageCounter++;
		}

		public int GetPopularity()
		{
			return usageCounter;
		}

		public override string ToString ()
		{
			return usageCounter.ToString();
		}
	}
}


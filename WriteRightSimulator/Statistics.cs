namespace Di.Kdd.WriteRightSimulator
{
	using Di.Kdd.TextPrediction;

	using System;

	public class Statistics : Di.Kdd.TextPrediction.Statistics
	{
		private long timestamp;

		public Statistics ()
		{
			this.timestamp = DateTime.Now.Ticks;
		}

		public Statistics (int usage)
		{
			this.usageCounter = usage;
			this.timestamp = DateTime.Now.Ticks;
		}

		public Statistics (int usage, long timestamp)
		{
			this.usageCounter = usage;
			this.timestamp = timestamp;
		}

		public long GetTimestamp ()
		{
			return this.timestamp;
		}

		public override void WordTyped ()
		{
			this.usageCounter++;
			this.timestamp = DateTime.Now.Ticks;
		}

		public override void InitFromString (string text)
		{
			var columns = text.Split(' ');

			this.usageCounter = int.Parse(columns[1]);
			this.timestamp = long.Parse(columns[2]);
		}

		public override string ToString ()
		{
			return this.usageCounter.ToString() + " " + this.timestamp.ToString();
		}
	}
}


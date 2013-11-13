
using Di.Kdd.TextPrediction;

using System;

namespace Di.Kdd.WriteRightSimulator
{
	public class WriteRightStatistics : Statistics
	{
		private long timestamp;

		public WriteRightStatistics ()
		{
			this.usageCounter = 1;
			this.timestamp = DateTime.Now.Ticks;
		}

		public WriteRightStatistics (int usage)
		{
			this.usageCounter = usage;
			this.timestamp = DateTime.Now.Ticks;
		}

		public WriteRightStatistics (int usage, long timestamp)
		{
			this.usageCounter = usage;
			this.timestamp = timestamp;
		}

		public override void WordTyped ()
		{
			this.usageCounter++;
			this.timestamp = DateTime.Now.Ticks;
		}

		public long GetTimestamp()
		{
			return this.timestamp;
		}
	}
}


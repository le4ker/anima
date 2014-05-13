namespace Di.Kdd.WriteRightSimulator
{
	using Di.Kdd.WriteRightSimulator;

	using System;

	public class TimeAwareWriteRight : WriteRight
	{
		public TimeAwareWriteRight (int timePartitions)
		{
			this.engine = new TimeAwarePredictionEngine (timePartitions);
		}

		public void SetTime(int hour)
		{
			((TimeAwarePredictionEngine)engine).SetTime (hour);
		}
	}
}

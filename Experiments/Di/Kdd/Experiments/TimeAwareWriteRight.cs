namespace Di.Kdd.WriteRightSimulator
{
	using Di.Kdd.WriteRightSimulator;

	using System;

	public class TimeAwareWriteRight : WriteRight
	{
		public TimeAwareWriteRight ()
		{
			this.engine = new TimeAwarePredictionEngine ();
		}

		public void SetTime(int hour)
		{
			((TimeAwarePredictionEngine)engine).SetTime (hour);
		}
	}
}

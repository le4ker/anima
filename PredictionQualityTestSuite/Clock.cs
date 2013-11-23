namespace Di.Kdd.PredictionQualityTestSuite
{
	using System;

	/* todo: Decide the order of Time's magnitude. 
	* I think it has to be in milliseconds, due to 
	* smartphone's Ticks (i.e. Android time is in milliseconds)
	*/

	public class Clock
	{
		public static long Minute = 60;
		public static long Hour = 60 * Clock.Minute;
		public static long Day = 24 * Clock.Hour;
		public static long Week = 7 * Clock.Day;

		private static long clock = 0;

		public static void Start ()
		{
			Clock.clock = DateTime.Now.Ticks;
		}

		public static void TimeFlies (long howMuch)
		{
			Clock.clock += howMuch;
		}

		public static long Now ()
		{				
			return Clock.clock;
		}
	}
}


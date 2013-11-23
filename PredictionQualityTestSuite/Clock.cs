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

		private static void LazyInitialization ()
		{
			if (clock == 0)
			{
				Clock.clock = DateTime.Now.Ticks;
			}
		}

		public static void TimeFlies (long howMuch)
		{
			Clock.LazyInitialization();

			Clock.clock += howMuch;
		}

		public static long Now ()
		{				
			Clock.LazyInitialization();

			return Clock.clock;
		}
	}
}


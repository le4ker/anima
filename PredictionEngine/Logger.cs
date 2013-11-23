namespace Di.Kdd.TextPrediction
{
	using System;
	using System.IO;

	public class Logger
	{
		private static bool logToFile = false;
		private static StreamWriter writer;
		private static object writterLock = new object();

		private static Logger loggerInstance;

		private Logger ()
		{
		}

		public static void LogToFile(string path)
		{
			lock (Logger.writterLock)
			{
				Logger.writer = new StreamWriter(path, true);
			}

			Logger.logToFile = true;
		}

		public static void LogToConsole()
		{
			lock (Logger.writterLock)
			{
				if (Logger.writer != null)
				{
					Logger.writer.Close();
					Logger.writer = null;
				}
			}

			Logger.logToFile = false;
		}

		public static void Log(string what)
		{
			if (Logger.loggerInstance == null)
			{
				Logger.loggerInstance = new Logger();
			}

			var log = DateTime.Now + " " + what;

			if (Logger.logToFile)
			{
				lock (Logger.writterLock)
				{
					Logger.writer.WriteLine(log);
				}
			}
			else
			{
				Console.WriteLine(log);
			}
		}

		public static void Log (string what, ConsoleColor color)
		{
			var original = Console.ForegroundColor;

			Console.ForegroundColor = color;

			Logger.Log(what);

			Console.ForegroundColor = original;
		}

		~Logger()
		{
			lock (Logger.writterLock)
			{

				Logger.writer.Close();
			}
		}
	}
}


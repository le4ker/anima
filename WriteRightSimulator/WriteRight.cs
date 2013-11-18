namespace Di.Kdd.WriteRightSimulator
{
	using System;
	using System.IO;

	public class WriteRight
	{
		private int k = 26;
		private int continuousSuccesses = 0;
		private const int AggresiveThreshold = 5;

		private TrimmablePredictionEngine engine = new TrimmablePredictionEngine();

		public void LoadDB(string dbPath)
		{
			engine.LoadDB(dbPath);

			if (File.Exists(dbPath) == false)
			{
				return;
			}

			var reader = File.OpenText(dbPath);

			var line = "";

			while ((line = reader.ReadLine()) != engine.GetEndOfDb())
			{
				continue;
			}

			line = reader.ReadLine();

			var columns = line.Split(' ');

			this.k = int.Parse(columns[0]);
			this.continuousSuccesses = int.Parse(columns[1]);
		}

		public void SaveDB(string dbPath)
		{
			engine.TrimAndSaveDb(dbPath);

			var writer = new StreamWriter(File.OpenWrite(dbPath));

			writer.WriteLine(this.ToString());
		}

		public override string ToString ()
		{
			return this.k.ToString() + " " + this.continuousSuccesses.ToString();
		}
	}
}


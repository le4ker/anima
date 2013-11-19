namespace Di.Kdd.WriteRightSimulator
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	public class WriteRight
	{
		private int k = 26;
		private int continuousSuccesses = 0;
		private int aggresiveThreshold = 5;

		private TrimmablePredictionEngine engine = new TrimmablePredictionEngine();

		#region Mutators

		public void SetK (int k)
		{
			this.k = k;
		}

		public void SetAggresiveThreshold (int aggresiveThreshold)
		{
			this.aggresiveThreshold = aggresiveThreshold;
		}

		public void SetTrimThreshold (int trimThreshold)
		{
			this.engine.SetThreshold(trimThreshold);
		}

		public void SetTrimPercentage (float trimPercentage)
		{
			this.engine.SetTrimPercentage(trimPercentage);
		}

		#endregion

		public bool ValidCharacter (char character)
		{
			return this.engine.ValidCharacter(character);
		}

		public void CharacterTyped (char character)
		{
			this.engine.CharacterTyped (character);
		}

		public Dictionary<char, float> GetPredictions ()
		{
			return engine.GetPredictions();
		}

		public Dictionary<char, float> GetTopKPredictions ()
		{
			return this.engine.GetPredictions().OrderByDescending(x => x.Value).Take(this.k).ToDictionary(x => x.Key, x => x.Value);
		}

		public void LoadDB (string dbPath)
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

		public void SaveDB (string dbPath)
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


namespace Di.Kdd.WriteRightSimulator
{
	using Di.Kdd.TextPrediction;

	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	public class WriteRight
	{
		private int k = 26;
		private int continuousSuccesses = 0;
		private int aggresiveThreshold = 5;
		private bool isIdle = false;

		private TrimmablePredictionEngine engine = new TrimmablePredictionEngine();

		public WriteRight ()
		{
			Trie.SetWordSeparators(" .,;:!?\n()[]*&@{}/_+=|%#'0123456789");
		}

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

		#region Public Methods

		public bool IsUnknownWord()
		{
			return this.engine.IsUnknownWord();
		}

		public bool IsValidCharacter (char character)
		{
			return this.engine.ValidCharacter(character);
		}

		public void CharacterTyped (char character)
		{
			this.engine.CharacterTyped (character);

			if (Trie.IsWordSeparator(character))
			{
				this.SuccessfullPrediction();
			}
		}

		public Dictionary<char, float> GetPredictions ()
		{
			return engine.GetPredictions();
		}

		public Dictionary<char, float> GetTopKPredictions ()
		{
			return this.engine.GetPredictions().Where(x => x.Value > 0.0F).OrderByDescending(x => x.Value).Take(this.k).ToDictionary(x => x.Key, x => x.Value);
		}

		public void BadPrediction ()
		{
			this.k++;
			this.continuousSuccesses = 0;
		}

		public void LoadDB (string dbPath)
		{
			engine.LoadDB(dbPath);

			if (File.Exists(dbPath) == false)
			{
				return;
			}

			using (var reader = File.OpenText(dbPath))
			{
				var line = "";

				while ((line = reader.ReadLine()) != engine.GetDbEndTrail())
				{
					continue;
				}

				line = reader.ReadLine();
				var columns = line.Split(' ');

				this.k = int.Parse(columns[0]);
				this.continuousSuccesses = int.Parse(columns[1]);
			}
		}

		public void SaveDB (string dbPath)
		{
			engine.TrimAndSaveDb(dbPath);

			using (var writer = new StreamWriter(dbPath, true))
			{
				writer.WriteLine(this.ToString());
			}
		}

		public override string ToString ()
		{
			return this.k.ToString() + " " + this.continuousSuccesses.ToString();
		}

		#endregion

		#region Private Methods

		private void SuccessfullPrediction ()
		{
			this.continuousSuccesses++;

			if (this.continuousSuccesses == this.aggresiveThreshold)
			{
				this.continuousSuccesses = 0;

				if (k > 1)
				{
					k--;
				}
			}
		}

		#endregion
	}
}


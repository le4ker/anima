namespace Di.Kdd.WriteRightSimulator
{
	using Di.Kdd.TextPrediction;
	using Di.Kdd.Utilities;

	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	public class WriteRight
	{
		private int k = 26;
		private int continuousSuccesses = 0;
		private bool isIdle = false;

		private int aggresiveThreshold = 1;

		protected TrimmablePredictionEngine engine = new TrimmablePredictionEngine();

		public WriteRight ()
		{
			Trie.SetWordSeparators(" .,;:!?\n()[]*&@{}<>/-_+=|%#©~$'`\"0123456789");
		}

		#region Mutators

		public void SetK (int k)
		{
			this.k = k;
		}

		public void SetWordsSize(int wordsSize)
		{
			this.engine.SetWordsSize(wordsSize);
		}

		public void SetAggresiveThreshold (int aggresiveThreshold)
		{
			this.aggresiveThreshold = aggresiveThreshold;
		}

		public void SetTrimThreshold (int trimThreshold)
		{
			this.engine.SetTrimThreshold(trimThreshold);
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

		public bool IsWordSeparator (char character)
		{
			return this.engine.IsWordSeparator(character);
		}

		public bool IsIdle ()
		{
			return this.isIdle;
		}

		public void CharacterTyped (char character)
		{
			this.engine.CharacterTyped(character);

			if (Trie.IsWordSeparator(character) && !this.isIdle)
			{
				this.SuccessfullPrediction();
			}
			else if (this.isIdle && Trie.IsWordSeparator(character))
			{
				this.isIdle = false;
			}
		}

		public Dictionary<char, float> GetPredictions ()
		{
			return this.isIdle ? null : this.engine.GetPredictions();
		}

		public Dictionary<char, float> GetTopKPredictions ()
		{
			return this.isIdle ? null : this.engine.GetPredictions().Where(x => x.Value > 0.0F).OrderByDescending(x => x.Value).Take(this.k).ToDictionary(x => x.Key, x => x.Value);
		}

		public void BadPrediction ()
		{
			if (this.k < 26)
			{
				this.k++;
			}

			this.continuousSuccesses = 0;
			this.isIdle = true;
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

			Logger.Log("Loaded writeright from DB");
		}

		public void SaveDB (string dbPath)
		{
			engine.TrimAndSaveDb(dbPath);

			using (var writer = new StreamWriter(dbPath, true))
			{
				writer.WriteLine(this.ToString());
			}

			Logger.Log("Saved writeright to DB");
		}

		public void PurgeDB (string dbPath)
		{
			if (File.Exists(dbPath))
			{
				File.Delete(dbPath);
			}
		}

		public override string ToString ()
		{
			return this.k.ToString() + " " + this.continuousSuccesses.ToString();
		}

		public void DontPersonalize()
		{
			this.engine.DontPersonalize ();
		}

		#endregion

		#region Private Methods

		private void SuccessfullPrediction ()
		{
			this.continuousSuccesses++;
			this.isIdle = false;

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


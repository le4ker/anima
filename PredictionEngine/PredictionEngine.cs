namespace Di.Kdd.TextPrediction
{
	using Di.Kdd.Utilities;

	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	public class PredictionEngine<StatisticsT> where StatisticsT : Statistics, new()
	{
		private int wordsSize = 10000;

		private Trie trie = new Trie();
		protected Dictionary<string, StatisticsT> knowledge = new Dictionary<string, StatisticsT>();

		private string currentWord = "";
		private int wordsTyped = 0;
		private Trie currentSubTrie;
		private bool isUnknownWord = false;

		private const string DataFolder = "../../Data/";
		private const string WordsFileName = "words.txt";
		private const string DbEndTrail = "±±±±±±±±±±±±±±";
		private float PersonalizationFactor = 1.0F;

		public PredictionEngine ()
		{
			this.currentSubTrie = this.trie;
		}

		public Boolean IsUnknownWord ()
		{
			return this.isUnknownWord;
		}

		public string GetDbEndTrail ()
		{
			return DbEndTrail;
		}

		public static void SetWordSeparators (string wordSeparators)
		{
			Trie.SetWordSeparators(wordSeparators);
		}

		public void SetWordsSize(int wordsSize)
		{
			this.wordsSize = wordsSize;
		}

		public bool IsWordSeparator (char character)
		{
			return Trie.IsWordSeparator(character);
		}

		#region Public Methods

		public Dictionary<char, float> GetPredictions ()
		{
			var popularity = 0;
			var postfixesCounter = 0;
			var evaluation = 0.0F;
			var evaluationSum = 0.0F;

			if (this.currentSubTrie == null)
			{
				this.isUnknownWord = true;
				return new Dictionary<char, float>();
			}

			foreach (var possibleNextLetter in Trie.LatinLetters)
			{
				popularity = this.currentSubTrie.GetPopularity(possibleNextLetter);
				postfixesCounter = this.currentSubTrie.GetSubtrieSize(possibleNextLetter);

				evaluation = this.Evaluate(popularity, postfixesCounter);

				evaluationSum += evaluation;
			}

			Dictionary<char, float> predictions = new Dictionary<char, float>();

			if (evaluationSum == 0)
			{
				foreach (var letter in Trie.LatinLetters)
				{
					predictions.Add(letter, 0.0F);
				}
			}
			else
			{
				foreach (var possibleNextLetter in Trie.LatinLetters)
				{
					popularity = this.currentSubTrie.GetPopularity(possibleNextLetter);
					postfixesCounter = this.currentSubTrie.GetSubtrieSize(possibleNextLetter);

					evaluation = this.Evaluate(popularity, postfixesCounter);

					predictions.Add(possibleNextLetter, evaluation / evaluationSum);
				}
			}

			return predictions.OrderByDescending(kv => kv.Value).ToDictionary(k => k.Key, v => v.Value);
		}

		public void CharacterTyped (char character)
		{
			if (Trie.IsWordSeparator(character))
			{
				this.WordTyped();

				return;
			}

			this.currentWord += character;

			if (this.currentSubTrie != null)
			{
				this.currentSubTrie = this.currentSubTrie.GetSubTrie(character);
			}
			else
			{
				this.isUnknownWord = true;
			}
		}

		public void PredictionCancelled ()
		{
			this.ResetState();
		}

		public void LoadDB (string dbPath)
		{
			if (File.Exists(dbPath) == false)
			{
				this.Init();
				this.GetTrained();

				return;
			}

			var reader = File.OpenText(dbPath);

			var line = "";

			while ((line = reader.ReadLine()) != DbEndTrail)
			{
				var columns = line.Split(' ');
				var statisticsString = line.Remove(0, columns[0].Length);

				var statistics = new StatisticsT();
				statistics.InitFromString(statisticsString);

				knowledge.Add(columns[0], statistics);
			}

			reader.Close();

			this.GetTrained();

			Logger.Log("Loaded prediction engine from DB");
		}

		public void SaveDB (string dbPath)
		{
			using (var writer = new StreamWriter(dbPath, false))
			{
				foreach (var data in this.knowledge)
				{
					writer.WriteLine("{0} {1}", data.Key, data.Value);
				}

				writer.WriteLine(DbEndTrail);
			}

			Logger.Log("Saved prediction engine to DB");
		}

		public bool ValidCharacter (char character)
		{
			return (Trie.IsLatinLetter(character) || Trie.IsWordSeparator(character));
		}

		public void DontPersonalize()
		{
			this.PersonalizationFactor = 0.0f;
		}

		#endregion

		#region Private Methods

		private void Init ()
		{
			var reader = File.OpenText(DataFolder + WordsFileName);

			var words = 0;
			var word = "";

			while (words < wordsSize && (word = reader.ReadLine()) != null)
			{
				if (this.knowledge.ContainsKey(word) || Trie.IsValidWord(word) == false)
				{
					continue;
				}

				this.knowledge.Add(word, new StatisticsT());
				this.trie.Add(word);
				words++;
			}

			reader.Close();
		}

		private void ResetState ()
		{
			this.currentWord = "";
			this.currentSubTrie = this.trie;
			this.isUnknownWord = false;
		}

		private void GetTrained ()
		{
			foreach (var data in this.knowledge)
			{
				this.trie.WasTyped(data.Key, data.Value.GetPopularity());
				this.wordsTyped += data.Value.GetPopularity();
			}
		}

		private void WordTyped ()
		{
			if (this.knowledge.ContainsKey(this.currentWord) == false)
			{
				var statistics = new StatisticsT();
				this.knowledge.Add(this.currentWord, statistics);
			}
			else
			{
				this.knowledge[this.currentWord].WordTyped();
			}

			if (this.isUnknownWord)
			{
				this.trie.Add(this.currentWord);
			}
			else
			{
				this.trie.WasTyped(this.currentWord);
			}

			this.wordsTyped++;

			this.ResetState();
		}

		private float Evaluate (int popularity, int prefixesCounter)
		{
			var usageRatio = PersonalizationFactor * this.wordsTyped / this.trie.Size();

			if (usageRatio > 1)
			{
				usageRatio = 1;
			}

			return usageRatio * popularity + (1 - usageRatio) * prefixesCounter;
		}

		#endregion
	}
}


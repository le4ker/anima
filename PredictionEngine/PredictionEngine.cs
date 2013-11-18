namespace Di.Kdd.TextPrediction
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	public class PredictionEngine
	{
		private Trie trie = new Trie();
		private Dictionary<string, Statistics> knowledge = new Dictionary<string, Statistics>();

		private string currentWord = "";
		private int wordsTyped = 0;
		private Trie currentSubTrie;
		private bool isUnknownWord = false;

		private const string WordsFileName = "words.txt";
		private const float PersonalizationFactor = 1.0F;

		#region Constructors

		public PredictionEngine ()
		{
			this.currentSubTrie = this.trie;
		}

		#endregion

		#region Public Methods

		public Dictionary<char, float> GetSortedPredictions()
		{
			var predictions = this.GetPredictions();

			return predictions.OrderByDescending(kv => kv.Value).ToDictionary(k => k.Key, v => v.Value);
		}

		public Dictionary<char, float> GetPredictions()
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

			foreach (char possibleNextLetter in Trie.LatinLetters)
			{
				popularity = this.currentSubTrie.GetPopularity(possibleNextLetter);
				postfixesCounter = this.currentSubTrie.GetSubtrieSize(possibleNextLetter);

				evaluation = this.Evaluate(popularity, postfixesCounter);

				evaluationSum += evaluation;
			}

			Dictionary<char, float> predictions = new Dictionary<char, float>();

			if (evaluationSum == 0)
			{
				foreach (char letter in Trie.LatinLetters)
				{
					predictions.Add(letter, 0.0F);
				}
			}
			else
			{
				foreach (char possibleNextLetter in Trie.LatinLetters)
				{
					popularity = this.currentSubTrie.GetPopularity(possibleNextLetter);
					postfixesCounter = this.currentSubTrie.GetSubtrieSize(possibleNextLetter);

					evaluation = this.Evaluate(popularity, postfixesCounter);

					predictions.Add(possibleNextLetter, evaluation / evaluationSum);
				}
			}

			return predictions;
		}

		public void CharTyped(char character)
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

		public void PredictionCancelled()
		{
			this.ResetState();
		}

		public void SaveDB(string fileName)
		{
			if (File.Exists(fileName))
			{
				File.Delete(fileName);
			}

			var writer = new StreamWriter(File.OpenWrite(fileName));

			foreach (KeyValuePair<string, Statistics> data in this.knowledge)
			{
				writer.WriteLine("{0} {1}", data.Key, data.Value);
			}

			writer.Close();
		}

		public void LoadDB(string fileName)
		{
			if (File.Exists(fileName) == false)
			{
				this.Init();
				this.GetTrained();

				return;
			}

			var reader = File.OpenText(fileName);

			var line = "";

			while ((line = reader.ReadLine()) != null)
			{
				var stringReader = new StringReader(line);
				var keyLength = line.IndexOf(' ');
				var keyArray = new char[keyLength];

				stringReader.ReadBlock(keyArray, 0, keyLength);
				var key = new string(keyArray);

				stringReader.Read();

				var statistics = new Statistics(stringReader.ReadToEnd());

				knowledge.Add(key, statistics);
			}

			reader.Close();

			this.GetTrained();
		}

		public bool ValidCharacter(char character)
		{
			return (Trie.IsLatinLetter(character) || Trie.IsWordSeparator(character));
		}

		#endregion

		#region Private Methods

		private void Init()
		{
			var reader = File.OpenText(WordsFileName);

			var word = "";

			while ((word = reader.ReadLine()) != null)
			{
				if (this.knowledge.ContainsKey(word))
				{
					continue;
				}

				this.knowledge.Add(word, new Statistics());
				this.trie.Add(word);
			}

			reader.Close();
		}

		private void ResetState()
		{
			this.currentWord = "";
			this.currentSubTrie = this.trie;
			this.isUnknownWord = false;
		}

		private void GetTrained()
		{
			foreach (KeyValuePair<string, Statistics> data in this.knowledge)
			{
				this.trie.WasTyped(data.Key, data.Value.GetPopularity());
				this.wordsTyped += data.Value.GetPopularity();
			}
		}

		private void WordTyped()
		{
			if (this.knowledge.ContainsKey(this.currentWord) == false)
			{
				Statistics statistics = new Statistics();
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

		private float Evaluate(int popularity, int prefixesCounter)
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


using Di.Kdd.TextPrediction;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Di.Kdd.WriteRightSimulator
{
	public class WriteRightPredictionEngine
	{
		private const string WordsFileName = "words.txt";

		private Trie trie = new Trie();
		private Dictionary<string, WriteRightStatistics> knowledge = new Dictionary<string, WriteRightStatistics>();
		private float personalizationFactor = 1.0F;

		private string currentWord = "";
		private int wordsTyped = 0;
		private Trie currentSubTrie;
		private bool unknownWord = false;

		private int k = 26;
		private int continuousSuccesses = 0;
		private bool isIdle = false;
		private const int AggressivenessThreshold = 5;

		private const int DefaultInitSize = 1000;
		private const int UpgradeThreshold = 1500;
		private const float UpgrageCleanPercentage = 0.3F;

		public const string LatinLetters ="abcdefghijklmnopqrstuvwxyz";
		private const string WordSeparators = ".,;:!?\n()[]*@{}/&lt;&gt;_+=|\"%#'0123456789";

		private bool PredictionCancelled = false;

		public WriteRightPredictionEngine () 
		{
			this.currentSubTrie = this.trie;
		}

		public void Init()
		{
			this.Init(DefaultInitSize);
		}

		private void Init(int howManyWords)
		{
			var reader = File.OpenText(WordsFileName);

			var word = "";

			for (var i = 0; i < DefaultInitSize && (word = reader.ReadLine()) != null; i++)
			{
				if (this.knowledge.ContainsKey(word))
				{
					continue;
				}

				this.knowledge.Add(word, new WriteRightStatistics(0, 0));
				this.trie.Add(word);
			}

			reader.Close();

			this.GetTrained();
		}

		public void Load (string fileName)
		{
			if (File.Exists(fileName) == false)
			{
				this.Init();

				return;
			}

			var reader = File.OpenText(fileName);
			string line;

			line = reader.ReadLine();
			var words = line.Split(' ');

			this.k = int.Parse(words[0]);
			this.continuousSuccesses = int.Parse(words[1]);

			while ((line = reader.ReadLine()) != null)
			{
				words = line.Split(' ');

				this.knowledge.Add(words[0], new WriteRightStatistics(Int32.Parse(words[1]), long.Parse(words[2])));
				this.trie.Add(words[0]);
			}

			reader.Close();

			this.GetTrained();
		}

		public void Save (string fileName)
		{
			if (File.Exists(fileName))
			{
				File.Delete(fileName);
			}

			var writer = new StreamWriter(File.OpenWrite(fileName));

			writer.WriteLine("{0} {1}", this.k, this.continuousSuccesses);

			foreach (var data in this.knowledge)
			{
				writer.WriteLine("{0} {1} {2}", data.Key, data.Value.GetPopularity(), data.Value.GetTimestamp());
			}

			writer.Close();
		}

		public bool ValidLetter(char letter)
		{
			letter = Char.ToLower(letter);

			return this.IsLatinLetter(letter) || this.IsWordSeparator(letter);
		}

		public void LetterTyped(char letter)
		{
			// todo
			if (WordSeparators.Contains(letter.ToString()))
			{
				this.WordTyped();
				return;
			}

			this.currentWord += letter;

			if (this.currentSubTrie != null)
			{
				this.currentSubTrie = this.currentSubTrie.GetSubTrie(letter);
			}
			else
			{
				this.unknownWord = true;
			}
		}

		public void BadPrediction()
		{
			this.isIdle = true;
		}

		private float Evaluate(int popularity, int prefixesCounter)
		{
			var usageRatio = this.personalizationFactor * this.wordsTyped / this.trie.Size();

			if (usageRatio > 1)
			{
				usageRatio = 1;
			}

			return usageRatio * popularity + (1 - usageRatio) * prefixesCounter;
		}

		public Dictionary<char, float> GetSortedPredictions()
		{
			var predictions = this.GetPredictions();

			predictions = predictions.OrderByDescending(kv => kv.Value).ToDictionary(k => k.Key, v => v.Value);

			var topKPredictions = new Dictionary<char, float>();

			for (var i = 0; i < this.k; i++)
			{
				topKPredictions.Add(predictions.First().Key, predictions.First().Value);
				predictions.Remove(predictions.First().Key);
			}

			return topKPredictions;
		}

		private Dictionary<char, float> GetPredictions()
		{
			var popularity = 0;
			var postfixesCounter = 0;
			var evaluation = 0.0F;
			var evaluationSum = 0.0F;

			if (this.currentSubTrie == null)
			{
				this.unknownWord = true;
				return new Dictionary<char, float>();
			}

			foreach (char possibleNextLetter in LatinLetters)
			{
				popularity = this.currentSubTrie.GetPopularity(possibleNextLetter);
				postfixesCounter = this.currentSubTrie.GetSubtrieSize(possibleNextLetter);

				evaluation = this.Evaluate(popularity, postfixesCounter);

				evaluationSum += evaluation;
			}

			Dictionary<char, float> predictions = new Dictionary<char, float>();

			if (evaluationSum == 0)
			{
				foreach (char letter in LatinLetters)
				{
					predictions.Add(letter, 0.0F);
				}
			}
			else
			{
				foreach (char possibleNextLetter in LatinLetters)
				{
					popularity = this.currentSubTrie.GetPopularity(possibleNextLetter);
					postfixesCounter = this.currentSubTrie.GetSubtrieSize(possibleNextLetter);

					evaluation = this.Evaluate(popularity, postfixesCounter);

					predictions.Add(possibleNextLetter, evaluation / evaluationSum);
				}
			}

			return predictions;
		}

		private void WordTyped()
		{
			//todo 
			if (this.knowledge.ContainsKey(this.currentWord) == false)
			{
				var statistics = new WriteRightStatistics();
				statistics.WordTyped();
				this.knowledge.Add(this.currentWord, statistics);
			}
			else
			{
				this.knowledge[this.currentWord].WordTyped();
			}

			if (this.unknownWord == false)
			{
				this.trie.WasTyped(this.currentWord);
			}
			else
			{
				this.knowledge.Add(this.currentWord, new WriteRightStatistics(1));
				this.trie.Add(this.currentWord);
			}

			this.wordsTyped++;

			this.Reset();
		}


		private void Reset()
		{
			this.currentWord = "";
			this.currentSubTrie = this.trie;
			this.unknownWord = false;
		}

		private void GetTrained()
		{
			foreach (var data in this.knowledge)
			{
				this.trie.WasTyped(data.Key, data.Value.GetPopularity());
				this.wordsTyped += data.Value.GetPopularity();
			}
		}

		private bool IsLatinLetter(char letter)
		{
			return LatinLetters.Contains(letter);
		}

		private bool IsWordSeparator(char letter)
		{
			return WordSeparators.Contains(letter);
		}
	}
}


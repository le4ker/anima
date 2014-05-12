using Di.Kdd.TextPrediction;
using System.Collections.Generic;

namespace Di.Kdd.WriteRightSimulator
{
	using System;

	public class TimeAwarePredictionEngine :TrimmablePredictionEngine
	{
		private const int TIME_PARTITIONS = 5;

		private int currentHour = 0;
		private Trie []timeTries = new Trie[] {new Trie(), new Trie(), new Trie(), new Trie(), new Trie() };

		public void SetTime(int hour)
		{
			this.currentHour = hour;
		}

		protected override Trie GetTrie ()
		{
			int nowTrie = this.currentHour * TimeAwarePredictionEngine.TIME_PARTITIONS / 24;
			return timeTries [nowTrie];
		}

		protected override void LearnNewWord (string newWord)
		{
			foreach (Trie trie in this.timeTries) 
			{
				trie.WasTyped (newWord);
			}
		}
	}
}

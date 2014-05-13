using Di.Kdd.TextPrediction;
using System.Collections.Generic;
using System.ComponentModel;

namespace Di.Kdd.WriteRightSimulator
{
	using System;

	public class TimeAwarePredictionEngine :TrimmablePredictionEngine
	{
		private int currentHour = 0;

		private int timePartitions;
		private Trie []timeTries = new Trie[] {new Trie(), new Trie(), new Trie(), new Trie(), new Trie(), new Trie() };

		public TimeAwarePredictionEngine(int timePartitions)
		{
			this.timePartitions = timePartitions;
		}

		public void SetTime(int hour)
		{
			this.currentHour = hour;
		}

		protected override Trie GetTrie ()
		{
			int nowTrie = this.currentHour * this.timePartitions / 24;
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

using Di.Kdd.TextPrediction;

using System;
using System.Collections.Generic;

namespace Di.Kdd.WriteRightSimulator
{
	public class WriteRightPredictionEngine : PredictionEngine
	{
		private Dictionary<string, WriteRightStatistics> knowledge = new Dictionary<string, WriteRightStatistics>();

		private int k = 26;
		private int continuousSuccesses = 0;
		private const int AggressivenessThreshold = 5;

		private const int DefaultInitSize = 1000;
		private const int UpgradeThreshold = 1500;
		private const float UpgrageCleanPercentage = 0.3F;

		private const string WordSeparators = ".,;:!?\n()[]*@{}/&lt;&gt;_+=|\"%#'0123456789";

		private bool PredictionCancelled = false;

		public WriteRightPredictionEngine () { }

		public override void Init()
		{
			this.Init(DefaultInitSize);
		}

		private void Init(int howManyWords)
		{

		}

		public override void Load (string fileName)
		{

		}

		public override void Save (string fileName)
		{

		}
	}
}


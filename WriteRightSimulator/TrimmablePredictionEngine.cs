namespace Di.Kdd.WriteRightSimulator
{
	using Di.Kdd.TextPrediction;
	using Di.Kdd.Utilities;

	using System;
	using System.Collections.Generic;
	using System.Linq;

	public class TrimmablePredictionEngine : TextPrediction.PredictionEngine<Statistics>
	{
		private int trimThreshold = 1500;
		private float trimPercentage = 0.2F;

		public void SetTrimThreshold (int trimThreshold)
		{
			this.trimThreshold = trimThreshold;

			Logger.Log("New Trim Threshold: " + this.trimThreshold);
		}

		public void SetTrimPercentage (float trimPercentage)
		{
			this.trimPercentage = trimPercentage;

			Logger.Log("New Trim Percentage: " + this.trimPercentage);
		}

		public void Trim()
		{
			if (this.knowledge.Count >= this.trimThreshold)
			{
				Logger.Log("Trimming knowledge, from " + this.knowledge.Count + " to " + this.trimPercentage * this.knowledge.Count);

				this.knowledge = this.knowledge.OrderByDescending(x => x.Value.GetTimestamp()).Take((int) (this.trimPercentage * this.knowledge.Count)).ToDictionary(x => x.Key, x=> x.Value);			
			}
		}

		public void TrimAndSaveDb (string dbPath)
		{
			if (this.knowledge.Count >= this.trimThreshold)
			{
				Logger.Log("Trimming knowledge, from " + this.knowledge.Count + " to " + this.trimPercentage * this.knowledge.Count);

				this.knowledge = this.knowledge.OrderByDescending(x => x.Value.GetTimestamp()).Take((int) (this.trimPercentage * this.knowledge.Count)).ToDictionary(x => x.Key, x=> x.Value);			
			}

			this.SaveDB(dbPath);
		}
	}
}


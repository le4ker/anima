namespace Di.Kdd.WriteRightSimulator
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public class TrimmablePredictionEngine : TextPrediction.PredictionEngine<Statistics>
	{
		private int threshold = 3000;
		private float trimPercentage = 0.3F;

		public void SetThreshold (int threshold)
		{
			this.threshold = threshold;
		}

		public void SetTrimPercentage (float trimPercentage)
		{
			this.trimPercentage = trimPercentage;
		}

		public void TrimAndSaveDb (string dbPath)
		{
			if (this.knowledge.Count >= this.threshold)
			{
				this.knowledge = this.knowledge.OrderByDescending(x => x.Value.GetTimestamp()).Take((int) (this.trimPercentage * this.knowledge.Count)).ToDictionary(x => x.Key, x=> x.Value);			
			}

			this.SaveDB(dbPath);
		}
	}
}


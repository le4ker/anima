namespace Di.Kdd.WriteRightSimulator
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public class TrimmablePredictionEngine : TextPrediction.PredictionEngine<WriteRightSimulator.Statistics>
	{
		private int threshold = 1500;
		private float trimPercentage = 0.3F;

		public void Trim (string dbPath)
		{
			if (this.knowledge.Count >= this.threshold)
			{
				this.knowledge = (Dictionary<string, Statistics>) this.knowledge
												.OrderByDescending(x => x.Value.GetTimestamp())
												.Take((int) (this.trimPercentage * this.knowledge.Count));
			
				this.SaveDB(dbPath);
			}
		}
	}
}


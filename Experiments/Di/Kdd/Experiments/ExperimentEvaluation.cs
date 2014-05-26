namespace Di.Kdd.Experiments
{
	using System;

	public class ExperimentEvaluation
	{
		private float score;
		private int total = 0;

		public void Hit(int predictionSetSize)
		{
			this.score += (float) 1 / predictionSetSize;
			this.total++;
		}

		public void Miss()
		{
			this.total++;
		}

		public float GetPrecission()
		{
			return (float) this.score / this.total;
		}
	}
}


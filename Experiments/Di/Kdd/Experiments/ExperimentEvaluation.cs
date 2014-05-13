namespace Di.Kdd.Experiments
{
	using System;

	public class ExperimentEvaluation
	{
		private float score;
		private int hits, misses;

		public void Hit(int predictionSetSize)
		{
			this.score += (float) 1 / predictionSetSize;
			this.hits++;
		}

		public void Miss()
		{
			this.misses++;
		}

		public float GetScore()
		{
			return this.score / (this.hits + this.misses);
		}
	}
}


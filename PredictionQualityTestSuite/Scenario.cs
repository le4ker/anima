namespace Di.Kdd.PredictionQualityTestSuite
{
	using System;

	public abstract class Scenario
	{
		private string name;
		private float result = 0.0F;
		private float successRate;

		public Scenario (string name, float successRate)
		{
			this.name = name;
			this.successRate = successRate;
		}

		#region Accessors

		public string GetName ()
		{
			return this.name;
		}

		public float GetResult ()
		{
			return this.result;
		}

		public float GetSuccessRate ()
		{
			return this.successRate;
		}

		#endregion

		abstract public void Setup ();

		abstract public bool Run ();

		abstract public void Teardown ();
	}
}


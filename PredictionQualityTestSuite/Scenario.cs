namespace Di.Kdd.PredictionQualityTestSuite
{
	using System;

	public abstract class Scenario
	{
		private string name;

		public Scenario (string name)
		{
			this.name = name;
		}

		public string GetName ()
		{
			return this.name;
		}

		abstract public void Setup ();

		abstract public void Run ();

		abstract public void Teardown ();
	}
}


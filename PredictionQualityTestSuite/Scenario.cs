namespace Di.Kdd.PredictionQualityTestSuite
{
	using System;

	public abstract class Scenario
	{
		abstract public void Setup ();

		abstract public void Run ();

		abstract public void Teardown ();
	}
}


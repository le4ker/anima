namespace Di.Kdd.PredictionQualityTestSuite
{
	using System;

	public class BasicScenario : Scenario
	{
		public BasicScenario () :  base("Basic", 0.8F)
		{
		}

		public override void Setup ()
		{
			this.emails = new Emails("zdnet-content.txt");
		}

		public override bool Run ()
		{


			return this.result >= this.successRate;
		}

		public override void Teardown ()
		{
		}
	}
}


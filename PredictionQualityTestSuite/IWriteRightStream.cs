namespace Di.Kdd.PredictionQualityTestSuite
{
	public interface IWriteRightStream
	{
		bool HasNext ();

		char PeekNext ();

		char ConsumeNext ();

		void Reset();
	}
}


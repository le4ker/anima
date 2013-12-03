namespace Di.Kdd.PredictionQualityTestSuite
{
	using System;
	using System.IO;

	public class DataStream
	{
		private int index = 0;
		private string stream = "";

		private const string DataFolder = "../../Data/";

		public DataStream (string path)
		{
			if (File.Exists(DataFolder + path) == false)
			{
				throw new IOException(DataFolder + path);
			}

			var line = "";

			using (var reader = new StreamReader(File.OpenRead(DataFolder + path)))
			{
				while ((line = reader.ReadLine()) != null)
				{
					this.stream += line;
				}
			}
		}

		public bool HasNext ()
		{
			return this.index < this.stream.Length;
		}

		public char PeekNext ()
		{
			if (this.HasNext())
			{
				return this.stream[this.index];
			}
			else
			{
				return '\0';
			}
		}

		public char ConsumeNext ()
		{
			if (this.HasNext())
			{
				return this.stream[this.index++];
			}
			else
			{
				return '\0';
			}
		}

		public void Reset()
		{
			this.index = 0;
		}
	}
}


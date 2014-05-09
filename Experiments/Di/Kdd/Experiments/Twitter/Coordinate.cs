namespace Di.Kdd.Experiments.Twitter
{
	using System;

	public class Coordinate
	{
		private double x, y;

		public Coordinate(string fromString)
		{
			string []tokens = fromString.Remove(0, 1).Remove (fromString.Length - 2, 1).Split (',');

			this.x = Double.Parse (tokens [0]);
			this.y = Double.Parse (tokens [1]);
		}

		public Coordinate (double x, double y)
		{
			this.x = x;
			this.y = y;
		}
	}
}


using System.Collections;
using System.Threading;
using C5;

namespace Di.Kdd.TextPrediction
{
	using Di.Kdd.Utilities;

	using System;
	using System.IO;
	using System.Linq;

	public class Dictionary
	{
		private const string DataFolder = "../../Data/";
		private const string WordsFileName = "words.txt";

		private static int size;
		private static Dictionary dic;

		private C5.ArrayList<string> words;

		public static Dictionary GetInstance(int size)
		{
			if (Dictionary.dic == null) {
				Dictionary.size = size;
				Dictionary.dic = new Dictionary (size);
			} 
			else if (size != Dictionary.size) 
			{
				Dictionary.size = size;
				Dictionary.dic = new Dictionary (size);
			}

			return Dictionary.dic;
		}

		private Dictionary(int size)
		{
			this.words = new ArrayList<string> ();

			var reader = File.OpenText(DataFolder + WordsFileName);

			var words = 0;
			var word = "";

			for (int i = 0; i < size; i++)
			{
				this.words.Add (reader.ReadLine ());
			}

			reader.Close();
		}

		public ArrayList<string> Words() { return this.words; }
	}
}

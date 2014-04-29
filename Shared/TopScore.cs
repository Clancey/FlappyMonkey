using System;
using System.IO.IsolatedStorage;
using System.IO;

namespace FlappyMonkey
{
	public static class TopScore
	{
		static TopScore ()
		{
			isoStore = IsolatedStorageFile.GetStore (IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
			current = getScore ();
		}

		public delegate void NewTopScoreDelegate (int oldScore,int newScore);
		public static event NewTopScoreDelegate OnNewTopScore;

		static int current;

		public static int Current {
			get {
				return current;
			}
			set {
				if (value <= current)
					return;

				var evt = OnNewTopScore;
				if (evt != null)
					evt (current, value);

				current = value;
				saveScore (current);
			}
		}

		static IsolatedStorageFile isoStore;
		static string fileName = "data";

		static void saveScore (int score)
		{
			using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream (fileName, FileMode.Create, isoStore)) {
				using (StreamWriter writer = new StreamWriter (isoStream)) {
					writer.WriteLine (score);
				}
			}

		}

		static int getScore ()
		{
			try {
				if (isoStore.FileExists (fileName)) {
					using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream (fileName, FileMode.Open, isoStore)) {
						using (StreamReader reader = new StreamReader (isoStream)) {
							int score;
							int.TryParse (reader.ReadToEnd (), out score);
							return score;
						}
					}
				}
			} catch (Exception ex) {
				Console.WriteLine (ex);
			}
			return 0;
		}
	}
}


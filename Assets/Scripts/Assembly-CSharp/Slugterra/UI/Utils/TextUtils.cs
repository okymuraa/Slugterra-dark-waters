namespace Slugterra.UI.Utils
{
	public static class TextUtils
	{
		private static string[] _slugLevels = new string[6] { " ", "I", "II", "III", "IV", "V" };

		public static string GetSlugLevel(int level)
		{
			if (level >= 0 && level < _slugLevels.Length)
			{
				return _slugLevels[level];
			}
			return level.ToString();
		}
	}
}

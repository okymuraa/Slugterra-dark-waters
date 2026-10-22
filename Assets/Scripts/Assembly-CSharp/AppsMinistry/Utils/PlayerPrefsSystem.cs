using UnityEngine;

namespace AppsMinistry.Utils
{
	public class PlayerPrefsSystem
	{
		public static bool HasKey(string key)
		{
			bool flag = false;
			return PlayerPrefs.HasKey(key);
		}
	}
}

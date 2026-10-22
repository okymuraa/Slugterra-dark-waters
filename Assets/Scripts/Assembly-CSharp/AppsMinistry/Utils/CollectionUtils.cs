using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppsMinistry.Utils
{
	public static class CollectionUtils
	{
		public static T PeekRandomItem<T>(this List<T> list)
		{
			if (list.IsNullOrEmpty())
			{
				Debug.LogWarning("List is null or empty");
				return default(T);
			}
			return list[Random.Range(0, list.Count)];
		}

		public static T PopRandomItem<T>(this List<T> list)
		{
			T val = list.PeekRandomItem();
			if (!list.IsNullOrEmpty())
			{
				list.Remove(val);
			}
			return val;
		}

		public static bool IsNullOrEmpty<T>(this List<T> list)
		{
			return list == null || list.Count == 0;
		}

		public static IEnumerator WaitForRealSeconds(float time)
		{
			float start = Time.realtimeSinceStartup;
			while (Time.realtimeSinceStartup < start + time)
			{
				yield return null;
			}
		}
	}
}

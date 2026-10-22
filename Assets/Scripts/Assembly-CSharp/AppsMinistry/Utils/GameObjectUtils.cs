using System.Collections.Generic;
using UnityEngine;

namespace AppsMinistry.Utils
{
	public static class GameObjectUtils
	{
		public static List<T> GetComponentsInHierarchy<T>(this GameObject go, bool includeInactive = true) where T : Component
		{
			List<T> list = new List<T>();
			List<T> list2 = new List<T>(go.GetComponents<T>());
			list2.AddRange(go.GetComponentsInChildren<T>(includeInactive));
			foreach (T item in list2)
			{
				if (item != null && !list.Contains(item))
				{
					list.Add(item);
				}
			}
			return list;
		}
	}
}

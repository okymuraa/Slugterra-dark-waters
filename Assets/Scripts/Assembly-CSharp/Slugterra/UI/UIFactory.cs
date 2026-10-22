using System;
using System.Collections.Generic;
using UnityEngine;

namespace Slugterra.UI
{
	public class UIFactory
	{
		private static UIFactory _instance;

		private IDictionary<Type, string> _resourcesPaths;

		private IDictionary<Type, GameObject> _loadedPrefabs;

		public static UIFactory Instance
		{
			get
			{
				return _instance;
			}
		}

		static UIFactory()
		{
			_instance = new UIFactory();
			_instance._resourcesPaths = new Dictionary<Type, string>();
			_instance._loadedPrefabs = new Dictionary<Type, GameObject>();
		}

		public GameObject GetPrefab(Type type)
		{
			if (!_loadedPrefabs.ContainsKey(type))
			{
				LoadPrefab(type);
			}
			return _loadedPrefabs[type];
		}

		private void LoadPrefab(Type type)
		{
			GameObject value = Resources.Load(_resourcesPaths[type]) as GameObject;
			_loadedPrefabs.Add(type, value);
		}

		public void Clear()
		{
			_loadedPrefabs.Clear();
			Resources.UnloadUnusedAssets();
		}
	}
}

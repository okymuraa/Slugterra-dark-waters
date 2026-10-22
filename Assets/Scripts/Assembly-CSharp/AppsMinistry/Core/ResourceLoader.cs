using System.Collections.Generic;
using UnityEngine;

namespace AppsMinistry.Core
{
	public class ResourceLoader<T> where T : Object
	{
		protected Dictionary<string, string> _stringResourcesPath = new Dictionary<string, string>();

		protected Dictionary<string, T> _loadedObjects = new Dictionary<string, T>();

		public T this[string name]
		{
			get
			{
				return GetObjectByName(name);
			}
		}

		public void Add(string name, string path)
		{
			_stringResourcesPath.Add(name, path);
		}

		public void Add(IDictionary<string, string> resourcesPath)
		{
			foreach (KeyValuePair<string, string> item in resourcesPath)
			{
				Add(item.Key, item.Value);
			}
		}

		public T GetObjectByName(string name)
		{
			string value;
			if (!_stringResourcesPath.TryGetValue(name, out value))
			{
				Debug.LogWarningFormat("ResourceLoader does not have string key {0}", name);
				return (T)null;
			}
			T value2;
			if (!_loadedObjects.TryGetValue(name, out value2))
			{
				value2 = Resources.Load<T>(value);
				_loadedObjects[name] = value2;
			}
			return value2;
		}

		public virtual void Clear()
		{
			_stringResourcesPath.Clear();
			_loadedObjects.Clear();
		}
	}
}

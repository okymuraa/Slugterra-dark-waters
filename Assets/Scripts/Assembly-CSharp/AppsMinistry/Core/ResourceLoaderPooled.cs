using System.Collections.Generic;
using PathologicalGames;
using UnityEngine;

namespace AppsMinistry.Core
{
	public class ResourceLoaderPooled : ResourceLoader<GameObject>
	{
		private Dictionary<string, bool> _pooled = new Dictionary<string, bool>();

		private SpawnPool _pool;

		public ResourceLoaderPooled(string poolName)
		{
			_pool = PoolManager.Pools.Create(poolName);
		}

		public void Add(string name, string path, bool pooled)
		{
			Add(name, path);
			if (pooled)
			{
				_pooled[name] = false;
			}
		}

		public GameObject Create(string name, Vector3 position, Quaternion rotation)
		{
			GameObject objectByName = GetObjectByName(name);
			Transform transform = null;
			if (objectByName != null)
			{
				bool value;
				if (_pooled.TryGetValue(name, out value))
				{
					if (!value)
					{
						CreatePrefabPool(objectByName);
						_pooled[name] = true;
					}
					transform = _pool.Spawn(objectByName, position, rotation);
				}
				else
				{
					GameObject gameObject = Object.Instantiate(objectByName, position, rotation);
					if (gameObject != null)
					{
						transform = gameObject.transform;
					}
				}
			}
			return (!(transform != null)) ? null : transform.gameObject;
		}

		public void Delete(GameObject instance, float time = 0f)
		{
			if (!(instance != null))
			{
				return;
			}
			time = Mathf.Clamp(time, 0f, float.PositiveInfinity);
			if (_pool.GetPrefab(instance) != null)
			{
				if (time != 0f)
				{
					_pool.Despawn(instance.transform, time);
				}
				else
				{
					_pool.Despawn(instance.transform);
				}
			}
			else if (time != 0f)
			{
				Object.Destroy(instance, time);
			}
			else
			{
				Object.Destroy(instance);
			}
		}

		private void CreatePrefabPool(GameObject prefab)
		{
			PrefabPool prefabPool = new PrefabPool(prefab.transform);
			prefabPool.preloadAmount = 0;
			_pool.CreatePrefabPool(prefabPool);
		}

		public override void Clear()
		{
			if (_pool != null)
			{
				PoolManager.Pools.Destroy(_pool.poolName);
			}
			_pooled.Clear();
			base.Clear();
		}
	}
}

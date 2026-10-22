using UnityEngine;

namespace KHD
{
	public class SingletonCrossSceneAutoCreate<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T _instance;

		private static bool applicationIsQuitting = false;

		private static object _lock = new object();

		public static T Instance
		{
			get
			{
				if (applicationIsQuitting)
				{
					Debug.LogWarning(string.Concat("[Singleton] Instance '", typeof(T), "' already destroyed on application quit. Won't create again - returning null."));
					return (T)null;
				}
				lock (_lock)
				{
					if (_instance == null)
					{
						_instance = (T)Object.FindObjectOfType(typeof(T));
						if (Object.FindObjectsOfType(typeof(T)).Length > 1)
						{
							Debug.LogError("[Singleton] Something went really wrong  - there should never be more than 1 singleton! Reopening the scene might fix it.");
							return _instance;
						}
						if (_instance == null)
						{
							GameObject gameObject = new GameObject();
							_instance = gameObject.AddComponent<T>();
							gameObject.name = "(singleton) " + typeof(T).ToString();
							Object.DontDestroyOnLoad(gameObject);
							Debug.Log(string.Concat("[Singleton] An instance of ", typeof(T), " is needed in the scene, so '", gameObject, "' was created."));
						}
						else
						{
							Debug.Log("[Singleton] Using instance already created: " + _instance.gameObject.name);
							Object.DontDestroyOnLoad(_instance.gameObject);
						}
					}
					return _instance;
				}
			}
		}

		public static bool IsExist()
		{
			return _instance != null;
		}

		protected virtual void OnDestroy()
		{
			_instance = (T)null;
			applicationIsQuitting = true;
		}
	}
}

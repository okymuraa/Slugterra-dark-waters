using UnityEngine;

namespace AppsMinistry.Core
{
	public class DonDestroyMonoSingleton<T> : MonoBehaviour where T : DonDestroyMonoSingleton<T>
	{
		private static T _instance;

		private static string COMPANY_NAME = "AppsMinistry";

		private static GameObject _baseGameObject;

		public static bool IsAlive
		{
			get
			{
				return _instance != null;
			}
		}

		public static T Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = Object.FindObjectOfType<T>();
					if (_instance == null)
					{
						GameObject gameObject = new GameObject(typeof(T).Name);
						_instance = gameObject.AddComponent<T>();
						Object.DontDestroyOnLoad(_instance);
						gameObject.transform.parent = GetBaseObject().transform;
					}
				}
				return _instance;
			}
		}

		protected virtual void OnCreate()
		{
		}

		private void Awake()
		{
			OnCreate();
		}

		protected virtual void OnReleaseResource()
		{
		}

		private void OnDestroy()
		{
			OnReleaseResource();
		}

		protected virtual void Update()
		{
		}

		public static GameObject GetBaseObject()
		{
			if (_baseGameObject == null)
			{
				_baseGameObject = GameObject.Find(COMPANY_NAME);
				if (_baseGameObject == null)
				{
					_baseGameObject = new GameObject(COMPANY_NAME);
				}
			}
			return _baseGameObject;
		}
	}
}

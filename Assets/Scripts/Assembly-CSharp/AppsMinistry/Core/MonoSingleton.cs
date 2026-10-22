using System;
using UnityEngine;

namespace AppsMinistry.Core
{
	public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
	{
		private static int _levelId;

		private static T _instance;

		private static bool _isQuit;

		private static AppSingletonAttribute singletonAttribute;

		private static string COMPANY_NAME;

		private static GameObject _baseGameObject;

		protected static bool IsQuit
		{
			get
			{
				return _isQuit;
			}
		}

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
				if (_instance == null && !_isQuit)
				{
					_instance = UnityEngine.Object.FindObjectOfType<T>();
					if (_instance == null)
					{
						GameObject gameObject = new GameObject(typeof(T).Name);
						_instance = gameObject.AddComponent<T>();
						Attribute[] customAttributes = Attribute.GetCustomAttributes(typeof(T));
						Attribute[] array = customAttributes;
						foreach (Attribute attribute in array)
						{
							if (attribute is AppSingletonAttribute)
							{
								singletonAttribute = attribute as AppSingletonAttribute;
							}
						}
						if (singletonAttribute != null)
						{
							UnityEngine.Object.DontDestroyOnLoad(_instance);
						}
						gameObject.transform.parent = GetBaseObject().transform;
					}
				}
				return _instance;
			}
		}

		static MonoSingleton()
		{
			COMPANY_NAME = "AppsMinistry";
			_isQuit = false;
			singletonAttribute = null;
		}

		protected virtual void OnCreate()
		{
		}

		private void Awake()
		{
			OnCreate();
			_levelId = Application.loadedLevel;
		}

		protected virtual void Update()
		{
			if (singletonAttribute == null && _levelId != Application.loadedLevel)
			{
				UnityEngine.Object.Destroy(gameObject);
			}
		}

		protected virtual void OnReleaseResource()
		{
		}

		private void OnDestroy()
		{
			_instance = (T)null;
			OnReleaseResource();
		}

		protected virtual void OnQuit()
		{
		}

		private void OnApplicationQuit()
		{
			_isQuit = true;
			OnQuit();
		}

		public static GameObject GetBaseObject()
		{
			if (_baseGameObject == null)
			{
				_baseGameObject = GameObject.Find(COMPANY_NAME);
				if (_baseGameObject == null)
				{
					_baseGameObject = new GameObject(COMPANY_NAME);
					UnityEngine.Object.DontDestroyOnLoad(_baseGameObject);
				}
			}
			return _baseGameObject;
		}
	}
}

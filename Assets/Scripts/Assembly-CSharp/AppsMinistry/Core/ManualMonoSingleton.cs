using UnityEngine;

namespace AppsMinistry.Core
{
	public class ManualMonoSingleton<T> : MonoBehaviour where T : ManualMonoSingleton<T>
	{
		private static T _instance;

		public static T ManualInstance
		{
			get
			{
				return _instance;
			}
		}

		protected virtual void OnCreate()
		{
		}

		private void Awake()
		{
			if (_instance != null)
			{
				Debug.LogErrorFormat("Instance of {0} already exists. GameObject \"{1}\" will be destroyed.", GetType().ToString(), gameObject.name);
				Object.DestroyImmediate(this);
			}
			else
			{
				_instance = GetComponent<T>();
				OnCreate();
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
	}
}

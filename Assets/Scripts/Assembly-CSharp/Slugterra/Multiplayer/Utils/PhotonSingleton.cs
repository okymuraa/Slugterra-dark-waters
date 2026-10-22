using Photon;
using UnityEngine;

namespace Slugterra.Multiplayer.Utils
{
	public class PhotonSingleton<T> : PunBehaviour where T : PhotonSingleton<T>
	{
		private static T _instance;

		public static T Instance
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
	}
}

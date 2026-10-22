using UnityEngine;

namespace AppsMinistry.Scene
{
	public class SceneController : MonoBehaviour
	{
		protected virtual void OnLoad()
		{
		}

		private void Awake()
		{
			OnLoad();
		}

		protected virtual void OnLoaded()
		{
		}

		private void Start()
		{
			OnLoaded();
		}

		protected virtual void OnClose()
		{
		}

		private void OnDestroy()
		{
			OnClose();
		}

		public virtual void OnRespawn()
		{
		}

		protected virtual void OnUpdate()
		{
		}

		private void Update()
		{
			OnUpdate();
		}
	}
}

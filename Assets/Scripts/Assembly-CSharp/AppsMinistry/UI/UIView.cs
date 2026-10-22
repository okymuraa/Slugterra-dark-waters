using UnityEngine;

namespace AppsMinistry.UI
{
	public class UIView : MonoBehaviour
	{
		protected virtual void OnStart()
		{
		}

		private void Start()
		{
			OnStart();
		}

		protected virtual void OnReleaseResource()
		{
		}

		private void OnDestroy()
		{
			OnReleaseResource();
		}

		protected virtual void OnApplyModel(object model)
		{
		}

		public void ApplyModel(object model)
		{
			OnApplyModel(model);
		}
	}
}

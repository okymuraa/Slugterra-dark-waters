using AppsMinistry.Core;

namespace AppsMinistry.UI
{
	public abstract class PopupView : UIView
	{
		protected virtual void OnCreate()
		{
		}

		private void Awake()
		{
			DonDestroyMonoSingleton<PopupManager>.Instance.RegisterPopup(this);
			OnCreate();
		}

		protected virtual void OnEnable()
		{
			DonDestroyMonoSingleton<PopupManager>.Instance.OnShowPopup(this);
		}

		protected virtual void OnDisable()
		{
			if (DonDestroyMonoSingleton<PopupManager>.IsAlive)
			{
				DonDestroyMonoSingleton<PopupManager>.Instance.OnClosePopup(this);
			}
		}

		public abstract void Close();

		protected override void OnReleaseResource()
		{
			base.OnReleaseResource();
			if (DonDestroyMonoSingleton<PopupManager>.IsAlive)
			{
				DonDestroyMonoSingleton<PopupManager>.Instance.UnRegisterPopup(this);
			}
		}
	}
}

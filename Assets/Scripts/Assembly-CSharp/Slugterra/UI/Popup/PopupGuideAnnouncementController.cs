using AppsMinistry.UI;

namespace Slugterra.UI.Popup
{
	public class PopupGuideAnnouncementController : UIController<PopupAnnouncementView>
	{
		protected override void _OnEnable()
		{
			View.OnCloseEvent += OnClose;
		}

		protected override void _OnDisable()
		{
			View.OnCloseEvent -= OnClose;
		}

		private void OnClose()
		{
			View.gameObject.SetActive(false);
		}
	}
}

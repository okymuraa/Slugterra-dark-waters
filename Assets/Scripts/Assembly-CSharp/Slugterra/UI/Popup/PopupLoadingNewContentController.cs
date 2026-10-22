using System;
using AppsMinistry.UI;

namespace Slugterra.UI.Popup
{
	public class PopupLoadingNewContentController : UIController<PopupLoadingNewContentView>
	{
		private void OnDecline(Action action)
		{
			if (action != null)
			{
				action();
			}
			View.gameObject.SetActive(false);
		}

		private void OnAccept(Action action)
		{
			if (action != null)
			{
				action();
			}
			View.gameObject.SetActive(false);
		}
	}
}

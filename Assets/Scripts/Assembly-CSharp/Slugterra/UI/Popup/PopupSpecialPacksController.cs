using System;
using AppsMinistry.UI;

namespace Slugterra.UI.Popup
{
	public class PopupSpecialPacksController : UIController<PopupSpecialPacksView>
	{
		private void OnDecline(Action action)
		{
			if (action != null)
			{
				action();
			}
			View.Hide();
		}

		private void OnAccept(Action action)
		{
			if (action != null)
			{
				action();
			}
			View.Hide();
		}
	}
}

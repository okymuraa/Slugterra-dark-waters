using System;
using AppsMinistry.UI;

namespace Slugterra.UI.Popup
{
	public class PopupConfirmOfferIgnoringController : UIController<PopupConfirmOfferIgnoringView>
	{
		protected override void _OnEnable()
		{
			View.OnDeclineEvent += OnDecline;
			View.OnAcceptEvent += OnAccept;
		}

		protected override void _OnDisable()
		{
			View.OnDeclineEvent -= OnDecline;
			View.OnAcceptEvent -= OnAccept;
		}

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

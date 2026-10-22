using System;
using AppsMinistry.UI;

namespace Slugterra.UI.Popup
{
	public class PopupUseRecommendedView : PopupView
	{
		public Action OnBtnContinueClick;

		public Action OnBtnCheckoutClick;

		public override void Close()
		{
			gameObject.SetActive(false);
		}

		public void Show()
		{
			gameObject.SetActive(true);
		}

		private void OnBtnContinue()
		{
			if (OnBtnContinueClick != null)
			{
				OnBtnContinueClick();
			}
		}

		private void OnBtnCheckout()
		{
			if (OnBtnCheckoutClick != null)
			{
				OnBtnCheckoutClick();
			}
		}
	}
}

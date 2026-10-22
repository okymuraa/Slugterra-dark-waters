using UnityEngine;

namespace Slugterra.UI.Shops.PaymentStates
{
	public class RecoverPaymentState : AbstractPaymentState
	{
		[SerializeField]
		private TextMesh _freeTitle;

		public override void Show()
		{
			base.Show();
			SetDefaultView();
		}

		public void SetAdView()
		{
			if (_freeTitle != null && !_freeTitle.gameObject.activeSelf)
			{
				_freeTitle.gameObject.SetActive(true);
			}
			if (_title != null && _title.gameObject.activeSelf)
			{
				_title.gameObject.SetActive(false);
			}
		}

		public void SetDefaultView()
		{
			if (_freeTitle != null && _freeTitle.gameObject.activeSelf)
			{
				_freeTitle.gameObject.SetActive(false);
			}
			if (_title != null && !_title.gameObject.activeSelf)
			{
				_title.gameObject.SetActive(true);
			}
		}
	}
}

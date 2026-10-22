using System;
using AppsMinistry.Core;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using Slugterra.InAppPurchases;
using UnityEngine;

namespace Slugterra.UI.Popup
{
	public class PopupSubscriptionAnnouncementView : PopupAnnouncementView
	{
		[SerializeField]
		private TextMesh _priceLbl;

		public override void Open(AnnouncementContents content, Action<AnnounceStatus> onClose, bool isBadge = false)
		{
			base.Open(content, onClose, isBadge);
			string empty = string.Empty;
			empty = DonDestroyMonoSingleton<SlugterraPurchaseContext>.Instance.GetFormattedPrice(content.InAppPurchaseIdAndroid);
			if (empty != null)
			{
				_priceLbl.text = empty;
			}
		}

		protected virtual void OnBtnBuyClick()
		{
			DonDestroyMonoSingleton<SlugterraPurchaseContext>.Instance.PurchaseProduct(_content.InAppPurchaseIdAndroid, OnSuccessBuy);
		}

		private void OnSuccessBuy()
		{
			_onClose(AnnounceStatus.Done);
			OnClose();
		}
	}
}

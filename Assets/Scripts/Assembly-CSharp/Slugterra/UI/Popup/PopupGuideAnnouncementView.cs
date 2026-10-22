using System;
using System.Collections.Generic;
using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using Slugterra.InAppPurchases;
using UnityEngine;

namespace Slugterra.UI.Popup
{
	public class PopupGuideAnnouncementView : PopupAnnouncementView
	{
		protected const float ItemOffset = 1.82f;

		[SerializeField]
		protected GameObject _listItemPrefab;

		[SerializeField]
		protected tk2dButton _buyButton;

		[SerializeField]
		protected TextMesh _buyPrice;

		[SerializeField]
		protected TextMesh _discountAmount;

		[SerializeField]
		protected TextMesh _discountAmountShade;

		[SerializeField]
		protected tk2dButton _closeButton;

		[SerializeField]
		protected TextMesh _titleText;

		[SerializeField]
		protected TextMesh _offerText;

		[SerializeField]
		protected TextMesh _specTitleText;

		[SerializeField]
		protected TextMesh _specOfferText;

		[SerializeField]
		protected Transform _listContainer;

		[SerializeField]
		protected tk2dUIScrollableArea _scrollableArea;

		protected List<GameObject> _listItems = new List<GameObject>();

		[SerializeField]
		protected GameObject _prontoIcon;

		[SerializeField]
		protected GameObject _trixieIcon;

		[SerializeField]
		protected GameObject _kordIcon;

		[SerializeField]
		protected GameObject _specialOfferBadge;

		[SerializeField]
		protected GameObject _orangeBadge;

		[SerializeField]
		protected GameObject _discountBadge;

		[SerializeField]
		protected GameObject _hitBadge;

		[SerializeField]
		protected GameObject _newBadge;

		[SerializeField]
		protected GameObject _vipBadge;

		[SerializeField]
		protected MoveAnimUI _appearingAnimation;

		private PopupConfirmOfferIgnoringView _popupConfirm;

		public override void Open(AnnouncementContents content, Action<AnnounceStatus> onClose, bool isBadge = false)
		{
			base.Open(content, onClose, isBadge);
			ClearScrollList();
			if (_specTitleText != null && (content.AnnouncementType == AnnouncementType.SpecialOffer || content.AnnouncementType == AnnouncementType.LimitedOffer || content.AnnouncementType == AnnouncementType.VipOffer))
			{
				_specTitleText.text = Localization.GetValue(content.TitleTextKey, string.Empty);
				_specOfferText.text = Localization.GetValue(content.ContentTextKey, string.Empty);
				_titleText.text = string.Empty;
				_offerText.text = string.Empty;
				_popupConfirm = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupConfirmOfferIgnoringView>();
			}
			else
			{
				_titleText.text = Localization.GetValue(content.TitleTextKey, string.Empty);
				_offerText.text = Localization.GetValue(content.ContentTextKey, string.Empty);
			}
			if (_discountAmount != null)
			{
				TextMesh discountAmount = _discountAmount;
				string text = string.Format("-{0}%", content.DiscountAmount);
				_discountAmountShade.text = text;
				discountAmount.text = text;
			}
			foreach (AnnounceHeaderBadge headerBadge in content.HeaderBadges)
			{
				SetupHeaderBadge(headerBadge);
			}
			switch (content.HeaderCharacter)
			{
			case AnnounceHeaderCharacter.Pronto:
				_prontoIcon.SetActive(true);
				break;
			case AnnounceHeaderCharacter.Trixie:
				_trixieIcon.SetActive(true);
				break;
			default:
				_kordIcon.SetActive(true);
				break;
			}
			if (_buyButton != null)
			{
				string empty = string.Empty;
				empty = DonDestroyMonoSingleton<SlugterraPurchaseContext>.Instance.GetFormattedPrice(content.InAppPurchaseIdAndroid);
				if (empty != null)
				{
					_buyPrice.text = empty;
				}
			}
			ShowList();
			if (_isBadgeOffer)
			{
				Close();
				_appearingAnimation.enabled = false;
				_appearingAnimation.transform.localPosition = _appearingAnimation.TargetPosition;
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
		}

		public override void Close()
		{
			OnBtnCloseClick();
		}

		private void ShowList()
		{
			AnnounceItemViewType announceItemViewType = AnnounceItemViewType.Price;
			AnnouncementType announcementType = _content.AnnouncementType;
			announceItemViewType = ((announcementType != AnnouncementType.Unlock && announcementType != AnnouncementType.UnlockAssistant) ? AnnounceItemViewType.Price : AnnounceItemViewType.Buy);
			foreach (SlugType slug in _content.ListContents.Slugs)
			{
				CreateNewListItem().ShowSlug(slug, announceItemViewType, this);
			}
			foreach (BlasterType blaster in _content.ListContents.Blasters)
			{
				CreateNewListItem().ShowBlaster(blaster, announceItemViewType, this);
			}
			foreach (ArmorType armor in _content.ListContents.Armors)
			{
				CreateNewListItem().ShowArmor(armor, announceItemViewType, this);
			}
			foreach (AllyType ally in _content.ListContents.Allies)
			{
				CreateNewListItem().ShowAlly(ally, announceItemViewType, this);
			}
		}

		private void SetupHeaderBadge(AnnounceHeaderBadge badge)
		{
			switch (badge)
			{
			case AnnounceHeaderBadge.Discount:
				_discountBadge.SetActive(true);
				break;
			case AnnounceHeaderBadge.Hit:
				_orangeBadge.SetActive(true);
				_hitBadge.SetActive(true);
				break;
			case AnnounceHeaderBadge.New:
				_orangeBadge.SetActive(true);
				_newBadge.SetActive(true);
				break;
			case AnnounceHeaderBadge.SpecialOffer:
				_specialOfferBadge.SetActive(true);
				break;
			case AnnounceHeaderBadge.Vip:
				_orangeBadge.SetActive(true);
				_vipBadge.SetActive(true);
				break;
			}
		}

		protected AnnouncementListItemController CreateNewListItem()
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(_listItemPrefab);
			gameObject.transform.parent = _listContainer;
			gameObject.transform.localPosition = -Vector3.up * _listItems.Count * 1.82f;
			_listItems.Add(gameObject);
			if (_scrollableArea != null)
			{
				_scrollableArea.ContentLength = (float)_listItems.Count * 1.82f;
			}
			return gameObject.GetComponent<AnnouncementListItemController>();
		}

		protected void ClearScrollList()
		{
			for (int num = _listItems.Count - 1; num >= 0; num--)
			{
				UnityEngine.Object.Destroy(_listItems[num].gameObject);
			}
			_listItems = new List<GameObject>();
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

		protected override void OnBtnCloseClick()
		{
			if (_isBadgeOffer)
			{
				_onClose(AnnounceStatus.Ignored);
				return;
			}
			if (_popupConfirm != null)
			{
				_popupConfirm.Show("popup_wait_title", "popup_are_you_sure", OnConfirmClosing, null);
				return;
			}
			_onClose(AnnounceStatus.Ignored);
			OnClose();
		}

		public void ItemDetailsClick()
		{
			_onClose(AnnounceStatus.Done);
			OnClose();
		}

		private void OnConfirmClosing()
		{
			_onClose(AnnounceStatus.Ignored);
			OnClose();
		}
	}
}

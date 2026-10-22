using System;
using AppsMinistry.UI;
using DG.Tweening;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using Slugterra.UI.Popup;
using UnityEngine;

namespace Slugterra.UI.Announcements
{
	public class InAppAnnouncementsView : UIView
	{
		[SerializeField]
		private GameObject _prefOneItemPopup;

		[SerializeField]
		private GameObject _prefMoreItemsPopup;

		[SerializeField]
		private GameObject _prefDiscountPopup;

		[SerializeField]
		private GameObject _prefAssistantUnlockPopup;

		[SerializeField]
		private GameObject _prefLowBalancePopup;

		[SerializeField]
		private GameObject _prefRewardMultiplierPopup;

		[SerializeField]
		private Transform _parentContainer;

		[SerializeField]
		private Transform _parentBadge;

		[SerializeField]
		private Transform _parentBadgeOffer;

		[SerializeField]
		private GameObject _bgShadow;

		[SerializeField]
		private ScrollController _shopScroll;

		private PopupAnnouncementView _popupView;

		private PopupAnnouncementView _badgeAnnouncementView;

		public void CreateAnnounce(AnnouncementContents contents, Action<AnnounceStatus> onClose)
		{
			GameObject original = _prefMoreItemsPopup;
			switch (contents.AnnouncementType)
			{
			case AnnouncementType.Unlock:
				original = ((contents.ListContents.Count() != 1) ? _prefMoreItemsPopup : _prefOneItemPopup);
				break;
			case AnnouncementType.UnlockAssistant:
				original = _prefAssistantUnlockPopup;
				break;
			case AnnouncementType.LimitedOffer:
			case AnnouncementType.SpecialOffer:
			case AnnouncementType.VipOffer:
				original = _prefDiscountPopup;
				break;
			case AnnouncementType.LowBalance:
				original = _prefLowBalancePopup;
				break;
			case AnnouncementType.RewardMultiplier:
				original = _prefRewardMultiplierPopup;
				break;
			}
			SetBGShadow();
			GameObject gameObject = UnityEngine.Object.Instantiate(original);
			gameObject.transform.parent = _parentContainer;
			gameObject.transform.localPosition = Vector3.zero;
			_popupView = gameObject.GetComponent<PopupAnnouncementView>();
			_popupView.Open(contents, onClose);
		}

		public void SetBadgeOffer(AnnouncementContents announce, Action<AnnounceStatus> onClose)
		{
			if (announce != null)
			{
				if (_badgeAnnouncementView != null)
				{
					_badgeAnnouncementView.Open(announce, onClose, true);
				}
				else
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(_prefDiscountPopup);
					gameObject.transform.parent = _parentBadgeOffer;
					gameObject.transform.localPosition = Vector3.zero;
					_badgeAnnouncementView = gameObject.GetComponent<PopupAnnouncementView>();
					_badgeAnnouncementView.Open(announce, onClose, true);
				}
				SetBadge();
			}
			else
			{
				SetBadge(false);
			}
		}

		public void SetBadge(bool enable = true)
		{
			_parentBadge.gameObject.SetActive(enable);
		}

		private void OnBadgeClick()
		{
			SetBGShadow();
			if (_shopScroll != null)
			{
				_shopScroll.allowInput = false;
			}
			_parentBadgeOffer.gameObject.SetActive(true);
			_parentBadge.DOLocalMoveY(3.84f, 0.33f).SetUpdate(true).SetEase(Ease.OutBack)
				.OnComplete(() =>
				{
				});
		}

		public void HideBadgeOffer()
		{
			_parentBadge.DOLocalMoveY(10.85f, 0.33f).SetUpdate(true).SetEase(Ease.InBack)
				.OnComplete(() =>
				{
					_parentBadgeOffer.gameObject.SetActive(false);
					if (_shopScroll != null)
					{
						_shopScroll.allowInput = true;
					}
					SetBGShadow(false);
				});
		}

		public void SetBGShadow(bool isEnable = true)
		{
			_bgShadow.SetActive(isEnable);
		}
	}
}

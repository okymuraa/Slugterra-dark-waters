using AppsMinistry.Core;
using AppsMinistry.Core.Ads;
using AppsMinistry.UI;
using AppsMinistry.Utils;
using Slugterra.App;
using Slugterra.InAppPurchases;
using Slugterra.UI.Popup;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class ShopIAPController : UIController<ShopIAPView>
	{
		private PopupConfirmView _popupConfirm;

		private int _currentTabId = -1;

		protected override void OnCreate()
		{
			base.OnCreate();
			View.OnTabDisactivate(InappTabEnum.Subscriptions);
			View.OnTabDisactivate(InappTabEnum.Gems);
			OnTabClick(InappTabEnum.Currency);
		}

		protected override void OnStart()
		{
			base.OnStart();
			_popupConfirm = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupConfirmView>();
		}

		protected override void _OnEnable()
		{
			View.OnCloseEvent += OnClose;
			View.OnTabEvent += OnTabClick;
		}

		protected override void _OnDisable()
		{
			View.OnCloseEvent -= OnClose;
			View.OnTabEvent -= OnTabClick;
			View.ShowAdsPanel(false);
		}

		private void OnClose()
		{
			View.gameObject.SetActive(false);
		}

		private void OnTabClick(InappTabEnum tabType)
		{
			if (_currentTabId != (int)tabType)
			{
				if (_currentTabId > -1)
				{
					View.OnTabDisactivate((InappTabEnum)_currentTabId);
				}
				_currentTabId = (int)tabType;
				View.OnTabActivate(tabType);
				switch (tabType)
				{
				case InappTabEnum.Currency:
					View.ApplyModel(InApps.GOLD_INAPPS);
					break;
				case InappTabEnum.Subscriptions:
					View.ApplyModel(InApps.SUBSCRIPTION_INAPPS);
					break;
				case InappTabEnum.Gems:
					View.ApplyModel(InApps.GEMS_INAPPS);
					break;
				}
			}
		}

		private void OnBtnWatchAds()
		{
			DonDestroyMonoSingleton<AdsManager>.Instance.RewardSuccessEvent = MonoSingleton<AppController>.Instance.Model.Wallet.OnGetRewardSuccessEvent;
			DonDestroyMonoSingleton<AdsManager>.Instance.RewardFailEvent = MonoSingleton<AppController>.Instance.Model.Wallet.OnGetRewardFailEvent;
			PopupAdsFeedBackView registeredPopup = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupAdsFeedBackView>();
			if (registeredPopup != null)
			{
				registeredPopup.OnCloseEvent += OnClose;
			}
			else
			{
				Debug.LogWarning("ShopIAPController::OnBtnWatchAds PopupAdsFeedBackView is NULL");
			}
			DonDestroyMonoSingleton<CheckInternetService>.Instance.CheckInternet(() =>
			{
				if (DonDestroyMonoSingleton<AdsManager>.Instance.RewardSuccessEvent != null)
				{
					DonDestroyMonoSingleton<AdsManager>.Instance.ShowIncentivizedAds("Wallet");
				}
				else if (DonDestroyMonoSingleton<AdsManager>.Instance.VideoAdsAvailable || DonDestroyMonoSingleton<AdsManager>.Instance.InterstitialAdsAvailable)
				{
					DonDestroyMonoSingleton<AdsManager>.Instance.ShowAds("Wallet");
				}
			}, (string errorMsg) =>
			{
				_popupConfirm.Show("alert_internet_error", () =>
				{
				}, null, false);
			});
		}
	}
}

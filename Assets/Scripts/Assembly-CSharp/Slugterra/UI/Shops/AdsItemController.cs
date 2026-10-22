using AppsMinistry.Core;
using AppsMinistry.Core.Ads;
using AppsMinistry.InAppPurchases;
using AppsMinistry.UI;
using AppsMinistry.Utils;
using Slugterra.App;
using Slugterra.UI.Popup;

namespace Slugterra.UI.Shops
{
	public class AdsItemController : UIController<AdsItemView>
	{
		private InAppPurchase _model;

		public void ApplyModel(InAppPurchase model)
		{
			View.ApplyModel(model);
			_model = model;
		}

		public void OnBtnFreeBonus()
		{
			View.DisableButton();
			PopupConfirmView _popupConfirm = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupConfirmView>();
			if (!View.IsGemsButton)
			{
				DonDestroyMonoSingleton<AdsManager>.Instance.RewardSuccessEvent = MonoSingleton<AppController>.Instance.Model.Wallet.OnGetRewardSuccessEvent;
				DonDestroyMonoSingleton<AdsManager>.Instance.RewardFailEvent = MonoSingleton<AppController>.Instance.Model.Wallet.OnGetRewardFailEvent;
			}
			else
			{
				DonDestroyMonoSingleton<AdsManager>.Instance.RewardSuccessEvent = MonoSingleton<AppController>.Instance.Model.Wallet.OnGetGemsRewardSuccessEvent;
				DonDestroyMonoSingleton<AdsManager>.Instance.RewardFailEvent = MonoSingleton<AppController>.Instance.Model.Wallet.OnGetGemsRewardFailEvent;
			}
			DonDestroyMonoSingleton<CheckInternetService>.Instance.CheckInternet(() =>
			{
				if (DonDestroyMonoSingleton<AdsManager>.Instance.RewardIncentivizeAdsAvailable)
				{
					DonDestroyMonoSingleton<AdsManager>.Instance.ShowIncentivizedAds(string.Empty, true);
				}
				else
				{
					_popupConfirm.Show("alert_ads_not_available", () =>
					{
					}, null, false);
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

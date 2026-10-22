using System;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core;
using AppsMinistry.Core.Ads;
using AppsMinistry.UI;
using AppsMinistry.Utils;
using Slugterra.App;
using Slugterra.UI.Shops;
using UnityEngine;

namespace Slugterra.UI.Popup
{
	public class PopupAdsFeedBackView : PopupView
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnCloseEvent__BackingField;

		[SerializeField]
		private GameObject _getMyBonusBtn;

		[SerializeField]
		private GameObject _getMoreBonusBtn;

		[SerializeField]
		private RewardDesc _coinsPart;

		[SerializeField]
		private RewardDesc _gemsPart;

		[SerializeField]
		private FreeBonusButton _freeBonusButton;

		private PopupConfirmView _popupConfirm;

		public event Action OnCloseEvent
		{
			add
			{
				Action action = OnCloseEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnCloseEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnCloseEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnCloseEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
		}

		public void ShowPopUpGems()
		{
			_coinsPart.lblCount.text = MonoSingleton<AppController>.Instance.Model.Wallet.GoldReward.ToString();
			_gemsPart.lblCount.text = MonoSingleton<AppController>.Instance.Model.Wallet.GemsReward.ToString();
			_coinsPart.parent.SetActive(false);
			_gemsPart.parent.SetActive(true);
			_getMoreBonusBtn.SetActive(false);
			_getMyBonusBtn.SetActive(true);
			gameObject.SetActive(true);
		}

		public void ShowPopUpCoins()
		{
			_coinsPart.lblCount.text = MonoSingleton<AppController>.Instance.Model.Wallet.GoldReward.ToString();
			_gemsPart.lblCount.text = MonoSingleton<AppController>.Instance.Model.Wallet.GemsReward.ToString();
			_gemsPart.parent.SetActive(false);
			_coinsPart.parent.SetActive(true);
			gameObject.SetActive(true);
			_getMoreBonusBtn.SetActive(false);
			_getMyBonusBtn.SetActive(true);
		}

		public void OnBtnCloseClick()
		{
			Close();
		}

		public void OnBtnFreeBonus()
		{
			_freeBonusButton.DisableButton();
			_popupConfirm = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupConfirmView>();
			DonDestroyMonoSingleton<AdsManager>.Instance.RewardSuccessEvent = MonoSingleton<AppController>.Instance.Model.Wallet.OnGetRewardSuccessEvent;
			DonDestroyMonoSingleton<AdsManager>.Instance.RewardFailEvent = MonoSingleton<AppController>.Instance.Model.Wallet.OnGetRewardFailEvent;
			DonDestroyMonoSingleton<CheckInternetService>.Instance.CheckInternet(() =>
			{
				if (DonDestroyMonoSingleton<AdsManager>.Instance.RewardIncentivizeAdsAvailable)
				{
					DonDestroyMonoSingleton<AdsManager>.Instance.ShowIncentivizedAds(string.Empty);
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

		protected override void OnStart()
		{
			gameObject.SetActive(false);
		}

		public override void Close()
		{
			if (OnCloseEvent__BackingField != null)
			{
				OnCloseEvent__BackingField();
			}
			OnCloseEvent__BackingField = null;
			gameObject.SetActive(false);
		}
	}
}

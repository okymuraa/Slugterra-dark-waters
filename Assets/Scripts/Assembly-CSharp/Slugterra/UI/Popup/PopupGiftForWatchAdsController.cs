using System;
using AppsMinistry.Core;
using AppsMinistry.Core.Ads;
using AppsMinistry.UI;
using DG.Tweening;
using UnityEngine;

namespace Slugterra.UI.Popup
{
	public class PopupGiftForWatchAdsController : UIController<PopupGiftForWatchAdsView>
	{
		private bool _isShowPopUp;

		protected override void OnStart()
		{
			base.OnStart();
			View.closeEvent = OnCloseAdsPopUpBtn;
		}

		private void OnBtnWatchAds()
		{
			if (DonDestroyMonoSingleton<AdsManager>.Instance.RewardSuccessEvent != null)
			{
				DonDestroyMonoSingleton<AdsManager>.Instance.ShowIncentivizedAds(Application.loadedLevelName);
			}
			else if (DonDestroyMonoSingleton<AdsManager>.Instance.VideoAdsAvailable || DonDestroyMonoSingleton<AdsManager>.Instance.InterstitialAdsAvailable)
			{
				DonDestroyMonoSingleton<AdsManager>.Instance.ShowAds(Application.loadedLevelName);
			}
			HidePopUp();
		}

		protected override void OnReleaseResource()
		{
			base.OnReleaseResource();
			View.closeEvent = null;
		}

		private void OnCloseAdsPopUpBtn()
		{
			HidePopUp();
		}

		public void ShowPopUp(string textToShow = "", Action callback = null)
		{
			gameObject.SetActive(true);
			if (DonDestroyMonoSingleton<AdsManager>.Instance.IncentivizeAdsAvailable())
			{
				DonDestroyMonoSingleton<AdsManager>.Instance.RewardSuccessEvent = callback;
				if (!string.IsNullOrEmpty(textToShow))
				{
					View.RewardText.text = MonoSingleton<Localization>.Instance.GetValueByKey(textToShow);
				}
				_isShowPopUp = true;
				transform.DOLocalMoveY(0f, 1f).SetUpdate(true).OnComplete(() =>
				{
					_isShowPopUp = true;
				});
			}
		}

		public void HidePopUp()
		{
			_isShowPopUp = false;
			transform.DOLocalMoveY(-1.4f, 0.35f).SetDelay(0.6f).SetUpdate(true)
				.OnComplete(() =>
				{
					gameObject.SetActive(false);
				});
		}

		private void Update()
		{
			if (_isShowPopUp && Input.touchCount > 0)
			{
				OnCloseAdsPopUpBtn();
			}
		}
	}
}

using System;
using AppsMinistry.Plugins;
using ColorTvPlugin;
using UnityEngine;

namespace AppsMinistry.Core.Ads
{
	public class ColorTVIAds : MonoBehaviour, IAds
	{
		private const string DISCOVERY_CENTER_PLACEMENT = "Pause";

		private const string APP_FEATURE_PLACEMENT = "InAppPurchase";

		private const string DIRECT_ENGAGEMENT_PLACEMENT = "StageOpen";

		private const string VIDEO_PLACEMENT = "BetweenLevels";

		private const string RANDOM_AD_PLACEMENT = "MainMenu";

		public string AndroidAppId = string.Empty;

		public string AppleTVAppId = "9778cfab-2c1d-47bf-929d-a1545f37534f";

		private string ChosenAppId = string.Empty;

		private Action GetRewardSuccessEvent;

		private Action GetRewardFailEvent;

		private bool _isIncentivizeAdsAvailable;

		private bool _isVideoAdsAvailable = true;

		private bool _isInterstitialAdsAvailable;

		private bool _adsShowing;

		private bool _canShowAd = true;

		public bool CanShowAd
		{
			set
			{
				_canShowAd = value;
			}
		}

		public Action RewardSuccessEvent
		{
			get
			{
				return GetRewardSuccessEvent;
			}
			set
			{
				GetRewardSuccessEvent = value;
			}
		}

		public Action RewardFailEvent
		{
			get
			{
				return GetRewardFailEvent;
			}
			set
			{
				GetRewardFailEvent = value;
			}
		}

		public bool IncentivizeAdsAvailable
		{
			get
			{
				return _isVideoAdsAvailable;
			}
		}

		public bool VideoAdsAvailable
		{
			get
			{
				return _isVideoAdsAvailable;
			}
		}

		public bool InterstitialAdsAvailable
		{
			get
			{
				return _isVideoAdsAvailable;
			}
		}

		public void Initialize()
		{
			Debug.Log("ColorTVIAds::Initialize");
			ChosenAppId = AndroidAppId;
			StartSDK();
		}

		public void StartSDK()
		{
			Debug.Log("ColorTVIAds::StartSDK");
			MonoBehaviour.print("Starting SDK");
			ColorTv.SetDebugMode(true);
			ColorTv.Init(ChosenAppId);
			ColorTvCallbacks.CurrencyEarned = (Action<ColorTvCurrency>)Delegate.Combine(ColorTvCallbacks.CurrencyEarned, new Action<ColorTvCurrency>(OnCurrencyEarned));
			ColorTvCallbacks.AdLoaded = (Action<string>)Delegate.Combine(ColorTvCallbacks.AdLoaded, new Action<string>(OnAdLoaded));
			ColorTvCallbacks.AdClosed = (Action<string>)Delegate.Combine(ColorTvCallbacks.AdClosed, new Action<string>(OnAdClosed));
			ColorTvCallbacks.AdError = (Action<ColorTvError>)Delegate.Combine(ColorTvCallbacks.AdError, new Action<ColorTvError>(OnError));
			ColorTvCallbacks.AdExpired = (Action<string>)Delegate.Combine(ColorTvCallbacks.AdExpired, new Action<string>(OnAdExpired));
		}

		public bool ShowIncentivizedAds()
		{
			if (_adsShowing)
			{
				return true;
			}
			if (IncentivizeAdsAvailable)
			{
				_adsShowing = true;
				ColorTv.LoadAd("InAppPurchase");
				_canShowAd = true;
			}
			return _adsShowing;
		}

		public void ShowAds(string screenName = "")
		{
			if (VideoAdsAvailable)
			{
				DonDestroyMonoSingleton<AnalyticsManager>.Instance.ShowVideoAds(screenName);
				ColorTv.LoadAd("InAppPurchase");
			}
		}

		public void OnShowDiscoveryCenter()
		{
			MonoBehaviour.print("Show Discovery Center button pressed");
			ColorTv.LoadAd("Pause");
		}

		public void OnShowAppFeature()
		{
			MonoBehaviour.print("Show App Feature button pressed");
			ColorTv.LoadAd("InAppPurchase");
		}

		public void OnShowDirectEngagement()
		{
			MonoBehaviour.print("Show Direct Engagement button pressed");
			ColorTv.LoadAd("StageOpen");
		}

		public void OnShowVideo()
		{
			MonoBehaviour.print("Show Video button pressed");
			ColorTv.LoadAd("BetweenLevels");
		}

		public void OnShowRandomAd()
		{
			MonoBehaviour.print("Show Random Ad button pressed");
			ColorTv.LoadAd("MainMenu");
		}

		public void OnAdLoaded(string placementId)
		{
			Debug.Log("Ad is available for placement: " + placementId);
			if (_canShowAd)
			{
				ColorTv.ShowAd(placementId);
			}
		}

		public void OnAdClosed(string placementId)
		{
			_adsShowing = false;
			Debug.Log("ColorTVIAds::Ad has been closed for placement: " + placementId);
			if (GetRewardFailEvent != null)
			{
				GetRewardFailEvent();
			}
		}

		public void OnAdExpired(string placementId)
		{
			_adsShowing = false;
			Debug.Log("ColorTVIAds::Ad has expired for placement: " + placementId);
			if (GetRewardFailEvent != null)
			{
				GetRewardFailEvent();
			}
		}

		public void OnError(ColorTvError error)
		{
			_adsShowing = false;
			Debug.Log(string.Concat("ColorTVIAds::Ad error occured for placement: ", error.PlacementId, ", with error code: ", error.ErrorCode, " and error message: ", error.ErrorMessage));
			if (GetRewardFailEvent != null)
			{
				GetRewardFailEvent();
			}
		}

		public void OnCurrencyEarned(ColorTvCurrency coins)
		{
			Debug.Log("ColorTVIAds::User has been awarded for placement " + coins.Placement + ": " + coins.Amount + " x " + coins.Type);
			if (GetRewardSuccessEvent != null)
			{
				GetRewardSuccessEvent();
			}
		}
	}
}

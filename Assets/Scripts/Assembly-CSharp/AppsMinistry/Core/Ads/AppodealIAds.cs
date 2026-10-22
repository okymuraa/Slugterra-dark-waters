using System;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;
using AppsMinistry.Plugins;
using UnityEngine;

namespace AppsMinistry.Core.Ads
{
	public class AppodealIAds : MonoBehaviour, IAds, IRewardedVideoAdListener
	{
		private string appKey = "49dbe43ad1290b14294f679d0d0354a656af02f35eaa0c83";

		private Action GetRewardSuccessEvent;

		private Action GetRewardFailEvent;

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

		public bool CanShowAd
		{
			set
			{
			}
		}

		public bool IncentivizeAdsAvailable
		{
			get
			{
				return Appodeal.isLoaded(128);
			}
		}

		public bool VideoAdsAvailable
		{
			get
			{
				return Appodeal.isLoaded(3);
			}
		}

		public bool InterstitialAdsAvailable
		{
			get
			{
				return Appodeal.isLoaded(3);
			}
		}

		public void Initialize()
		{
			Appodeal.disableLocationPermissionCheck();
			Appodeal.disableNetwork("inmobi");
			Appodeal.disableNetwork("startapp");
			Appodeal.disableNetwork("amazon_ads");
			Appodeal.disableNetwork("flurry");
			Appodeal.disableNetwork("admob");
			Appodeal.initialize(appKey, 131);
			Appodeal.setRewardedVideoCallbacks(this);
		}

		public bool ShowIncentivizedAds()
		{
			if (IncentivizeAdsAvailable)
			{
				Appodeal.show(128);
				return true;
			}
			return false;
		}

		public void ShowAds(string screenName = "")
		{
			if (InterstitialAdsAvailable)
			{
				DonDestroyMonoSingleton<AnalyticsManager>.Instance.ShowInterstitialAds(screenName);
				Appodeal.show(3);
			}
		}

		public void onRewardedVideoLoaded()
		{
			Debug.Log("Rewarded Video loaded");
		}

		public void onRewardedVideoFailedToLoad()
		{
			Debug.Log("Rewarded Video failed to load");
		}

		public void onRewardedVideoShown()
		{
			AudioListener.volume = 0f;
		}

		public void onRewardedVideoClosed(bool b)
		{
			AudioListener.volume = 1f;
		}

		public void onRewardedVideoFinished(int amount, string name)
		{
			AudioListener.volume = 1f;
			if (GetRewardSuccessEvent != null)
			{
				GetRewardSuccessEvent();
			}
			DonDestroyMonoSingleton<AnalyticsManager>.Instance.TrackAppsFlyerAdsAttribution();
			GetRewardSuccessEvent = null;
		}
	}
}

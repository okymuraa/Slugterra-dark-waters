using System;
using UnityEngine;

namespace AppsMinistry.Core.Ads
{
	public class AdMobIAds : MonoBehaviour, IAds
	{
		private bool _isInterstitionalLoaded;

		private const string _adUnitId = "";

		public Action RewardSuccessEvent
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Action RewardFailEvent
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public bool IncentivizeAdsAvailable
		{
			get
			{
				return false;
			}
		}

		public bool CanShowAd
		{
			set
			{
			}
		}

		public bool VideoAdsAvailable
		{
			get
			{
				return false;
			}
		}

		public bool InterstitialAdsAvailable
		{
			get
			{
				return false;
			}
		}

		public void Initialize()
		{
		}

		public bool ShowIncentivizedAds()
		{
			return false;
		}

		public void ShowAds(string screenName)
		{
			if (!InterstitialAdsAvailable)
			{
				Debug.Log("AdMobIAds::ShowAds Interstitial is not ready yet.");
				Initialize();
			}
		}

		private void OnDestroy()
		{
		}
	}
}

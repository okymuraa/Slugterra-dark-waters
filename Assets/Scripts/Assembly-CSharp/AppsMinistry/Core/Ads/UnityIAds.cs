using System;
using UnityEngine;

namespace AppsMinistry.Core.Ads
{
	public class UnityIAds : MonoBehaviour, IAds
	{
		public bool CanShowAd
		{
			set
			{
			}
		}

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
		}
	}
}

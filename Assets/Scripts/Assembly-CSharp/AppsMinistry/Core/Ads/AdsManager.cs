using System;
using System.Collections.Generic;
using AppsMinistry.Utils;
using UnityEngine;

namespace AppsMinistry.Core.Ads
{
	[AppSingleton]
	public class AdsManager : DonDestroyMonoSingleton<AdsManager>
	{
		public const float adsTimeInterval = 86400f;

		public bool startLogTiming = true;

		private Dictionary<string, IAds> adsNetworks = new Dictionary<string, IAds>();

		private AppodealIAds appodealIAds;

		private ColorTVIAds colorTVIAds;

		private long _firstAdsShowTime;

		private int _currentAdsShowCounter;

		private bool _canShowAd = true;

		private int _rewardAdsLimitPerTimeInterval = 10;

		private int _respawnForAdsLimitPerLevel = 3;

		private static bool isInit;

		public int CurrentAdsShowCounter
		{
			get
			{
				return _currentAdsShowCounter;
			}
		}

		public bool CanShowAd
		{
			set
			{
				_canShowAd = value;
			}
		}

		public int RewardAdsLimitPerTimeInterval
		{
			get
			{
				return _rewardAdsLimitPerTimeInterval;
			}
		}

		public int RespawnForAdsLimitPerLevel
		{
			get
			{
				return _respawnForAdsLimitPerLevel;
			}
		}

		public Action RewardSuccessEvent
		{
			get
			{
				string text = "Appodeal";
				text = "Appodeal";
				return adsNetworks[text].RewardSuccessEvent;
			}
			set
			{
				string text = "Appodeal";
				text = "Appodeal";
				adsNetworks[text].RewardSuccessEvent = value;
			}
		}

		public Action RewardFailEvent
		{
			get
			{
				string text = "Appodeal";
				text = "Appodeal";
				return adsNetworks[text].RewardFailEvent;
			}
			set
			{
				string text = "Appodeal";
				text = "Appodeal";
				adsNetworks[text].RewardFailEvent = value;
			}
		}

		public float WaitTimeInterval
		{
			get
			{
				return 86400f;
			}
		}

		public long DeltaTime
		{
			get
			{
				return UnixTimeNow() - _firstAdsShowTime;
			}
		}

		public bool RewardIncentivizeAdsAvailable
		{
			get
			{
				if ((float)DeltaTime < 86400f && _currentAdsShowCounter >= RewardAdsLimitPerTimeInterval)
				{
					return false;
				}
				if ((float)DeltaTime > 86400f)
				{
					_currentAdsShowCounter = 0;
					PlayerPrefs.SetInt("AdsCounter", _currentAdsShowCounter);
					_firstAdsShowTime = UnixTimeNow();
					startLogTiming = false;
				}
				return adsNetworks["Appodeal"].IncentivizeAdsAvailable;
			}
		}

		public bool VideoAdsAvailable
		{
			get
			{
				return adsNetworks["Appodeal"].VideoAdsAvailable;
			}
		}

		public bool InterstitialAdsAvailable
		{
			get
			{
				return adsNetworks["Appodeal"].InterstitialAdsAvailable;
			}
		}

		protected override void OnCreate()
		{
			if (!isInit)
			{
				isInit = true;
				appodealIAds = gameObject.AddComponent<AppodealIAds>();
				colorTVIAds = gameObject.AddComponent<ColorTVIAds>();
				adsNetworks.Add("Appodeal", appodealIAds);
				adsNetworks["Appodeal"].Initialize();
				_firstAdsShowTime = UnixTimeNow();
				_currentAdsShowCounter = 0;
				if (PlayerPrefsSystem.HasKey("AdsFirstShowTime"))
				{
					string value = PlayerPrefs.GetString("AdsFirstShowTime");
					_firstAdsShowTime = Convert.ToInt64(value);
				}
				else
				{
					startLogTiming = false;
				}
				if (PlayerPrefsSystem.HasKey("AdsCounter"))
				{
					_currentAdsShowCounter = PlayerPrefs.GetInt("AdsCounter");
				}
				if ((float)DeltaTime > 86400f)
				{
					_currentAdsShowCounter = 0;
					PlayerPrefs.SetInt("AdsCounter", _currentAdsShowCounter);
					_firstAdsShowTime = UnixTimeNow();
					startLogTiming = false;
				}
			}
		}

		public bool IncentivizeAdsAvailable()
		{
			return adsNetworks["Appodeal"].IncentivizeAdsAvailable;
		}

		public void ShowIncentivizedAds(string screenName = "", bool useCounter = false)
		{
			if (!startLogTiming)
			{
				_firstAdsShowTime = UnixTimeNow();
				startLogTiming = true;
				PlayerPrefs.SetString("AdsFirstShowTime", _firstAdsShowTime.ToString());
			}
			if (adsNetworks["Appodeal"].ShowIncentivizedAds() && useCounter)
			{
				_currentAdsShowCounter++;
				PlayerPrefs.SetInt("AdsCounter", _currentAdsShowCounter);
			}
		}

		public void ShowAds(string screenName = "")
		{
			adsNetworks["Appodeal"].ShowAds(screenName);
		}

		public long UnixTimeNow()
		{
			return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
		}
	}
}

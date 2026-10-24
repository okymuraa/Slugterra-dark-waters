using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;
using UnityEngine;

namespace AppodealAds.Unity.Android
{
	public class AndroidAppodealClient : IAppodealAdsClient
	{
		private bool isShow;

		private AndroidJavaClass appodealClass;

		private AndroidJavaClass appodealUnityClass;

		private AndroidJavaClass appodealBannerClass;

		private AndroidJavaObject appodealBannerInstance;

		private AndroidJavaObject userSettings;

		private AndroidJavaObject activity;

		private static bool IsAndroidRuntime
		{
			get
			{
				return Application.platform == RuntimePlatform.Android && !Application.isEditor;
			}
		}

		public AndroidJavaClass getAppodealClass()
		{
			if (!IsAndroidRuntime)
			{
				return null;
			}
			if (appodealClass == null)
			{
				appodealClass = new AndroidJavaClass("com.appodeal.ads.Appodeal");
			}
			return appodealClass;
		}

		public AndroidJavaClass getAppodealUnityClass()
		{
			if (!IsAndroidRuntime)
			{
				return null;
			}
			if (appodealUnityClass == null)
			{
				appodealUnityClass = new AndroidJavaClass("com.appodeal.unity.AppodealUnity");
			}
			return appodealUnityClass;
		}

		public AndroidJavaObject getAppodealBannerInstance()
		{
			if (!IsAndroidRuntime)
			{
				return null;
			}
			if (appodealBannerInstance == null)
			{
				appodealBannerClass = new AndroidJavaClass("com.appodeal.unity.AppodealUnityBannerView");
				appodealBannerInstance = appodealBannerClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]);
			}
			return appodealBannerInstance;
		}

		public AndroidJavaObject getActivity()
		{
			if (!IsAndroidRuntime)
			{
				return null;
			}
			if (activity == null)
			{
				AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				activity = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			}
			return activity;
		}

		public void initialize(string appKey, int adTypes)
		{
			if (!IsAndroidRuntime) return;
			getAppodealClass().CallStatic("setFramework", "unity", Appodeal.getPluginVersion());
			if ((adTypes & 0x40) > 0)
			{
				getAppodealClass().CallStatic("setFramework", "unity", Appodeal.getPluginVersion(), false, false);
				getAppodealClass().CallStatic("disableNetwork", getActivity(), "amazon_ads", 4);
			}
			getAppodealClass().CallStatic("initialize", getActivity(), appKey, adTypes);
		}

		public bool show(int adTypes)
		{
			if (!IsAndroidRuntime) return false;
			return getAppodealUnityClass().CallStatic<bool>("show", new object[2] { getActivity(), adTypes });
		}

		public bool show(int adTypes, string placement)
		{
			if (!IsAndroidRuntime) return false;
			return getAppodealUnityClass().CallStatic<bool>("show", new object[3] { getActivity(), adTypes, placement });
		}

		public bool showBannerView(int YAxis, int XAxis, string Placement)
		{
			if (!IsAndroidRuntime) return false;
			return getAppodealBannerInstance().Call<bool>("showBannerView", new object[4] { getActivity(), XAxis, YAxis, Placement });
		}

		public bool isLoaded(int adTypes)
		{
			if (!IsAndroidRuntime) return false;
			return getAppodealClass().CallStatic<bool>("isLoaded", new object[1] { adTypes });
		}

		public void cache(int adTypes)
		{
			if (!IsAndroidRuntime) return;
			getAppodealClass().CallStatic("cache", getActivity(), adTypes);
		}

		public void hide(int adTypes)
		{
			if (!IsAndroidRuntime) return;
			getAppodealClass().CallStatic("hide", getActivity(), adTypes);
		}

		public void hideBannerView()
		{
			if (!IsAndroidRuntime) return;
			getAppodealBannerInstance().Call("hideBannerView", getActivity());
		}

		public bool isPrecache(int adTypes)
		{
			if (!IsAndroidRuntime) return false;
			return getAppodealClass().CallStatic<bool>("isPrecache", new object[1] { adTypes });
		}

		public void setAutoCache(int adTypes, bool autoCache)
		{
			if (!IsAndroidRuntime) return;
			getAppodealClass().CallStatic("setAutoCache", adTypes, autoCache);
		}

		public void onResume()
		{
			if (!IsAndroidRuntime) return;
			getAppodealClass().CallStatic("onResume", getActivity(), 4);
		}

		public void setSmartBanners(bool value)
		{
			if (!IsAndroidRuntime) return;
			getAppodealClass().CallStatic("setSmartBanners", value);
			getAppodealBannerInstance().Call("setSmartBanners", value);
		}

		public void setBannerAnimation(bool value)
		{
			if (!IsAndroidRuntime) return;
			getAppodealClass().CallStatic("setBannerAnimation", value);
		}

		public void setBannerBackground(bool value)
		{
		}

		public void setTabletBanners(bool value)
		{
			if (!IsAndroidRuntime) return;
			getAppodealClass().CallStatic("set728x90Banners", value);
		}

		public void setTesting(bool test)
		{
			if (!IsAndroidRuntime) return;
			getAppodealClass().CallStatic("setTesting", test);
		}

		public void setLogLevel(Appodeal.LogLevel logging)
		{
			if (!IsAndroidRuntime) return;
			switch (logging)
			{
			case Appodeal.LogLevel.None:
				getAppodealClass().CallStatic("setLogLevel", new AndroidJavaClass("com.appodeal.ads.utils.Log$LogLevel").GetStatic<AndroidJavaObject>("none"));
				break;
			case Appodeal.LogLevel.Debug:
				getAppodealClass().CallStatic("setLogLevel", new AndroidJavaClass("com.appodeal.ads.utils.Log$LogLevel").GetStatic<AndroidJavaObject>("debug"));
				break;
			case Appodeal.LogLevel.Verbose:
				getAppodealClass().CallStatic("setLogLevel", new AndroidJavaClass("com.appodeal.ads.utils.Log$LogLevel").GetStatic<AndroidJavaObject>("verbose"));
				break;
			}
		}

		public void setChildDirectedTreatment(bool value)
		{
			if (!IsAndroidRuntime) return;
			getAppodealClass().CallStatic("setChildDirectedTreatment", value);
		}

		public void disableNetwork(string network)
		{
			if (!IsAndroidRuntime) return;
			getAppodealClass().CallStatic("disableNetwork", getActivity(), network);
		}

		public void disableNetwork(string network, int adTypes)
		{
			if (!IsAndroidRuntime) return;
			getAppodealClass().CallStatic("disableNetwork", getActivity(), network, adTypes);
		}

		public void disableLocationPermissionCheck()
		{
			if (!IsAndroidRuntime) return;
			getAppodealClass().CallStatic("disableLocationPermissionCheck");
		}

		public void disableWriteExternalStoragePermissionCheck()
		{
			if (!IsAndroidRuntime) return;
			getAppodealClass().CallStatic("disableWriteExternalStoragePermissionCheck");
		}

		public void setTriggerOnLoadedOnPrecache(int adTypes, bool onLoadedTriggerBoth)
		{
			if (!IsAndroidRuntime) return;
			getAppodealClass().CallStatic("setTriggerOnLoadedOnPrecache", adTypes, onLoadedTriggerBoth);
		}

		public void muteVideosIfCallsMuted(bool value)
		{
			if (!IsAndroidRuntime) return;
			getAppodealClass().CallStatic("muteVideosIfCallsMuted", value);
		}

		public void showTestScreen()
		{
			if (!IsAndroidRuntime) return;
			getAppodealClass().CallStatic("startTestActivity", getActivity());
		}

		public string getVersion()
		{
			if (!IsAndroidRuntime) return "0.0.0";
			return getAppodealClass().CallStatic<string>("getVersion", new object[0]);
		}

		public bool canShow(int adTypes)
		{
			if (!IsAndroidRuntime) return false;
			return getAppodealClass().CallStatic<bool>("canShow", new object[1] { adTypes });
		}

		public bool canShow(int adTypes, string placement)
		{
			if (!IsAndroidRuntime) return false;
			return getAppodealClass().CallStatic<bool>("canShow", new object[2] { adTypes, placement });
		}

		public void setCustomRule(string name, bool value)
		{
			if (!IsAndroidRuntime) return;
			getAppodealClass().CallStatic("setCustomRule", name, value);
		}

		public void setCustomRule(string name, int value)
		{
			if (!IsAndroidRuntime) return;
			getAppodealClass().CallStatic("setCustomRule", name, value);
		}

		public void setCustomRule(string name, double value)
		{
			if (!IsAndroidRuntime) return;
			getAppodealClass().CallStatic("setCustomRule", name, value);
		}

		public void setCustomRule(string name, string value)
		{
			if (!IsAndroidRuntime) return;
			getAppodealClass().CallStatic("setCustomRule", name, value);
		}

		public void trackInAppPurchase(double amount, string currency)
		{
			if (!IsAndroidRuntime) return;
			getAppodealClass().CallStatic("trackInAppPurchase", getActivity(), amount, currency);
		}

		public string getRewardCurrency(string placement)
		{
			if (!IsAndroidRuntime) return string.Empty;
			AndroidJavaObject androidJavaObject = getAppodealClass().CallStatic<AndroidJavaObject>("getRewardParameters", new object[1] { placement });
			return androidJavaObject.Get<string>("second");
		}

		public int getRewardAmount(string placement)
		{
			if (!IsAndroidRuntime) return 0;
			AndroidJavaObject androidJavaObject = getAppodealClass().CallStatic<AndroidJavaObject>("getRewardParameters", new object[1] { placement });
			AndroidJavaObject androidJavaObject2 = androidJavaObject.Get<AndroidJavaObject>("first");
			return androidJavaObject2.Call<int>("intValue", new object[0]);
		}

		public string getRewardCurrency()
		{
			if (!IsAndroidRuntime) return string.Empty;
			AndroidJavaObject androidJavaObject = getAppodealClass().CallStatic<AndroidJavaObject>("getRewardParameters", new object[0]);
			return androidJavaObject.Get<string>("second");
		}

		public int getRewardAmount()
		{
			if (!IsAndroidRuntime) return 0;
			AndroidJavaObject androidJavaObject = getAppodealClass().CallStatic<AndroidJavaObject>("getRewardParameters", new object[0]);
			AndroidJavaObject androidJavaObject2 = androidJavaObject.Get<AndroidJavaObject>("first");
			return androidJavaObject2.Call<int>("intValue", new object[0]);
		}

		public void getUserSettings()
		{
			if (!IsAndroidRuntime) return;
			userSettings = getAppodealClass().CallStatic<AndroidJavaObject>("getUserSettings", new object[1] { getActivity() });
		}

		public void setUserId(string id)
		{
			if (!IsAndroidRuntime || userSettings == null) return;
			userSettings.Call<AndroidJavaObject>("setUserId", new object[1] { id });
		}

		public void setAge(int age)
		{
			if (!IsAndroidRuntime || userSettings == null) return;
			userSettings.Call<AndroidJavaObject>("setAge", new object[1] { age });
		}

		public void setGender(UserSettings.Gender gender)
		{
			if (!IsAndroidRuntime || userSettings == null) return;
			switch (gender)
			{
			case UserSettings.Gender.OTHER:
				userSettings.Call<AndroidJavaObject>("setGender", new object[1] { new AndroidJavaClass("com.appodeal.ads.UserSettings$Gender").GetStatic<AndroidJavaObject>("OTHER") });
				break;
			case UserSettings.Gender.MALE:
				userSettings.Call<AndroidJavaObject>("setGender", new object[1] { new AndroidJavaClass("com.appodeal.ads.UserSettings$Gender").GetStatic<AndroidJavaObject>("MALE") });
				break;
			case UserSettings.Gender.FEMALE:
				userSettings.Call<AndroidJavaObject>("setGender", new object[1] { new AndroidJavaClass("com.appodeal.ads.UserSettings$Gender").GetStatic<AndroidJavaObject>("FEMALE") });
				break;
			}
		}

		public void setInterstitialCallbacks(IInterstitialAdListener listener)
		{
			if (!IsAndroidRuntime) return;
			getAppodealClass().CallStatic("setInterstitialCallbacks", new AppodealInterstitialCallbacks(listener));
		}

		public void setNonSkippableVideoCallbacks(INonSkippableVideoAdListener listener)
		{
			if (!IsAndroidRuntime) return;
			getAppodealClass().CallStatic("setNonSkippableVideoCallbacks", new AppodealNonSkippableVideoCallbacks(listener));
		}

		public void setRewardedVideoCallbacks(IRewardedVideoAdListener listener)
		{
			if (!IsAndroidRuntime) return;
			getAppodealClass().CallStatic("setRewardedVideoCallbacks", new AppodealRewardedVideoCallbacks(listener));
		}

		public void setBannerCallbacks(IBannerAdListener listener)
		{
			if (!IsAndroidRuntime) return;
			getAppodealClass().CallStatic("setBannerCallbacks", new AppodealBannerCallbacks(listener));
		}

		public void requestAndroidMPermissions(IPermissionGrantedListener listener)
		{
			if (!IsAndroidRuntime) return;
			getAppodealClass().CallStatic("requestAndroidMPermissions", getActivity(), new AppodealPermissionCallbacks(listener));
		}
	}
}

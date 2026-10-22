using UnityEngine;

namespace ColorTvPlugin
{
	public class ColorTvGameObjectBehaviour : MonoBehaviour
	{
		private AndroidJavaClass ColorTvSDKClass;

		private AndroidJavaObject ColorTvWrapperObject;

		private AndroidJavaObject Activity;

		private void Start()
		{
			if (Object.FindObjectsOfType<ColorTvGameObjectBehaviour>().Length > 1)
			{
				Object.Destroy(gameObject);
			}
			Object.DontDestroyOnLoad(gameObject);
		}

		private void OnApplicationQuit()
		{
			ColorTvSDKClass.CallStatic("onDestroy");
			ColorTvSDKClass.CallStatic("clearOnCurrencyEarnedListeners");
		}

		internal void InitPlugin()
		{
			ColorTvSDKClass = new AndroidJavaClass("com.colortv.android.ColorTvSdk");
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			Activity = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			ColorTvWrapperObject = new AndroidJavaObject("com.colortv.android.unity.wrapper.ColorTvSDKWrapper");
			ColorTvWrapperObject.Call("initWrapper", Activity);
		}

		internal void InitSDK(string appId)
		{
			Activity.Call("runOnUiThread", (AndroidJavaRunnable)(() =>
			{
				ColorTvSDKClass.CallStatic("init", Activity, appId);
				ColorTvWrapperObject.Call("registerColorTvAdListener");
				ColorTvWrapperObject.Call("registerOnCurrencyEarnedListener");
				ColorTvSDKClass.CallStatic("onCreate");
			}));
		}

		internal void LoadAd(string placementId)
		{
			Activity.Call("runOnUiThread", (AndroidJavaRunnable)(() =>
			{
				ColorTvSDKClass.CallStatic("loadAd", placementId);
			}));
		}

		internal void ShowAd(string placementId)
		{
			Activity.Call("runOnUiThread", (AndroidJavaRunnable)(() =>
			{
				ColorTvSDKClass.CallStatic("showAd", placementId);
			}));
		}

		internal void SetDebugMode(bool enabled)
		{
			ColorTvSDKClass.CallStatic("setDebugMode", enabled);
		}

		internal void SetUserId(string userId)
		{
			ColorTvSDKClass.CallStatic("setUserId", userId);
		}

		internal void SetUserAge(int age)
		{
			ColorTvWrapperObject.Call("setUserAge", age);
		}

		internal void SetUserGender(ColorTv.Gender gender)
		{
			ColorTvWrapperObject.Call("setUserGender", gender.ToString());
		}

		internal void SetUserKeywords(string keywords)
		{
			ColorTvWrapperObject.Call("setUserKeywords", keywords);
		}

		internal void SetRecordAudioEnabled(bool enabled)
		{
			ColorTvSDKClass.CallStatic("setRecordAudioEnabled", enabled);
		}

		private void OnCurrencyEarned(string result)
		{
			ColorTvCurrency colorTvCurrency = ColorTvCurrency.FromString(result);
			if (colorTvCurrency == null)
			{
				Debug.LogWarning("Something went wrong with currency earned - no coins awarded");
			}
			else if (ColorTvCallbacks.CurrencyEarned != null)
			{
				ColorTvCallbacks.CurrencyEarned(colorTvCurrency);
			}
			else
			{
				Debug.LogWarning("Coins have been awarded, but no currency earned listener was registered");
			}
		}

		private void OnAdLoaded(string placementId)
		{
			if (ColorTvCallbacks.AdLoaded != null)
			{
				ColorTvCallbacks.AdLoaded(placementId);
			}
		}

		private void OnAdClosed(string placementId)
		{
			if (ColorTvCallbacks.AdClosed != null)
			{
				ColorTvCallbacks.AdClosed(placementId);
			}
		}

		private void OnAdExpired(string placementId)
		{
			if (ColorTvCallbacks.AdExpired != null)
			{
				ColorTvCallbacks.AdExpired(placementId);
			}
		}

		private void OnError(string errorString)
		{
			ColorTvError obj = ColorTvError.FromString(errorString);
			if (ColorTvCallbacks.AdError != null)
			{
				ColorTvCallbacks.AdError(obj);
			}
		}
	}
}

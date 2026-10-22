using UnityEngine;

public class AppsMinistrySdk_Dummy : IAppsMinistrySdk
{
	private AndroidJavaObject mCurrentActivity;

	private AndroidJavaClass mSDKClass;

	private AndroidJavaObject mIntent;

	private bool isSdkLoggingEnabled;

	public AppsMinistrySdk_Dummy()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			mSDKClass = new AndroidJavaClass("com.appsministry.sdk.general.AppsMinistrySdk");
			mCurrentActivity = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			if (mCurrentActivity == null)
			{
				Debug.Log("AppsMinistrySdk_Android  mCurrentActivity is null");
			}
			mIntent = mCurrentActivity.Call<AndroidJavaObject>("getIntent", new object[0]);
			if (mIntent == null)
			{
				Debug.Log("AppsMinistrySdk_Android  mIntent is null");
			}
		}
	}

	public void AppsMinistrySdkConfigure(int appId, string key)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
		}
	}

	public string AppsMinistrySDKCurrentSystemLanguage()
	{
		string result = "en";
		string text = "us";
		if (Application.platform != RuntimePlatform.Android)
		{
			return result;
		}
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("java/util/Locale");
		AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("getDefault", new object[0]);
		result = androidJavaObject.Call<string>("getLanguage", new object[0]);
		text = androidJavaObject.Call<string>("getCountry", new object[0]);
		return result + "-" + text;
	}

	public string AppsMinistrySDKVersion()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return "null";
		}
		return mSDKClass.CallStatic<string>("getSdkVersion", new object[0]);
	}

	public bool AMIsOnline()
	{
		if (Application.platform == RuntimePlatform.Android && Application.internetReachability == NetworkReachability.NotReachable)
		{
			return false;
		}
		return true;
	}

	public void AMStartNetworkStateNotification()
	{
	}

	public void AMStopNetworkStateNotification()
	{
	}

	public void AMLoggerEnableSDKLogging()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			mSDKClass.CallStatic("enableSDKLogging");
			isSdkLoggingEnabled = true;
		}
	}

	public void AMLoggerDisableSDKLogging()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			mSDKClass.CallStatic("disableSDKLogging");
			isSdkLoggingEnabled = false;
		}
	}

	public bool AMLoggerIsSDKLoggingEnabled()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return isSdkLoggingEnabled;
		}
		return isSdkLoggingEnabled;
	}

	public void AMLoggerPresentLogViewController()
	{
	}

	public void AMCrashApp()
	{
	}

	public bool AMStoreKitValidatorValidate(string productId, string transactionId, string base64encodedReceipt)
	{
		return true;
	}

	public void AMEnableApplicationNotification()
	{
	}

	public void AMDisableApplicationNotification()
	{
	}

	public int AMStoreKitSubscriptionExpirationDate(string productId, string base64encodedReceipt)
	{
		return 0;
	}

	public string AMPushServiceToken()
	{
		return string.Empty;
	}

	public void AMPushServiceStartSession(int appId, string key, string androidSenderId)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
		}
	}

	public void AMPushServiceStartSessionWithOptions(bool manualMode, bool testMode, string language, string androidSenderId)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
		}
	}

	public void AMPushServiceStartSessionWithOptions(int appId, string key, bool manualMode, bool testMode, string language, string androidSenderId)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
		}
	}

	public void AMPushServiceUpdateToken()
	{
	}

	public void AMPushServiceUpdateCurrentLanguage(string language)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
		}
	}

	public void AMPushUpdate()
	{
	}

	public void AMRichMediaStartSession(int appId, string key)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
		}
	}

	public void AMRichMediaStartSessionWithOptions(bool manualMode, bool testMode, string language, string orienation)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
		}
	}

	public void AMRichmediaUpdateCurrentLanguage(string language)
	{
	}

	public void AMRichMediaShow(string ScreenName)
	{
	}

	public bool AMRichMediaIsShowing()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return false;
		}
		return false;
	}
}

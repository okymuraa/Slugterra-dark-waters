using System;
using UnityEngine;

public class AppsMinistrySdk_Android : IAppsMinistrySdk
{
	private class RichMediaCallback : AndroidJavaProxy
	{
		public RichMediaCallback()
			: base("com.appsministry.sdk.richmedia.AMRichMedia$AMRichMediaCompletionListener")
		{
		}

		public void completion(bool shown, bool clicked)
		{
			if (richMediaServiceListener != null)
			{
				richMediaServiceListener.didShow((!shown) ? "error" : string.Empty);
				richMediaServiceListener.didClose((!clicked) ? string.Empty : "clicked");
			}
		}

		public void showing()
		{
			richMediaServiceListener.willShow(string.Empty);
		}
	}

	private AndroidJavaObject mCurrentActivity;

	private AndroidJavaClass mRichmediaClass;

	private AndroidJavaClass mPushServiceClass;

	private AndroidJavaClass mSDKClass;

	private AndroidJavaObject mPushServiceActivity;

	private AndroidJavaObject mRichmedia;

	private AndroidJavaObject mIntent;

	private static RichMediaService richMediaServiceListener;

	private static PushService pushServiceListener;

	private bool isSdkLoggingEnabled;

	public AppsMinistrySdk_Android()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			mSDKClass = new AndroidJavaClass("com.appsministry.sdk.general.AppsMinistrySdk");
			mRichmediaClass = new AndroidJavaClass("com.appsministry.sdk.richmedia.AMRichMedia");
			mPushServiceClass = new AndroidJavaClass("com.appsministry.sdk.push.PushService");
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
			richMediaServiceListener = UnityEngine.Object.FindObjectOfType<RichMediaService>();
			pushServiceListener = UnityEngine.Object.FindObjectOfType<PushService>();
		}
	}

	public void AppsMinistrySdkConfigure(int appId, string key)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			mSDKClass.CallStatic("setContext", mCurrentActivity);
			mSDKClass.CallStatic("configure", appId.ToString(), key);
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
		if (Application.platform != RuntimePlatform.Android)
		{
			return string.Empty;
		}
		return string.Empty;
	}

	public void AMPushServiceStartSession(int appId, string key, string androidSenderId)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			mPushServiceActivity = mPushServiceClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]);
			mPushServiceActivity.Call("startSession", appId.ToString(), key, androidSenderId);
		}
	}

	public void AMPushServiceStartSessionWithOptions(bool manualMode, bool testMode, string language, string androidSenderId)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			mPushServiceActivity = mPushServiceClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]);
			mPushServiceActivity.Call("startSessionWithSenderId", androidSenderId);
		}
	}

	public void AMPushServiceStartSessionWithOptions(int appId, string key, bool manualMode, bool testMode, string language, string androidSenderId)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			mPushServiceActivity = mPushServiceClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]);
			mPushServiceActivity.Call("startSession", appId.ToString(), key, androidSenderId);
		}
	}

	public void AMPushServiceUpdateToken()
	{
	}

	public void AMPushServiceUpdateCurrentLanguage(string language)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			mPushServiceActivity.Call("setCurrentLanguage", language);
		}
	}

	public void AMPushUpdate()
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		if (mCurrentActivity == null)
		{
			mCurrentActivity = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
		}
		if (mIntent == null)
		{
			mIntent = mCurrentActivity.Call<AndroidJavaObject>("getIntent", new object[0]);
		}
		if (mCurrentActivity != null && mIntent != null)
		{
			string text = mRichmedia.GetStatic<string>("kAMRichMediaPushAction");
			string text2 = mRichmedia.GetStatic<string>("kAMRichMediaPushMessage");
			bool flag = mIntent.Call<bool>("getBooleanExtra", new object[2] { text, false });
			if (flag)
			{
				string arg = mIntent.Call<string>("getStringExtra", new object[1] { text2 });
				string format = "\"waking\":\"{0}\", \"message\":\"{1}\"";
				format = string.Format(format, flag.ToString(), arg);
				format = "{" + format + "}";
				pushServiceListener.didReceiveRemoteNotification(format);
			}
		}
	}

	public void AMRichMediaStartSession(int appId, string key)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			mRichmedia = mRichmediaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]);
			mRichmedia.Call("startSession");
		}
	}

	public void AMRichMediaStartSessionWithOptions(bool manualMode, bool testMode, string language, string orienation)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		mRichmedia = mRichmediaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]);
		string text = mRichmedia.GetStatic<string>("kAMRichMediaOptionOrientationKey");
		string text2 = "All";
		if (orienation == string.Empty || orienation == null)
		{
			text2 = mRichmedia.GetStatic<string>("kAMRichMediaOptionOrientationAll");
		}
		if (orienation.ToLower() == "landscape")
		{
			text2 = mRichmedia.GetStatic<string>("kAMRichMediaOptionOrientationLandscape");
		}
		if (orienation.ToLower() == "portrait")
		{
			text2 = mRichmedia.GetStatic<string>("kAMRichMediaOptionOrientationPortrait");
		}
		Debug.Log("AppsMinistrySdk_Android Initialize SDK with Options :" + text + " : " + text2);
		AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.util.HashMap");
		IntPtr methodID = AndroidJNIHelper.GetMethodID(androidJavaObject.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
		object[] array = new object[2];
		using (AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("java.lang.String", text))
		{
			using (AndroidJavaObject androidJavaObject3 = new AndroidJavaObject("java.lang.String", text2))
			{
				array[0] = androidJavaObject2;
				array[1] = androidJavaObject3;
				AndroidJNI.CallObjectMethod(androidJavaObject.GetRawObject(), methodID, AndroidJNIHelper.CreateJNIArgArray(array));
			}
		}
		mRichmedia.Call("startSessionWithOptions", androidJavaObject);
	}

	public void AMRichmediaUpdateCurrentLanguage(string language)
	{
	}

	public void AMRichMediaShow(string ScreenName)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		mCurrentActivity.Call("runOnUiThread", (AndroidJavaRunnable)(() =>
		{
			if (mRichmedia == null)
			{
				mRichmedia = mRichmediaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]);
			}
			mRichmedia.Call("show", new RichMediaCallback());
		}));
	}

	public bool AMRichMediaIsShowing()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return false;
		}
		return mRichmedia.Call<bool>("isShowing", new object[0]);
	}
}

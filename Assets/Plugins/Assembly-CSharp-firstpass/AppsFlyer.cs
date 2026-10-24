using System;
using System.Collections.Generic;
using UnityEngine;

public class AppsFlyer : MonoBehaviour
{
	private static AndroidJavaClass obj;

	private static AndroidJavaObject cls_AppsFlyer;

	private static AndroidJavaClass propertiesClass;

	private static AndroidJavaObject afPropertiesInstance;

	private static AndroidJavaClass cls_AppsFlyerHelper;

	private static string devKey;

	private static bool IsAndroidRuntime
	{
		get
		{
			return Application.platform == RuntimePlatform.Android && !Application.isEditor;
		}
	}

	public static void trackEvent(string eventName, string eventValue)
	{
	}

	public static void setCurrencyCode(string currencyCode)
	{
		if (IsAndroidRuntime && cls_AppsFlyer != null)
		{
			cls_AppsFlyer.Call("setCurrencyCode", currencyCode);
		}
	}

	public static void setCustomerUserID(string customerUserID)
	{
		if (IsAndroidRuntime && cls_AppsFlyer != null)
		{
			cls_AppsFlyer.Call("setAppUserId", customerUserID);
		}
	}

	public static void loadConversionData(string callbackObject)
	{
		if (!IsAndroidRuntime || cls_AppsFlyerHelper == null)
		{
			return;
		}
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				cls_AppsFlyerHelper.CallStatic("createConversionDataListener", androidJavaObject, callbackObject);
			}
		}
	}

	[Obsolete("Use loadConversionData(string callbackObject)")]
	public static void loadConversionData(string callbackObject, string callbackMethod, string callbackFailedMethod)
	{
		loadConversionData(callbackObject);
	}

	public static void setCollectIMEI(bool shouldCollect)
	{
		if (IsAndroidRuntime && cls_AppsFlyer != null)
		{
			cls_AppsFlyer.Call("setCollectIMEI", shouldCollect);
		}
	}

	public static void setCollectAndroidID(bool shouldCollect)
	{
		if (IsAndroidRuntime && cls_AppsFlyer != null)
		{
			cls_AppsFlyer.Call("setCollectAndroidID", shouldCollect);
		}
	}

	public static void init(string key)
	{
		devKey = key;
		if (!IsAndroidRuntime)
		{
			return;
		}
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				androidJavaObject.Call("runOnUiThread", new AndroidJavaRunnable(init_cb));
			}
		}
	}

	private static void init_cb()
	{
		if (!IsAndroidRuntime || cls_AppsFlyer == null)
		{
			return;
		}
		trackAppLaunch();
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("getApplication", new object[0]);
				cls_AppsFlyer.Call("startTracking", androidJavaObject2, devKey);
			}
		}
	}

	public static void setAppsFlyerKey(string key)
	{
	}

	public static void trackAppLaunch()
	{
		if (!IsAndroidRuntime || cls_AppsFlyer == null)
		{
			return;
		}
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				cls_AppsFlyer.Call("trackAppLaunch", androidJavaObject, devKey);
			}
		}
	}

	public static void setAppID(string packageName)
	{
		if (IsAndroidRuntime && cls_AppsFlyer != null)
		{
			cls_AppsFlyer.Call("setAppId", packageName);
		}
	}

	public static void createValidateInAppListener(string aObject, string callbackMethod, string callbackFailedMethod)
	{
		if (!IsAndroidRuntime || cls_AppsFlyerHelper == null)
		{
			return;
		}
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				cls_AppsFlyerHelper.CallStatic("createValidateInAppListener", androidJavaObject, aObject, callbackMethod, callbackFailedMethod);
			}
		}
	}

	public static void validateReceipt(string publicKey, string purchaseData, string signature, string price, string currency, Dictionary<string, string> extraParams)
	{
		if (!IsAndroidRuntime || cls_AppsFlyer == null)
		{
			return;
		}
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				AndroidJavaObject androidJavaObject2 = null;
				if (extraParams != null)
				{
					androidJavaObject2 = ConvertHashMap(extraParams);
				}
				cls_AppsFlyer.Call("validateAndTrackInAppPurchase", androidJavaObject, publicKey, signature, purchaseData, price, currency, androidJavaObject2);
			}
		}
	}

	public static void trackRichEvent(string eventName, Dictionary<string, string> eventValues)
	{
		if (!IsAndroidRuntime || cls_AppsFlyer == null)
		{
			return;
		}
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				AndroidJavaObject androidJavaObject2 = ConvertHashMap(eventValues);
				cls_AppsFlyer.Call("trackEvent", androidJavaObject, eventName, androidJavaObject2);
			}
		}
	}

	private static AndroidJavaObject ConvertHashMap(Dictionary<string, string> dict)
	{
		AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.util.HashMap");
		IntPtr methodID = AndroidJNIHelper.GetMethodID(androidJavaObject.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
		object[] array = new object[2];
		foreach (KeyValuePair<string, string> item in dict)
		{
			using (AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("java.lang.String", item.Key))
			{
				using (AndroidJavaObject androidJavaObject3 = new AndroidJavaObject("java.lang.String", item.Value))
				{
					array[0] = androidJavaObject2;
					array[1] = androidJavaObject3;
					AndroidJNI.CallObjectMethod(androidJavaObject.GetRawObject(), methodID, AndroidJNIHelper.CreateJNIArgArray(array));
				}
			}
		}
		return androidJavaObject;
	}

	public static void setImeiData(string imeiData)
	{
		if (IsAndroidRuntime && cls_AppsFlyer != null)
		{
			cls_AppsFlyer.Call("setImeiData", imeiData);
		}
	}

	public static void setAndroidIdData(string androidIdData)
	{
		if (IsAndroidRuntime && cls_AppsFlyer != null)
		{
			cls_AppsFlyer.Call("setAndroidIdData", androidIdData);
		}
	}

	public static void setIsDebug(bool isDebug)
	{
		if (IsAndroidRuntime && cls_AppsFlyer != null)
		{
			cls_AppsFlyer.Call("setDebugLog", isDebug);
		}
	}

	public static void setIsSandbox(bool isSandbox)
	{
	}

	public static void getConversionData()
	{
	}

	public static void handleOpenUrl(string url, string sourceApplication, string annotation)
	{
	}

	public static string getAppsFlyerId()
	{
		if (!IsAndroidRuntime || cls_AppsFlyer == null)
		{
			return string.Empty;
		}
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				return cls_AppsFlyer.Call<string>("getAppsFlyerUID", new object[1] { androidJavaObject });
			}
		}
	}

	public static void setGCMProjectNumber(string googleGCMNumber)
	{
		if (IsAndroidRuntime && cls_AppsFlyer != null)
		{
			cls_AppsFlyer.Call("setGCMProjectNumber", googleGCMNumber);
		}
	}

	public static void updateServerUninstallToken(string token)
	{
		if (!IsAndroidRuntime)
		{
			return;
		}
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.appsflyer.AppsFlyerLib");
		AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]);
		using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject androidJavaObject2 = androidJavaClass2.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				androidJavaObject.Call("updateServerUninstallToken", androidJavaObject2, token);
			}
		}
	}

	public static void enableUninstallTracking(string senderId)
	{
		if (!IsAndroidRuntime)
		{
			return;
		}
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.appsflyer.AppsFlyerLib");
		AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]);
		androidJavaObject.Call("updateServerUninstallToken", senderId);
	}
}

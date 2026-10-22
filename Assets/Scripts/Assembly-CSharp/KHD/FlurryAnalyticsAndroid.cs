using System;
using System.Collections.Generic;
using UnityEngine;

namespace KHD
{
	public static class FlurryAnalyticsAndroid
	{
		private const string FLURRY_ANGENT_CLASS_NAME = "com.flurry.android.FlurryAgent";

		private const string UNITY_PLAYER_CLASS_NAME = "com.unity3d.player.UnityPlayer";

		private const string UNITY_PLAYER_ACTIVITY_NAME = "currentActivity";

		private static AndroidJavaClass _flurry;

		private static AndroidJavaClass Flurry
		{
			get
			{
				if (!IsAndroidPlayer())
				{
					return null;
				}
				if (_flurry == null)
				{
					_flurry = new AndroidJavaClass("com.flurry.android.FlurryAgent");
				}
				return _flurry;
			}
		}

		public static void Dispose()
		{
			if (_flurry != null)
			{
				_flurry.Dispose();
				_flurry = null;
			}
		}

		public static void Init(string apiKey, bool captureUncaughtExceptions = false)
		{
			DebugLog("Init with key: " + apiKey + " capture exceptions: " + captureUncaughtExceptions);
			if (!IsAndroidPlayer())
			{
				return;
			}
			if (apiKey != null)
			{
				apiKey = apiKey.ToUpper();
			}
			if (string.IsNullOrEmpty(apiKey))
			{
				DebugLogError("Android API key is null or empty, please provide valid API key");
			}
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					Flurry.CallStatic("setCaptureUncaughtExceptions", captureUncaughtExceptions);
					Flurry.CallStatic("init", androidJavaObject, apiKey.ToUpper());
				}
			}
		}

		public static void OnStartSession()
		{
			DebugLog("Session started");
			if (!IsAndroidPlayer())
			{
				return;
			}
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					Flurry.CallStatic("onStartSession", androidJavaObject);
				}
			}
		}

		public static void OnEndSession()
		{
			DebugLog("Session ended");
			if (!IsAndroidPlayer())
			{
				return;
			}
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					Flurry.CallStatic("onStartSession", androidJavaObject);
				}
			}
		}

		public static void SetLogEnabled(bool enabled)
		{
			if (IsAndroidPlayer())
			{
				Flurry.CallStatic("setLogEnabled", enabled);
			}
		}

		public static void SetVersionName(string versionName)
		{
			DebugLog("Application version changed to: " + versionName);
			if (IsAndroidPlayer())
			{
				Flurry.CallStatic("setVersionName", versionName);
			}
		}

		public static void SetReportLocation(bool report)
		{
			DebugLog("Set report location: " + report);
			if (IsAndroidPlayer())
			{
				Flurry.CallStatic("setReportLocation", report);
			}
		}

		public static void SetContinueSessionMillis(long milliseconds)
		{
			DebugLog("Set continue session seconds to: " + milliseconds / 1000);
			if (IsAndroidPlayer())
			{
				Flurry.CallStatic("setContinueSessionMillis", milliseconds);
			}
		}

		public static void SetCaptureUncaughtExceptions(bool enabled)
		{
			DebugLog("Set capture unchaught exceptions: " + enabled);
			if (IsAndroidPlayer())
			{
				Flurry.CallStatic("setCaptureUncaughtExceptions", enabled);
			}
		}

		public static void SetPulseEnabled(bool enabled)
		{
			if (IsAndroidPlayer())
			{
				Flurry.CallStatic("setPulseEnabled", enabled);
			}
		}

		public static void SetUserId(string userId)
		{
			DebugLog("Set unique user id: " + userId);
			if (IsAndroidPlayer())
			{
				Flurry.CallStatic("setUserId", userId);
			}
		}

		public static void SetGender(FlurryAnalytics.Gender gender)
		{
			DebugLog("Set user gender: " + gender);
			if (IsAndroidPlayer())
			{
				Flurry.CallStatic("setGender", (gender == FlurryAnalytics.Gender.Male) ? ((byte)1) : ((byte)0));
			}
		}

		public static void SetAge(int age)
		{
			DebugLog("Set user age: " + age);
			if (IsAndroidPlayer())
			{
				Flurry.CallStatic("setAge", age);
			}
		}

		public static void SetLocation(float latitude, float longitude)
		{
			DebugLog("Set location: " + latitude + ", " + longitude);
			if (IsAndroidPlayer())
			{
				Flurry.CallStatic("setLocation", latitude, longitude);
			}
		}

		public static void LogEvent(string eventName, bool isTimed = false)
		{
			DebugLog("Log event: " + eventName + " isTimed: " + isTimed);
			if (IsAndroidPlayer())
			{
				Flurry.CallStatic<AndroidJavaObject>("logEvent", new object[2] { eventName, isTimed });
			}
		}

		public static void LogEventWithParameters(string eventName, Dictionary<string, string> parameters, bool isTimed = false)
		{
			DebugLog("Log event: " + eventName + " isTimed: " + isTimed + " with parameters");
			if (!IsAndroidPlayer())
			{
				return;
			}
			using (AndroidJavaObject androidJavaObject = ConvertDictionaryToJavaHashMap(parameters))
			{
				Flurry.CallStatic<AndroidJavaObject>("logEvent", new object[3] { eventName, androidJavaObject, isTimed });
			}
		}

		public static void EndTimedEvent(string eventName, Dictionary<string, string> parameters = null)
		{
			DebugLog("End timed event: " + eventName);
			if (!IsAndroidPlayer())
			{
				return;
			}
			if (parameters == null)
			{
				Flurry.CallStatic("endTimedEvent", eventName);
				return;
			}
			using (AndroidJavaObject androidJavaObject = ConvertDictionaryToJavaHashMap(parameters))
			{
				Flurry.CallStatic("endTimedEvent", eventName, androidJavaObject);
			}
		}

		public static void LogPayment(string productName, string productId, int quantity, double price, string currency, string transactionId, Dictionary<string, string> parameters)
		{
			DebugLog("Log payment: " + productName);
			if (parameters == null)
			{
				parameters = new Dictionary<string, string>();
			}
			using (AndroidJavaObject androidJavaObject = ConvertDictionaryToJavaHashMap(parameters))
			{
				Flurry.CallStatic<AndroidJavaObject>("logPayment", new object[7] { productName, productId, quantity, price, currency, transactionId, androidJavaObject });
			}
		}

		private static AndroidJavaObject ConvertDictionaryToJavaHashMap(Dictionary<string, string> parameters)
		{
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.util.HashMap");
			IntPtr methodID = AndroidJNIHelper.GetMethodID(androidJavaObject.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
			foreach (KeyValuePair<string, string> parameter in parameters)
			{
				using (AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("java.lang.String", parameter.Key))
				{
					using (AndroidJavaObject androidJavaObject3 = new AndroidJavaObject("java.lang.String", parameter.Value))
					{
						AndroidJNI.CallObjectMethod(androidJavaObject.GetRawObject(), methodID, AndroidJNIHelper.CreateJNIArgArray(new object[2] { androidJavaObject2, androidJavaObject3 }));
					}
				}
			}
			return androidJavaObject;
		}

		private static bool EditorDebugLogEnabled()
		{
			return false;
		}

		private static bool IsAndroidPlayer()
		{
			return Application.platform == RuntimePlatform.Android;
		}

		private static void DebugLog(string log)
		{
			if (EditorDebugLogEnabled())
			{
				Debug.Log("[FlurryAnalyticsPlugin]: " + log);
			}
		}

		private static void DebugLogError(string error)
		{
			Debug.Log("[FlurryAnalyticsPlugin]: " + error);
		}
	}
}

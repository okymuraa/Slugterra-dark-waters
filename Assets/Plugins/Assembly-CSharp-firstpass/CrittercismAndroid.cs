using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using UnityEngine;

public static class CrittercismAndroid
{
	private static bool isInitialized;

	private static readonly string CRITTERCISM_CLASS = "com.crittercism.app.Crittercism";

	private static AndroidJavaClass mCrittercismsPlugin;

	private static volatile bool logUnhandledExceptionAsCrash;

	public static void Init(string appID)
	{
		CrittercismConfig crittercismConfig = new CrittercismConfig();
		crittercismConfig.SetLogcatReportingEnabled(true);
		crittercismConfig.SetCustomVersionName("2.0.5");
		Init(appID, crittercismConfig);
	}

	public static void Init(string appID, CrittercismConfig config)
	{
		if (isInitialized)
		{
			Debug.Log("CrittercismAndroid is already initialized.");
			return;
		}
		Debug.Log("Initializing Crittercism with app id " + appID);
		mCrittercismsPlugin = new AndroidJavaClass(CRITTERCISM_CLASS);
		if (mCrittercismsPlugin == null)
		{
			Debug.Log("CrittercismAndroid failed to initialize.  Unable to find class " + CRITTERCISM_CLASS);
			return;
		}
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				PluginCallStatic("initialize", androidJavaObject, appID, config.GetAndroidConfig());
			}
		}
		Application.logMessageReceived += OnLogMessageReceived;
		isInitialized = true;
	}

	private static string StackTrace(Exception e)
	{
		string stackTrace = e.StackTrace;
		List<Exception> list = new List<Exception>();
		list.Add(e);
		if (stackTrace != null)
		{
			stackTrace = e.GetType().FullName + " : " + e.Message + "\r\n" + stackTrace;
			Exception innerException = e.InnerException;
			while (innerException != null && list.IndexOf(innerException) < 0)
			{
				list.Add(innerException);
				stackTrace = innerException.GetType().FullName + " : " + innerException.Message + "\r\n" + innerException.StackTrace + "\r\n" + stackTrace;
				innerException = innerException.InnerException;
			}
		}
		else
		{
			stackTrace = string.Empty;
		}
		return stackTrace;
	}

	public static void LogHandledException(Exception e)
	{
		string fullName = e.GetType().FullName;
		string message = e.Message;
		string text = StackTrace(e);
		PluginCallStatic("_logHandledException", fullName, message, text);
	}

	private static void LogUnhandledException(Exception e)
	{
		string fullName = e.GetType().FullName;
		string message = e.Message;
		string text = StackTrace(e);
		PluginCallStatic((!logUnhandledExceptionAsCrash) ? "_logHandledException" : "_logCrashException", fullName, message, text);
	}

	public static void LogNetworkRequest(string method, string uriString, long latency, long bytesRead, long bytesSent, HttpStatusCode responseCode, WebExceptionStatus exceptionStatus)
	{
		if (isInitialized)
		{
			PluginCallStatic("logNetworkRequest", method, uriString, latency, bytesRead, bytesSent, (int)responseCode, (int)exceptionStatus);
		}
	}

	public static bool GetOptOut()
	{
		if (!isInitialized)
		{
			return false;
		}
		return PluginCallStatic<bool>("getOptOutStatus", new object[0]);
	}

	public static void SetOptOut(bool optOutStatus)
	{
		if (isInitialized)
		{
			PluginCallStatic("setOptOutStatus", optOutStatus);
		}
	}

	public static bool DidCrashOnLastLoad()
	{
		if (!isInitialized)
		{
			return false;
		}
		return PluginCallStatic<bool>("didCrashOnLastLoad", new object[0]);
	}

	public static void SetUsername(string username)
	{
		if (isInitialized)
		{
			PluginCallStatic("setUsername", username);
		}
	}

	public static void SetMetadata(string[] keys, string[] values)
	{
		if (!isInitialized)
		{
			return;
		}
		if (keys.Length != values.Length)
		{
			Debug.Log("Crittercism.SetMetadata given arrays of different lengths");
			return;
		}
		for (int i = 0; i < keys.Length; i++)
		{
			SetValue(keys[i], values[i]);
		}
	}

	public static void SetValue(string key, string value)
	{
		if (!isInitialized)
		{
			return;
		}
		using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("org.json.JSONObject"))
		{
			androidJavaObject.Call<AndroidJavaObject>("put", new object[2] { key, value });
			PluginCallStatic("setMetadata", androidJavaObject);
		}
	}

	public static void LeaveBreadcrumb(string breadcrumb)
	{
		if (isInitialized)
		{
			PluginCallStatic("leaveBreadcrumb", breadcrumb);
		}
	}

	public static void BeginUserflow(string userflowName)
	{
		if (isInitialized)
		{
			PluginCallStatic("beginTransaction", userflowName);
		}
	}

	[Obsolete("BeginTransaction is deprecated, please use BeginUserflow instead.")]
	public static void BeginTransaction(string userflowName)
	{
		BeginUserflow(userflowName);
	}

	public static void CancelUserflow(string userflowName)
	{
		if (isInitialized)
		{
			PluginCallStatic("cancelTransaction", userflowName);
		}
	}

	[Obsolete("CancelTransaction is deprecated, please use CancelUserflow instead.")]
	public static void CancelTransaction(string userflowName)
	{
		CancelUserflow(userflowName);
	}

	public static void EndUserflow(string userflowName)
	{
		if (isInitialized)
		{
			PluginCallStatic("endTransaction", userflowName);
		}
	}

	[Obsolete("EndTransaction is deprecated, please use EndUserflow instead.")]
	public static void EndTransaction(string userflowName)
	{
		EndUserflow(userflowName);
	}

	public static void FailUserflow(string userflowName)
	{
		if (isInitialized)
		{
			PluginCallStatic("failTransaction", userflowName);
		}
	}

	[Obsolete("FailTransaction is deprecated, please use FailUserflow instead.")]
	public static void FailTransaction(string userflowName)
	{
		FailUserflow(userflowName);
	}

	public static void SetUserflowValue(string userflowName, int value)
	{
		if (isInitialized)
		{
			PluginCallStatic("setTransactionValue", userflowName, value);
		}
	}

	[Obsolete("SetTransactionValue is deprecated, please use SetUserflowValue instead.")]
	public static void SetTransactionValue(string userflowName, int value)
	{
		SetUserflowValue(userflowName, value);
	}

	public static int GetUserflowValue(string userflowName)
	{
		if (!isInitialized)
		{
			return -1;
		}
		return PluginCallStatic<int>("getTransactionValue", new object[1] { userflowName });
	}

	[Obsolete("GetTransactionValue is deprecated, please use GetUserflowValue instead.")]
	public static int GetTransactionValue(string userflowName)
	{
		return GetUserflowValue(userflowName);
	}

	private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs args)
	{
		if (isInitialized && args != null && args.ExceptionObject != null)
		{
			Exception e = args.ExceptionObject as Exception;
			LogUnhandledException(e);
		}
	}

	public static void SetLogUnhandledExceptionAsCrash(bool value)
	{
		logUnhandledExceptionAsCrash = value;
	}

	public static bool GetLogUnhandledExceptionAsCrash()
	{
		return logUnhandledExceptionAsCrash;
	}

	private static void OnLogMessageReceived(string name, string stack, LogType type)
	{
		if (type == LogType.Exception && isInitialized)
		{
			if (logUnhandledExceptionAsCrash)
			{
				PluginCallStatic("_logCrashException", name, name, stack);
			}
			else
			{
				stack = new Regex("\r\n").Replace(stack, "\n\tat");
				PluginCallStatic("_logHandledException", name, name, stack);
			}
		}
	}

	private static void PluginCallStatic(string methodName, params object[] args)
	{
		mCrittercismsPlugin.CallStatic(methodName, args);
	}

	private static RetType PluginCallStatic<RetType>(string methodName, params object[] args)
	{
		return mCrittercismsPlugin.CallStatic<RetType>(methodName, args);
	}
}

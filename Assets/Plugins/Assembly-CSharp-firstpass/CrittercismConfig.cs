using UnityEngine;

public class CrittercismConfig
{
	private static readonly string CRITTERCISM_CONFIG_CLASS = "com.crittercism.app.CrittercismConfig";

	private AndroidJavaObject mCrittercismConfig;

	public CrittercismConfig()
	{
		mCrittercismConfig = new AndroidJavaObject(CRITTERCISM_CONFIG_CLASS);
	}

	public AndroidJavaObject GetAndroidConfig()
	{
		return mCrittercismConfig;
	}

	public string GetCustomVersionName()
	{
		return CallConfigMethod<string>("getCustomVersionName", new object[0]);
	}

	public void SetCustomVersionName(string customVersionName)
	{
		CallConfigMethod("setCustomVersionName", customVersionName);
	}

	public bool IsLogcatReportingEnabled()
	{
		return CallConfigMethod<bool>("isLogcatReportingEnabled", new object[0]);
	}

	public void SetLogcatReportingEnabled(bool shouldCollectLogcat)
	{
		CallConfigMethod("setLogcatReportingEnabled", shouldCollectLogcat);
	}

	public bool IsServiceMonitoringEnabled()
	{
		return CallConfigMethod<bool>("isServiceMonitoringEnabled", new object[0]);
	}

	public void SetServiceMonitoringEnabled(bool isServiceMonitoringEnabled)
	{
		CallConfigMethod("setServiceMonitoringEnabled", isServiceMonitoringEnabled);
	}

	private void CallConfigMethod(string methodName, params object[] args)
	{
		mCrittercismConfig.Call(methodName, args);
	}

	private RetType CallConfigMethod<RetType>(string methodName, params object[] args)
	{
		return mCrittercismConfig.Call<RetType>(methodName, args);
	}
}

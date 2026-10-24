public class AppsMinistrySdkBinding
{
	private static AppsMinistrySdkBinding instance;

	private IAppsMinistrySdk appsMinistrySdk;

	public static AppsMinistrySdkBinding Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new AppsMinistrySdkBinding();
			}
			return instance;
		}
	}

	private AppsMinistrySdkBinding()
	{
		if (UnityEngine.Application.platform == UnityEngine.RuntimePlatform.Android && !UnityEngine.Application.isEditor)
		{
			appsMinistrySdk = new AppsMinistrySdk_Android();
		}
		else
		{
			appsMinistrySdk = new AppsMinistrySdk_Dummy();
		}
	}

	public void AppsMinistrySdkConfigure(int appId, string key)
	{
		appsMinistrySdk.AppsMinistrySdkConfigure(appId, key);
	}

	public string AppsMinistrySDKCurrentSystemLanguage()
	{
		return appsMinistrySdk.AppsMinistrySDKCurrentSystemLanguage();
	}

	public string AppsMinistrySDKVersion()
	{
		return appsMinistrySdk.AppsMinistrySDKVersion();
	}

	public void AMLoggerEnableSDKLogging()
	{
		appsMinistrySdk.AMLoggerEnableSDKLogging();
	}

	public void AMLoggerDisableSDKLogging()
	{
		appsMinistrySdk.AMLoggerDisableSDKLogging();
	}

	public bool AMLoggerIsSDKLoggingEnabled()
	{
		return appsMinistrySdk.AMLoggerIsSDKLoggingEnabled();
	}

	public void AMLoggerPresentLogViewController()
	{
		appsMinistrySdk.AMLoggerPresentLogViewController();
	}

	public void AMCrashApp()
	{
		appsMinistrySdk.AMCrashApp();
	}

	public bool AMStoreKitValidatorValidate(string productId, string transactionId, string base64encodedReceipt)
	{
		return appsMinistrySdk.AMStoreKitValidatorValidate(productId, transactionId, base64encodedReceipt);
	}

	public int AMStoreKitSubscriptionExpirationDate(string productId, string base64encodedReceipt)
	{
		return appsMinistrySdk.AMStoreKitSubscriptionExpirationDate(productId, base64encodedReceipt);
	}

	public void AMEnableApplicationNotification()
	{
		appsMinistrySdk.AMEnableApplicationNotification();
	}

	public void AMDisableApplicationNotification()
	{
		appsMinistrySdk.AMDisableApplicationNotification();
	}

	public string AMPushServiceToken()
	{
		return appsMinistrySdk.AMPushServiceToken();
	}

	public void AMPushServiceStartSession(int appId, string key, string androidServerId = "")
	{
		appsMinistrySdk.AMPushServiceStartSession(appId, key, androidServerId);
	}

	public void AMPushServiceStartSessionWithOptions(bool manualMode, bool testMode, string language, string androidServerId = "")
	{
		appsMinistrySdk.AMPushServiceStartSessionWithOptions(manualMode, testMode, language, androidServerId);
	}

	public void AMPushServiceStartSessionWithOptions(int appId, string key, bool manualMode, bool testMode, string language, string androidServerId = "")
	{
		appsMinistrySdk.AMPushServiceStartSessionWithOptions(appId, key, manualMode, testMode, language, androidServerId);
	}

	public void AMPushServiceUpdateToken()
	{
		appsMinistrySdk.AMPushServiceUpdateToken();
	}

	public void AMPushServiceUpdateCurrentLanguage(string language)
	{
		appsMinistrySdk.AMPushServiceUpdateCurrentLanguage(language);
	}

	public void AMPushUpdate()
	{
		appsMinistrySdk.AMPushUpdate();
	}

	public void AMRichMediaStartSession(int appId, string key)
	{
		appsMinistrySdk.AMRichMediaStartSession(appId, key);
	}

	public void AMRichMediaStartSessionWithOptions(bool manualMode, bool testMode, string language, string orienation)
	{
		appsMinistrySdk.AMRichMediaStartSessionWithOptions(manualMode, testMode, language, orienation);
	}

	public void AMRichmediaUpdateCurrentLanguage(string language)
	{
		appsMinistrySdk.AMRichmediaUpdateCurrentLanguage(language);
	}

	public void AMRichMediaShow(string screenName = "")
	{
		appsMinistrySdk.AMRichMediaShow(screenName);
	}

	public bool AMRichMediaIsShowing()
	{
		return appsMinistrySdk.AMRichMediaIsShowing();
	}

	public bool AMIsOnline()
	{
		return appsMinistrySdk.AMIsOnline();
	}

	public void AMStartNetworkStateNotification()
	{
		appsMinistrySdk.AMStartNetworkStateNotification();
	}

	public void AMStopNetworkStateNotification()
	{
		appsMinistrySdk.AMStopNetworkStateNotification();
	}
}

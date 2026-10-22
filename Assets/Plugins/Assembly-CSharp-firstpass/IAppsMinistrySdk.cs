public interface IAppsMinistrySdk
{
	void AppsMinistrySdkConfigure(int appId, string key);

	string AppsMinistrySDKCurrentSystemLanguage();

	string AppsMinistrySDKVersion();

	void AMLoggerEnableSDKLogging();

	void AMLoggerDisableSDKLogging();

	bool AMLoggerIsSDKLoggingEnabled();

	void AMLoggerPresentLogViewController();

	void AMCrashApp();

	bool AMStoreKitValidatorValidate(string productId, string transactionId, string base64encodedReceipt);

	void AMEnableApplicationNotification();

	void AMDisableApplicationNotification();

	bool AMIsOnline();

	void AMStartNetworkStateNotification();

	void AMStopNetworkStateNotification();

	string AMPushServiceToken();

	void AMPushServiceStartSession(int appId, string key, string androidSenderId = "");

	void AMPushServiceStartSessionWithOptions(bool manualMode, bool testMode, string language, string androidSenderId = "");

	void AMPushServiceStartSessionWithOptions(int appId, string key, bool manualMode, bool testMode, string language, string androidSenderId = "");

	void AMPushServiceUpdateToken();

	void AMPushServiceUpdateCurrentLanguage(string language);

	void AMPushUpdate();

	int AMStoreKitSubscriptionExpirationDate(string productId, string base64encodedReceipt);

	void AMRichMediaStartSession(int appId, string key);

	void AMRichMediaStartSessionWithOptions(bool manualMode, bool testMode, string language, string orienation);

	void AMRichmediaUpdateCurrentLanguage(string language);

	void AMRichMediaShow(string screenName);

	bool AMRichMediaIsShowing();
}

using UnityEngine;

namespace KHD
{
	public class FlurryAnalyticsHelper : MonoBehaviour
	{
		[SerializeField]
		private string _iOSApiKey;

		[SerializeField]
		private string _androidApiKey;

		[SerializeField]
		private bool _enableDebugLog;

		[SerializeField]
		private bool _sendCrashReports = true;

		[SerializeField]
		private bool _replicateDataToUnityAnalytics;

		[Space(10f)]
		[HideInInspector]
		[SerializeField]
		private bool _iOSIAPReportingEnabled;

		private void Awake()
		{
			SingletonCrossSceneAutoCreate<FlurryAnalytics>.Instance.SetDebugLogEnabled(_enableDebugLog);
			SingletonCrossSceneAutoCreate<FlurryAnalytics>.Instance.replicateDataToUnityAnalytics = _replicateDataToUnityAnalytics;
			SingletonCrossSceneAutoCreate<FlurryAnalytics>.Instance.StartSession(_iOSApiKey, _androidApiKey, _sendCrashReports);
		}
	}
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

namespace KHD
{
	[AddComponentMenu("")]
	public class FlurryAnalytics : SingletonCrossSceneAutoCreate<FlurryAnalytics>
	{
		public enum Gender
		{
			Female = 0,
			Male = 1
		}

		public bool replicateDataToUnityAnalytics { get; set; }

		protected override void OnDestroy()
		{
			base.OnDestroy();
			FlurryAnalyticsAndroid.Dispose();
		}

		public void StartSession(string iOSApiKey, string androidApiKey, bool sendErrorsToFlurry)
		{
			FlurryAnalyticsAndroid.Init(androidApiKey, sendErrorsToFlurry);
			FlurryAnalyticsAndroid.OnStartSession();
		}

		public void SetDebugLogEnabled(bool enabled)
		{
			FlurryAnalyticsAndroid.SetLogEnabled(enabled);
		}

		public void SetAppVersion(string appVersion)
		{
			FlurryAnalyticsAndroid.SetVersionName(appVersion);
		}

		public void SetSessionContinueSeconds(int seconds)
		{
			FlurryAnalyticsAndroid.SetContinueSessionMillis(seconds * 1000);
		}

		public void SetPulseEnabled(bool enabled)
		{
			FlurryAnalyticsAndroid.SetPulseEnabled(enabled);
		}

		public void SetUserId(string userId)
		{
			FlurryAnalyticsAndroid.SetUserId(userId);
			ReplicateUserIdToUnityAnalytics(userId);
		}

		public void SetAge(int age)
		{
			FlurryAnalyticsAndroid.SetAge(age);
		}

		public void SetGender(Gender gender)
		{
			FlurryAnalyticsAndroid.SetGender(gender);
			ReplicateUserGenderToUnityAnalytics(gender);
		}

		public void LogEvent(string eventName)
		{
			FlurryAnalyticsAndroid.LogEvent(eventName);
			ReplicateEventToUnityAnalytics(eventName);
		}

		public void LogEvent(string eventName, bool isTimed)
		{
			FlurryAnalyticsAndroid.LogEvent(eventName, isTimed);
			ReplicateEventToUnityAnalytics(eventName);
		}

		public void LogEventWithParameters(string eventName, Dictionary<string, string> parameters)
		{
			FlurryAnalyticsAndroid.LogEventWithParameters(eventName, parameters);
			ReplicateEventToUnityAnalytics(eventName, parameters);
		}

		public void LogEventWithParameters(string eventName, Dictionary<string, string> parameters, bool isTimed)
		{
			FlurryAnalyticsAndroid.LogEventWithParameters(eventName, parameters, isTimed);
			ReplicateEventToUnityAnalytics(eventName, parameters);
		}

		public void EndTimedEvent(string eventName, Dictionary<string, string> parameters = null)
		{
			FlurryAnalyticsAndroid.EndTimedEvent(eventName, parameters);
		}

		private void ReplicateUserIdToUnityAnalytics(string userId)
		{
#if !UNITY_2018_3_OR_NEWER
			if (replicateDataToUnityAnalytics)
			{
				Analytics.SetUserId(userId);
			}
#endif
		}

		private void ReplicateUserGenderToUnityAnalytics(Gender gender)
		{
#if !UNITY_2018_3_OR_NEWER
			if (replicateDataToUnityAnalytics)
			{
				Analytics.SetUserGender((gender != Gender.Male) ? UnityEngine.Analytics.Gender.Female : UnityEngine.Analytics.Gender.Male);
			}
#endif
		}

		private void ReplicateEventToUnityAnalytics(string eventName, Dictionary<string, string> parameters = null)
		{
			if (!replicateDataToUnityAnalytics)
			{
				return;
			}
			Dictionary<string, object> dictionary = null;
			if (parameters != null && parameters.Count > 0)
			{
				dictionary = new Dictionary<string, object>();
				foreach (KeyValuePair<string, string> parameter in parameters)
				{
					dictionary.Add(parameter.Key, parameter.Value);
				}
			}
			Analytics.CustomEvent(eventName, dictionary);
		}

		private void DataReplicationToUnityAnalyticsWarning()
		{
			Debug.LogWarning("Unity Analytics is disabled, please turn off data replication.To enable Unity Analytics please use this guide http://docs.unity3d.com/Manual/UnityAnalyticsOverview.html");
		}
	}
}

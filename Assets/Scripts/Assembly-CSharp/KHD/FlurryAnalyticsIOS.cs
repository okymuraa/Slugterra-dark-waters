using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace KHD
{
	public static class FlurryAnalyticsIOS
	{
		public static void StartSession(string apiKey, bool sendCrashReports)
		{
		}

		public static void SetDebugLogEnabled(bool enabled)
		{
		}

		public static void SetAppVersion(string appVersion)
		{
		}

		public static void SetSessionContinueSeconds(int seconds)
		{
		}

		public static void SetPulseEnabled(bool enabled)
		{
		}

		public static void SetIAPReportingEnabled(bool enabled)
		{
		}

		public static void SetUserId(string userId)
		{
		}

		public static void SetAge(int age)
		{
		}

		public static void SetGender(FlurryAnalytics.Gender gender)
		{
		}

		public static void SetLocation(float latitude, float longitude, float horizontalAccuracy, float verticalAccuracy)
		{
		}

		public static void LogEvent(string eventName, bool isTimed = false)
		{
		}

		public static void LogEventWithParameters(string eventName, Dictionary<string, string> parameters, bool isTimed = false)
		{
		}

		public static void EndTimedEvent(string eventName, Dictionary<string, string> parameters = null)
		{
		}

		public static void SetSessionReportsOnCloseEnabled(bool enabled)
		{
		}

		public static void SetSessionReportsOnPauseEnabled(bool enabled)
		{
		}

		private static string ConvertParameters(Dictionary<string, string> parameters)
		{
			if (parameters == null)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("{\n");
			bool flag = true;
			foreach (KeyValuePair<string, string> parameter in parameters)
			{
				if (!flag)
				{
					stringBuilder.Append(',');
				}
				SerializeString(stringBuilder, parameter.Key);
				stringBuilder.Append(":");
				SerializeString(stringBuilder, parameter.Value);
				flag = false;
			}
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		private static void SerializeString(StringBuilder builder, string str)
		{
			builder.Append('"');
			char[] array = str.ToCharArray();
			char[] array2 = array;
			foreach (char c in array2)
			{
				switch (c)
				{
				case '"':
					builder.Append("\\\"");
					continue;
				case '\\':
					builder.Append("\\\\");
					continue;
				case '\b':
					builder.Append("\\b");
					continue;
				case '\f':
					builder.Append("\\f");
					continue;
				case '\n':
					builder.Append("\\n");
					continue;
				case '\r':
					builder.Append("\\r");
					continue;
				case '\t':
					builder.Append("\\t");
					continue;
				}
				int num = Convert.ToInt32(c);
				if (num >= 32 && num <= 126)
				{
					builder.Append(c);
				}
				else
				{
					builder.Append("\\u" + Convert.ToString(num, 16).PadLeft(4, '0'));
				}
			}
			builder.Append('"');
		}

		private static bool IsIPhonePlayer()
		{
			return Application.platform == RuntimePlatform.IPhonePlayer;
		}

		private static bool EditorDebugLogEnabled()
		{
			return false;
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

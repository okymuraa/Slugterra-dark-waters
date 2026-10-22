using System;
using UnityEngine;

namespace AppsMinistry.Core
{
	public sealed class MonoLog
	{
		[Flags]
		public enum Channel
		{
			All = 1,
			Exception = 2,
			GameCenter = 4,
			UI = 8,
			Core = 0x10,
			Plugin = 0x20,
			Game = 0x40,
			Configuration = 0x80,
			Shop = 0x100,
			Analytics = 0x200,
			Store = 0x400,
			Ads = 0x800
		}

		private static string COMPANY_NAME;

		private static string PROJECT_NAME;

		private static Channel _channels;

		static MonoLog()
		{
			COMPANY_NAME = "AppsMinistry";
			PROJECT_NAME = "Slugterra";
			_channels |= Channel.UI;
			_channels |= Channel.Analytics;
		}

		public static void Log(Channel chanel, string message)
		{
			if ((_channels & Channel.All) == Channel.All || (_channels & chanel) == chanel)
			{
				Debug.Log(string.Format("{0}.{1}: {2}", COMPANY_NAME, PROJECT_NAME, message));
			}
		}

		public static void LogWarning(Channel chanel, string message)
		{
			if ((_channels & Channel.All) == Channel.All || (_channels & chanel) == chanel)
			{
				Debug.LogWarning(string.Format("{0}.{1}: {2}", COMPANY_NAME, PROJECT_NAME, message));
			}
		}

		public static void LogError(Channel chanel, string message)
		{
			if ((_channels & Channel.All) == Channel.All || (_channels & chanel) == chanel)
			{
				Debug.LogError(string.Format("{0}.{1}: {2}", COMPANY_NAME, PROJECT_NAME, message));
			}
		}
	}
}

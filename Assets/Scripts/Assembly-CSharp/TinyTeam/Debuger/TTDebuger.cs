using System;
using UnityEngine;

namespace TinyTeam.Debuger
{
	public class TTDebuger
	{
		public static bool EnableLog = true;

		public static void Log(object message)
		{
			Log(message, null);
		}

		public static void Log(object message, string customType)
		{
			if (EnableLog)
			{
				Console.Log(message, customType);
			}
		}

		public static void Log(object message, string customType, Color col)
		{
			if (EnableLog)
			{
				Console.Log(message, customType, col);
			}
		}

		public static void LogError(object message)
		{
			LogError(message, null);
		}

		public static void LogError(object message, string customType)
		{
			if (EnableLog)
			{
				Console.LogError(message, customType);
			}
		}

		public static void LogWarning(object message)
		{
			LogWarning(message, null);
		}

		public static void LogWarning(object message, string customType)
		{
			if (EnableLog)
			{
				Console.LogWarning(message, customType);
			}
		}

		public static void RegisterCommand(string commandString, Func<string[], object> commandCallback, string CMD_Discribes)
		{
			Console.RegisterCommand(commandString, commandCallback, CMD_Discribes);
		}

		public static void UnRegisterCommand(string commandString)
		{
			Console.UnRegisterCommand(commandString);
		}
	}
}

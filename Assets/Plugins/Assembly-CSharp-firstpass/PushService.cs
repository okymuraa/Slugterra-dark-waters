using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using MiniJSON;
using UnityEngine;

public class PushService : MonoBehaviour
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> didReceiveRemoteNotificationEvent__BackingField;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> didReceiveLocalNotificationEvent__BackingField;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> didRegisterUserNotificationSettingsEvent__BackingField;

	public static event Action<string> didReceiveRemoteNotificationEvent
	{
		add
		{
			Action<string> action = didReceiveRemoteNotificationEvent__BackingField;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref didReceiveRemoteNotificationEvent__BackingField, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<string> action = didReceiveRemoteNotificationEvent__BackingField;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref didReceiveRemoteNotificationEvent__BackingField, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	public static event Action<string> didReceiveLocalNotificationEvent
	{
		add
		{
			Action<string> action = didReceiveLocalNotificationEvent__BackingField;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref didReceiveLocalNotificationEvent__BackingField, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<string> action = didReceiveLocalNotificationEvent__BackingField;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref didReceiveLocalNotificationEvent__BackingField, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	public static event Action<string> didRegisterUserNotificationSettingsEvent
	{
		add
		{
			Action<string> action = didRegisterUserNotificationSettingsEvent__BackingField;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref didRegisterUserNotificationSettingsEvent__BackingField, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<string> action = didRegisterUserNotificationSettingsEvent__BackingField;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref didRegisterUserNotificationSettingsEvent__BackingField, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	private void Awake()
	{
		if (transform.parent == null)
		{
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
		}
	}

	private void Start()
	{
		AppsMinistrySdkBinding.Instance.AMPushUpdate();
	}

	public void didReceiveRemoteNotification(string result)
	{
		Dictionary<string, object> dictionary = Json.Deserialize(result) as Dictionary<string, object>;
		if (dictionary != null)
		{
			string text = ((!dictionary.ContainsKey("waking")) ? "false" : dictionary["waking"].ToString().ToLower());
			string value = ((!dictionary.ContainsKey("message")) ? string.Empty : dictionary["message"].ToString());
			if (text == "true" && string.IsNullOrEmpty(value))
			{
			}
		}
		if (didReceiveRemoteNotificationEvent__BackingField != null)
		{
			didReceiveRemoteNotificationEvent__BackingField(result);
		}
	}

	public void didReceiveLocalNotification(string result)
	{
		Dictionary<string, object> dictionary = Json.Deserialize(result) as Dictionary<string, object>;
		if (dictionary != null)
		{
			string text = ((!dictionary.ContainsKey("waking")) ? "false" : dictionary["waking"].ToString().ToLower());
			string value = ((!dictionary.ContainsKey("message")) ? string.Empty : dictionary["message"].ToString());
			if (text == "true" && string.IsNullOrEmpty(value))
			{
			}
		}
		if (didReceiveLocalNotificationEvent__BackingField != null)
		{
			didReceiveLocalNotificationEvent__BackingField(result);
		}
	}

	public void didRegisterUserNotificationSettings(string result)
	{
		if (didRegisterUserNotificationSettingsEvent__BackingField != null)
		{
			didRegisterUserNotificationSettingsEvent__BackingField(result);
		}
	}
}

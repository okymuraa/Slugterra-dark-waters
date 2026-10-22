using System;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

public class RichMediaService : MonoBehaviour
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> willShowEvent__BackingField;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> didShowEvent__BackingField;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> didCloseEvent__BackingField;

	public static event Action<string> willShowEvent
	{
		add
		{
			Action<string> action = willShowEvent__BackingField;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref willShowEvent__BackingField, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<string> action = willShowEvent__BackingField;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref willShowEvent__BackingField, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	public static event Action<string> didShowEvent
	{
		add
		{
			Action<string> action = didShowEvent__BackingField;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref didShowEvent__BackingField, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<string> action = didShowEvent__BackingField;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref didShowEvent__BackingField, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	public static event Action<string> didCloseEvent
	{
		add
		{
			Action<string> action = didCloseEvent__BackingField;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref didCloseEvent__BackingField, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<string> action = didCloseEvent__BackingField;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref didCloseEvent__BackingField, (Action<string>)Delegate.Remove(action2, value), action);
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

	public void willShow(string result)
	{
		if (willShowEvent__BackingField != null)
		{
			willShowEvent__BackingField(result);
		}
	}

	public void didShow(string result)
	{
		UnityEngine.Debug.Log("RichMediaService didShow " + result);
		if (didShowEvent__BackingField != null)
		{
			didShowEvent__BackingField(result);
		}
	}

	public void didClose(string result)
	{
		UnityEngine.Debug.Log("RichMediaService didClose " + result);
		if (didCloseEvent__BackingField != null)
		{
			didCloseEvent__BackingField(result);
		}
	}
}

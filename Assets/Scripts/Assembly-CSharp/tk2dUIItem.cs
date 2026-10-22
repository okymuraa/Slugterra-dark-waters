using System;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

[AddComponentMenu("2D Toolkit/UI/Core/tk2dUIItem")]
public class tk2dUIItem : MonoBehaviour
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action OnDown__BackingField;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action OnUp__BackingField;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action OnClick__BackingField;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action OnRelease__BackingField;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action OnHoverOver__BackingField;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action OnHoverOut__BackingField;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<tk2dUIItem> OnDownUIItem__BackingField;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<tk2dUIItem> OnUpUIItem__BackingField;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<tk2dUIItem> OnClickUIItem__BackingField;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<tk2dUIItem> OnReleaseUIItem__BackingField;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<tk2dUIItem> OnHoverOverUIItem__BackingField;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<tk2dUIItem> OnHoverOutUIItem__BackingField;

	public GameObject sendMessageTarget;

	public string SendMessageOnDownMethodName = string.Empty;

	public string SendMessageOnUpMethodName = string.Empty;

	public string SendMessageOnClickMethodName = string.Empty;

	public string SendMessageOnReleaseMethodName = string.Empty;

	[SerializeField]
	private bool isChildOfAnotherUIItem;

	public bool registerPressFromChildren;

	public bool isHoverEnabled;

	public Transform[] editorExtraBounds = new Transform[0];

	public Transform[] editorIgnoreBounds = new Transform[0];

	private bool isPressed;

	private bool isHoverOver;

	private tk2dUITouch touch;

	private tk2dUIItem parentUIItem;

	public bool IsPressed
	{
		get
		{
			return isPressed;
		}
	}

	public tk2dUITouch Touch
	{
		get
		{
			return touch;
		}
	}

	public tk2dUIItem ParentUIItem
	{
		get
		{
			return parentUIItem;
		}
	}

	public event Action OnDown
	{
		add
		{
			Action action = OnDown__BackingField;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnDown__BackingField, (Action)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action action = OnDown__BackingField;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnDown__BackingField, (Action)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	public event Action OnUp
	{
		add
		{
			Action action = OnUp__BackingField;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnUp__BackingField, (Action)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action action = OnUp__BackingField;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnUp__BackingField, (Action)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	public event Action OnClick
	{
		add
		{
			Action action = OnClick__BackingField;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnClick__BackingField, (Action)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action action = OnClick__BackingField;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnClick__BackingField, (Action)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	public event Action OnRelease
	{
		add
		{
			Action action = OnRelease__BackingField;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnRelease__BackingField, (Action)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action action = OnRelease__BackingField;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnRelease__BackingField, (Action)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	public event Action OnHoverOver
	{
		add
		{
			Action action = OnHoverOver__BackingField;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnHoverOver__BackingField, (Action)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action action = OnHoverOver__BackingField;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnHoverOver__BackingField, (Action)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	public event Action OnHoverOut
	{
		add
		{
			Action action = OnHoverOut__BackingField;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnHoverOut__BackingField, (Action)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action action = OnHoverOut__BackingField;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnHoverOut__BackingField, (Action)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	public event Action<tk2dUIItem> OnDownUIItem
	{
		add
		{
			Action<tk2dUIItem> action = OnDownUIItem__BackingField;
			Action<tk2dUIItem> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnDownUIItem__BackingField, (Action<tk2dUIItem>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<tk2dUIItem> action = OnDownUIItem__BackingField;
			Action<tk2dUIItem> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnDownUIItem__BackingField, (Action<tk2dUIItem>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	public event Action<tk2dUIItem> OnUpUIItem
	{
		add
		{
			Action<tk2dUIItem> action = OnUpUIItem__BackingField;
			Action<tk2dUIItem> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnUpUIItem__BackingField, (Action<tk2dUIItem>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<tk2dUIItem> action = OnUpUIItem__BackingField;
			Action<tk2dUIItem> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnUpUIItem__BackingField, (Action<tk2dUIItem>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	public event Action<tk2dUIItem> OnClickUIItem
	{
		add
		{
			Action<tk2dUIItem> action = OnClickUIItem__BackingField;
			Action<tk2dUIItem> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnClickUIItem__BackingField, (Action<tk2dUIItem>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<tk2dUIItem> action = OnClickUIItem__BackingField;
			Action<tk2dUIItem> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnClickUIItem__BackingField, (Action<tk2dUIItem>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	public event Action<tk2dUIItem> OnReleaseUIItem
	{
		add
		{
			Action<tk2dUIItem> action = OnReleaseUIItem__BackingField;
			Action<tk2dUIItem> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnReleaseUIItem__BackingField, (Action<tk2dUIItem>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<tk2dUIItem> action = OnReleaseUIItem__BackingField;
			Action<tk2dUIItem> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnReleaseUIItem__BackingField, (Action<tk2dUIItem>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	public event Action<tk2dUIItem> OnHoverOverUIItem
	{
		add
		{
			Action<tk2dUIItem> action = OnHoverOverUIItem__BackingField;
			Action<tk2dUIItem> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnHoverOverUIItem__BackingField, (Action<tk2dUIItem>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<tk2dUIItem> action = OnHoverOverUIItem__BackingField;
			Action<tk2dUIItem> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnHoverOverUIItem__BackingField, (Action<tk2dUIItem>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	public event Action<tk2dUIItem> OnHoverOutUIItem
	{
		add
		{
			Action<tk2dUIItem> action = OnHoverOutUIItem__BackingField;
			Action<tk2dUIItem> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnHoverOutUIItem__BackingField, (Action<tk2dUIItem>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<tk2dUIItem> action = OnHoverOutUIItem__BackingField;
			Action<tk2dUIItem> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnHoverOutUIItem__BackingField, (Action<tk2dUIItem>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	private void Awake()
	{
		if (isChildOfAnotherUIItem)
		{
			UpdateParent();
		}
	}

	private void Start()
	{
		if (tk2dUIManager.Instance == null)
		{
			UnityEngine.Debug.LogError("Unable to find tk2dUIManager. Please create a tk2dUIManager in the scene before proceeding.");
		}
		if (isChildOfAnotherUIItem && parentUIItem == null)
		{
			UpdateParent();
		}
	}

	public void UpdateParent()
	{
		parentUIItem = GetParentUIItem();
	}

	public void ManuallySetParent(tk2dUIItem newParentUIItem)
	{
		parentUIItem = newParentUIItem;
	}

	public void RemoveParent()
	{
		parentUIItem = null;
	}

	public bool Press(tk2dUITouch touch)
	{
		return Press(touch, null);
	}

	public bool Press(tk2dUITouch touch, tk2dUIItem sentFromChild)
	{
		if (isPressed)
		{
			return false;
		}
		if (!isPressed)
		{
			this.touch = touch;
			if ((registerPressFromChildren || sentFromChild == null) && enabled)
			{
				isPressed = true;
				if (OnDown__BackingField != null)
				{
					OnDown__BackingField();
				}
				if (OnDownUIItem__BackingField != null)
				{
					OnDownUIItem__BackingField(this);
				}
				DoSendMessage(SendMessageOnDownMethodName);
			}
			if (parentUIItem != null)
			{
				parentUIItem.Press(touch, this);
			}
		}
		return true;
	}

	public void UpdateTouch(tk2dUITouch touch)
	{
		this.touch = touch;
		if (parentUIItem != null)
		{
			parentUIItem.UpdateTouch(touch);
		}
	}

	private void DoSendMessage(string methodName)
	{
		if (sendMessageTarget != null && methodName.Length > 0)
		{
			sendMessageTarget.SendMessage(methodName, this, SendMessageOptions.RequireReceiver);
		}
	}

	public void Release()
	{
		if (isPressed)
		{
			isPressed = false;
			if (OnUp__BackingField != null)
			{
				OnUp__BackingField();
			}
			if (OnUpUIItem__BackingField != null)
			{
				OnUpUIItem__BackingField(this);
			}
			DoSendMessage(SendMessageOnUpMethodName);
			if (OnClick__BackingField != null)
			{
				OnClick__BackingField();
			}
			if (OnClickUIItem__BackingField != null)
			{
				OnClickUIItem__BackingField(this);
			}
			DoSendMessage(SendMessageOnClickMethodName);
		}
		if (OnRelease__BackingField != null)
		{
			OnRelease__BackingField();
		}
		if (OnReleaseUIItem__BackingField != null)
		{
			OnReleaseUIItem__BackingField(this);
		}
		DoSendMessage(SendMessageOnReleaseMethodName);
		if (parentUIItem != null)
		{
			parentUIItem.Release();
		}
	}

	public void CurrentOverUIItem(tk2dUIItem overUIItem)
	{
		if (!(overUIItem != this))
		{
			return;
		}
		if (isPressed)
		{
			if (!CheckIsUIItemChildOfMe(overUIItem))
			{
				Exit();
				if (parentUIItem != null)
				{
					parentUIItem.CurrentOverUIItem(overUIItem);
				}
			}
		}
		else if (parentUIItem != null)
		{
			parentUIItem.CurrentOverUIItem(overUIItem);
		}
	}

	public bool CheckIsUIItemChildOfMe(tk2dUIItem uiItem)
	{
		tk2dUIItem tk2dUIItem2 = null;
		bool result = false;
		if (uiItem != null)
		{
			tk2dUIItem2 = uiItem.parentUIItem;
		}
		while (tk2dUIItem2 != null)
		{
			if (tk2dUIItem2 == this)
			{
				result = true;
				break;
			}
			tk2dUIItem2 = tk2dUIItem2.parentUIItem;
		}
		return result;
	}

	public void Exit()
	{
		if (isPressed)
		{
			isPressed = false;
			if (OnUp__BackingField != null)
			{
				OnUp__BackingField();
			}
			if (OnUpUIItem__BackingField != null)
			{
				OnUpUIItem__BackingField(this);
			}
			DoSendMessage(SendMessageOnUpMethodName);
		}
	}

	public bool HoverOver(tk2dUIItem prevHover)
	{
		bool flag = false;
		tk2dUIItem tk2dUIItem2 = null;
		if (!isHoverOver)
		{
			if (OnHoverOver__BackingField != null)
			{
				OnHoverOver__BackingField();
			}
			if (OnHoverOverUIItem__BackingField != null)
			{
				OnHoverOverUIItem__BackingField(this);
			}
			isHoverOver = true;
		}
		if (prevHover == this)
		{
			flag = true;
		}
		if (parentUIItem != null && parentUIItem.isHoverEnabled)
		{
			tk2dUIItem2 = parentUIItem;
		}
		if (tk2dUIItem2 == null)
		{
			return flag;
		}
		return tk2dUIItem2.HoverOver(prevHover) || flag;
	}

	public void HoverOut(tk2dUIItem currHoverButton)
	{
		if (isHoverOver)
		{
			if (OnHoverOut__BackingField != null)
			{
				OnHoverOut__BackingField();
			}
			if (OnHoverOutUIItem__BackingField != null)
			{
				OnHoverOutUIItem__BackingField(this);
			}
			isHoverOver = false;
		}
		if (parentUIItem != null && parentUIItem.isHoverEnabled)
		{
			if (currHoverButton == null)
			{
				parentUIItem.HoverOut(currHoverButton);
			}
			else if (!parentUIItem.CheckIsUIItemChildOfMe(currHoverButton) && currHoverButton != parentUIItem)
			{
				parentUIItem.HoverOut(currHoverButton);
			}
		}
	}

	private tk2dUIItem GetParentUIItem()
	{
		Transform parent = transform.parent;
		while (parent != null)
		{
			tk2dUIItem component = parent.GetComponent<tk2dUIItem>();
			if (component != null)
			{
				return component;
			}
			parent = parent.parent;
		}
		return null;
	}

	public void SimulateClick()
	{
		if (OnDown__BackingField != null)
		{
			OnDown__BackingField();
		}
		if (OnDownUIItem__BackingField != null)
		{
			OnDownUIItem__BackingField(this);
		}
		DoSendMessage(SendMessageOnDownMethodName);
		if (OnUp__BackingField != null)
		{
			OnUp__BackingField();
		}
		if (OnUpUIItem__BackingField != null)
		{
			OnUpUIItem__BackingField(this);
		}
		DoSendMessage(SendMessageOnUpMethodName);
		if (OnClick__BackingField != null)
		{
			OnClick__BackingField();
		}
		if (OnClickUIItem__BackingField != null)
		{
			OnClickUIItem__BackingField(this);
		}
		DoSendMessage(SendMessageOnClickMethodName);
		if (OnRelease__BackingField != null)
		{
			OnRelease__BackingField();
		}
		if (OnReleaseUIItem__BackingField != null)
		{
			OnReleaseUIItem__BackingField(this);
		}
		DoSendMessage(SendMessageOnReleaseMethodName);
	}

	public void InternalSetIsChildOfAnotherUIItem(bool state)
	{
		isChildOfAnotherUIItem = state;
	}

	public bool InternalGetIsChildOfAnotherUIItem()
	{
		return isChildOfAnotherUIItem;
	}
}

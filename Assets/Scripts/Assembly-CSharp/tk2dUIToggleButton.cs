using System;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

[AddComponentMenu("2D Toolkit/UI/tk2dUIToggleButton")]
public class tk2dUIToggleButton : tk2dUIBaseItemControl
{
	public GameObject offStateGO;

	public GameObject onStateGO;

	public bool activateOnPress;

	[SerializeField]
	private bool isOn = true;

	private bool isInToggleGroup;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<tk2dUIToggleButton> OnToggle__BackingField;

	public string SendMessageOnToggleMethodName = string.Empty;

	public bool IsOn
	{
		get
		{
			return isOn;
		}
		set
		{
			if (isOn != value)
			{
				isOn = value;
				SetState();
				if (OnToggle__BackingField != null)
				{
					OnToggle__BackingField(this);
				}
			}
		}
	}

	public bool IsInToggleGroup
	{
		get
		{
			return isInToggleGroup;
		}
		set
		{
			isInToggleGroup = value;
		}
	}

	public event Action<tk2dUIToggleButton> OnToggle
	{
		add
		{
			Action<tk2dUIToggleButton> action = OnToggle__BackingField;
			Action<tk2dUIToggleButton> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnToggle__BackingField, (Action<tk2dUIToggleButton>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<tk2dUIToggleButton> action = OnToggle__BackingField;
			Action<tk2dUIToggleButton> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnToggle__BackingField, (Action<tk2dUIToggleButton>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	private void Start()
	{
		SetState();
	}

	private void OnEnable()
	{
		if ((bool)uiItem)
		{
			uiItem.OnClick += ButtonClick;
			uiItem.OnDown += ButtonDown;
		}
	}

	private void OnDisable()
	{
		if ((bool)uiItem)
		{
			uiItem.OnClick -= ButtonClick;
			uiItem.OnDown -= ButtonDown;
		}
	}

	private void ButtonClick()
	{
		if (!activateOnPress)
		{
			ButtonToggle();
		}
	}

	private void ButtonDown()
	{
		if (activateOnPress)
		{
			ButtonToggle();
		}
	}

	private void ButtonToggle()
	{
		if (!isOn || !isInToggleGroup)
		{
			isOn = !isOn;
			SetState();
			if (OnToggle__BackingField != null)
			{
				OnToggle__BackingField(this);
			}
			DoSendMessage(SendMessageOnToggleMethodName, this);
		}
	}

	private void SetState()
	{
		tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(offStateGO, !isOn);
		tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(onStateGO, isOn);
	}
}

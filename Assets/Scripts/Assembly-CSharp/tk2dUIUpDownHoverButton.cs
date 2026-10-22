using System;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

[AddComponentMenu("2D Toolkit/UI/tk2dUIUpDownHoverButton")]
public class tk2dUIUpDownHoverButton : tk2dUIBaseItemControl
{
	public GameObject upStateGO;

	public GameObject downStateGO;

	public GameObject hoverOverStateGO;

	[SerializeField]
	private bool useOnReleaseInsteadOfOnUp;

	private bool isDown;

	private bool isHover;

	public string SendMessageOnToggleOverMethodName = string.Empty;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<tk2dUIUpDownHoverButton> OnToggleOver__BackingField;

	public bool UseOnReleaseInsteadOfOnUp
	{
		get
		{
			return useOnReleaseInsteadOfOnUp;
		}
	}

	public bool IsOver
	{
		get
		{
			return isDown || isHover;
		}
		set
		{
			if (value == isDown && !isHover)
			{
				return;
			}
			if (value)
			{
				isHover = true;
				SetState();
				if (OnToggleOver__BackingField != null)
				{
					OnToggleOver__BackingField(this);
				}
			}
			else if (isDown && isHover)
			{
				isDown = false;
				isHover = false;
				SetState();
				if (OnToggleOver__BackingField != null)
				{
					OnToggleOver__BackingField(this);
				}
			}
			else if (isDown)
			{
				isDown = false;
				SetState();
				if (OnToggleOver__BackingField != null)
				{
					OnToggleOver__BackingField(this);
				}
			}
			else
			{
				isHover = false;
				SetState();
				if (OnToggleOver__BackingField != null)
				{
					OnToggleOver__BackingField(this);
				}
			}
			DoSendMessage(SendMessageOnToggleOverMethodName, this);
		}
	}

	public event Action<tk2dUIUpDownHoverButton> OnToggleOver
	{
		add
		{
			Action<tk2dUIUpDownHoverButton> action = OnToggleOver__BackingField;
			Action<tk2dUIUpDownHoverButton> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnToggleOver__BackingField, (Action<tk2dUIUpDownHoverButton>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<tk2dUIUpDownHoverButton> action = OnToggleOver__BackingField;
			Action<tk2dUIUpDownHoverButton> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnToggleOver__BackingField, (Action<tk2dUIUpDownHoverButton>)Delegate.Remove(action2, value), action);
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
			uiItem.OnDown += ButtonDown;
			if (useOnReleaseInsteadOfOnUp)
			{
				uiItem.OnRelease += ButtonUp;
			}
			else
			{
				uiItem.OnUp += ButtonUp;
			}
			uiItem.OnHoverOver += ButtonHoverOver;
			uiItem.OnHoverOut += ButtonHoverOut;
		}
	}

	private void OnDisable()
	{
		if ((bool)uiItem)
		{
			uiItem.OnDown -= ButtonDown;
			if (useOnReleaseInsteadOfOnUp)
			{
				uiItem.OnRelease -= ButtonUp;
			}
			else
			{
				uiItem.OnUp -= ButtonUp;
			}
			uiItem.OnHoverOver -= ButtonHoverOver;
			uiItem.OnHoverOut -= ButtonHoverOut;
		}
	}

	private void ButtonUp()
	{
		if (isDown)
		{
			isDown = false;
			SetState();
			if (!isHover && OnToggleOver__BackingField != null)
			{
				OnToggleOver__BackingField(this);
			}
		}
	}

	private void ButtonDown()
	{
		if (!isDown)
		{
			isDown = true;
			SetState();
			if (!isHover && OnToggleOver__BackingField != null)
			{
				OnToggleOver__BackingField(this);
			}
		}
	}

	private void ButtonHoverOver()
	{
		if (!isHover)
		{
			isHover = true;
			SetState();
			if (!isDown && OnToggleOver__BackingField != null)
			{
				OnToggleOver__BackingField(this);
			}
		}
	}

	private void ButtonHoverOut()
	{
		if (isHover)
		{
			isHover = false;
			SetState();
			if (!isDown && OnToggleOver__BackingField != null)
			{
				OnToggleOver__BackingField(this);
			}
		}
	}

	public void SetState()
	{
		tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(upStateGO, !isDown && !isHover);
		if (downStateGO == hoverOverStateGO)
		{
			tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(downStateGO, isDown || isHover);
			return;
		}
		tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(downStateGO, isDown);
		tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(hoverOverStateGO, isHover);
	}

	public void InternalSetUseOnReleaseInsteadOfOnUp(bool state)
	{
		useOnReleaseInsteadOfOnUp = state;
	}
}

using System;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.UI;

namespace Slugterra.UI.Popup
{
	public class PopupSlugsWarningView : PopupView
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnCheckClickEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnStartClickEvent__BackingField;

		public event Action OnCheckClickEvent
		{
			add
			{
				Action action = OnCheckClickEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnCheckClickEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnCheckClickEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnCheckClickEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnStartClickEvent
		{
			add
			{
				Action action = OnStartClickEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnStartClickEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnStartClickEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnStartClickEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		protected override void OnStart()
		{
			gameObject.SetActive(false);
		}

		private void OnBtnCheckClick()
		{
			if (OnCheckClickEvent__BackingField != null)
			{
				OnCheckClickEvent__BackingField();
			}
		}

		private void OnBtnStartClick()
		{
			if (OnStartClickEvent__BackingField != null)
			{
				OnStartClickEvent__BackingField();
			}
		}

		public void Show()
		{
			gameObject.SetActive(true);
		}

		public override void Close()
		{
			gameObject.SetActive(false);
		}
	}
}

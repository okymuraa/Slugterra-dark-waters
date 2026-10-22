using System;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.UI;

namespace Slugterra.UI.Popup
{
	public class PopupTutorialCheckOutView : PopupView
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnCloseEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnArenaEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnSurvivalEvent__BackingField;

		public event Action OnCloseEvent
		{
			add
			{
				Action action = OnCloseEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnCloseEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnCloseEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnCloseEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnArenaEvent
		{
			add
			{
				Action action = OnArenaEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnArenaEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnArenaEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnArenaEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnSurvivalEvent
		{
			add
			{
				Action action = OnSurvivalEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnSurvivalEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnSurvivalEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnSurvivalEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
		}

		public void OnBtnCloseClick()
		{
			Close();
		}

		protected override void OnStart()
		{
			gameObject.SetActive(false);
		}

		public override void Close()
		{
			if (OnCloseEvent__BackingField != null)
			{
				OnCloseEvent__BackingField();
			}
			OnCloseEvent__BackingField = null;
			gameObject.SetActive(false);
		}

		private void OnBtnSurvivalClick()
		{
			if (OnSurvivalEvent__BackingField != null)
			{
				OnSurvivalEvent__BackingField();
			}
		}

		private void OnMapArenaClick()
		{
			if (OnArenaEvent__BackingField != null)
			{
				OnArenaEvent__BackingField();
			}
		}
	}
}

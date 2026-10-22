using System;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.UI;
using Slugterra.UI.Notification;
using Slugterra.UI.Popup;
using UnityEngine;

namespace Slugterra.UI.MainMenu
{
	public class MainMenuView : UIView
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnSettingsEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnMapEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnGameServiceEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnMultiplayerEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnClearCacheEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnFaceBookEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnBtnSurvivalEvent__BackingField;

		[SerializeField]
		private PopupConfirmController confirmPopUp;

		[SerializeField]
		private BadgeIconView survivalBadge;

		private NotificationController _notificationController;

		public BadgeIconView SurvivalBadge
		{
			get
			{
				return survivalBadge;
			}
		}

		public PopupConfirmController ConfirmPopUp
		{
			get
			{
				if (confirmPopUp == null)
				{
					confirmPopUp = UnityEngine.Object.FindObjectOfType<PopupConfirmController>();
				}
				return confirmPopUp;
			}
		}

		public event Action OnSettingsEvent
		{
			add
			{
				Action action = OnSettingsEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnSettingsEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnSettingsEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnSettingsEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnMapEvent
		{
			add
			{
				Action action = OnMapEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnMapEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnMapEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnMapEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnGameServiceEvent
		{
			add
			{
				Action action = OnGameServiceEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnGameServiceEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnGameServiceEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnGameServiceEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnMultiplayerEvent
		{
			add
			{
				Action action = OnMultiplayerEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnMultiplayerEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnMultiplayerEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnMultiplayerEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnClearCacheEvent
		{
			add
			{
				Action action = OnClearCacheEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnClearCacheEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnClearCacheEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnClearCacheEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnFaceBookEvent
		{
			add
			{
				Action action = OnFaceBookEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnFaceBookEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnFaceBookEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnFaceBookEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnBtnSurvivalEvent
		{
			add
			{
				Action action = OnBtnSurvivalEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBtnSurvivalEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnBtnSurvivalEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBtnSurvivalEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		private void Awake()
		{
		}

		private void OnBtnSettingsClick()
		{
			if (OnSettingsEvent__BackingField != null)
			{
				OnSettingsEvent__BackingField();
			}
		}

		private void OnBtnMapClick()
		{
			if (OnMapEvent__BackingField != null)
			{
				OnMapEvent__BackingField();
			}
		}

		private void OnBtnGameServiceClick()
		{
			if (OnGameServiceEvent__BackingField != null)
			{
				OnGameServiceEvent__BackingField();
			}
		}

		private void OnBtnClearCacheClick()
		{
			if (OnClearCacheEvent__BackingField != null)
			{
				OnClearCacheEvent__BackingField();
			}
		}

		private void OnBtnMultiplayerClick()
		{
			if (OnMultiplayerEvent__BackingField != null)
			{
				OnMultiplayerEvent__BackingField();
			}
		}

		private void OnBtnFaceBookClick()
		{
			if (OnFaceBookEvent__BackingField != null)
			{
				OnFaceBookEvent__BackingField();
			}
		}

		private void OnBtnSurvivalClick()
		{
			if (OnBtnSurvivalEvent__BackingField != null)
			{
				OnBtnSurvivalEvent__BackingField();
			}
		}
	}
}

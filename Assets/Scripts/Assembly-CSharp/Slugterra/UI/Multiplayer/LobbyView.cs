using System;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.UI;
using Slugterra.Multiplayer.Lobby;
using UnityEngine;

namespace Slugterra.UI.Multiplayer
{
	public class LobbyView : UIView
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnBackEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnQuickMatchEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnPrivateMatchEvent__BackingField;

		[SerializeField]
		private FoesPanelController _foesPanelController;

		[SerializeField]
		private PopupChallengeController _popUpChallengeController;

		[SerializeField]
		private LobbyManager _lobbyManager;

		public PopupChallengeController PopupChallengeController
		{
			get
			{
				return _popUpChallengeController;
			}
		}

		public FoesPanelController FoesPanel
		{
			get
			{
				return _foesPanelController;
			}
		}

		public LobbyManager LobbyManager
		{
			get
			{
				return _lobbyManager;
			}
		}

		public event Action OnBackEvent
		{
			add
			{
				Action action = OnBackEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBackEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnBackEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBackEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnQuickMatchEvent
		{
			add
			{
				Action action = OnQuickMatchEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnQuickMatchEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnQuickMatchEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnQuickMatchEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnPrivateMatchEvent
		{
			add
			{
				Action action = OnPrivateMatchEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPrivateMatchEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnPrivateMatchEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPrivateMatchEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		private void OnBtnBackClick()
		{
			if (OnBackEvent__BackingField != null)
			{
				OnBackEvent__BackingField();
			}
		}

		private void OnBtnQuickMatchClick()
		{
			if (OnQuickMatchEvent__BackingField != null)
			{
				OnQuickMatchEvent__BackingField();
			}
		}

		private void OnBtnPrivateMatchClick()
		{
			if (OnPrivateMatchEvent__BackingField != null)
			{
				OnPrivateMatchEvent__BackingField();
			}
		}
	}
}

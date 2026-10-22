using System;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.UI;
using UnityEngine;

namespace Slugterra.UI.Multiplayer
{
	public class PopupChallengeView : PopupView
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnAcceptEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnCloseEvent__BackingField;

		[SerializeField]
		private PlayerInfoView _playerInfo;

		[SerializeField]
		private GameObject _acceptView;

		[SerializeField]
		private GameObject _rejectView;

		public PlayerInfoView PlayerInfo
		{
			get
			{
				return _playerInfo;
			}
		}

		public event Action OnAcceptEvent
		{
			add
			{
				Action action = OnAcceptEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnAcceptEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnAcceptEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnAcceptEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

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

		protected override void OnStart()
		{
			gameObject.SetActive(false);
		}

		private void OnBtnAcceptClick()
		{
			if (OnAcceptEvent__BackingField != null)
			{
				OnAcceptEvent__BackingField();
			}
		}

		private void OnBtnCloseClick()
		{
			if (OnCloseEvent__BackingField != null)
			{
				OnCloseEvent__BackingField();
			}
		}

		public void ConfigurePopUp(bool isMasterClient)
		{
			if (_acceptView != null)
			{
				_acceptView.SetActive(isMasterClient);
			}
			if (_rejectView != null)
			{
				_rejectView.SetActive(!isMasterClient);
			}
		}

		public override void Close()
		{
			if (OnCloseEvent__BackingField != null)
			{
				OnCloseEvent__BackingField();
			}
			OnCloseEvent__BackingField = null;
		}
	}
}

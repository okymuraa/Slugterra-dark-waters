using System;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.UI;
using UnityEngine;

namespace Slugterra.UI.Popup
{
	public class PopupFacebookGoalsView : PopupView
	{
		[Serializable]
		public struct FacebookActionStateView
		{
			public GameObject ButtonEnabled;

			public GameObject ButtonDisabled;

			public GameObject CompleteCheckbox;

			public void SetEnabledState()
			{
				ButtonEnabled.SetActive(true);
				ButtonDisabled.SetActive(false);
				CompleteCheckbox.SetActive(false);
			}

			public void SetDisabledState()
			{
				ButtonEnabled.SetActive(false);
				ButtonDisabled.SetActive(true);
				CompleteCheckbox.SetActive(false);
			}

			public void SetRewardedState()
			{
				ButtonEnabled.SetActive(false);
				ButtonDisabled.SetActive(false);
				CompleteCheckbox.SetActive(true);
			}
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnCloseEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnLoginEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnLikeEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnShareEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnInviteEvent__BackingField;

		[SerializeField]
		private GameObject _walletView;

		[SerializeField]
		private FacebookActionStateView _loginStateView;

		[SerializeField]
		private FacebookActionStateView _likeStateView;

		[SerializeField]
		private FacebookActionStateView _shareStateView;

		[SerializeField]
		private FacebookActionStateView _inviteStateView;

		public GameObject WalletView
		{
			get
			{
				return _walletView;
			}
		}

		public FacebookActionStateView LoginStateView
		{
			get
			{
				return _loginStateView;
			}
		}

		public FacebookActionStateView LikeStateView
		{
			get
			{
				return _likeStateView;
			}
		}

		public FacebookActionStateView ShareStateView
		{
			get
			{
				return _shareStateView;
			}
		}

		public FacebookActionStateView InviteStateView
		{
			get
			{
				return _inviteStateView;
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

		public event Action OnLoginEvent
		{
			add
			{
				Action action = OnLoginEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnLoginEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnLoginEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnLoginEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnLikeEvent
		{
			add
			{
				Action action = OnLikeEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnLikeEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnLikeEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnLikeEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnShareEvent
		{
			add
			{
				Action action = OnShareEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnShareEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnShareEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnShareEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnInviteEvent
		{
			add
			{
				Action action = OnInviteEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnInviteEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnInviteEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnInviteEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
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

		private void OnBtnLoginClick()
		{
			if (OnLoginEvent__BackingField != null)
			{
				OnLoginEvent__BackingField();
			}
		}

		private void OnBtnLikeClick()
		{
			if (OnLikeEvent__BackingField != null)
			{
				OnLikeEvent__BackingField();
			}
		}

		private void OnBtnShareClick()
		{
			if (OnShareEvent__BackingField != null)
			{
				OnShareEvent__BackingField();
			}
		}

		private void OnBtnInviteClick()
		{
			if (OnInviteEvent__BackingField != null)
			{
				OnInviteEvent__BackingField();
			}
		}
	}
}

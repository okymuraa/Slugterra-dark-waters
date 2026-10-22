using System;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.UI;
using UnityEngine;

namespace Slugterra.UI.Popup
{
	public class PopupMultiplayerAvailabilityView : PopupView
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnCloseEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnCampaignEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnMultiplayerEvent__BackingField;

		private bool lockBack;

		[SerializeField]
		private GameObject _multiplayerAvailable;

		[SerializeField]
		private GameObject _multiplayerUnAvailable;

		public bool LockBack
		{
			set
			{
				lockBack = value;
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

		public event Action OnCampaignEvent
		{
			add
			{
				Action action = OnCampaignEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnCampaignEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnCampaignEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnCampaignEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
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
			if (!lockBack)
			{
				if (OnCloseEvent__BackingField != null)
				{
					OnCloseEvent__BackingField();
				}
				OnCloseEvent__BackingField = null;
				gameObject.SetActive(false);
			}
		}

		private void OnBtnMultiplayerClick()
		{
			if (OnMultiplayerEvent__BackingField != null)
			{
				OnMultiplayerEvent__BackingField();
			}
		}

		private void OnBtnMapClick()
		{
			if (OnCampaignEvent__BackingField != null)
			{
				OnCampaignEvent__BackingField();
			}
		}

		public void ShowAvalablePopUp()
		{
			_multiplayerUnAvailable.SetActive(false);
			_multiplayerAvailable.SetActive(true);
			gameObject.SetActive(true);
		}

		public void ShowUnAvalablePopUp()
		{
			_multiplayerAvailable.SetActive(false);
			_multiplayerUnAvailable.SetActive(true);
			gameObject.SetActive(true);
		}
	}
}

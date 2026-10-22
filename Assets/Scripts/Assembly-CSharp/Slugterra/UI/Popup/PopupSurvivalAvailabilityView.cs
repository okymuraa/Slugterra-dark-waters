using System;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Domain.Game.UI;
using Slugterra.Scene.Game;
using UnityEngine;

namespace Slugterra.UI.Popup
{
	public class PopupSurvivalAvailabilityView : PopupView
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnCloseEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnCampaignEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnSurvivalEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnMultiplayerEvent__BackingField;

		[SerializeField]
		private GameObject _available;

		[SerializeField]
		private GameObject _unavailable;

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
			_available.SetActive(false);
			_unavailable.SetActive(true);
			if (MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(QuietLawn_2_3)).Status == LevelStatus.Completed)
			{
				_unavailable.SetActive(false);
				_available.SetActive(true);
			}
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
	}
}

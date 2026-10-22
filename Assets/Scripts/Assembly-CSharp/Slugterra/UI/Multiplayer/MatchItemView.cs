using System;
using System.Diagnostics;
using System.Threading;
using ExitGames.Client.Photon.LoadBalancing;
using Slugterra.Multiplayer.Lobby.Match;
using UnityEngine;

namespace Slugterra.UI.Multiplayer
{
	public class MatchItemView : PlayerInfoView
	{
		public const float ItemHeight = 1.29f;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnPlayEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnSendInviteEvent__BackingField;

		[SerializeField]
		private GameObject _offlineRoot;

		[SerializeField]
		private GameObject _onlineRoot;

		public event Action OnPlayEvent
		{
			add
			{
				Action action = OnPlayEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPlayEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnPlayEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPlayEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnSendInviteEvent
		{
			add
			{
				Action action = OnSendInviteEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnSendInviteEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnSendInviteEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnSendInviteEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		protected override void OnApplyModel(object model)
		{
			if (model is ExitGames.Client.Photon.LoadBalancing.RoomInfo)
			{
				ExitGames.Client.Photon.LoadBalancing.RoomInfo roomInfo = model as ExitGames.Client.Photon.LoadBalancing.RoomInfo;
				PlayerData model2 = PlayerData.FromJson((string)roomInfo.CustomProperties["OD"]);
				OnApplyModel(model2);
			}
		}

		private void OnBtnPlayClick()
		{
			if (OnPlayEvent__BackingField != null)
			{
				OnPlayEvent__BackingField();
			}
		}

		private void OnBtnSendInviteClick()
		{
			if (OnSendInviteEvent__BackingField != null)
			{
				OnSendInviteEvent__BackingField();
			}
		}
	}
}

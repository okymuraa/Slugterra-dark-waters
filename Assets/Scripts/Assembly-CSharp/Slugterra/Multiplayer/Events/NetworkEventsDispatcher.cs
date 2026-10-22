using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Slugterra.Multiplayer.Player;
using Slugterra.Multiplayer.Utils;

namespace Slugterra.Multiplayer.Events
{
	public class NetworkEventsDispatcher : PhotonSingleton<NetworkEventsDispatcher>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Action<NetworkPlayerRoot> OnPlayerReadyEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Action<NetworkPlayerRoot> OnPlayerReadyForRestartEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Action<PhotonPlayer, DisconnectReason> OnPlayerDisconnectedEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Action<double> OnDuelStartEvent__BackingField;

		private List<int> _disconnectedPlayers = new List<int>(10);

		public static event Action<NetworkPlayerRoot> OnPlayerReadyEvent
		{
			add
			{
				Action<NetworkPlayerRoot> action = OnPlayerReadyEvent__BackingField;
				Action<NetworkPlayerRoot> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPlayerReadyEvent__BackingField, (Action<NetworkPlayerRoot>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<NetworkPlayerRoot> action = OnPlayerReadyEvent__BackingField;
				Action<NetworkPlayerRoot> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPlayerReadyEvent__BackingField, (Action<NetworkPlayerRoot>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public static event Action<NetworkPlayerRoot> OnPlayerReadyForRestartEvent
		{
			add
			{
				Action<NetworkPlayerRoot> action = OnPlayerReadyForRestartEvent__BackingField;
				Action<NetworkPlayerRoot> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPlayerReadyForRestartEvent__BackingField, (Action<NetworkPlayerRoot>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<NetworkPlayerRoot> action = OnPlayerReadyForRestartEvent__BackingField;
				Action<NetworkPlayerRoot> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPlayerReadyForRestartEvent__BackingField, (Action<NetworkPlayerRoot>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public static event Action<PhotonPlayer, DisconnectReason> OnPlayerDisconnectedEvent
		{
			add
			{
				Action<PhotonPlayer, DisconnectReason> action = OnPlayerDisconnectedEvent__BackingField;
				Action<PhotonPlayer, DisconnectReason> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPlayerDisconnectedEvent__BackingField, (Action<PhotonPlayer, DisconnectReason>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<PhotonPlayer, DisconnectReason> action = OnPlayerDisconnectedEvent__BackingField;
				Action<PhotonPlayer, DisconnectReason> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPlayerDisconnectedEvent__BackingField, (Action<PhotonPlayer, DisconnectReason>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public static event Action<double> OnDuelStartEvent
		{
			add
			{
				Action<double> action = OnDuelStartEvent__BackingField;
				Action<double> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnDuelStartEvent__BackingField, (Action<double>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<double> action = OnDuelStartEvent__BackingField;
				Action<double> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnDuelStartEvent__BackingField, (Action<double>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		protected override void OnCreate()
		{
			PhotonNetwork.OnEventCall = (PhotonNetwork.EventCallback)Delegate.Combine(PhotonNetwork.OnEventCall, new PhotonNetwork.EventCallback(OnEvent));
		}

		private void OnEvent(byte eventcode, object content, int senderid)
		{
			switch (eventcode)
			{
			case 0:
				OnPlayerReady(content, senderid);
				break;
			case 1:
				OnPlayerReadyForRestart(content, senderid);
				break;
			case 2:
				OnPlayerLeftGame(content, senderid);
				break;
			case 3:
				OnDuelStart(content, senderid);
				break;
			}
		}

		public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
		{
			PlayerDisconnected(otherPlayer, DisconnectReason.Unknown);
		}

		private void OnPlayerReady(object content, int senderid)
		{
			if (OnPlayerReadyEvent__BackingField != null)
			{
				PhotonView photonView = PhotonView.Find((int)content);
				NetworkPlayerRoot component = photonView.gameObject.GetComponent<NetworkPlayerRoot>();
				OnPlayerReadyEvent__BackingField(component);
			}
		}

		private void OnPlayerReadyForRestart(object content, int senderid)
		{
			if (OnPlayerReadyForRestartEvent__BackingField != null)
			{
				PhotonView photonView = PhotonView.Find((int)content);
				NetworkPlayerRoot component = photonView.gameObject.GetComponent<NetworkPlayerRoot>();
				OnPlayerReadyForRestartEvent__BackingField(component);
			}
		}

		private void OnPlayerLeftGame(object content, int senderid)
		{
			PhotonPlayer otherPlayer = PhotonPlayer.Find(senderid);
			PlayerDisconnected(otherPlayer, DisconnectReason.LeftGame);
		}

		private void PlayerDisconnected(PhotonPlayer otherPlayer, DisconnectReason reason)
		{
			if (OnPlayerDisconnectedEvent__BackingField != null && !_disconnectedPlayers.Contains(otherPlayer.ID))
			{
				OnPlayerDisconnectedEvent__BackingField(otherPlayer, reason);
			}
		}

		private void OnDuelStart(object content, int senderid)
		{
			if (OnDuelStartEvent__BackingField != null)
			{
				OnDuelStartEvent__BackingField((double)content);
			}
		}

		public void RaisePlayerReady(NetworkPlayerRoot player)
		{
			PhotonNetwork.RaiseEvent(0, player.photonView.viewID, true, RaiseEventOptions.Default);
			if (OnPlayerReadyEvent__BackingField != null)
			{
				OnPlayerReadyEvent__BackingField(player);
			}
		}

		public void RaisePlayerReadyForRestart(NetworkPlayerRoot player)
		{
			PhotonNetwork.RaiseEvent(1, player.photonView.viewID, true, RaiseEventOptions.Default);
			if (OnPlayerReadyForRestartEvent__BackingField != null)
			{
				OnPlayerReadyForRestartEvent__BackingField(player);
			}
		}

		public void RaisePlayerLeftGame()
		{
			PhotonNetwork.RaiseEvent(2, null, true, RaiseEventOptions.Default);
			if (OnPlayerDisconnectedEvent__BackingField != null)
			{
				OnPlayerDisconnectedEvent__BackingField(PhotonNetwork.player, DisconnectReason.LeftGame);
			}
		}

		public void RaiseDuelStart(double startTime)
		{
			PhotonNetwork.RaiseEvent(3, startTime, true, RaiseEventOptions.Default);
			if (OnDuelStartEvent__BackingField != null)
			{
				OnDuelStartEvent__BackingField(startTime);
			}
		}
	}
}

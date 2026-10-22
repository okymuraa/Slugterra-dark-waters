using Photon;

namespace Slugterra.Multiplayer
{
	public class NetworkConnector : PunBehaviour
	{
		private static TypedLobby _lobby;

		public static bool IsConnected
		{
			get
			{
				return !PhotonNetwork.offlineMode && PhotonNetwork.connectionState == ConnectionState.Connected;
			}
		}

		public static TypedLobby Lobby
		{
			get
			{
				if (_lobby == null)
				{
					_lobby = new TypedLobby("SlugterraLobby", LobbyType.SqlLobby);
				}
				return _lobby;
			}
		}

		public void Connect()
		{
			if (PhotonNetwork.connectionState == ConnectionState.Disconnected)
			{
				PhotonNetwork.automaticallySyncScene = true;
				PhotonNetwork.autoJoinLobby = false;
				PhotonNetwork.sendRate = 25;
				PhotonNetwork.sendRateOnSerialize = 25;
				PhotonNetwork.ConnectUsingSettings("v4.2");
			}
		}

		public void Disconnect()
		{
			if (PhotonNetwork.connected)
			{
				PhotonNetwork.Disconnect();
			}
		}

		public override void OnConnectedToMaster()
		{
			PhotonNetwork.JoinLobby(Lobby);
		}

		private void OnDestroy()
		{
			Disconnect();
		}
	}
}

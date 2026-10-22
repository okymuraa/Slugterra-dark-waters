namespace ExitGames.Client.Photon.LoadBalancing
{
	public enum ClientState
	{
		Uninitialized = 0,
		ConnectingToMasterserver = 1,
		ConnectedToMaster = 2,
		Queued = 3,
		Authenticated = 4,
		JoinedLobby = 5,
		DisconnectingFromMasterserver = 6,
		ConnectingToGameserver = 7,
		ConnectedToGameserver = 8,
		Joining = 9,
		Joined = 10,
		Leaving = 11,
		Left = 12,
		DisconnectingFromGameserver = 13,
		QueuedComingFromGameserver = 14,
		Disconnecting = 15,
		Disconnected = 16,
		ConnectingToNameServer = 17,
		ConnectedToNameServer = 18,
		Authenticating = 19,
		DisconnectingFromNameServer = 20
	}
}

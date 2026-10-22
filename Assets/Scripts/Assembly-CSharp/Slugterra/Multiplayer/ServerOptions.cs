using ExitGames.Client.Photon;

namespace Slugterra.Multiplayer
{
	public static class ServerOptions
	{
		public static void CreateRoom(string name)
		{
			RoomOptions roomOptions = new RoomOptions();
			roomOptions.maxPlayers = 2;
			roomOptions.customRoomProperties = new Hashtable();
			roomOptions.customRoomProperties.Add("MP", "ScorchedSparksMultiplayer");
			roomOptions.customRoomProperties.Add("OD", NetworkManager.LocalPlayerData.ToJson());
			roomOptions.customRoomPropertiesForLobby = new string[2] { "MP", "OD" };
			PhotonNetwork.CreateRoom(name, roomOptions, NetworkConnector.Lobby);
		}
	}
}

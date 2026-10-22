using System.Collections.Generic;
using AppsMinistry.Core;
using Slugterra.Configuration;
using Slugterra.Multiplayer.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Slugterra.Multiplayer
{
	public class ServerManager : PhotonSingleton<ServerManager>
	{
		private readonly List<string> MPLevels = new List<string> { "Lobby", "ScorchedSparksMultiplayer" };

		[SerializeField]
		private NetworkConnector _connector;

		protected override void OnCreate()
		{
			ConfigPhotonPrefabs();
			PhotonNetwork.playerName = "Server";
		}

		private void Start()
		{
			_connector.Connect();
		}

		private void ConfigPhotonPrefabs()
		{
			Dictionary<string, GameObject> dictionary = new Dictionary<string, GameObject>();
			dictionary.Add("Player", Resources.Load<GameObject>("Prefabs/Multiplayer/Player/Player"));
			List<SlugConfig> slugConfigs = MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugConfigs();
			foreach (SlugConfig item in slugConfigs)
			{
				dictionary[item.slugName] = Resources.Load<GameObject>(item.gameNetworkPrefab);
			}
			PhotonNetwork.UsePrefabCache = true;
			PhotonNetwork.PrefabCache = dictionary;
			NetworkingPeer.UsePrefabCache = true;
			NetworkingPeer.PrefabCache = dictionary;
		}

		public override void OnJoinedRoom()
		{
			if (PhotonNetwork.room.customProperties.ContainsKey("QM"))
			{
				PhotonNetwork.LoadLevel((string)PhotonNetwork.room.customProperties["MP"]);
			}
		}

		public override void OnLeftRoom()
		{
			base.OnLeftRoom();
			Debug.Log("NetworkManager::OnLeftRoom ");
		}

		public override void OnMasterClientSwitched(PhotonPlayer newMasterClient)
		{
			base.OnMasterClientSwitched(newMasterClient);
			Debug.Log("NetworkManager::OnMasterClientSwitched " + newMasterClient.ID);
		}

		public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
		{
			base.OnPhotonPlayerConnected(newPlayer);
			if (PhotonNetwork.room.playerCount == PhotonNetwork.room.maxPlayers)
			{
				Debug.Log("NetworkManager OnPhotonPlayerConnected roomName: " + PhotonNetwork.room.name + " Ready for Game ");
			}
		}

		public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
		{
			base.OnPhotonPlayerDisconnected(otherPlayer);
			Debug.Log("NetworkManager OnPhotonPlayerDisconnected otherPlayer " + otherPlayer.ID);
		}

		private void OnLevelWasLoaded(int level)
		{
			if (!MPLevels.Contains(SceneManager.GetActiveScene().name))
			{
				Object.DestroyImmediate(gameObject);
			}
		}

		public override void OnConnectedToMaster()
		{
			string text = "Server";
			ServerOptions.CreateRoom(text);
		}

		private void OnApplicationQuit()
		{
			PhotonNetwork.Disconnect();
		}
	}
}

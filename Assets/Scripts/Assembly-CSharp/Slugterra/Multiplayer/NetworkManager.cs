using System.Collections.Generic;
using AppsMinistry.Core;
using AppsMinistry.UI;
using ExitGames.Client.Photon;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using Slugterra.Multiplayer.Lobby;
using Slugterra.Multiplayer.Lobby.Match;
using Slugterra.Multiplayer.Utils;
using Slugterra.UI.Multiplayer;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Slugterra.Multiplayer
{
	public class NetworkManager : PhotonSingleton<NetworkManager>
	{
		private readonly List<string> MPLevels = new List<string> { "Lobby", "ScorchedSparksMultiplayer" };

		private static PlayerData _localPlayerData;

		[SerializeField]
		private NetworkConnector _connector;

		[SerializeField]
		private LobbyManager lobbyManager;

		private PopupChallengeView _popupConfirm;

		public static PlayerData LocalPlayerData
		{
			get
			{
				if (_localPlayerData == null)
				{
					_localPlayerData = CreateLocalPlayerData();
				}
				return _localPlayerData;
			}
		}

		protected override void OnCreate()
		{
			Object.DontDestroyOnLoad(gameObject);
			ConfigPhotonPrefabs();
			PhotonNetwork.playerName = LocalPlayerData.Name;
			Hashtable hashtable = new Hashtable();
			hashtable.Add("PD", LocalPlayerData.ToJson());
			PhotonNetwork.SetPlayerCustomProperties(hashtable);
		}

		private void Start()
		{
			_connector.Connect();
			_popupConfirm = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupChallengeView>();
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

		private static PlayerData CreateLocalPlayerData()
		{
			PlayerData playerData = new PlayerData();
			playerData.Name = GetPlayerName();
			playerData.Rank = 0;
			playerData.AvatarId = 0;
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			playerData.AttackSlug = new PlayerDataSlug(missionSetting.AttackSlug);
			playerData.ControlSlug = new PlayerDataSlug(missionSetting.DefenceSlug);
			playerData.SpecialSlug = new PlayerDataSlug(missionSetting.SpecialSlug);
			return playerData;
		}

		public static string GetPlayerName()
		{
			string empty = string.Empty;
			empty = DonDestroyMonoSingleton<GameServiceContext>.Instance.PlayerAlias;
			if (string.IsNullOrEmpty(empty))
			{
				empty = "unknown" + Random.Range(1, 1000);
			}
			return empty;
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
				Hashtable customProperties = newPlayer.customProperties;
				PlayerData model = PlayerData.FromJson((string)customProperties["PD"]);
				_popupConfirm.PlayerInfo.OnApplyModel(model);
				_popupConfirm.ConfigurePopUp(PhotonNetwork.isMasterClient);
				_popupConfirm.gameObject.SetActive(true);
			}
		}

		public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
		{
			base.OnPhotonPlayerDisconnected(otherPlayer);
			Debug.Log("NetworkManager OnPhotonPlayerDisconnected otherPlayer " + otherPlayer.ID);
			PhotonNetwork.LeaveRoom();
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
			if (!PhotonSingleton<Matchmaking>.Instance.IsMatchmakingStarted)
			{
				lobbyManager.Connect();
				string message = DonDestroyMonoSingleton<GameServiceContext>.Instance.PlayerIdentifier();
				Debug.Log(message);
				ServerOptions.CreateRoom(message);
			}
		}

		private void OnApplicationQuit()
		{
			PhotonNetwork.Disconnect();
		}
	}
}

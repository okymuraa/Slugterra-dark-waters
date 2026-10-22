using AppsMinistry.Core;
using AppsMinistry.UI;
using ExitGames.Client.Photon;
using ExitGames.Client.Photon.LoadBalancing;
using Slugterra.Multiplayer.Lobby.Match;
using Slugterra.Multiplayer.Utils;
using Slugterra.UI.Multiplayer;
using UnityEngine;

namespace Slugterra.Multiplayer
{
	public class Matchmaking : PhotonSingleton<Matchmaking>
	{
		[SerializeField]
		private PopupChallengeView _popupConfirm;

		private ExitGames.Client.Photon.LoadBalancing.RoomInfo _roomInfo;

		private bool startQuickMatch = true;

		private bool _connectToRoom;

		private string _roomId = string.Empty;

		public bool IsMatchmakingStarted { get; private set; }

		public MatchmakingType MatchmakingType { get; private set; }

		private void Start()
		{
			_popupConfirm = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupChallengeView>();
		}

		public void StartMatchmaking(MatchmakingType type = MatchmakingType.Random, ExitGames.Client.Photon.LoadBalancing.RoomInfo roomInfo = null)
		{
			if (!IsMatchmakingStarted)
			{
				MatchmakingType = type;
				IsMatchmakingStarted = true;
				if (MatchmakingType == MatchmakingType.Random)
				{
					StartRandomMatchmaking();
				}
				if (MatchmakingType == MatchmakingType.OneVsOne && PhotonNetwork.inRoom)
				{
					_roomInfo = roomInfo;
					_roomId = roomInfo.Name;
					_connectToRoom = PhotonNetwork.LeaveRoom();
				}
			}
		}

		private void StartRandomMatchmaking()
		{
			if (PhotonNetwork.inRoom)
			{
				startQuickMatch = PhotonNetwork.LeaveRoom();
			}
		}

		public override void OnConnectedToMaster()
		{
			base.OnConnectedToMaster();
			if (startQuickMatch)
			{
				RoomOptions roomOptions = new RoomOptions();
				roomOptions.MaxPlayers = 2;
				roomOptions.CustomRoomProperties = new Hashtable();
				roomOptions.CustomRoomProperties.Add("MP", "0_DesertBase_1_FirstView");
				roomOptions.CustomRoomProperties.Add("OD", NetworkManager.LocalPlayerData.ToJson());
				roomOptions.CustomRoomProperties.Add("QM", "true");
				roomOptions.CustomRoomPropertiesForLobby = new string[3] { "MP", "OD", "QM" };
				PhotonNetwork.JoinOrCreateRoom("RandomRoom", roomOptions, NetworkConnector.Lobby);
			}
			if (_connectToRoom)
			{
				PhotonNetwork.JoinRoom(_roomId);
				PlayerData model = PlayerData.FromJson((string)_roomInfo.CustomProperties["OD"]);
				_popupConfirm.PlayerInfo.OnApplyModel(model);
				_popupConfirm.ConfigurePopUp(PhotonNetwork.isMasterClient);
				_popupConfirm.gameObject.SetActive(true);
			}
			_connectToRoom = false;
			startQuickMatch = false;
		}

		private void CreateRandomRoom()
		{
			ServerOptions.CreateRoom(null);
		}

		private void OnPhotonRandomJoinFailed()
		{
			if (IsMatchmakingStarted && MatchmakingType == MatchmakingType.Random)
			{
				CreateRandomRoom();
			}
		}
	}
}

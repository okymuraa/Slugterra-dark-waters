using System;
using System.Collections.Generic;
using AppsMinistry.Core;
using AppsMinistry.UI;
using ExitGames.Client.Photon.LoadBalancing;
using Slugterra.App;
using Slugterra.Domain.App;
using Slugterra.Multiplayer;
using Slugterra.Multiplayer.Lobby;
using Slugterra.Multiplayer.Utils;
using Slugterra.Scene;
using Slugterra.Scene.Multiplayer;

namespace Slugterra.UI.Multiplayer
{
	public class LobbyController : UIController<LobbyView>
	{
		private PrivateGameCodeView _popupPivateGameCode;

		protected override void OnStart()
		{
			base.OnStart();
			InvokeRepeating("RefreshMatchList", 0.2f, 10f);
		}

		protected override void _OnEnable()
		{
			_popupPivateGameCode = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PrivateGameCodeView>();
			View.OnBackEvent += OnBack;
			View.OnQuickMatchEvent += CreateMatch;
			View.OnPrivateMatchEvent += PrivateMatch;
			LobbyManager lobbyManager = View.LobbyManager;
			lobbyManager.updateRoomsListAction = (Action<Dictionary<string, ExitGames.Client.Photon.LoadBalancing.RoomInfo>>)Delegate.Combine(lobbyManager.updateRoomsListAction, new Action<Dictionary<string, ExitGames.Client.Photon.LoadBalancing.RoomInfo>>(View.FoesPanel.FillModel));
		}

		protected override void _OnDisable()
		{
			View.OnBackEvent -= OnBack;
			View.OnQuickMatchEvent -= CreateMatch;
			View.OnPrivateMatchEvent -= PrivateMatch;
			LobbyManager lobbyManager = View.LobbyManager;
			lobbyManager.updateRoomsListAction = (Action<Dictionary<string, ExitGames.Client.Photon.LoadBalancing.RoomInfo>>)Delegate.Remove(lobbyManager.updateRoomsListAction, new Action<Dictionary<string, ExitGames.Client.Photon.LoadBalancing.RoomInfo>>(View.FoesPanel.FillModel));
		}

		private void OnBack()
		{
			SceneLoader.LoadPrevScene();
		}

		protected override void OnReleaseResource()
		{
			base.OnReleaseResource();
		}

		private void CreateMatch()
		{
			AppModel model = MonoSingleton<AppController>.Instance.Model;
			model.MissionSetting.Mission = typeof(ScorchedSparksMultiplayer);
			PhotonSingleton<Matchmaking>.Instance.StartMatchmaking();
		}

		private void PrivateMatch()
		{
			_popupPivateGameCode.gameObject.SetActive(true);
		}

		private void DebugBack()
		{
		}

		private void RefreshMatchList()
		{
		}
	}
}

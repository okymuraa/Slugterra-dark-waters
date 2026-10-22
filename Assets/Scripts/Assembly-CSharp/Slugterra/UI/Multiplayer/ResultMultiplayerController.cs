using AppsMinistry.Core;
using AppsMinistry.Core.Facebook;
using AppsMinistry.Core.Input;
using AppsMinistry.Domain;
using AppsMinistry.UI;
using Slugterra.Scene;
using Slugterra.UI.Game;
using Slugterra.UI.Popup;
using UnityEngine;

namespace Slugterra.UI.Multiplayer
{
	public class ResultMultiplayerController : UIController<ResultMultiplayerView>
	{
		private PopupConfirmView _popup;

		private GameController _gameController;

		protected override void OnStart()
		{
			base.OnStart();
			_popup = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupConfirmView>();
		}

		protected override void _OnEnable()
		{
			if (_gameController == null)
			{
				_gameController = Object.FindObjectOfType<GameController>();
			}
			else
			{
				PhotonNetwork.Disconnect();
			}
			View.OnSharePhotoEvent += OnSharePhoto;
			View.OnShareVideoEvent += OnShareVideo;
			View.OnNextEvent += OnNext;
			View.OnHelpEvent += OnHelp;
			View.OnBtnLeaderboardEvent += OnLeaderboard;
			DonDestroyMonoSingleton<InputManager>.Instance.BackBtnEvent += OnMainMenu;
			FacebookController.OnShared += OnFeedShare;
		}

		protected override void _OnDisable()
		{
			View.OnSharePhotoEvent -= OnSharePhoto;
			View.OnShareVideoEvent -= OnShareVideo;
			View.OnNextEvent -= OnNext;
			View.OnHelpEvent -= OnHelp;
			View.OnBtnLeaderboardEvent -= OnLeaderboard;
			DonDestroyMonoSingleton<InputManager>.Instance.BackBtnEvent -= OnMainMenu;
			FacebookController.OnShared -= OnFeedShare;
		}

		private void OnSharePhoto()
		{
			DonDestroyMonoSingleton<FacebookController>.Instance.FeedShare();
		}

		private void OnShareVideo()
		{
			View.SetButtonsState(false);
		}

		private void OnNext()
		{
			SceneLoader.LoadScene<MultiplayerSceneController>(true);
		}

		private void OnLeaderboard()
		{
			DonDestroyMonoSingleton<GameServiceContext>.Instance.ShowGameCenterViewController(GameServiceViewControllerState.Leaderboards, GameServiceLeaderboardTimeScope.AllTime, "CgkIjZj4j6kZEAIQHQ");
		}

		private void OnHelp()
		{
			PopupTvOSManualView registeredPopup = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupTvOSManualView>();
			registeredPopup.ShowHelp();
		}

		private void OnMainMenu()
		{
			SceneLoader.LoadScene<MultiplayerSceneController>(true);
		}

		private void OnFeedShare()
		{
		}
	}
}

using AppsMinistry.Core;
using AppsMinistry.Core.Facebook;
using AppsMinistry.Core.Input;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Domain.Game.UI;
using Slugterra.Scene;
using Slugterra.Scene.Game;
using Slugterra.Scene.Multiplayer;
using Slugterra.UI.Popup;
using UnityEngine;

namespace Slugterra.UI.Game
{
	public class WinController : UIController<WinView>
	{
		private PopupConfirmView _popup;

		private GameController _gameController;

		protected override void OnCreate()
		{
			base.OnCreate();
		}

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
			if (!(_gameController != null) || _gameController.Model.GameWon)
			{
			}
			View.OnSharePhotoEvent += OnSharePhoto;
			View.OnShareVideoEvent += OnShareVideo;
			View.OnNextEvent += OnNext;
			View.OnHelpEvent += OnHelp;
			DonDestroyMonoSingleton<InputManager>.Instance.BackBtnEvent += OnMainMenu;
			FacebookController.OnShared += OnFeedShare;
		}

		protected override void _OnDisable()
		{
			View.OnSharePhotoEvent -= OnSharePhoto;
			View.OnShareVideoEvent -= OnShareVideo;
			View.OnNextEvent -= OnNext;
			View.OnHelpEvent -= OnHelp;
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
			LevelItem currentGameLevel = MonoSingleton<AppController>.Instance.GetCurrentGameLevel();
			if (currentGameLevel != null && currentGameLevel.Scene == typeof(ShaneTutor_0_1))
			{
				SceneLoader.LoadScene<MapSceneController>();
			}
			else if (currentGameLevel != null && currentGameLevel.Scene == typeof(TutorialMultiplayer))
			{
				SceneLoader.LoadScene<MultiplayerSceneController>(true);
			}
			else
			{
				SceneLoader.LoadFromPrev<MapSceneController>(true);
			}
		}

		private void OnHelp()
		{
			PopupTvOSManualView registeredPopup = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupTvOSManualView>();
			registeredPopup.ShowHelp();
		}

		private void OnMainMenu()
		{
			_popup.Show(string.Empty, () =>
			{
				SceneLoader.LoadScene<MainSceneController>();
			});
		}

		private void OnFeedShare()
		{
		}
	}
}

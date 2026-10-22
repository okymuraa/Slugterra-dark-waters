using System;
using AppsMinistry.Core;
using AppsMinistry.Core.Facebook;
using AppsMinistry.Core.Input;
using AppsMinistry.Core.Sound;
using AppsMinistry.Domain;
using AppsMinistry.Scene;
using AppsMinistry.UI;
using Slugterra.Game.Survival;
using Slugterra.Scene;
using Slugterra.UI.Popup;
using UnityEngine;

namespace Slugterra.UI.Game
{
	public class SurvivalController : UIController<SurvivalView>
	{
		private PopupConfirmView _popup;

		private GameController _gameController;

		protected override void OnCreate()
		{
			base.OnCreate();
			string leaderBoardID = GetLeaderBoardID();
			if (!string.IsNullOrEmpty(leaderBoardID))
			{
				MonoSingleton<SurvivalManager>.Instance.Initialize();
			}
		}

		protected override void OnStart()
		{
			base.OnStart();
			_popup = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupConfirmView>();
			MonoSingleton<SoundManager>.Instance.Play("SuccessFinishLevel");
		}

		protected override void _OnEnable()
		{
			View.OnSharePhotoEvent += OnSharePhoto;
			View.OnShareVideoEvent += OnShareVideo;
			View.OnNextEvent += OnNext;
			View.OnMainMenuEvent += OnMainMenu;
			View.OnRestartEvent += OnRestart;
			View.OnHelpEvent += OnHelp;
			View.OnBtnChallengeEvent += OnBtnChallenge;
			if (_gameController == null)
			{
				_gameController = UnityEngine.Object.FindObjectOfType<GameController>();
			}
			View.OnBtnGameServiceEvent += OnGameService;
			DonDestroyMonoSingleton<InputManager>.Instance.BackBtnEvent += OnMainMenu;
		}

		public static string GetLeaderBoardID()
		{
			SlugterraSceneController slugterraSceneController = UnityEngine.Object.FindObjectOfType<SlugterraSceneController>();
			LeaderBoardIDAttribute leaderBoardIDAttribute = Attribute.GetCustomAttribute(slugterraSceneController.GetType(), typeof(LeaderBoardIDAttribute)) as LeaderBoardIDAttribute;
			if (leaderBoardIDAttribute != null && !string.IsNullOrEmpty(leaderBoardIDAttribute.LeaderBoardId))
			{
				return leaderBoardIDAttribute.LeaderBoardId;
			}
			return string.Empty;
		}

		public static string GetSurvivaBestScoresKey()
		{
			string leaderBoardID = GetLeaderBoardID();
			return "survivalBestScore" + leaderBoardID;
		}

		public static bool IsSurvival(Type level)
		{
			LeaderBoardIDAttribute leaderBoardIDAttribute = Attribute.GetCustomAttribute(level, typeof(LeaderBoardIDAttribute)) as LeaderBoardIDAttribute;
			return leaderBoardIDAttribute != null;
		}

		protected override void _OnDisable()
		{
			View.OnSharePhotoEvent -= OnSharePhoto;
			View.OnShareVideoEvent -= OnShareVideo;
			View.OnNextEvent -= OnNext;
			View.OnMainMenuEvent -= OnMainMenu;
			View.OnRestartEvent -= OnRestart;
			View.OnHelpEvent -= OnHelp;
			View.OnBtnChallengeEvent -= OnBtnChallenge;
			View.OnBtnGameServiceEvent -= OnGameService;
			DonDestroyMonoSingleton<InputManager>.Instance.BackBtnEvent -= OnMainMenu;
		}

		private void OnSharePhoto()
		{
			DonDestroyMonoSingleton<FacebookController>.Instance.FeedShare();
		}

		private void OnShareVideo()
		{
			View.ShareVideo.SetActive(false);
			View.SharePhoto.SetActive(true);
		}

		private void OnNext()
		{
			DonDestroyMonoSingleton<GameServiceContext>.Instance.SelectedChallenge = null;
			SceneLoader.LoadScene<SurvivalMapSceneController>();
		}

		private void OnMainMenu()
		{
			DonDestroyMonoSingleton<GameServiceContext>.Instance.SelectedChallenge = null;
			SceneLoader.LoadScene<MainSceneController>();
		}

		private void OnRestart()
		{
			_popup.Show(string.Empty, () =>
			{
				SceneLoader.ReloadScene();
			});
		}

		private void OnHelp()
		{
			PopupTvOSManualView registeredPopup = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupTvOSManualView>();
			registeredPopup.ShowHelp();
		}

		private void OnGameService()
		{
			DonDestroyMonoSingleton<GameServiceContext>.Instance.ShowGameCenterViewController(GameServiceViewControllerState.Leaderboards, GameServiceLeaderboardTimeScope.AllTime, null);
		}

		private void OnBtnChallenge()
		{
			string text = GetLeaderBoardID();
			if (string.IsNullOrEmpty(text))
			{
				text = null;
			}
			DonDestroyMonoSingleton<GameServiceContext>.Instance.ShowGameCenterViewController(GameServiceViewControllerState.Leaderboards, GameServiceLeaderboardTimeScope.AllTime, text);
		}
	}
}

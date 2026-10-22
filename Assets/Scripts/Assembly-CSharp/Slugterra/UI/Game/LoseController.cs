using AppsMinistry.Core;
using AppsMinistry.Core.Ads;
using AppsMinistry.Core.Input;
using AppsMinistry.Plugins;
using AppsMinistry.UI;
using Slugterra.Scene;
using Slugterra.UI.Popup;
using UnityEngine;

namespace Slugterra.UI.Game
{
	public class LoseController : UIController<LoseView>
	{
		private PopupConfirmView _popupConfirm;

		private GameController _gameController;

		protected override void OnStart()
		{
			_popupConfirm = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupConfirmView>();
		}

		protected override void _OnEnable()
		{
			if (_gameController == null)
			{
				_gameController = Object.FindObjectOfType<GameController>();
			}
			if (_gameController != null && _gameController.Model.GameFailed)
			{
				DonDestroyMonoSingleton<AnalyticsManager>.Instance.LevelFailed();
			}
			View.OnMapEvent += OnMap;
			View.OnMainMenuEvent += OnMainMenu;
			View.OnRestartEvent += OnRestart;
			View.OnNextEvent += OnNext;
			View.OnHelpEvent += OnHelp;
			DonDestroyMonoSingleton<InputManager>.Instance.BackBtnEvent += OnMainMenu;
		}

		protected override void _OnDisable()
		{
			View.OnMapEvent -= OnMap;
			View.OnMainMenuEvent -= OnMainMenu;
			View.OnRestartEvent -= OnRestart;
			DonDestroyMonoSingleton<InputManager>.Instance.BackBtnEvent -= OnMainMenu;
			View.OnNextEvent -= OnNext;
			View.OnHelpEvent -= OnHelp;
		}

		private void OnMap()
		{
			_popupConfirm.Show(string.Empty, () =>
			{
				if (DonDestroyMonoSingleton<AdsManager>.Instance.VideoAdsAvailable || DonDestroyMonoSingleton<AdsManager>.Instance.InterstitialAdsAvailable)
				{
					DonDestroyMonoSingleton<AdsManager>.Instance.ShowAds(string.Empty);
				}
				SceneLoader.LoadFromPrev<MapSceneController>(true);
			});
		}

		private void OnMainMenu()
		{
			_popupConfirm.Show(string.Empty, () =>
			{
				if (DonDestroyMonoSingleton<AdsManager>.Instance.VideoAdsAvailable || DonDestroyMonoSingleton<AdsManager>.Instance.InterstitialAdsAvailable)
				{
					DonDestroyMonoSingleton<AdsManager>.Instance.ShowAds(string.Empty);
				}
				SceneLoader.LoadScene<MainSceneController>();
			});
		}

		private void OnRestart()
		{
			_popupConfirm.Show(string.Empty, () =>
			{
				SceneLoader.ReloadScene();
			});
		}

		private void OnNext()
		{
			SceneLoader.LoadScene<MainSceneController>(true);
		}

		private void OnHelp()
		{
			PopupTvOSManualView registeredPopup = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupTvOSManualView>();
			registeredPopup.ShowHelp();
		}
	}
}

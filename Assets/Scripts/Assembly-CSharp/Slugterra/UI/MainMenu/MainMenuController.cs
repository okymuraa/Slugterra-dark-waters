using System.Collections.Generic;
using System.IO;
using AppsMinistry.Core;
using AppsMinistry.Core.Facebook;
using AppsMinistry.Domain;
using AppsMinistry.UI;
using AppsMinistry.Utils;
using Slugterra.App;
using Slugterra.Domain.App;
using Slugterra.Domain.Game.UI;
using Slugterra.InAppPurchases;
using Slugterra.Scene;
using Slugterra.Scene.Game;
using Slugterra.UI.Popup;
using UnityEngine;

namespace Slugterra.UI.MainMenu
{
	public class MainMenuController : UIController<MainMenuView>
	{
		private const float PLAY_TAUNT_TIME = 6f;

		[SerializeField]
		private List<Animator> _assistantAnimators;

		private float _nextPlayTaunt;

		private bool _isCheckChallengesBusy;

		protected override void OnStart()
		{
			base.OnStart();
			_nextPlayTaunt = Time.time + Random.Range(3f, 6f);
			DonDestroyMonoSingleton<SubscriptionManager>.Instance.Init();
			MonoSingleton<AppController>.Instance.Model.MissionSetting.ClearOfferedItems();
		}

		protected override void _OnEnable()
		{
			View.OnSettingsEvent += OnSettings;
			View.OnMapEvent += OnMap;
			View.OnGameServiceEvent += OnGameService;
			View.OnMultiplayerEvent += OnMultiplayer;
			View.OnClearCacheEvent += OnClearCache;
			View.OnFaceBookEvent += OnFaceBook;
			View.OnBtnSurvivalEvent += OnSurvival;
			DonDestroyMonoSingleton<GameServiceContext>.Instance.OnChallengesLoaded += OnChallengesLoadedEvent;
		}

		protected override void _OnDisable()
		{
			if (DonDestroyMonoSingleton<GameServiceContext>.IsAlive)
			{
				DonDestroyMonoSingleton<GameServiceContext>.Instance.OnChallengesLoaded -= OnChallengesLoadedEvent;
			}
			View.OnSettingsEvent -= OnSettings;
			View.OnMapEvent -= OnMap;
			View.OnGameServiceEvent -= OnGameService;
			View.OnMultiplayerEvent -= OnMultiplayer;
			View.OnClearCacheEvent -= OnClearCache;
			View.OnFaceBookEvent -= OnFaceBook;
			View.OnBtnSurvivalEvent -= OnSurvival;
		}

		private void CheckChallenges()
		{
			if (!_isCheckChallengesBusy)
			{
				_isCheckChallengesBusy = true;
				DonDestroyMonoSingleton<GameServiceContext>.Instance.LoadChallengesList();
			}
		}

		private void OnChallengesLoadedEvent(List<GameServiceChallenge> gameCenterChallenges)
		{
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();
			if (_assistantAnimators != null && _assistantAnimators.Count != 0 && _nextPlayTaunt < Time.time)
			{
				int index = Random.Range(0, _assistantAnimators.Count);
				if (_assistantAnimators[index] != null)
				{
					_assistantAnimators[index].SetTrigger("OnTaunt");
				}
				_nextPlayTaunt = Time.time + Random.Range(3f, 6f);
			}
		}

		private void OnSettings()
		{
			SceneLoader.LoadScene<SettingsSceneController>();
		}

		private void OnMap()
		{
			PlayerPrefs.SetInt(typeof(ShaneTutor_0_1).Name, 1);
			SceneLoader.LoadScene<MapSceneController>();
		}

		private void OnGameService()
		{
			DonDestroyMonoSingleton<GameServiceContext>.Instance.ShowGameCenterViewController(GameServiceViewControllerState.Achievements, GameServiceLeaderboardTimeScope.AllTime, null);
		}

		private void OnFaceBook()
		{
			DonDestroyMonoSingleton<CheckInternetService>.Instance.CheckInternet(() =>
			{
				Debug.Log("MainMenuController::OnFaceBook " + DonDestroyMonoSingleton<FacebookController>.Instance.IsLoggedIn());
				DonDestroyMonoSingleton<FacebookController>.Instance.FeedShare();
			}, (string errorMsg) =>
			{
			});
		}

		private void OnClearCache()
		{
			if (File.Exists(string.Format("{0}/Main.cnf", Application.persistentDataPath)))
			{
				File.Delete(string.Format("{0}/Main.cnf", Application.persistentDataPath));
			}
			MonoSingleton<AppController>.Instance.Model = new AppModel();
			MonoSingleton<AppController>.Instance.Model.Load();
		}

		private void OnMultiplayer()
		{
			if (MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(QuietLawn_2_3)).Status == LevelStatus.Completed)
			{
				SceneLoader.LoadScene<MultiplayerSceneController>();
				return;
			}
			PopupSurvivalAvailabilityView registeredPopup = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupSurvivalAvailabilityView>();
			registeredPopup.gameObject.SetActive(true);
		}

		private void OnSurvival()
		{
			if (MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(QuietLawn_2_3)).Status == LevelStatus.Completed)
			{
				SceneLoader.LoadScene<SurvivalMapSceneController>();
				return;
			}
			PopupSurvivalAvailabilityView registeredPopup = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupSurvivalAvailabilityView>();
			registeredPopup.gameObject.SetActive(true);
		}
	}
}

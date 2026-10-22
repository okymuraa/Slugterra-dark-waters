using AppsMinistry.Core;
using AppsMinistry.Domain;
using AppsMinistry.UI;
using AppsMinistry.Utils;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App;
using Slugterra.Domain.Game.UI;
using Slugterra.Scene;
using Slugterra.Scene.Daily;
using Slugterra.Scene.Game;
using Slugterra.UI.Popup;
using UnityEngine;

namespace Slugterra.UI.MapMenu
{
	public class MapMenuController : UIController<MapMenuView>
	{
		protected override void OnStart()
		{
			base.OnStart();
			View.ApplyModel(MonoSingleton<AppController>.Instance.Model.Levels);
		}

		protected override void _OnEnable()
		{
			EnableControll();
		}

		public void EnableControll()
		{
			View.OnBackEvent += OnBack;
			View.OnEquipEvent += OnEquip;
			View.OnBackpackEvent += OnBackpack;
			View.OnPlayEvent += OnPlay;
			View.OnGameServiceEvent += OnGameService;
		}

		protected override void _OnDisable()
		{
			DisableControll();
		}

		public void DisableControll()
		{
			View.OnBackEvent -= OnBack;
			View.OnEquipEvent -= OnEquip;
			View.OnBackpackEvent -= OnBackpack;
			View.OnPlayEvent -= OnPlay;
			View.OnGameServiceEvent -= OnGameService;
		}

		private void OnBack()
		{
			DonDestroyMonoSingleton<GameServiceContext>.Instance.SelectedChallenge = null;
			SceneLoader.LoadScene<MainSceneController>();
		}

		private void OnEquip()
		{
			Debug.Log("MapMenuController::OnEquip LOAD SHOP ");
			Presetting(LevelItemController.SelectedLevel);
			SceneLoader.LoadScene<ShopSceneController>();
		}

		private void OnBackpack()
		{
			Presetting(LevelItemController.SelectedLevel);
			OffersConfiguration offersConfiguration = MonoSingleton<GameConfiguration>.Instance.OffersConfiguration;
			Offer offer = offersConfiguration.GetOffer(LevelItemController.SelectedLevel.Model.Scene.Name);
			LevelItem model = LevelItemController.SelectedLevel.Model;
			if (offer != null && LevelItemController.SelectedLevel.Model.Status != LevelStatus.Completed)
			{
				PopupOfferItemsForViewingAdsView registeredPopup = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupOfferItemsForViewingAdsView>();
				registeredPopup.Show();
			}
			else if (PlayerPrefs.GetInt(typeof(ScorchedSparksTutorial_1_1).Name) != 1 && model.Scene.Name != "ScorchedSparksTutorial_1_1" && model.Type == LevelType.Arena)
			{
				PopupTutorialCheckOutView registeredPopup2 = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupTutorialCheckOutView>();
				registeredPopup2.gameObject.SetActive(true);
			}
			else
			{
				SceneLoader.LoadScene<BackpackSceneController>();
			}
		}

		private void OnGameService()
		{
			DonDestroyMonoSingleton<GameServiceContext>.Instance.ShowGameCenterViewController(GameServiceViewControllerState.Achievements, GameServiceLeaderboardTimeScope.AllTime, null);
		}

		private void OnPlay()
		{
			AppModel model = MonoSingleton<AppController>.Instance.Model;
			model.MissionSetting.Mission = LevelItemController.SelectedLevel.Model.Scene;
			SceneLoader.LoadScene(MonoSingleton<AppController>.Instance.Model.MissionSetting.Mission, true);
		}

		private void Presetting(LevelItemController item)
		{
			AppModel model = MonoSingleton<AppController>.Instance.Model;
			model.MissionSetting.Mission = item.Model.Scene;
			if (item.Model.Scene.BaseType != typeof(DailySceneController))
			{
				LevelItem.Current = item.Model.Order;
				LevelItem.Last = item.Model.Order;
			}
		}

		private void ShowPopupMultiplayerAvailability()
		{
			PopupMultiplayerAvailabilityView registeredPopup = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupMultiplayerAvailabilityView>();
			if (MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(ScorchedSparksTutorial_1_1)).Status == LevelStatus.Completed && !PlayerPrefsSystem.HasKey("multiplayeravailabilitymessageshowed"))
			{
				PlayerPrefs.SetInt("multiplayeravailabilitymessageshowed", 0);
				registeredPopup.ShowAvalablePopUp();
			}
		}
	}
}

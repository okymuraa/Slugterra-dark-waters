using System;
using System.Collections.Generic;
using AppsMinistry.Core;
using AppsMinistry.Domain;
using AppsMinistry.Domain.Observer;
using AppsMinistry.Scene;
using AppsMinistry.UI;
using AppsMinistry.Utils;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Game.HealthRestoration;
using Slugterra.Domain.Game.UI;
using Slugterra.Domain.Shops;
using Slugterra.Scene;
using Slugterra.Scene.Game;
using Slugterra.Scene.Multiplayer;
using Slugterra.UI.Game;
using Slugterra.UI.Popup;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class EquipStoreController : UIController<EquipStoreView>, IObserver
	{
		[SerializeField]
		private List<ShopTabController> _tabs;

		[SerializeField]
		private GameObject _enabledAssistantButton;

		[SerializeField]
		private GameObject _disabledAssistantButton;

		[SerializeField]
		private ScaleAnimUI _slugsAnim;

		[SerializeField]
		private ScaleAnimUI _weaponAnim;

		[SerializeField]
		private GameObject _assistantButton;

		[SerializeField]
		private GameObject _foesButton;

		[SerializeField]
		private List<ItemDashboardView> dashboards;

		private ShopTabController _currentTab;

		private ShopModel _model;

		private PopupGiftsView _popupGifts;

		private PopupConfirmView _popupConfirm;

		private bool _isMultiplayer;

		private MissionSetting _missionSetting;

		private bool _showRecoverSlugsCount;

		private bool _isRecommendationsIgnored;

		private string _brokenObjectLocalization;

		private const string SLUG_TIRED_LOCALIZATION_KEY = "popup_warning_slugs_tired";

		private const string BLASTER_BROKEN_LOCALIZATION_KEY = "popup_warning_blasters_broken";

		public GameObject[] TutorController;

		private RestoreHealthConfiguration healthRestoreManager
		{
			get
			{
				if (MonoSingleton<GameConfiguration>.IsAlive)
				{
					return MonoSingleton<GameConfiguration>.Instance.RestoreHealthConfigurationManager;
				}
				return null;
			}
		}

		protected override void OnCreate()
		{
			_popupGifts = UnityEngine.Object.FindObjectOfType<PopupGiftsView>();
			_missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			Type mission = MonoSingleton<AppController>.Instance.Model.MissionSetting.Mission;
			Attribute[] customAttributes = Attribute.GetCustomAttributes(mission);
			Attribute attribute = Array.Find(customAttributes, (Attribute a) => a is SceneNameAttribute);
			if (attribute != null)
			{
				_isMultiplayer = (attribute as SceneNameAttribute).IsMultiplayer;
			}
			dashboards.ForEach((ItemDashboardView obj) =>
			{
				obj.RecalculateTabBadges();
			});
		}

		protected override void OnStart()
		{
			_model = MonoSingleton<AppController>.Instance.Model.ShopModel;
			LevelItem levelItem = MonoSingleton<AppController>.Instance.Model.GetLevelItem(_missionSetting.Mission);
			if (levelItem.Type == LevelType.Arena)
			{
				if (_foesButton != null && !_foesButton.activeSelf)
				{
					_foesButton.SetActive(true);
				}
				if (_assistantButton != null && _assistantButton.activeSelf)
				{
					_assistantButton.SetActive(false);
				}
			}
			else
			{
				if (_foesButton != null && _foesButton.activeSelf)
				{
					_foesButton.SetActive(false);
				}
				if (_assistantButton != null && !_assistantButton.activeSelf)
				{
					_assistantButton.SetActive(true);
				}
			}
			OnTab(typeof(EliPreviewTabController));
			if (SurvivalController.IsSurvival(_missionSetting.Mission))
			{
				_disabledAssistantButton.SetActive(true);
				_enabledAssistantButton.SetActive(false);
			}
			_popupConfirm = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupConfirmView>();
			CheckGameState();
		}

		public virtual void OnObjectChanged(object observable)
		{
			if (observable is MissionSetting)
			{
				OnMissionSettingChanged(observable as MissionSetting);
			}
		}

		protected virtual void OnMissionSettingChanged(MissionSetting settings)
		{
			CheckGameState();
		}

		private void CheckGameState()
		{
			if (_missionSetting.Blaster != null)
			{
				BlasterItem blaster = MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(_missionSetting.Blaster.Type);
				AbstractHealthData healthConfiguration = healthRestoreManager.GetHealthConfiguration(_missionSetting.Blaster.Type);
				if (blaster.Energy <= 0f && healthConfiguration.IsFullyRestored)
				{
					BlasterLevels blasterParams = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterParams(blaster.Type, blaster.Level);
					blaster.Energy = Mathf.Round(blasterParams.Energy * 0.1f);
				}
				if (!healthConfiguration.IsFullyRestored)
				{
					_brokenObjectLocalization = "popup_warning_blasters_broken";
					return;
				}
			}
			if (_missionSetting.AttackSlug != null)
			{
				AbstractHealthData healthConfiguration2 = healthRestoreManager.GetHealthConfiguration(_missionSetting.AttackSlug.Type);
				if (!healthConfiguration2.IsFullyRestored)
				{
					_brokenObjectLocalization = "popup_warning_slugs_tired";
				}
			}
		}

		protected override void _OnEnable()
		{
			View.OnBackEvent += OnBack;
			View.OnPlayEvent += OnPlay;
			View.OnPlayFailEvent += OnPlayFailed;
			View.OnGiftsEvent += OnGifts;
			View.OnTabEvent += OnTab;
			View.OnTabSelectElementEvent += OnSlugTabElementSelect;
			View.OnAssistantTabSelectElementEvent += OnAssistantTabElementSelect;
			View.OnBlasterTabSelectElementEvent += OnBlasterTabElementSelect;
			View.OnGameServiceEvent += OnGameService;
			healthRestoreManager.OnUpdateValues += CheckGameState;
			DownloadManager instance = DonDestroyMonoSingleton<DownloadManager>.Instance;
			instance.finishedBundleLoading = (Action<string>)Delegate.Combine(instance.finishedBundleLoading, new Action<string>(BundleDownloaded));
			_missionSetting.AddObserver(this);
			CutScene_ShopScene.TutorialShopSceneComplete = PlayerPrefsSystem.HasKey(typeof(ScorchedSparksTutorial_1_1).Name) || _isMultiplayer;
		}

		protected override void _OnDisable()
		{
			View.OnBackEvent -= OnBack;
			View.OnPlayEvent -= OnPlay;
			View.OnPlayFailEvent -= OnPlayFailed;
			View.OnGiftsEvent -= OnGifts;
			View.OnTabEvent -= OnTab;
			View.OnTabSelectElementEvent -= OnSlugTabElementSelect;
			View.OnAssistantTabSelectElementEvent -= OnAssistantTabElementSelect;
			View.OnBlasterTabSelectElementEvent -= OnBlasterTabElementSelect;
			View.OnGameServiceEvent -= OnGameService;
			if (healthRestoreManager != null)
			{
				healthRestoreManager.OnUpdateValues -= CheckGameState;
			}
			if (DonDestroyMonoSingleton<DownloadManager>.IsAlive)
			{
				DownloadManager instance = DonDestroyMonoSingleton<DownloadManager>.Instance;
				instance.finishedBundleLoading = (Action<string>)Delegate.Remove(instance.finishedBundleLoading, new Action<string>(BundleDownloaded));
			}
			_missionSetting.RemoveObserver(this);
		}

		private void OnBack()
		{
			DonDestroyMonoSingleton<GameServiceContext>.Instance.SelectedChallenge = null;
			SceneLoader.LoadPrevScene(true);
		}

		private void OnPlay()
		{
			if (!_showRecoverSlugsCount && ((_missionSetting.DefenceSlug != null && !healthRestoreManager.GetHealthConfiguration(_missionSetting.DefenceSlug.Type).IsFullyRestored) || (_missionSetting.SpecialSlug != null && !healthRestoreManager.GetHealthConfiguration(_missionSetting.SpecialSlug.Type).IsFullyRestored)))
			{
				PopupSlugsWarningView registeredPopup = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupSlugsWarningView>();
				registeredPopup.Show();
				_showRecoverSlugsCount = true;
				return;
			}
			if (_missionSetting.DefenceSlug != null && !healthRestoreManager.GetHealthConfiguration(_missionSetting.DefenceSlug.Type).IsFullyRestored)
			{
				_missionSetting.SetDefaultDefenceSlug();
			}
			if (_missionSetting.SpecialSlug != null && !healthRestoreManager.GetHealthConfiguration(_missionSetting.SpecialSlug.Type).IsFullyRestored)
			{
				_missionSetting.SetDefaultSpecialSlug();
			}
			if (MonoSingleton<AppController>.Instance.Model.GetLevelItem(_missionSetting.Mission).Type == LevelType.Action && !SurvivalController.IsSurvival(_missionSetting.Mission) && _missionSetting.Assistant != null && !healthRestoreManager.GetHealthConfiguration(_missionSetting.Assistant.Type).IsFullyRestored)
			{
				PopupAssistantWarningView registeredPopup2 = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupAssistantWarningView>();
				registeredPopup2.Show();
			}
			else if (!IsRecommendedItemsEquiped() && !_isRecommendationsIgnored)
			{
				_isRecommendationsIgnored = true;
				PopupUseRecommendedView registeredPopup3 = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupUseRecommendedView>();
				registeredPopup3.Show();
			}
			else
			{
				LoadGameScene();
			}
		}

		private void OnGameService()
		{
			DonDestroyMonoSingleton<GameServiceContext>.Instance.ShowGameCenterViewController(GameServiceViewControllerState.Achievements, GameServiceLeaderboardTimeScope.AllTime, null);
		}

		private bool IsRecommendedItemsEquiped()
		{
			List<string> levelRecommendationsList = MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetLevelRecommendationsList(MonoSingleton<AppController>.Instance.Model.GetLevelItem(_missionSetting.Mission).Scene.Name);
			if (levelRecommendationsList.Count == 0)
			{
				return true;
			}
			RecommendationConfig recommendationById = MonoSingleton<GameConfiguration>.Instance.RecommendationConfiguration.GetRecommendationById(levelRecommendationsList[0]);
			bool flag = true;
			foreach (RecommendedItem item in recommendationById.Items)
			{
				if (!item.IsRequired)
				{
					continue;
				}
				switch (item.Type)
				{
				case ItemType.Blaster:
					if (_missionSetting.Blaster.Type.ToString() != item.ItemValue)
					{
						flag = false;
					}
					break;
				case ItemType.Slug:
					if (_missionSetting.AttackSlug.Type.ToString() != item.ItemValue && _missionSetting.DefenceSlug.Type.ToString() != item.ItemValue && _missionSetting.SpecialSlug.Type.ToString() != item.ItemValue)
					{
						flag = false;
					}
					break;
				case ItemType.Armor:
					if (_missionSetting.Armor.Type.ToString() != item.ItemValue)
					{
						flag = false;
					}
					break;
				case ItemType.Ally:
					if (_missionSetting.Assistant.Type.ToString() != item.ItemValue)
					{
						flag = false;
					}
					break;
				}
				if (flag)
				{
					continue;
				}
				break;
			}
			return flag;
		}

		private void LoadGameScene()
		{
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			Type mission = missionSetting.Mission;
			if (MonoSingleton<AppController>.Instance.Model.MissionSetting.Mission == typeof(ScorchedSparksMultiplayer))
			{
				SceneLoader.LoadScene<Lobby>();
			}
			else
			{
				SceneLoader.LoadScene(missionSetting.Mission, true);
			}
		}

		private void BundleDownloaded(string bundleName)
		{
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			string levelAsset = MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetLevelAsset(missionSetting.Mission.Name);
			if (levelAsset != null && !(bundleName == levelAsset))
			{
			}
		}

		private void OnGifts()
		{
			if (!DonDestroyMonoSingleton<GameServiceContext>.Instance.IsPlayerAuthenticated())
			{
				DonDestroyMonoSingleton<GameServiceContext>.Instance.AuthenticateLocalPlayer();
			}
			_popupGifts.Show();
		}

		private bool OnSlugTabElementSelect(Type tabType, SlugGroup group)
		{
			if (!(_currentTab != null) || _currentTab.GetType() != tabType)
			{
				if (_currentTab != null)
				{
					_currentTab.gameObject.SetActive(false);
				}
				_currentTab = GetTab(tabType);
			}
			if (_currentTab != null)
			{
				MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
				SlugItem slugItem = null;
				switch (group)
				{
				case SlugGroup.Control:
					slugItem = missionSetting.DefenceSlug;
					break;
				case SlugGroup.Special:
					slugItem = missionSetting.SpecialSlug;
					break;
				case SlugGroup.Attack:
					slugItem = missionSetting.AttackSlug;
					break;
				}
				if (!_currentTab.gameObject.activeSelf)
				{
					_currentTab.gameObject.SetActive(true);
					_currentTab.ApplyModel(GetTabModel(tabType));
				}
				if (slugItem == null)
				{
					_currentTab.View.GotoLastScrollValue();
				}
				else if (slugItem.Type != SlugType.None)
				{
					_currentTab.View.ShopItemsScrollScontroller.SelectItemByName(slugItem.Type.ToString());
				}
				else
				{
					_currentTab.View.ShopItemsScrollScontroller.SelectItemByGroup(group);
				}
			}
			else
			{
				Debug.LogWarning("Shop: tab is null");
			}
			return true;
		}

		private bool OnAssistantTabElementSelect(Type tabType)
		{
			if (_currentTab != null && _currentTab.GetType() == tabType)
			{
				return false;
			}
			if (_currentTab != null)
			{
				_currentTab.gameObject.SetActive(false);
			}
			_currentTab = GetTab(tabType);
			if (_currentTab != null)
			{
				_currentTab.gameObject.SetActive(true);
				_currentTab.ApplyModel(GetTabModel(tabType));
			}
			if (_currentTab != null && _currentTab.GetType() == typeof(AssistantsTabController))
			{
				MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
				AssistantItem assistant = missionSetting.Assistant;
				if (assistant == null)
				{
					_currentTab.View.GotoLastScrollValue();
				}
				else if (assistant.Type != AllyType.None)
				{
					_currentTab.View.ShopItemsScrollScontroller.SelectItemByName(assistant.Type.ToString());
				}
			}
			else if (_currentTab != null && _currentTab.GetType() == typeof(FoesTabController))
			{
				DuelSetting duelSetting = MonoSingleton<AppController>.Instance.Model.DuelSetting;
				FoeItem foe = duelSetting.Foe;
				EnemyType enemyType = (EnemyType)Enum.Parse(typeof(EnemyType), foe.Type);
				if (foe == null)
				{
					_currentTab.View.GotoLastScrollValue();
				}
				else if (enemyType != EnemyType.None)
				{
					_currentTab.View.ShopItemsScrollScontroller.SelectItemByName(foe.Type);
				}
			}
			else
			{
				Debug.LogWarning("Shop: tab is null");
			}
			return true;
		}

		private bool OnBlasterTabElementSelect(Type tabType)
		{
			if (_currentTab != null && _currentTab.GetType() == tabType && _currentTab.GetType() != typeof(BlastersTabController))
			{
				return false;
			}
			if (_currentTab != null)
			{
				_currentTab.gameObject.SetActive(false);
			}
			_currentTab = GetTab(tabType);
			if (_currentTab != null)
			{
				MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
				BlasterItem blaster = missionSetting.Blaster;
				_currentTab.gameObject.SetActive(true);
				_currentTab.ApplyModel(GetTabModel(tabType));
				if (blaster == null)
				{
					_currentTab.View.GotoLastScrollValue();
				}
				else
				{
					_currentTab.View.ShopItemsScrollScontroller.SelectItemByName(blaster.Type.ToString());
				}
			}
			else
			{
				Debug.LogWarning("Shop: tab is null");
			}
			return true;
		}

		private bool OnTab(Type tabType)
		{
			if (_currentTab != null && _currentTab.GetType() == tabType)
			{
				_currentTab.UpdateCategory();
				return false;
			}
			if (_currentTab != null)
			{
				_currentTab.gameObject.SetActive(false);
			}
			_currentTab = GetTab(tabType);
			if (_currentTab != null)
			{
				_currentTab.gameObject.SetActive(true);
				_currentTab.ApplyModel(GetTabModel(tabType));
				_currentTab.View.GotoLastScrollValue();
			}
			else
			{
				Debug.LogWarning("Shop: tab is null");
			}
			if (TutorController.Length > 0)
			{
				for (byte b = 0; b < TutorController.Length; b++)
				{
					if (TutorController[b].activeSelf)
					{
						TutorController[b].SendMessage("Tap", tabType);
					}
				}
			}
			return true;
		}

		private ShopTabController GetTab(Type tabType)
		{
			if (_tabs == null || _tabs.Count == 0)
			{
				Debug.LogWarning("Shop: Can't find tab: tabs is null or empty");
				return null;
			}
			foreach (ShopTabController tab in _tabs)
			{
				if (tab.GetType() == tabType)
				{
					return tab;
				}
			}
			return _tabs[0];
		}

		private object GetTabModel(Type tabType)
		{
			if (tabType == typeof(EliPreviewTabController))
			{
				return MonoSingleton<AppController>.Instance.Model.MissionSetting;
			}
			return _model;
		}

		public void OpenTabAtItem(object itemValue)
		{
			ShopTabController shopTabController = null;
			Type tabType = null;
			if (itemValue is ArmorType)
			{
				tabType = typeof(ArmorsTabController);
			}
			else if (itemValue is BlasterType)
			{
				tabType = typeof(BlastersTabController);
			}
			else if (itemValue is SlugType)
			{
				tabType = typeof(SlugsTabController);
			}
			else if (itemValue is AllyType)
			{
				tabType = typeof(AssistantsTabController);
			}
			shopTabController = GetTab(tabType);
			shopTabController.ToViewValue = (int)itemValue;
			OnTab(tabType);
		}

		private void OnPlayFailed()
		{
			PopupItemsWarningView registeredPopup = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupItemsWarningView>();
			registeredPopup.Show(_brokenObjectLocalization);
		}
	}
}

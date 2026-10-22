using System;
using System.Collections.Generic;
using AppsMinistry;
using AppsMinistry.Core;
using AppsMinistry.Core.Input.Window;
using AppsMinistry.Domain;
using AppsMinistry.Domain.Observer;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Game.UI;
using Slugterra.Domain.Shops;
using Slugterra.Scene;
using Slugterra.Scene.Daily;
using Slugterra.Scene.Game;
using Slugterra.Scene.Multiplayer;
using Slugterra.UI.MapMenu;
using Slugterra.UI.Popup;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class BackpackController : UIController<BackpackView>, IObserver
	{
		[SerializeField]
		private GameObject _contentRecommended;

		[SerializeField]
		private GameObject _contentSpecialPacks;

		[SerializeField]
		private tk2dUIScrollableArea _scrollableRecommended;

		[SerializeField]
		private tk2dUIScrollableArea _scrollableSpecialPacks;

		[SerializeField]
		private GameObject _recommendedItemPrefab;

		[SerializeField]
		private GameObject _specialPackItemPrefab;

		[SerializeField]
		private GameObject _arenaRivalsChoicePrefab;

		[SerializeField]
		private GameObject _arsenalButtonOn;

		[SerializeField]
		private GameObject _arsenalButtonOff;

		[SerializeField]
		private GameObject _arsenalButtonRoot;

		[SerializeField]
		private GameObject _specialPacksButton;

		[SerializeField]
		private GameObject _badgeRecommended;

		[SerializeField]
		private GameObject _badgeSpecialPacks;

		[SerializeField]
		private GameObject _badgeForSpecialPacks;

		[SerializeField]
		private GameObject _badgeForArsenal;

		private Dictionary<string, RecommendedItemController> _recommendedItems = new Dictionary<string, RecommendedItemController>();

		private List<RecommendedItemController> _recommendedItemUI = new List<RecommendedItemController>();

		private Dictionary<string, SpecialPackItemController> _specialPacksItems = new Dictionary<string, SpecialPackItemController>();

		private MissionSetting _missionSetting;

		private LevelItem levelItem;

		public GameObject Tutorial;

		protected override void _OnEnable()
		{
			View.OnBackEvent += OnBack;
			View.OnEquipEvent += OnEquip;
			View.OnStartEvent += LoadGameScene;
			View.OnRecommendedEvent += OnRecommended;
			View.OnSpecialPacksEvent += OnSpecialPacks;
			View.OnGameServiceEvent += OnGameService;
			if (_specialPacksItems.Count > 0)
			{
				foreach (KeyValuePair<string, SpecialPackItemController> specialPacksItem in _specialPacksItems)
				{
					SpecialPackItemView view = specialPacksItem.Value.View;
					view.OnBtnBuyItem = (Action)Delegate.Combine(view.OnBtnBuyItem, new Action(OnSpecialPacksBuy));
				}
			}
			if (_recommendedItems.Count > 0)
			{
				foreach (KeyValuePair<string, RecommendedItemController> recommendedItem in _recommendedItems)
				{
					RecommendedItemView view2 = recommendedItem.Value.View;
					view2.OnBtnBuyItem = (Action)Delegate.Combine(view2.OnBtnBuyItem, new Action(OnSpecialPacksBuy));
					recommendedItem.Value.View.OnBtnUpgaradeItemEvent += OnSpecialPacksBuy;
					recommendedItem.Value.View.OnRedrawAllEvent += OnSpecialPacksBuy;
				}
			}
			if (MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(QuietLawn_2_3)).Status == LevelStatus.Completed)
			{
				PlayerPrefs.SetInt("NotificationAllow", 1);
				DonDestroyMonoSingleton<PushManager>.Instance.StartSession();
			}
			CheckBadges();
		}

		protected override void _OnDisable()
		{
			View.OnBackEvent -= OnBack;
			View.OnEquipEvent -= OnEquip;
			View.OnStartEvent -= LoadGameScene;
			View.OnRecommendedEvent -= OnRecommended;
			View.OnSpecialPacksEvent -= OnSpecialPacks;
			View.OnGameServiceEvent -= OnGameService;
			foreach (KeyValuePair<string, SpecialPackItemController> specialPacksItem in _specialPacksItems)
			{
				SpecialPackItemView view = specialPacksItem.Value.View;
				view.OnBtnBuyItem = (Action)Delegate.Remove(view.OnBtnBuyItem, new Action(OnSpecialPacksBuy));
			}
			foreach (KeyValuePair<string, RecommendedItemController> recommendedItem in _recommendedItems)
			{
				RecommendedItemView view2 = recommendedItem.Value.View;
				view2.OnBtnBuyItem = (Action)Delegate.Remove(view2.OnBtnBuyItem, new Action(OnSpecialPacksBuy));
				recommendedItem.Value.View.OnBtnUpgaradeItemEvent -= OnSpecialPacksBuy;
				recommendedItem.Value.View.OnRedrawAllEvent -= OnSpecialPacksBuy;
			}
		}

		private void OnBack()
		{
			DonDestroyMonoSingleton<GameServiceContext>.Instance.SelectedChallenge = null;
			SceneLoader.LoadPrevScene();
		}

		protected override void OnCreate()
		{
		}

		protected override void OnStart()
		{
			_missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			levelItem = MonoSingleton<AppController>.Instance.Model.GetLevelItem(_missionSetting.Mission);
			if (levelItem.Type == LevelType.Arena)
			{
				_arenaRivalsChoicePrefab.SetActive(true);
				gameObject.SetActive(false);
			}
			else
			{
				_arenaRivalsChoicePrefab.SetActive(false);
			}
			OnRecommended();
			bool flag = _missionSetting.OfferedArmor != null || _missionSetting.OfferedAssistant != null || _missionSetting.OfferedAttackSlug != null || _missionSetting.OfferedBlaster != null || _missionSetting.OfferedDefenceSlug != null || _missionSetting.OfferedSpecialSlug != null;
			_arsenalButtonRoot.SetActive(!flag);
			_specialPacksButton.SetActive(!flag);
			CheckBadges();
		}

		private static Vector3 CameraSize(Camera cam)
		{
			Vector3 vector = cam.ViewportToWorldPoint(Vector3.one);
			Vector3 vector2 = cam.ViewportToWorldPoint(Vector3.zero);
			return vector - vector2;
		}

		private void CheckBadges()
		{
			List<SpecialPacksConfig> specialPacksById = MonoSingleton<GameConfiguration>.Instance.SpecialPacksConfiguration.GetSpecialPacksById(GetCurrentLevelId());
			bool active = false;
			foreach (SpecialPacksConfig item in specialPacksById)
			{
				if (!GetIsShown(item.Name + "Special"))
				{
					active = true;
					break;
				}
			}
			_badgeForSpecialPacks.SetActive(active);
			_badgeForArsenal.SetActive(false);
		}

		private bool CheskSlugsBadges()
		{
			foreach (SlugItem attackSlug in MonoSingleton<AppController>.Instance.Model.ShopModel.AttackSlugs)
			{
				SlugConfig slugConfiguration = MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugConfiguration(attackSlug.Type);
				if ((attackSlug.IsBought || !attackSlug.IsLocked) && attackSlug.Type != SlugType.Flopper && slugConfiguration.isShowInStore && !GetIsShown(attackSlug.Type.ToString()))
				{
					return true;
				}
			}
			foreach (SlugItem defenceSlug in MonoSingleton<AppController>.Instance.Model.ShopModel.DefenceSlugs)
			{
				SlugConfig slugConfiguration2 = MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugConfiguration(defenceSlug.Type);
				if ((defenceSlug.IsBought || !defenceSlug.IsLocked) && slugConfiguration2.isShowInStore && !GetIsShown(defenceSlug.Type.ToString()))
				{
					return true;
				}
			}
			foreach (SlugItem specialSlug in MonoSingleton<AppController>.Instance.Model.ShopModel.SpecialSlugs)
			{
				SlugConfig slugConfiguration3 = MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugConfiguration(specialSlug.Type);
				if ((specialSlug.IsBought || !specialSlug.IsLocked) && slugConfiguration3.isShowInStore && !GetIsShown(specialSlug.Type.ToString()))
				{
					return true;
				}
			}
			return false;
		}

		private bool CheskAssistantsBadges()
		{
			foreach (AssistantItem assistant in MonoSingleton<AppController>.Instance.Model.ShopModel.Assistants)
			{
				HumanoidConfig alliesConfig = MonoSingleton<GameConfiguration>.Instance.UnitConfigurationManager.GetAlliesConfig(assistant.Type);
				if ((!assistant.IsLocked || assistant.IsBought) && alliesConfig.isShowInStore && !GetIsShown(assistant.Type.ToString()))
				{
					return true;
				}
			}
			return false;
		}

		private bool CheskArmorBadges()
		{
			foreach (ArmorItem armor in MonoSingleton<AppController>.Instance.Model.ShopModel.Armors)
			{
				ArmorConfig armorConfiguration = MonoSingleton<GameConfiguration>.Instance.ArmorConfigManager.GetArmorConfiguration(armor.Type);
				if ((!armor.IsLocked || armor.IsBought) && armorConfiguration.isShowInStore && !GetIsShown(armor.Type.ToString()))
				{
					return true;
				}
			}
			return false;
		}

		private bool CheskBlastersBadges()
		{
			foreach (BlasterItem blaster in MonoSingleton<AppController>.Instance.Model.ShopModel.Blasters)
			{
				BlasterConfig blusterConfiguration = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlusterConfiguration(blaster.Type);
				if ((!blaster.IsLocked || blaster.IsBought) && blusterConfiguration.isShowInStore && !GetIsShown(blaster.Type.ToString()))
				{
					return true;
				}
			}
			return false;
		}

		private void OnRecommended()
		{
			_scrollableRecommended.gameObject.SetActive(true);
			_scrollableSpecialPacks.gameObject.SetActive(false);
			_badgeRecommended.SetActive(true);
			_badgeSpecialPacks.SetActive(false);
			List<string> levelRecommendationsList = MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetLevelRecommendationsList(levelItem.Scene.Name);
			RecommendationConfig recommendationById = MonoSingleton<GameConfiguration>.Instance.RecommendationConfiguration.GetRecommendationById(levelRecommendationsList[0]);
			PopupGuideAnnouncementView popup = new PopupGuideAnnouncementView();
			float contentLength = (float)(recommendationById.Items.Count - 1) * 2.88f;
			_scrollableRecommended.ContentLength = contentLength;
			for (int i = 0; i < recommendationById.Items.Count; i++)
			{
				RecommendedItem recommendedItem = recommendationById.Items[i];
				if (_recommendedItems.ContainsKey(recommendedItem.ItemValue))
				{
					Debug.LogWarning("Already have Key: " + recommendedItem.ItemValue);
					return;
				}
				RecommendedItemController recommendedItemController = CreateNewListRecommendedItem(new Vector3((float)i * 2.88f, 0f, 0f));
				_recommendedItems.Add(recommendedItem.ItemValue, recommendedItemController);
				RecommendedItemView view = recommendedItemController.View;
				view.OnBtnBuyItem = (Action)Delegate.Combine(view.OnBtnBuyItem, new Action(OnSpecialPacksBuy));
				recommendedItemController.View.OnBtnUpgaradeItemEvent += OnSpecialPacksBuy;
				recommendedItemController.View.OnRedrawAllEvent += OnSpecialPacksBuy;
				if (recommendedItem.Type == ItemType.Slug)
				{
					SlugType slugTypeByName = MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugTypeByName(recommendedItem.ItemValue);
					recommendedItemController.ShowSlug(slugTypeByName, AnnounceItemViewType.Buy, popup, recommendedItem.UpgradeLevel, recommendedItem.IsRequired);
					SetIsShown(slugTypeByName.ToString());
				}
				else if (recommendedItem.Type == ItemType.Blaster)
				{
					BlasterType blasterTypeByName = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterTypeByName(recommendedItem.ItemValue);
					recommendedItemController.ShowBlaster(blasterTypeByName, AnnounceItemViewType.Buy, popup, recommendedItem.IsRequired);
					SetIsShown(blasterTypeByName.ToString());
				}
				else if (recommendedItem.Type == ItemType.Armor)
				{
					ArmorType armorTypeByName = MonoSingleton<GameConfiguration>.Instance.ArmorConfigManager.GetArmorTypeByName(recommendedItem.ItemValue);
					recommendedItemController.ShowArmor(armorTypeByName, AnnounceItemViewType.Buy, popup, recommendedItem.IsRequired);
					SetIsShown(armorTypeByName.ToString());
				}
				else if (recommendedItem.Type == ItemType.Ally)
				{
					AllyType assistantTypeByName = MonoSingleton<GameConfiguration>.Instance.UnitConfigurationManager.GetAssistantTypeByName(recommendedItem.ItemValue);
					recommendedItemController.ShowAlly(assistantTypeByName, AnnounceItemViewType.Buy, popup, recommendedItem.UpgradeLevel, recommendedItem.IsRequired);
					SetIsShown(assistantTypeByName.ToString());
				}
				else if (recommendedItem.Type == ItemType.Consumable)
				{
					ConsumableType consumableTypeByName = MonoSingleton<GameConfiguration>.Instance.ConsumableConfigurationManager.GetConsumableTypeByName(recommendedItem.ItemValue);
					recommendedItemController.ShowConsumable(consumableTypeByName, AnnounceItemViewType.Buy, popup, recommendedItem.UpgradeLevel, recommendedItem.IsRequired);
					SetIsShown(consumableTypeByName.ToString());
				}
				_recommendedItemUI.Add(recommendedItemController);
			}
			if (Tutorial != null && Tutorial.activeSelf)
			{
				Tutorial.SendMessage("StartBackpack", _recommendedItemUI);
			}
		}

		private void OnSpecialPacks()
		{
			_scrollableRecommended.gameObject.SetActive(false);
			_scrollableSpecialPacks.gameObject.SetActive(true);
			_badgeRecommended.SetActive(false);
			_badgeSpecialPacks.SetActive(true);
			ShopModel shopModel = MonoSingleton<AppController>.Instance.Model.ShopModel;
			string levelSpecialPack = MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetLevelSpecialPack(levelItem.Scene.Name);
			SpecialPacksConfig specialPackById = MonoSingleton<GameConfiguration>.Instance.SpecialPacksConfiguration.GetSpecialPackById(levelSpecialPack);
			List<SpecialPacksConfig> specialPacksById = MonoSingleton<GameConfiguration>.Instance.SpecialPacksConfiguration.GetSpecialPacksById(GetCurrentLevelId());
			PopupGuideAnnouncementView popupGuideAnnouncementView = new PopupGuideAnnouncementView();
			float num = (float)(specialPacksById.Count - 1) * 7.28f;
			_scrollableSpecialPacks.ContentLength = num;
			_contentSpecialPacks.transform.localPosition = new Vector3(0f - num, 0f, 0f);
			float num2 = 0f;
			foreach (SpecialPacksConfig item in specialPacksById)
			{
				specialPackById = shopModel.GetSpecialPack(item.Id);
				SpecialPackItemController specialPackItemController;
				if (_specialPacksItems.ContainsKey(specialPackById.Id))
				{
					Debug.LogWarning("Already have Key: " + specialPackById.Id);
					specialPackItemController = _specialPacksItems[specialPackById.Id];
				}
				else
				{
					specialPackItemController = CreateNewListSpecialPacksItem(new Vector3(num2, 0f, 0f));
					SpecialPackItemView view = specialPackItemController.View;
					view.OnBtnBuyItem = (Action)Delegate.Combine(view.OnBtnBuyItem, new Action(OnSpecialPacksBuy));
					_specialPacksItems.Add(specialPackById.Id, specialPackItemController);
					num2 += 7.28f;
				}
				specialPackItemController.View.ShowBadge(!GetIsShown(item.Name + "Special"));
				specialPackItemController.ShowItems(specialPackById);
				if (!GetIsShown(item.Name + "Special"))
				{
					SetIsShown(item.Name + "Special");
				}
			}
			_badgeForSpecialPacks.SetActive(false);
			InputWindowTk2dScrollFacade component = _scrollableSpecialPacks.GetComponent<InputWindowTk2dScrollFacade>();
			if (component != null)
			{
				component.ReInitialize();
			}
		}

		private void OnGameService()
		{
			DonDestroyMonoSingleton<GameServiceContext>.Instance.ShowGameCenterViewController(GameServiceViewControllerState.Achievements, GameServiceLeaderboardTimeScope.AllTime, null);
		}

		private void OnSpecialPacksBuy()
		{
		}

		protected int GetCurrentLevelId()
		{
			bool flag = MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(QuietLawn_2_1)).Status == LevelStatus.Unlocked || MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(QuietLawn_2_1)).Status == LevelStatus.Completed;
			bool flag2 = MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(DarkPeriphery_3_1)).Status == LevelStatus.Unlocked || MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(DarkPeriphery_3_1)).Status == LevelStatus.Completed;
			bool flag3 = MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(Deadweed_4_1)).Status == LevelStatus.Unlocked || MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(Deadweed_4_1)).Status == LevelStatus.Completed;
			bool flag4 = MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(GhoulDep_5_1)).Status == LevelStatus.Unlocked || MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(GhoulDep_5_1)).Status == LevelStatus.Completed;
			int result;
			if (MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(Train_6_1)).Status == LevelStatus.Unlocked || MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(Train_6_1)).Status == LevelStatus.Completed)
			{
				result = 6;
			}
			else if (flag4)
			{
				result = 5;
			}
			else if (flag3)
			{
				result = 4;
			}
			else
			{
				result = (flag2 ? 3 : ((!flag) ? 1 : 2));
			}
			return result;
		}

		protected RecommendedItemController CreateNewListRecommendedItem(Vector3 pos)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(_recommendedItemPrefab);
			gameObject.SetActive(true);
			gameObject.transform.parent = _contentRecommended.transform;
			gameObject.transform.localPosition = pos;
			return gameObject.GetComponent<RecommendedItemController>();
		}

		protected SpecialPackItemController CreateNewListSpecialPacksItem(Vector3 pos)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(_specialPackItemPrefab);
			gameObject.SetActive(true);
			gameObject.transform.parent = _contentSpecialPacks.transform;
			gameObject.transform.localPosition = pos;
			return gameObject.GetComponent<SpecialPackItemController>();
		}

		private void OnEquip()
		{
			Play(LevelItemController.SelectedLevel);
		}

		private void Play(LevelItemController item)
		{
			AppModel model = MonoSingleton<AppController>.Instance.Model;
			if (item != null)
			{
				model.MissionSetting.Mission = item.Model.Scene;
			}
			SceneLoader.LoadScene<ShopSceneController>(true);
			if (item != null && item.Model.Scene.BaseType != typeof(DailySceneController))
			{
				LevelItem.Current = item.Model.Order;
				LevelItem.Last = item.Model.Order;
			}
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

		private void SetIsShown(string name)
		{
			PlayerPrefs.SetInt(name + "IsShown", 1);
		}

		private bool GetIsShown(string name)
		{
			return PlayerPrefs.GetInt(name + "IsShown", 0) == 1;
		}

		public virtual void OnObjectChanged(object observable)
		{
		}
	}
}

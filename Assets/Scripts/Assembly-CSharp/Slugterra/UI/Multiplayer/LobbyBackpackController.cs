using System;
using System.Collections.Generic;
using AppsMinistry.Core;
using AppsMinistry.Domain;
using AppsMinistry.Domain.Observer;
using AppsMinistry.UI;
using AppsMinistry.Utils;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Game.UI;
using Slugterra.Scene;
using Slugterra.Scene.Daily;
using Slugterra.Scene.Game;
using Slugterra.Scene.Multiplayer;
using Slugterra.UI.MapMenu;
using Slugterra.UI.Popup;
using Slugterra.UI.Shops;
using Slugterra.UI.SurvivalMenu;
using UnityEngine;

namespace Slugterra.UI.Multiplayer
{
	public class LobbyBackpackController : UIController<LobbyBackpackView>, IObserver
	{
		public TextMesh _playerNickname;

		public TextMesh _playerRank;

		public GameObject _available;

		public GameObject _unavailable;

		[SerializeField]
		private GameObject _content;

		[SerializeField]
		private tk2dUIScrollableArea _scrollable;

		[SerializeField]
		private GameObject _recommendedItemPrefab;

		[SerializeField]
		private GameObject _specialPackItemPrefab;

		private Dictionary<RecommendedItem, RecommendedItemController> _recommendedItems = new Dictionary<RecommendedItem, RecommendedItemController>();

		private List<RecommendedItemController> _recommendedItemUI = new List<RecommendedItemController>();

		private List<RecommendedItemController> _recommendedItemRequiredUI = new List<RecommendedItemController>();

		private MissionSetting _missionSetting;

		private LevelItem levelItem;

		private int randomLevelID;

		private float randomFLevelID;

		private Type[] _rndlevels = new Type[2];

		private bool _connectionLost;

		[SerializeField]
		private GameObject _btnStartOn;

		[SerializeField]
		private GameObject _btnStartOff;

		protected override void _OnEnable()
		{
			View.OnBackEvent += OnBack;
			View.OnEquipEvent += OnEquip;
			View.OnStartEvent += LoadGameScene;
			View.OnRecommendedEvent += OnRecommended;
			View.OnGameServiceEvent += OnGameService;
			CheckInternetService instance = DonDestroyMonoSingleton<CheckInternetService>.Instance;
			instance.onInternetConnectionLost = (Action)Delegate.Combine(instance.onInternetConnectionLost, new Action(InternetConnectionLost));
			CheckInternetService instance2 = DonDestroyMonoSingleton<CheckInternetService>.Instance;
			instance2.onInternetConnectionAppear = (Action)Delegate.Combine(instance2.onInternetConnectionAppear, new Action(InternetConnectionAppear));
			OnEnableEventsSP();
			CheckContinue();
			SetPlayerNickname();
		}

		protected override void _OnDisable()
		{
			View.OnBackEvent -= OnBack;
			View.OnEquipEvent -= OnEquip;
			View.OnStartEvent -= LoadGameScene;
			View.OnRecommendedEvent -= OnRecommended;
			View.OnGameServiceEvent -= OnGameService;
			CheckInternetService instance = DonDestroyMonoSingleton<CheckInternetService>.Instance;
			instance.onInternetConnectionLost = (Action)Delegate.Remove(instance.onInternetConnectionLost, new Action(InternetConnectionLost));
			CheckInternetService instance2 = DonDestroyMonoSingleton<CheckInternetService>.Instance;
			instance2.onInternetConnectionAppear = (Action)Delegate.Remove(instance2.onInternetConnectionAppear, new Action(InternetConnectionAppear));
			OnDisableEventsSP();
		}

		private void InternetConnectionLost()
		{
			_btnStartOn.SetActive(false);
			_btnStartOff.SetActive(true);
			_connectionLost = true;
		}

		private void InternetConnectionAppear()
		{
			_btnStartOn.SetActive(true);
			_btnStartOff.SetActive(false);
			_connectionLost = false;
		}

		private void OnBack()
		{
			View._tickets.SetActive(true);
			MultiplayerMenuView component = View._tickets.GetComponent<MultiplayerMenuView>();
			component.returnclick = false;
			component.CurTicketCount++;
			component.UpdateItem();
			gameObject.SetActive(false);
			OnDisableEventsSP();
		}

		private void OnDisableEventsSP()
		{
			foreach (RecommendedItemController item in _recommendedItemUI)
			{
				RecommendedItemView view = item.View;
				view.OnBtnBuyItem = (Action)Delegate.Remove(view.OnBtnBuyItem, new Action(CheckContinue));
				item.View.OnBtnUpgaradeItemEvent -= CheckContinue;
				item.View.OnRedrawAllEvent -= CheckContinue;
				item.View.OnEquipItemEvent -= CheckContinue;
			}
		}

		private void OnEnableEventsSP()
		{
			foreach (RecommendedItemController item in _recommendedItemUI)
			{
				RecommendedItemView view = item.View;
				view.OnBtnBuyItem = (Action)Delegate.Combine(view.OnBtnBuyItem, new Action(CheckContinue));
				item.View.OnBtnUpgaradeItemEvent += CheckContinue;
				item.View.OnRedrawAllEvent += CheckContinue;
				item.View.OnEquipItemEvent += CheckContinue;
			}
		}

		protected override void OnCreate()
		{
		}

		protected override void OnStart()
		{
			_rndlevels[0] = typeof(EastSet1Multiplayer);
			_rndlevels[1] = typeof(DesertBase1Multiplayer);
			randomFLevelID = UnityEngine.Random.Range(0, 100);
			randomLevelID = ((randomFLevelID > 50f) ? 1 : 0);
			levelItem = MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(EastSet1Multiplayer));
			OnRecommended();
			SetPlayerNickname();
		}

		private static Vector3 CameraSize(Camera cam)
		{
			Vector3 vector = cam.ViewportToWorldPoint(Vector3.one);
			Vector3 vector2 = cam.ViewportToWorldPoint(Vector3.zero);
			return vector - vector2;
		}

		private void OnRecommended()
		{
			_scrollable.gameObject.SetActive(true);
			List<string> levelRecommendationsList = MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetLevelRecommendationsList(levelItem.Scene.Name);
			RecommendationConfig recommendationById = MonoSingleton<GameConfiguration>.Instance.RecommendationConfiguration.GetRecommendationById(levelRecommendationsList[0]);
			PopupGuideAnnouncementView popup = new PopupGuideAnnouncementView();
			float contentLength = (float)(recommendationById.Items.Count - 1) * 2.88f;
			_scrollable.ContentLength = contentLength;
			for (int i = 0; i < recommendationById.Items.Count; i++)
			{
				RecommendedItem recommendedItem = recommendationById.Items[i];
				if (_recommendedItems.ContainsKey(recommendedItem))
				{
					Debug.LogWarning("Already have Key: " + recommendedItem.ItemValue);
					CheckContinue();
					return;
				}
				RecommendedItemController recommendedItemController = CreateNewListRecommendedItem(new Vector3((float)i * 2.88f, 0f, 0f));
				_recommendedItems.Add(recommendedItem, recommendedItemController);
				if (recommendedItem.Type == ItemType.Slug)
				{
					SlugType slugTypeByName = MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugTypeByName(recommendedItem.ItemValue);
					recommendedItemController.ShowSlug(slugTypeByName, AnnounceItemViewType.Buy, popup, 1, recommendedItem.IsRequired);
					SetIsShown(slugTypeByName.ToString());
				}
				else if (recommendedItem.Type == ItemType.Blaster)
				{
					BlasterType blasterTypeByName = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterTypeByName(recommendedItem.ItemValue);
					recommendedItemController.View.NeedRate = false;
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
				RecommendedItemView view = recommendedItemController.View;
				view.OnBtnBuyItem = (Action)Delegate.Combine(view.OnBtnBuyItem, new Action(CheckContinue));
				recommendedItemController.View.OnBtnUpgaradeItemEvent += CheckContinue;
				recommendedItemController.View.OnRedrawAllEvent += CheckContinue;
				recommendedItemController.View.OnEquipItemEvent += CheckContinue;
				_recommendedItemUI.Add(recommendedItemController);
				if (recommendedItem.IsRequired)
				{
					_recommendedItemRequiredUI.Add(recommendedItemController);
				}
			}
			CheckContinue();
		}

		public void CheckContinue()
		{
			bool flag = true;
			foreach (RecommendedItemController item in _recommendedItemRequiredUI)
			{
				if (item.View.CurrentViewType != AnnounceItemViewType.Acquired)
				{
					flag = false;
					break;
				}
			}
			_available.SetActive(flag);
			_unavailable.SetActive(!flag);
		}

		private void SetPlayerNickname()
		{
			if (DonDestroyMonoSingleton<GameServiceContext>.Instance.IsPlayerAuthenticated())
			{
				string playerAlias = DonDestroyMonoSingleton<GameServiceContext>.Instance.PlayerAlias;
				string text = MonoSingleton<AppController>.Instance.Model.DuelSetting.Rank.ToString("#####");
				_playerNickname.text = playerAlias;
				_playerRank.text = text;
			}
		}

		private void OnGameService()
		{
			DonDestroyMonoSingleton<GameServiceContext>.Instance.ShowGameCenterViewController(GameServiceViewControllerState.Achievements, GameServiceLeaderboardTimeScope.AllTime, null);
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
			gameObject.transform.parent = _content.transform;
			gameObject.transform.localPosition = pos;
			return gameObject.GetComponent<RecommendedItemController>();
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
			missionSetting.Mission = typeof(EastSet1Multiplayer);
			SceneLoader.LoadScene<EastSet1Multiplayer>(true);
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

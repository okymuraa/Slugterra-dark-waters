using System;
using System.Collections.Generic;
using AppsMinistry;
using AppsMinistry.Core;
using AppsMinistry.Scene;
using AppsMinistry.UI;
using AppsMinistry.Utils;
using Slugterra.App;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Game.UI;
using Slugterra.Domain.Shops;
using Slugterra.Scene;
using Slugterra.Scene.Game;
using Slugterra.Scene.Multiplayer;
using Slugterra.UI.Popup;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class ShopInappTabController : UIController<EquipStoreView>
	{
		[SerializeField]
		private List<ShopTabController> _tabs;

		[SerializeField]
		private TabIconView _assistantTabButton;

		[SerializeField]
		private TabIconView _foeTabButton;

		private ShopTabController _currentTab;

		private ShopModel _model;

		private PopupGiftsView _popupGifts;

		private PopupConfirmView _popupConfirm;

		private bool _isMultiplayer;

		protected override void OnCreate()
		{
			_popupGifts = UnityEngine.Object.FindObjectOfType<PopupGiftsView>();
			Type mission = MonoSingleton<AppController>.Instance.Model.MissionSetting.Mission;
			Attribute[] customAttributes = Attribute.GetCustomAttributes(mission);
			Attribute attribute = Array.Find(customAttributes, (Attribute a) => a is SceneNameAttribute);
			if (attribute != null)
			{
				_isMultiplayer = (attribute as SceneNameAttribute).IsMultiplayer;
			}
		}

		protected override void OnStart()
		{
			_model = MonoSingleton<AppController>.Instance.Model.ShopModel;
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			LevelItem levelItem = MonoSingleton<AppController>.Instance.Model.GetLevelItem(missionSetting.Mission);
			if (levelItem.Type == LevelType.Arena)
			{
				_assistantTabButton.gameObject.SetActive(false);
				_foeTabButton.gameObject.SetActive(true);
				OnTab(typeof(FoesTabController));
			}
			else
			{
				_assistantTabButton.gameObject.SetActive(true);
				_foeTabButton.gameObject.SetActive(false);
				OnTab(typeof(EliPreviewTabController));
			}
			_popupConfirm = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupConfirmView>();
		}

		protected override void _OnEnable()
		{
			View.OnBackEvent += OnBack;
			View.OnPlayEvent += OnPlay;
			View.OnGiftsEvent += OnGifts;
			View.OnTabEvent += OnTab;
			CutScene_ShopScene.TutorialShopSceneComplete = PlayerPrefsSystem.HasKey(typeof(ScorchedSparksTutorial_1_1).Name) || _isMultiplayer;
			if (MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(QuietLawn_2_4)).Status == LevelStatus.Completed)
			{
				PlayerPrefs.SetInt("NotificationAllow", 1);
				DonDestroyMonoSingleton<PushManager>.Instance.StartSession();
			}
		}

		protected override void _OnDisable()
		{
			View.OnBackEvent -= OnBack;
			View.OnPlayEvent -= OnPlay;
			View.OnGiftsEvent -= OnGifts;
			View.OnTabEvent -= OnTab;
		}

		private void OnBack()
		{
			SceneLoader.LoadPrevScene();
		}

		private void OnPlay()
		{
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			if (MonoSingleton<AppController>.Instance.Model.MissionSetting.Mission == typeof(ScorchedSparksMultiplayer))
			{
				SceneLoader.LoadScene<Lobby>();
			}
			else
			{
				SceneLoader.LoadScene(missionSetting.Mission, true);
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

		private bool OnTab(Type tabType)
		{
			if (_currentTab != null && _currentTab.GetType() == tabType)
			{
				return true;
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
				Debug.LogWarningFormat("<b><color=red>{0}</color></b>", "ShopInappTabController::OnTab Shop: tab is null");
			}
			return true;
		}

		private ShopTabController GetTab(Type tabType)
		{
			if (_tabs == null || _tabs.Count == 0)
			{
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
	}
}

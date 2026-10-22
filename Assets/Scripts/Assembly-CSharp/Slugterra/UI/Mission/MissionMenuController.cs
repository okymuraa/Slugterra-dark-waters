using System;
using System.Collections.Generic;
using AppsMinistry.Core;
using AppsMinistry.Domain.Observer;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Shops;
using Slugterra.Scene;
using UnityEngine;

namespace Slugterra.UI.Mission
{
	public class MissionMenuController : UIController<MissionMenuView>, IObserver
	{
		[SerializeField]
		private List<MissionTabController> _tabs;

		private MissionTabController _currentTab;

		private ShopModel _shopModel;

		private MissionSetting _settings;

		public void OnObjectChanged(object observable)
		{
			if (observable is MissionSetting)
			{
				View.ApplyModel(observable);
			}
		}

		protected override void OnStart()
		{
			_shopModel = MonoSingleton<AppController>.Instance.Model.ShopModel;
			_settings = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			_settings.AddObserver(this);
			View.ApplyModel(_settings);
			OnTab(typeof(BlastersTabController));
		}

		protected override void _OnEnable()
		{
			View.OnBackEvent += OnBackEvent;
			View.OnPlayEvent += OnPlayEvent;
			View.OnSettingsEvent += OnSettings;
			View.OnTabEvent += OnTab;
		}

		protected override void _OnDisable()
		{
			View.OnBackEvent -= OnBackEvent;
			View.OnPlayEvent -= OnPlayEvent;
			View.OnSettingsEvent -= OnSettings;
			View.OnTabEvent -= OnTab;
		}

		private void OnBackEvent()
		{
			SceneLoader.LoadPrevScene();
		}

		private void OnPlayEvent()
		{
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			SceneLoader.LoadScene(missionSetting.Mission, true);
		}

		private void OnSettings()
		{
			SceneLoader.LoadScene<SettingsSceneController>();
		}

		private void OnTab(Type tabType)
		{
			if (_currentTab != null)
			{
				_currentTab.gameObject.SetActive(false);
			}
			_currentTab = GetTab(tabType);
			if (_currentTab != null)
			{
				_currentTab.gameObject.SetActive(true);
				_currentTab.ApplyModel(_shopModel);
			}
			else
			{
				Debug.LogWarning("MissionMenuController::OnTab Shop: tab is null");
			}
		}

		private MissionTabController GetTab(Type tabType)
		{
			if (_tabs == null || _tabs.Count == 0)
			{
				Debug.LogWarning("MissionMenuController::GetTab Shop: Can't find tab: tabs is null or empty");
				return null;
			}
			foreach (MissionTabController tab in _tabs)
			{
				if (tab.GetType() == tabType)
				{
					return tab;
				}
			}
			return _tabs[0];
		}

		protected override void OnReleaseResource()
		{
			if (_settings != null)
			{
				_settings.RemoveObserver(this);
			}
		}
	}
}

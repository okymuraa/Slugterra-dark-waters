using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Game.HealthRestoration;
using Slugterra.UI.Shops;
using UnityEngine;

namespace Slugterra.UI.Popup
{
	public class PopupAssistantWarningController : UIController<PopupAssistantWarningView>
	{
		private MissionSetting _missionSettings;

		private EquipStoreView _equipStoreView;

		private bool _isRecoverClicked;

		private RestoreHealthConfiguration healthRestoreManager
		{
			get
			{
				return MonoSingleton<GameConfiguration>.Instance.RestoreHealthConfigurationManager;
			}
		}

		protected override void OnCreate()
		{
			base.OnCreate();
			_equipStoreView = Object.FindObjectOfType<EquipStoreView>();
			_missionSettings = MonoSingleton<AppController>.Instance.Model.MissionSetting;
		}

		private void OnEnable()
		{
			View.OnRecoverClick += OnRecoverClickHandler;
			View.OnCloseClick += OnCloseClickHandler;
			healthRestoreManager.OnUpdateValues += OnHealthValuesUpdateHandler;
			if (_missionSettings.Assistant != null && _missionSettings.Assistant.Type != AllyType.None)
			{
				View.SetAssistantSprite(_missionSettings.Assistant.Type);
				View.SetPrice((int)healthRestoreManager.GetHealPrice(_missionSettings.Assistant.Type, _missionSettings.Assistant.Level));
			}
		}

		private void OnDisable()
		{
			View.OnRecoverClick -= OnRecoverClickHandler;
			View.OnCloseClick -= OnCloseClickHandler;
			healthRestoreManager.OnUpdateValues -= OnHealthValuesUpdateHandler;
		}

		private void OnHealthValuesUpdateHandler()
		{
			if (_isRecoverClicked && _missionSettings.Assistant != null && _missionSettings.Assistant.Type != AllyType.None)
			{
				AbstractHealthData healthConfiguration = healthRestoreManager.GetHealthConfiguration(_missionSettings.Assistant.Type);
				if (healthConfiguration.IsFullyRestored)
				{
					_equipStoreView.SendMessage("OnBtnStartClick");
					View.Close();
				}
			}
		}

		private void OnRecoverClickHandler()
		{
			if (_missionSettings.Assistant != null)
			{
				AbstractHealthData healthConfiguration = healthRestoreManager.GetHealthConfiguration(_missionSettings.Assistant.Type);
				if (!healthConfiguration.IsFullyRestored)
				{
					_isRecoverClicked = true;
					MonoSingleton<AppController>.Instance.Model.ShopModel.OnRecoverAssistant(_missionSettings.Assistant);
				}
				else
				{
					_equipStoreView.SendMessage("OnBtnStartClick");
					View.Close();
				}
			}
		}

		private void OnCloseClickHandler()
		{
			_missionSettings.SetDefaultAssistant();
			_equipStoreView.SendMessage("OnBtnStartClick");
			View.Close();
		}
	}
}

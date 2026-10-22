using AppsMinistry.Core;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Game.HealthRestoration;
using Slugterra.Domain.Shops;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class AssistantItemFacade : ItemFacade
	{
		[SerializeField]
		private CountDownBarViewController _countdownBar;

		private AssistantItem _model;

		private RestoreHealthConfiguration healthRestoreManager
		{
			get
			{
				if (MonoSingleton<GameConfiguration>.Instance != null)
				{
					return MonoSingleton<GameConfiguration>.Instance.RestoreHealthConfigurationManager;
				}
				return null;
			}
		}

		public override void OnObjectChanged(object observable)
		{
			base.OnObjectChanged(observable);
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			healthRestoreManager.OnUpdateValues += OnRestorationValuesUpdatedHandler;
		}

		protected override void OnDisable()
		{
			if (healthRestoreManager != null)
			{
				healthRestoreManager.OnUpdateValues -= OnRestorationValuesUpdatedHandler;
			}
			_countdownBar.Show(false);
			if (_model != null)
			{
				_model.RemoveObserver(this);
			}
			base.OnDisable();
		}

		protected override void OnApplyModel(object model)
		{
			if (model is AssistantItem)
			{
				if (_model != null)
				{
					_model.RemoveObserver(this);
				}
				_model = model as AssistantItem;
				_model.AddObserver(this);
			}
		}

		protected override void OnMissionSettingChanged(MissionSetting settings)
		{
		}

		private void OnHireOrFire()
		{
			if (IsWithPlayer(_model))
			{
				_missionSetting.SetDefaultAssistant();
			}
			else
			{
				_missionSetting.Assistant = _model;
			}
		}

		private bool IsWithPlayer(AssistantItem assistant)
		{
			if (assistant != null && _missionSetting.Assistant != null)
			{
				return assistant.Type == _missionSetting.Assistant.Type;
			}
			return false;
		}

		private void OnRestorationValuesUpdatedHandler()
		{
		}
	}
}

using System;
using AppsMinistry.Core;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Game.HealthRestoration;
using Slugterra.Scene;
using UnityEngine;

namespace Slugterra.Game.Units.Ally.Assistant
{
	public class AssistantStateObserver : MonoBehaviour
	{
		private const float MINIMAL_HEALTH_PERCENT = 0.85f;

		private AssistantController _assistant { get; set; }

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

		private void OnEnable()
		{
			SceneLoader.OnSceneReload += OnSceneReloadHandler;
		}

		private void OnDisable()
		{
			if (_assistant != null)
			{
				_assistant.OnDamageRecievedEvent -= OnDamageRecievedEventHandler;
			}
			SceneLoader.OnSceneReload -= OnSceneReloadHandler;
		}

		public void SetAssistant(AssistantController controller)
		{
			_assistant = controller;
			_assistant.OnDamageRecievedEvent += OnDamageRecievedEventHandler;
		}

		public void GoToHospital()
		{
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			GameConfiguration instance = MonoSingleton<GameConfiguration>.Instance;
			RestoreHealthConfiguration restoreHealthConfiguration = healthRestoreManager;
			if (missionSetting != null && !(instance == null) && restoreHealthConfiguration != null)
			{
				int level = missionSetting.Assistant.Level;
				HumanoidLevels alliesConfigurationLevel = instance.UnitConfigurationManager.GetAlliesConfigurationLevel(_assistant.Type, level);
				float num = (_assistant.Model.MaxHealth - _assistant.Model.Health) / _assistant.Model.MaxHealth;
				float num2 = num * (float)alliesConfigurationLevel.restorationTimeInMinutes;
				DateTime timeWhenObjectIsRestored = DateTime.UtcNow.AddMinutes(num2);
				AbstractHealthData healthConfiguration = restoreHealthConfiguration.GetHealthConfiguration(_assistant.Type);
				if (num >= 0.85f)
				{
					healthConfiguration.TimeWhenObjectIsRestored = timeWhenObjectIsRestored;
					restoreHealthConfiguration.Save();
				}
				else if (!healthConfiguration.IsFullyRestored)
				{
					healthConfiguration.Heal();
					restoreHealthConfiguration.Save();
				}
			}
		}

		private void ResetAssistant()
		{
			healthRestoreManager.GetHealthConfiguration(_assistant.Type).Heal();
		}

		private void OnApplicationPause(bool pauseStatus)
		{
			if (pauseStatus)
			{
				GoToHospital();
			}
			else
			{
				ResetAssistant();
			}
		}

		private void OnSceneReloadHandler()
		{
			Debug.Log("reset assistant due reload");
			ResetAssistant();
		}

		private void OnDamageRecievedEventHandler(float damage, Attacker attacker)
		{
			GoToHospital();
		}
	}
}

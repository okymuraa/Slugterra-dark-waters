using System;
using System.Collections.Generic;
using AppsMinistry.Core;
using AppsMinistry.Utils;
using Newtonsoft.Json;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.Game.HealthRestoration;
using Slugterra.Domain.Game.UI;
using Slugterra.Domain.Game.Units;
using Slugterra.Domain.Shops;
using Slugterra.Game.Units;
using Slugterra.Scene;
using Slugterra.Scene.Game;
using UnityEngine;

namespace Slugterra.Game.Blasters
{
	public class BlasterStateObserver : MonoBehaviour
	{
		internal class ObservedBlaster
		{
			public BlasterLevels blasterConfig;

			public BlasterItem blasterItem;

			public ObservedBlaster(BlasterItem blaster)
			{
				blasterItem = blaster;
				blasterConfig = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterParams(blasterItem.Type, blasterItem.Level);
			}
		}

		private Unit _unitModel;

		private HumanoidController _observableController;

		private ObservedBlaster _observedBlaster;

		private List<BlasterItem> _blasters;

		private const string BLASTER_CONFIG_NAME = "Blasters.cfg";

		private RestoreHealthConfiguration healthRestoreManager
		{
			get
			{
				return MonoSingleton<GameConfiguration>.Instance.RestoreHealthConfigurationManager;
			}
		}

		private void OnEnable()
		{
			SceneLoader.OnSceneChangedEvent += OnSceneChangedHandler;
		}

		private void OnDisable()
		{
			SceneLoader.OnSceneChangedEvent -= OnSceneChangedHandler;
			_observableController.OnDamageRecievedEvent -= OnControllerRecieveDamageHandler;
			SaveBlasterConfiguration();
		}

		public void Activate(HumanoidController observableController)
		{
			_observableController = observableController;
			LoadBlasterConfiguration();
			if (IsObservableLevel() && _blasters != null && _blasters.Count > 0)
			{
				SetHandlingBlaster();
				bool canBreak = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlusterConfiguration(_observedBlaster.blasterItem.Type).canBreak;
				if (_observedBlaster.blasterItem.IsBought && !_observedBlaster.blasterItem.IsLocked && canBreak)
				{
					_observableController.OnDamageRecievedEvent += OnControllerRecieveDamageHandler;
				}
			}
		}

		private void SetHandlingBlaster()
		{
			BlasterItem blasterItem = _blasters.Find((BlasterItem a) => a.Type == _observableController.BlasterType);
			if (blasterItem != null)
			{
				_observedBlaster = new ObservedBlaster(blasterItem);
			}
		}

		private bool IsObservableLevel()
		{
			LevelItem currentGameLevel = MonoSingleton<AppController>.Instance.GetCurrentGameLevel();
			if (currentGameLevel.Scene == typeof(ShaneTutor_0_1) || currentGameLevel.Scene == typeof(ScorchedSparksTutorial_1_1) || currentGameLevel.Scene == typeof(QuietLawn_2_1) || currentGameLevel.Scene == typeof(QuietLawn_2_2))
			{
				return false;
			}
			return true;
		}

		private void OnControllerRecieveDamageHandler(float damage, Attacker attacker)
		{
			if (_observedBlaster != null && attacker.slugType != SlugType.None)
			{
				_observedBlaster.blasterItem.Energy--;
				if (_observedBlaster.blasterItem.Energy <= 0f)
				{
					AbstractHealthData healthConfiguration = healthRestoreManager.GetHealthConfiguration(_observedBlaster.blasterItem.Type);
					DateTime timeWhenObjectIsRestored = DateTime.UtcNow.AddMinutes(_observedBlaster.blasterConfig.restorationTimeInMinutes);
					healthConfiguration.TimeWhenObjectIsRestored = timeWhenObjectIsRestored;
					healthRestoreManager.Save();
				}
			}
		}

		private void OnSceneChangedHandler()
		{
			SaveBlasterConfiguration();
		}

		private void OnApplicationPause(bool isPaused)
		{
			if (isPaused)
			{
				SaveBlasterConfiguration();
			}
		}

		private void LoadBlasterConfiguration()
		{
			_blasters = MonoSingleton<AppController>.Instance.Model.ShopModel.Blasters;
		}

		private void SaveBlasterConfiguration()
		{
			if (_blasters != null)
			{
				string stringToSave = JsonConvert.SerializeObject(_blasters);
				IOSystem.SaveStrings(Application.persistentDataPath, "Blasters.cfg", stringToSave, false, true);
			}
		}
	}
}

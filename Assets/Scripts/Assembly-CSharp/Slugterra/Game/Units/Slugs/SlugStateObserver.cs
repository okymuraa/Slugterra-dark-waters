using System;
using System.Collections.Generic;
using AppsMinistry.Core;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Game.HealthRestoration;
using Slugterra.Domain.Game.UI;
using Slugterra.Domain.Shops;
using Slugterra.Game.Mission;
using Slugterra.Scene;
using Slugterra.Scene.Game;
using UnityEngine;

namespace Slugterra.Game.Units.Slugs
{
	public class SlugStateObserver : MonoBehaviour
	{
		internal class ObservedSlug
		{
			public SlugType slugType;

			public int firedCount;

			public SlugLevels slugConfig;

			public ObservedSlug(SlugType type, int level)
			{
				slugType = type;
				firedCount = 0;
				slugConfig = MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugParams(type, level);
			}
		}

		private const int FIRE_COUNT_TO_TIRED = 5;

		private Dictionary<SlugType, ObservedSlug> _usedSlugs = new Dictionary<SlugType, ObservedSlug>();

		private List<Type> _levelIgnoreList = new List<Type>
		{
			typeof(ShaneTutor_0_1),
			typeof(ScorchedSparksTutorial_1_1),
			typeof(QuietLawn_2_1),
			typeof(QuietLawn_2_2),
			typeof(ShopSceneController),
			typeof(SlugPreview)
		};

		private RestoreHealthConfiguration healthRestoreManager
		{
			get
			{
				return MonoSingleton<GameConfiguration>.Instance.RestoreHealthConfigurationManager;
			}
		}

		private void Start()
		{
			if (!IsObservableLevel())
			{
				UnityEngine.Object.Destroy(gameObject);
				return;
			}
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			AddObservableSlug(missionSetting.AttackSlug);
			AddObservableSlug(missionSetting.DefenceSlug);
			AddObservableSlug(missionSetting.SpecialSlug);
		}

		private void OnEnable()
		{
			MonoSingleton<MissionStatisticsController>.Instance.OnEliFireEvent += OnEliFireHandler;
		}

		private void OnDisable()
		{
			if (MonoSingleton<MissionStatisticsController>.IsAlive)
			{
				MonoSingleton<MissionStatisticsController>.Instance.OnEliFireEvent -= OnEliFireHandler;
			}
		}

		private bool IsObservableLevel()
		{
			LevelItem currentGameLevel = MonoSingleton<AppController>.Instance.GetCurrentGameLevel();
			return !_levelIgnoreList.Contains(currentGameLevel.Scene);
		}

		private void AddObservableSlug(SlugItem slugItem)
		{
			if (slugItem != null && !_usedSlugs.ContainsKey(slugItem.Type))
			{
				ObservedSlug value = new ObservedSlug(slugItem.Type, slugItem.Level);
				_usedSlugs.Add(slugItem.Type, value);
			}
		}

		private void CheckSlug(SlugType type)
		{
			ObservedSlug value;
			if (type != SlugType.Flopper && _usedSlugs.TryGetValue(type, out value))
			{
				value.firedCount++;
				if (value.firedCount > 5)
				{
					AbstractHealthData healthConfiguration = healthRestoreManager.GetHealthConfiguration(value.slugType);
					DateTime timeWhenObjectIsRestored = DateTime.UtcNow.AddMinutes(value.slugConfig.restorationTimeInMinutes);
					healthConfiguration.TimeWhenObjectIsRestored = timeWhenObjectIsRestored;
					healthRestoreManager.Save();
				}
			}
		}

		private void OnEliFireHandler(SlugType type)
		{
			CheckSlug(type);
		}
	}
}

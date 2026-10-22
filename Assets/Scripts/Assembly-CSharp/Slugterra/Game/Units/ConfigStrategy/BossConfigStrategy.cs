using System.Collections.Generic;
using AppsMinistry.Core;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.Game.UI;
using Slugterra.Domain.Game.Units;
using Slugterra.Domain.Game.Units.Slugs;
using Slugterra.Game.Units.Enemy;
using Slugterra.Scene.Game;
using UnityEngine;

namespace Slugterra.Game.Units.ConfigStrategy
{
	public class BossConfigStrategy : EnemyConfigStrategy
	{
		public BossConfigStrategy(BossController controller)
			: base(controller)
		{
		}

		protected override void UseResourceConfig(ref Humanoid model)
		{
			BossController bossController = _controller as BossController;
			ArmorLevels armorParams = _configurations.ArmorConfigManager.GetArmorParams(enemyConfig.ArmorType, enemyConfig.ArmorLevel);
			BlasterLevels blasterParams = _configurations.BlasterConfigManager.GetBlasterParams(enemyConfig.BlasterType, enemyConfig.BlasterLevel);
			model.SetArmor(armorParams);
			model.SetBlaster(blasterParams, enemyConfig.BlasterType);
			FoeLevels foeLevels = enemyConfig as FoeLevels;
			bossController.SetSlugs(foeLevels.usedSlugs);
			base.UseResourceConfig(ref model);
		}

		protected override void LoadConfigurationLevels()
		{
			LevelItem currentGameLevel = MonoSingleton<AppController>.Instance.GetCurrentGameLevel();
			if (currentGameLevel.Scene == typeof(ScorchedSparksTutorial_1_1))
			{
				enemyConfig = _configurations.UnitConfigurationManager.GetFoeConfigurationLevel(EnemyType.BossKord, 6);
				return;
			}
			int num = 0;
			IDictionary<SlugType, Slug> slugs = _gameController.Model.Eli.Slugs;
			foreach (KeyValuePair<SlugType, Slug> item in slugs)
			{
				if (num < item.Value.Level)
				{
					num = item.Value.Level;
				}
			}
			enemyConfig = _configurations.UnitConfigurationManager.GetFoeConfigurationLevel(_controller.mEnemyType, num);
		}

		protected override GameObject GetBlaster(BlasterType type)
		{
			return GameFactory.GetBlasterPrefab(type);
		}
	}
}

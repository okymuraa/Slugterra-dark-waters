using System.Collections.Generic;
using AppsMinistry.Core;
using Slugterra.Configuration;
using Slugterra.Domain.Game.UI;
using Slugterra.Domain.Game.Units;
using Slugterra.Domain.Game.Units.Slugs;
using Slugterra.Game.Units.Enemy;
using Slugterra.UI.Game;
using UnityEngine;

namespace Slugterra.Game.Units.ConfigStrategy
{
	public class EnemyConfigStrategy : IUnitConfigStrategy<Humanoid>
	{
		protected EnemyController _controller;

		protected HumanoidLevels enemyConfig;

		protected GameController _gameController;

		protected GameConfiguration _configurations
		{
			get
			{
				return MonoSingleton<GameConfiguration>.Instance;
			}
		}

		public EnemyConfigStrategy(EnemyController controller)
		{
			_controller = controller;
		}

		public void LoadConfiguration(ref Humanoid model)
		{
			_gameController = Object.FindObjectOfType<GameController>();
			if (_gameController == null)
			{
				Debug.LogWarning("Failed to find GameController in EnemyConfigStrategy, can't load configurations!");
				return;
			}
			LoadConfigurationLevels();
			if (_controller.UseDefaultConfig)
			{
				LoadAndSetMaterial(ref enemyConfig);
			}
			model = new Humanoid(enemyConfig);
			if (_gameController.LevelType != LevelType.Arena)
			{
				SetArmorAndBlasterFromPrefab(ref model);
				if (_controller.UseDefaultConfig)
				{
					UseResourceConfig(ref model);
				}
				else
				{
					UsePrefabConfig(ref model);
				}
			}
			else
			{
				UseResourceConfig(ref model);
			}
			GameObject gameObject = null;
			if (_controller.BlasterType != BlasterType.None)
			{
				gameObject = GetBlaster(model.Blaster.Type);
				GameObject gameObject2 = Object.Instantiate(gameObject, _controller.ShotPoint.position, Quaternion.identity);
				gameObject2.transform.parent = _controller.ShotPoint;
				gameObject2.transform.localPosition = Vector3.zero;
				gameObject2.transform.localEulerAngles = Vector3.zero;
			}
		}

		protected virtual void LoadConfigurationLevels()
		{
			int num = _controller.Level;
			if (_controller.UseAdaptiveConfig)
			{
				IDictionary<SlugType, Slug> slugs = _gameController.Model.Eli.Slugs;
				if (slugs != null)
				{
					num = 0;
					foreach (KeyValuePair<SlugType, Slug> item in slugs)
					{
						if (num < item.Value.Level)
						{
							num = item.Value.Level;
						}
					}
					if (num < _controller.Level)
					{
						num = _controller.Level;
					}
				}
			}
			_controller.SetLevel(num);
			enemyConfig = _configurations.UnitConfigurationManager.GetEnemiesConfigurationLevel(_controller.mEnemyType, num);
		}

		protected virtual GameObject GetBlaster(BlasterType type)
		{
			return GameFactory.GetEnemyBlasterPrefab(type);
		}

		protected virtual void UsePrefabConfig(ref Humanoid model)
		{
			model.MaxHealth = _controller.MaxHealth;
			model.AddHealth(model.MaxHealth, true);
		}

		protected void SetArmorAndBlasterFromPrefab(ref Humanoid model)
		{
			ArmorLevels armorParams = _configurations.ArmorConfigManager.GetArmorParams(_controller.ArmorType, _controller.ArmorLevel);
			if (armorParams == null)
			{
				armorParams = _configurations.ArmorConfigManager.GetArmorParams(ArmorType.Default, 0);
			}
			model.SetArmor(armorParams);
			if (_controller.BlasterType != BlasterType.None)
			{
				BlasterLevels blasterParams = _configurations.BlasterConfigManager.GetBlasterParams(_controller.BlasterType, _controller.BlasterLevel);
				model.SetBlaster(blasterParams, _controller.BlasterType);
			}
		}

		private void LoadAndSetMaterial(ref HumanoidLevels enemyConfig)
		{
			if (string.IsNullOrEmpty(enemyConfig.MaterialPath))
			{
				return;
			}
			Material material = Resources.Load<Material>(enemyConfig.MaterialPath);
			if (material == null)
			{
				Debug.LogWarning(string.Format("[Config: Failed to load material! check path in config {0}]", _controller.mEnemyType));
				return;
			}
			SkinnedMeshRenderer componentInChildren = _controller.GameObject.GetComponentInChildren<SkinnedMeshRenderer>();
			if (componentInChildren != null)
			{
				componentInChildren.material = material;
			}
			else
			{
				Debug.LogWarning("[Config: Failed to find child with skinnedMesh!]");
			}
		}

		protected virtual void UseResourceConfig(ref Humanoid model)
		{
			_controller.SetMaxHealth(model.MaxHealth);
		}
	}
}

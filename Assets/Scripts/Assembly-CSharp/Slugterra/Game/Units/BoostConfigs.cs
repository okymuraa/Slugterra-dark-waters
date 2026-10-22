using System;
using System.Collections.Generic;
using AppsMinistry.Core;
using Slugterra.Configuration;
using Slugterra.Domain.Game.Units;
using Slugterra.Domain.Game.Units.Slugs;
using Slugterra.Domain.Shops;
using Slugterra.Game.Mission;
using UnityEngine;

namespace Slugterra.Game.Units
{
	public static class BoostConfigs
	{
		private static Dictionary<TargetType, ITarget> _targets;

		private static Dictionary<ConsumableType, Type> _types;

		static BoostConfigs()
		{
			_targets = new Dictionary<TargetType, ITarget>();
			_types = new Dictionary<ConsumableType, Type>();
			_targets.Add(TargetType.Eli, new PlayerTarget());
			_targets.Add(TargetType.Assistant, new AssistanTarget());
			_targets.Add(TargetType.EliAndAssistant, new PlayerAndAssistantTarget());
			_targets.Add(TargetType.Area, new AreaTarget());
			_types.Add(ConsumableType.First, typeof(FirstAidKit));
			_types.Add(ConsumableType.SpeedBoost, typeof(SpeedMotion));
			_types.Add(ConsumableType.ArmorBoost, typeof(ArmorBoost));
			_types.Add(ConsumableType.SlugFood, typeof(SlugFoodBoost));
		}

		public static bool UseItem(ConsumableItem item)
		{
			if (item.Count >= 1)
			{
				Type value = null;
				if (_types.TryGetValue(item.Type, out value))
				{
					ITarget value2 = null;
					ConsumableProperties consumableProperties = MonoSingleton<GameConfiguration>.Instance.ConsumableConfigurationManager.GetConsumableProperties(item.Type);
					if (_targets.TryGetValue(consumableProperties.Target, out value2) && IsAnyReasonToUse(value, value2, consumableProperties))
					{
						value2.Apply(value, consumableProperties);
						item.UseBoost();
						if (item.Type == ConsumableType.First)
						{
							MonoSingleton<MissionStatisticsController>.Instance.Model.UsedMedKitCount++;
						}
						else if (item.Type == ConsumableType.SlugFood)
						{
							MonoSingleton<MissionStatisticsController>.Instance.Model.UsedSlugFoodCount++;
						}
						else if (item.Type == ConsumableType.SpeedBoost)
						{
							MonoSingleton<MissionStatisticsController>.Instance.Model.UsedSpeedBoostCount++;
						}
						else if (item.Type == ConsumableType.ArmorBoost)
						{
							MonoSingleton<MissionStatisticsController>.Instance.Model.UsedArmorBoostCount++;
						}
						return true;
					}
				}
			}
			return false;
		}

		private static bool IsAnyReasonToUse(Type boostType, ITarget target, ConsumableProperties config)
		{
			if (boostType == typeof(FirstAidKit))
			{
				return IsAnyReasonToUseHealthBoost(target.FindTarget(config));
			}
			if (boostType == typeof(SlugFoodBoost))
			{
				return IsAnyReasonToUseSlugFoodBoost(target.FindTarget(config));
			}
			return true;
		}

		private static bool IsAnyReasonToUseHealthBoost(object target)
		{
			if (target is UnitController)
			{
				UnitController unitController = target as UnitController;
				return unitController.Model.MaxHealth - unitController.Model.Health > Mathf.Epsilon && !unitController.Model.IsDead();
			}
			if (target is List<UnitController>)
			{
				List<UnitController> list = target as List<UnitController>;
				foreach (UnitController item in list)
				{
					if (item == null || !(item.Model.MaxHealth - item.Model.Health > Mathf.Epsilon) || item.Model.IsDead())
					{
						continue;
					}
					return true;
				}
				return false;
			}
			return true;
		}

		private static bool IsAnyReasonToUseSlugFoodBoost(object target)
		{
			if (target is UnitController)
			{
				UnitController controller = target as UnitController;
				return IsAnySlugCalldown(controller);
			}
			if (target is List<UnitController>)
			{
				List<UnitController> list = target as List<UnitController>;
				foreach (UnitController item in list)
				{
					if (item == null || !IsAnySlugCalldown(item))
					{
						continue;
					}
					return true;
				}
				return false;
			}
			return true;
		}

		private static bool IsAnySlugCalldown(UnitController controller)
		{
			Humanoid humanoid = controller.Model as Humanoid;
			if (humanoid != null)
			{
				IDictionary<SlugType, Slug> slugs = humanoid.Slugs;
				foreach (KeyValuePair<SlugType, Slug> item in slugs)
				{
					if (item.Value.NextFire > Time.time)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}

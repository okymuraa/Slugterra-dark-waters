using System;
using System.Collections.Generic;
using AppsMinistry.Core;
using Slugterra.Configuration;
using Slugterra.Game.Units.Enemy;
using Slugterra.UI.Shops;
using UnityEngine;

namespace Slugterra.Game
{
	public static class GameFactory
	{
		private class ContinuePrice
		{
			public PaymentButtonCurrency currency;

			public float price;
		}

		private static Dictionary<string, string> _stringResourcesPath;

		private static Dictionary<string, UnityEngine.Object> _loadedPrefab;

		private static readonly List<ContinuePrice> respawnPrices;

		static GameFactory()
		{
			respawnPrices = new List<ContinuePrice>
			{
				new ContinuePrice
				{
					currency = PaymentButtonCurrency.Coins,
					price = 100f
				},
				new ContinuePrice
				{
					currency = PaymentButtonCurrency.Coins,
					price = 200f
				},
				new ContinuePrice
				{
					currency = PaymentButtonCurrency.Coins,
					price = 500f
				},
				new ContinuePrice
				{
					currency = PaymentButtonCurrency.Gems,
					price = 30f
				}
			};
			_loadedPrefab = new Dictionary<string, UnityEngine.Object>();
			_stringResourcesPath = new Dictionary<string, string>();
			List<SlugConfig> slugConfigs = MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugConfigs();
			for (int i = 0; i < slugConfigs.Count; i++)
			{
				string text = null;
				text = ((!slugConfigs[i].isGhoul) ? slugConfigs[i].slugName : slugConfigs[i].ghoulName);
				_stringResourcesPath.Add(text + "_uiSlug", slugConfigs[i].uiPrefab);
				_stringResourcesPath.Add(text, slugConfigs[i].gamePrefab);
				_stringResourcesPath.Add(text + "_net", slugConfigs[i].gameNetworkPrefab);
			}
			List<BlasterConfig> blasterConfigs = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterConfigs();
			string text2 = "Prefabs/Equipments/Blasters/{0}";
			for (int j = 0; j < blasterConfigs.Count; j++)
			{
				_stringResourcesPath.Add(blasterConfigs[j].blasterName + "_uiBlast", blasterConfigs[j].uiPrefab);
				_stringResourcesPath.Add(blasterConfigs[j].blasterName, blasterConfigs[j].gamePrefab);
				_stringResourcesPath.Add(string.Format("{0}{1}", blasterConfigs[j].blasterName, "Enemy"), blasterConfigs[j].gamePrefab);
			}
			string format = "Materials/Units/{0}/{1}";
			_stringResourcesPath.Add(string.Format("{0}{1}", typeof(SentinelController), 0), string.Format(format, "Enemy/Sentinel", "SentinelMaterialLevel1"));
			_stringResourcesPath.Add(string.Format("{0}{1}", typeof(SentinelController), 1), string.Format(format, "Enemy/Sentinel", "SentinelMaterialLevel2"));
			_stringResourcesPath.Add(string.Format("{0}{1}", typeof(SentinelController), 2), string.Format(format, "Enemy/Sentinel", "SentinelMaterialLevel3"));
			_stringResourcesPath.Add(string.Format("{0}{1}", typeof(SentinelController), 3), string.Format(format, "Enemy/Sentinel", "SentinelMaterialLevel4"));
			_stringResourcesPath.Add(string.Format("{0}{1}", typeof(SentinelController), 4), string.Format(format, "Enemy/Sentinel", "SentinelMaterialLevel5"));
			_stringResourcesPath.Add(string.Format("{0}{1}", typeof(SentinelController), 5), string.Format(format, "Enemy/Sentinel", "SentinelMaterialLevel6"));
			_stringResourcesPath.Add(string.Format("{0}{1}", typeof(SentinelController), 6), string.Format(format, "Enemy/Sentinel", "SentinelMaterialLevel5"));
			_stringResourcesPath.Add(string.Format("{0}{1}", typeof(SentinelController), 7), string.Format(format, "Enemy/Sentinel", "SentinelMaterialLevel5"));
			_stringResourcesPath.Add(string.Format("{0}{1}", typeof(SentinelController), 8), string.Format(format, "Enemy/Sentinel", "SentinelMaterialLevel5"));
			_stringResourcesPath.Add(string.Format("{0}{1}", typeof(SentinelController), 9), string.Format(format, "Enemy/Sentinel", "SentinelMaterialLevel5"));
			List<ArmorConfig> armorConfigs = MonoSingleton<GameConfiguration>.Instance.ArmorConfigManager.GetArmorConfigs();
			for (int k = 0; k < armorConfigs.Count; k++)
			{
				_stringResourcesPath.Add(string.Format("Armor{0}", armorConfigs[k].armorName), armorConfigs[k].uiPrefab);
			}
			string format2 = "Prefabs/UI/Shop/Assistants/{0}";
			List<HumanoidConfig> allies = MonoSingleton<GameConfiguration>.Instance.UnitConfigurationManager.GetAllies();
			for (int l = 0; l < allies.Count; l++)
			{
				_stringResourcesPath.Add(allies[l].unitName + "_uiAlly", allies[l].uiPrefab);
				_stringResourcesPath.Add(allies[l].unitName, allies[l].gamePrefab);
			}
			List<FoeConfig> bosses = MonoSingleton<GameConfiguration>.Instance.UnitConfigurationManager.GetBosses();
			for (int m = 0; m < bosses.Count; m++)
			{
				_stringResourcesPath.Add(bosses[m].unitName + "_uiFoe", bosses[m].uiPrefab);
				_stringResourcesPath.Add(bosses[m].unitName, bosses[m].gamePrefab);
			}
			_stringResourcesPath.Add("EliShopPlatform", string.Format(format2, "prf_EliShop_Stand"));
			_stringResourcesPath.Add("EliArmorShopModel", "Prefabs/UI/Armors/EliArmor_ShopModel");
			string format3 = "Prefabs/UI/Shop/Consumables/{0}";
			_stringResourcesPath.Add(ConsumableType.First.ToString() + "_uiConsume", string.Format(format3, "prfConsumeItem_Health"));
			_stringResourcesPath.Add(ConsumableType.SlugFood.ToString() + "_uiConsume", string.Format(format3, "prfConsumeItem_SlugFood"));
			_stringResourcesPath.Add(ConsumableType.SpeedBoost.ToString() + "_uiConsume", string.Format(format3, "prfConsumeItem_Speed"));
			_stringResourcesPath.Add(ConsumableType.ArmorBoost.ToString() + "_uiConsume", string.Format(format3, "prfConsumeItem_Defend"));
			string format4 = "Prefabs/UI/{0}";
			_stringResourcesPath.Add(ConsumableType.First.ToString() + "_effect", string.Format(format4, "BoostFX_Healing"));
			_stringResourcesPath.Add(ConsumableType.First.ToString() + "_effectArena", string.Format(format4, "BoostFX_Healing"));
			format4 = "Prefabs/Game/Slug/Particle/{0}";
			_stringResourcesPath.Add(ConsumableType.SpeedBoost.ToString() + "_effect", string.Format(format4, "SpeedMoveAbilityPositiveParticle_GamePlay"));
			_stringResourcesPath.Add(ConsumableType.SpeedBoost.ToString() + "_effectArena", string.Format(format4, "SpeedMoveAbilityPositiveParticle_Arena"));
			_stringResourcesPath.Add(ConsumableType.ArmorBoost.ToString() + "_effect", string.Format(format4, "ArmourShildDebuffAbilityPositive_GamePlay"));
			_stringResourcesPath.Add(ConsumableType.ArmorBoost.ToString() + "_effectArena", string.Format(format4, "ArmourShildDebuffAbilityPositive_Arena"));
		}

		public static GameObject GetPrefab<T>() where T : MonoBehaviour
		{
			return GetPrefab(typeof(T));
		}

		public static GameObject GetPrefab(Type type)
		{
			return GetPrefab(type.ToString()) as GameObject;
		}

		private static UnityEngine.Object GetPrefab(string type)
		{
			if (!_stringResourcesPath.ContainsKey(type))
			{
				Debug.Log(string.Format("UnitFactory does not have string key {0}", type));
				return null;
			}
			if (!_loadedPrefab.ContainsKey(type))
			{
				_loadedPrefab[type] = Resources.Load(_stringResourcesPath[type]);
			}
			return _loadedPrefab[type];
		}

		public static GameObject GetSlugPrefab(SlugType slugType)
		{
			return GetPrefab(slugType.ToString()) as GameObject;
		}

		public static GameObject GetSlugPrefabUI(SlugType slugType)
		{
			return GetPrefab(slugType.ToString() + "_uiSlug") as GameObject;
		}

		public static GameObject GetSlugPrefabNet(SlugType slugType)
		{
			return GetPrefab(slugType.ToString() + "_net") as GameObject;
		}

		public static GameObject GetBlasterPrefab(BlasterType blaster)
		{
			return GetPrefab(blaster.ToString()) as GameObject;
		}

		public static GameObject GetBlasterPrefabUI(BlasterType blaster)
		{
			return GetPrefab(blaster.ToString() + "_uiBlast") as GameObject;
		}

		public static GameObject GetEnemyBlasterPrefab(BlasterType blaster)
		{
			return GetPrefab(blaster.ToString() + "Enemy") as GameObject;
		}

		public static Material GetUnitMaterial(Type unitType, int level)
		{
			return GetPrefab(string.Format("{0}{1}", unitType, level)) as Material;
		}

		public static Material GetArmorMaterial(ArmorType armorType)
		{
			return GetPrefab(string.Format("Armor{0}", armorType)) as Material;
		}

		public static GameObject GetAssistantUI(AllyType allyType)
		{
			return GetPrefab(allyType.ToString() + "_uiAlly") as GameObject;
		}

		public static GameObject GetFoeUI(string type)
		{
			return GetPrefab(type + "_uiFoe") as GameObject;
		}

		public static GameObject GetFoe(string type)
		{
			return GetPrefab(type) as GameObject;
		}

		public static GameObject GetAssistant(AllyType allyType)
		{
			return GetPrefab(allyType.ToString()) as GameObject;
		}

		public static GameObject GetShopArmor()
		{
			return GetPrefab("EliArmorShopModel") as GameObject;
		}

		public static GameObject GetConsumable(ConsumableType consumableType)
		{
			return GetPrefab(consumableType.ToString() + "_uiConsume") as GameObject;
		}

		public static GameObject GetEliShopPlatform()
		{
			return GetPrefab("EliShopPlatform") as GameObject;
		}

		public static GameObject GetBoostEffectGamePlay(ConsumableType consumableType)
		{
			return GetPrefab(consumableType.ToString() + "_effect") as GameObject;
		}

		public static GameObject GetBoostEffectArena(ConsumableType consumableType)
		{
			return GetPrefab(consumableType.ToString() + "_effectArena") as GameObject;
		}

		public static int GetRespawnCost(int respawnNumber)
		{
			if (respawnPrices.Count - 1 >= respawnNumber)
			{
				return Mathf.RoundToInt(respawnPrices[respawnNumber].price);
			}
			if (respawnNumber > respawnPrices.Count - 1)
			{
				return Mathf.RoundToInt(respawnPrices[respawnPrices.Count - 1].price);
			}
			return 0;
		}

		public static PaymentButtonCurrency GetRespawnCurrency(int respawnNumber)
		{
			if (respawnPrices.Count - 1 >= respawnNumber)
			{
				return respawnPrices[respawnNumber].currency;
			}
			if (respawnNumber > respawnPrices.Count - 1)
			{
				return respawnPrices[respawnPrices.Count - 1].currency;
			}
			return PaymentButtonCurrency.Coins;
		}

		public static bool CanRespawn(int respawnNumber)
		{
			return respawnNumber < respawnPrices.Count;
		}

		public static void Clear()
		{
			_loadedPrefab.Clear();
		}
	}
}

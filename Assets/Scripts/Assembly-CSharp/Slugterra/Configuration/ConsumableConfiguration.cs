using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Slugterra.Configuration
{
	public class ConsumableConfiguration
	{
		public const int SmallPack = 5;

		public const int LargePack = 10;

		private Dictionary<string, ConsumableType> consumableTypeByName = new Dictionary<string, ConsumableType>();

		private ConsumablesConfig_ consumables = new ConsumablesConfig_();

		public void Initialize()
		{
			TextAsset textAsset = (TextAsset)Resources.Load("Configs/Consumable/Consumables", typeof(TextAsset));
			if (textAsset != null)
			{
				consumables = JsonConvert.DeserializeObject<ConsumablesConfig_>(textAsset.text);
			}
			for (ConsumableType consumableType = ConsumableType.First; consumableType < ConsumableType.Last; consumableType++)
			{
				if (!consumableTypeByName.ContainsKey(consumableType.ToString()))
				{
					if (consumableType.ToString() == "First" || consumableType.ToString() == "HealthBoost")
					{
						consumableTypeByName.Add("First", consumableType);
						consumableTypeByName.Add("HealthBoost", consumableType);
					}
					else
					{
						consumableTypeByName.Add(consumableType.ToString(), consumableType);
					}
				}
			}
		}

		public ConsumableProperties GetConsumableProperties(ConsumableType type)
		{
			if (consumables.consumables.ContainsKey(type))
			{
				return consumables.consumables[type];
			}
			Debug.LogErrorFormat("ConsumableConfiguration::GetConsumableProperties Key {0} Not Found ", type);
			return null;
		}

		public float GetConsumablePrice(ConsumableType type, int packType)
		{
			ConsumableProperties consumableProperties = GetConsumableProperties(type);
			float result = consumableProperties.priceData.smallPackPrice;
			if (packType == 1)
			{
				result = consumableProperties.priceData.largePackPrice;
			}
			return result;
		}

		public ConsumableType GetConsumableTypeByName(string consumableName)
		{
			if (consumableTypeByName != null && consumableTypeByName.Count > 0 && !string.IsNullOrEmpty(consumableName) && consumableTypeByName.ContainsKey(consumableName))
			{
				return consumableTypeByName[consumableName];
			}
			return ConsumableType.None;
		}
	}
}

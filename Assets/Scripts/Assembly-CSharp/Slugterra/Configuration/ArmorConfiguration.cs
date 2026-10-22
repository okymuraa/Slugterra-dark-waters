using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Slugterra.Configuration
{
	public class ArmorConfiguration
	{
		public const int MAX_ARMOR_LEVEL = 6;

		private Armors armors = new Armors();

		private Dictionary<string, ArmorType> armorTypeByName = new Dictionary<string, ArmorType>();

		private Dictionary<string, int> armorByName = new Dictionary<string, int>();

		public void Initialize()
		{
			TextAsset textAsset = (TextAsset)Resources.Load("Configs/Equipments/Armors/Armors", typeof(TextAsset));
			if (textAsset != null)
			{
				armors = JsonConvert.DeserializeObject<Armors>(textAsset.text);
			}
			for (ArmorType armorType = ArmorType.Default; armorType < ArmorType.Size; armorType++)
			{
				if (!armorTypeByName.ContainsKey(armorType.ToString()))
				{
					armorTypeByName.Add(armorType.ToString(), armorType);
				}
			}
			if (armors == null || armors.armorsData == null)
			{
				return;
			}
			for (int i = 0; i < armors.armorsData.Count; i++)
			{
				string armorName = armors.armorsData[i].armorName;
				if (!armorByName.ContainsKey(armorName))
				{
					armorByName.Add(armorName, i);
				}
			}
		}

		public ArmorConfig GetArmorConfiguration(ArmorType armorName)
		{
			string key = armorName.ToString();
			if (armorByName.ContainsKey(key))
			{
				int index = armorByName[key];
				return armors.armorsData[index];
			}
			return null;
		}

		public ArmorLevels GetArmorParams(ArmorType armorName, int armorLevel)
		{
			string key = armorName.ToString();
			if (armorByName.ContainsKey(key))
			{
				int index = armorByName[key];
				ArmorConfig armorConfig = armors.armorsData[index];
				return armorConfig.levels[armorLevel];
			}
			return null;
		}

		public List<ArmorConfig> GetArmorConfigs()
		{
			return armors.armorsData;
		}

		public float GetArmorPriceGold(ArmorType armorName, int armorLevel)
		{
			ArmorLevels armorParams = GetArmorParams(armorName, armorLevel);
			return armorParams.priceGold;
		}

		public ArmorType GetArmorTypeByName(string armorName)
		{
			if (armorTypeByName != null && armorTypeByName.Count > 0 && !string.IsNullOrEmpty(armorName) && armorTypeByName.ContainsKey(armorName))
			{
				return armorTypeByName[armorName];
			}
			return ArmorType.Default;
		}
	}
}

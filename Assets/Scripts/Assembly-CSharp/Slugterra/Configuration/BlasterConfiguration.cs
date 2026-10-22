using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Slugterra.Configuration
{
	public class BlasterConfiguration
	{
		public const int MAX_BLASTER_LEVEL = 6;

		private Blasters blasters = new Blasters();

		private Dictionary<string, BlasterType> blasterTypeByName = new Dictionary<string, BlasterType>();

		private Dictionary<string, int> blasterByName = new Dictionary<string, int>();

		public float MaxBlasterEnergy { get; private set; }

		public void Initialize()
		{
			TextAsset textAsset = (TextAsset)Resources.Load("Configs/Equipments/Blasters/Blaster", typeof(TextAsset));
			if (textAsset != null)
			{
				blasters = JsonConvert.DeserializeObject<Blasters>(textAsset.text);
			}
			for (BlasterType blasterType = BlasterType.SimpleBlaster; blasterType < BlasterType.Size; blasterType++)
			{
				if (!blasterTypeByName.ContainsKey(blasterType.ToString()))
				{
					blasterTypeByName.Add(blasterType.ToString(), blasterType);
				}
			}
			if (blasters == null || blasters.blasterData == null)
			{
				return;
			}
			for (int i = 0; i < blasters.blasterData.Count; i++)
			{
				string blasterName = blasters.blasterData[i].blasterName;
				if (!blasterByName.ContainsKey(blasterName))
				{
					blasterByName.Add(blasterName, i);
				}
			}
			FindMaxEnergy();
		}

		public List<BlasterConfig> GetBlasterConfigs()
		{
			return blasters.blasterData;
		}

		public BlasterLevels GetBlasterParams(BlasterType blasterName, int levelNumber)
		{
			string key = blasterName.ToString();
			if (blasterByName.ContainsKey(key))
			{
				int index = blasterByName[key];
				BlasterConfig blasterConfig = blasters.blasterData[index];
				BlasterLevels result = blasterConfig.levels[0];
				if (blasterName != BlasterType.SimpleBlaster)
				{
					result = blasterConfig.levels[levelNumber];
				}
				return result;
			}
			return null;
		}

		public BlasterConfig GetBlusterConfiguration(BlasterType blasterType)
		{
			string key = blasterType.ToString();
			if (blasterByName.ContainsKey(key))
			{
				int index = blasterByName[key];
				return blasters.blasterData[index];
			}
			return null;
		}

		public float GetBlasterPriceGold(BlasterType blastType, int blastLevel)
		{
			BlasterLevels blasterParams = GetBlasterParams(blastType, blastLevel);
			return blasterParams.priceGold;
		}

		public float GetBlasterPriceGems(BlasterType blastType, int blastLevel)
		{
			BlasterLevels blasterParams = GetBlasterParams(blastType, blastLevel);
			return blasterParams.priceGems;
		}

		public string GetBlasterInApp(BlasterType blastType, int blastLevel)
		{
			BlasterLevels blasterParams = GetBlasterParams(blastType, blastLevel);
			return blasterParams.productId;
		}

		public BlasterType GetBlasterTypeByName(string blasterName)
		{
			if (blasterTypeByName != null && blasterTypeByName.Count > 0 && !string.IsNullOrEmpty(blasterName) && blasterTypeByName.ContainsKey(blasterName))
			{
				return blasterTypeByName[blasterName];
			}
			return BlasterType.None;
		}

		private void FindMaxEnergy()
		{
			MaxBlasterEnergy = 0f;
			for (int i = 0; i < blasters.blasterData.Count; i++)
			{
				BlasterConfig blasterConfig = blasters.blasterData[i];
				for (int j = 0; j < blasterConfig.levels.Count; j++)
				{
					BlasterLevels blasterLevels = blasterConfig.levels[j];
					if (blasterLevels.Energy > MaxBlasterEnergy)
					{
						MaxBlasterEnergy = blasterLevels.Energy;
					}
				}
			}
		}
	}
}

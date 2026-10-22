using System.Collections.Generic;
using Newtonsoft.Json;

namespace Slugterra.Configuration
{
	public class LevelProperties : BasePriceConfig
	{
		public string levelName = string.Empty;

		public string levelDesc = string.Empty;

		public Dictionary<int, string> levelObjectives = new Dictionary<int, string>();

		public List<ArmorType> armorsToUnlock = new List<ArmorType>();

		public List<SlugType> slugsToUnlock = new List<SlugType>();

		public List<BlasterType> blastersToUnlock = new List<BlasterType>();

		public List<AllyType> alliesToUnlock = new List<AllyType>();

		public List<AllyType> allyToObjective = new List<AllyType>();

		public List<SlugType> slugsToObjective = new List<SlugType>();

		public List<BlasterType> blastersToObjective = new List<BlasterType>();

		public List<ArmorType> armorsToObjective = new List<ArmorType>();

		public List<string> recommendedItems = new List<string>();

		public string specialPacks = string.Empty;

		public List<int> crystalsPrize = new List<int>();

		public List<string> bossesToUnlock = new List<string>();

		public bool loadFromAsset;

		public bool haveHDAsset;

		public string assetBundleName = string.Empty;

		public float mainObjectiveRewardGold = 100f;

		public float mainObjectiveRepeatRewardGold = 50f;

		public float firstObjectiveRewardGold = 100f;

		public float firstObjectiveRepeatRewardGold = 50f;

		public float secondaryObjectiveRewardGold = 100f;

		public float secondaryObjectiveRepeatRewardGold = 50f;

		public float allObjectivesCompleteAdditionalRewardGold;

		public float allObjectivesRepeatCompleteAdditionalRewardGold;

		public float mainObjectiveFailRewardGold = 100f;

		public float mainObjectiveRewardGems = 10f;

		public float mainObjectiveRepeatRewardGems = 5f;

		public float firstObjectiveRewardGems = 10f;

		public float firstObjectiveRepeatRewardGems = 5f;

		public float secondaryObjectiveRewardGems = 10f;

		public float secondaryObjectiveRepeatRewardGems = 5f;

		public float allObjectivesCompleteAdditionalRewardGems;

		public float allObjectivesRepeatCompleteAdditionalRewardGems;

		public float mainObjectiveFailRewardGems = 10f;

		[JsonIgnore]
		public float realMainObjectiveGoldReward;

		[JsonIgnore]
		public float realFirstObjectiveGoldReward;

		[JsonIgnore]
		public float realSecondaryObjectiveGoldReward;

		[JsonIgnore]
		public float realMainObjectiveGemsReward;

		[JsonIgnore]
		public float realFirstObjectiveGemsReward;

		[JsonIgnore]
		public float realSecondaryObjectiveGemsReward;

		[JsonIgnore]
		public float realAdditionalObjectiveGoldReward;

		[JsonIgnore]
		public float realAdditionalObjectiveGemsReward;

		[JsonIgnore]
		public float realMainObjectiveFailGoldReward;

		[JsonIgnore]
		public float realMainObjectiveFailGemsReward;

		public LevelProperties()
		{
			levelObjectives = new Dictionary<int, string>();
		}

		public LevelProperties(string name)
		{
			levelName = name;
		}
	}
}

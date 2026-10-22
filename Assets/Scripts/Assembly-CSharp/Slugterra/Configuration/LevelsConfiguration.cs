using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Slugterra.Configuration
{
	public class LevelsConfiguration
	{
		public const int PRIMARY_OBJECTIVE = 0;

		public const int SECOND_OBJECTIVE = 1;

		public const int THIRD_OBJECTIVE = 2;

		private GameLevels gameLevels;

		private Dictionary<string, int> levelsByName = new Dictionary<string, int>();

		public void Initialize()
		{
			TextAsset textAsset = (TextAsset)Resources.Load("Configs/Levels/Levels", typeof(TextAsset));
			if (textAsset != null)
			{
				gameLevels = JsonConvert.DeserializeObject<GameLevels>(textAsset.text);
			}
			levelsByName = new Dictionary<string, int>();
			if (gameLevels == null || gameLevels.levelsProperties == null)
			{
				return;
			}
			for (int i = 0; i < gameLevels.levelsProperties.Count; i++)
			{
				string levelName = gameLevels.levelsProperties[i].levelName;
				if (!levelsByName.ContainsKey(levelName))
				{
					levelsByName.Add(levelName, i);
				}
			}
		}

		public string GetLevelDesc(string levelName)
		{
			string result = "ouups!";
			if (gameLevels != null && levelsByName.ContainsKey(levelName))
			{
				LevelProperties levelProperties = gameLevels.levelsProperties[levelsByName[levelName]];
				result = levelProperties.levelDesc;
			}
			return result;
		}

		public bool GetLevelDestination(string levelName)
		{
			bool result = false;
			if (gameLevels != null && levelsByName.ContainsKey(levelName))
			{
				LevelProperties levelProperties = gameLevels.levelsProperties[levelsByName[levelName]];
				result = levelProperties.loadFromAsset;
			}
			return result;
		}

		public bool GetLevelHaveHD(string levelName)
		{
			bool result = false;
			if (gameLevels != null && levelsByName.ContainsKey(levelName))
			{
				LevelProperties levelProperties = gameLevels.levelsProperties[levelsByName[levelName]];
				result = levelProperties.haveHDAsset;
			}
			return result;
		}

		public string GetLevelAsset(string levelName)
		{
			string result = string.Empty;
			if (gameLevels != null && levelsByName.ContainsKey(levelName))
			{
				LevelProperties levelProperties = gameLevels.levelsProperties[levelsByName[levelName]];
				result = levelProperties.assetBundleName;
			}
			return result;
		}

		public string GetObjectiveDescription(string levelName, int objectiveNumber)
		{
			string result = "ouups!";
			if (gameLevels != null && levelsByName.ContainsKey(levelName))
			{
				LevelProperties levelProperties = gameLevels.levelsProperties[levelsByName[levelName]];
				if (levelProperties.levelObjectives.Count - 1 >= objectiveNumber)
				{
					result = levelProperties.levelObjectives[objectiveNumber];
				}
			}
			return result;
		}

		public LevelProperties GetLevelProperties(string levelName)
		{
			LevelProperties result = null;
			if (gameLevels != null && gameLevels.levelsProperties != null)
			{
				result = gameLevels.levelsProperties[levelsByName[levelName]];
			}
			return result;
		}

		public List<ArmorType> GetArmorsToUnlock(string levelName)
		{
			if (gameLevels != null && levelsByName.ContainsKey(levelName))
			{
				LevelProperties levelProperties = gameLevels.levelsProperties[levelsByName[levelName]];
				return levelProperties.armorsToUnlock;
			}
			return new List<ArmorType>();
		}

		public List<BlasterType> GetBlastersToUnlock(string levelName)
		{
			if (gameLevels != null && levelsByName.ContainsKey(levelName))
			{
				LevelProperties levelProperties = gameLevels.levelsProperties[levelsByName[levelName]];
				return levelProperties.blastersToUnlock;
			}
			return new List<BlasterType>();
		}

		public List<AllyType> GetAlliesToUnlock(string levelName)
		{
			if (gameLevels != null && levelsByName.ContainsKey(levelName))
			{
				LevelProperties levelProperties = gameLevels.levelsProperties[levelsByName[levelName]];
				return levelProperties.alliesToUnlock;
			}
			return new List<AllyType>();
		}

		public List<SlugType> GetSlugsToUnlock(string levelName)
		{
			if (gameLevels != null && levelsByName.ContainsKey(levelName))
			{
				LevelProperties levelProperties = gameLevels.levelsProperties[levelsByName[levelName]];
				return levelProperties.slugsToUnlock;
			}
			return new List<SlugType>();
		}

		public List<string> GetBossesToUnlock(string levelName)
		{
			if (gameLevels != null && levelsByName.ContainsKey(levelName))
			{
				LevelProperties levelProperties = gameLevels.levelsProperties[levelsByName[levelName]];
				return levelProperties.bossesToUnlock;
			}
			return new List<string>();
		}

		public List<int> GetCrystalPrize(string levelName)
		{
			if (gameLevels != null && levelsByName.ContainsKey(levelName))
			{
				LevelProperties levelProperties = gameLevels.levelsProperties[levelsByName[levelName]];
				return levelProperties.crystalsPrize;
			}
			List<int> list = new List<int>();
			list.Add(0);
			list.Add(0);
			list.Add(0);
			return list;
		}

		public List<string> GetLevelRecommendationsList(string levelName)
		{
			if (gameLevels != null && levelsByName.ContainsKey(levelName))
			{
				LevelProperties levelProperties = gameLevels.levelsProperties[levelsByName[levelName]];
				return levelProperties.recommendedItems;
			}
			return new List<string>();
		}

		public string GetLevelSpecialPack(string levelName)
		{
			if (gameLevels != null && levelsByName.ContainsKey(levelName))
			{
				LevelProperties levelProperties = gameLevels.levelsProperties[levelsByName[levelName]];
				return levelProperties.specialPacks;
			}
			return string.Empty;
		}

		public string GetLevelWhenArmorWillUnlocked(ArmorType armorType)
		{
			for (int i = 0; i < gameLevels.levelsProperties.Count; i++)
			{
				LevelProperties levelProperties = gameLevels.levelsProperties[i];
				if (levelProperties.armorsToUnlock != null && levelProperties.armorsToUnlock.Count > 0 && levelProperties.armorsToUnlock.Contains(armorType))
				{
					return levelProperties.levelName;
				}
			}
			return string.Empty;
		}

		public string GetLevelWhenSlugWillUnlocked(SlugType slugType)
		{
			for (int i = 0; i < gameLevels.levelsProperties.Count; i++)
			{
				LevelProperties levelProperties = gameLevels.levelsProperties[i];
				if (levelProperties.slugsToUnlock != null && levelProperties.slugsToUnlock.Count > 0 && levelProperties.slugsToUnlock.Contains(slugType))
				{
					return levelProperties.levelName;
				}
			}
			return string.Empty;
		}

		public string GetLevelWhenBlasterWillUnlocked(BlasterType blasterType)
		{
			for (int i = 0; i < gameLevels.levelsProperties.Count; i++)
			{
				LevelProperties levelProperties = gameLevels.levelsProperties[i];
				if (levelProperties.blastersToUnlock != null && levelProperties.blastersToUnlock.Count > 0 && levelProperties.blastersToUnlock.Contains(blasterType))
				{
					return levelProperties.levelName;
				}
			}
			return string.Empty;
		}

		public string GetLevelWhenAssistantWillUnlocked(AllyType assistantType)
		{
			for (int i = 0; i < gameLevels.levelsProperties.Count; i++)
			{
				LevelProperties levelProperties = gameLevels.levelsProperties[i];
				if (levelProperties.alliesToUnlock != null && levelProperties.alliesToUnlock.Count > 0 && levelProperties.alliesToUnlock.Contains(assistantType))
				{
					return levelProperties.levelName;
				}
			}
			return string.Empty;
		}

		public string GetLevelWhenBossWillUnlocked(string bossName)
		{
			for (int i = 0; i < gameLevels.levelsProperties.Count; i++)
			{
				LevelProperties levelProperties = gameLevels.levelsProperties[i];
				if (levelProperties.bossesToUnlock != null && levelProperties.bossesToUnlock.Count > 0 && levelProperties.bossesToUnlock.Contains(bossName))
				{
					return levelProperties.levelName;
				}
			}
			return string.Empty;
		}
	}
}

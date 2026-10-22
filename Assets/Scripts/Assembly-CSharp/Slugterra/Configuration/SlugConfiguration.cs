using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Slugterra.Configuration
{
	public class SlugConfiguration
	{
		public const int MAX_SLUG_LEVEL = 5;

		private Slugs slugs = new Slugs();

		private Dictionary<string, SlugType> slugTypeByName = new Dictionary<string, SlugType>();

		private Dictionary<string, int> slugByName = new Dictionary<string, int>();

		public void Initialize()
		{
			TextAsset textAsset = (TextAsset)Resources.Load("Configs/Units/Slugs/Slugs", typeof(TextAsset));
			if (textAsset != null)
			{
				slugs = JsonConvert.DeserializeObject<Slugs>(textAsset.text);
			}
			slugByName = new Dictionary<string, int>();
			for (SlugType slugType = SlugType.Flopper; slugType < SlugType.Size; slugType++)
			{
				if (!slugTypeByName.ContainsKey(slugType.ToString()))
				{
					slugTypeByName.Add(slugType.ToString(), slugType);
				}
			}
			for (int i = 0; i < slugs.slugsData.Count; i++)
			{
				string value = ((!slugs.slugsData[i].isGhoul) ? slugs.slugsData[i].slugName : slugs.slugsData[i].ghoulName);
				SlugType key = GameConfiguration.ParseEnum<SlugType>(value);
				SlugGroup value2 = GameConfiguration.ParseEnum<SlugGroup>(slugs.slugsData[i].slugGroup);
				SlugTypeHelper.TypeToGroup.Add(key, value2);
			}
			if (slugs != null && slugs.slugsData != null)
			{
				for (int j = 0; j < slugs.slugsData.Count; j++)
				{
					string key2 = ((!slugs.slugsData[j].isGhoul) ? slugs.slugsData[j].slugName : slugs.slugsData[j].ghoulName);
					slugByName.Add(key2, j);
				}
			}
		}

		public SlugType GetSlugTypeByName(string slugName)
		{
			if (slugTypeByName != null && slugTypeByName.Count > 0 && !string.IsNullOrEmpty(slugName) && slugTypeByName.ContainsKey(slugName))
			{
				return slugTypeByName[slugName];
			}
			return SlugType.None;
		}

		public SlugLevels GetSlugParams(SlugType slugType, int slugLevel)
		{
			string key = slugType.ToString();
			if (slugByName.ContainsKey(key))
			{
				int index = slugByName[key];
				SlugConfig slugConfig = slugs.slugsData[index];
				if (slugLevel < slugConfig.levels.Count)
				{
					return slugConfig.levels[slugLevel];
				}
				return slugConfig.levels[slugConfig.levels.Count - 1];
			}
			return null;
		}

		public float GetSlugPriceGold(SlugType slugType, int slugLevel)
		{
			SlugLevels slugParams = GetSlugParams(slugType, slugLevel);
			return slugParams.priceGold;
		}

		public float GetSlugPriceGems(SlugType slugType, int slugLevel)
		{
			SlugLevels slugParams = GetSlugParams(slugType, slugLevel);
			return slugParams.priceGems;
		}

		public float GetSlugPrices(SlugType slugType, int slugLevelFrom, int slugLevelTo)
		{
			float num = 0f;
			if (slugLevelFrom <= slugLevelTo)
			{
				for (int i = slugLevelFrom; i <= slugLevelTo; i++)
				{
					num += GetSlugPriceGold(slugType, i);
				}
			}
			else
			{
				num = GetSlugPriceGold(slugType, slugLevelFrom);
			}
			return num;
		}

		public SlugConfig GetSlugConfiguration(SlugType slugType)
		{
			string key = slugType.ToString();
			if (slugByName.ContainsKey(key))
			{
				int index = slugByName[key];
				return slugs.slugsData[index];
			}
			return null;
		}

		public List<SlugConfig> GetSlugConfigs()
		{
			return slugs.slugsData;
		}
	}
}

using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Slugterra.Configuration
{
	public class SpecialPacksConfiguration
	{
		private List<SpecialPacksConfig> SpecialPacks;

		public void Initialize()
		{
			TextAsset textAsset = (TextAsset)Resources.Load("Configs/SpecialPacks/SpecialPacks", typeof(TextAsset));
			if (textAsset != null)
			{
				SpecialPacks = JsonConvert.DeserializeObject<List<SpecialPacksConfig>>(textAsset.text);
			}
		}

		public SpecialPacksConfig GetSpecialPackById(string id)
		{
			return SpecialPacks.Find((SpecialPacksConfig config) => config.Id == id);
		}

		public List<SpecialPacksConfig> GetAllSpecialPacks()
		{
			return SpecialPacks;
		}

		public List<SpecialPacksConfig> GetSpecialPacksById(int id)
		{
			List<SpecialPacksConfig> list = new List<SpecialPacksConfig>();
			foreach (SpecialPacksConfig specialPack in SpecialPacks)
			{
				if (specialPack.ShowId <= id)
				{
					list.Add(specialPack);
				}
			}
			return list;
		}
	}
}

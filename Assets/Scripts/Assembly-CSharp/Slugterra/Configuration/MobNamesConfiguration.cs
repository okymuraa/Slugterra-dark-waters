using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Slugterra.Configuration
{
	public class MobNamesConfiguration
	{
		private MobNames mobNames;

		private Dictionary<string, string> mobsByName = new Dictionary<string, string>();

		public void Initialize()
		{
			TextAsset textAsset = (TextAsset)Resources.Load("Configs/Multiplayer/MobNames", typeof(TextAsset));
			if (textAsset != null)
			{
				mobNames = JsonConvert.DeserializeObject<MobNames>(textAsset.text);
			}
			mobsByName = new Dictionary<string, string>();
			if (mobNames != null && mobNames.mobNamesProperties != null)
			{
				for (int i = 0; i < mobNames.mobNamesProperties.Count; i++)
				{
					mobsByName.Add(mobNames.mobNamesProperties[i].Id, mobNames.mobNamesProperties[i].Name);
				}
			}
		}

		public string GetRandomMobName()
		{
			string result = "ouups!";
			if (mobsByName != null)
			{
				string key = "bot" + Random.Range(0, mobsByName.Count).ToString("#000");
				result = mobsByName[key];
			}
			return result;
		}
	}
}

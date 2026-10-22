using System;
using System.Collections.Generic;
using AppsMinistry.Core;
using Newtonsoft.Json;
using Slugterra.Configuration;

namespace Slugterra.Domain.Game.HealthRestoration
{
	public class RestoreHealthConfiguration : AbstractHealthConfiguration
	{
		private Dictionary<string, RestoreHealthData> _configs = new Dictionary<string, RestoreHealthData>();

		protected override string CONFIGURATION_FILE_NAME
		{
			get
			{
				return "RestoreHealthConfiguration.cfg";
			}
		}

		protected override string GetJSONObject()
		{
			return JsonConvert.SerializeObject(_configs);
		}

		protected override void LoadFromJSON(string str)
		{
			_configs = JsonConvert.DeserializeObject<Dictionary<string, RestoreHealthData>>(str);
		}

		protected override void InitDefaultValues()
		{
		}

		public AbstractHealthData GetHealthConfiguration(Enum type)
		{
			return GetHealthConfiguration(type.ToString(), ref _configs);
		}

		public float GetHealPrice(AllyType type, int level)
		{
			return MonoSingleton<GameConfiguration>.Instance.UnitConfigurationManager.GetAlliesConfigurationLevel(type, level).restorationPrice;
		}

		public float GetHealPrice(SlugType type, int level)
		{
			return MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugParams(type, level).restorationPrice;
		}

		public float GetHealPrice(BlasterType type, int level)
		{
			return MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterParams(type, level).restorationPrice;
		}

		public TimeSpan GetSmallestRestoreTime()
		{
			DateTime dateTime = DateTime.UtcNow.AddHours(1.0);
			foreach (KeyValuePair<string, RestoreHealthData> config in _configs)
			{
				if (!config.Value.IsFullyRestored && config.Value.TimeWhenObjectIsRestored < dateTime)
				{
					dateTime = config.Value.TimeWhenObjectIsRestored;
				}
			}
			return dateTime - DateTime.UtcNow;
		}

		public bool HasItemsToRestore()
		{
			foreach (KeyValuePair<string, RestoreHealthData> config in _configs)
			{
				if (!config.Value.IsFullyRestored)
				{
					return true;
				}
			}
			return false;
		}
	}
}

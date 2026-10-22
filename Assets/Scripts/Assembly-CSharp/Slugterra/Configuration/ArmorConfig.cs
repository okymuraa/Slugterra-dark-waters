using System.Collections.Generic;

namespace Slugterra.Configuration
{
	public class ArmorConfig : BaseConfig
	{
		public string armorName = string.Empty;

		public string gamePrefab = string.Empty;

		public string uiPrefab = string.Empty;

		public string uiIcon = string.Empty;

		public List<ArmorLevels> levels = new List<ArmorLevels>();

		public ArmorConfig()
		{
			levels = new List<ArmorLevels>();
		}
	}
}

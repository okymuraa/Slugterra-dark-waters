using System.Collections.Generic;

namespace Slugterra.Configuration
{
	public class BlasterConfig : BaseConfig
	{
		public string blasterName = string.Empty;

		public string gamePrefab = string.Empty;

		public string uiPrefab = string.Empty;

		public string uiIcon = string.Empty;

		public bool canBreak = true;

		public List<BlasterLevels> levels = new List<BlasterLevels>();

		public BlasterConfig()
		{
			levels = new List<BlasterLevels>();
		}
	}
}

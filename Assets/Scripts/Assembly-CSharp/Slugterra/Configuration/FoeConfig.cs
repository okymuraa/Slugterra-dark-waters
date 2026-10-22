using System.Collections.Generic;

namespace Slugterra.Configuration
{
	public class FoeConfig : BaseConfig
	{
		public string unitName = string.Empty;

		public string gamePrefab = string.Empty;

		public string uiPrefab = string.Empty;

		public List<FoeLevels> levels = new List<FoeLevels>();

		public FoeConfig()
		{
			levels = new List<FoeLevels>();
		}
	}
}

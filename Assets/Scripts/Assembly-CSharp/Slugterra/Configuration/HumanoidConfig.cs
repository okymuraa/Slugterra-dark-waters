using System.Collections.Generic;

namespace Slugterra.Configuration
{
	public class HumanoidConfig : BaseConfig
	{
		public string unitName = string.Empty;

		public string gamePrefab = string.Empty;

		public string uiPrefab = string.Empty;

		public List<HumanoidLevels> levels = new List<HumanoidLevels>();

		public HumanoidConfig()
		{
			levels = new List<HumanoidLevels>();
		}
	}
}

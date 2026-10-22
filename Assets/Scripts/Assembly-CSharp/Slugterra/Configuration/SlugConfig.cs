using System.Collections.Generic;

namespace Slugterra.Configuration
{
	public class SlugConfig : BaseConfig
	{
		public string slugName = string.Empty;

		public string slugGroup = string.Empty;

		public string ghoulName = string.Empty;

		public bool isGhoul;

		public string gamePrefab = string.Empty;

		public string gameNetworkPrefab = string.Empty;

		public string uiPrefab = string.Empty;

		public List<SlugLevels> levels = new List<SlugLevels>();

		public SlugConfig()
		{
			slugName = string.Empty;
			levels = new List<SlugLevels>();
		}
	}
}

using System.Collections.Generic;

namespace Slugterra.Configuration
{
	public class AnimalConfig : BaseConfig
	{
		public string unitName = string.Empty;

		public string gamePrefab = string.Empty;

		public string uiPrefab = string.Empty;

		public List<AnimalLevels> levels = new List<AnimalLevels>();

		public AnimalConfig()
		{
			levels = new List<AnimalLevels>();
		}
	}
}

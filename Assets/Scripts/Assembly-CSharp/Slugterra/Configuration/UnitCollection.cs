using System.Collections.Generic;

namespace Slugterra.Configuration
{
	public class UnitCollection
	{
		public List<HumanoidConfig> alliesConfigs = new List<HumanoidConfig>();

		public List<HumanoidConfig> enemiesConfigs = new List<HumanoidConfig>();

		public List<AnimalConfig> animalsConfigs = new List<AnimalConfig>();

		public List<FoeConfig> bossesConfigs = new List<FoeConfig>();
	}
}

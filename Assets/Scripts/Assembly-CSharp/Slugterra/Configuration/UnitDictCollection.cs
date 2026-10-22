using System.Collections.Generic;

namespace Slugterra.Configuration
{
	public class UnitDictCollection
	{
		public Dictionary<AllyType, HumanoidConfig> alliesConfigs = new Dictionary<AllyType, HumanoidConfig>();

		public Dictionary<AnimalType, AnimalConfig> animalsConfigs = new Dictionary<AnimalType, AnimalConfig>();

		public Dictionary<EnemyType, HumanoidConfig> enemyConfigs = new Dictionary<EnemyType, HumanoidConfig>();

		public Dictionary<EnemyType, FoeConfig> bossConfigs = new Dictionary<EnemyType, FoeConfig>();
	}
}

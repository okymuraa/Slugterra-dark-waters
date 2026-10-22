using Slugterra.Configuration;

namespace Slugterra.Domain.Game.Units.Mechanisms
{
	public class Wagon : Mechanism
	{
		public Wagon(float maxHealth, float speed)
			: base(maxHealth, speed)
		{
		}

		public Wagon(WagonConfig config)
			: base(config.Health, config.Speed)
		{
		}

		public Wagon(EnemyWagonConfig config)
			: base(config.Health, config.Speed)
		{
		}
	}
}

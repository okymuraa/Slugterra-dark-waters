using Slugterra.Configuration;

namespace Slugterra.Domain.Game.Units.Mechanisms
{
	public class Fernando : Mechanism
	{
		public Fernando(float maxHealth, float speed)
			: base(maxHealth, speed)
		{
		}

		public Fernando(FernandoConfig config)
			: base(config.Health, config.Speed)
		{
		}
	}
}

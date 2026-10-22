using Slugterra.Configuration;

namespace Slugterra.Domain.Game.Units
{
	public class Animal : Unit
	{
		private float _damage;

		private float _evasion;

		public float Damage
		{
			get
			{
				return _damage * CurrentAttackCoeficient();
			}
		}

		public float Evasion
		{
			get
			{
				return _evasion;
			}
		}

		public Animal(AnimalLevels config)
			: base(config)
		{
			_damage = config.Damage;
			_evasion = config.Evasion;
		}
	}
}

using AppsMinistry.Domain;

namespace Slugterra.Domain.Game.Environments
{
	public class Environment : DomainObject
	{
		public readonly float MaxHealth;

		private float _health;

		public float Health
		{
			get
			{
				return _health;
			}
			set
			{
				_health = value;
				CommitAndNotify();
			}
		}

		public Environment(float maxHealth = 2f)
		{
			MaxHealth = maxHealth;
			Health = maxHealth;
		}

		public bool IsDead()
		{
			if (Health > 0f)
			{
				return false;
			}
			return true;
		}
	}
}

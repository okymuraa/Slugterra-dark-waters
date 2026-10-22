using AppsMinistry.Domain;

namespace Slugterra.Domain.Game.Units.Mechanisms
{
	public class Mechanism : DomainObject
	{
		public float MaxHealth;

		private float _health;

		private float _speed;

		private float _speedAnimationTrailer;

		private float _speedAnimationFandango;

		private bool _brake;

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

		public float Speed
		{
			get
			{
				return _speed;
			}
			set
			{
				if (value > 0f)
				{
					_speed = value;
				}
			}
		}

		public float SpeedAnimationTrailer
		{
			get
			{
				return _speedAnimationTrailer;
			}
			set
			{
				if (value > 0f)
				{
					_speedAnimationTrailer = value;
				}
			}
		}

		public float SpeedAnimationFandango
		{
			get
			{
				return _speedAnimationFandango;
			}
			set
			{
				if (value > 0f)
				{
					_speedAnimationFandango = value;
				}
			}
		}

		public bool Brake
		{
			get
			{
				return _brake;
			}
			set
			{
				_brake = value;
				CommitAndNotify();
			}
		}

		public Mechanism(float maxHealth, float speed)
		{
			MaxHealth = maxHealth;
			_health = maxHealth;
			_speed = speed;
		}

		public bool IsDead()
		{
			if (_health > 0f)
			{
				return false;
			}
			return true;
		}
	}
}

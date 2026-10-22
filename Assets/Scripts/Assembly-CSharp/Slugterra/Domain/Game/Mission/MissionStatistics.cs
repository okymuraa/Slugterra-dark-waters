using AppsMinistry.Domain;

namespace Slugterra.Domain.Game.Mission
{
	internal class MissionStatistics : DomainObject
	{
		private int _killedEnemyCount;

		private int _killedByEnvironment;

		private float _damageEliTook;

		private int _usedMedKitCount;

		private int _usedSlugFoodCount;

		private int _usedArmorBoostCount;

		private int _usedSpeedBoostCount;

		private float _time;

		public int KilledEnemyCount
		{
			get
			{
				return _killedEnemyCount;
			}
			set
			{
				if (_killedEnemyCount != value && value >= 0)
				{
					_killedEnemyCount = value;
					CommitAndNotify();
				}
			}
		}

		public int KilledByEnvironment
		{
			get
			{
				return _killedByEnvironment;
			}
			set
			{
				if (_killedByEnvironment != value && value >= 0)
				{
					_killedByEnvironment = value;
					CommitAndNotify();
				}
			}
		}

		public float DamageEliTook
		{
			get
			{
				return _damageEliTook;
			}
			set
			{
				if (value != _damageEliTook)
				{
					_damageEliTook = value;
				}
				CommitAndNotify();
			}
		}

		public int UsedMedKitCount
		{
			get
			{
				return _usedMedKitCount;
			}
			set
			{
				if (_usedMedKitCount != value && value >= 0)
				{
					_usedMedKitCount = value;
					CommitAndNotify();
				}
			}
		}

		public int UsedSlugFoodCount
		{
			get
			{
				return _usedSlugFoodCount;
			}
			set
			{
				if (_usedSlugFoodCount != value && value >= 0)
				{
					_usedSlugFoodCount = value;
					CommitAndNotify();
				}
			}
		}

		public int UsedArmorBoostCount
		{
			get
			{
				return _usedArmorBoostCount;
			}
			set
			{
				if (_usedArmorBoostCount != value && value >= 0)
				{
					_usedArmorBoostCount = value;
					CommitAndNotify();
				}
			}
		}

		public int UsedSpeedBoostCount
		{
			get
			{
				return _usedSpeedBoostCount;
			}
			set
			{
				if (_usedSpeedBoostCount != value && value >= 0)
				{
					_usedSpeedBoostCount = value;
					CommitAndNotify();
				}
			}
		}

		public float Time
		{
			get
			{
				return _time;
			}
			set
			{
				_time = value;
			}
		}
	}
}

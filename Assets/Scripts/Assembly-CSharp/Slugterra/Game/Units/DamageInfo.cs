using UnityEngine;

namespace Slugterra.Game.Units
{
	public class DamageInfo
	{
		private Transform _parent;

		private float _time;

		private float _damage;

		public Transform Parent
		{
			get
			{
				return _parent;
			}
		}

		public float Time
		{
			get
			{
				return _time;
			}
		}

		public float Damage
		{
			get
			{
				return _damage;
			}
		}

		public DamageInfo(Transform parent, float time, float damage)
		{
			_parent = parent;
			_time = time;
			_damage = damage;
		}
	}
}

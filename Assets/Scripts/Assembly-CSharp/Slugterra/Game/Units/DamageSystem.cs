using System.Collections.Generic;
using UnityEngine;

namespace Slugterra.Game.Units
{
	public class DamageSystem : MonoBehaviour
	{
		private const float CLEAR_TIME = 3f;

		private IList<DamageInfo> _damages;

		private IDictionary<Transform, float> _damagePerUnit;

		private Transform _unitMaxDamage;

		private Transform _unitLastDamage;

		public Transform UnitMaxDamage
		{
			get
			{
				return _unitMaxDamage;
			}
		}

		public Transform UnitLastDamage
		{
			get
			{
				return _unitLastDamage;
			}
		}

		private void Awake()
		{
			_damages = new List<DamageInfo>();
			_damagePerUnit = new Dictionary<Transform, float>();
		}

		private void Update()
		{
			CalculateUnitDamage();
			FindUnitMaxDamage();
		}

		private void CalculateUnitDamage()
		{
			_damagePerUnit.Clear();
			for (int i = 0; i < _damages.Count; i++)
			{
				DamageInfo damageInfo = _damages[i];
				CharacterController characterController = null;
				if (damageInfo.Parent != null)
				{
					characterController = damageInfo.Parent.GetComponent<CharacterController>();
				}
				if (damageInfo.Time + 3f < Time.time || damageInfo.Parent == null || characterController == null || !characterController.enabled)
				{
					_damages.RemoveAt(i);
					i--;
					continue;
				}
				if (!_damagePerUnit.ContainsKey(damageInfo.Parent))
				{
					_damagePerUnit.Add(damageInfo.Parent, 0f);
				}
				_damagePerUnit[damageInfo.Parent] += damageInfo.Damage;
			}
		}

		private void FindUnitMaxDamage()
		{
			Transform transform = null;
			float num = 0f;
			if (_damagePerUnit.Count == 0)
			{
				_unitMaxDamage = null;
				_unitLastDamage = null;
				return;
			}
			foreach (Transform key in _damagePerUnit.Keys)
			{
				if (_damagePerUnit[key] > num)
				{
					transform = key;
					num = _damagePerUnit[key];
				}
			}
			if (transform != null && transform != _unitMaxDamage)
			{
				_unitMaxDamage = transform;
			}
		}

		public void OnDamage(Transform parent, float damage)
		{
			if (enabled && !(parent == null) && !(damage <= 0f))
			{
				_damages.Add(new DamageInfo(parent, Time.time, damage));
				_unitLastDamage = parent;
			}
		}

		public void Clear()
		{
			_damages.Clear();
			_damagePerUnit.Clear();
			_unitLastDamage = null;
			_unitMaxDamage = null;
		}
	}
}

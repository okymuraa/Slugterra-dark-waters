using System.Collections.Generic;
using UnityEngine;

namespace Slugterra.Game.Units.Slugs
{
	public class RandomTargetDamageEffect : UnitEffect
	{
		private List<UnitController> _unitControllers;

		private float _damage;

		private GameObject[] _effects;

		private float _finishTime;

		private float _nextDamage;

		private float _damageInterval = 1f;

		private Attacker _attacker;

		protected override void OnStart(object[] args)
		{
			_unitControllers = (List<UnitController>)args[0];
			_effects = new GameObject[_unitControllers.Count];
			float num = (float)args[1];
			_finishTime = Time.time + num;
			_damage = (float)args[2];
			_damageInterval = (float)args[3];
			_attacker = (Attacker)args[4];
			GameObject gameObject = (GameObject)args[5];
			if (gameObject != null)
			{
				for (int i = 0; i < _unitControllers.Count; i++)
				{
					_effects[i] = Object.Instantiate(gameObject, _unitControllers[i].Transform.position, Quaternion.identity);
					_effects[i].transform.parent = _unitControllers[i].Transform;
					_effects[i].transform.localPosition = Vector3.zero;
					Object.Destroy(_effects[i], num + 3f);
				}
			}
		}

		private void Update()
		{
			if (_finishTime <= Time.time || _unitControllers.Count == 0)
			{
				Finish();
				return;
			}
			for (int i = 0; i < _unitControllers.Count; i++)
			{
				if (_unitControllers[i] == null || _unitControllers[i].Model.IsDead())
				{
					_unitControllers.RemoveAt(i);
					i--;
				}
			}
			if (Time.time > _nextDamage)
			{
				float time = Time.time;
				if (_unitControllers.Count > 0)
				{
					int index = Random.Range(0, _unitControllers.Count);
					_unitControllers[index].OnDamage(_damage, _attacker);
					_nextDamage = time + _damageInterval;
				}
			}
		}

		protected override void OnFinish()
		{
			base.OnFinish();
			for (int i = 0; i < _effects.Length; i++)
			{
				if (_effects[i] != null)
				{
					_effects[i].GetComponent<ParticleSystem>().Stop();
				}
			}
		}
	}
}

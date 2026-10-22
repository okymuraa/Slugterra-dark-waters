using Slugterra.UI.Game;
using UnityEngine;

namespace Slugterra.Game.Units.Slugs
{
	public class DamageOverTimeEffect : UnitEffect
	{
		private UnitController _unitController;

		private float _dps;

		private GameObject _effect;

		private float _finishTime;

		private float _nextHelthUpdate;

		private float _lastUpdate;

		private const float SPAN = 0.1f;

		private float _appliedDamage;

		private float _fullDamage;

		private GameController _gameController;

		private bool _isMultiplayer;

		private Attacker _attacker;

		protected override void OnStart(object[] args)
		{
			_unitController = (UnitController)args[0];
			float num = (float)args[1];
			_finishTime = Time.time + num;
			_lastUpdate = Time.time;
			_dps = (float)args[2];
			_fullDamage = num * _dps;
			_attacker = (Attacker)args[3];
			if (_gameController == null)
			{
				_gameController = Object.FindObjectOfType<GameController>();
			}
			if (_gameController != null && _gameController.IsMultiplayer && _unitController != null)
			{
				_unitController.OnDamage(_fullDamage, _attacker);
				_appliedDamage = _fullDamage;
				_isMultiplayer = true;
				_nextHelthUpdate = _finishTime;
			}
			if (_effect == null)
			{
				GameObject gameObject = (GameObject)args[4];
				if (gameObject != null)
				{
					_effect = Object.Instantiate(gameObject, transform.position, Quaternion.identity);
					_effect.transform.parent = transform;
				}
			}
		}

		private void Update()
		{
			if (_unitController == null || _unitController.Model.IsDead())
			{
				Finish();
			}
			else if (_finishTime <= Time.time)
			{
				Finish();
			}
			else if (Time.time > _nextHelthUpdate && !_isMultiplayer)
			{
				float time = Time.time;
				float num = _dps * (time - _lastUpdate);
				_unitController.OnDamage(num, _attacker);
				_appliedDamage += num;
				_nextHelthUpdate = time + 0.1f;
				_lastUpdate = time;
			}
		}

		protected override void OnFinish()
		{
			base.OnFinish();
			if (_unitController != null)
			{
				_unitController.OnDamage(_fullDamage - _appliedDamage, _attacker);
				_unitController = null;
			}
			if (_effect != null)
			{
				Object.Destroy(_effect);
			}
		}
	}
}

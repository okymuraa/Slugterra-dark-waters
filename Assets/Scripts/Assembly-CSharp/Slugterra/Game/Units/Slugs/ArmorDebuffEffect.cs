using Slugterra.Domain.Game.Units;
using UnityEngine;

namespace Slugterra.Game.Units.Slugs
{
	public class ArmorDebuffEffect : UnitEffect
	{
		private float _finishTime;

		private Unit _unit;

		private GameObject _effect;

		protected override void OnStart(object[] args)
		{
			base.OnStart(args);
			_unit = (Unit)args[0];
			_finishTime = (float)args[1] + Time.time;
			float coeficient = (float)args[2];
			_unit.AddDamageCoeficient(GetType(), coeficient);
			if (_effect == null)
			{
				GameObject gameObject = args[3] as GameObject;
				if (gameObject != null)
				{
					_effect = Object.Instantiate(gameObject, transform.position, Quaternion.identity);
					_effect.transform.parent = transform;
				}
			}
		}

		private void Update()
		{
			if (_unit == null || _unit.IsDead())
			{
				Finish();
			}
			else if (_finishTime <= Time.time)
			{
				Finish();
			}
		}

		protected override void OnFinish()
		{
			base.OnFinish();
			if (_unit != null)
			{
				_unit.RemoveDamageCoeficient(GetType());
				_unit = null;
			}
			if (_effect != null)
			{
				Object.Destroy(_effect);
			}
		}
	}
}

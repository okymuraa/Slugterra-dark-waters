using Slugterra.Domain.Game.Units;
using UnityEngine;

namespace Slugterra.Game.Units.Slugs
{
	public class RegenerationEffect : UnitEffect
	{
		private float _finishTime;

		private Unit _model;

		private float _hpPerSecond;

		private GameObject _effect;

		protected override void OnStart(object[] args)
		{
			base.OnStart(args);
			_model = (Unit)args[0];
			_finishTime = (float)args[1] + Time.time;
			_hpPerSecond = (float)args[2];
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
			if (_model == null || _model.IsDead())
			{
				Finish();
			}
			else if (_finishTime <= Time.time)
			{
				Finish();
			}
			else
			{
				_model.Health += _hpPerSecond;
			}
		}

		protected override void OnFinish()
		{
			base.OnFinish();
			if (_model != null)
			{
				_model = null;
			}
			if (_effect != null)
			{
				Object.Destroy(_effect);
			}
		}
	}
}

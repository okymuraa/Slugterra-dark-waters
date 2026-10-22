using UnityEngine;

namespace Slugterra.Game.Units.Slugs
{
	public class BlackHoleEffect : UnitEffect
	{
		private float _finishTime;

		private float _time;

		private UnitController _unitController;

		private float _velocity;

		private Vector3 _targetPosition;

		private Vector3 _vectorVelocity;

		private GameObject _effect;

		protected override void OnStart(object[] args)
		{
			base.OnStart(args);
			_unitController = (UnitController)args[0];
			_time = (float)args[1];
			_finishTime = _time + Time.time;
			_targetPosition = (Vector3)args[2];
			_velocity = (float)args[3];
			_vectorVelocity = (_unitController.Transform.position - _targetPosition).normalized * _velocity;
			_unitController.Model.AddSpeedCoeficient(GetType(), 0f);
			if (_effect == null)
			{
				GameObject gameObject = args[4] as GameObject;
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
			else if (Vector3.SqrMagnitude(_unitController.Transform.position - _targetPosition) > 0.1f)
			{
				_unitController.Transform.position -= _vectorVelocity * Time.deltaTime;
			}
		}

		protected override void OnFinish()
		{
			base.OnFinish();
			if (_unitController != null)
			{
				_unitController.Model.AddSpeedCoeficient(GetType(), 0f);
				_unitController = null;
			}
			if (_effect != null)
			{
				Object.Destroy(_effect);
			}
		}
	}
}

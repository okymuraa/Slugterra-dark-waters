using UnityEngine;

namespace Slugterra.Game.Units.Slugs
{
	public class AttackRateEffect : UnitEffect
	{
		private float _finishTime;

		private HumanoidController _controller;

		private GameObject _effect;

		protected override void OnStart(object[] args)
		{
			base.OnStart(args);
			_controller = args[0] as HumanoidController;
			if (_controller == null)
			{
				Finish();
				return;
			}
			_finishTime = (float)args[1] + Time.time;
			_controller.Model.AddAttackSpeedCoeficient(GetType(), 1f / (float)args[2]);
			_controller.UpdateBlasterCallDown();
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
			if (_controller.Model == null || _controller.Model.IsDead())
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
			if (_controller != null && _controller.Model != null)
			{
				_controller.Model.RemoveAttackSpeedCoeficient(GetType());
				_controller.UpdateBlasterCallDown();
			}
			if (_effect != null)
			{
				Object.Destroy(_effect);
			}
		}
	}
}

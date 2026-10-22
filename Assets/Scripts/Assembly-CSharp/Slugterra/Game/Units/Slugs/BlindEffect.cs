using UnityEngine;

namespace Slugterra.Game.Units.Slugs
{
	public class BlindEffect : UnitEffect
	{
		private float _finishTime;

		private UnitController _unitController;

		private GameObject _effect;

		protected override void OnStart(object[] args)
		{
			base.OnStart(args);
			_unitController = (UnitController)args[0];
			_finishTime = (float)args[1] + Time.time;
			float num = (float)args[2];
			_unitController.DamageSystem.enabled = false;
			_unitController.DamageSystem.Clear();
			_unitController.AttackTarget = null;
			_unitController.SetViewRadius(_unitController.Model.ViewRadius * num);
			if (_unitController.NewVersionAI)
			{
				_unitController.MyBehavior.Blind();
			}
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
			if (_unitController == null || _unitController.Model.IsDead())
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
			if (_unitController != null)
			{
				if (_unitController.NewVersionAI)
				{
					_unitController.MyBehavior.UnBlind();
				}
				_unitController.SetViewRadius(_unitController.Model.ViewRadius);
				_unitController.DamageSystem.enabled = true;
				_unitController = null;
			}
			if (_effect != null)
			{
				Object.Destroy(_effect);
			}
		}
	}
}

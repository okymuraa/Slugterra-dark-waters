using UnityEngine;

namespace Slugterra.Game.Units.Slugs
{
	internal class SilenceEffect : UnitEffect
	{
		private float _finishTime;

		private UnitController _unitController;

		private GameObject _effect;

		private float _originRadius;

		protected override void OnStart(object[] args)
		{
			base.OnStart(args);
			_unitController = (UnitController)args[0];
			_finishTime = (float)args[1] + Time.time;
			float num = (float)args[2];
			_originRadius = _unitController.SocialRadius;
			_unitController.SocialRadius *= num;
			if (args.Length > 3)
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
				_unitController.SocialRadius = _originRadius;
				_unitController = null;
			}
			if (_effect != null)
			{
				Object.Destroy(_effect);
			}
		}
	}
}

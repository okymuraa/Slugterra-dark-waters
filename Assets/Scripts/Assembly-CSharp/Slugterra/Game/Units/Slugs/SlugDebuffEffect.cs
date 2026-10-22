using UnityEngine;

namespace Slugterra.Game.Units.Slugs
{
	public class SlugDebuffEffect : UnitEffect
	{
		private float _finishTime;

		private UnitController _controller;

		private GameObject _effect;

		protected override void OnStart(object[] args)
		{
			base.OnStart(args);
			_controller = (UnitController)args[0];
			_finishTime = (float)args[1] + Time.time;
			if (_effect == null)
			{
				GameObject gameObject = (GameObject)args[2];
				if (gameObject != null)
				{
					_effect = Object.Instantiate(gameObject, transform.position, Quaternion.identity);
					_effect.transform.parent = transform;
				}
			}
		}

		private void Update()
		{
			if (_controller == null)
			{
				Finish();
			}
			else if (_controller.Model.IsDead())
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
			if (_controller != null)
			{
				_controller = null;
			}
			if (_effect != null)
			{
				Object.Destroy(_effect);
			}
		}
	}
}

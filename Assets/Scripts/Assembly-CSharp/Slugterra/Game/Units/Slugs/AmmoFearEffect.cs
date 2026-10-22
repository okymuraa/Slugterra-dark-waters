using UnityEngine;

namespace Slugterra.Game.Units.Slugs
{
	internal class AmmoFearEffect : UnitEffect
	{
		private HumanoidController _controller;

		private float _finishTime;

		protected override void OnStart(object[] args)
		{
			base.OnStart(args);
			_controller = (HumanoidController)args[0];
			_finishTime = (float)args[1] + Time.time;
			_controller.DisabledSpecialSlug = true;
		}

		private void Update()
		{
			if (_finishTime <= Time.time)
			{
				Finish();
			}
		}

		protected override void OnFinish()
		{
			base.OnFinish();
			if (_controller != null)
			{
				_controller.DisabledSpecialSlug = false;
				_controller = null;
			}
		}
	}
}

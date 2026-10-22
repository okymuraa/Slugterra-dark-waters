using UnityEngine;

namespace Slugterra.Game.Units.Animals.SlugScorpion
{
	public class TurnState : SlugScorpionState
	{
		private const float ROTATE_TIME = 0.16f;

		private float _angle;

		private float _angleVelocity;

		private float _finishTime;

		protected override void OnStart(object[] args)
		{
			_angle = (float)args[0];
			_angleVelocity = 0f;
			float f = Mathf.DeltaAngle(Controller.Transform.eulerAngles.y, _angle);
			if (Mathf.Abs(f) < 15f)
			{
				Finish();
				return;
			}
			Controller.Animator.SetTrigger("Turn");
			_finishTime = Time.time + 0.3f;
		}

		protected override void OnUpdate()
		{
			float y = Controller.Transform.eulerAngles.y;
			if (_finishTime < Time.time)
			{
				Finish();
				return;
			}
			y = Mathf.SmoothDampAngle(y, _angle, ref _angleVelocity, 0.16f);
			Controller.Transform.eulerAngles = new Vector3(Controller.Transform.eulerAngles.x, y, Controller.Transform.eulerAngles.z);
		}
	}
}

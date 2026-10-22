using UnityEngine;

namespace Slugterra.Game.Units.Ally.Assistant
{
	public class AssistState : HumanoidState
	{
		private const float ROTATE_TIME = 0.16f;

		private Transform _enemyTransform;

		private RotateToCommand _rotateToCommand;

		private float _angle;

		private float _finishTime;

		private float _angleVelocity;

		protected override void OnStart(object[] args)
		{
			_enemyTransform = (Transform)args[1];
			Vector3 normalized = (_enemyTransform.position - Controller.Animator.transform.position).normalized;
			_angle = Quaternion.LookRotation(normalized).eulerAngles.y;
			_finishTime = Time.time + 0.3f;
			Setup(0f, Controller.Transform);
		}

		protected override void OnUpdate()
		{
			float y = Controller.Animator.transform.eulerAngles.y;
			if (_finishTime < Time.time)
			{
				Finish();
				return;
			}
			y = Mathf.SmoothDampAngle(y, _angle, ref _angleVelocity, 0.16f);
			Controller.Animator.transform.eulerAngles = new Vector3(Controller.Animator.transform.eulerAngles.x, y, Controller.Animator.transform.eulerAngles.z);
			if (Controller.ViewArea.Contains(_enemyTransform))
			{
				Controller.AttackTarget = _enemyTransform;
			}
		}
	}
}

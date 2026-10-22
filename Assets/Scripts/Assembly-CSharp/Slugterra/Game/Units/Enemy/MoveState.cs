using AppsMinistry.Core.Command;
using UnityEngine;

namespace Slugterra.Game.Units.Enemy
{
	public class MoveState : EnemyState
	{
		private Vector3 _targetPosition;

		private MoveCommand _moveCommand;

		protected override void OnStart(object[] args)
		{
			_targetPosition = (Vector3)args[0];
			Controller.Model.CurrentSpeed = (float)args[1];
			Controller.PathFinder.target = Controller.MoveTarget;
			Controller.PathFinder.target.position = _targetPosition;
			Controller.Animator.transform.localEulerAngles = Vector3.zero;
			_moveCommand = MonoCommand.ExecuteOn<MoveCommand>(gameObject, new object[3]
			{
				Controller,
				Controller.PathFinder.target,
				0.45f
			});
			_moveCommand.AsyncToken.AddResponder(new Responder<MoveCommand>(OnMoveFinishSuccessed, OnMoveFinishFailed));
		}

		protected override void OnUpdate()
		{
			Controller.Animator.SetFloat("VelocityX", Controller.PathFinder.MoveVelocity.x);
			Controller.Animator.SetFloat("VelocityY", Controller.PathFinder.MoveVelocity.z);
		}

		protected override void OnFinish()
		{
			if (_moveCommand != null)
			{
				Object.Destroy(_moveCommand);
			}
		}

		private void OnMoveFinishSuccessed(MoveCommand moveCommand)
		{
			Controller.Animator.SetFloat("VelocityX", 0f);
			Controller.Animator.SetFloat("VelocityY", 0f);
			Finish();
		}

		private void OnMoveFinishFailed(MoveCommand moveCommand)
		{
		}
	}
}

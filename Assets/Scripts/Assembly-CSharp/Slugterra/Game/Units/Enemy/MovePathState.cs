using AppsMinistry.Core.Command;
using UnityEngine;

namespace Slugterra.Game.Units.Enemy
{
	public class MovePathState : EnemyState
	{
		private MoveCommand _moveCommand;

		protected override void OnStart(object[] args)
		{
			Controller.PathFinder.target = Controller.MoveTarget;
			Controller.PathFinder.target.position = Controller.MovePath.GetNextPoint(Controller.MovePathIndex).position;
			Controller.Animator.transform.localEulerAngles = Vector3.zero;
			if (Controller.MovePath.UseCustomSpeed)
			{
				Controller.Model.CurrentSpeed = Controller.MovePath.CustomSpeed;
			}
			else
			{
				Controller.Model.Run();
			}
			_moveCommand = MonoCommand.ExecuteOn<MoveCommand>(gameObject, new object[3]
			{
				Controller,
				Controller.PathFinder.target,
				0.4f
			});
			_moveCommand.AsyncToken.AddResponder(new Responder<MoveCommand>(OnMoveFinishSuccessed, OnMoveFinishFailed));
		}

		protected override void OnUpdate()
		{
			float num = Vector3.SqrMagnitude(Controller.PathFinder.target.position - Controller.Transform.position);
			if (num < 2.25f && Controller.MovePathIndex < Controller.MovePath.GetPointCount() - 1)
			{
				Controller.MovePathIndex++;
				if (Controller.MovePath.GetNextPoint(Controller.MovePathIndex) != null)
				{
					Controller.PathFinder.target.position = Controller.MovePath.GetNextPoint(Controller.MovePathIndex).position;
					Controller.PathFinder.SearchPath();
				}
			}
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
			Finish();
		}

		private void OnMoveFinishFailed(MoveCommand moveCommand)
		{
		}
	}
}

using AppsMinistry.Core.Command;
using UnityEngine;

namespace Slugterra.Game.Units.Animals.DinoRat
{
	public class MovePathState : DinoRatState
	{
		private MoveCommand _moveCommand;

		protected override void OnStart(object[] args)
		{
			Controller.PathFinder.enabled = true;
			Controller.PathFinder.target = Controller.MoveTarget;
			Controller.PathFinder.target.position = Controller.MovePath.GetNextPoint(Controller.MovePathIndex).position;
			Controller.Animator.transform.localEulerAngles = Vector3.zero;
			Controller.Model.Run();
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
				Controller.PathFinder.target.position = Controller.MovePath.GetNextPoint(Controller.MovePathIndex).position;
				Controller.PathFinder.SearchPath();
			}
			Controller.Animator.SetFloat("Speed", Controller.Model.CurrentSpeed);
		}

		protected override void OnFinish()
		{
			if (_moveCommand != null)
			{
				Object.Destroy(_moveCommand);
			}
			Controller.Animator.SetFloat("Speed", 0f);
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

using AppsMinistry.Core.Command;
using Pathfinding;
using UnityEngine;

namespace Slugterra.Game.Units.Enemy
{
	public class RunAwayState : EnemyState
	{
		private Transform _finishPoint;

		private MoveCommand _moveCommand;

		protected override void OnStart(object[] args)
		{
			Controller.Animator.SetBool("EnemyInSight", false);
			Controller.RotateToTarget = false;
			Controller.Animator.transform.localEulerAngles = Vector3.zero;
			if (Controller.AttackTarget != null)
			{
				FleePath fleePath = FleePath.Construct(Controller.Transform.position, Controller.AttackTarget.position, 10000, OnPathComplete);
				fleePath.spread = 2000;
				fleePath.aimStrength = 0.5f;
				AstarPath.StartPath(fleePath);
			}
			else
			{
				RandomPath randomPath = RandomPath.Construct(Controller.Transform.position, 10000, OnPathComplete);
				randomPath.spread = 2000;
				randomPath.aimStrength = 0.5f;
				randomPath.aim = Vector3.zero;
				AstarPath.StartPath(randomPath);
			}
		}

		private void OnPathComplete(Path p)
		{
			if (!(Controller.PathFinder == null) && !(Controller == null) && !IsFinished)
			{
				_finishPoint = new GameObject("FinishPoint").transform;
				_finishPoint.position = ((RandomPath)p).endPoint;
				Controller.Model.Run();
				_moveCommand = MonoCommand.ExecuteOn<MoveCommand>(gameObject, new object[3] { Controller, _finishPoint, 0.45f });
				_moveCommand.AsyncToken.AddResponder(new Responder<MoveCommand>(OnMoveFinish));
			}
		}

		private void OnMoveFinish(MoveCommand moveCommand)
		{
			StateMachine.NextState<PatrolState>(new object[0]);
			Finish();
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

		protected override void OnReleaseResources()
		{
			if (_finishPoint != null)
			{
				Object.Destroy(_finishPoint.gameObject);
			}
		}
	}
}

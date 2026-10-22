using AppsMinistry.Core.Command;
using UnityEngine;

namespace Slugterra.Game.Units.Animals.DinoRat
{
	public class PatrolState : DinoRatState
	{
		private const float DISTANCE_TO_POINT = 0.45f;

		private float _minWaitTime;

		private float _maxWaitTime;

		private MonoCommand<WaitCommand> _waitCommand;

		private MonoCommand<MoveCommand> _moveCommand;

		protected override void OnStart(object[] args)
		{
			_minWaitTime = Controller.PatrolPath.MinWaitTime;
			_maxWaitTime = Controller.PatrolPath.MaxWaitTime;
			Controller.PathFinder.enabled = true;
			Controller.PathFinder.target = Controller.MoveTarget;
			Controller.Animator.transform.localEulerAngles = Vector3.zero;
			Wait();
		}

		private void Wait()
		{
			if (!IsFinished)
			{
				float num = Random.Range(_minWaitTime, _maxWaitTime);
				Controller.PathFinder.target = Controller.MoveTarget;
				Controller.PathFinder.target.position = Controller.Transform.position;
				Controller.PathFinder.speed = 0f;
				Controller.Animator.SetFloat("Speed", 0f);
				_waitCommand = MonoCommand.ExecuteOn<WaitCommand>(Controller.GameObject, new object[1] { num });
				_waitCommand.AsyncToken.AddResponder(new Responder<WaitCommand>(OnWaitFinish));
			}
		}

		private void OnWaitFinish(WaitCommand waitCommand)
		{
			Move();
		}

		private void Move()
		{
			if (!IsFinished)
			{
				Transform nextPoint = Controller.PatrolPath.GetNextPoint();
				if (nextPoint == null)
				{
					StateMachine.NextState<IdleState>(new object[0]);
					Finish();
					return;
				}
				Controller.Model.Walk();
				Controller.Animator.SetFloat("Speed", Controller.Model.CurrentSpeed);
				_moveCommand = MonoCommand.ExecuteOn<MoveCommand>(gameObject, new object[3] { Controller, nextPoint, 0.45f });
				_moveCommand.AsyncToken.AddResponder(new Responder<MoveCommand>(OnMoveFinish));
			}
		}

		private void OnMoveFinish(MoveCommand moveCommand)
		{
			Wait();
		}

		protected override void OnFinish()
		{
			if (_waitCommand != null)
			{
				Object.Destroy(_waitCommand);
			}
		}
	}
}

using AppsMinistry.Core.Command;
using UnityEngine;

namespace Slugterra.Game.Units.Enemy
{
	public class PatrolState : EnemyState
	{
		private const float DISTANCE_TO_POINT = 0.45f;

		private float _minWaitTime;

		private float _maxWaitTime;

		private MonoCommand<WaitCommand> _waitCommand;

		private MonoCommand<MoveCommand> _moveCommand;

		private Transform _nextPoint;

		private bool _rotate;

		private float _rotateTime;

		private float _angleVelocity;

		protected override void OnStart(object[] args)
		{
			_minWaitTime = Controller.PatrolPath.MinWaitTime;
			_maxWaitTime = Controller.PatrolPath.MaxWaitTime;
			Controller.PathFinder.target = Controller.MoveTarget;
			Controller.Animator.transform.localEulerAngles = Vector3.zero;
			_rotate = false;
			_rotateTime = 0.2f;
			float waitTime = Random.Range(_minWaitTime, _maxWaitTime);
			Wait(waitTime);
		}

		private void Wait(float waitTime)
		{
			if (!IsFinished)
			{
				Controller.PathFinder.target = Controller.MoveTarget;
				Controller.PathFinder.target.position = Controller.Transform.position;
				Controller.PathFinder.speed = 0f;
				_waitCommand = MonoCommand.ExecuteOn<WaitCommand>(Controller.GameObject, new object[1] { waitTime });
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
				_nextPoint = Controller.PatrolPath.GetNextPoint();
				if (_nextPoint == null)
				{
					StateMachine.NextState<IdleState>(new object[0]);
					Finish();
					return;
				}
				Controller.Model.Walk();
				_moveCommand = MonoCommand.ExecuteOn<MoveCommand>(gameObject, new object[3] { Controller, _nextPoint, 0.45f });
				_moveCommand.AsyncToken.AddResponder(new Responder<MoveCommand>(OnMoveFinish));
			}
		}

		private void OnMoveFinish(MoveCommand moveCommand)
		{
			if (_nextPoint != null)
			{
				_rotate = true;
			}
		}

		protected override void OnUpdate()
		{
			if (_rotate)
			{
				float num = Mathf.SmoothDampAngle(Controller.Transform.eulerAngles.y, _nextPoint.eulerAngles.y, ref _angleVelocity, _rotateTime);
				Controller.Transform.eulerAngles = new Vector3(Controller.Transform.eulerAngles.x, num, Controller.Transform.eulerAngles.z);
				if (Mathf.Abs(num - _nextPoint.eulerAngles.y) < 1f)
				{
					_rotate = false;
					float waitTime = ((Controller.PatrolPath.PathPoints.Length != 1) ? Random.Range(_minWaitTime, _maxWaitTime) : float.PositiveInfinity);
					Wait(waitTime);
					Controller.Animator.SetFloat("VelocityX", 0f);
					Controller.Animator.SetFloat("VelocityY", 0f);
				}
				else
				{
					Controller.Animator.SetFloat("VelocityX", 3f);
					Controller.Animator.SetFloat("VelocityY", 0f);
				}
			}
			else
			{
				Controller.Animator.SetFloat("VelocityX", Controller.PathFinder.MoveVelocity.x);
				Controller.Animator.SetFloat("VelocityY", Controller.PathFinder.MoveVelocity.z);
			}
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

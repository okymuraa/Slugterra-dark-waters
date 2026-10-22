using AppsMinistry.Core;
using AppsMinistry.Core.Command;
using Pathfinding;
using UnityEngine;

namespace Slugterra.Game.Units.Enemy
{
	public class HideState : EnemyState
	{
		private float _hideTime;

		private Vector3 _lastPosition;

		private CoverNode _coverNode;

		private Transform _controlPoint;

		private MoveCommand _moveCommand;

		private Transform _finishPoint;

		protected override void OnStart(object[] args)
		{
			_hideTime = (float)args[0];
			_lastPosition = Controller.Transform.position;
			_finishPoint = new GameObject("FinishPoint").transform;
			_finishPoint.transform.parent = Controller.Transform.parent;
			Hide();
		}

		private void Hide()
		{
			if (_coverNode != null && _controlPoint != null)
			{
				_coverNode.ReleasePoint(_controlPoint);
			}
			_coverNode = null;
			_coverNode = MonoSingleton<CoverSystem>.Instance.GetOptimalNode(Controller.Transform, Controller.AttackTarget, Controller.Model.ViewRadius);
			if (_coverNode == null)
			{
				_coverNode = MonoSingleton<CoverSystem>.Instance.GetNearestNode(Controller.Transform, Controller.AttackTarget, Controller.Model.ViewRadius);
			}
			if (_coverNode != null && _coverNode.IsContainEmptyPoint())
			{
				Transform controlPoint = _coverNode.GetControlPoint(Controller.AttackTarget);
				if (controlPoint != null && controlPoint != _controlPoint)
				{
					_controlPoint = controlPoint;
					MoveToControlPoint();
				}
				else
				{
					OnChase();
				}
			}
			else
			{
				OnChase();
			}
		}

		private void OnChase()
		{
			StateMachine.NextState<ChaseState>(new object[4] { Controller.AttackTarget, Controller.Model, Controller.MoveToWarPointChance, Controller.WaitInWarPointTime });
			Finish();
		}

		private void MoveToControlPoint()
		{
			Controller.Model.Run();
			_moveCommand = MonoCommand.ExecuteOn<MoveCommand>(gameObject, new object[3] { Controller, _controlPoint, 0.45f });
			_moveCommand.AsyncToken.AddResponder(new Responder<MoveCommand>(OnMoveToControlPointFinish));
		}

		private void MoveFromControlPoint()
		{
			Controller.Model.Run();
			if (Controller.AttackTarget != null)
			{
				RandomPath randomPath = RandomPath.Construct(Controller.Transform.position, 1000, OnPathComplete);
				randomPath.spread = 1000;
				randomPath.aimStrength = 0.5f;
				randomPath.aim = Vector3.zero;
				AstarPath.StartPath(randomPath);
			}
			else
			{
				_finishPoint.position = _lastPosition;
				_moveCommand = MonoCommand.ExecuteOn<MoveCommand>(gameObject, new object[3] { Controller, _finishPoint, 0.45f });
				_moveCommand.AsyncToken.AddResponder(new Responder<MoveCommand>(OnMoveFromControlPointFinish));
			}
		}

		private void OnPathComplete(Path p)
		{
			if (!(Controller.PathFinder == null) && !(Controller == null) && !IsFinished)
			{
				_finishPoint.position = ((RandomPath)p).endPoint;
				_moveCommand = MonoCommand.ExecuteOn<MoveCommand>(gameObject, new object[3] { Controller, _finishPoint, 0.45f });
				_moveCommand.AsyncToken.AddResponder(new Responder<MoveCommand>(OnMoveFromControlPointFinish));
			}
		}

		private void OnMoveToControlPointFinish(MoveCommand moveCommand)
		{
			Controller.Model.CurrentSpeed = 0f;
			ScheduleUpdate(_hideTime);
		}

		private void OnMoveFromControlPointFinish(MoveCommand moveCommand)
		{
			if (Controller.AttackTarget != null)
			{
				Hide();
				return;
			}
			StateMachine.NextState<PatrolState>(new object[0]);
			Finish();
		}

		protected override void OnUpdate()
		{
			Controller.Animator.SetFloat("VelocityX", Controller.PathFinder.MoveVelocity.x);
			Controller.Animator.SetFloat("VelocityY", Controller.PathFinder.MoveVelocity.z);
		}

		protected override void OnScheduleUpdate()
		{
			MoveFromControlPoint();
		}

		protected override void OnFinish()
		{
			if (_coverNode != null && _controlPoint != null)
			{
				_coverNode.ReleasePoint(_controlPoint);
			}
			if (_moveCommand != null)
			{
				Object.Destroy(_moveCommand);
			}
		}

		protected override void OnReleaseResources()
		{
			StopAllCoroutines();
			if (_finishPoint != null)
			{
				Object.Destroy(_finishPoint.gameObject);
			}
		}
	}
}

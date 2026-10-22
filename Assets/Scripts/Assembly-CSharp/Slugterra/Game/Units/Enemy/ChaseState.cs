using AppsMinistry.Core;
using AppsMinistry.Core.Command;
using Pathfinding;
using Slugterra.Domain.Game.Units;
using UnityEngine;

namespace Slugterra.Game.Units.Enemy
{
	public class ChaseState : EnemyState
	{
		private Transform _target;

		private float _distanceForFire;

		private WarNode _oldWarNode;

		private Transform _oldWarPoint;

		private MonoCommand<MoveCommand> _moveCommand;

		private float _chance;

		private float _waitTime;

		private float _nextMove;

		protected override void OnStart(object[] args)
		{
			_target = (Transform)args[0];
			_distanceForFire = (Controller.Model as Humanoid).Blaster.Range;
			_chance = (float)args[2];
			_waitTime = (float)args[3];
			MoveToWarNode();
		}

		private void MoveToWarNode()
		{
			Controller.Model.Run();
			_nextMove = Time.time + _waitTime;
			float num = Random.Range(0f, 1f);
			if (Controller.AttackTarget == null)
			{
				return;
			}
			WarNode node = MonoSingleton<WarSystem>.Instance.GetNode(Controller.Transform, Controller.AttackTarget, _distanceForFire);
			if (node == null)
			{
				if (IsOutOfBlasterRange())
				{
					MoveToTarget();
				}
				else if (num < _chance)
				{
					MoveToRandomPoint();
				}
			}
			else if (num < _chance)
			{
				Transform optimalPoint = node.GetOptimalPoint(Controller.Transform.position, Controller.AttackTarget.position, _distanceForFire);
				if (_oldWarNode != null && _oldWarPoint != null)
				{
					_oldWarNode.ReleasePoint(_oldWarPoint);
				}
				_oldWarNode = node;
				_oldWarPoint = optimalPoint;
				_moveCommand = MonoCommand.ExecuteOn<MoveCommand>(gameObject, new object[3] { Controller, optimalPoint, 0.45f });
				_moveCommand.AsyncToken.AddResponder(new Responder<MoveCommand>(OnMoveFinish));
			}
		}

		private void MoveToTarget()
		{
			_moveCommand = MonoCommand.ExecuteOn<MoveCommand>(gameObject, new object[3] { Controller, _target, _distanceForFire });
			_moveCommand.AsyncToken.AddResponder(new Responder<MoveCommand>(OnMoveFinish));
		}

		private void MoveToRandomPoint()
		{
			FleePath fleePath = FleePath.Construct(Controller.Transform.position, Controller.AttackTarget.position, 1500, OnPathComplete);
			fleePath.spread = 1000;
			fleePath.aimStrength = 0.5f;
			AstarPath.StartPath(fleePath);
		}

		private void OnPathComplete(Path p)
		{
			if (!(Controller.PathFinder == null) && !(Controller == null) && !IsFinished)
			{
				Controller.Model.Run();
				Controller.PathFinder.target.position = ((FleePath)p).endPoint;
				_moveCommand = MonoCommand.ExecuteOn<MoveCommand>(gameObject, new object[3]
				{
					Controller,
					Controller.PathFinder.target,
					0.45f
				});
				_moveCommand.AsyncToken.AddResponder(new Responder<MoveCommand>(OnMoveFinish));
			}
		}

		private void OnMoveFinish(MoveCommand moveCommand)
		{
			_moveCommand = null;
		}

		protected override void OnUpdate()
		{
			if (Controller.AttackTarget == null || Controller.AttackTarget != _target)
			{
				Finish();
				return;
			}
			if (_moveCommand == null && (IsOutOfBlasterRange() || _nextMove <= Time.time))
			{
				MoveToWarNode();
			}
			Controller.Animator.SetFloat("VelocityX", Controller.PathFinder.MoveVelocity.x);
			Controller.Animator.SetFloat("VelocityY", Controller.PathFinder.MoveVelocity.z);
		}

		private bool IsOutOfBlasterRange()
		{
			float num = Vector3.SqrMagnitude(_target.position - Controller.Transform.position);
			float num2 = _distanceForFire * _distanceForFire;
			return num > num2;
		}

		protected override void OnFinish()
		{
			Controller.Animator.SetFloat("VelocityX", 0f);
			Controller.Animator.SetFloat("VelocityY", 0f);
			if (_oldWarNode != null && _oldWarPoint != null)
			{
				_oldWarNode.ReleasePoint(_oldWarPoint);
			}
		}
	}
}

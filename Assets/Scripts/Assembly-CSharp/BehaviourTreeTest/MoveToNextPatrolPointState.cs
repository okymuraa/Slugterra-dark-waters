using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Slugterra.Game.Units;
using UnityEngine;

namespace BehaviourTreeTest
{
	[TaskCategory("Slugterra/Units")]
	public class MoveToNextPatrolPointState : Action
	{
		public UnitControllerVar unitController;

		public PathHolderVar pathHolder;

		public SharedBool isAlive;

		public SharedBool istargetFinded;

		private UnitController _unitController;

		private PatrolPathHolder _pathHolder;

		private TaskStatus _currentStatus;

		private Transform _nextPathPoint;

		private void MoveToNextPoint()
		{
			_nextPathPoint = _pathHolder.GetNextPathPoint();
			_unitController.PathFinder.pickNextWaypointDist = 10f;
			_unitController.PathFinder.target = _nextPathPoint;
			_unitController.PathFinder.speed = _unitController.Model.WalkSpeed;
			_unitController.PathFinder.canMove = true;
			_unitController.PathFinder.SearchPath();
			if (_unitController.PathFinder.speed == 0f)
			{
				_unitController.PathFinder.speed = 2f;
			}
			_unitController.Model.Walk();
		}

		public override void OnStart()
		{
			if (_unitController == null)
			{
				_unitController = unitController.Value;
			}
			if (_pathHolder == null)
			{
				_pathHolder = pathHolder.Value;
			}
			_currentStatus = TaskStatus.Running;
			if (isAlive.Value)
			{
				MoveToNextPoint();
			}
			else
			{
				_currentStatus = TaskStatus.Failure;
			}
		}

		public override TaskStatus OnUpdate()
		{
			_unitController.Animator.SetFloat("VelocityX", _unitController.PathFinder.MoveVelocity.x);
			_unitController.Animator.SetFloat("VelocityY", _unitController.PathFinder.MoveVelocity.z);
			if (!isAlive.Value)
			{
				_unitController.PathFinder.enabled = false;
				_currentStatus = TaskStatus.Failure;
			}
			if (istargetFinded.Value)
			{
				_currentStatus = TaskStatus.Failure;
			}
			else if (_unitController.PathFinder.TargetReached)
			{
				_currentStatus = TaskStatus.Success;
			}
			return _currentStatus;
		}
	}
}

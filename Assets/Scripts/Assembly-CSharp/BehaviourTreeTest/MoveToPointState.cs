using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Slugterra.Game.Units;
using UnityEngine;

namespace BehaviourTreeTest
{
	[TaskCategory("Slugterra/Units")]
	public class MoveToPointState : Action
	{
		public UnitControllerVar unitController;

		public SharedTransform nextPathPoint;

		public SharedFloat moveSpeed;

		public SharedBool recalculatePath;

		public SharedBool isAlive;

		public SharedVector3 announceTargetPosition;

		private UnitController _unitController;

		private Transform _nextPathPoint;

		private TaskStatus _currentStatus;

		public override void OnStart()
		{
			if (_unitController == null)
			{
				_unitController = unitController.Value;
			}
			if (_nextPathPoint == null)
			{
				_nextPathPoint = new GameObject("~customPathPoint").transform;
			}
			_nextPathPoint.parent = _unitController.transform;
			_nextPathPoint.position = announceTargetPosition.Value;
			_currentStatus = TaskStatus.Running;
		}

		public override TaskStatus OnUpdate()
		{
			_unitController.Animator.SetFloat("VelocityX", _unitController.PathFinder.MoveVelocity.x);
			_unitController.Animator.SetFloat("VelocityY", _unitController.PathFinder.MoveVelocity.z);
			if (!isAlive.Value)
			{
				_unitController.PathFinder.canMove = false;
				_currentStatus = TaskStatus.Failure;
			}
			else if (nextPathPoint.Value != null && recalculatePath.Value)
			{
				recalculatePath.Value = false;
				_nextPathPoint = nextPathPoint.Value;
				_unitController.PathFinder.target = _nextPathPoint;
				_unitController.PathFinder.speed = ((moveSpeed.Value != 0f) ? moveSpeed.Value : _unitController.Model.WalkSpeed);
				_unitController.PathFinder.canMove = true;
				_unitController.PathFinder.SearchPath();
			}
			else if (_unitController.PathFinder.TargetReached)
			{
				_currentStatus = TaskStatus.Success;
			}
			return _currentStatus;
		}
	}
}

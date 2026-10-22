using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Slugterra.Domain.Game.Units;
using Slugterra.Domain.Game.Units.Slugs;
using Slugterra.Game.Units;
using UnityEngine;

namespace BehaviourTreeTest
{
	[TaskCategory("Slugterra/Units")]
	public class ChaseState : Action
	{
		public SharedTransform attackTarget;

		public UnitControllerVar attackTargetController;

		public UnitControllerVar unitController;

		public SharedBool isAlive;

		public SharedBool isTargetFinded;

		private UnitController _unitController;

		private Transform _attackTarget;

		private Transform _shootPoint;

		private Slug _slug;

		private Transform _movePoint = new GameObject("~movePoint").transform;

		private float _distanceForFire;

		private float _distanceForMe;

		private Vector3 _oldPos;

		private Quaternion _oldAngle;

		private float _oldUnitAroundAngle;

		private float _shootRange;

		private TaskStatus _currentStatus;

		public override void OnStart()
		{
			if (_unitController == null)
			{
				_unitController = unitController.Value;
			}
			if (_attackTarget == null)
			{
				_attackTarget = attackTarget.Value;
			}
			if (isAlive.Value && isTargetFinded.Value)
			{
				_currentStatus = TaskStatus.Running;
				Chase();
			}
			else
			{
				_currentStatus = TaskStatus.Failure;
			}
		}

		private void Chase()
		{
			_distanceForFire = (_unitController.Model as Humanoid).Blaster.Range;
			_distanceForMe = 0.35f * _distanceForFire;
			float num = Vector3.Angle(Vector3.forward, (_unitController.transform.position - _attackTarget.position).normalized);
			num = ((!((_unitController.transform.position - _attackTarget.position).x > 0f)) ? (360f - num) : num);
			num += Mathf.Sign(Random.Range(-1, 1)) * 30f;
			Vector3 position = _attackTarget.position + Quaternion.AngleAxis(num, Vector3.up) * Vector3.forward * _distanceForMe;
			_movePoint.position = position;
			_movePoint.parent = attackTargetController.Value.transform;
			_movePoint.localPosition = new Vector3(_movePoint.localPosition.x, 0f, _movePoint.localPosition.z);
			_movePoint.rotation = Quaternion.Euler(0f, num, 0f);
			_unitController.PathFinder.target = _movePoint;
			_unitController.PathFinder.speed = _unitController.Model.RunSpeed;
			_unitController.PathFinder.canMove = true;
			_unitController.PathFinder.SearchPath();
			_unitController.Model.Run();
		}

		public override TaskStatus OnUpdate()
		{
			if (_currentStatus == TaskStatus.Running)
			{
				_unitController.Animator.SetFloat("VelocityX", _unitController.PathFinder.MoveVelocity.x);
				_unitController.Animator.SetFloat("VelocityY", _unitController.PathFinder.MoveVelocity.z);
				if (!isAlive.Value || !isTargetFinded.Value)
				{
					_currentStatus = TaskStatus.Failure;
				}
				if (Vector3.Distance(_oldPos, _unitController.transform.position) < _unitController.Model.RunSpeed * Time.deltaTime && Mathf.Abs(_oldAngle.eulerAngles.y - _unitController.transform.rotation.eulerAngles.y) < 5f * Time.deltaTime)
				{
					_currentStatus = TaskStatus.Success;
				}
				_oldPos = _unitController.transform.position;
				_oldAngle = _unitController.transform.rotation;
			}
			if (!isAlive.Value)
			{
				_unitController.PathFinder.canMove = false;
			}
			return _currentStatus;
		}
	}
}

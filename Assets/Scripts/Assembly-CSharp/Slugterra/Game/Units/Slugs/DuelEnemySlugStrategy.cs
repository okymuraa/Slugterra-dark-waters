using System;
using AppsMinistry.Core;
using Slugterra.Game.Duel;
using UnityEngine;

namespace Slugterra.Game.Units.Slugs
{
	public class DuelEnemySlugStrategy : EnemySlugStrategy
	{
		private const float SMOOTH_TIME = 0.15f;

		private const float MAX_ROTATE_Z = 35f;

		private float _speed = 7.5f;

		private float _turnTime = 0.15f;

		private Vector3 _velocity;

		private Vector3 _rotateVelocity;

		private Transform _target;

		private bool _isDecreaseDistance;

		private float _lastDistance;

		private float _destroyTime;

		private const float VISION_DISTANCE = 5f;

		private Transform _referencePoint;

		private Vector3 _moveTarget;

		private float _repathRate = 0.05f;

		private float _nextRepath;

		private Transform _transform;

		private Transform _moveTargetObject;

		private int _pathLayerMask;

		public DuelEnemySlugStrategy(SlugController controller, int[] layers, Transform target)
			: base(controller, layers)
		{
			_target = target;
			if (controller is CrystalidSlugController)
			{
				_turnTime = 2f;
			}
			_velocity = Vector3.zero;
			_rotateVelocity = Vector3.zero;
			_isDecreaseDistance = true;
			_transform = Controller.Transform;
			_referencePoint = new GameObject("Reference Point").transform;
			_referencePoint.parent = _transform;
			_referencePoint.localPosition = Vector3.zero;
			_referencePoint.localEulerAngles = Vector3.zero;
			_moveTarget = _target.position;
			_nextRepath = 0.5f;
			_pathLayerMask = 1 << LayerMask.NameToLayer("TouchLayer");
			_pathLayerMask |= 1 << LayerMask.NameToLayer("UnitsConstraints");
			_pathLayerMask = ~_pathLayerMask;
			_moveTargetObject = new GameObject("MoveTarget").transform;
			SlugTurnTime component = controller.GetComponent<SlugTurnTime>();
			if (component != null)
			{
				_turnTime = component.TurnTime[controller.Model.Level];
			}
		}

		public override void OnUpdate()
		{
			UpdateDistance();
			AutomaticControlUpdate();
		}

		private void AutomaticControlUpdate()
		{
			_nextRepath -= Time.deltaTime;
			if (_nextRepath < 0f)
			{
				if (Vector3.SqrMagnitude(_target.position - _transform.position) > 75f)
				{
					PathFind();
				}
				else
				{
					_moveTarget = _target.position;
				}
				_nextRepath = _repathRate;
			}
			_moveTargetObject.position = _moveTarget;
			_referencePoint.LookAt(_moveTarget, Vector3.up);
			float y = _referencePoint.eulerAngles.y;
			float f = y - _transform.eulerAngles.y;
			Vector3 eulerAngles = _transform.eulerAngles;
			if (Time.timeScale != 0f)
			{
				eulerAngles.y = Mathf.SmoothDampAngle(eulerAngles.y, y, ref _rotateVelocity.y, _turnTime);
				if (Mathf.Abs(f) > 5f)
				{
					if (_referencePoint.localEulerAngles.y > 180f)
					{
						eulerAngles.z = Mathf.SmoothDampAngle(_transform.eulerAngles.z, 35f, ref _rotateVelocity.z, 0.15f);
					}
					else
					{
						eulerAngles.z = Mathf.SmoothDampAngle(_transform.eulerAngles.z, -35f, ref _rotateVelocity.z, 0.15f);
					}
				}
				else
				{
					eulerAngles.z = Mathf.SmoothDampAngle(_transform.eulerAngles.z, 0f, ref _rotateVelocity.z, 0.15f);
				}
			}
			_transform.eulerAngles = eulerAngles;
			float f2 = (float)Math.PI / 180f * eulerAngles.y;
			float z = _speed * Mathf.Cos(f2);
			float x = _speed * Mathf.Sin(f2);
			Vector3 vector = new Vector3(x, 0f, z);
			_transform.position = Vector3.SmoothDamp(_transform.position, _transform.position + vector, ref _velocity, 1f);
			if (Vector3.Magnitude(_target.transform.position - _transform.position) < 5f && _turnTime < 1f)
			{
				_turnTime += Time.deltaTime * 0.525f;
				if (_turnTime > 1f)
				{
					_turnTime = 1f;
				}
				_transform.position = Vector3.MoveTowards(_transform.position, _target.position + Vector3.up * 1.25f, Time.deltaTime * 5.5f);
			}
		}

		private void PathFind()
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			Vector3 forward = _transform.forward;
			Debug.DrawRay(_transform.position, _transform.forward * 5f, Color.yellow);
			RaycastHit hitInfo;
			if (Physics.Raycast(_transform.position, forward, out hitInfo, 5f, _pathLayerMask))
			{
				flag = true;
			}
			Vector3 vector = _transform.forward * 5f - _transform.right * 2f;
			vector.y = 0f;
			vector = vector.normalized;
			Debug.DrawRay(_transform.position, vector * 5f, Color.green);
			if (Physics.Raycast(_transform.position, vector, out hitInfo, 5f, _pathLayerMask))
			{
				flag2 = true;
			}
			Vector3 vector2 = _transform.forward * 5f + _transform.right * 2f;
			vector2.y = 0f;
			vector2 = vector2.normalized;
			Debug.DrawRay(_transform.position, vector2 * 5f, Color.yellow);
			if (Physics.Raycast(_transform.position, vector2, out hitInfo, 5f, _pathLayerMask))
			{
				flag3 = true;
			}
			Vector3 right = _transform.right;
			right.y = 0f;
			right = right.normalized;
			if (flag)
			{
				if (flag3)
				{
					_moveTarget = _transform.position - right;
				}
				else
				{
					_moveTarget = _transform.position + right;
				}
			}
			else if (flag3)
			{
				_moveTarget = _transform.position - right;
			}
			else if (flag2)
			{
				_moveTarget = _transform.position + right;
			}
			else
			{
				_moveTarget = _target.position;
			}
		}

		private void UpdateDistance()
		{
			if (_target == null)
			{
				return;
			}
			if (Vector3.Angle(MonoSingleton<DuelManager>.Instance.PlayerController.Animator.transform.forward, Controller.Transform.forward) < 40f)
			{
				Controller.SelfDestruction();
			}
			float num = Vector3.SqrMagnitude(Controller.Transform.position - _target.position);
			if (_lastDistance < num)
			{
				if (_isDecreaseDistance)
				{
					_destroyTime = 0.5f;
				}
				_isDecreaseDistance = false;
				_destroyTime -= Time.deltaTime;
				if (_destroyTime < 0f)
				{
					Controller.SelfDestruction();
				}
			}
			else
			{
				_isDecreaseDistance = true;
			}
			_lastDistance = num;
		}
	}
}

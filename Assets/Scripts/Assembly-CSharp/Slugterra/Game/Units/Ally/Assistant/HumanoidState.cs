using System;
using AppsMinistry.Core.State;
using UnityEngine;

namespace Slugterra.Game.Units.Ally.Assistant
{
	public class HumanoidState : MonoState
	{
		private float _prevDistance;

		private float _stopDistance;

		protected const float STOP_DISTANCE = 0.2f;

		private const float HOLD_TIME = 0.5f;

		private float _holdTime;

		private Transform _target;

		protected Vector3 _offset = Vector3.zero;

		protected HumanoidController Controller
		{
			get
			{
				return StateMachine.Controller as HumanoidController;
			}
		}

		protected void Setup(float speed, Transform target, float stopDistance = 0.2f, float turningSpeed = 5f, bool canMove = true)
		{
			_target = target;
			Controller.Model.CurrentSpeed = speed;
			Controller.PathFinder.speed = Controller.Model.CurrentSpeed;
			Controller.PathFinder.turningSpeed = turningSpeed;
			Controller.PathFinder.canMove = canMove;
			Controller.PathFinder.target.position = target.position + _offset;
			Controller.PathFinder.SearchPath();
			_prevDistance = Vector3.SqrMagnitude(target.position - Controller.Transform.position);
			_stopDistance = stopDistance;
			_holdTime = -1f;
		}

		protected void MoveAnimation()
		{
			float num = Controller.PathFinder.MoveVelocity.x;
			float num2 = Controller.PathFinder.MoveVelocity.z;
			if (Controller.RotateToTarget)
			{
				RotateToCommand component = GetComponent<RotateToCommand>();
				float f = component.Angle / 180f * (float)Math.PI;
				num = num * Mathf.Cos(f) - num2 * Mathf.Sin(f);
				num2 = num * Mathf.Sin(f) + num2 * Mathf.Cos(f);
			}
			else
			{
				Controller.Animator.transform.localEulerAngles = Vector3.zero;
			}
			Controller.Animator.SetFloat("VelocityX", num);
			Controller.Animator.SetFloat("VelocityY", num2);
		}

		protected void CheckDistance(bool checkStick = false, bool linked = false)
		{
			if (linked)
			{
				Controller.PathFinder.target.position = _target.position + _offset;
			}
			float num = Vector3.SqrMagnitude(Controller.PathFinder.target.position - Controller.Transform.position);
			if (num < _stopDistance * _stopDistance)
			{
				Finish();
			}
			if (!checkStick)
			{
				return;
			}
			if (Mathf.Abs(_prevDistance - num) < 0.001f)
			{
				if (_holdTime < 0f)
				{
					_holdTime = Time.time + 0.5f;
				}
				else if (_holdTime < Time.time)
				{
					Finish();
					return;
				}
			}
			else
			{
				_holdTime = -1f;
			}
			_prevDistance = num;
		}
	}
}

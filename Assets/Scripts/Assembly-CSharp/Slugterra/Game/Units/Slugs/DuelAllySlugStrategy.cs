using System;
using System.Collections;
using AppsMinistry.Core;
using AppsMinistry.Core.Input;
using Slugterra.App;
using Slugterra.Domain.App.Settings;
using Slugterra.Game.Duel;
using Slugterra.UI.Game;
using Slugterra.UI.Game.Camera;
using UnityEngine;

namespace Slugterra.Game.Units.Slugs
{
	public class DuelAllySlugStrategy : AllySlugStrategy
	{
		private enum InputEvent
		{
			None = 0,
			PressLeft = 1,
			PressRight = 2
		}

		private InputEvent currentEvent;

		private const float SMOOTH_TIME = 0.15f;

		private float _speed = 5f;

		private float _angle;

		private float _angleOffset;

		private Vector3 _angleVectorOffset;

		private Vector3 _velocity;

		private Vector3 _startPosition;

		private SlugSmoothFollow _slugSmoothFollow;

		private float _rotateZ;

		private float _rotateVelocityZ;

		private Transform _target;

		private bool _isDecreaseDistance;

		private float _lastDistance;

		private float _destroyTime;

		private float _joystickOffset;

		private Vector3 _currentAcceleration;

		private SlugControl _slugControl;

		private bool _isReleased;

		private Vector3 _rotateVelocity;

		private const float MAX_ROTATE_Z = 35f;

		private const float VISION_DISTANCE = 5f;

		private Transform _referencePoint;

		private Transform _scanDirection;

		private Vector3 _moveTarget;

		private float _repathRate = 0.05f;

		private float _nextRepath;

		private Transform _transform;

		private Transform _moveTargetObject;

		private InGameView _inGameView;

		private int _pathLayerMask;

		private float _turnTime = 0.3f;

		public DuelAllySlugStrategy(SlugController controller, int[] layers, Transform target)
			: base(controller, layers)
		{
			_slugControl = MonoSingleton<AppController>.Instance.Model.GameSetting.SlugControl;
			_target = target;
			_angle = 90f;
			_angleOffset = 1.2f;
			if (controller is CrystalidSlugController)
			{
				_angleOffset = 0.2f;
				_turnTime = 2f;
			}
			_angleVectorOffset = new Vector3(0f, _angleOffset, 0f);
			_velocity = Vector3.zero;
			_rotateZ = 35f;
			_joystickOffset = 0f;
			_currentAcceleration = Vector3.zero;
			currentEvent = InputEvent.None;
			_isDecreaseDistance = true;
			_slugSmoothFollow = MonoSingleton<DuelManager>.Instance.SlugSmoothFollow;
			_transform = Controller.Transform;
			_referencePoint = new GameObject("Reference Point").transform;
			_referencePoint.parent = _transform;
			_referencePoint.localPosition = Vector3.zero;
			_referencePoint.localEulerAngles = Vector3.zero;
			_scanDirection = new GameObject("Scan Direction").transform;
			_scanDirection.parent = _transform;
			_scanDirection.localPosition = Vector3.zero;
			_scanDirection.localEulerAngles = Vector3.zero;
			_moveTarget = _target.position;
			_nextRepath = 0.5f;
			_moveTargetObject = new GameObject("MoveTarget").transform;
			DonDestroyMonoSingleton<InputManager>.Instance.OnTouchStart += OnTouchStart;
			DonDestroyMonoSingleton<InputManager>.Instance.OnTouch += OnTouch;
			DonDestroyMonoSingleton<InputManager>.Instance.OnTouchEnd += OnTouchEnd;
			DonDestroyMonoSingleton<InputManager>.Instance.OnAccelMoveRight += OnAccelMoveRight;
			DonDestroyMonoSingleton<InputManager>.Instance.OnAccelMoveLeft += OnAccelMoveLeft;
			DonDestroyMonoSingleton<InputManager>.Instance.OnAccelerometerX += OnAccelerometerX;
			DonDestroyMonoSingleton<InputManager>.Instance.OnButtonDownEvent += OnButtonDownEvent;
			DonDestroyMonoSingleton<InputManager>.Instance.OnButtonPressEvent += OnButtonPressEvent;
			DonDestroyMonoSingleton<InputManager>.Instance.OnAcceleration += OnAcceleration;
			DonDestroyMonoSingleton<InputManager>.Instance.OnJoysticOnMove_movement += OnMove;
			DonDestroyMonoSingleton<InputManager>.Instance.OnJoysticOnMoveEnd_movement += OnMoveEnd;
			DonDestroyMonoSingleton<InputManager>.Instance.OnNextTarget += OnReleaseSlug;
			_inGameView = UnityEngine.Object.FindObjectOfType<InGameView>();
			if (_inGameView != null)
			{
				_inGameView.OnReleaseEvent += OnReleaseSlug;
			}
			_pathLayerMask = 1 << LayerMask.NameToLayer("TouchLayer");
			_pathLayerMask |= 1 << LayerMask.NameToLayer("UnitsConstraints");
			_pathLayerMask = ~_pathLayerMask;
			if (Controller.IsGoingToAttackOwner())
			{
				Controller.StartCoroutine(OwnerAttack(3f));
			}
			SlugTurnTime component = controller.GetComponent<SlugTurnTime>();
			if (component != null)
			{
				_turnTime = component.TurnTime[controller.Model.Level];
			}
		}

		public override void OnReleaseResource()
		{
			if (!_isReleased)
			{
				OnReleaseSlug();
			}
			GameController gameController = UnityEngine.Object.FindObjectOfType<GameController>();
			if (gameController != null && MonoSingleton<DuelManager>.Instance != null && MonoSingleton<DuelManager>.Instance.StateMachine.CurrentState is SlugFlyState && !_isReleased)
			{
				MonoSingleton<DuelManager>.Instance.StateMachine.ChangeState<FightState>(new object[0]);
			}
			MonoSingleton<DuelManager>.Instance.RemoveAllySlug(Controller);
			base.OnReleaseResource();
		}

		private void OnReleaseSlug()
		{
			if (DonDestroyMonoSingleton<InputManager>.Instance != null)
			{
				DonDestroyMonoSingleton<InputManager>.Instance.OnTouchStart -= OnTouchStart;
				DonDestroyMonoSingleton<InputManager>.Instance.OnTouch -= OnTouch;
				DonDestroyMonoSingleton<InputManager>.Instance.OnTouchEnd -= OnTouchEnd;
				DonDestroyMonoSingleton<InputManager>.Instance.OnAccelMoveRight -= OnAccelMoveRight;
				DonDestroyMonoSingleton<InputManager>.Instance.OnAccelMoveLeft -= OnAccelMoveLeft;
				DonDestroyMonoSingleton<InputManager>.Instance.OnAccelerometerX -= OnAccelerometerX;
				DonDestroyMonoSingleton<InputManager>.Instance.OnAcceleration -= OnAcceleration;
				DonDestroyMonoSingleton<InputManager>.Instance.OnJoysticOnMove_movement -= OnMove;
				DonDestroyMonoSingleton<InputManager>.Instance.OnJoysticOnMoveEnd_movement -= OnMoveEnd;
				DonDestroyMonoSingleton<InputManager>.Instance.OnNextTarget -= OnReleaseSlug;
				DonDestroyMonoSingleton<InputManager>.Instance.OnButtonDownEvent -= OnButtonDownEvent;
				DonDestroyMonoSingleton<InputManager>.Instance.OnButtonPressEvent -= OnButtonPressEvent;
				_isReleased = true;
			}
			if (_inGameView != null)
			{
				_inGameView.OnReleaseEvent -= OnReleaseSlug;
			}
			if (MonoSingleton<DuelManager>.Instance != null && MonoSingleton<DuelManager>.Instance.StateMachine.CurrentState is SlugFlyState)
			{
				MonoSingleton<DuelManager>.Instance.StateMachine.ChangeState<FightState>(new object[0]);
			}
		}

		private void OnTouchStart(Vector3 position)
		{
			if (position.x > (float)(Screen.width / 2))
			{
				currentEvent = InputEvent.PressRight;
				_slugSmoothFollow.SetRotateZ(1f);
			}
			else
			{
				currentEvent = InputEvent.PressLeft;
				_slugSmoothFollow.SetRotateZ(-1f);
			}
			_startPosition = position;
			_joystickOffset = (position - _startPosition).x;
		}

		private void OnTouch(Vector3 position, float actionTime)
		{
			if (position.x > (float)(Screen.width / 2))
			{
				currentEvent = InputEvent.PressRight;
				_slugSmoothFollow.SetRotateZ(1f);
			}
			else
			{
				currentEvent = InputEvent.PressLeft;
				_slugSmoothFollow.SetRotateZ(-1f);
			}
			_startPosition = position;
			_joystickOffset = (position - _startPosition).x;
		}

		public virtual void OnAccelMoveRight()
		{
		}

		public virtual void OnAccelMoveLeft()
		{
		}

		public virtual void OnAccelerometerX(float x)
		{
			if (x > 0.1f)
			{
				currentEvent = InputEvent.PressRight;
				_slugSmoothFollow.SetRotateZ(x * 2f);
			}
			else if (x < -0.1f)
			{
				currentEvent = InputEvent.PressLeft;
				_slugSmoothFollow.SetRotateZ(x * 2f);
			}
			else
			{
				currentEvent = InputEvent.None;
				_slugSmoothFollow.SetRotateZ(0f);
			}
		}

		private void OnTouchEnd(Vector3 position, float actionTime)
		{
			currentEvent = InputEvent.None;
			_slugSmoothFollow.SetRotateZ(0f);
		}

		private void OnAcceleration(Vector3 acceleration)
		{
			_currentAcceleration = acceleration;
		}

		public virtual void OnMove(Vector2 axies)
		{
			if (axies.x > 0f)
			{
				currentEvent = InputEvent.PressRight;
				_slugSmoothFollow.SetRotateZ(1f);
			}
			else
			{
				currentEvent = InputEvent.PressLeft;
				_slugSmoothFollow.SetRotateZ(-1f);
			}
		}

		private void OnButtonDownEvent(AppsMinistry.Core.Input.InputEvent e)
		{
			if (e == AppsMinistry.Core.Input.InputEvent.Enter)
			{
				OnReleaseSlug();
			}
		}

		private void OnButtonPressEvent(AppsMinistry.Core.Input.InputEvent e)
		{
			if (e == AppsMinistry.Core.Input.InputEvent.Enter && MonoSingleton<DuelManager>.Instance != null && MonoSingleton<DuelManager>.Instance.StateMachine.CurrentState is SlugFlyState)
			{
				OnReleaseSlug();
			}
		}

		private void OnMoveEnd()
		{
			currentEvent = InputEvent.None;
			_slugSmoothFollow.SetRotateZ(0f);
		}

		private void ManualControlUpdate()
		{
			float z = 0f;
			if (_slugControl == SlugControl.Tap)
			{
				if (currentEvent == InputEvent.PressLeft)
				{
					Controller.Transform.eulerAngles -= _angleVectorOffset;
					z = Mathf.SmoothDampAngle(Controller.Transform.eulerAngles.z, _rotateZ, ref _rotateVelocityZ, 0.15f);
				}
				else if (currentEvent == InputEvent.PressRight)
				{
					Controller.Transform.eulerAngles += _angleVectorOffset;
					z = Mathf.SmoothDampAngle(Controller.Transform.eulerAngles.z, 0f - _rotateZ, ref _rotateVelocityZ, 0.15f);
				}
				else
				{
					z = Mathf.SmoothDampAngle(Controller.Transform.eulerAngles.z, 0f, ref _rotateVelocityZ, 0.15f);
				}
			}
			else if (_slugControl == SlugControl.Joystick)
			{
				float num = (float)Screen.width * 0.15f;
				float num2 = Mathf.Max(Mathf.Min(_joystickOffset, num), 0f - num);
				Controller.Transform.eulerAngles += _angleVectorOffset * (num2 * 2f / num);
				_slugSmoothFollow.SetRotateZ(num2 / num);
				z = Mathf.SmoothDampAngle(Controller.Transform.eulerAngles.z, (0f - _rotateZ) * num2 / num, ref _rotateVelocityZ, 0.15f);
			}
			else if (_slugControl == SlugControl.Accelerometer)
			{
				z = Mathf.SmoothDampAngle(Controller.Transform.eulerAngles.z, _rotateZ * _currentAcceleration.x * -2f, ref _rotateVelocityZ, 0.15f);
				Controller.Transform.eulerAngles += Vector3.up * _currentAcceleration.x * 2f;
				_slugSmoothFollow.SetRotateZ(_currentAcceleration.x);
			}
			Controller.Transform.eulerAngles = new Vector3(Controller.Transform.eulerAngles.x, Controller.Transform.eulerAngles.y, z);
			_angle = Controller.Transform.eulerAngles.y;
			float f = _angle / 180f * (float)Math.PI;
			float z2 = _speed / MonoSingleton<AppController>.Instance.TimeScale * Mathf.Cos(f);
			float x = _speed / MonoSingleton<AppController>.Instance.TimeScale * Mathf.Sin(f);
			Vector3 vector = new Vector3(x, 0f, z2);
			Controller.Transform.position = Vector3.SmoothDamp(Controller.Transform.position, Controller.Transform.position + vector, ref _velocity, 1f);
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
				_transform.position = Vector3.MoveTowards(_transform.position, _target.position + Vector3.up * 1.25f, Time.deltaTime * 3f);
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
			_scanDirection.LookAt(_target);
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

		protected virtual void AutoFigting(Transform enemy)
		{
			if (enemy != null)
			{
				SlugController component = enemy.GetComponent<SlugController>();
				int level = Controller.Model.Level;
				int level2 = component.Model.Level;
				if (level < level2)
				{
					Controller.OnExplosion();
					Controller.SelfDestruction();
				}
				else if (level == level2)
				{
					Controller.OnExplosion();
					Controller.SelfDestruction();
					component.OnExplosion();
					component.SelfDestruction();
				}
				else
				{
					component.OnExplosion();
					component.SelfDestruction();
				}
			}
		}

		protected virtual void ManualFigting(Transform enemy)
		{
			MonoSingleton<DuelManager>.Instance.StateMachine.ChangeState<SlugFightState>(new object[2] { Controller.Transform, enemy });
			MonoSingleton<DuelManager>.Instance.CreateSlugCamera(Controller, enemy.GetComponent<SlugController>());
		}

		protected virtual Transform DetectCollision()
		{
			return MonoSingleton<DuelManager>.Instance.IsDetectCollision(Controller.Transform);
		}

		public override void OnUpdate()
		{
			UpdateDistance();
			Transform transform = DetectCollision();
			if (_isReleased)
			{
				AutomaticControlUpdate();
				if (transform != null)
				{
					AutoFigting(transform);
				}
			}
			else
			{
				ManualControlUpdate();
				if (transform != null)
				{
					ManualFigting(transform);
				}
			}
		}

		private void UpdateDistance()
		{
			if (_target == null || Vector3.Angle(MonoSingleton<DuelManager>.Instance.BossTransform.position - Controller.Transform.position, Controller.Transform.forward) < 90f)
			{
				return;
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

		public override bool OnTriggerEnter(Transform transform)
		{
			return base.OnTriggerEnter(transform);
		}

		private IEnumerator OwnerAttack(float time)
		{
			yield return new WaitForSeconds(time);
			OnReleaseSlug();
			_target = Controller.Parent.Transform;
			_layers[0] = LayerMask.NameToLayer("AllyLayer");
			Controller.gameObject.layer = LayerMask.NameToLayer("EnemySlugLayer");
		}
	}
}

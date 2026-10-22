using System;
using System.Collections;
using AppsMinistry.Core;
using AppsMinistry.Core.Sound;
using AppsMinistry.Utils;
using Slugterra.Configuration;
using Slugterra.Domain.Game.Units;
using Slugterra.Domain.Game.Units.Slugs;
using Slugterra.Domain.UI.Game;
using Slugterra.Game.Duel;
using Slugterra.Game.Input;
using Slugterra.UI.Game;
using UnityEngine;

namespace Slugterra.Game.Units.Player.Strategy
{
	public class DuelJoystickInputStrategy : BaseSplineInputStrategy
	{
		private enum InputEvent
		{
			None = 0,
			PressLeft = 1,
			PressRight = 2,
			SwipeLeft = 3,
			SwipeRight = 4
		}

		private float _speed;

		private Vector3 _prevPosition;

		private float _rollSpeed = 12f;

		private bool _isRolling;

		private float _lastPlaySound;

		private InputEvent _currentEvent;

		private Transform _enemy;

		private float currentY;

		private float speed = 0.01f;

		private InGameModel _inGameModel;

		private InGameModel InGameModel
		{
			get
			{
				if (_inGameModel == null)
				{
					InGameController inGameController = UnityEngine.Object.FindObjectOfType<InGameController>();
					if (inGameController != null)
					{
						_inGameModel = inGameController.Model;
					}
				}
				return _inGameModel;
			}
		}

		public DuelJoystickInputStrategy(PlayerController controller)
			: base(controller)
		{
			_currentEvent = InputEvent.None;
		}

		protected override void OnInitialize()
		{
			_prevPosition = Controller.Transform.position;
			if (CurrentSpline != null)
			{
				_speed = 1f / CurrentSpline.Length;
				_rollSpeed = 1f / CurrentSpline.Length * _rollSpeed;
				Controller.Model.CurrentSpeed = Controller.Model.WalkSpeed;
			}
		}

		public override void OnMove(Vector2 axises)
		{
			_currentEvent = InputEvent.PressLeft;
			CurrentParam -= Time.deltaTime * _speed * Controller.Model.CurrentSpeed * axises.x;
			Vector3 direction = (Controller.Transform.position - _prevPosition) / Time.deltaTime;
			direction = Controller.Transform.InverseTransformDirection(direction);
			Vector2 vector = new Vector2(direction.x, direction.z);
			if (Controller.RotateToTarget)
			{
				vector = MathUtil.Rotate(vector, Controller.RotateToCommand.Angle);
			}
			else
			{
				Controller.Animator.transform.localEulerAngles = Vector3.zero;
			}
			Controller.Animator.SetFloat("VelocityX", vector.x);
			if (-0.2f < vector.y && vector.y < 0.2f)
			{
				currentY = Mathf.Clamp(currentY, -0.2f, 0.2f);
				if (vector.y < -0.066f)
				{
					currentY = Mathf.SmoothStep(currentY, -0.2f, 0.2f);
				}
				else if (-0.066 < (double)vector.y && vector.y < 0.066f)
				{
					currentY = Mathf.SmoothStep(currentY, 0f, 0.2f);
				}
				else
				{
					currentY = Mathf.SmoothStep(currentY, 0.2f, 0.2f);
				}
				Controller.Animator.SetFloat("VelocityY", currentY);
			}
			else
			{
				currentY = vector.y;
				Controller.Animator.SetFloat("VelocityY", vector.y);
			}
			_prevPosition = Controller.Transform.position;
			if (0f < CurrentParam && CurrentParam < 1f)
			{
				PlayWalkSound();
			}
		}

		public override void OnMoveEnd()
		{
			_currentEvent = InputEvent.None;
			Controller.Animator.SetFloat("VelocityX", 0f);
			Controller.Animator.SetFloat("VelocityY", 0f);
		}

		public override void OnPress(Vector3 position)
		{
			if (_currentEvent == InputEvent.SwipeLeft || _currentEvent == InputEvent.SwipeRight)
			{
				return;
			}
			if (MonoSingleton<GameCollisionMap>.Instance.IsTapOnEnemy(position, out _enemy))
			{
				if (Controller.AttackTarget != _enemy)
				{
					Controller.AttackTarget = _enemy;
				}
			}
			else if (position.x > (float)(Screen.width / 2))
			{
				_currentEvent = InputEvent.PressRight;
			}
			else
			{
				_currentEvent = InputEvent.PressLeft;
			}
		}

		public override void OnRelease(Vector3 direction, float actionTime)
		{
			base.OnRelease(direction, actionTime);
			if (_currentEvent != InputEvent.SwipeLeft && _currentEvent != InputEvent.SwipeRight)
			{
				_currentEvent = InputEvent.None;
			}
		}

		public override void OnDoubleTap(Vector3 position)
		{
			if (MonoSingleton<GameCollisionMap>.Instance.IsTapOnEnemy(position, out _enemy) && Controller.AttackTarget == _enemy)
			{
				Controller.AttackTarget = MonoSingleton<DuelManager>.Instance.BossTransform;
			}
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			if (_currentEvent != InputEvent.PressLeft && _currentEvent != InputEvent.PressRight)
			{
				Controller.Animator.SetFloat("VelocityX", 0f);
				Controller.Animator.SetFloat("VelocityY", 0f);
			}
			if (_currentEvent == InputEvent.SwipeLeft || _currentEvent == InputEvent.SwipeRight)
			{
				float num = Controller.Animator.GetFloat("RollMove");
				if (_currentEvent == InputEvent.SwipeLeft)
				{
					CurrentParam += Time.deltaTime * _rollSpeed * num;
				}
				else
				{
					CurrentParam -= Time.deltaTime * _rollSpeed * num;
				}
				if (Controller.Animator.GetCurrentAnimatorStateInfo(0).IsName("Roll_Pistol") && _isRolling)
				{
					_isRolling = false;
					Controller.Animator.SetBool("RollTrigger", false);
				}
				else if (!_isRolling && !Controller.Animator.GetCurrentAnimatorStateInfo(0).IsName("Roll_Pistol") && !Controller.Animator.IsInTransition(0))
				{
					_currentEvent = InputEvent.None;
					Controller.Animator.SetFloat("RollDirectionX", 0f);
					Controller.Animator.SetFloat("RollDirectionY", 0f);
				}
			}
		}

		public override void OnSwipe(Vector2 direction)
		{
			if (Controller.Model.CurrentSpeed != 0f)
			{
				bool flag = direction.x < 0f;
				float y = Camera.main.transform.eulerAngles.y;
				y = y / 180f * (float)Math.PI;
				Controller.Animator.SetBool("RollTrigger", true);
				_isRolling = true;
				Controller.StartCoroutine(UnsetRolling(0.3f));
				if (flag)
				{
					direction.y = Mathf.Cos(y - (float)Math.PI / 2f);
					direction.x = Mathf.Sin(y - (float)Math.PI / 2f);
					_currentEvent = InputEvent.SwipeLeft;
				}
				else
				{
					direction.y = Mathf.Cos(y + (float)Math.PI / 2f);
					direction.x = Mathf.Sin(y + (float)Math.PI / 2f);
					_currentEvent = InputEvent.SwipeRight;
				}
				direction.Normalize();
				y = Controller.Animator.transform.localEulerAngles.y + Controller.Transform.localEulerAngles.y;
				direction = MathUtil.Rotate(direction, y);
				Controller.Animator.SetFloat("RollDirectionX", direction.x);
				Controller.Animator.SetFloat("RollDirectionY", direction.y);
				OnRoll(direction);
			}
		}

		private IEnumerator UnsetRolling(float time)
		{
			yield return new WaitForSeconds(time);
			if (_isRolling)
			{
				_isRolling = false;
			}
		}

		public override void OnUseSlug(SlugType slugType)
		{
			if (MonoSingleton<DuelManager>.Instance.StateMachine.CurrentState is FightState)
			{
				Slug slug = (Controller.Model as Humanoid).GetSlug(slugType);
				if (Controller.IsCanUseSlug(slug) && slug.Available)
				{
					Controller.SetNextSlug(slugType);
				}
			}
		}

		private void PlayWalkSound()
		{
			if (!((double)(Time.time - _lastPlaySound) < (double)(1f / Controller.Model.CurrentSpeed) * 1.5))
			{
				SoundItem soundItem = Controller.PlayWalkSound();
				if (soundItem != null)
				{
					soundItem.transform.parent = Controller.Transform;
					soundItem.transform.localPosition = Vector3.zero;
					_lastPlaySound = Time.time;
				}
			}
		}
	}
}

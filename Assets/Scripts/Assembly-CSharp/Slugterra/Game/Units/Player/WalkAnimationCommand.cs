using AppsMinistry.Core.Command;
using AppsMinistry.Core.Sound;
using AppsMinistry.Utils;
using UnityEngine;

namespace Slugterra.Game.Units.Player
{
	[SingletonCommand]
	public class WalkAnimationCommand : MonoCommand<WalkAnimationCommand>
	{
		private PlayerController _controller;

		private Animator _animator;

		private Transform _animatorTransform;

		private UnitPathFinder _pathFinder;

		private SoundItem _walkSound;

		private float _lastPlaySound;

		private float _prevDistance;

		protected override void OnStart(object[] args)
		{
			base.OnStart(args);
			_controller = (PlayerController)args[0];
			if (_controller == null)
			{
				Finish();
				return;
			}
			_animator = _controller.Animator;
			if (_animator == null)
			{
				Finish();
				return;
			}
			_animatorTransform = _animator.transform;
			_pathFinder = _controller.PathFinder;
			if (_pathFinder == null)
			{
				Finish();
			}
			else
			{
				_prevDistance = 0f;
			}
		}

		private void Update()
		{
			Vector2 vector = new Vector2(_pathFinder.MoveVelocity.x, _pathFinder.MoveVelocity.z);
			if (_controller.RotateToTarget)
			{
				RotateToCommand component = GetComponent<RotateToCommand>();
				vector = MathUtil.Rotate(vector, component.Angle);
			}
			else
			{
				_animatorTransform.localEulerAngles = Vector3.zero;
			}
			_animator.SetFloat("VelocityX", vector.x);
			_animator.SetFloat("VelocityY", vector.y);
			float num = Vector3.SqrMagnitude(_pathFinder.target.position - _controller.Transform.position);
			if (!(Mathf.Abs(_prevDistance - num) < 0.001f))
			{
				PlayWalkSound();
			}
			_prevDistance = num;
		}

		private void PlayWalkSound()
		{
			if (!(Time.time - _lastPlaySound < 1f / _controller.Model.CurrentSpeed * 2f))
			{
				_walkSound = _controller.PlayWalkSound();
				if (_walkSound != null)
				{
					_walkSound.transform.parent = transform;
					_walkSound.transform.localPosition = Vector3.zero;
					_lastPlaySound = Time.time;
				}
			}
		}

		private void OnDisable()
		{
			if (_animator != null)
			{
				_animator.SetFloat("VelocityX", 0f);
				_animator.SetFloat("VelocityY", 0f);
			}
		}

		protected override void OnFinish()
		{
			if (_animator != null)
			{
				_animator.SetFloat("VelocityX", 0f);
				_animator.SetFloat("VelocityY", 0f);
			}
		}
	}
}

using AppsMinistry.Utils;
using UnityEngine;

namespace Slugterra.Game.Units.Player
{
	public class RollState : PlayerState
	{
		private float _rollSpeed = 8.5f;

		private Vector3 _rollOffset;

		private CharacterController _characterController;

		private bool _isRolling;

		private bool _isTap;

		private float _distance;

		private bool _isSwipe;

		private Vector2 _direction;

		protected override void OnStart(object[] args)
		{
			Debug.Log("RollState::OnStart");
			base.OnStart(args);
			_isTap = false;
			_characterController = Controller.GetComponent<CharacterController>();
			Vector2 vector = (Vector2)args[0];
			if (args.Length > 1)
			{
				Vector2 vector2 = (Vector2)args[1];
				_rollOffset = new Vector3(vector2.x, 0f, vector2.y).normalized * _rollSpeed;
			}
			else
			{
				_rollOffset = new Vector3(vector.x, 0f, vector.y).normalized * _rollSpeed;
			}
			if (Controller.PathFinder != null)
			{
				Controller.PathFinder.enabled = false;
			}
			Controller.RotateToTarget = false;
			vector.Normalize();
			float angle = Controller.Animator.transform.localEulerAngles.y + Controller.Transform.localEulerAngles.y;
			vector = MathUtil.Rotate(vector, angle);
			_isRolling = true;
			Controller.Animator.SetFloat("RollDirectionX", vector.x);
			Controller.Animator.SetFloat("RollDirectionY", vector.y);
			Controller.Animator.SetBool("RollTrigger", _isRolling);
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();
			float num = Controller.Animator.GetFloat("RollMove");
			_characterController.SimpleMove(_rollOffset * num);
			if (Controller.Animator.GetCurrentAnimatorStateInfo(0).IsName("Roll_Pistol") && _isRolling)
			{
				_isRolling = false;
				Controller.Animator.SetBool("RollTrigger", _isRolling);
			}
			else if (!_isRolling && !Controller.Animator.GetCurrentAnimatorStateInfo(0).IsName("Roll_Pistol") && !Controller.Animator.IsInTransition(0))
			{
				if (_isTap)
				{
					StateMachine.NextState<MoveState>(new object[1] { _distance });
				}
				else if (_isSwipe)
				{
					StateMachine.NextState<RollState>(new object[1] { _direction });
				}
				else
				{
					StateMachine.NextState<IdleState>(new object[0]);
				}
				Finish();
			}
		}

		public void OnTap()
		{
			_isTap = true;
			_isSwipe = false;
			Controller.Model.Walk();
			_distance = 0.2f;
		}

		public void OnDoubleTap(float distance = 0.2f)
		{
			_isTap = true;
			_isSwipe = false;
			Controller.Model.Run();
			_distance = distance;
		}

		public void OnSwipe(Vector2 direction)
		{
			Debug.Log("RollState::OnSwipe");
			_isSwipe = true;
			_isTap = false;
			_direction = direction;
		}

		protected override void OnFinish()
		{
			Controller.Animator.SetFloat("RollDirectionX", 0f);
			Controller.Animator.SetFloat("RollDirectionY", 0f);
			if (Controller.AttackTarget != null)
			{
				Controller.RotateToTarget = true;
			}
			if (Controller.PathFinder != null)
			{
				Controller.PathFinder.enabled = true;
			}
		}
	}
}

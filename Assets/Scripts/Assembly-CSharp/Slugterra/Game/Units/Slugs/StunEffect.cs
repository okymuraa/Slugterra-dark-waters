using Slugterra.Game.Units.Player;
using UnityEngine;

namespace Slugterra.Game.Units.Slugs
{
	public class StunEffect : UnitEffect
	{
		private float _finishTime;

		private UnitController _controller;

		private GameObject _effect;

		protected override void OnStart(object[] args)
		{
			base.OnStart(args);
			_controller = (UnitController)args[0];
			if (_controller == null)
			{
				Finish();
				return;
			}
			if (_controller.IsFallDown)
			{
				Finish();
				return;
			}
			_finishTime = (float)args[1] + Time.time;
			if (_effect == null)
			{
				GameObject gameObject = (GameObject)args[2];
				if (gameObject != null)
				{
					_effect = Object.Instantiate(gameObject, transform.position, Quaternion.identity);
					_effect.transform.parent = transform;
				}
			}
			if (_controller.NewVersionAI)
			{
				_controller.MyBehavior.Stun();
			}
			if (_controller.PathFinder != null)
			{
				_controller.PathFinder.enabled = false;
				WalkAnimationCommand component = _controller.GetComponent<WalkAnimationCommand>();
				if (component != null)
				{
					component.Finish();
				}
				if (_controller.StateMachine.CurrentState != null)
				{
					_controller.StateMachine.CurrentState.enabled = false;
				}
				_controller.StateMachine.enabled = false;
				_controller.Animator.SetFloat("VelocityX", 0f);
				_controller.Animator.SetFloat("VelocityY", 0f);
			}
			_controller.Model.AddSpeedCoeficient(GetType(), 0f);
		}

		private void Update()
		{
			if (_controller == null)
			{
				Finish();
			}
			else if (_controller.Model.IsDead())
			{
				Finish();
			}
			else if (_finishTime <= Time.time)
			{
				Finish();
			}
		}

		protected override void OnFinish()
		{
			base.OnFinish();
			if (_effect != null)
			{
				Object.Destroy(_effect);
			}
			if (!(_controller != null))
			{
				return;
			}
			if (_controller.NewVersionAI)
			{
				_controller.MyBehavior.UnStun();
			}
			if (_controller.PathFinder != null && !_controller.IsFallDown)
			{
				_controller.PathFinder.enabled = true;
				_controller.StateMachine.enabled = true;
				if (_controller.StateMachine.CurrentState != null)
				{
					_controller.StateMachine.CurrentState.enabled = true;
					_controller.StateMachine.CurrentState.Finish();
				}
			}
			_controller.Model.RemoveSpeedCoeficient(GetType());
			_controller = null;
		}
	}
}

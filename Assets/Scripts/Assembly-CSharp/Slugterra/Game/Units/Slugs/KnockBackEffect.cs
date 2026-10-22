using UnityEngine;

namespace Slugterra.Game.Units.Slugs
{
	public class KnockBackEffect : UnitEffect
	{
		private float _finishTime;

		private UnitController _controller;

		private GameObject _effect;

		protected override void OnStart(object[] args)
		{
			base.OnStart(args);
			_controller = (UnitController)args[0];
			if (_controller == null || _controller.IsFallDown || _controller.GetComponent<ArmorBoost>() != null)
			{
				return;
			}
			AnimatorStateInfo currentAnimatorStateInfo = _controller.Animator.GetCurrentAnimatorStateInfo(0);
			AnimatorStateInfo nextAnimatorStateInfo = _controller.Animator.GetNextAnimatorStateInfo(0);
			if (currentAnimatorStateInfo.IsName("FallDown") || nextAnimatorStateInfo.IsName("FallDown") || _controller.Model.IsDead())
			{
				return;
			}
			_finishTime = (float)args[1] + Time.time;
			float num = (float)args[2];
			float num2 = Random.Range(0f, 1f);
			if (num2 >= num)
			{
				Finish();
				return;
			}
			_controller.OnFallDown();
			if (_effect == null)
			{
				GameObject gameObject = (GameObject)args[3];
				if (gameObject != null)
				{
					_effect = Object.Instantiate(gameObject, transform.position, Quaternion.identity);
					_effect.transform.parent = transform;
				}
			}
			if (_controller.PathFinder != null)
			{
				_controller.PathFinder.enabled = false;
			}
			if (_controller.StateMachine.CurrentState != null)
			{
				_controller.StateMachine.CurrentState.enabled = false;
			}
			_controller.Animator.SetFloat("VelocityX", 0f);
			_controller.Animator.SetFloat("VelocityY", 0f);
			_controller.enabled = false;
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
			if (_controller != null && !_controller.Model.IsDead())
			{
				_controller.OnStandUp();
				if (_controller.StateMachine.CurrentState != null)
				{
					_controller.StateMachine.CurrentState.Finish();
				}
				_controller.enabled = true;
			}
			if (_effect != null)
			{
				Object.Destroy(_effect);
			}
			base.OnFinish();
		}
	}
}

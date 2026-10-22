using AppsMinistry.Utils;
using UnityEngine;

namespace Slugterra.Game.Units.Animals.SlugHound
{
	[MecanimState(new string[] { "EvasionLeft", "EvasionRight" })]
	public class EvasionState : SlugHoundState
	{
		private enum Jump
		{
			Left = 0,
			Right = 1
		}

		private Jump _jump;

		private Animator _animator;

		private Vector3 _jumpOffset;

		protected override void OnStart(object[] args)
		{
			base.OnStart(args);
			_animator = Controller.Animator;
			_jump = (Jump)(Random.Range(0, 10) % 2);
			Controller.Animator.SetTrigger((_jump != Jump.Left) ? "EvasionRight" : "EvasionLeft");
			Controller.PathFinder.enabled = false;
			Vector2 vector = Vector2.up * 0.07f;
			float angle = Controller.Transform.eulerAngles.y - 90f + (float)((_jump != Jump.Left) ? (-90) : 90);
			vector = MathUtil.Rotate(vector, angle);
			_jumpOffset = new Vector3(vector.x, 0f, vector.y);
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();
			AnimatorStateInfo currentAnimatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
			float num = Controller.Animator.GetFloat("Jump");
			Controller.Transform.position += _jumpOffset * num;
			if (IsPlay && !currentAnimatorStateInfo.IsName(MecanimState.Name))
			{
				Finish();
			}
		}
	}
}

using UnityEngine;

namespace Slugterra.Game.Units.Mechanisms.Spline
{
	public class IdleState : TeamState
	{
		protected override void OnStart(object[] args)
		{
			foreach (AnimationState item in Controller.Animation)
			{
				item.speed = 0f;
			}
			Controller.Animator.SetFloat("Speed", 0f);
			if (Controller.FernandoAnimator != null)
			{
				Controller.FernandoAnimator.SetFloat("Speed", 0f);
			}
		}
	}
}

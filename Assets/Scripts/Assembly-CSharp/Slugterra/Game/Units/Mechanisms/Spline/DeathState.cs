using UnityEngine;

namespace Slugterra.Game.Units.Mechanisms.Spline
{
	public class DeathState : TeamState
	{
		protected override void OnStart(object[] args)
		{
			foreach (AnimationState item in Controller.Animation)
			{
				item.speed = 0f;
			}
			Controller.Animator.SetTrigger("Death");
			if (Controller.FernandoAnimator != null)
			{
				Controller.FernandoAnimator.SetTrigger("Death");
			}
		}
	}
}

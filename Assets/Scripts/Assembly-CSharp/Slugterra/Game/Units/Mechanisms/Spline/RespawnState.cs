using UnityEngine;

namespace Slugterra.Game.Units.Mechanisms.Spline
{
	internal class RespawnState : TeamState
	{
		protected override void OnStart(object[] args)
		{
			Collider[] componentsInChildren = Controller.GetComponentsInChildren<Collider>();
			Collider[] array = componentsInChildren;
			foreach (Collider collider in array)
			{
				collider.enabled = true;
			}
			Controller.Animator.SetTrigger("Respawn");
			if (Controller.FernandoAnimator != null)
			{
				Controller.FernandoAnimator.SetTrigger("Respawn");
			}
		}
	}
}

using UnityEngine;

namespace Slugterra.Game.Units.Mechanisms
{
	internal class RespawnState : WagonState
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
			Controller.PathFinder.enabled = true;
			Controller.enabled = true;
		}
	}
}

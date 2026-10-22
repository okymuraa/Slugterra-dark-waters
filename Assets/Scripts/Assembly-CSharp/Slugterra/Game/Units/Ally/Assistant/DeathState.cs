using UnityEngine;

namespace Slugterra.Game.Units.Ally.Assistant
{
	public class DeathState : HumanoidState
	{
		protected override void OnStart(object[] args)
		{
			base.OnStart(args);
			Collider[] componentsInChildren = Controller.GetComponentsInChildren<Collider>();
			Collider[] array = componentsInChildren;
			foreach (Collider collider in array)
			{
				collider.enabled = false;
			}
			Controller.Animator.SetLayerWeight(1, 0f);
			Controller.Animator.SetBool("EnemyInSight", false);
			Controller.Animator.SetTrigger("Death");
			Controller.RotateToTarget = false;
			Controller.AttackTarget = null;
			CharacterController component = Controller.GetComponent<CharacterController>();
			if (component != null)
			{
				Object.Destroy(component);
			}
			Controller.PathFinder.enabled = false;
			Controller.enabled = false;
		}
	}
}

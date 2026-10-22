using UnityEngine;

namespace Slugterra.Game.Units.Enemy
{
	public class DeathState : EnemyState
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
			Transform transform = Controller.Transform.Find("Character_Shadow");
			if (transform != null)
			{
				Object.Destroy(transform.gameObject);
			}
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

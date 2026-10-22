using UnityEngine;

namespace Slugterra.Game.Units.Animals.SlugHound
{
	public class DeathState : SlugHoundState
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
			Transform transform = Controller.Transform.Find("Character_Shadow");
			if (transform != null)
			{
				Object.Destroy(transform.gameObject);
			}
			Controller.Animator.SetFloat("Speed", 0f);
			Controller.Animator.SetTrigger("Death");
			Controller.PathFinder.enabled = false;
			CharacterController component = Controller.GetComponent<CharacterController>();
			if (component != null)
			{
				Object.Destroy(component);
			}
			Controller.enabled = false;
		}
	}
}

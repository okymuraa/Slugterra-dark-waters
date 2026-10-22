using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityCharacterController
{
	[TaskDescription("Returns Success if the collider hit another object, otherwise Failure.")]
	[TaskCategory("Basic/CharacterController")]
	public class HasColliderHit : Conditional
	{
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("The tag of the GameObject to check for a collision against")]
		public SharedString tag = string.Empty;

		[Tooltip("The object that started the collision")]
		public SharedGameObject collidedGameObject;

		private bool enteredCollision;

		public override TaskStatus OnUpdate()
		{
			return (!enteredCollision) ? TaskStatus.Failure : TaskStatus.Success;
		}

		public override void OnEnd()
		{
			enteredCollision = false;
		}

		public override void OnControllerColliderHit(ControllerColliderHit hit)
		{
			if (string.IsNullOrEmpty(tag.Value) || tag.Value.Equals(hit.gameObject.tag))
			{
				collidedGameObject.Value = hit.gameObject;
				enteredCollision = true;
			}
		}

		public override void OnReset()
		{
			targetGameObject = null;
			tag = string.Empty;
			collidedGameObject = null;
		}
	}
}

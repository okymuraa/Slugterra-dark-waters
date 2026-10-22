using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	[TaskCategory("Physics")]
	[TaskDescription("Returns success when a collision ends.")]
	[HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=110")]
	public class HasExitedCollision : Conditional
	{
		[Tooltip("The tag of the GameObject to check for a collision against")]
		public SharedString tag = string.Empty;

		[Tooltip("The object that exited the collision")]
		public SharedGameObject collidedGameObject;

		private bool exitedCollision;

		public override TaskStatus OnUpdate()
		{
			return (!exitedCollision) ? TaskStatus.Failure : TaskStatus.Success;
		}

		public override void OnEnd()
		{
			exitedCollision = false;
		}

		public override void OnCollisionExit(Collision collision)
		{
			if (string.IsNullOrEmpty(tag.Value) || tag.Value.Equals(collision.gameObject.tag))
			{
				collidedGameObject.Value = collision.gameObject;
				exitedCollision = true;
			}
		}

		public override void OnReset()
		{
			collidedGameObject = null;
		}
	}
}

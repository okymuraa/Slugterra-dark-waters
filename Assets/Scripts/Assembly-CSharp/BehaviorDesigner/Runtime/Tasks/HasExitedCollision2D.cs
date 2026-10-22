using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	[HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=110")]
	[TaskDescription("Returns success when a 2D collision ends.")]
	[TaskCategory("Physics")]
	public class HasExitedCollision2D : Conditional
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

		public override void OnCollisionExit2D(Collision2D collision)
		{
			if (string.IsNullOrEmpty(tag.Value) || tag.Value.Equals(collision.gameObject.tag))
			{
				collidedGameObject.Value = collision.gameObject;
				exitedCollision = true;
			}
		}

		public override void OnReset()
		{
			tag = string.Empty;
			collidedGameObject = null;
		}
	}
}

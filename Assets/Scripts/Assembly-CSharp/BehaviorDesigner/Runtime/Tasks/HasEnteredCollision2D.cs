using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	[TaskCategory("Physics")]
	[HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=110")]
	[TaskDescription("Returns success when a 2D collision starts.")]
	public class HasEnteredCollision2D : Conditional
	{
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

		public override void OnCollisionEnter2D(Collision2D collision)
		{
			if (string.IsNullOrEmpty(tag.Value) || tag.Value.Equals(collision.gameObject.tag))
			{
				collidedGameObject.Value = collision.gameObject;
				enteredCollision = true;
			}
		}

		public override void OnReset()
		{
			tag = string.Empty;
			collidedGameObject = null;
		}
	}
}

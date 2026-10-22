using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	[TaskCategory("Physics")]
	[TaskDescription("Returns success when an object enters the trigger.")]
	[HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=110")]
	public class HasEnteredTrigger : Conditional
	{
		[Tooltip("The tag of the GameObject to check for a trigger against")]
		public SharedString tag = string.Empty;

		[Tooltip("The object that entered the trigger")]
		public SharedGameObject otherGameObject;

		private bool enteredTrigger;

		public override TaskStatus OnUpdate()
		{
			return (!enteredTrigger) ? TaskStatus.Failure : TaskStatus.Success;
		}

		public override void OnEnd()
		{
			enteredTrigger = false;
		}

		public override void OnTriggerEnter(Collider other)
		{
			if (string.IsNullOrEmpty(tag.Value) || tag.Value.Equals(other.gameObject.tag))
			{
				otherGameObject.Value = other.gameObject;
				enteredTrigger = true;
			}
		}

		public override void OnReset()
		{
			tag = string.Empty;
			otherGameObject = null;
		}
	}
}

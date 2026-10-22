using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	[TaskDescription("Returns success when an object exits the trigger.")]
	[TaskCategory("Physics")]
	[HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=110")]
	public class HasExitedTrigger : Conditional
	{
		[Tooltip("The tag of the GameObject to check for a trigger against")]
		public SharedString tag = string.Empty;

		[Tooltip("The object that exited the trigger")]
		public SharedGameObject otherGameObject;

		private bool exitedTrigger;

		public override TaskStatus OnUpdate()
		{
			return (!exitedTrigger) ? TaskStatus.Failure : TaskStatus.Success;
		}

		public override void OnEnd()
		{
			exitedTrigger = false;
		}

		public override void OnTriggerExit(Collider other)
		{
			if (string.IsNullOrEmpty(tag.Value) || tag.Value.Equals(other.gameObject.tag))
			{
				otherGameObject.Value = other.gameObject;
				exitedTrigger = true;
			}
		}

		public override void OnReset()
		{
			tag = string.Empty;
			otherGameObject = null;
		}
	}
}

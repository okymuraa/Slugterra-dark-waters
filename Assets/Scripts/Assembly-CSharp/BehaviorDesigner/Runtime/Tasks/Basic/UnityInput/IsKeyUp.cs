using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityInput
{
	[TaskDescription("Returns success when the specified key is released.")]
	[TaskCategory("Basic/Input")]
	public class IsKeyUp : Conditional
	{
		[Tooltip("The key to test")]
		public KeyCode key;

		public override TaskStatus OnUpdate()
		{
			return (!Input.GetKeyUp(key)) ? TaskStatus.Failure : TaskStatus.Success;
		}

		public override void OnReset()
		{
			key = KeyCode.None;
		}
	}
}

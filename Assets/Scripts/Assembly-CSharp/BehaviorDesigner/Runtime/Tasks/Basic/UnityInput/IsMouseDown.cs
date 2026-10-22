using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityInput
{
	[TaskDescription("Returns success when the specified mouse button is pressed.")]
	[TaskCategory("Basic/Input")]
	public class IsMouseDown : Conditional
	{
		[Tooltip("The button index")]
		public SharedInt buttonIndex;

		public override TaskStatus OnUpdate()
		{
			return (!Input.GetMouseButtonDown(buttonIndex.Value)) ? TaskStatus.Failure : TaskStatus.Success;
		}

		public override void OnReset()
		{
			buttonIndex = 0;
		}
	}
}

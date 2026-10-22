using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityInput
{
	[TaskDescription("Stores the state of the specified mouse button.")]
	[TaskCategory("Basic/Input")]
	public class GetMouseButton : Action
	{
		[Tooltip("The index of the button")]
		public SharedInt buttonIndex;

		[Tooltip("The stored result")]
		[RequiredField]
		public SharedBool storeResult;

		public override TaskStatus OnUpdate()
		{
			storeResult.Value = Input.GetMouseButton(buttonIndex.Value);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			buttonIndex = 0;
			storeResult = false;
		}
	}
}

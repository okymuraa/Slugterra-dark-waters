using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityInput
{
	[TaskCategory("Basic/Input")]
	[TaskDescription("Stores the pressed state of the specified key.")]
	public class GetKey : Action
	{
		[Tooltip("The key to test.")]
		public KeyCode key;

		[Tooltip("The stored result")]
		[RequiredField]
		public SharedBool storeResult;

		public override TaskStatus OnUpdate()
		{
			storeResult.Value = Input.GetKey(key);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			key = KeyCode.None;
			storeResult = false;
		}
	}
}

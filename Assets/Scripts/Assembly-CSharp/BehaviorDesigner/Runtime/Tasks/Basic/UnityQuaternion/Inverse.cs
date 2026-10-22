using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityQuaternion
{
	[TaskDescription("Stores the inverse of the specified quaternion.")]
	[TaskCategory("Basic/Quaternion")]
	public class Inverse : Action
	{
		[Tooltip("The target quaternion")]
		public SharedQuaternion targetQuaternion;

		[Tooltip("The stored quaternion")]
		[RequiredField]
		public SharedQuaternion storeResult;

		public override TaskStatus OnUpdate()
		{
			storeResult.Value = Quaternion.Inverse(targetQuaternion.Value);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			targetQuaternion = (storeResult = Quaternion.identity);
		}
	}
}

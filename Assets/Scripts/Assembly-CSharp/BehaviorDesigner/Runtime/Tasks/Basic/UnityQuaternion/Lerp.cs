using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityQuaternion
{
	[TaskCategory("Basic/Quaternion")]
	[TaskDescription("Lerps between two quaternions.")]
	public class Lerp : Action
	{
		[Tooltip("The from rotation")]
		public SharedQuaternion fromQuaternion;

		[Tooltip("The to rotation")]
		public SharedQuaternion toQuaternion;

		[Tooltip("The amount to lerp")]
		public SharedFloat amount;

		[RequiredField]
		[Tooltip("The stored result")]
		public SharedQuaternion storeResult;

		public override TaskStatus OnUpdate()
		{
			storeResult.Value = Quaternion.Lerp(fromQuaternion.Value, toQuaternion.Value, amount.Value);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			fromQuaternion = (toQuaternion = (storeResult = Quaternion.identity));
			amount = 0f;
		}
	}
}

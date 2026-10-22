using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityQuaternion
{
	[TaskDescription("Stores the quaternion after a rotation.")]
	[TaskCategory("Basic/Quaternion")]
	public class RotateTowards : Action
	{
		[Tooltip("The from rotation")]
		public SharedQuaternion fromQuaternion;

		[Tooltip("The to rotation")]
		public SharedQuaternion toQuaternion;

		[Tooltip("The maximum degrees delta")]
		public SharedFloat maxDeltaDegrees;

		[RequiredField]
		[Tooltip("The stored result")]
		public SharedQuaternion storeResult;

		public override TaskStatus OnUpdate()
		{
			storeResult.Value = Quaternion.RotateTowards(fromQuaternion.Value, toQuaternion.Value, maxDeltaDegrees.Value);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			fromQuaternion = (toQuaternion = (storeResult = Quaternion.identity));
			maxDeltaDegrees = 0f;
		}
	}
}

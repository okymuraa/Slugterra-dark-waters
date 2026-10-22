using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityQuaternion
{
	[TaskDescription("Stores the rotation which rotates the specified degrees around the specified axis.")]
	[TaskCategory("Basic/Quaternion")]
	public class AngleAxis : Action
	{
		[Tooltip("The number of degrees")]
		public SharedFloat degrees;

		[Tooltip("The axis direction")]
		public SharedVector3 axis;

		[RequiredField]
		[Tooltip("The stored result")]
		public SharedQuaternion storeResult;

		public override TaskStatus OnUpdate()
		{
			storeResult.Value = Quaternion.AngleAxis(degrees.Value, axis.Value);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			degrees = 0f;
			axis = Vector3.zero;
			storeResult = Quaternion.identity;
		}
	}
}

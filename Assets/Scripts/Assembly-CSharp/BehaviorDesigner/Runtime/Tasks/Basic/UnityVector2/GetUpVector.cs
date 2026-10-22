using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityVector2
{
	[TaskCategory("Basic/Vector2")]
	[TaskDescription("Stores the up vector value.")]
	public class GetUpVector : Action
	{
		[RequiredField]
		[Tooltip("The stored result")]
		public SharedVector2 storeResult;

		public override TaskStatus OnUpdate()
		{
			storeResult.Value = Vector2.up;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			storeResult = Vector2.zero;
		}
	}
}

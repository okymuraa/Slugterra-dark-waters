using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityInput
{
	[TaskDescription("Stores the mouse position.")]
	[TaskCategory("Basic/Input")]
	public class GetMousePosition : Action
	{
		[Tooltip("The stored result")]
		[RequiredField]
		public SharedVector2 storeResult;

		public override TaskStatus OnUpdate()
		{
			storeResult.Value = Input.mousePosition;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			storeResult = Vector2.zero;
		}
	}
}

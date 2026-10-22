using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityTime
{
	[TaskDescription("Returns the time in seconds it took to complete the last frame.")]
	[TaskCategory("Basic/Time")]
	public class GetDeltaTime : Action
	{
		[Tooltip("The variable to store the result")]
		public SharedFloat storeResult;

		public override TaskStatus OnUpdate()
		{
			storeResult.Value = Time.deltaTime;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			storeResult.Value = 0f;
		}
	}
}

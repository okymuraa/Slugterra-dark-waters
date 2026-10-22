using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityTime
{
	[TaskDescription("Returns the time in second since the start of the game.")]
	[TaskCategory("Basic/Time")]
	public class GetTime : Action
	{
		[Tooltip("The variable to store the result")]
		public SharedFloat storeResult;

		public override TaskStatus OnUpdate()
		{
			storeResult.Value = Time.time;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			storeResult.Value = 0f;
		}
	}
}

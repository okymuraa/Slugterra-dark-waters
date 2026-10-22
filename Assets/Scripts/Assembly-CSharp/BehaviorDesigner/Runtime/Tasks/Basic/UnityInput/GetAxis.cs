using UnityStandardAssets.CrossPlatformInput;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityInput
{
	[TaskDescription("Stores the value of the specified axis and stores it in a float.")]
	[TaskCategory("Basic/Input")]
	public class GetAxis : Action
	{
		[Tooltip("The name of the axis")]
		public SharedString axisName;

		[Tooltip("Axis values are in the range -1 to 1. Use the multiplier to set a larger range")]
		public SharedFloat multiplier;

		[Tooltip("The stored result")]
		[RequiredField]
		public SharedFloat storeResult;

		public override TaskStatus OnUpdate()
		{
			float num = CrossPlatformInputManager.GetAxis(axisName.Value);
			if (!multiplier.IsNone)
			{
				num *= multiplier.Value;
			}
			storeResult.Value = num;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			axisName = string.Empty;
			multiplier = 1f;
			storeResult = 0f;
		}
	}
}

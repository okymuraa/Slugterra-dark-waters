using UnityStandardAssets.CrossPlatformInput;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityInput
{
	[TaskDescription("Stores the state of the specified button.")]
	[TaskCategory("Basic/Input")]
	public class GetButton : Action
	{
		[Tooltip("The name of the button")]
		public SharedString buttonName;

		[Tooltip("The stored result")]
		[RequiredField]
		public SharedBool storeResult;

		public override TaskStatus OnUpdate()
		{
			storeResult.Value = CrossPlatformInputManager.GetButton(buttonName.Value);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			buttonName = "Fire1";
			storeResult = false;
		}
	}
}

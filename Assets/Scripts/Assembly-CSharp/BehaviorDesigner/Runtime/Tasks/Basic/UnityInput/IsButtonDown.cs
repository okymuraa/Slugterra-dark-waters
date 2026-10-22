using UnityStandardAssets.CrossPlatformInput;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityInput
{
	[TaskDescription("Returns success when the specified button is pressed.")]
	[TaskCategory("Basic/Input")]
	public class IsButtonDown : Conditional
	{
		[Tooltip("The name of the button")]
		public SharedString buttonName;

		public override TaskStatus OnUpdate()
		{
			return (!CrossPlatformInputManager.GetButtonDown(buttonName.Value)) ? TaskStatus.Failure : TaskStatus.Success;
		}

		public override void OnReset()
		{
			buttonName = "Fire1";
		}
	}
}

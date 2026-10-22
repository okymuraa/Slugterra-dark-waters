using UnityStandardAssets.CrossPlatformInput;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityInput
{
	[TaskDescription("Returns success when the specified button is released.")]
	[TaskCategory("Basic/Input")]
	public class IsButtonUp : Conditional
	{
		[Tooltip("The name of the button")]
		public SharedString buttonName;

		public override TaskStatus OnUpdate()
		{
			return (!CrossPlatformInputManager.GetButtonUp(buttonName.Value)) ? TaskStatus.Failure : TaskStatus.Success;
		}

		public override void OnReset()
		{
			buttonName = "Fire1";
		}
	}
}

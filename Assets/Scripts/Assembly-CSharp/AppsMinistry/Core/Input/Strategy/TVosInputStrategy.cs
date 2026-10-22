using EasyInput.Core;

namespace AppsMinistry.Core.Input.Strategy
{
	public class TVosInputStrategy : BaseJoysticInputStrategy
	{
		public override void Activate()
		{
			EasyInputHelper.On_Dpad += onDpad;
			EasyInputHelper.On_TouchEnd += onTouchEnd;
		}

		public override void Deactivate()
		{
			EasyInputHelper.On_Dpad -= onDpad;
			EasyInputHelper.On_TouchEnd -= onTouchEnd;
		}

		private void onTouch(InputTouch inputTouch)
		{
			Use_OnMoveEvent_movement(inputTouch.currentTouchPosition);
		}

		private void onTouchEnd(InputTouch inputTouch)
		{
			Use_OnMoveEndEvent_movement();
		}

		private void onDpad(ControllerAxis axis)
		{
			Use_OnMoveEvent_movement(axis.axisValue);
		}
	}
}

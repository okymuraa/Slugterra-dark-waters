using EasyInput.Core;
using UnityEngine;

namespace EasyInput.StandardControllers
{
	[AddComponentMenu("EasyInputHelper/Standard Controllers/StandardAxisController")]
	public class StandardAxisController : MonoBehaviour
	{
		public EasyInputConstants.PLAYER_NUMBER player;

		public EasyInputConstants.CONTROLLER_AXIS control;

		public EasyInputConstants.AXIS axisHorizontal;

		public EasyInputConstants.AXIS axisVertical = EasyInputConstants.AXIS.YAxis;

		public EasyInputConstants.ACTION_TYPE action;

		public float sensitivity = 0.01f;

		private Vector3 actionVector3;

		private float horizontal;

		private float vertical;

		private void OnEnable()
		{
			switch (control)
			{
			case EasyInputConstants.CONTROLLER_AXIS.LeftStick:
				EasyInputHelper.On_LeftStick += localAxis;
				break;
			case EasyInputConstants.CONTROLLER_AXIS.RightStick:
				EasyInputHelper.On_RightStick += localAxis;
				break;
			case EasyInputConstants.CONTROLLER_AXIS.DPad:
				EasyInputHelper.On_Dpad += localAxis;
				break;
			case EasyInputConstants.CONTROLLER_AXIS.LeftTrigger:
				EasyInputHelper.On_LeftTrigger += localAxis;
				break;
			case EasyInputConstants.CONTROLLER_AXIS.RightTrigger:
				EasyInputHelper.On_RightTrigger += localAxis;
				break;
			}
		}

		private void OnDestroy()
		{
			switch (control)
			{
			case EasyInputConstants.CONTROLLER_AXIS.LeftStick:
				EasyInputHelper.On_LeftStick -= localAxis;
				break;
			case EasyInputConstants.CONTROLLER_AXIS.RightStick:
				EasyInputHelper.On_RightStick -= localAxis;
				break;
			case EasyInputConstants.CONTROLLER_AXIS.DPad:
				EasyInputHelper.On_Dpad -= localAxis;
				break;
			case EasyInputConstants.CONTROLLER_AXIS.LeftTrigger:
				EasyInputHelper.On_LeftTrigger -= localAxis;
				break;
			case EasyInputConstants.CONTROLLER_AXIS.RightTrigger:
				EasyInputHelper.On_RightTrigger -= localAxis;
				break;
			}
		}

		private void Update()
		{
		}

		private void localAxis(ControllerAxis axis)
		{
			if (axis.player == player || player == EasyInputConstants.PLAYER_NUMBER.Any)
			{
				horizontal = axis.axisValue.x * sensitivity * Time.deltaTime * 100f;
				vertical = axis.axisValue.y * sensitivity * Time.deltaTime * 100f;
				actionVector3 = EasyInputUtilities.getControllerVector3(horizontal, vertical, axisHorizontal, axisVertical);
				switch (action)
				{
				case EasyInputConstants.ACTION_TYPE.Position:
					transform.position += actionVector3;
					break;
				case EasyInputConstants.ACTION_TYPE.Rotation:
					transform.Rotate(actionVector3, Space.World);
					break;
				case EasyInputConstants.ACTION_TYPE.LocalPosition:
					transform.Translate(actionVector3);
					break;
				case EasyInputConstants.ACTION_TYPE.LocalRotation:
					transform.Rotate(actionVector3);
					break;
				case EasyInputConstants.ACTION_TYPE.LocalScale:
					transform.localScale += actionVector3;
					break;
				default:
					Debug.Log("Invalid Action");
					break;
				}
			}
		}
	}
}

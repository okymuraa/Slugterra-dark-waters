using System;
using EasyInput.Core;
using UnityEngine;
using UnityEngine.Events;

namespace EasyInput.StandardControllers
{
	[AddComponentMenu("EasyInputHelper/Custom Controllers/CustomAxisController")]
	public class CustomAxisController : MonoBehaviour
	{
		[Serializable]
		public class AxisHandler : UnityEvent<ControllerAxis>
		{
		}

		public EasyInputConstants.PLAYER_NUMBER player;

		[SerializeField]
		public AxisHandler onLeftStick;

		[SerializeField]
		public AxisHandler onRightStick;

		[SerializeField]
		public AxisHandler onDpad;

		[SerializeField]
		public AxisHandler onLeftTrigger;

		[SerializeField]
		public AxisHandler onRightTrigger;

		private void OnEnable()
		{
			EasyInputHelper.On_LeftStick += localLeftStick;
			EasyInputHelper.On_RightStick += localRightStick;
			EasyInputHelper.On_Dpad += localDpad;
			EasyInputHelper.On_LeftTrigger += localLeftTrigger;
			EasyInputHelper.On_RightTrigger += localRightTrigger;
		}

		private void OnDestroy()
		{
			EasyInputHelper.On_LeftStick -= localLeftStick;
			EasyInputHelper.On_RightStick -= localRightStick;
			EasyInputHelper.On_Dpad -= localDpad;
			EasyInputHelper.On_LeftTrigger -= localLeftTrigger;
			EasyInputHelper.On_RightTrigger -= localRightTrigger;
		}

		private void Update()
		{
		}

		private void localLeftStick(ControllerAxis axis)
		{
			if (axis.player == player || player == EasyInputConstants.PLAYER_NUMBER.Any)
			{
				onLeftStick.Invoke(axis);
			}
		}

		private void localRightStick(ControllerAxis axis)
		{
			if (axis.player == player || player == EasyInputConstants.PLAYER_NUMBER.Any)
			{
				onRightStick.Invoke(axis);
			}
		}

		private void localDpad(ControllerAxis axis)
		{
			if (axis.player == player || player == EasyInputConstants.PLAYER_NUMBER.Any)
			{
				onDpad.Invoke(axis);
			}
		}

		private void localLeftTrigger(ControllerAxis axis)
		{
			if (axis.player == player || player == EasyInputConstants.PLAYER_NUMBER.Any)
			{
				onLeftTrigger.Invoke(axis);
			}
		}

		private void localRightTrigger(ControllerAxis axis)
		{
			if (axis.player == player || player == EasyInputConstants.PLAYER_NUMBER.Any)
			{
				onRightTrigger.Invoke(axis);
			}
		}
	}
}

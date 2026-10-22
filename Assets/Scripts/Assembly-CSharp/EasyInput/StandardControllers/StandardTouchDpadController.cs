using EasyInput.Core;
using UnityEngine;

namespace EasyInput.StandardControllers
{
	[AddComponentMenu("EasyInputHelper/Standard Controllers/StandardTouchDpadController")]
	public class StandardTouchDpadController : MonoBehaviour
	{
		[Range(0f, 1f)]
		public float deadZone = 0.2f;

		public EasyInputConstants.AXIS axisHorizontal;

		public EasyInputConstants.AXIS axisVertical = EasyInputConstants.AXIS.YAxis;

		public EasyInputConstants.ACTION_TYPE action;

		public float sensitivity = 1f;

		public EasyInputConstants.DPAD_MODE dpadMode;

		private Vector3 actionVector3;

		private float horizontal;

		private float vertical;

		private bool isClicking;

		private void OnEnable()
		{
			EasyInputHelper.On_Touch += localAxis;
			if (dpadMode == EasyInputConstants.DPAD_MODE.RegisterOnlyWhenNotClicking)
			{
				EasyInputHelper.On_ClickStart += localClickStart;
				EasyInputHelper.On_ClickEnd += localClickEnd;
			}
		}

		private void OnDestroy()
		{
			EasyInputHelper.On_Touch -= localAxis;
			if (dpadMode == EasyInputConstants.DPAD_MODE.RegisterOnlyWhenNotClicking)
			{
				EasyInputHelper.On_ClickStart -= localClickStart;
				EasyInputHelper.On_ClickEnd -= localClickEnd;
			}
		}

		private void Update()
		{
		}

		private void localAxis(InputTouch touch)
		{
			if (dpadMode == EasyInputConstants.DPAD_MODE.RegisterAlways || !isClicking)
			{
				if (Mathf.Sqrt(Mathf.Pow(touch.currentTouchPosition.x, 2f) + Mathf.Pow(touch.currentTouchPosition.y, 2f)) > deadZone)
				{
					horizontal = touch.currentTouchPosition.x * sensitivity * Time.deltaTime * 100f;
					vertical = touch.currentTouchPosition.y * sensitivity * Time.deltaTime * 100f;
				}
				else
				{
					horizontal = 0f;
					vertical = 0f;
				}
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

		private void localClickStart(ButtonClick click)
		{
			if (click.button == EasyInputConstants.CONTROLLER_BUTTON.AButton)
			{
				isClicking = true;
			}
		}

		private void localClickEnd(ButtonClick click)
		{
			if (click.button == EasyInputConstants.CONTROLLER_BUTTON.AButton)
			{
				isClicking = false;
			}
		}
	}
}

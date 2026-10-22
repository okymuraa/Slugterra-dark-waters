using System.Collections;
using EasyInput.Core;
using UnityEngine;
using UnityEngine.UI;

namespace EasyInput.Misc
{
	[AddComponentMenu("EasyInputHelper/Miscellaneous/ControllerDiagnostics")]
	public class ControllerDiagnostics : MonoBehaviour
	{
		public Text clickStartValue;

		public Text clickEndValue;

		public Text clickValue;

		public Text LongStartValue;

		public Text LongEndValue;

		public Text LongValue;

		public Text quickEndValue;

		public Text doubleEndValue;

		public Text leftStickValue;

		public Text rightStickValue;

		public Text dpadValue;

		public Text leftTriggerValue;

		public Text rightTriggerValue;

		private bool touching;

		private bool longTouching;

		private bool leftStick;

		private bool rightStick;

		private bool dPad;

		private bool leftTrigger;

		private bool rightTrigger;

		private void OnEnable()
		{
			EasyInputHelper.On_ClickStart += localTouchStart;
			EasyInputHelper.On_ClickEnd += localTouchEnd;
			EasyInputHelper.On_Click += localTouch;
			EasyInputHelper.On_LongClickStart += localLongTouchStart;
			EasyInputHelper.On_LongClickEnd += localLongTouchEnd;
			EasyInputHelper.On_LongClick += localLongTouch;
			EasyInputHelper.On_QuickClickEnd += localQuickTouchEnd;
			EasyInputHelper.On_DoubleClickEnd += localDoubleTouchEnd;
			EasyInputHelper.On_LeftStick += localLeftStick;
			EasyInputHelper.On_RightStick += localRightStick;
			EasyInputHelper.On_Dpad += localDpad;
			EasyInputHelper.On_LeftTrigger += localLeftTrigger;
			EasyInputHelper.On_RightTrigger += localRightTrigger;
		}

		private void OnDestroy()
		{
			EasyInputHelper.On_ClickStart -= localTouchStart;
			EasyInputHelper.On_ClickEnd -= localTouchEnd;
			EasyInputHelper.On_Click -= localTouch;
			EasyInputHelper.On_LongClickStart -= localLongTouchStart;
			EasyInputHelper.On_LongClickEnd -= localLongTouchEnd;
			EasyInputHelper.On_LongClick -= localLongTouch;
			EasyInputHelper.On_QuickClickEnd -= localQuickTouchEnd;
			EasyInputHelper.On_DoubleClickEnd -= localDoubleTouchEnd;
			EasyInputHelper.On_LeftStick -= localLeftStick;
			EasyInputHelper.On_RightStick -= localRightStick;
			EasyInputHelper.On_Dpad -= localDpad;
			EasyInputHelper.On_LeftTrigger -= localLeftTrigger;
			EasyInputHelper.On_RightTrigger -= localRightTrigger;
		}

		private void Update()
		{
			if (!touching)
			{
				clickValue.text = string.Empty;
			}
			else
			{
				clickValue.text = "Fired";
				touching = false;
			}
			if (!longTouching)
			{
				LongValue.text = string.Empty;
			}
			else
			{
				LongValue.text = "Fired";
				longTouching = false;
			}
			if (!leftStick)
			{
				leftStickValue.text = string.Empty;
			}
			else
			{
				leftStickValue.text = "Pushed";
				leftStick = false;
			}
			if (!rightStick)
			{
				rightStickValue.text = string.Empty;
			}
			else
			{
				rightStickValue.text = "Pushed";
				rightStick = false;
			}
			if (!dPad)
			{
				dpadValue.text = string.Empty;
			}
			else
			{
				dpadValue.text = "Pushed";
				dPad = false;
			}
			if (!leftTrigger)
			{
				leftTriggerValue.text = string.Empty;
			}
			else
			{
				leftTriggerValue.text = "Pushed";
				leftTrigger = false;
			}
			if (!rightTrigger)
			{
				rightTriggerValue.text = string.Empty;
				return;
			}
			rightTriggerValue.text = "Pushed";
			rightTrigger = false;
		}

		private void localTouchStart(ButtonClick touch)
		{
			clickStartValue.text = "Fired";
			StartCoroutine(ClearTextTouchStart(clickStartValue, 0.5f));
		}

		private IEnumerator ClearTextTouchStart(Text textObject, float delay)
		{
			yield return new WaitForSeconds(delay);
			textObject.text = string.Empty;
		}

		private void localTouchEnd(ButtonClick touch)
		{
			clickEndValue.text = "Fired";
			StartCoroutine(ClearTextTouchEnd(clickEndValue, 0.5f));
		}

		private IEnumerator ClearTextTouchEnd(Text textObject, float delay)
		{
			yield return new WaitForSeconds(delay);
			textObject.text = string.Empty;
		}

		private void localTouch(ButtonClick touch)
		{
			touching = true;
		}

		private void localLongTouchStart(ButtonClick touch)
		{
			LongStartValue.text = "Fired";
			StartCoroutine(ClearLongTextTouchStart(LongStartValue, 0.5f));
		}

		private IEnumerator ClearLongTextTouchStart(Text textObject, float delay)
		{
			yield return new WaitForSeconds(delay);
			textObject.text = string.Empty;
		}

		private void localLongTouchEnd(ButtonClick touch)
		{
			LongEndValue.text = "Fired";
			StartCoroutine(ClearLongTextTouchEnd(LongEndValue, 0.5f));
		}

		private IEnumerator ClearLongTextTouchEnd(Text textObject, float delay)
		{
			yield return new WaitForSeconds(delay);
			textObject.text = string.Empty;
		}

		private void localLongTouch(ButtonClick touch)
		{
			longTouching = true;
		}

		private void localQuickTouchEnd(ButtonClick touch)
		{
			quickEndValue.text = "Fired";
			StartCoroutine(ClearQuickTextTouchEnd(quickEndValue, 0.5f));
		}

		private IEnumerator ClearQuickTextTouchEnd(Text textObject, float delay)
		{
			yield return new WaitForSeconds(delay);
			textObject.text = string.Empty;
		}

		private void localDoubleTouchEnd(ButtonClick touch)
		{
			doubleEndValue.text = "Fired";
			StartCoroutine(ClearDoubleTextTouchEnd(doubleEndValue, 0.5f));
		}

		private IEnumerator ClearDoubleTextTouchEnd(Text textObject, float delay)
		{
			yield return new WaitForSeconds(delay);
			textObject.text = string.Empty;
		}

		private void localLeftStick(ControllerAxis axis)
		{
			if (axis.axisValue.x > 0.05f || axis.axisValue.y > 0.05f || axis.axisValue.x < -0.05f || axis.axisValue.y < -0.05f)
			{
				leftStick = true;
			}
		}

		private void localRightStick(ControllerAxis axis)
		{
			if (axis.axisValue.x > 0.05f || axis.axisValue.y > 0.05f || axis.axisValue.x < -0.05f || axis.axisValue.y < -0.05f)
			{
				rightStick = true;
			}
		}

		private void localDpad(ControllerAxis axis)
		{
			if (axis.axisValue.x > 0.05f || axis.axisValue.y > 0.05f || axis.axisValue.x < -0.05f || axis.axisValue.y < -0.05f)
			{
				dPad = true;
			}
		}

		private void localLeftTrigger(ControllerAxis axis)
		{
			if (axis.axisValue.x > 0.05f || axis.axisValue.y > 0.05f || axis.axisValue.x < -0.05f || axis.axisValue.y < -0.05f)
			{
				leftTrigger = true;
			}
		}

		private void localRightTrigger(ControllerAxis axis)
		{
			if (axis.axisValue.x > 0.05f || axis.axisValue.y > 0.05f || axis.axisValue.x < -0.05f || axis.axisValue.y < -0.05f)
			{
				rightTrigger = true;
			}
		}
	}
}

using System.Collections;
using EasyInput.Core;
using UnityEngine;
using UnityEngine.UI;

namespace EasyInput.Misc
{
	[AddComponentMenu("EasyInputHelper/Miscellaneous/TouchDiagnostics")]
	public class TouchDiagnostics : MonoBehaviour
	{
		public Text touchStartValue;

		public Text touchEndValue;

		public Text touchValue;

		public Text LongStartValue;

		public Text LongEndValue;

		public Text LongValue;

		public Text quickEndValue;

		public Text doubleEndValue;

		public Text swipeValue;

		public Text accelXValue;

		public Text accelYValue;

		public Text accelZValue;

		public Text userAccelXValue;

		public Text userAccelYValue;

		public Text userAccelZValue;

		public Text gravityXValue;

		public Text gravityYValue;

		public Text gravityZValue;

		private bool touching;

		private bool longTouching;

		private void OnEnable()
		{
			EasyInputHelper.On_TouchStart += localTouchStart;
			EasyInputHelper.On_TouchEnd += localTouchEnd;
			EasyInputHelper.On_Touch += localTouch;
			EasyInputHelper.On_LongTouchStart += localLongTouchStart;
			EasyInputHelper.On_LongTouchEnd += localLongTouchEnd;
			EasyInputHelper.On_LongTouch += localLongTouch;
			EasyInputHelper.On_QuickTouchEnd += localQuickTouchEnd;
			EasyInputHelper.On_DoubleTouchEnd += localDoubleTouchEnd;
			EasyInputHelper.On_SwipeDetected += localSwipe;
			EasyInputHelper.On_Accelerometer += localAccel;
			EasyInputHelper.On_Gyro += localGyro;
		}

		private void OnDestroy()
		{
			EasyInputHelper.On_TouchStart -= localTouchStart;
			EasyInputHelper.On_TouchEnd -= localTouchEnd;
			EasyInputHelper.On_Touch -= localTouch;
			EasyInputHelper.On_LongTouchStart -= localLongTouchStart;
			EasyInputHelper.On_LongTouchEnd -= localLongTouchEnd;
			EasyInputHelper.On_LongTouch -= localLongTouch;
			EasyInputHelper.On_QuickTouchEnd -= localQuickTouchEnd;
			EasyInputHelper.On_DoubleTouchEnd -= localDoubleTouchEnd;
			EasyInputHelper.On_SwipeDetected -= localSwipe;
			EasyInputHelper.On_Accelerometer -= localAccel;
			EasyInputHelper.On_Gyro -= localGyro;
		}

		private void Update()
		{
			if (!touching)
			{
				touchValue.text = string.Empty;
			}
			else
			{
				touchValue.text = "Fired";
				touching = false;
			}
			if (!longTouching)
			{
				LongValue.text = string.Empty;
				return;
			}
			LongValue.text = "Fired";
			longTouching = false;
		}

		private void localTouchStart(InputTouch touch)
		{
			touchStartValue.text = "Fired";
			StartCoroutine(ClearTextTouchStart(touchStartValue, 0.5f));
		}

		private IEnumerator ClearTextTouchStart(Text textObject, float delay)
		{
			yield return new WaitForSeconds(delay);
			textObject.text = string.Empty;
		}

		private void localTouchEnd(InputTouch touch)
		{
			touchEndValue.text = "Fired";
			StartCoroutine(ClearTextTouchEnd(touchEndValue, 0.5f));
		}

		private IEnumerator ClearTextTouchEnd(Text textObject, float delay)
		{
			yield return new WaitForSeconds(delay);
			textObject.text = string.Empty;
		}

		private void localTouch(InputTouch touch)
		{
			touching = true;
		}

		private void localLongTouchStart(InputTouch touch)
		{
			LongStartValue.text = "Fired";
			StartCoroutine(ClearLongTextTouchStart(LongStartValue, 0.5f));
		}

		private IEnumerator ClearLongTextTouchStart(Text textObject, float delay)
		{
			yield return new WaitForSeconds(delay);
			textObject.text = string.Empty;
		}

		private void localLongTouchEnd(InputTouch touch)
		{
			LongEndValue.text = "Fired";
			StartCoroutine(ClearLongTextTouchEnd(LongEndValue, 0.5f));
		}

		private IEnumerator ClearLongTextTouchEnd(Text textObject, float delay)
		{
			yield return new WaitForSeconds(delay);
			textObject.text = string.Empty;
		}

		private void localLongTouch(InputTouch touch)
		{
			longTouching = true;
		}

		private void localQuickTouchEnd(InputTouch touch)
		{
			quickEndValue.text = "Fired";
			StartCoroutine(ClearQuickTextTouchEnd(quickEndValue, 0.5f));
		}

		private IEnumerator ClearQuickTextTouchEnd(Text textObject, float delay)
		{
			yield return new WaitForSeconds(delay);
			textObject.text = string.Empty;
		}

		private void localDoubleTouchEnd(InputTouch touch)
		{
			doubleEndValue.text = "Fired";
			StartCoroutine(ClearDoubleTextTouchEnd(doubleEndValue, 0.5f));
		}

		private IEnumerator ClearDoubleTextTouchEnd(Text textObject, float delay)
		{
			yield return new WaitForSeconds(delay);
			textObject.text = string.Empty;
		}

		private void localSwipe(InputTouch touch)
		{
			swipeValue.text = "Fired";
			StartCoroutine(ClearSwipeTextTouchEnd(swipeValue, 0.5f));
		}

		private IEnumerator ClearSwipeTextTouchEnd(Text textObject, float delay)
		{
			yield return new WaitForSeconds(delay);
			textObject.text = string.Empty;
		}

		private void localAccel(Vector3 accel)
		{
			accelXValue.text = accel.x.ToString();
			accelYValue.text = accel.y.ToString();
			accelZValue.text = accel.z.ToString();
		}

		private void localGyro(Vector3 gravity, Vector3 userAccel)
		{
			userAccelXValue.text = userAccel.x.ToString();
			userAccelYValue.text = userAccel.y.ToString();
			userAccelZValue.text = userAccel.z.ToString();
			gravityXValue.text = gravity.x.ToString();
			gravityYValue.text = gravity.y.ToString();
			gravityZValue.text = gravity.z.ToString();
		}
	}
}

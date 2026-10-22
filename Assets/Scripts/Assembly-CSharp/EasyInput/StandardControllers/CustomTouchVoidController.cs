using System;
using EasyInput.Core;
using UnityEngine;
using UnityEngine.Events;

namespace EasyInput.StandardControllers
{
	[AddComponentMenu("EasyInputHelper/Custom Controllers/CustomTouchVoidController")]
	public class CustomTouchVoidController : MonoBehaviour
	{
		[Serializable]
		public class TouchHandler : UnityEvent
		{
		}

		[SerializeField]
		public TouchHandler onTouchStart;

		[SerializeField]
		public TouchHandler onTouchEnd;

		[SerializeField]
		public TouchHandler onTouch;

		[SerializeField]
		public TouchHandler onQuickTouchEnd;

		[SerializeField]
		public TouchHandler onDoubleTouchEnd;

		[SerializeField]
		public TouchHandler onLongTouchStart;

		[SerializeField]
		public TouchHandler onLongTouchEnd;

		[SerializeField]
		public TouchHandler onLongTouch;

		[SerializeField]
		public TouchHandler onSwipe;

		private void OnEnable()
		{
			EasyInputHelper.On_Touch += localButtonClick;
			EasyInputHelper.On_TouchEnd += localButtonClickEnd;
			EasyInputHelper.On_TouchStart += localButtonClickStart;
			EasyInputHelper.On_DoubleTouchEnd += localButtonDoubleClickEnd;
			EasyInputHelper.On_LongTouch += localButtonLongClick;
			EasyInputHelper.On_LongTouchEnd += localButtonLongClickEnd;
			EasyInputHelper.On_LongTouchStart += localButtonLongClickStart;
			EasyInputHelper.On_QuickTouchEnd += localButtonQuickClickEnd;
			EasyInputHelper.On_SwipeDetected += localSwipe;
		}

		private void OnDestroy()
		{
			EasyInputHelper.On_Touch -= localButtonClick;
			EasyInputHelper.On_TouchEnd -= localButtonClickEnd;
			EasyInputHelper.On_TouchStart -= localButtonClickStart;
			EasyInputHelper.On_DoubleTouchEnd -= localButtonDoubleClickEnd;
			EasyInputHelper.On_LongTouch -= localButtonLongClick;
			EasyInputHelper.On_LongTouchEnd -= localButtonLongClickEnd;
			EasyInputHelper.On_LongTouchStart -= localButtonLongClickStart;
			EasyInputHelper.On_QuickTouchEnd -= localButtonQuickClickEnd;
			EasyInputHelper.On_SwipeDetected -= localSwipe;
		}

		private void Update()
		{
		}

		private void localButtonClick(InputTouch button)
		{
			onTouch.Invoke();
		}

		private void localButtonClickStart(InputTouch button)
		{
			onTouchStart.Invoke();
		}

		private void localButtonClickEnd(InputTouch button)
		{
			onTouchEnd.Invoke();
		}

		private void localButtonQuickClickEnd(InputTouch button)
		{
			onQuickTouchEnd.Invoke();
		}

		private void localButtonDoubleClickEnd(InputTouch button)
		{
			onDoubleTouchEnd.Invoke();
		}

		private void localButtonLongClick(InputTouch button)
		{
			onLongTouch.Invoke();
		}

		private void localButtonLongClickStart(InputTouch button)
		{
			onLongTouchStart.Invoke();
		}

		private void localButtonLongClickEnd(InputTouch button)
		{
			onLongTouchEnd.Invoke();
		}

		private void localSwipe(InputTouch button)
		{
			onSwipe.Invoke();
		}
	}
}

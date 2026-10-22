using System;
using EasyInput.Core;
using UnityEngine.UI;

namespace UnityEngine.EventSystems
{
	[AddComponentMenu("EasyInputHelper/Input Modules/Easy Input Module")]
	public class EasyInputModule : BaseInputModule
	{
		public float repeatEventRate = 0.25f;

		public EasyInputConstants.INPUT_MODULE_BUTTON_MODE buttonMode;

		[Range(0.0001f, 1f)]
		public float scrollAmount = 0.01f;

		[Range(1f, 100f)]
		public int scrollSpeedMultiplier = 3;

		private Vector2[] leftStick = new Vector2[3];

		private Vector2[] dpad = new Vector2[3];

		private InputTouch touchpad = new InputTouch();

		private bool submit;

		private bool cancel;

		private bool submitForOneFrame;

		private bool cancelForOneFrame;

		private int leftStepsFired = 1;

		private int rightStepsFired = 1;

		private int upStepsFired = 1;

		private int downStepsFired = 1;

		private DateTime lastEvent = DateTime.UtcNow;

		private Scrollbar currentScrollbar;

		private Slider currentSlider;

		protected EasyInputModule()
		{
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			EasyInputHelper.On_ClickStart += localClickStart;
			EasyInputHelper.On_ClickEnd += localClickEnd;
			EasyInputHelper.On_LeftStick += localLeftStick;
			EasyInputHelper.On_Dpad += localDpad;
			EasyInputHelper.On_Touch += localTouch;
			EasyInputHelper.On_TouchEnd += localTouchEnd;
		}

		protected override void OnDestroy()
		{
			EasyInputHelper.On_ClickStart -= localClickStart;
			EasyInputHelper.On_ClickEnd -= localClickEnd;
			EasyInputHelper.On_LeftStick -= localLeftStick;
			EasyInputHelper.On_Dpad -= localDpad;
			EasyInputHelper.On_Touch -= localTouch;
			EasyInputHelper.On_TouchEnd -= localTouchEnd;
			base.OnDestroy();
		}

		public override void UpdateModule()
		{
		}

		public override bool IsModuleSupported()
		{
			return true;
		}

		public override bool ShouldActivateModule()
		{
			if (!base.ShouldActivateModule())
			{
				return false;
			}
			bool flag = false;
			flag |= submit;
			flag |= cancel;
			flag |= leftStick[0] != Vector2.zero;
			flag |= leftStick[1] != Vector2.zero;
			flag |= leftStick[2] != Vector2.zero;
			flag |= dpad[0] != Vector2.zero;
			flag |= dpad[1] != Vector2.zero;
			flag |= dpad[2] != Vector2.zero;
			return flag | (touchpad.currentTouchPosition != EasyInputConstants.NOT_TOUCHING);
		}

		public override void ActivateModule()
		{
			base.ActivateModule();
			GameObject gameObject = eventSystem.currentSelectedGameObject;
			if (gameObject == null)
			{
				gameObject = eventSystem.firstSelectedGameObject;
			}
			eventSystem.SetSelectedGameObject(null, GetBaseEventData());
			eventSystem.SetSelectedGameObject(gameObject, GetBaseEventData());
		}

		public override void DeactivateModule()
		{
			base.DeactivateModule();
			eventSystem.SetSelectedGameObject(null, GetBaseEventData());
		}

		public override void Process()
		{
			bool flag = SendUpdateEventToSelectedObject();
			if (eventSystem.sendNavigationEvents)
			{
				if (!flag)
				{
					flag |= SendTouchEventToSelectedObject();
				}
				if (!flag)
				{
					flag |= SendMoveEventToSelectedObject();
				}
				if (!flag)
				{
					SendSubmitEventToSelectedObject();
				}
			}
		}

		private bool SendSubmitEventToSelectedObject()
		{
			if (eventSystem.currentSelectedGameObject == null)
			{
				return false;
			}
			if ((DateTime.UtcNow - lastEvent).TotalSeconds < (double)repeatEventRate)
			{
				return false;
			}
			BaseEventData baseEventData = GetBaseEventData();
			if (submit)
			{
				lastEvent = DateTime.UtcNow;
				ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.submitHandler);
				if (submitForOneFrame)
				{
					submit = false;
					submitForOneFrame = false;
				}
			}
			if (cancel)
			{
				lastEvent = DateTime.UtcNow;
				ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.cancelHandler);
				if (cancelForOneFrame)
				{
					cancel = false;
					cancelForOneFrame = false;
				}
			}
			return baseEventData.used;
		}

		private bool SendTouchEventToSelectedObject()
		{
			Vector2 vector = Vector2.zero;
			Vector2 zero = Vector2.zero;
			if (EventSystem.current.currentSelectedGameObject.GetComponent<Scrollbar>() != null)
			{
				currentScrollbar = EventSystem.current.currentSelectedGameObject.GetComponent<Scrollbar>();
				currentSlider = null;
				if (currentScrollbar.direction == Scrollbar.Direction.LeftToRight || currentScrollbar.direction == Scrollbar.Direction.RightToLeft)
				{
					zero.x = scrollAmount * 4f;
					zero.y = EasyInputHelper.mInstance.requiredSwipeLength;
				}
				else
				{
					zero.x = EasyInputHelper.mInstance.requiredSwipeLength;
					zero.y = scrollAmount * 4f;
				}
			}
			else if (EventSystem.current.currentSelectedGameObject.GetComponent<ScrollRect>() != null)
			{
				if (EventSystem.current.currentSelectedGameObject.GetComponent<ScrollRect>().verticalScrollbar != null && EventSystem.current.currentSelectedGameObject.GetComponent<ScrollRect>().horizontalScrollbar != null)
				{
					currentScrollbar = EventSystem.current.currentSelectedGameObject.GetComponent<ScrollRect>().verticalScrollbar;
				}
				else if (EventSystem.current.currentSelectedGameObject.GetComponent<ScrollRect>().verticalScrollbar != null)
				{
					currentScrollbar = EventSystem.current.currentSelectedGameObject.GetComponent<ScrollRect>().verticalScrollbar;
				}
				else if (EventSystem.current.currentSelectedGameObject.GetComponent<ScrollRect>().horizontalScrollbar != null)
				{
					currentScrollbar = EventSystem.current.currentSelectedGameObject.GetComponent<ScrollRect>().horizontalScrollbar;
				}
				else
				{
					currentScrollbar = null;
				}
				currentSlider = null;
				if (currentScrollbar != null)
				{
					if (currentScrollbar.direction == Scrollbar.Direction.LeftToRight || currentScrollbar.direction == Scrollbar.Direction.RightToLeft)
					{
						zero.x = scrollAmount * 4f;
						zero.y = EasyInputHelper.mInstance.requiredSwipeLength;
					}
					else
					{
						zero.x = EasyInputHelper.mInstance.requiredSwipeLength;
						zero.y = scrollAmount * 4f;
					}
				}
			}
			else if (EventSystem.current.currentSelectedGameObject.GetComponent<Slider>() != null)
			{
				currentSlider = EventSystem.current.currentSelectedGameObject.GetComponent<Slider>();
				currentScrollbar = null;
				if (currentSlider.direction == Slider.Direction.LeftToRight || currentSlider.direction == Slider.Direction.RightToLeft)
				{
					zero.x = scrollAmount * 4f;
					zero.y = EasyInputHelper.mInstance.requiredSwipeLength;
				}
				else
				{
					zero.x = EasyInputHelper.mInstance.requiredSwipeLength;
					zero.y = scrollAmount * 4f;
				}
			}
			else
			{
				currentScrollbar = null;
				currentSlider = null;
				zero.x = EasyInputHelper.mInstance.requiredSwipeLength;
				zero.y = EasyInputHelper.mInstance.requiredSwipeLength;
			}
			if (touchpad.currentTouchPosition != EasyInputConstants.NOT_TOUCHING)
			{
				vector = touchpad.currentTouchPosition - touchpad.originalTouchPosition;
				if (vector.x > zero.x * (float)rightStepsFired)
				{
					vector.x = 1f;
					rightStepsFired++;
					if (currentScrollbar != null || currentSlider != null)
					{
						vector.x = scrollSpeedMultiplier;
						rightStepsFired += scrollSpeedMultiplier - 1;
					}
				}
				else if (vector.x < (0f - zero.x) * (float)leftStepsFired)
				{
					vector.x = -1f;
					leftStepsFired++;
					if (currentScrollbar != null || currentSlider != null)
					{
						vector.x = -scrollSpeedMultiplier;
						leftStepsFired += scrollSpeedMultiplier - 1;
					}
				}
				else
				{
					vector.x = 0f;
				}
				if (vector.y > zero.y * (float)upStepsFired)
				{
					vector.y = 1f;
					upStepsFired++;
					if (currentScrollbar != null || currentSlider != null)
					{
						vector.y = scrollSpeedMultiplier;
						upStepsFired += scrollSpeedMultiplier - 1;
					}
				}
				else if (vector.y < (0f - zero.y) * (float)downStepsFired)
				{
					vector.y = -1f;
					downStepsFired++;
					if (currentScrollbar != null || currentSlider != null)
					{
						vector.y = -scrollSpeedMultiplier;
						downStepsFired += scrollSpeedMultiplier - 1;
					}
				}
				else
				{
					vector.y = 0f;
				}
			}
			if (currentScrollbar != null)
			{
				if (currentScrollbar.direction == Scrollbar.Direction.LeftToRight)
				{
					currentScrollbar.value += scrollAmount * vector.x;
					vector.x = 0f;
				}
				else if (currentScrollbar.direction == Scrollbar.Direction.RightToLeft)
				{
					currentScrollbar.value += (0f - scrollAmount) * vector.x;
					vector.x = 0f;
				}
				else if (currentScrollbar.direction == Scrollbar.Direction.TopToBottom)
				{
					currentScrollbar.value += (0f - scrollAmount) * vector.y;
					vector.y = 0f;
				}
				else if (currentScrollbar.direction == Scrollbar.Direction.BottomToTop)
				{
					currentScrollbar.value += scrollAmount * vector.y;
					vector.y = 0f;
				}
			}
			if (currentSlider != null)
			{
				if (currentSlider.direction == Slider.Direction.LeftToRight)
				{
					currentSlider.value += scrollAmount * vector.x;
					vector.x = 0f;
				}
				else if (currentSlider.direction == Slider.Direction.RightToLeft)
				{
					currentSlider.value += (0f - scrollAmount) * vector.x;
					vector.x = 0f;
				}
				else if (currentSlider.direction == Slider.Direction.TopToBottom)
				{
					currentSlider.value += (0f - scrollAmount) * vector.y;
					vector.y = 0f;
				}
				else if (currentSlider.direction == Slider.Direction.BottomToTop)
				{
					currentSlider.value += scrollAmount * vector.y;
					vector.y = 0f;
				}
			}
			AxisEventData axisEventData = GetAxisEventData(vector.x, vector.y, 0.6f);
			if (!Mathf.Approximately(axisEventData.moveVector.x, 0f) || !Mathf.Approximately(axisEventData.moveVector.y, 0f))
			{
				ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler);
			}
			return axisEventData.used;
		}

		private bool SendMoveEventToSelectedObject()
		{
			if ((DateTime.UtcNow - lastEvent).TotalSeconds < (double)repeatEventRate)
			{
				return false;
			}
			Vector2 vector = Vector2.zero;
			if (vector == Vector2.zero)
			{
				for (int i = 0; i < 2; i++)
				{
					vector = leftStick[i];
					if (vector != Vector2.zero)
					{
						break;
					}
				}
			}
			if (vector == Vector2.zero)
			{
				for (int j = 0; j < 2; j++)
				{
					vector = dpad[j];
					if (vector != Vector2.zero)
					{
						break;
					}
				}
			}
			AxisEventData axisEventData = GetAxisEventData(vector.x, vector.y, 0.6f);
			if (!Mathf.Approximately(axisEventData.moveVector.x, 0f) || !Mathf.Approximately(axisEventData.moveVector.y, 0f))
			{
				lastEvent = DateTime.UtcNow;
				ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler);
			}
			return axisEventData.used;
		}

		private bool SendUpdateEventToSelectedObject()
		{
			if (eventSystem.currentSelectedGameObject == null)
			{
				return false;
			}
			BaseEventData baseEventData = GetBaseEventData();
			ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.updateSelectedHandler);
			return baseEventData.used;
		}

		private void localClickStart(ButtonClick button)
		{
			if (buttonMode == EasyInputConstants.INPUT_MODULE_BUTTON_MODE.FireAtRepeatRate || buttonMode == EasyInputConstants.INPUT_MODULE_BUTTON_MODE.FireOnceAtButtonDown)
			{
				if (button.button == EasyInputConstants.CONTROLLER_BUTTON.AButton)
				{
					submit = true;
				}
				if (button.button == EasyInputConstants.CONTROLLER_BUTTON.BButton || button.button == EasyInputConstants.CONTROLLER_BUTTON.XButton)
				{
					cancel = true;
				}
			}
			if (buttonMode == EasyInputConstants.INPUT_MODULE_BUTTON_MODE.FireOnceAtButtonDown)
			{
				if (button.button == EasyInputConstants.CONTROLLER_BUTTON.AButton)
				{
					submitForOneFrame = true;
				}
				if (button.button == EasyInputConstants.CONTROLLER_BUTTON.BButton || button.button == EasyInputConstants.CONTROLLER_BUTTON.XButton)
				{
					cancelForOneFrame = true;
				}
			}
		}

		private void localClickEnd(ButtonClick button)
		{
			if (buttonMode == EasyInputConstants.INPUT_MODULE_BUTTON_MODE.FireAtRepeatRate || buttonMode == EasyInputConstants.INPUT_MODULE_BUTTON_MODE.FireOnceAtButtonDown)
			{
				if (button.button == EasyInputConstants.CONTROLLER_BUTTON.AButton)
				{
					submit = false;
				}
				if (button.button == EasyInputConstants.CONTROLLER_BUTTON.BButton || button.button == EasyInputConstants.CONTROLLER_BUTTON.XButton)
				{
					cancel = false;
				}
				return;
			}
			if (button.button == EasyInputConstants.CONTROLLER_BUTTON.AButton)
			{
				submit = true;
				submitForOneFrame = true;
			}
			if (button.button == EasyInputConstants.CONTROLLER_BUTTON.BButton || button.button == EasyInputConstants.CONTROLLER_BUTTON.XButton)
			{
				cancel = true;
				cancelForOneFrame = true;
			}
		}

		private void localLeftStick(ControllerAxis axis)
		{
			leftStick[(int)axis.player] = axis.axisValue;
		}

		private void localDpad(ControllerAxis axis)
		{
			dpad[(int)axis.player] = axis.axisValue;
		}

		private void localTouch(InputTouch touch)
		{
			touchpad = touch;
		}

		private void localTouchEnd(InputTouch touch)
		{
			touchpad.currentTouchPosition = EasyInputConstants.NOT_TOUCHING;
			leftStepsFired = 1;
			rightStepsFired = 1;
			upStepsFired = 1;
			downStepsFired = 1;
		}
	}
}

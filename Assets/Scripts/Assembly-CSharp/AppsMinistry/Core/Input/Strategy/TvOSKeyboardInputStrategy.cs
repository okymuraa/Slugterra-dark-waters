using System;
using EasyInput.Core;
using UnityEngine;

namespace AppsMinistry.Core.Input.Strategy
{
	public class TvOSKeyboardInputStrategy : TouchInputStrategy
	{
		public EasyInputConstants.ACTION_TYPE action = EasyInputConstants.ACTION_TYPE.Rotation;

		public EasyInputConstants.INPUT_MODULE_BUTTON_MODE buttonMode;

		public float deadZone = 0.2f;

		public EasyInputConstants.AXIS axisHorizontal;

		public EasyInputConstants.AXIS axisVertical = EasyInputConstants.AXIS.YAxis;

		public float sensitivity = 0.01f;

		private float onAccelerometerX;

		private float onAccelerometerY;

		private float onAccelerometerZ;

		private float horizontal;

		private float vertical;

		private bool clickStartAButton;

		private bool clickSkipMission;

		private bool swipeDetected;

		private bool swipeCharacter;

		private bool _longClickStart;

		private Vector2 storedTouch = Vector2.zero;

		private bool tutorSkipping;

		private bool showHelp;

		public override void Activate()
		{
			base.Activate();
			EasyInputHelper.On_ClickStart += onClickStart;
			EasyInputHelper.On_ClickEnd += onClickEnd;
			EasyInputHelper.On_LongClickStart += onLongClickStart;
			EasyInputHelper.On_QuickClickEnd += onQuickClickEnd;
			EasyInputHelper.On_DoubleClickEnd += onDoubleClickEnd;
			EasyInputHelper.On_TouchStart += onTouchStart;
			EasyInputHelper.On_Touch += onTouch;
			EasyInputHelper.On_TouchEnd += onTouchEnd;
			EasyInputHelper.On_QuickTouchEnd += onQuickTouchEnd;
			EasyInputHelper.On_LongTouchStart += onLongTouchStart;
			EasyInputHelper.On_LongTouch += onLongTouch;
			EasyInputHelper.On_LongTouchEnd += onLongTouchEnd;
			EasyInputHelper.On_DoubleTouchEnd += onDoubleTouchEnd;
			EasyInputHelper.On_SwipeDetected += onSwipeDetected;
			EasyInputHelper.On_LeftStick += onLeftStick;
			EasyInputHelper.On_RightStick += onRightStick;
			EasyInputHelper.On_Dpad += onDpad;
			EasyInputHelper.On_LeftTrigger += onLeftTrigger;
			EasyInputHelper.On_RightTrigger += onRightTrigger;
			EasyInputHelper.On_Accelerometer += onAccelerometer;
		}

		public override void Deactivate()
		{
			base.Deactivate();
			EasyInputHelper.On_ClickStart -= onClickStart;
			EasyInputHelper.On_ClickEnd -= onClickEnd;
			EasyInputHelper.On_LongClickStart -= onLongClickStart;
			EasyInputHelper.On_QuickClickEnd -= onQuickClickEnd;
			EasyInputHelper.On_DoubleClickEnd -= onDoubleClickEnd;
			EasyInputHelper.On_TouchStart -= onTouchStart;
			EasyInputHelper.On_Touch -= onTouch;
			EasyInputHelper.On_TouchEnd -= onTouchEnd;
			EasyInputHelper.On_QuickTouchEnd -= onQuickTouchEnd;
			EasyInputHelper.On_LongTouchStart -= onLongTouchStart;
			EasyInputHelper.On_LongTouch -= onLongTouch;
			EasyInputHelper.On_LongTouchEnd -= onLongTouchEnd;
			EasyInputHelper.On_DoubleTouchEnd -= onDoubleTouchEnd;
			EasyInputHelper.On_SwipeDetected -= onSwipeDetected;
			EasyInputHelper.On_LeftStick -= onLeftStick;
			EasyInputHelper.On_RightStick -= onRightStick;
			EasyInputHelper.On_Dpad -= onDpad;
			EasyInputHelper.On_LeftTrigger -= onLeftTrigger;
			EasyInputHelper.On_RightTrigger -= onRightTrigger;
			EasyInputHelper.On_Accelerometer -= onAccelerometer;
		}

		public override void Update()
		{
			base.Update();
		}

		private void onQuickClickEnd(ButtonClick button)
		{
		}

		private void onDoubleClickEnd(ButtonClick button)
		{
			float x = storedTouch.x;
			float y = storedTouch.y;
			if (button.button == EasyInputConstants.CONTROLLER_BUTTON.MenuButton)
			{
				if (x > 0.5f && y > 0.5f)
				{
					clickSkipMission = true;
				}
				else if (x < -0.5f && y < -0.5f && clickSkipMission)
				{
					OnSkipMission();
					clickSkipMission = false;
				}
			}
		}

		private void onTouchStart(InputTouch touch)
		{
			storedTouch = touch.currentTouchPosition;
			if (!clickStartAButton && !swipeCharacter)
			{
				Use_OnMoveEvent(storedTouch);
			}
		}

		private void onTouch(InputTouch touch)
		{
			storedTouch = touch.currentTouchPosition;
			if (!swipeCharacter)
			{
				if (clickStartAButton)
				{
					Use_OnMoveEndEvent();
				}
				else
				{
					Use_OnMoveEvent(storedTouch);
				}
			}
		}

		private void onTouchEnd(InputTouch touch)
		{
			if (!swipeCharacter)
			{
				Use_OnMoveEndEvent();
				float x = touch.currentTouchPosition.x;
				float y = touch.currentTouchPosition.y;
				if (x > 0.5f && y < 0.5f && y > -0.5f)
				{
					OnUsePreviousPowerUp();
					return;
				}
				if (x < -0.5f && y < 0.5f && y > -0.5f)
				{
					OnUseNextPowerUp();
					return;
				}
				if (x < 0.5f && x > -0.5f && y > 0.5f)
				{
					OnUseNextSlug();
					return;
				}
				if (x < 0.5f && x > -0.5f && y < -0.5f)
				{
					OnUsePreviousSlug();
					return;
				}
			}
			swipeDetected = false;
			swipeCharacter = false;
		}

		private void onQuickTouchEnd(InputTouch touch)
		{
		}

		private void onLongTouchStart(InputTouch touch)
		{
		}

		private void onLongTouch(InputTouch touch)
		{
		}

		private void onLongTouchEnd(InputTouch touch)
		{
		}

		private void onDoubleTouchEnd(InputTouch touch)
		{
		}

		private void onSwipeDetected(InputTouch touch)
		{
			EasyInputConstants.SWIPE_TYPE swipeType = touch.swipeType;
			InputEvent inputEvent = InputEvent.None;
			if (touch.swipeType == EasyInputConstants.SWIPE_TYPE.Up)
			{
				inputEvent = InputEvent.Up;
			}
			if (touch.swipeType == EasyInputConstants.SWIPE_TYPE.Down)
			{
				inputEvent = InputEvent.Down;
			}
			if (touch.swipeType == EasyInputConstants.SWIPE_TYPE.Right)
			{
				inputEvent = InputEvent.Right;
			}
			if (touch.swipeType == EasyInputConstants.SWIPE_TYPE.Left)
			{
				inputEvent = InputEvent.Left;
			}
			if (inputEvent != InputEvent.None)
			{
				Use_OnButtonUp(inputEvent);
			}
			swipeDetected = true;
		}

		private void onClickStart(ButtonClick button)
		{
			float x = storedTouch.x;
			float y = storedTouch.y;
			if (button.button == EasyInputConstants.CONTROLLER_BUTTON.AButton)
			{
				clickStartAButton = true;
				if (x < 0.5f && x > -0.5f && y < 0.5f && y > -0.5f)
				{
					Use_OnButtonUp(InputEvent.Enter);
				}
				else if (x > 0.5f)
				{
					swipeCharacter = true;
				}
				else if (x < -0.5f && y < 0.5f && y > -0.5f)
				{
					swipeCharacter = true;
				}
				else if (y > 0.5f)
				{
					swipeCharacter = true;
				}
				else if (y < -0.5f)
				{
					swipeCharacter = true;
				}
				if (swipeCharacter)
				{
					Use_OnMoveEndEvent();
					Use_OnSwipeClick(storedTouch);
				}
				else if (x < -0.5f && y > 0.5f && !tutorSkipping)
				{
					OnShowHelpForController();
					showHelp = true;
					tutorSkipping = true;
				}
			}
			else if (button.button == EasyInputConstants.CONTROLLER_BUTTON.XButton)
			{
				Use_OnButtonUp(InputEvent.XButton);
			}
			else if (button.button == EasyInputConstants.CONTROLLER_BUTTON.MenuButton)
			{
				tutorSkipping = !tutorSkipping;
				showHelp = false;
				OnBackBtn();
			}
		}

		private void onClickEnd(ButtonClick button)
		{
			float x = storedTouch.x;
			float y = storedTouch.y;
			if (button.button == EasyInputConstants.CONTROLLER_BUTTON.AButton)
			{
				clickStartAButton = false;
				if (x < 0.5f && x > -0.5f && y < 0.5f && y > -0.5f)
				{
					if (_longClickStart)
					{
						_longClickStart = false;
					}
					else
					{
						OnUsePowerUp();
					}
				}
			}
			else if (button.button == EasyInputConstants.CONTROLLER_BUTTON.XButton)
			{
				if (_longClickStart)
				{
					_longClickStart = false;
				}
				else
				{
					Use_OnBegineAttack();
				}
			}
			else if (button.button != EasyInputConstants.CONTROLLER_BUTTON.MenuButton)
			{
			}
		}

		private void onLongClickStart(ButtonClick button)
		{
			float x = storedTouch.x;
			float y = storedTouch.y;
			if (button.button == EasyInputConstants.CONTROLLER_BUTTON.AButton && x < 0.5f && x > -0.5f && y < 0.5f && y > -0.5f)
			{
				OnOpenMinimap();
			}
			if (button.button == EasyInputConstants.CONTROLLER_BUTTON.XButton)
			{
				_longClickStart = true;
				OnUseSlug();
			}
		}

		private void onLeftStick(ControllerAxis axis)
		{
		}

		private void onRightStick(ControllerAxis axis)
		{
		}

		private void onLeftTrigger(ControllerAxis axis)
		{
		}

		private void onRightTrigger(ControllerAxis axis)
		{
		}

		private void onDpad(ControllerAxis axis)
		{
		}

		private void onAccelerometer(Vector3 accel)
		{
			if ((double)accel.x > 0.05 || (double)accel.x < -0.05)
			{
				Use_OnAccelerometerX(accel.x);
			}
			if ((double)Math.Abs(accel.x - onAccelerometerX) > 0.2)
			{
				clickSkipMission = false;
				onAccelerometerX = accel.x;
				if ((double)accel.x > 0.3)
				{
					Use_OnPrevTargetBtnEvent();
					Use_OnAccelMoveRight();
				}
				else if ((double)accel.x < -0.3)
				{
					Use_OnNextTargetBtnEvent();
					Use_OnAccelMoveLeft();
				}
				else if (!((double)accel.y > 0.3) && !((double)accel.y < -0.3))
				{
				}
			}
			if ((double)Math.Abs(accel.y - onAccelerometerY) > 0.2)
			{
				onAccelerometerY = accel.y;
				if (!((double)accel.y > 0.3) && !((double)accel.y < -0.3))
				{
				}
			}
			if ((double)Math.Abs(accel.z - onAccelerometerZ) > 0.2)
			{
				onAccelerometerZ = accel.z;
			}
		}

		private bool GetKeyUp(KeyCode key)
		{
			return UnityEngine.Input.GetKeyUp(key);
		}

		private bool GetKeyDown(KeyCode key)
		{
			return UnityEngine.Input.GetKeyDown(key);
		}

		private bool GetKeyPress(KeyCode key)
		{
			return UnityEngine.Input.GetKey(key);
		}
	}
}

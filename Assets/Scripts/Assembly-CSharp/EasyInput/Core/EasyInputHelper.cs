using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

namespace EasyInput.Core
{
	[AddComponentMenu("EasyInputHelper/Manager/EasyInputHelper")]
	public class EasyInputHelper : MonoBehaviour
	{
		public delegate void onTouchStartHandler(InputTouch touch);

		public delegate void onTouchHandler(InputTouch touch);

		public delegate void onTouchEndHandler(InputTouch touch);

		public delegate void quickTouchEndHandler(InputTouch touch);

		public delegate void longTouchStartHandler(InputTouch touch);

		public delegate void longTouchHandler(InputTouch touch);

		public delegate void longTouchEndHandler(InputTouch touch);

		public delegate void doubleTouchEndHandler(InputTouch touch);

		public delegate void swipeDistanceHandler(InputTouch touch);

		public delegate void onClickStartHandler(ButtonClick button);

		public delegate void onClickHandler(ButtonClick button);

		public delegate void onClickEndHandler(ButtonClick button);

		public delegate void quickClickEndHandler(ButtonClick button);

		public delegate void longClickStartHandler(ButtonClick button);

		public delegate void longClickHandler(ButtonClick button);

		public delegate void longClickEndHandler(ButtonClick button);

		public delegate void doubleClickEndHandler(ButtonClick button);

		public delegate void onAxisHandler(ControllerAxis axis);

		public delegate void AccelerometerHandler(Vector3 acceleration);

		public delegate void GyroHandler(Vector3 gravity, Vector3 userAcceleration);

		public delegate void MotionHandler(Motion motion);

		[Tooltip("The maximum distance of movement to be considered for the quickpress, longpress, and doublepress")]
		public float maxPressLength = 0.1f;

		[Tooltip("The distance of movement to fire a swipe event. Only one event will be fired when you reach this distance")]
		public float requiredSwipeLength = 0.6f;

		[Tooltip("The longest you can hold in seconds and still be considered a quick press")]
		public float maxQuickTapTime = 0.2f;

		[Tooltip("The shortest amount of time in seconds that will be considered a long press")]
		public float minLongTapTime = 1f;

		[Tooltip("The longest double tap in seconds and still be considered a double press")]
		public float maxDoubleTapTime = 0.5f;

		[Tooltip("If its the title screen we'll allow exit to home. If not we can handle the menu button in game. This only works on physical Apple TV")]
		public bool titleScreen;

		public EasyInputConstants.REMOTE_ORIENTATION orientation;

		[HideInInspector]
		public static EasyInputConstants.PLAYER_NUMBER siriRemotePlayer;

		public static EasyInputHelper mInstance;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static onTouchStartHandler On_TouchStart__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static onTouchHandler On_Touch__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static onTouchEndHandler On_TouchEnd__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static quickTouchEndHandler On_QuickTouchEnd__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static longTouchStartHandler On_LongTouchStart__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static longTouchHandler On_LongTouch__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static longTouchEndHandler On_LongTouchEnd__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static doubleTouchEndHandler On_DoubleTouchEnd__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static swipeDistanceHandler On_SwipeDetected__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static onClickStartHandler On_ClickStart__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static onClickHandler On_Click__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static onClickEndHandler On_ClickEnd__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static quickClickEndHandler On_QuickClickEnd__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static longClickStartHandler On_LongClickStart__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static longClickHandler On_LongClick__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static longClickEndHandler On_LongClickEnd__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static doubleClickEndHandler On_DoubleClickEnd__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static onAxisHandler On_LeftStick__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static onAxisHandler On_RightStick__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static onAxisHandler On_Dpad__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static onAxisHandler On_LeftTrigger__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static onAxisHandler On_RightTrigger__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static AccelerometerHandler On_Accelerometer__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static GyroHandler On_Gyro__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static MotionHandler On_Motion__BackingField;

		private Touch touch;

		private bool swipeAllowed;

		private bool currentlyTouching;

		private bool previousTouching;

		private string touchAxisHorizontal = string.Empty;

		private string touchAxisVertical = string.Empty;

		private Vector3 temp;

		private Vector3 temp1;

		private bool startupLock;

		private DateTime startup;

		private bool exitHomeLock;

		private int framesWithinThresholdX = 1;

		private int framesWithinThresholdY = 1;

		private int framesWithinThresholdZ = 1;

		private int framesVelocitySameX = 1;

		private int framesVelocitySameY = 1;

		private int framesVelocitySameZ = 1;

		private InputTouch myTouch;

		private Motion myMotion;

		private ControllerAxis[] axisArray;

		private ButtonClick[] buttonArray;

		public static event onTouchStartHandler On_TouchStart
		{
			add
			{
				onTouchStartHandler onTouchStartHandler2 = On_TouchStart__BackingField;
				onTouchStartHandler onTouchStartHandler3;
				do
				{
					onTouchStartHandler3 = onTouchStartHandler2;
					onTouchStartHandler2 = Interlocked.CompareExchange(ref On_TouchStart__BackingField, (onTouchStartHandler)Delegate.Combine(onTouchStartHandler3, value), onTouchStartHandler2);
				}
				while ((object)onTouchStartHandler2 != onTouchStartHandler3);
			}
			remove
			{
				onTouchStartHandler onTouchStartHandler2 = On_TouchStart__BackingField;
				onTouchStartHandler onTouchStartHandler3;
				do
				{
					onTouchStartHandler3 = onTouchStartHandler2;
					onTouchStartHandler2 = Interlocked.CompareExchange(ref On_TouchStart__BackingField, (onTouchStartHandler)Delegate.Remove(onTouchStartHandler3, value), onTouchStartHandler2);
				}
				while ((object)onTouchStartHandler2 != onTouchStartHandler3);
			}
		}

		public static event onTouchHandler On_Touch
		{
			add
			{
				onTouchHandler onTouchHandler2 = On_Touch__BackingField;
				onTouchHandler onTouchHandler3;
				do
				{
					onTouchHandler3 = onTouchHandler2;
					onTouchHandler2 = Interlocked.CompareExchange(ref On_Touch__BackingField, (onTouchHandler)Delegate.Combine(onTouchHandler3, value), onTouchHandler2);
				}
				while ((object)onTouchHandler2 != onTouchHandler3);
			}
			remove
			{
				onTouchHandler onTouchHandler2 = On_Touch__BackingField;
				onTouchHandler onTouchHandler3;
				do
				{
					onTouchHandler3 = onTouchHandler2;
					onTouchHandler2 = Interlocked.CompareExchange(ref On_Touch__BackingField, (onTouchHandler)Delegate.Remove(onTouchHandler3, value), onTouchHandler2);
				}
				while ((object)onTouchHandler2 != onTouchHandler3);
			}
		}

		public static event onTouchEndHandler On_TouchEnd
		{
			add
			{
				onTouchEndHandler onTouchEndHandler2 = On_TouchEnd__BackingField;
				onTouchEndHandler onTouchEndHandler3;
				do
				{
					onTouchEndHandler3 = onTouchEndHandler2;
					onTouchEndHandler2 = Interlocked.CompareExchange(ref On_TouchEnd__BackingField, (onTouchEndHandler)Delegate.Combine(onTouchEndHandler3, value), onTouchEndHandler2);
				}
				while ((object)onTouchEndHandler2 != onTouchEndHandler3);
			}
			remove
			{
				onTouchEndHandler onTouchEndHandler2 = On_TouchEnd__BackingField;
				onTouchEndHandler onTouchEndHandler3;
				do
				{
					onTouchEndHandler3 = onTouchEndHandler2;
					onTouchEndHandler2 = Interlocked.CompareExchange(ref On_TouchEnd__BackingField, (onTouchEndHandler)Delegate.Remove(onTouchEndHandler3, value), onTouchEndHandler2);
				}
				while ((object)onTouchEndHandler2 != onTouchEndHandler3);
			}
		}

		public static event quickTouchEndHandler On_QuickTouchEnd
		{
			add
			{
				quickTouchEndHandler quickTouchEndHandler2 = On_QuickTouchEnd__BackingField;
				quickTouchEndHandler quickTouchEndHandler3;
				do
				{
					quickTouchEndHandler3 = quickTouchEndHandler2;
					quickTouchEndHandler2 = Interlocked.CompareExchange(ref On_QuickTouchEnd__BackingField, (quickTouchEndHandler)Delegate.Combine(quickTouchEndHandler3, value), quickTouchEndHandler2);
				}
				while ((object)quickTouchEndHandler2 != quickTouchEndHandler3);
			}
			remove
			{
				quickTouchEndHandler quickTouchEndHandler2 = On_QuickTouchEnd__BackingField;
				quickTouchEndHandler quickTouchEndHandler3;
				do
				{
					quickTouchEndHandler3 = quickTouchEndHandler2;
					quickTouchEndHandler2 = Interlocked.CompareExchange(ref On_QuickTouchEnd__BackingField, (quickTouchEndHandler)Delegate.Remove(quickTouchEndHandler3, value), quickTouchEndHandler2);
				}
				while ((object)quickTouchEndHandler2 != quickTouchEndHandler3);
			}
		}

		public static event longTouchStartHandler On_LongTouchStart
		{
			add
			{
				longTouchStartHandler longTouchStartHandler2 = On_LongTouchStart__BackingField;
				longTouchStartHandler longTouchStartHandler3;
				do
				{
					longTouchStartHandler3 = longTouchStartHandler2;
					longTouchStartHandler2 = Interlocked.CompareExchange(ref On_LongTouchStart__BackingField, (longTouchStartHandler)Delegate.Combine(longTouchStartHandler3, value), longTouchStartHandler2);
				}
				while ((object)longTouchStartHandler2 != longTouchStartHandler3);
			}
			remove
			{
				longTouchStartHandler longTouchStartHandler2 = On_LongTouchStart__BackingField;
				longTouchStartHandler longTouchStartHandler3;
				do
				{
					longTouchStartHandler3 = longTouchStartHandler2;
					longTouchStartHandler2 = Interlocked.CompareExchange(ref On_LongTouchStart__BackingField, (longTouchStartHandler)Delegate.Remove(longTouchStartHandler3, value), longTouchStartHandler2);
				}
				while ((object)longTouchStartHandler2 != longTouchStartHandler3);
			}
		}

		public static event longTouchHandler On_LongTouch
		{
			add
			{
				longTouchHandler longTouchHandler2 = On_LongTouch__BackingField;
				longTouchHandler longTouchHandler3;
				do
				{
					longTouchHandler3 = longTouchHandler2;
					longTouchHandler2 = Interlocked.CompareExchange(ref On_LongTouch__BackingField, (longTouchHandler)Delegate.Combine(longTouchHandler3, value), longTouchHandler2);
				}
				while ((object)longTouchHandler2 != longTouchHandler3);
			}
			remove
			{
				longTouchHandler longTouchHandler2 = On_LongTouch__BackingField;
				longTouchHandler longTouchHandler3;
				do
				{
					longTouchHandler3 = longTouchHandler2;
					longTouchHandler2 = Interlocked.CompareExchange(ref On_LongTouch__BackingField, (longTouchHandler)Delegate.Remove(longTouchHandler3, value), longTouchHandler2);
				}
				while ((object)longTouchHandler2 != longTouchHandler3);
			}
		}

		public static event longTouchEndHandler On_LongTouchEnd
		{
			add
			{
				longTouchEndHandler longTouchEndHandler2 = On_LongTouchEnd__BackingField;
				longTouchEndHandler longTouchEndHandler3;
				do
				{
					longTouchEndHandler3 = longTouchEndHandler2;
					longTouchEndHandler2 = Interlocked.CompareExchange(ref On_LongTouchEnd__BackingField, (longTouchEndHandler)Delegate.Combine(longTouchEndHandler3, value), longTouchEndHandler2);
				}
				while ((object)longTouchEndHandler2 != longTouchEndHandler3);
			}
			remove
			{
				longTouchEndHandler longTouchEndHandler2 = On_LongTouchEnd__BackingField;
				longTouchEndHandler longTouchEndHandler3;
				do
				{
					longTouchEndHandler3 = longTouchEndHandler2;
					longTouchEndHandler2 = Interlocked.CompareExchange(ref On_LongTouchEnd__BackingField, (longTouchEndHandler)Delegate.Remove(longTouchEndHandler3, value), longTouchEndHandler2);
				}
				while ((object)longTouchEndHandler2 != longTouchEndHandler3);
			}
		}

		public static event doubleTouchEndHandler On_DoubleTouchEnd
		{
			add
			{
				doubleTouchEndHandler doubleTouchEndHandler2 = On_DoubleTouchEnd__BackingField;
				doubleTouchEndHandler doubleTouchEndHandler3;
				do
				{
					doubleTouchEndHandler3 = doubleTouchEndHandler2;
					doubleTouchEndHandler2 = Interlocked.CompareExchange(ref On_DoubleTouchEnd__BackingField, (doubleTouchEndHandler)Delegate.Combine(doubleTouchEndHandler3, value), doubleTouchEndHandler2);
				}
				while ((object)doubleTouchEndHandler2 != doubleTouchEndHandler3);
			}
			remove
			{
				doubleTouchEndHandler doubleTouchEndHandler2 = On_DoubleTouchEnd__BackingField;
				doubleTouchEndHandler doubleTouchEndHandler3;
				do
				{
					doubleTouchEndHandler3 = doubleTouchEndHandler2;
					doubleTouchEndHandler2 = Interlocked.CompareExchange(ref On_DoubleTouchEnd__BackingField, (doubleTouchEndHandler)Delegate.Remove(doubleTouchEndHandler3, value), doubleTouchEndHandler2);
				}
				while ((object)doubleTouchEndHandler2 != doubleTouchEndHandler3);
			}
		}

		public static event swipeDistanceHandler On_SwipeDetected
		{
			add
			{
				swipeDistanceHandler swipeDistanceHandler2 = On_SwipeDetected__BackingField;
				swipeDistanceHandler swipeDistanceHandler3;
				do
				{
					swipeDistanceHandler3 = swipeDistanceHandler2;
					swipeDistanceHandler2 = Interlocked.CompareExchange(ref On_SwipeDetected__BackingField, (swipeDistanceHandler)Delegate.Combine(swipeDistanceHandler3, value), swipeDistanceHandler2);
				}
				while ((object)swipeDistanceHandler2 != swipeDistanceHandler3);
			}
			remove
			{
				swipeDistanceHandler swipeDistanceHandler2 = On_SwipeDetected__BackingField;
				swipeDistanceHandler swipeDistanceHandler3;
				do
				{
					swipeDistanceHandler3 = swipeDistanceHandler2;
					swipeDistanceHandler2 = Interlocked.CompareExchange(ref On_SwipeDetected__BackingField, (swipeDistanceHandler)Delegate.Remove(swipeDistanceHandler3, value), swipeDistanceHandler2);
				}
				while ((object)swipeDistanceHandler2 != swipeDistanceHandler3);
			}
		}

		public static event onClickStartHandler On_ClickStart
		{
			add
			{
				onClickStartHandler onClickStartHandler2 = On_ClickStart__BackingField;
				onClickStartHandler onClickStartHandler3;
				do
				{
					onClickStartHandler3 = onClickStartHandler2;
					onClickStartHandler2 = Interlocked.CompareExchange(ref On_ClickStart__BackingField, (onClickStartHandler)Delegate.Combine(onClickStartHandler3, value), onClickStartHandler2);
				}
				while ((object)onClickStartHandler2 != onClickStartHandler3);
			}
			remove
			{
				onClickStartHandler onClickStartHandler2 = On_ClickStart__BackingField;
				onClickStartHandler onClickStartHandler3;
				do
				{
					onClickStartHandler3 = onClickStartHandler2;
					onClickStartHandler2 = Interlocked.CompareExchange(ref On_ClickStart__BackingField, (onClickStartHandler)Delegate.Remove(onClickStartHandler3, value), onClickStartHandler2);
				}
				while ((object)onClickStartHandler2 != onClickStartHandler3);
			}
		}

		public static event onClickHandler On_Click
		{
			add
			{
				onClickHandler onClickHandler2 = On_Click__BackingField;
				onClickHandler onClickHandler3;
				do
				{
					onClickHandler3 = onClickHandler2;
					onClickHandler2 = Interlocked.CompareExchange(ref On_Click__BackingField, (onClickHandler)Delegate.Combine(onClickHandler3, value), onClickHandler2);
				}
				while ((object)onClickHandler2 != onClickHandler3);
			}
			remove
			{
				onClickHandler onClickHandler2 = On_Click__BackingField;
				onClickHandler onClickHandler3;
				do
				{
					onClickHandler3 = onClickHandler2;
					onClickHandler2 = Interlocked.CompareExchange(ref On_Click__BackingField, (onClickHandler)Delegate.Remove(onClickHandler3, value), onClickHandler2);
				}
				while ((object)onClickHandler2 != onClickHandler3);
			}
		}

		public static event onClickEndHandler On_ClickEnd
		{
			add
			{
				onClickEndHandler onClickEndHandler2 = On_ClickEnd__BackingField;
				onClickEndHandler onClickEndHandler3;
				do
				{
					onClickEndHandler3 = onClickEndHandler2;
					onClickEndHandler2 = Interlocked.CompareExchange(ref On_ClickEnd__BackingField, (onClickEndHandler)Delegate.Combine(onClickEndHandler3, value), onClickEndHandler2);
				}
				while ((object)onClickEndHandler2 != onClickEndHandler3);
			}
			remove
			{
				onClickEndHandler onClickEndHandler2 = On_ClickEnd__BackingField;
				onClickEndHandler onClickEndHandler3;
				do
				{
					onClickEndHandler3 = onClickEndHandler2;
					onClickEndHandler2 = Interlocked.CompareExchange(ref On_ClickEnd__BackingField, (onClickEndHandler)Delegate.Remove(onClickEndHandler3, value), onClickEndHandler2);
				}
				while ((object)onClickEndHandler2 != onClickEndHandler3);
			}
		}

		public static event quickClickEndHandler On_QuickClickEnd
		{
			add
			{
				quickClickEndHandler quickClickEndHandler2 = On_QuickClickEnd__BackingField;
				quickClickEndHandler quickClickEndHandler3;
				do
				{
					quickClickEndHandler3 = quickClickEndHandler2;
					quickClickEndHandler2 = Interlocked.CompareExchange(ref On_QuickClickEnd__BackingField, (quickClickEndHandler)Delegate.Combine(quickClickEndHandler3, value), quickClickEndHandler2);
				}
				while ((object)quickClickEndHandler2 != quickClickEndHandler3);
			}
			remove
			{
				quickClickEndHandler quickClickEndHandler2 = On_QuickClickEnd__BackingField;
				quickClickEndHandler quickClickEndHandler3;
				do
				{
					quickClickEndHandler3 = quickClickEndHandler2;
					quickClickEndHandler2 = Interlocked.CompareExchange(ref On_QuickClickEnd__BackingField, (quickClickEndHandler)Delegate.Remove(quickClickEndHandler3, value), quickClickEndHandler2);
				}
				while ((object)quickClickEndHandler2 != quickClickEndHandler3);
			}
		}

		public static event longClickStartHandler On_LongClickStart
		{
			add
			{
				longClickStartHandler longClickStartHandler2 = On_LongClickStart__BackingField;
				longClickStartHandler longClickStartHandler3;
				do
				{
					longClickStartHandler3 = longClickStartHandler2;
					longClickStartHandler2 = Interlocked.CompareExchange(ref On_LongClickStart__BackingField, (longClickStartHandler)Delegate.Combine(longClickStartHandler3, value), longClickStartHandler2);
				}
				while ((object)longClickStartHandler2 != longClickStartHandler3);
			}
			remove
			{
				longClickStartHandler longClickStartHandler2 = On_LongClickStart__BackingField;
				longClickStartHandler longClickStartHandler3;
				do
				{
					longClickStartHandler3 = longClickStartHandler2;
					longClickStartHandler2 = Interlocked.CompareExchange(ref On_LongClickStart__BackingField, (longClickStartHandler)Delegate.Remove(longClickStartHandler3, value), longClickStartHandler2);
				}
				while ((object)longClickStartHandler2 != longClickStartHandler3);
			}
		}

		public static event longClickHandler On_LongClick
		{
			add
			{
				longClickHandler longClickHandler2 = On_LongClick__BackingField;
				longClickHandler longClickHandler3;
				do
				{
					longClickHandler3 = longClickHandler2;
					longClickHandler2 = Interlocked.CompareExchange(ref On_LongClick__BackingField, (longClickHandler)Delegate.Combine(longClickHandler3, value), longClickHandler2);
				}
				while ((object)longClickHandler2 != longClickHandler3);
			}
			remove
			{
				longClickHandler longClickHandler2 = On_LongClick__BackingField;
				longClickHandler longClickHandler3;
				do
				{
					longClickHandler3 = longClickHandler2;
					longClickHandler2 = Interlocked.CompareExchange(ref On_LongClick__BackingField, (longClickHandler)Delegate.Remove(longClickHandler3, value), longClickHandler2);
				}
				while ((object)longClickHandler2 != longClickHandler3);
			}
		}

		public static event longClickEndHandler On_LongClickEnd
		{
			add
			{
				longClickEndHandler longClickEndHandler2 = On_LongClickEnd__BackingField;
				longClickEndHandler longClickEndHandler3;
				do
				{
					longClickEndHandler3 = longClickEndHandler2;
					longClickEndHandler2 = Interlocked.CompareExchange(ref On_LongClickEnd__BackingField, (longClickEndHandler)Delegate.Combine(longClickEndHandler3, value), longClickEndHandler2);
				}
				while ((object)longClickEndHandler2 != longClickEndHandler3);
			}
			remove
			{
				longClickEndHandler longClickEndHandler2 = On_LongClickEnd__BackingField;
				longClickEndHandler longClickEndHandler3;
				do
				{
					longClickEndHandler3 = longClickEndHandler2;
					longClickEndHandler2 = Interlocked.CompareExchange(ref On_LongClickEnd__BackingField, (longClickEndHandler)Delegate.Remove(longClickEndHandler3, value), longClickEndHandler2);
				}
				while ((object)longClickEndHandler2 != longClickEndHandler3);
			}
		}

		public static event doubleClickEndHandler On_DoubleClickEnd
		{
			add
			{
				doubleClickEndHandler doubleClickEndHandler2 = On_DoubleClickEnd__BackingField;
				doubleClickEndHandler doubleClickEndHandler3;
				do
				{
					doubleClickEndHandler3 = doubleClickEndHandler2;
					doubleClickEndHandler2 = Interlocked.CompareExchange(ref On_DoubleClickEnd__BackingField, (doubleClickEndHandler)Delegate.Combine(doubleClickEndHandler3, value), doubleClickEndHandler2);
				}
				while ((object)doubleClickEndHandler2 != doubleClickEndHandler3);
			}
			remove
			{
				doubleClickEndHandler doubleClickEndHandler2 = On_DoubleClickEnd__BackingField;
				doubleClickEndHandler doubleClickEndHandler3;
				do
				{
					doubleClickEndHandler3 = doubleClickEndHandler2;
					doubleClickEndHandler2 = Interlocked.CompareExchange(ref On_DoubleClickEnd__BackingField, (doubleClickEndHandler)Delegate.Remove(doubleClickEndHandler3, value), doubleClickEndHandler2);
				}
				while ((object)doubleClickEndHandler2 != doubleClickEndHandler3);
			}
		}

		public static event onAxisHandler On_LeftStick
		{
			add
			{
				onAxisHandler onAxisHandler2 = On_LeftStick__BackingField;
				onAxisHandler onAxisHandler3;
				do
				{
					onAxisHandler3 = onAxisHandler2;
					onAxisHandler2 = Interlocked.CompareExchange(ref On_LeftStick__BackingField, (onAxisHandler)Delegate.Combine(onAxisHandler3, value), onAxisHandler2);
				}
				while ((object)onAxisHandler2 != onAxisHandler3);
			}
			remove
			{
				onAxisHandler onAxisHandler2 = On_LeftStick__BackingField;
				onAxisHandler onAxisHandler3;
				do
				{
					onAxisHandler3 = onAxisHandler2;
					onAxisHandler2 = Interlocked.CompareExchange(ref On_LeftStick__BackingField, (onAxisHandler)Delegate.Remove(onAxisHandler3, value), onAxisHandler2);
				}
				while ((object)onAxisHandler2 != onAxisHandler3);
			}
		}

		public static event onAxisHandler On_RightStick
		{
			add
			{
				onAxisHandler onAxisHandler2 = On_RightStick__BackingField;
				onAxisHandler onAxisHandler3;
				do
				{
					onAxisHandler3 = onAxisHandler2;
					onAxisHandler2 = Interlocked.CompareExchange(ref On_RightStick__BackingField, (onAxisHandler)Delegate.Combine(onAxisHandler3, value), onAxisHandler2);
				}
				while ((object)onAxisHandler2 != onAxisHandler3);
			}
			remove
			{
				onAxisHandler onAxisHandler2 = On_RightStick__BackingField;
				onAxisHandler onAxisHandler3;
				do
				{
					onAxisHandler3 = onAxisHandler2;
					onAxisHandler2 = Interlocked.CompareExchange(ref On_RightStick__BackingField, (onAxisHandler)Delegate.Remove(onAxisHandler3, value), onAxisHandler2);
				}
				while ((object)onAxisHandler2 != onAxisHandler3);
			}
		}

		public static event onAxisHandler On_Dpad
		{
			add
			{
				onAxisHandler onAxisHandler2 = On_Dpad__BackingField;
				onAxisHandler onAxisHandler3;
				do
				{
					onAxisHandler3 = onAxisHandler2;
					onAxisHandler2 = Interlocked.CompareExchange(ref On_Dpad__BackingField, (onAxisHandler)Delegate.Combine(onAxisHandler3, value), onAxisHandler2);
				}
				while ((object)onAxisHandler2 != onAxisHandler3);
			}
			remove
			{
				onAxisHandler onAxisHandler2 = On_Dpad__BackingField;
				onAxisHandler onAxisHandler3;
				do
				{
					onAxisHandler3 = onAxisHandler2;
					onAxisHandler2 = Interlocked.CompareExchange(ref On_Dpad__BackingField, (onAxisHandler)Delegate.Remove(onAxisHandler3, value), onAxisHandler2);
				}
				while ((object)onAxisHandler2 != onAxisHandler3);
			}
		}

		public static event onAxisHandler On_LeftTrigger
		{
			add
			{
				onAxisHandler onAxisHandler2 = On_LeftTrigger__BackingField;
				onAxisHandler onAxisHandler3;
				do
				{
					onAxisHandler3 = onAxisHandler2;
					onAxisHandler2 = Interlocked.CompareExchange(ref On_LeftTrigger__BackingField, (onAxisHandler)Delegate.Combine(onAxisHandler3, value), onAxisHandler2);
				}
				while ((object)onAxisHandler2 != onAxisHandler3);
			}
			remove
			{
				onAxisHandler onAxisHandler2 = On_LeftTrigger__BackingField;
				onAxisHandler onAxisHandler3;
				do
				{
					onAxisHandler3 = onAxisHandler2;
					onAxisHandler2 = Interlocked.CompareExchange(ref On_LeftTrigger__BackingField, (onAxisHandler)Delegate.Remove(onAxisHandler3, value), onAxisHandler2);
				}
				while ((object)onAxisHandler2 != onAxisHandler3);
			}
		}

		public static event onAxisHandler On_RightTrigger
		{
			add
			{
				onAxisHandler onAxisHandler2 = On_RightTrigger__BackingField;
				onAxisHandler onAxisHandler3;
				do
				{
					onAxisHandler3 = onAxisHandler2;
					onAxisHandler2 = Interlocked.CompareExchange(ref On_RightTrigger__BackingField, (onAxisHandler)Delegate.Combine(onAxisHandler3, value), onAxisHandler2);
				}
				while ((object)onAxisHandler2 != onAxisHandler3);
			}
			remove
			{
				onAxisHandler onAxisHandler2 = On_RightTrigger__BackingField;
				onAxisHandler onAxisHandler3;
				do
				{
					onAxisHandler3 = onAxisHandler2;
					onAxisHandler2 = Interlocked.CompareExchange(ref On_RightTrigger__BackingField, (onAxisHandler)Delegate.Remove(onAxisHandler3, value), onAxisHandler2);
				}
				while ((object)onAxisHandler2 != onAxisHandler3);
			}
		}

		public static event AccelerometerHandler On_Accelerometer
		{
			add
			{
				AccelerometerHandler accelerometerHandler = On_Accelerometer__BackingField;
				AccelerometerHandler accelerometerHandler2;
				do
				{
					accelerometerHandler2 = accelerometerHandler;
					accelerometerHandler = Interlocked.CompareExchange(ref On_Accelerometer__BackingField, (AccelerometerHandler)Delegate.Combine(accelerometerHandler2, value), accelerometerHandler);
				}
				while ((object)accelerometerHandler != accelerometerHandler2);
			}
			remove
			{
				AccelerometerHandler accelerometerHandler = On_Accelerometer__BackingField;
				AccelerometerHandler accelerometerHandler2;
				do
				{
					accelerometerHandler2 = accelerometerHandler;
					accelerometerHandler = Interlocked.CompareExchange(ref On_Accelerometer__BackingField, (AccelerometerHandler)Delegate.Remove(accelerometerHandler2, value), accelerometerHandler);
				}
				while ((object)accelerometerHandler != accelerometerHandler2);
			}
		}

		public static event GyroHandler On_Gyro
		{
			add
			{
				GyroHandler gyroHandler = On_Gyro__BackingField;
				GyroHandler gyroHandler2;
				do
				{
					gyroHandler2 = gyroHandler;
					gyroHandler = Interlocked.CompareExchange(ref On_Gyro__BackingField, (GyroHandler)Delegate.Combine(gyroHandler2, value), gyroHandler);
				}
				while ((object)gyroHandler != gyroHandler2);
			}
			remove
			{
				GyroHandler gyroHandler = On_Gyro__BackingField;
				GyroHandler gyroHandler2;
				do
				{
					gyroHandler2 = gyroHandler;
					gyroHandler = Interlocked.CompareExchange(ref On_Gyro__BackingField, (GyroHandler)Delegate.Remove(gyroHandler2, value), gyroHandler);
				}
				while ((object)gyroHandler != gyroHandler2);
			}
		}

		public static event MotionHandler On_Motion
		{
			add
			{
				MotionHandler motionHandler = On_Motion__BackingField;
				MotionHandler motionHandler2;
				do
				{
					motionHandler2 = motionHandler;
					motionHandler = Interlocked.CompareExchange(ref On_Motion__BackingField, (MotionHandler)Delegate.Combine(motionHandler2, value), motionHandler);
				}
				while ((object)motionHandler != motionHandler2);
			}
			remove
			{
				MotionHandler motionHandler = On_Motion__BackingField;
				MotionHandler motionHandler2;
				do
				{
					motionHandler2 = motionHandler;
					motionHandler = Interlocked.CompareExchange(ref On_Motion__BackingField, (MotionHandler)Delegate.Remove(motionHandler2, value), motionHandler);
				}
				while ((object)motionHandler != motionHandler2);
			}
		}

		public static void resetMotion()
		{
			mInstance.myMotion.lastResetTimestamp = Time.time;
			mInstance.myMotion.currentRelativePositionSinceReset = Vector3.zero;
			mInstance.myMotion.totalRotationRateSinceReset = Vector3.zero;
			mInstance.myMotion.totalVelocitySinceReset = Vector3.zero;
			mInstance.myMotion.currentOrientation = EasyInputConstants.NOT_VALID;
			mInstance.myMotion.lastFrameOrientation = EasyInputConstants.NOT_VALID;
			mInstance.myMotion.lastFrameSmoothedGravity = EasyInputConstants.NOT_VALID;
			mInstance.myMotion.lastFrameSmoothedUserAcceleration = EasyInputConstants.NOT_VALID;
			mInstance.myMotion.lastFrameRotationRate = EasyInputConstants.NOT_VALID;
			mInstance.myMotion.currentVelocity = Vector3.zero;
			mInstance.myMotion.lastFrameVelocity = Vector3.zero;
			mInstance.framesWithinThresholdX = 1;
			mInstance.framesWithinThresholdY = 1;
			mInstance.framesWithinThresholdZ = 1;
			mInstance.framesVelocitySameX = 1;
			mInstance.framesVelocitySameY = 1;
			mInstance.framesVelocitySameZ = 1;
		}

		private void Awake()
		{
			mInstance = this;
			axisArray = new ControllerAxis[15];
			buttonArray = new ButtonClick[21];
			myTouch = new InputTouch();
			myMotion = new Motion();
			resetMotion();
			On_TouchStart__BackingField = null;
			On_Touch__BackingField = null;
			On_TouchEnd__BackingField = null;
			On_QuickTouchEnd__BackingField = null;
			On_LongTouchStart__BackingField = null;
			On_LongTouch__BackingField = null;
			On_LongTouchEnd__BackingField = null;
			On_DoubleTouchEnd__BackingField = null;
			On_SwipeDetected__BackingField = null;
			On_ClickStart__BackingField = null;
			On_Click__BackingField = null;
			On_ClickEnd__BackingField = null;
			On_QuickClickEnd__BackingField = null;
			On_LongClickStart__BackingField = null;
			On_LongClick__BackingField = null;
			On_LongClickEnd__BackingField = null;
			On_DoubleClickEnd__BackingField = null;
			On_LeftStick__BackingField = null;
			On_RightStick__BackingField = null;
			On_Dpad__BackingField = null;
			On_LeftTrigger__BackingField = null;
			On_RightTrigger__BackingField = null;
			On_Accelerometer__BackingField = null;
			On_Gyro__BackingField = null;
			On_Motion__BackingField = null;
		}

		private void Start()
		{
			gameObject.SetActive(false);
		}

		private void OnApplicationPause(bool pauseStatus)
		{
		}

		private IEnumerator IndependentUpdate()
		{
			while (true)
			{
				_Update();
				yield return null;
			}
		}

		private void _Update()
		{
			if (exitHomeLock && (DateTime.UtcNow - startup).TotalSeconds > 0.5)
			{
				exitHomeLock = false;
			}
			if (startupLock)
			{
				if ((DateTime.UtcNow - startup).TotalSeconds > 0.10000000149011612)
				{
					startupLock = false;
				}
			}
			else
			{
				populateMyTouch();
				processTouchEvents();
				processControllerButtonEvents();
				processControllerAxisEvents();
				processMotionEvents();
			}
		}

		public static void resetMotion(Vector3 newPosition, Quaternion newOrientation)
		{
		}

		private void populateMyTouch()
		{
		}

		private void processTouchEvents()
		{
			if (currentlyTouching)
			{
				if (!previousTouching)
				{
					myTouch.swipeType = EasyInputConstants.SWIPE_TYPE.None;
					if (myTouch.previousTouchBeginTimestamp != EasyInputConstants.NO_TIMESTAMP && myTouch.currentTouchBeginTimestamp - myTouch.previousTouchBeginTimestamp < maxDoubleTapTime && EasyInputUtilities.determineDistance(myTouch.previousTouchOriginalPosition, myTouch.originalTouchPosition) < maxPressLength)
					{
						myTouch.touchType = EasyInputConstants.TOUCH_TYPE.DoublePress;
					}
					else
					{
						myTouch.touchType = EasyInputConstants.TOUCH_TYPE.QuickPress;
					}
					if (On_TouchStart__BackingField != null)
					{
						On_TouchStart__BackingField(myTouch);
					}
					if (On_Touch__BackingField != null)
					{
						On_Touch__BackingField(myTouch);
					}
				}
				else
				{
					if (myTouch.touchType == EasyInputConstants.TOUCH_TYPE.Miscellaneous && EasyInputUtilities.determineDistance(myTouch.originalTouchPosition, myTouch.currentTouchPosition) < maxPressLength && Time.time - myTouch.currentTouchBeginTimestamp < maxQuickTapTime)
					{
						myTouch.touchType = EasyInputConstants.TOUCH_TYPE.QuickPress;
					}
					if ((myTouch.touchType == EasyInputConstants.TOUCH_TYPE.Miscellaneous || myTouch.touchType == EasyInputConstants.TOUCH_TYPE.QuickPress || myTouch.touchType == EasyInputConstants.TOUCH_TYPE.DoublePress) && EasyInputUtilities.determineDistance(myTouch.originalTouchPosition, myTouch.currentTouchPosition) < maxPressLength && Time.time - myTouch.currentTouchBeginTimestamp > minLongTapTime)
					{
						myTouch.touchType = EasyInputConstants.TOUCH_TYPE.LongPress;
						if (On_LongTouchStart__BackingField != null)
						{
							On_LongTouchStart__BackingField(myTouch);
						}
					}
					if (EasyInputUtilities.determineDistance(myTouch.originalTouchPosition, myTouch.currentTouchPosition) > requiredSwipeLength)
					{
						if (myTouch.touchType == EasyInputConstants.TOUCH_TYPE.LongPress && On_LongTouchEnd__BackingField != null)
						{
							On_LongTouchEnd__BackingField(myTouch);
						}
						myTouch.touchType = EasyInputConstants.TOUCH_TYPE.Swipe;
						myTouch.swipeType = EasyInputUtilities.determineSwipeType(myTouch);
						if (swipeAllowed)
						{
							if (On_SwipeDetected__BackingField != null)
							{
								On_SwipeDetected__BackingField(myTouch);
							}
							swipeAllowed = false;
						}
					}
					if (myTouch.touchType != EasyInputConstants.TOUCH_TYPE.Swipe && myTouch.touchType != EasyInputConstants.TOUCH_TYPE.Miscellaneous)
					{
						if (myTouch.touchType == EasyInputConstants.TOUCH_TYPE.QuickPress)
						{
							if (EasyInputUtilities.determineDistance(myTouch.originalTouchPosition, myTouch.currentTouchPosition) > maxPressLength)
							{
								myTouch.touchType = EasyInputConstants.TOUCH_TYPE.Miscellaneous;
							}
							if (Time.time - myTouch.currentTouchBeginTimestamp > maxQuickTapTime)
							{
								myTouch.touchType = EasyInputConstants.TOUCH_TYPE.Miscellaneous;
							}
						}
						else if (myTouch.touchType == EasyInputConstants.TOUCH_TYPE.LongPress)
						{
							if (EasyInputUtilities.determineDistance(myTouch.originalTouchPosition, myTouch.currentTouchPosition) > maxPressLength)
							{
								myTouch.touchType = EasyInputConstants.TOUCH_TYPE.Miscellaneous;
								if (On_LongTouchEnd__BackingField != null)
								{
									On_LongTouchEnd__BackingField(myTouch);
								}
							}
						}
						else if (myTouch.touchType == EasyInputConstants.TOUCH_TYPE.DoublePress)
						{
							if (EasyInputUtilities.determineDistance(myTouch.originalTouchPosition, myTouch.currentTouchPosition) > maxPressLength)
							{
								myTouch.touchType = EasyInputConstants.TOUCH_TYPE.Miscellaneous;
							}
							if (Time.time - myTouch.currentTouchBeginTimestamp > maxDoubleTapTime)
							{
								myTouch.touchType = EasyInputConstants.TOUCH_TYPE.Miscellaneous;
							}
						}
					}
					if (On_Touch__BackingField != null)
					{
						On_Touch__BackingField(myTouch);
					}
					if (myTouch.touchType == EasyInputConstants.TOUCH_TYPE.LongPress && On_LongTouch__BackingField != null)
					{
						On_LongTouch__BackingField(myTouch);
					}
				}
				previousTouching = true;
				return;
			}
			if (previousTouching)
			{
				if (On_TouchEnd__BackingField != null)
				{
					On_TouchEnd__BackingField(myTouch);
				}
				if (myTouch.touchType == EasyInputConstants.TOUCH_TYPE.QuickPress)
				{
					if (Time.time - myTouch.currentTouchBeginTimestamp < maxQuickTapTime && On_QuickTouchEnd__BackingField != null)
					{
						On_QuickTouchEnd__BackingField(myTouch);
					}
				}
				else if (myTouch.touchType == EasyInputConstants.TOUCH_TYPE.LongPress)
				{
					if (Time.time - myTouch.currentTouchBeginTimestamp > minLongTapTime && On_LongTouchEnd__BackingField != null)
					{
						On_LongTouchEnd__BackingField(myTouch);
					}
				}
				else if (myTouch.touchType == EasyInputConstants.TOUCH_TYPE.DoublePress && Time.time - myTouch.previousTouchBeginTimestamp < maxDoubleTapTime && On_DoubleTouchEnd__BackingField != null)
				{
					On_DoubleTouchEnd__BackingField(myTouch);
				}
				myTouch.previousTouchBeginTimestamp = myTouch.currentTouchBeginTimestamp;
				myTouch.previousTouchOriginalPosition = myTouch.originalTouchPosition;
			}
			myTouch.originalTouchPosition = EasyInputConstants.NOT_TOUCHING;
			myTouch.currentTouchPosition = EasyInputConstants.NOT_TOUCHING;
			myTouch.currentTouchBeginTimestamp = EasyInputConstants.NO_TIMESTAMP;
			myTouch.touchType = EasyInputConstants.TOUCH_TYPE.Miscellaneous;
			myTouch.swipeType = EasyInputConstants.SWIPE_TYPE.None;
			swipeAllowed = true;
			previousTouching = false;
		}

		private void setupControllerButtons()
		{
			for (int i = 1; i < 4; i++)
			{
				buttonArray[i * 7 - 7] = new ButtonClick();
				buttonArray[i * 7 - 7].player = (EasyInputConstants.PLAYER_NUMBER)(i - 1);
				buttonArray[i * 7 - 7].button = EasyInputConstants.CONTROLLER_BUTTON.AButton;
				buttonArray[i * 7 - 6] = new ButtonClick();
				buttonArray[i * 7 - 6].player = (EasyInputConstants.PLAYER_NUMBER)(i - 1);
				buttonArray[i * 7 - 6].button = EasyInputConstants.CONTROLLER_BUTTON.BButton;
				buttonArray[i * 7 - 5] = new ButtonClick();
				buttonArray[i * 7 - 5].player = (EasyInputConstants.PLAYER_NUMBER)(i - 1);
				buttonArray[i * 7 - 5].button = EasyInputConstants.CONTROLLER_BUTTON.XButton;
				buttonArray[i * 7 - 4] = new ButtonClick();
				buttonArray[i * 7 - 4].player = (EasyInputConstants.PLAYER_NUMBER)(i - 1);
				buttonArray[i * 7 - 4].button = EasyInputConstants.CONTROLLER_BUTTON.YButton;
				buttonArray[i * 7 - 3] = new ButtonClick();
				buttonArray[i * 7 - 3].player = (EasyInputConstants.PLAYER_NUMBER)(i - 1);
				buttonArray[i * 7 - 3].button = EasyInputConstants.CONTROLLER_BUTTON.LeftBumper;
				buttonArray[i * 7 - 2] = new ButtonClick();
				buttonArray[i * 7 - 2].player = (EasyInputConstants.PLAYER_NUMBER)(i - 1);
				buttonArray[i * 7 - 2].button = EasyInputConstants.CONTROLLER_BUTTON.RightBumper;
				buttonArray[i * 7 - 1] = new ButtonClick();
				buttonArray[i * 7 - 1].player = (EasyInputConstants.PLAYER_NUMBER)(i - 1);
				buttonArray[i * 7 - 1].button = EasyInputConstants.CONTROLLER_BUTTON.MenuButton;
			}
		}

		private void setupControllerAxes()
		{
			for (int i = 1; i < 4; i++)
			{
				axisArray[i * 5 - 5] = new ControllerAxis();
				axisArray[i * 5 - 5].player = (EasyInputConstants.PLAYER_NUMBER)(i - 1);
				axisArray[i * 5 - 5].axis = EasyInputConstants.CONTROLLER_AXIS.LeftStick;
				axisArray[i * 5 - 4] = new ControllerAxis();
				axisArray[i * 5 - 4].player = (EasyInputConstants.PLAYER_NUMBER)(i - 1);
				axisArray[i * 5 - 4].axis = EasyInputConstants.CONTROLLER_AXIS.RightStick;
				axisArray[i * 5 - 3] = new ControllerAxis();
				axisArray[i * 5 - 3].player = (EasyInputConstants.PLAYER_NUMBER)(i - 1);
				axisArray[i * 5 - 3].axis = EasyInputConstants.CONTROLLER_AXIS.DPad;
				axisArray[i * 5 - 2] = new ControllerAxis();
				axisArray[i * 5 - 2].player = (EasyInputConstants.PLAYER_NUMBER)(i - 1);
				axisArray[i * 5 - 2].axis = EasyInputConstants.CONTROLLER_AXIS.LeftTrigger;
				axisArray[i * 5 - 1] = new ControllerAxis();
				axisArray[i * 5 - 1].player = (EasyInputConstants.PLAYER_NUMBER)(i - 1);
				axisArray[i * 5 - 1].axis = EasyInputConstants.CONTROLLER_AXIS.RightTrigger;
			}
			string[] joystickNames = Input.GetJoystickNames();
			if (joystickNames == null || joystickNames.Length < 2 || joystickNames[0].Contains("by Remote"))
			{
				touchAxisHorizontal = "ATV_P1_Remote_Horizontal";
				touchAxisVertical = "ATV_P1_Remote_Vertical";
				siriRemotePlayer = EasyInputConstants.PLAYER_NUMBER.Player1;
			}
			else if (joystickNames[1].Contains("by Remote"))
			{
				touchAxisHorizontal = "ATV_P2_Remote_Horizontal";
				touchAxisVertical = "ATV_P2_Remote_Vertical";
				siriRemotePlayer = EasyInputConstants.PLAYER_NUMBER.Player2;
			}
			else
			{
				touchAxisHorizontal = "ATV_P3_Remote_Horizontal";
				touchAxisVertical = "ATV_P3_Remote_Vertical";
				siriRemotePlayer = EasyInputConstants.PLAYER_NUMBER.Player3;
			}
		}

		private void processControllerButtonEvents()
		{
			for (int i = 0; i < 21; i++)
			{
				processSpecificButton(ref buttonArray[i]);
			}
		}

		private void processControllerAxisEvents()
		{
			for (int i = 0; i < 15; i++)
			{
				processSpecificAxis(ref axisArray[i]);
			}
		}

		private void processMotionEvents()
		{
			if (orientation == EasyInputConstants.REMOTE_ORIENTATION.TouchpadTop)
			{
				if (On_Accelerometer__BackingField != null)
				{
					On_Accelerometer__BackingField(Input.acceleration);
				}
				if (On_Gyro__BackingField != null)
				{
					On_Gyro__BackingField(Input.gyro.gravity, Input.gyro.userAcceleration);
				}
				if (On_Motion__BackingField != null)
				{
					deriveMotionEvents(Input.gyro.gravity, Input.gyro.userAcceleration);
				}
			}
			else if (orientation == EasyInputConstants.REMOTE_ORIENTATION.TouchpadLeft)
			{
				temp.x = 0f - Input.acceleration.y;
				temp.y = Input.acceleration.x;
				temp.z = Input.acceleration.z;
				if (On_Accelerometer__BackingField != null)
				{
					On_Accelerometer__BackingField(temp);
				}
				temp.x = 0f - Input.gyro.gravity.y;
				temp.y = Input.gyro.gravity.x;
				temp.z = Input.gyro.gravity.z;
				temp1.x = 0f - Input.gyro.userAcceleration.y;
				temp1.y = Input.gyro.userAcceleration.x;
				temp1.z = Input.gyro.userAcceleration.z;
				if (On_Gyro__BackingField != null)
				{
					On_Gyro__BackingField(temp, temp1);
				}
				if (On_Motion__BackingField != null)
				{
					deriveMotionEvents(temp, temp1);
				}
			}
			else if (orientation == EasyInputConstants.REMOTE_ORIENTATION.TouchpadRight)
			{
				temp.x = Input.acceleration.y;
				temp.y = 0f - Input.acceleration.x;
				temp.z = Input.acceleration.z;
				if (On_Accelerometer__BackingField != null)
				{
					On_Accelerometer__BackingField(temp);
				}
				temp.x = Input.gyro.gravity.y;
				temp.y = 0f - Input.gyro.gravity.x;
				temp.z = Input.gyro.gravity.z;
				temp1.x = Input.gyro.userAcceleration.y;
				temp1.y = 0f - Input.gyro.userAcceleration.x;
				temp1.z = Input.gyro.userAcceleration.z;
				if (On_Gyro__BackingField != null)
				{
					On_Gyro__BackingField(temp, temp1);
				}
				if (On_Motion__BackingField != null)
				{
					deriveMotionEvents(temp, temp1);
				}
			}
		}

		private void deriveMotionEvents(Vector3 gravity, Vector3 userAcceleration)
		{
			if (myMotion.currentOrientation == EasyInputConstants.NOT_VALID)
			{
				myMotion.currentRawGravity = gravity;
				myMotion.currentRawUserAcceleration = userAcceleration;
				myMotion.currentSmoothedGravity = gravity;
				myMotion.currentSmoothedUserAcceleration = userAcceleration;
				framesWithinThresholdX = 1;
				framesWithinThresholdY = 1;
				framesWithinThresholdZ = 1;
				framesVelocitySameX = 1;
				framesVelocitySameY = 1;
				framesVelocitySameZ = 1;
				myMotion.lastFrameOrientation = EasyInputConstants.NOT_VALID;
				myMotion.currentRotationRate = Vector3.zero;
				temp.z = Mathf.Atan2(gravity.z, gravity.y) * (180f / (float)Math.PI);
				temp.x = Mathf.Atan2(0f - gravity.x, Mathf.Sqrt(Mathf.Pow(gravity.y, 2f) + Mathf.Pow(gravity.z, 2f))) * (180f / (float)Math.PI);
				temp.y = Mathf.Atan2(Mathf.Sqrt(Mathf.Pow(gravity.x, 2f) + Mathf.Pow(gravity.z, 2f)), gravity.y) * (180f / (float)Math.PI);
				myMotion.currentOrientation = temp;
			}
			else
			{
				myMotion.lastFrameOrientation = myMotion.currentOrientation;
				myMotion.lastFrameVelocity = myMotion.currentVelocity;
				myMotion.lastFrameSmoothedGravity = myMotion.currentSmoothedGravity;
				myMotion.lastFrameSmoothedUserAcceleration = myMotion.currentSmoothedUserAcceleration;
				myMotion.lastFrameRotationRate = myMotion.currentRotationRate;
				myMotion.currentRawGravity = gravity;
				myMotion.currentRawUserAcceleration = userAcceleration;
				myMotion.currentSmoothedGravity = EasyInputUtilities.dampenSensorNoise(myMotion.currentRawGravity, myMotion.lastFrameSmoothedGravity, EasyInputConstants.MINUMUM_NOISE_THRESHOLD, framesWithinThresholdX, framesWithinThresholdY, framesWithinThresholdZ);
				myMotion.currentSmoothedUserAcceleration = EasyInputUtilities.dampenSensorNoise(myMotion.currentRawUserAcceleration, myMotion.lastFrameSmoothedUserAcceleration, EasyInputConstants.MINUMUM_NOISE_THRESHOLD, framesWithinThresholdX, framesWithinThresholdY, framesWithinThresholdZ);
				framesWithinThresholdX = ((myMotion.currentRawGravity.x == myMotion.currentSmoothedGravity.x) ? 1 : (framesWithinThresholdX + 1));
				framesWithinThresholdY = ((myMotion.currentRawGravity.y == myMotion.currentSmoothedGravity.y) ? 1 : (framesWithinThresholdY + 1));
				framesWithinThresholdZ = ((myMotion.currentRawGravity.z == myMotion.currentSmoothedGravity.z) ? 1 : (framesWithinThresholdZ + 1));
				temp.z = Mathf.Atan2(gravity.z, gravity.y) * (180f / (float)Math.PI);
				temp.x = Mathf.Atan2(0f - gravity.x, Mathf.Sqrt(Mathf.Pow(gravity.y, 2f) + Mathf.Pow(gravity.z, 2f))) * (180f / (float)Math.PI);
				temp.y = Mathf.Atan2(Mathf.Sqrt(Mathf.Pow(gravity.x, 2f) + Mathf.Pow(gravity.z, 2f)), gravity.y) * (180f / (float)Math.PI);
				myMotion.currentOrientation = temp;
				if (myMotion.currentOrientation.x < 0f && myMotion.currentRawGravity.y > 0f)
				{
					myMotion.currentOrientation.x = -180f - myMotion.currentOrientation.x;
				}
				if (myMotion.currentOrientation.x > 0f && myMotion.currentRawGravity.y > 0f)
				{
					myMotion.currentOrientation.x = 180f - myMotion.currentOrientation.x;
				}
				if (myMotion.currentOrientation.z > 0f)
				{
					myMotion.currentOrientation.z = 180f - myMotion.currentOrientation.z;
				}
				if (myMotion.currentOrientation.z < 0f)
				{
					myMotion.currentOrientation.z = -180f - myMotion.currentOrientation.z;
				}
				myMotion.currentRotationRate = (myMotion.lastFrameOrientation - myMotion.currentOrientation) * Time.deltaTime;
				if (myMotion.lastFrameOrientation.x - myMotion.currentOrientation.x > 350f)
				{
					myMotion.currentRotationRate.x = (360f - (myMotion.lastFrameOrientation.x - myMotion.currentOrientation.x)) * Time.deltaTime;
				}
				else if (myMotion.lastFrameOrientation.x - myMotion.currentOrientation.x < -350f)
				{
					myMotion.currentRotationRate.x = (360f + (myMotion.lastFrameOrientation.x - myMotion.currentOrientation.x)) * Time.deltaTime;
				}
				if (myMotion.lastFrameOrientation.y - myMotion.currentOrientation.y > 350f)
				{
					myMotion.currentRotationRate.y = (360f - (myMotion.lastFrameOrientation.y - myMotion.currentOrientation.y)) * Time.deltaTime;
				}
				else if (myMotion.lastFrameOrientation.y - myMotion.currentOrientation.y < -350f)
				{
					myMotion.currentRotationRate.y = (360f + (myMotion.lastFrameOrientation.y - myMotion.currentOrientation.y)) * Time.deltaTime;
				}
				if (myMotion.lastFrameOrientation.z - myMotion.currentOrientation.z > 350f)
				{
					myMotion.currentRotationRate.z = (360f - (myMotion.lastFrameOrientation.z - myMotion.currentOrientation.z)) * Time.deltaTime;
				}
				else if (myMotion.lastFrameOrientation.z - myMotion.currentOrientation.z < -350f)
				{
					myMotion.currentRotationRate.z = (360f + (myMotion.lastFrameOrientation.z - myMotion.currentOrientation.z)) * Time.deltaTime;
				}
				if (myMotion.currentRotationRate.x < EasyInputConstants.MINUMUM_NOISE_THRESHOLD && myMotion.currentRotationRate.x > 0f - EasyInputConstants.MINUMUM_NOISE_THRESHOLD)
				{
					myMotion.currentRotationRate.x = 0f;
				}
				if (myMotion.currentRotationRate.y < EasyInputConstants.MINUMUM_NOISE_THRESHOLD && myMotion.currentRotationRate.y > 0f - EasyInputConstants.MINUMUM_NOISE_THRESHOLD)
				{
					myMotion.currentRotationRate.y = 0f;
				}
				if (myMotion.currentRotationRate.z < EasyInputConstants.MINUMUM_NOISE_THRESHOLD && myMotion.currentRotationRate.z > 0f - EasyInputConstants.MINUMUM_NOISE_THRESHOLD)
				{
					myMotion.currentRotationRate.z = 0f;
				}
				myMotion.currentVelocity = -(myMotion.currentSmoothedUserAcceleration * Time.deltaTime) + myMotion.lastFrameVelocity;
				framesVelocitySameX = ((myMotion.currentVelocity.x != myMotion.lastFrameVelocity.x) ? 1 : (framesVelocitySameX + 1));
				framesVelocitySameY = ((myMotion.currentVelocity.y != myMotion.lastFrameVelocity.y) ? 1 : (framesVelocitySameY + 1));
				framesVelocitySameZ = ((myMotion.currentVelocity.z != myMotion.lastFrameVelocity.z) ? 1 : (framesVelocitySameZ + 1));
				if (framesVelocitySameX > EasyInputConstants.CONSECUTIVE_FRAMES_MOVE_VELOCITY_TO_ZERO)
				{
					myMotion.currentVelocity.x = 0f;
					framesVelocitySameX = 1;
				}
				if (framesVelocitySameY > EasyInputConstants.CONSECUTIVE_FRAMES_MOVE_VELOCITY_TO_ZERO)
				{
					myMotion.currentVelocity.y = 0f;
					framesVelocitySameY = 1;
				}
				if (framesVelocitySameZ > EasyInputConstants.CONSECUTIVE_FRAMES_MOVE_VELOCITY_TO_ZERO)
				{
					myMotion.currentVelocity.z = 0f;
					framesVelocitySameZ = 1;
				}
				myMotion.currentRelativePositionSinceReset += myMotion.currentVelocity * Time.deltaTime;
				myMotion.totalRotationRateSinceReset += myMotion.currentRotationRate;
				myMotion.totalVelocitySinceReset += myMotion.currentVelocity;
			}
			On_Motion__BackingField(myMotion);
		}

		private void processSpecificButton(ref ButtonClick buttonClick)
		{
			buttonClick.currentlyPressed = getControllerButtonState(buttonClick.player, buttonClick.button);
			if (buttonClick.currentlyPressed)
			{
				if (!buttonClick.previousFramePressed)
				{
					buttonClick.currentClickBeginTimestamp = Time.time;
					if (buttonClick.previousClickBeginTimestamp != EasyInputConstants.NO_TIMESTAMP && buttonClick.currentClickBeginTimestamp - buttonClick.previousClickBeginTimestamp < maxDoubleTapTime)
					{
						buttonClick.clickType = EasyInputConstants.CLICK_TYPE.DoublePress;
					}
					else
					{
						buttonClick.clickType = EasyInputConstants.CLICK_TYPE.QuickPress;
					}
					if (On_ClickStart__BackingField != null)
					{
						On_ClickStart__BackingField(buttonClick);
					}
					if (On_Click__BackingField != null)
					{
						On_Click__BackingField(buttonClick);
					}
				}
				else
				{
					if (buttonClick.clickType == EasyInputConstants.CLICK_TYPE.Miscellaneous && Time.time - buttonClick.currentClickBeginTimestamp < maxQuickTapTime)
					{
						buttonClick.clickType = EasyInputConstants.CLICK_TYPE.QuickPress;
					}
					if (buttonClick.clickType != EasyInputConstants.CLICK_TYPE.LongPress && Time.time - buttonClick.currentClickBeginTimestamp > minLongTapTime)
					{
						buttonClick.clickType = EasyInputConstants.CLICK_TYPE.LongPress;
						if (On_LongClickStart__BackingField != null)
						{
							On_LongClickStart__BackingField(buttonClick);
						}
					}
					if (buttonClick.clickType != EasyInputConstants.CLICK_TYPE.Miscellaneous)
					{
						if (buttonClick.clickType == EasyInputConstants.CLICK_TYPE.QuickPress)
						{
							if (Time.time - buttonClick.currentClickBeginTimestamp > maxQuickTapTime)
							{
								buttonClick.clickType = EasyInputConstants.CLICK_TYPE.Miscellaneous;
							}
						}
						else if (buttonClick.clickType == EasyInputConstants.CLICK_TYPE.DoublePress && Time.time - buttonClick.currentClickBeginTimestamp > maxDoubleTapTime)
						{
							buttonClick.clickType = EasyInputConstants.CLICK_TYPE.Miscellaneous;
						}
					}
					if (On_Click__BackingField != null)
					{
						On_Click__BackingField(buttonClick);
					}
					if (buttonClick.clickType == EasyInputConstants.CLICK_TYPE.LongPress && On_LongClick__BackingField != null)
					{
						On_LongClick__BackingField(buttonClick);
					}
				}
				buttonClick.previousFramePressed = true;
				return;
			}
			if (buttonClick.previousFramePressed)
			{
				if (On_ClickEnd__BackingField != null)
				{
					On_ClickEnd__BackingField(buttonClick);
				}
				if (buttonClick.clickType == EasyInputConstants.CLICK_TYPE.QuickPress)
				{
					if (Time.time - buttonClick.currentClickBeginTimestamp < maxQuickTapTime && On_QuickClickEnd__BackingField != null)
					{
						On_QuickClickEnd__BackingField(buttonClick);
					}
				}
				else if (buttonClick.clickType == EasyInputConstants.CLICK_TYPE.LongPress)
				{
					if (Time.time - buttonClick.currentClickBeginTimestamp > minLongTapTime && On_LongClickEnd__BackingField != null)
					{
						On_LongClickEnd__BackingField(buttonClick);
					}
				}
				else if (buttonClick.clickType == EasyInputConstants.CLICK_TYPE.DoublePress && Time.time - buttonClick.previousClickBeginTimestamp < maxDoubleTapTime && On_DoubleClickEnd__BackingField != null)
				{
					On_DoubleClickEnd__BackingField(buttonClick);
				}
				buttonClick.previousClickBeginTimestamp = buttonClick.currentClickBeginTimestamp;
			}
			buttonClick.currentClickBeginTimestamp = EasyInputConstants.NO_TIMESTAMP;
			buttonClick.clickType = EasyInputConstants.CLICK_TYPE.Miscellaneous;
			buttonClick.previousFramePressed = false;
		}

		private void processSpecificAxis(ref ControllerAxis axis)
		{
			switch (axis.player)
			{
			case EasyInputConstants.PLAYER_NUMBER.Player1:
				switch (axis.axis)
				{
				case EasyInputConstants.CONTROLLER_AXIS.LeftStick:
					axis.axisValue.x = Input.GetAxisRaw("ATV_P1_LeftStick_Horizontal");
					axis.axisValue.y = Input.GetAxisRaw("ATV_P1_LeftStick_Vertical");
					if (On_LeftStick__BackingField != null)
					{
						On_LeftStick__BackingField(axis);
					}
					break;
				case EasyInputConstants.CONTROLLER_AXIS.RightStick:
					axis.axisValue.x = Input.GetAxisRaw("ATV_P1_RightStick_Horizontal");
					axis.axisValue.y = Input.GetAxisRaw("ATV_P1_RightStick_Vertical");
					if (On_RightStick__BackingField != null)
					{
						On_RightStick__BackingField(axis);
					}
					break;
				case EasyInputConstants.CONTROLLER_AXIS.DPad:
					axis.axisValue.x = Input.GetAxisRaw("ATV_P1_Dpad_Horizontal");
					axis.axisValue.y = Input.GetAxisRaw("ATV_P1_Dpad_Vertical");
					if (On_Dpad__BackingField != null)
					{
						On_Dpad__BackingField(axis);
					}
					break;
				case EasyInputConstants.CONTROLLER_AXIS.LeftTrigger:
					axis.axisValue.x = Input.GetAxisRaw("ATV_P1_LeftTrigger");
					axis.axisValue.y = Input.GetAxisRaw("ATV_P1_LeftTrigger");
					if (On_LeftTrigger__BackingField != null)
					{
						On_LeftTrigger__BackingField(axis);
					}
					break;
				case EasyInputConstants.CONTROLLER_AXIS.RightTrigger:
					axis.axisValue.x = Input.GetAxisRaw("ATV_P1_RightTrigger");
					axis.axisValue.y = Input.GetAxisRaw("ATV_P1_RightTrigger");
					if (On_RightTrigger__BackingField != null)
					{
						On_RightTrigger__BackingField(axis);
					}
					break;
				}
				break;
			case EasyInputConstants.PLAYER_NUMBER.Player2:
				switch (axis.axis)
				{
				case EasyInputConstants.CONTROLLER_AXIS.LeftStick:
					axis.axisValue.x = Input.GetAxisRaw("ATV_P2_LeftStick_Horizontal");
					axis.axisValue.y = Input.GetAxisRaw("ATV_P2_LeftStick_Vertical");
					if (On_LeftStick__BackingField != null)
					{
						On_LeftStick__BackingField(axis);
					}
					break;
				case EasyInputConstants.CONTROLLER_AXIS.RightStick:
					axis.axisValue.x = Input.GetAxisRaw("ATV_P2_RightStick_Horizontal");
					axis.axisValue.y = Input.GetAxisRaw("ATV_P2_RightStick_Vertical");
					if (On_RightStick__BackingField != null)
					{
						On_RightStick__BackingField(axis);
					}
					break;
				case EasyInputConstants.CONTROLLER_AXIS.DPad:
					axis.axisValue.x = Input.GetAxisRaw("ATV_P2_Dpad_Horizontal");
					axis.axisValue.y = Input.GetAxisRaw("ATV_P2_Dpad_Vertical");
					if (On_Dpad__BackingField != null)
					{
						On_Dpad__BackingField(axis);
					}
					break;
				case EasyInputConstants.CONTROLLER_AXIS.LeftTrigger:
					axis.axisValue.x = Input.GetAxisRaw("ATV_P2_LeftTrigger");
					axis.axisValue.y = Input.GetAxisRaw("ATV_P2_LeftTrigger");
					if (On_LeftTrigger__BackingField != null)
					{
						On_LeftTrigger__BackingField(axis);
					}
					break;
				case EasyInputConstants.CONTROLLER_AXIS.RightTrigger:
					axis.axisValue.x = Input.GetAxisRaw("ATV_P2_RightTrigger");
					axis.axisValue.y = Input.GetAxisRaw("ATV_P2_RightTrigger");
					if (On_RightTrigger__BackingField != null)
					{
						On_RightTrigger__BackingField(axis);
					}
					break;
				}
				break;
			case EasyInputConstants.PLAYER_NUMBER.Player3:
				switch (axis.axis)
				{
				case EasyInputConstants.CONTROLLER_AXIS.LeftStick:
					axis.axisValue.x = Input.GetAxisRaw("ATV_P3_LeftStick_Horizontal");
					axis.axisValue.y = Input.GetAxisRaw("ATV_P3_LeftStick_Vertical");
					if (On_LeftStick__BackingField != null)
					{
						On_LeftStick__BackingField(axis);
					}
					break;
				case EasyInputConstants.CONTROLLER_AXIS.RightStick:
					axis.axisValue.x = Input.GetAxisRaw("ATV_P3_RightStick_Horizontal");
					axis.axisValue.y = Input.GetAxisRaw("ATV_P3_RightStick_Vertical");
					if (On_RightStick__BackingField != null)
					{
						On_RightStick__BackingField(axis);
					}
					break;
				case EasyInputConstants.CONTROLLER_AXIS.DPad:
					axis.axisValue.x = Input.GetAxisRaw("ATV_P3_Dpad_Horizontal");
					axis.axisValue.y = Input.GetAxisRaw("ATV_P3_Dpad_Vertical");
					if (On_Dpad__BackingField != null)
					{
						On_Dpad__BackingField(axis);
					}
					break;
				case EasyInputConstants.CONTROLLER_AXIS.LeftTrigger:
					axis.axisValue.x = Input.GetAxisRaw("ATV_P3_LeftTrigger");
					axis.axisValue.y = Input.GetAxisRaw("ATV_P3_LeftTrigger");
					if (On_LeftTrigger__BackingField != null)
					{
						On_LeftTrigger__BackingField(axis);
					}
					break;
				case EasyInputConstants.CONTROLLER_AXIS.RightTrigger:
					axis.axisValue.x = Input.GetAxisRaw("ATV_P3_RightTrigger");
					axis.axisValue.y = Input.GetAxisRaw("ATV_P3_RightTrigger");
					if (On_RightTrigger__BackingField != null)
					{
						On_RightTrigger__BackingField(axis);
					}
					break;
				}
				break;
			}
		}

		private bool getControllerButtonState(EasyInputConstants.PLAYER_NUMBER player, EasyInputConstants.CONTROLLER_BUTTON button)
		{
			return false;
		}
	}
}

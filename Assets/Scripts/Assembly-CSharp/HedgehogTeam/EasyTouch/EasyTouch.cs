using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HedgehogTeam.EasyTouch
{
	public class EasyTouch : MonoBehaviour
	{
		[Serializable]
		private class DoubleTap
		{
			public bool inDoubleTap;

			public bool inWait;

			public float time;

			public int count;

			public Finger finger;

			public void Stop()
			{
				inDoubleTap = false;
				inWait = false;
				time = 0f;
				count = 0;
			}
		}

		private class PickedObject
		{
			public GameObject pickedObj;

			public Camera pickedCamera;

			public bool isGUI;
		}

		public delegate void TouchCancelHandler(Gesture gesture);

		public delegate void Cancel2FingersHandler(Gesture gesture);

		public delegate void TouchStartHandler(Gesture gesture);

		public delegate void TouchDownHandler(Gesture gesture);

		public delegate void TouchUpHandler(Gesture gesture);

		public delegate void SimpleTapHandler(Gesture gesture);

		public delegate void DoubleTapHandler(Gesture gesture);

		public delegate void LongTapStartHandler(Gesture gesture);

		public delegate void LongTapHandler(Gesture gesture);

		public delegate void LongTapEndHandler(Gesture gesture);

		public delegate void DragStartHandler(Gesture gesture);

		public delegate void DragHandler(Gesture gesture);

		public delegate void DragEndHandler(Gesture gesture);

		public delegate void SwipeStartHandler(Gesture gesture);

		public delegate void SwipeHandler(Gesture gesture);

		public delegate void SwipeEndHandler(Gesture gesture);

		public delegate void TouchStart2FingersHandler(Gesture gesture);

		public delegate void TouchDown2FingersHandler(Gesture gesture);

		public delegate void TouchUp2FingersHandler(Gesture gesture);

		public delegate void SimpleTap2FingersHandler(Gesture gesture);

		public delegate void DoubleTap2FingersHandler(Gesture gesture);

		public delegate void LongTapStart2FingersHandler(Gesture gesture);

		public delegate void LongTap2FingersHandler(Gesture gesture);

		public delegate void LongTapEnd2FingersHandler(Gesture gesture);

		public delegate void TwistHandler(Gesture gesture);

		public delegate void TwistEndHandler(Gesture gesture);

		public delegate void PinchInHandler(Gesture gesture);

		public delegate void PinchOutHandler(Gesture gesture);

		public delegate void PinchEndHandler(Gesture gesture);

		public delegate void PinchHandler(Gesture gesture);

		public delegate void DragStart2FingersHandler(Gesture gesture);

		public delegate void Drag2FingersHandler(Gesture gesture);

		public delegate void DragEnd2FingersHandler(Gesture gesture);

		public delegate void SwipeStart2FingersHandler(Gesture gesture);

		public delegate void Swipe2FingersHandler(Gesture gesture);

		public delegate void SwipeEnd2FingersHandler(Gesture gesture);

		public delegate void EasyTouchIsReadyHandler();

		public delegate void OverUIElementHandler(Gesture gesture);

		public delegate void UIElementTouchUpHandler(Gesture gesture);

		public enum GesturePriority
		{
			Tap = 0,
			Slips = 1
		}

		public enum DoubleTapDetection
		{
			BySystem = 0,
			ByTime = 1
		}

		public enum GestureType
		{
			Tap = 0,
			Drag = 1,
			Swipe = 2,
			None = 3,
			LongTap = 4,
			Pinch = 5,
			Twist = 6,
			Cancel = 7,
			Acquisition = 8
		}

		public enum SwipeDirection
		{
			None = 0,
			Left = 1,
			Right = 2,
			Up = 3,
			Down = 4,
			UpLeft = 5,
			UpRight = 6,
			DownLeft = 7,
			DownRight = 8,
			Other = 9,
			All = 10
		}

		public enum TwoFingerPickMethod
		{
			Finger = 0,
			Average = 1
		}

		public enum EvtType
		{
			None = 0,
			On_TouchStart = 1,
			On_TouchDown = 2,
			On_TouchUp = 3,
			On_SimpleTap = 4,
			On_DoubleTap = 5,
			On_LongTapStart = 6,
			On_LongTap = 7,
			On_LongTapEnd = 8,
			On_DragStart = 9,
			On_Drag = 10,
			On_DragEnd = 11,
			On_SwipeStart = 12,
			On_Swipe = 13,
			On_SwipeEnd = 14,
			On_TouchStart2Fingers = 15,
			On_TouchDown2Fingers = 16,
			On_TouchUp2Fingers = 17,
			On_SimpleTap2Fingers = 18,
			On_DoubleTap2Fingers = 19,
			On_LongTapStart2Fingers = 20,
			On_LongTap2Fingers = 21,
			On_LongTapEnd2Fingers = 22,
			On_Twist = 23,
			On_TwistEnd = 24,
			On_Pinch = 25,
			On_PinchIn = 26,
			On_PinchOut = 27,
			On_PinchEnd = 28,
			On_DragStart2Fingers = 29,
			On_Drag2Fingers = 30,
			On_DragEnd2Fingers = 31,
			On_SwipeStart2Fingers = 32,
			On_Swipe2Fingers = 33,
			On_SwipeEnd2Fingers = 34,
			On_EasyTouchIsReady = 35,
			On_Cancel = 36,
			On_Cancel2Fingers = 37,
			On_OverUIElement = 38,
			On_UIElementTouchUp = 39
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static TouchCancelHandler On_Cancel__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Cancel2FingersHandler On_Cancel2Fingers__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static TouchStartHandler On_TouchStart__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static TouchDownHandler On_TouchDown__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static TouchUpHandler On_TouchUp__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static SimpleTapHandler On_SimpleTap__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static DoubleTapHandler On_DoubleTap__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static LongTapStartHandler On_LongTapStart__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static LongTapHandler On_LongTap__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static LongTapEndHandler On_LongTapEnd__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static DragStartHandler On_DragStart__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static DragHandler On_Drag__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static DragEndHandler On_DragEnd__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static SwipeStartHandler On_SwipeStart__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static SwipeHandler On_Swipe__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static SwipeEndHandler On_SwipeEnd__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static TouchStart2FingersHandler On_TouchStart2Fingers__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static TouchDown2FingersHandler On_TouchDown2Fingers__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static TouchUp2FingersHandler On_TouchUp2Fingers__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static SimpleTap2FingersHandler On_SimpleTap2Fingers__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static DoubleTap2FingersHandler On_DoubleTap2Fingers__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static LongTapStart2FingersHandler On_LongTapStart2Fingers__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static LongTap2FingersHandler On_LongTap2Fingers__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static LongTapEnd2FingersHandler On_LongTapEnd2Fingers__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static TwistHandler On_Twist__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static TwistEndHandler On_TwistEnd__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static PinchHandler On_Pinch__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static PinchInHandler On_PinchIn__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static PinchOutHandler On_PinchOut__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static PinchEndHandler On_PinchEnd__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static DragStart2FingersHandler On_DragStart2Fingers__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Drag2FingersHandler On_Drag2Fingers__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static DragEnd2FingersHandler On_DragEnd2Fingers__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static SwipeStart2FingersHandler On_SwipeStart2Fingers__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Swipe2FingersHandler On_Swipe2Fingers__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static SwipeEnd2FingersHandler On_SwipeEnd2Fingers__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static EasyTouchIsReadyHandler On_EasyTouchIsReady__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static OverUIElementHandler On_OverUIElement__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static UIElementTouchUpHandler On_UIElementTouchUp__BackingField;

		private static EasyTouch _instance;

		private Gesture _currentGesture = new Gesture();

		private List<Gesture> _currentGestures = new List<Gesture>();

		public bool enable;

		public bool enableRemote;

		public GesturePriority gesturePriority;

		public float StationaryTolerance;

		public float longTapTime;

		public float swipeTolerance;

		public float minPinchLength;

		public float minTwistAngle;

		public DoubleTapDetection doubleTapDetection;

		public float doubleTapTime;

		public bool alwaysSendSwipe;

		public bool enable2FingersGesture;

		public bool enableTwist;

		public bool enablePinch;

		public bool enable2FingersSwipe;

		public TwoFingerPickMethod twoFingerPickMethod;

		public List<ECamera> touchCameras;

		public bool autoSelect;

		public LayerMask pickableLayers3D;

		public bool enable2D;

		public LayerMask pickableLayers2D;

		public bool autoUpdatePickedObject;

		public bool allowUIDetection;

		public bool enableUIMode;

		public bool autoUpdatePickedUI;

		public bool enabledNGuiMode;

		public LayerMask nGUILayers;

		public List<Camera> nGUICameras;

		public bool enableSimulation;

		public KeyCode twistKey;

		public KeyCode swipeKey;

		public bool showGuiInspector;

		public bool showSelectInspector;

		public bool showGestureInspector;

		public bool showTwoFingerInspector;

		public bool showSecondFingerInspector;

		private EasyTouchInput input = new EasyTouchInput();

		private Finger[] fingers = new Finger[100];

		public Texture secondFingerTexture;

		private TwoFingerGesture twoFinger = new TwoFingerGesture();

		private int oldTouchCount;

		private DoubleTap[] singleDoubleTap = new DoubleTap[100];

		private Finger[] tmpArray = new Finger[100];

		private PickedObject pickedObject = new PickedObject();

		private List<RaycastResult> uiRaycastResultCache = new List<RaycastResult>();

		private PointerEventData uiPointerEventData;

		private EventSystem uiEventSystem;

		public static EasyTouch instance
		{
			get
			{
				if (!_instance)
				{
					_instance = UnityEngine.Object.FindObjectOfType(typeof(EasyTouch)) as EasyTouch;
					if (!_instance)
					{
						GameObject gameObject = new GameObject("Easytouch");
						_instance = gameObject.AddComponent<EasyTouch>();
					}
				}
				return _instance;
			}
		}

		public static Gesture current
		{
			get
			{
				return instance._currentGesture;
			}
		}

		public static event TouchCancelHandler On_Cancel
		{
			add
			{
				TouchCancelHandler touchCancelHandler = On_Cancel__BackingField;
				TouchCancelHandler touchCancelHandler2;
				do
				{
					touchCancelHandler2 = touchCancelHandler;
					touchCancelHandler = Interlocked.CompareExchange(ref On_Cancel__BackingField, (TouchCancelHandler)Delegate.Combine(touchCancelHandler2, value), touchCancelHandler);
				}
				while ((object)touchCancelHandler != touchCancelHandler2);
			}
			remove
			{
				TouchCancelHandler touchCancelHandler = On_Cancel__BackingField;
				TouchCancelHandler touchCancelHandler2;
				do
				{
					touchCancelHandler2 = touchCancelHandler;
					touchCancelHandler = Interlocked.CompareExchange(ref On_Cancel__BackingField, (TouchCancelHandler)Delegate.Remove(touchCancelHandler2, value), touchCancelHandler);
				}
				while ((object)touchCancelHandler != touchCancelHandler2);
			}
		}

		public static event Cancel2FingersHandler On_Cancel2Fingers
		{
			add
			{
				Cancel2FingersHandler cancel2FingersHandler = On_Cancel2Fingers__BackingField;
				Cancel2FingersHandler cancel2FingersHandler2;
				do
				{
					cancel2FingersHandler2 = cancel2FingersHandler;
					cancel2FingersHandler = Interlocked.CompareExchange(ref On_Cancel2Fingers__BackingField, (Cancel2FingersHandler)Delegate.Combine(cancel2FingersHandler2, value), cancel2FingersHandler);
				}
				while ((object)cancel2FingersHandler != cancel2FingersHandler2);
			}
			remove
			{
				Cancel2FingersHandler cancel2FingersHandler = On_Cancel2Fingers__BackingField;
				Cancel2FingersHandler cancel2FingersHandler2;
				do
				{
					cancel2FingersHandler2 = cancel2FingersHandler;
					cancel2FingersHandler = Interlocked.CompareExchange(ref On_Cancel2Fingers__BackingField, (Cancel2FingersHandler)Delegate.Remove(cancel2FingersHandler2, value), cancel2FingersHandler);
				}
				while ((object)cancel2FingersHandler != cancel2FingersHandler2);
			}
		}

		public static event TouchStartHandler On_TouchStart
		{
			add
			{
				TouchStartHandler touchStartHandler = On_TouchStart__BackingField;
				TouchStartHandler touchStartHandler2;
				do
				{
					touchStartHandler2 = touchStartHandler;
					touchStartHandler = Interlocked.CompareExchange(ref On_TouchStart__BackingField, (TouchStartHandler)Delegate.Combine(touchStartHandler2, value), touchStartHandler);
				}
				while ((object)touchStartHandler != touchStartHandler2);
			}
			remove
			{
				TouchStartHandler touchStartHandler = On_TouchStart__BackingField;
				TouchStartHandler touchStartHandler2;
				do
				{
					touchStartHandler2 = touchStartHandler;
					touchStartHandler = Interlocked.CompareExchange(ref On_TouchStart__BackingField, (TouchStartHandler)Delegate.Remove(touchStartHandler2, value), touchStartHandler);
				}
				while ((object)touchStartHandler != touchStartHandler2);
			}
		}

		public static event TouchDownHandler On_TouchDown
		{
			add
			{
				TouchDownHandler touchDownHandler = On_TouchDown__BackingField;
				TouchDownHandler touchDownHandler2;
				do
				{
					touchDownHandler2 = touchDownHandler;
					touchDownHandler = Interlocked.CompareExchange(ref On_TouchDown__BackingField, (TouchDownHandler)Delegate.Combine(touchDownHandler2, value), touchDownHandler);
				}
				while ((object)touchDownHandler != touchDownHandler2);
			}
			remove
			{
				TouchDownHandler touchDownHandler = On_TouchDown__BackingField;
				TouchDownHandler touchDownHandler2;
				do
				{
					touchDownHandler2 = touchDownHandler;
					touchDownHandler = Interlocked.CompareExchange(ref On_TouchDown__BackingField, (TouchDownHandler)Delegate.Remove(touchDownHandler2, value), touchDownHandler);
				}
				while ((object)touchDownHandler != touchDownHandler2);
			}
		}

		public static event TouchUpHandler On_TouchUp
		{
			add
			{
				TouchUpHandler touchUpHandler = On_TouchUp__BackingField;
				TouchUpHandler touchUpHandler2;
				do
				{
					touchUpHandler2 = touchUpHandler;
					touchUpHandler = Interlocked.CompareExchange(ref On_TouchUp__BackingField, (TouchUpHandler)Delegate.Combine(touchUpHandler2, value), touchUpHandler);
				}
				while ((object)touchUpHandler != touchUpHandler2);
			}
			remove
			{
				TouchUpHandler touchUpHandler = On_TouchUp__BackingField;
				TouchUpHandler touchUpHandler2;
				do
				{
					touchUpHandler2 = touchUpHandler;
					touchUpHandler = Interlocked.CompareExchange(ref On_TouchUp__BackingField, (TouchUpHandler)Delegate.Remove(touchUpHandler2, value), touchUpHandler);
				}
				while ((object)touchUpHandler != touchUpHandler2);
			}
		}

		public static event SimpleTapHandler On_SimpleTap
		{
			add
			{
				SimpleTapHandler simpleTapHandler = On_SimpleTap__BackingField;
				SimpleTapHandler simpleTapHandler2;
				do
				{
					simpleTapHandler2 = simpleTapHandler;
					simpleTapHandler = Interlocked.CompareExchange(ref On_SimpleTap__BackingField, (SimpleTapHandler)Delegate.Combine(simpleTapHandler2, value), simpleTapHandler);
				}
				while ((object)simpleTapHandler != simpleTapHandler2);
			}
			remove
			{
				SimpleTapHandler simpleTapHandler = On_SimpleTap__BackingField;
				SimpleTapHandler simpleTapHandler2;
				do
				{
					simpleTapHandler2 = simpleTapHandler;
					simpleTapHandler = Interlocked.CompareExchange(ref On_SimpleTap__BackingField, (SimpleTapHandler)Delegate.Remove(simpleTapHandler2, value), simpleTapHandler);
				}
				while ((object)simpleTapHandler != simpleTapHandler2);
			}
		}

		public static event DoubleTapHandler On_DoubleTap
		{
			add
			{
				DoubleTapHandler doubleTapHandler = On_DoubleTap__BackingField;
				DoubleTapHandler doubleTapHandler2;
				do
				{
					doubleTapHandler2 = doubleTapHandler;
					doubleTapHandler = Interlocked.CompareExchange(ref On_DoubleTap__BackingField, (DoubleTapHandler)Delegate.Combine(doubleTapHandler2, value), doubleTapHandler);
				}
				while ((object)doubleTapHandler != doubleTapHandler2);
			}
			remove
			{
				DoubleTapHandler doubleTapHandler = On_DoubleTap__BackingField;
				DoubleTapHandler doubleTapHandler2;
				do
				{
					doubleTapHandler2 = doubleTapHandler;
					doubleTapHandler = Interlocked.CompareExchange(ref On_DoubleTap__BackingField, (DoubleTapHandler)Delegate.Remove(doubleTapHandler2, value), doubleTapHandler);
				}
				while ((object)doubleTapHandler != doubleTapHandler2);
			}
		}

		public static event LongTapStartHandler On_LongTapStart
		{
			add
			{
				LongTapStartHandler longTapStartHandler = On_LongTapStart__BackingField;
				LongTapStartHandler longTapStartHandler2;
				do
				{
					longTapStartHandler2 = longTapStartHandler;
					longTapStartHandler = Interlocked.CompareExchange(ref On_LongTapStart__BackingField, (LongTapStartHandler)Delegate.Combine(longTapStartHandler2, value), longTapStartHandler);
				}
				while ((object)longTapStartHandler != longTapStartHandler2);
			}
			remove
			{
				LongTapStartHandler longTapStartHandler = On_LongTapStart__BackingField;
				LongTapStartHandler longTapStartHandler2;
				do
				{
					longTapStartHandler2 = longTapStartHandler;
					longTapStartHandler = Interlocked.CompareExchange(ref On_LongTapStart__BackingField, (LongTapStartHandler)Delegate.Remove(longTapStartHandler2, value), longTapStartHandler);
				}
				while ((object)longTapStartHandler != longTapStartHandler2);
			}
		}

		public static event LongTapHandler On_LongTap
		{
			add
			{
				LongTapHandler longTapHandler = On_LongTap__BackingField;
				LongTapHandler longTapHandler2;
				do
				{
					longTapHandler2 = longTapHandler;
					longTapHandler = Interlocked.CompareExchange(ref On_LongTap__BackingField, (LongTapHandler)Delegate.Combine(longTapHandler2, value), longTapHandler);
				}
				while ((object)longTapHandler != longTapHandler2);
			}
			remove
			{
				LongTapHandler longTapHandler = On_LongTap__BackingField;
				LongTapHandler longTapHandler2;
				do
				{
					longTapHandler2 = longTapHandler;
					longTapHandler = Interlocked.CompareExchange(ref On_LongTap__BackingField, (LongTapHandler)Delegate.Remove(longTapHandler2, value), longTapHandler);
				}
				while ((object)longTapHandler != longTapHandler2);
			}
		}

		public static event LongTapEndHandler On_LongTapEnd
		{
			add
			{
				LongTapEndHandler longTapEndHandler = On_LongTapEnd__BackingField;
				LongTapEndHandler longTapEndHandler2;
				do
				{
					longTapEndHandler2 = longTapEndHandler;
					longTapEndHandler = Interlocked.CompareExchange(ref On_LongTapEnd__BackingField, (LongTapEndHandler)Delegate.Combine(longTapEndHandler2, value), longTapEndHandler);
				}
				while ((object)longTapEndHandler != longTapEndHandler2);
			}
			remove
			{
				LongTapEndHandler longTapEndHandler = On_LongTapEnd__BackingField;
				LongTapEndHandler longTapEndHandler2;
				do
				{
					longTapEndHandler2 = longTapEndHandler;
					longTapEndHandler = Interlocked.CompareExchange(ref On_LongTapEnd__BackingField, (LongTapEndHandler)Delegate.Remove(longTapEndHandler2, value), longTapEndHandler);
				}
				while ((object)longTapEndHandler != longTapEndHandler2);
			}
		}

		public static event DragStartHandler On_DragStart
		{
			add
			{
				DragStartHandler dragStartHandler = On_DragStart__BackingField;
				DragStartHandler dragStartHandler2;
				do
				{
					dragStartHandler2 = dragStartHandler;
					dragStartHandler = Interlocked.CompareExchange(ref On_DragStart__BackingField, (DragStartHandler)Delegate.Combine(dragStartHandler2, value), dragStartHandler);
				}
				while ((object)dragStartHandler != dragStartHandler2);
			}
			remove
			{
				DragStartHandler dragStartHandler = On_DragStart__BackingField;
				DragStartHandler dragStartHandler2;
				do
				{
					dragStartHandler2 = dragStartHandler;
					dragStartHandler = Interlocked.CompareExchange(ref On_DragStart__BackingField, (DragStartHandler)Delegate.Remove(dragStartHandler2, value), dragStartHandler);
				}
				while ((object)dragStartHandler != dragStartHandler2);
			}
		}

		public static event DragHandler On_Drag
		{
			add
			{
				DragHandler dragHandler = On_Drag__BackingField;
				DragHandler dragHandler2;
				do
				{
					dragHandler2 = dragHandler;
					dragHandler = Interlocked.CompareExchange(ref On_Drag__BackingField, (DragHandler)Delegate.Combine(dragHandler2, value), dragHandler);
				}
				while ((object)dragHandler != dragHandler2);
			}
			remove
			{
				DragHandler dragHandler = On_Drag__BackingField;
				DragHandler dragHandler2;
				do
				{
					dragHandler2 = dragHandler;
					dragHandler = Interlocked.CompareExchange(ref On_Drag__BackingField, (DragHandler)Delegate.Remove(dragHandler2, value), dragHandler);
				}
				while ((object)dragHandler != dragHandler2);
			}
		}

		public static event DragEndHandler On_DragEnd
		{
			add
			{
				DragEndHandler dragEndHandler = On_DragEnd__BackingField;
				DragEndHandler dragEndHandler2;
				do
				{
					dragEndHandler2 = dragEndHandler;
					dragEndHandler = Interlocked.CompareExchange(ref On_DragEnd__BackingField, (DragEndHandler)Delegate.Combine(dragEndHandler2, value), dragEndHandler);
				}
				while ((object)dragEndHandler != dragEndHandler2);
			}
			remove
			{
				DragEndHandler dragEndHandler = On_DragEnd__BackingField;
				DragEndHandler dragEndHandler2;
				do
				{
					dragEndHandler2 = dragEndHandler;
					dragEndHandler = Interlocked.CompareExchange(ref On_DragEnd__BackingField, (DragEndHandler)Delegate.Remove(dragEndHandler2, value), dragEndHandler);
				}
				while ((object)dragEndHandler != dragEndHandler2);
			}
		}

		public static event SwipeStartHandler On_SwipeStart
		{
			add
			{
				SwipeStartHandler swipeStartHandler = On_SwipeStart__BackingField;
				SwipeStartHandler swipeStartHandler2;
				do
				{
					swipeStartHandler2 = swipeStartHandler;
					swipeStartHandler = Interlocked.CompareExchange(ref On_SwipeStart__BackingField, (SwipeStartHandler)Delegate.Combine(swipeStartHandler2, value), swipeStartHandler);
				}
				while ((object)swipeStartHandler != swipeStartHandler2);
			}
			remove
			{
				SwipeStartHandler swipeStartHandler = On_SwipeStart__BackingField;
				SwipeStartHandler swipeStartHandler2;
				do
				{
					swipeStartHandler2 = swipeStartHandler;
					swipeStartHandler = Interlocked.CompareExchange(ref On_SwipeStart__BackingField, (SwipeStartHandler)Delegate.Remove(swipeStartHandler2, value), swipeStartHandler);
				}
				while ((object)swipeStartHandler != swipeStartHandler2);
			}
		}

		public static event SwipeHandler On_Swipe
		{
			add
			{
				SwipeHandler swipeHandler = On_Swipe__BackingField;
				SwipeHandler swipeHandler2;
				do
				{
					swipeHandler2 = swipeHandler;
					swipeHandler = Interlocked.CompareExchange(ref On_Swipe__BackingField, (SwipeHandler)Delegate.Combine(swipeHandler2, value), swipeHandler);
				}
				while ((object)swipeHandler != swipeHandler2);
			}
			remove
			{
				SwipeHandler swipeHandler = On_Swipe__BackingField;
				SwipeHandler swipeHandler2;
				do
				{
					swipeHandler2 = swipeHandler;
					swipeHandler = Interlocked.CompareExchange(ref On_Swipe__BackingField, (SwipeHandler)Delegate.Remove(swipeHandler2, value), swipeHandler);
				}
				while ((object)swipeHandler != swipeHandler2);
			}
		}

		public static event SwipeEndHandler On_SwipeEnd
		{
			add
			{
				SwipeEndHandler swipeEndHandler = On_SwipeEnd__BackingField;
				SwipeEndHandler swipeEndHandler2;
				do
				{
					swipeEndHandler2 = swipeEndHandler;
					swipeEndHandler = Interlocked.CompareExchange(ref On_SwipeEnd__BackingField, (SwipeEndHandler)Delegate.Combine(swipeEndHandler2, value), swipeEndHandler);
				}
				while ((object)swipeEndHandler != swipeEndHandler2);
			}
			remove
			{
				SwipeEndHandler swipeEndHandler = On_SwipeEnd__BackingField;
				SwipeEndHandler swipeEndHandler2;
				do
				{
					swipeEndHandler2 = swipeEndHandler;
					swipeEndHandler = Interlocked.CompareExchange(ref On_SwipeEnd__BackingField, (SwipeEndHandler)Delegate.Remove(swipeEndHandler2, value), swipeEndHandler);
				}
				while ((object)swipeEndHandler != swipeEndHandler2);
			}
		}

		public static event TouchStart2FingersHandler On_TouchStart2Fingers
		{
			add
			{
				TouchStart2FingersHandler touchStart2FingersHandler = On_TouchStart2Fingers__BackingField;
				TouchStart2FingersHandler touchStart2FingersHandler2;
				do
				{
					touchStart2FingersHandler2 = touchStart2FingersHandler;
					touchStart2FingersHandler = Interlocked.CompareExchange(ref On_TouchStart2Fingers__BackingField, (TouchStart2FingersHandler)Delegate.Combine(touchStart2FingersHandler2, value), touchStart2FingersHandler);
				}
				while ((object)touchStart2FingersHandler != touchStart2FingersHandler2);
			}
			remove
			{
				TouchStart2FingersHandler touchStart2FingersHandler = On_TouchStart2Fingers__BackingField;
				TouchStart2FingersHandler touchStart2FingersHandler2;
				do
				{
					touchStart2FingersHandler2 = touchStart2FingersHandler;
					touchStart2FingersHandler = Interlocked.CompareExchange(ref On_TouchStart2Fingers__BackingField, (TouchStart2FingersHandler)Delegate.Remove(touchStart2FingersHandler2, value), touchStart2FingersHandler);
				}
				while ((object)touchStart2FingersHandler != touchStart2FingersHandler2);
			}
		}

		public static event TouchDown2FingersHandler On_TouchDown2Fingers
		{
			add
			{
				TouchDown2FingersHandler touchDown2FingersHandler = On_TouchDown2Fingers__BackingField;
				TouchDown2FingersHandler touchDown2FingersHandler2;
				do
				{
					touchDown2FingersHandler2 = touchDown2FingersHandler;
					touchDown2FingersHandler = Interlocked.CompareExchange(ref On_TouchDown2Fingers__BackingField, (TouchDown2FingersHandler)Delegate.Combine(touchDown2FingersHandler2, value), touchDown2FingersHandler);
				}
				while ((object)touchDown2FingersHandler != touchDown2FingersHandler2);
			}
			remove
			{
				TouchDown2FingersHandler touchDown2FingersHandler = On_TouchDown2Fingers__BackingField;
				TouchDown2FingersHandler touchDown2FingersHandler2;
				do
				{
					touchDown2FingersHandler2 = touchDown2FingersHandler;
					touchDown2FingersHandler = Interlocked.CompareExchange(ref On_TouchDown2Fingers__BackingField, (TouchDown2FingersHandler)Delegate.Remove(touchDown2FingersHandler2, value), touchDown2FingersHandler);
				}
				while ((object)touchDown2FingersHandler != touchDown2FingersHandler2);
			}
		}

		public static event TouchUp2FingersHandler On_TouchUp2Fingers
		{
			add
			{
				TouchUp2FingersHandler touchUp2FingersHandler = On_TouchUp2Fingers__BackingField;
				TouchUp2FingersHandler touchUp2FingersHandler2;
				do
				{
					touchUp2FingersHandler2 = touchUp2FingersHandler;
					touchUp2FingersHandler = Interlocked.CompareExchange(ref On_TouchUp2Fingers__BackingField, (TouchUp2FingersHandler)Delegate.Combine(touchUp2FingersHandler2, value), touchUp2FingersHandler);
				}
				while ((object)touchUp2FingersHandler != touchUp2FingersHandler2);
			}
			remove
			{
				TouchUp2FingersHandler touchUp2FingersHandler = On_TouchUp2Fingers__BackingField;
				TouchUp2FingersHandler touchUp2FingersHandler2;
				do
				{
					touchUp2FingersHandler2 = touchUp2FingersHandler;
					touchUp2FingersHandler = Interlocked.CompareExchange(ref On_TouchUp2Fingers__BackingField, (TouchUp2FingersHandler)Delegate.Remove(touchUp2FingersHandler2, value), touchUp2FingersHandler);
				}
				while ((object)touchUp2FingersHandler != touchUp2FingersHandler2);
			}
		}

		public static event SimpleTap2FingersHandler On_SimpleTap2Fingers
		{
			add
			{
				SimpleTap2FingersHandler simpleTap2FingersHandler = On_SimpleTap2Fingers__BackingField;
				SimpleTap2FingersHandler simpleTap2FingersHandler2;
				do
				{
					simpleTap2FingersHandler2 = simpleTap2FingersHandler;
					simpleTap2FingersHandler = Interlocked.CompareExchange(ref On_SimpleTap2Fingers__BackingField, (SimpleTap2FingersHandler)Delegate.Combine(simpleTap2FingersHandler2, value), simpleTap2FingersHandler);
				}
				while ((object)simpleTap2FingersHandler != simpleTap2FingersHandler2);
			}
			remove
			{
				SimpleTap2FingersHandler simpleTap2FingersHandler = On_SimpleTap2Fingers__BackingField;
				SimpleTap2FingersHandler simpleTap2FingersHandler2;
				do
				{
					simpleTap2FingersHandler2 = simpleTap2FingersHandler;
					simpleTap2FingersHandler = Interlocked.CompareExchange(ref On_SimpleTap2Fingers__BackingField, (SimpleTap2FingersHandler)Delegate.Remove(simpleTap2FingersHandler2, value), simpleTap2FingersHandler);
				}
				while ((object)simpleTap2FingersHandler != simpleTap2FingersHandler2);
			}
		}

		public static event DoubleTap2FingersHandler On_DoubleTap2Fingers
		{
			add
			{
				DoubleTap2FingersHandler doubleTap2FingersHandler = On_DoubleTap2Fingers__BackingField;
				DoubleTap2FingersHandler doubleTap2FingersHandler2;
				do
				{
					doubleTap2FingersHandler2 = doubleTap2FingersHandler;
					doubleTap2FingersHandler = Interlocked.CompareExchange(ref On_DoubleTap2Fingers__BackingField, (DoubleTap2FingersHandler)Delegate.Combine(doubleTap2FingersHandler2, value), doubleTap2FingersHandler);
				}
				while ((object)doubleTap2FingersHandler != doubleTap2FingersHandler2);
			}
			remove
			{
				DoubleTap2FingersHandler doubleTap2FingersHandler = On_DoubleTap2Fingers__BackingField;
				DoubleTap2FingersHandler doubleTap2FingersHandler2;
				do
				{
					doubleTap2FingersHandler2 = doubleTap2FingersHandler;
					doubleTap2FingersHandler = Interlocked.CompareExchange(ref On_DoubleTap2Fingers__BackingField, (DoubleTap2FingersHandler)Delegate.Remove(doubleTap2FingersHandler2, value), doubleTap2FingersHandler);
				}
				while ((object)doubleTap2FingersHandler != doubleTap2FingersHandler2);
			}
		}

		public static event LongTapStart2FingersHandler On_LongTapStart2Fingers
		{
			add
			{
				LongTapStart2FingersHandler longTapStart2FingersHandler = On_LongTapStart2Fingers__BackingField;
				LongTapStart2FingersHandler longTapStart2FingersHandler2;
				do
				{
					longTapStart2FingersHandler2 = longTapStart2FingersHandler;
					longTapStart2FingersHandler = Interlocked.CompareExchange(ref On_LongTapStart2Fingers__BackingField, (LongTapStart2FingersHandler)Delegate.Combine(longTapStart2FingersHandler2, value), longTapStart2FingersHandler);
				}
				while ((object)longTapStart2FingersHandler != longTapStart2FingersHandler2);
			}
			remove
			{
				LongTapStart2FingersHandler longTapStart2FingersHandler = On_LongTapStart2Fingers__BackingField;
				LongTapStart2FingersHandler longTapStart2FingersHandler2;
				do
				{
					longTapStart2FingersHandler2 = longTapStart2FingersHandler;
					longTapStart2FingersHandler = Interlocked.CompareExchange(ref On_LongTapStart2Fingers__BackingField, (LongTapStart2FingersHandler)Delegate.Remove(longTapStart2FingersHandler2, value), longTapStart2FingersHandler);
				}
				while ((object)longTapStart2FingersHandler != longTapStart2FingersHandler2);
			}
		}

		public static event LongTap2FingersHandler On_LongTap2Fingers
		{
			add
			{
				LongTap2FingersHandler longTap2FingersHandler = On_LongTap2Fingers__BackingField;
				LongTap2FingersHandler longTap2FingersHandler2;
				do
				{
					longTap2FingersHandler2 = longTap2FingersHandler;
					longTap2FingersHandler = Interlocked.CompareExchange(ref On_LongTap2Fingers__BackingField, (LongTap2FingersHandler)Delegate.Combine(longTap2FingersHandler2, value), longTap2FingersHandler);
				}
				while ((object)longTap2FingersHandler != longTap2FingersHandler2);
			}
			remove
			{
				LongTap2FingersHandler longTap2FingersHandler = On_LongTap2Fingers__BackingField;
				LongTap2FingersHandler longTap2FingersHandler2;
				do
				{
					longTap2FingersHandler2 = longTap2FingersHandler;
					longTap2FingersHandler = Interlocked.CompareExchange(ref On_LongTap2Fingers__BackingField, (LongTap2FingersHandler)Delegate.Remove(longTap2FingersHandler2, value), longTap2FingersHandler);
				}
				while ((object)longTap2FingersHandler != longTap2FingersHandler2);
			}
		}

		public static event LongTapEnd2FingersHandler On_LongTapEnd2Fingers
		{
			add
			{
				LongTapEnd2FingersHandler longTapEnd2FingersHandler = On_LongTapEnd2Fingers__BackingField;
				LongTapEnd2FingersHandler longTapEnd2FingersHandler2;
				do
				{
					longTapEnd2FingersHandler2 = longTapEnd2FingersHandler;
					longTapEnd2FingersHandler = Interlocked.CompareExchange(ref On_LongTapEnd2Fingers__BackingField, (LongTapEnd2FingersHandler)Delegate.Combine(longTapEnd2FingersHandler2, value), longTapEnd2FingersHandler);
				}
				while ((object)longTapEnd2FingersHandler != longTapEnd2FingersHandler2);
			}
			remove
			{
				LongTapEnd2FingersHandler longTapEnd2FingersHandler = On_LongTapEnd2Fingers__BackingField;
				LongTapEnd2FingersHandler longTapEnd2FingersHandler2;
				do
				{
					longTapEnd2FingersHandler2 = longTapEnd2FingersHandler;
					longTapEnd2FingersHandler = Interlocked.CompareExchange(ref On_LongTapEnd2Fingers__BackingField, (LongTapEnd2FingersHandler)Delegate.Remove(longTapEnd2FingersHandler2, value), longTapEnd2FingersHandler);
				}
				while ((object)longTapEnd2FingersHandler != longTapEnd2FingersHandler2);
			}
		}

		public static event TwistHandler On_Twist
		{
			add
			{
				TwistHandler twistHandler = On_Twist__BackingField;
				TwistHandler twistHandler2;
				do
				{
					twistHandler2 = twistHandler;
					twistHandler = Interlocked.CompareExchange(ref On_Twist__BackingField, (TwistHandler)Delegate.Combine(twistHandler2, value), twistHandler);
				}
				while ((object)twistHandler != twistHandler2);
			}
			remove
			{
				TwistHandler twistHandler = On_Twist__BackingField;
				TwistHandler twistHandler2;
				do
				{
					twistHandler2 = twistHandler;
					twistHandler = Interlocked.CompareExchange(ref On_Twist__BackingField, (TwistHandler)Delegate.Remove(twistHandler2, value), twistHandler);
				}
				while ((object)twistHandler != twistHandler2);
			}
		}

		public static event TwistEndHandler On_TwistEnd
		{
			add
			{
				TwistEndHandler twistEndHandler = On_TwistEnd__BackingField;
				TwistEndHandler twistEndHandler2;
				do
				{
					twistEndHandler2 = twistEndHandler;
					twistEndHandler = Interlocked.CompareExchange(ref On_TwistEnd__BackingField, (TwistEndHandler)Delegate.Combine(twistEndHandler2, value), twistEndHandler);
				}
				while ((object)twistEndHandler != twistEndHandler2);
			}
			remove
			{
				TwistEndHandler twistEndHandler = On_TwistEnd__BackingField;
				TwistEndHandler twistEndHandler2;
				do
				{
					twistEndHandler2 = twistEndHandler;
					twistEndHandler = Interlocked.CompareExchange(ref On_TwistEnd__BackingField, (TwistEndHandler)Delegate.Remove(twistEndHandler2, value), twistEndHandler);
				}
				while ((object)twistEndHandler != twistEndHandler2);
			}
		}

		public static event PinchHandler On_Pinch
		{
			add
			{
				PinchHandler pinchHandler = On_Pinch__BackingField;
				PinchHandler pinchHandler2;
				do
				{
					pinchHandler2 = pinchHandler;
					pinchHandler = Interlocked.CompareExchange(ref On_Pinch__BackingField, (PinchHandler)Delegate.Combine(pinchHandler2, value), pinchHandler);
				}
				while ((object)pinchHandler != pinchHandler2);
			}
			remove
			{
				PinchHandler pinchHandler = On_Pinch__BackingField;
				PinchHandler pinchHandler2;
				do
				{
					pinchHandler2 = pinchHandler;
					pinchHandler = Interlocked.CompareExchange(ref On_Pinch__BackingField, (PinchHandler)Delegate.Remove(pinchHandler2, value), pinchHandler);
				}
				while ((object)pinchHandler != pinchHandler2);
			}
		}

		public static event PinchInHandler On_PinchIn
		{
			add
			{
				PinchInHandler pinchInHandler = On_PinchIn__BackingField;
				PinchInHandler pinchInHandler2;
				do
				{
					pinchInHandler2 = pinchInHandler;
					pinchInHandler = Interlocked.CompareExchange(ref On_PinchIn__BackingField, (PinchInHandler)Delegate.Combine(pinchInHandler2, value), pinchInHandler);
				}
				while ((object)pinchInHandler != pinchInHandler2);
			}
			remove
			{
				PinchInHandler pinchInHandler = On_PinchIn__BackingField;
				PinchInHandler pinchInHandler2;
				do
				{
					pinchInHandler2 = pinchInHandler;
					pinchInHandler = Interlocked.CompareExchange(ref On_PinchIn__BackingField, (PinchInHandler)Delegate.Remove(pinchInHandler2, value), pinchInHandler);
				}
				while ((object)pinchInHandler != pinchInHandler2);
			}
		}

		public static event PinchOutHandler On_PinchOut
		{
			add
			{
				PinchOutHandler pinchOutHandler = On_PinchOut__BackingField;
				PinchOutHandler pinchOutHandler2;
				do
				{
					pinchOutHandler2 = pinchOutHandler;
					pinchOutHandler = Interlocked.CompareExchange(ref On_PinchOut__BackingField, (PinchOutHandler)Delegate.Combine(pinchOutHandler2, value), pinchOutHandler);
				}
				while ((object)pinchOutHandler != pinchOutHandler2);
			}
			remove
			{
				PinchOutHandler pinchOutHandler = On_PinchOut__BackingField;
				PinchOutHandler pinchOutHandler2;
				do
				{
					pinchOutHandler2 = pinchOutHandler;
					pinchOutHandler = Interlocked.CompareExchange(ref On_PinchOut__BackingField, (PinchOutHandler)Delegate.Remove(pinchOutHandler2, value), pinchOutHandler);
				}
				while ((object)pinchOutHandler != pinchOutHandler2);
			}
		}

		public static event PinchEndHandler On_PinchEnd
		{
			add
			{
				PinchEndHandler pinchEndHandler = On_PinchEnd__BackingField;
				PinchEndHandler pinchEndHandler2;
				do
				{
					pinchEndHandler2 = pinchEndHandler;
					pinchEndHandler = Interlocked.CompareExchange(ref On_PinchEnd__BackingField, (PinchEndHandler)Delegate.Combine(pinchEndHandler2, value), pinchEndHandler);
				}
				while ((object)pinchEndHandler != pinchEndHandler2);
			}
			remove
			{
				PinchEndHandler pinchEndHandler = On_PinchEnd__BackingField;
				PinchEndHandler pinchEndHandler2;
				do
				{
					pinchEndHandler2 = pinchEndHandler;
					pinchEndHandler = Interlocked.CompareExchange(ref On_PinchEnd__BackingField, (PinchEndHandler)Delegate.Remove(pinchEndHandler2, value), pinchEndHandler);
				}
				while ((object)pinchEndHandler != pinchEndHandler2);
			}
		}

		public static event DragStart2FingersHandler On_DragStart2Fingers
		{
			add
			{
				DragStart2FingersHandler dragStart2FingersHandler = On_DragStart2Fingers__BackingField;
				DragStart2FingersHandler dragStart2FingersHandler2;
				do
				{
					dragStart2FingersHandler2 = dragStart2FingersHandler;
					dragStart2FingersHandler = Interlocked.CompareExchange(ref On_DragStart2Fingers__BackingField, (DragStart2FingersHandler)Delegate.Combine(dragStart2FingersHandler2, value), dragStart2FingersHandler);
				}
				while ((object)dragStart2FingersHandler != dragStart2FingersHandler2);
			}
			remove
			{
				DragStart2FingersHandler dragStart2FingersHandler = On_DragStart2Fingers__BackingField;
				DragStart2FingersHandler dragStart2FingersHandler2;
				do
				{
					dragStart2FingersHandler2 = dragStart2FingersHandler;
					dragStart2FingersHandler = Interlocked.CompareExchange(ref On_DragStart2Fingers__BackingField, (DragStart2FingersHandler)Delegate.Remove(dragStart2FingersHandler2, value), dragStart2FingersHandler);
				}
				while ((object)dragStart2FingersHandler != dragStart2FingersHandler2);
			}
		}

		public static event Drag2FingersHandler On_Drag2Fingers
		{
			add
			{
				Drag2FingersHandler drag2FingersHandler = On_Drag2Fingers__BackingField;
				Drag2FingersHandler drag2FingersHandler2;
				do
				{
					drag2FingersHandler2 = drag2FingersHandler;
					drag2FingersHandler = Interlocked.CompareExchange(ref On_Drag2Fingers__BackingField, (Drag2FingersHandler)Delegate.Combine(drag2FingersHandler2, value), drag2FingersHandler);
				}
				while ((object)drag2FingersHandler != drag2FingersHandler2);
			}
			remove
			{
				Drag2FingersHandler drag2FingersHandler = On_Drag2Fingers__BackingField;
				Drag2FingersHandler drag2FingersHandler2;
				do
				{
					drag2FingersHandler2 = drag2FingersHandler;
					drag2FingersHandler = Interlocked.CompareExchange(ref On_Drag2Fingers__BackingField, (Drag2FingersHandler)Delegate.Remove(drag2FingersHandler2, value), drag2FingersHandler);
				}
				while ((object)drag2FingersHandler != drag2FingersHandler2);
			}
		}

		public static event DragEnd2FingersHandler On_DragEnd2Fingers
		{
			add
			{
				DragEnd2FingersHandler dragEnd2FingersHandler = On_DragEnd2Fingers__BackingField;
				DragEnd2FingersHandler dragEnd2FingersHandler2;
				do
				{
					dragEnd2FingersHandler2 = dragEnd2FingersHandler;
					dragEnd2FingersHandler = Interlocked.CompareExchange(ref On_DragEnd2Fingers__BackingField, (DragEnd2FingersHandler)Delegate.Combine(dragEnd2FingersHandler2, value), dragEnd2FingersHandler);
				}
				while ((object)dragEnd2FingersHandler != dragEnd2FingersHandler2);
			}
			remove
			{
				DragEnd2FingersHandler dragEnd2FingersHandler = On_DragEnd2Fingers__BackingField;
				DragEnd2FingersHandler dragEnd2FingersHandler2;
				do
				{
					dragEnd2FingersHandler2 = dragEnd2FingersHandler;
					dragEnd2FingersHandler = Interlocked.CompareExchange(ref On_DragEnd2Fingers__BackingField, (DragEnd2FingersHandler)Delegate.Remove(dragEnd2FingersHandler2, value), dragEnd2FingersHandler);
				}
				while ((object)dragEnd2FingersHandler != dragEnd2FingersHandler2);
			}
		}

		public static event SwipeStart2FingersHandler On_SwipeStart2Fingers
		{
			add
			{
				SwipeStart2FingersHandler swipeStart2FingersHandler = On_SwipeStart2Fingers__BackingField;
				SwipeStart2FingersHandler swipeStart2FingersHandler2;
				do
				{
					swipeStart2FingersHandler2 = swipeStart2FingersHandler;
					swipeStart2FingersHandler = Interlocked.CompareExchange(ref On_SwipeStart2Fingers__BackingField, (SwipeStart2FingersHandler)Delegate.Combine(swipeStart2FingersHandler2, value), swipeStart2FingersHandler);
				}
				while ((object)swipeStart2FingersHandler != swipeStart2FingersHandler2);
			}
			remove
			{
				SwipeStart2FingersHandler swipeStart2FingersHandler = On_SwipeStart2Fingers__BackingField;
				SwipeStart2FingersHandler swipeStart2FingersHandler2;
				do
				{
					swipeStart2FingersHandler2 = swipeStart2FingersHandler;
					swipeStart2FingersHandler = Interlocked.CompareExchange(ref On_SwipeStart2Fingers__BackingField, (SwipeStart2FingersHandler)Delegate.Remove(swipeStart2FingersHandler2, value), swipeStart2FingersHandler);
				}
				while ((object)swipeStart2FingersHandler != swipeStart2FingersHandler2);
			}
		}

		public static event Swipe2FingersHandler On_Swipe2Fingers
		{
			add
			{
				Swipe2FingersHandler swipe2FingersHandler = On_Swipe2Fingers__BackingField;
				Swipe2FingersHandler swipe2FingersHandler2;
				do
				{
					swipe2FingersHandler2 = swipe2FingersHandler;
					swipe2FingersHandler = Interlocked.CompareExchange(ref On_Swipe2Fingers__BackingField, (Swipe2FingersHandler)Delegate.Combine(swipe2FingersHandler2, value), swipe2FingersHandler);
				}
				while ((object)swipe2FingersHandler != swipe2FingersHandler2);
			}
			remove
			{
				Swipe2FingersHandler swipe2FingersHandler = On_Swipe2Fingers__BackingField;
				Swipe2FingersHandler swipe2FingersHandler2;
				do
				{
					swipe2FingersHandler2 = swipe2FingersHandler;
					swipe2FingersHandler = Interlocked.CompareExchange(ref On_Swipe2Fingers__BackingField, (Swipe2FingersHandler)Delegate.Remove(swipe2FingersHandler2, value), swipe2FingersHandler);
				}
				while ((object)swipe2FingersHandler != swipe2FingersHandler2);
			}
		}

		public static event SwipeEnd2FingersHandler On_SwipeEnd2Fingers
		{
			add
			{
				SwipeEnd2FingersHandler swipeEnd2FingersHandler = On_SwipeEnd2Fingers__BackingField;
				SwipeEnd2FingersHandler swipeEnd2FingersHandler2;
				do
				{
					swipeEnd2FingersHandler2 = swipeEnd2FingersHandler;
					swipeEnd2FingersHandler = Interlocked.CompareExchange(ref On_SwipeEnd2Fingers__BackingField, (SwipeEnd2FingersHandler)Delegate.Combine(swipeEnd2FingersHandler2, value), swipeEnd2FingersHandler);
				}
				while ((object)swipeEnd2FingersHandler != swipeEnd2FingersHandler2);
			}
			remove
			{
				SwipeEnd2FingersHandler swipeEnd2FingersHandler = On_SwipeEnd2Fingers__BackingField;
				SwipeEnd2FingersHandler swipeEnd2FingersHandler2;
				do
				{
					swipeEnd2FingersHandler2 = swipeEnd2FingersHandler;
					swipeEnd2FingersHandler = Interlocked.CompareExchange(ref On_SwipeEnd2Fingers__BackingField, (SwipeEnd2FingersHandler)Delegate.Remove(swipeEnd2FingersHandler2, value), swipeEnd2FingersHandler);
				}
				while ((object)swipeEnd2FingersHandler != swipeEnd2FingersHandler2);
			}
		}

		public static event EasyTouchIsReadyHandler On_EasyTouchIsReady
		{
			add
			{
				EasyTouchIsReadyHandler easyTouchIsReadyHandler = On_EasyTouchIsReady__BackingField;
				EasyTouchIsReadyHandler easyTouchIsReadyHandler2;
				do
				{
					easyTouchIsReadyHandler2 = easyTouchIsReadyHandler;
					easyTouchIsReadyHandler = Interlocked.CompareExchange(ref On_EasyTouchIsReady__BackingField, (EasyTouchIsReadyHandler)Delegate.Combine(easyTouchIsReadyHandler2, value), easyTouchIsReadyHandler);
				}
				while ((object)easyTouchIsReadyHandler != easyTouchIsReadyHandler2);
			}
			remove
			{
				EasyTouchIsReadyHandler easyTouchIsReadyHandler = On_EasyTouchIsReady__BackingField;
				EasyTouchIsReadyHandler easyTouchIsReadyHandler2;
				do
				{
					easyTouchIsReadyHandler2 = easyTouchIsReadyHandler;
					easyTouchIsReadyHandler = Interlocked.CompareExchange(ref On_EasyTouchIsReady__BackingField, (EasyTouchIsReadyHandler)Delegate.Remove(easyTouchIsReadyHandler2, value), easyTouchIsReadyHandler);
				}
				while ((object)easyTouchIsReadyHandler != easyTouchIsReadyHandler2);
			}
		}

		public static event OverUIElementHandler On_OverUIElement
		{
			add
			{
				OverUIElementHandler overUIElementHandler = On_OverUIElement__BackingField;
				OverUIElementHandler overUIElementHandler2;
				do
				{
					overUIElementHandler2 = overUIElementHandler;
					overUIElementHandler = Interlocked.CompareExchange(ref On_OverUIElement__BackingField, (OverUIElementHandler)Delegate.Combine(overUIElementHandler2, value), overUIElementHandler);
				}
				while ((object)overUIElementHandler != overUIElementHandler2);
			}
			remove
			{
				OverUIElementHandler overUIElementHandler = On_OverUIElement__BackingField;
				OverUIElementHandler overUIElementHandler2;
				do
				{
					overUIElementHandler2 = overUIElementHandler;
					overUIElementHandler = Interlocked.CompareExchange(ref On_OverUIElement__BackingField, (OverUIElementHandler)Delegate.Remove(overUIElementHandler2, value), overUIElementHandler);
				}
				while ((object)overUIElementHandler != overUIElementHandler2);
			}
		}

		public static event UIElementTouchUpHandler On_UIElementTouchUp
		{
			add
			{
				UIElementTouchUpHandler uIElementTouchUpHandler = On_UIElementTouchUp__BackingField;
				UIElementTouchUpHandler uIElementTouchUpHandler2;
				do
				{
					uIElementTouchUpHandler2 = uIElementTouchUpHandler;
					uIElementTouchUpHandler = Interlocked.CompareExchange(ref On_UIElementTouchUp__BackingField, (UIElementTouchUpHandler)Delegate.Combine(uIElementTouchUpHandler2, value), uIElementTouchUpHandler);
				}
				while ((object)uIElementTouchUpHandler != uIElementTouchUpHandler2);
			}
			remove
			{
				UIElementTouchUpHandler uIElementTouchUpHandler = On_UIElementTouchUp__BackingField;
				UIElementTouchUpHandler uIElementTouchUpHandler2;
				do
				{
					uIElementTouchUpHandler2 = uIElementTouchUpHandler;
					uIElementTouchUpHandler = Interlocked.CompareExchange(ref On_UIElementTouchUp__BackingField, (UIElementTouchUpHandler)Delegate.Remove(uIElementTouchUpHandler2, value), uIElementTouchUpHandler);
				}
				while ((object)uIElementTouchUpHandler != uIElementTouchUpHandler2);
			}
		}

		public EasyTouch()
		{
			enable = true;
			allowUIDetection = true;
			enableUIMode = true;
			autoUpdatePickedUI = false;
			enabledNGuiMode = false;
			nGUICameras = new List<Camera>();
			autoSelect = true;
			touchCameras = new List<ECamera>();
			pickableLayers3D = 1;
			enable2D = false;
			pickableLayers2D = 1;
			gesturePriority = GesturePriority.Tap;
			StationaryTolerance = 15f;
			longTapTime = 1f;
			doubleTapDetection = DoubleTapDetection.BySystem;
			doubleTapTime = 0.3f;
			swipeTolerance = 0.85f;
			alwaysSendSwipe = false;
			enable2FingersGesture = true;
			twoFingerPickMethod = TwoFingerPickMethod.Finger;
			enable2FingersSwipe = true;
			enablePinch = true;
			minPinchLength = 0f;
			enableTwist = true;
			minTwistAngle = 0f;
			enableSimulation = true;
			twistKey = KeyCode.LeftAlt;
			swipeKey = KeyCode.LeftControl;
		}

		private void OnEnable()
		{
			if (Application.isPlaying && Application.isEditor)
			{
				Init();
			}
		}

		private void Awake()
		{
			Init();
		}

		private void Start()
		{
			for (int i = 0; i < 100; i++)
			{
				singleDoubleTap[i] = new DoubleTap();
			}
			int num = touchCameras.FindIndex((ECamera c) => c.camera == Camera.main);
			if (num < 0)
			{
				touchCameras.Add(new ECamera(Camera.main, false));
			}
			if (On_EasyTouchIsReady__BackingField != null)
			{
				On_EasyTouchIsReady__BackingField();
			}
			_currentGestures.Add(new Gesture());
		}

		private void Init()
		{
		}

		private void OnDrawGizmos()
		{
		}

		private void Update()
		{
			if (!enable || !(instance == this))
			{
				return;
			}
			if (Application.isPlaying && Input.touchCount > 0)
			{
				enableRemote = true;
			}
			if (Application.isPlaying && Input.touchCount == 0)
			{
				enableRemote = false;
			}
			int num = input.TouchCount();
			if (oldTouchCount == 2 && num != 2 && num > 0)
			{
				CreateGesture2Finger(EvtType.On_Cancel2Fingers, Vector2.zero, Vector2.zero, Vector2.zero, 0f, SwipeDirection.None, 0f, Vector2.zero, 0f, 0f, 0f);
			}
			UpdateTouches(true, num);
			twoFinger.oldPickedObject = twoFinger.pickedObject;
			if (enable2FingersGesture && num == 2)
			{
				TwoFinger();
			}
			for (int i = 0; i < 100; i++)
			{
				if (fingers[i] != null)
				{
					OneFinger(i);
				}
			}
			oldTouchCount = num;
		}

		private void LateUpdate()
		{
			if (_currentGestures.Count > 1)
			{
				_currentGestures.RemoveAt(0);
			}
			else
			{
				_currentGestures[0] = new Gesture();
			}
			_currentGesture = _currentGestures[0];
		}

		private void UpdateTouches(bool realTouch, int touchCount)
		{
			fingers.CopyTo(tmpArray, 0);
			if (realTouch || enableRemote)
			{
				ResetTouches();
				for (int i = 0; i < touchCount; i++)
				{
					Touch touch = Input.GetTouch(i);
					for (int j = 0; j < 100; j++)
					{
						if (fingers[i] != null)
						{
							break;
						}
						if (tmpArray[j] != null && tmpArray[j].fingerIndex == touch.fingerId)
						{
							fingers[i] = tmpArray[j];
						}
					}
					if (fingers[i] == null)
					{
						fingers[i] = new Finger();
						fingers[i].fingerIndex = touch.fingerId;
						fingers[i].gesture = GestureType.None;
						fingers[i].phase = TouchPhase.Began;
					}
					else
					{
						fingers[i].phase = touch.phase;
					}
					if (fingers[i].phase != TouchPhase.Began)
					{
						fingers[i].deltaPosition = touch.position - fingers[i].position;
					}
					else
					{
						fingers[i].deltaPosition = Vector2.zero;
					}
					fingers[i].position = touch.position;
					fingers[i].tapCount = touch.tapCount;
					fingers[i].deltaTime = touch.deltaTime;
					fingers[i].touchCount = touchCount;
				}
			}
			else
			{
				for (int k = 0; k < touchCount; k++)
				{
					fingers[k] = input.GetMouseTouch(k, fingers[k]);
					fingers[k].touchCount = touchCount;
				}
			}
		}

		private void ResetTouches()
		{
			for (int i = 0; i < 100; i++)
			{
				fingers[i] = null;
			}
		}

		private void OneFinger(int fingerIndex)
		{
			if (fingers[fingerIndex].gesture == GestureType.None)
			{
				if (!singleDoubleTap[fingerIndex].inDoubleTap)
				{
					singleDoubleTap[fingerIndex].inDoubleTap = true;
					singleDoubleTap[fingerIndex].time = 0f;
					singleDoubleTap[fingerIndex].count = 1;
				}
				fingers[fingerIndex].startTimeAction = Time.realtimeSinceStartup;
				fingers[fingerIndex].gesture = GestureType.Acquisition;
				fingers[fingerIndex].startPosition = fingers[fingerIndex].position;
				if (autoSelect && GetPickedGameObject(fingers[fingerIndex]))
				{
					fingers[fingerIndex].pickedObject = pickedObject.pickedObj;
					fingers[fingerIndex].isGuiCamera = pickedObject.isGUI;
					fingers[fingerIndex].pickedCamera = pickedObject.pickedCamera;
				}
				if (allowUIDetection)
				{
					fingers[fingerIndex].isOverGui = IsScreenPositionOverUI(fingers[fingerIndex].position);
					fingers[fingerIndex].pickedUIElement = GetFirstUIElementFromCache();
				}
				CreateGesture(fingerIndex, EvtType.On_TouchStart, fingers[fingerIndex], SwipeDirection.None, 0f, Vector2.zero);
			}
			if (singleDoubleTap[fingerIndex].inDoubleTap)
			{
				singleDoubleTap[fingerIndex].time += Time.deltaTime;
			}
			fingers[fingerIndex].actionTime = Time.realtimeSinceStartup - fingers[fingerIndex].startTimeAction;
			if (fingers[fingerIndex].phase == TouchPhase.Canceled)
			{
				fingers[fingerIndex].gesture = GestureType.Cancel;
			}
			if (fingers[fingerIndex].phase != TouchPhase.Ended && fingers[fingerIndex].phase != TouchPhase.Canceled)
			{
				if (fingers[fingerIndex].phase == TouchPhase.Stationary && fingers[fingerIndex].actionTime >= longTapTime && fingers[fingerIndex].gesture == GestureType.Acquisition)
				{
					fingers[fingerIndex].gesture = GestureType.LongTap;
					CreateGesture(fingerIndex, EvtType.On_LongTapStart, fingers[fingerIndex], SwipeDirection.None, 0f, Vector2.zero);
				}
				if (((fingers[fingerIndex].gesture == GestureType.Acquisition || fingers[fingerIndex].gesture == GestureType.LongTap) && fingers[fingerIndex].phase == TouchPhase.Moved && gesturePriority == GesturePriority.Slips) || ((fingers[fingerIndex].gesture == GestureType.Acquisition || fingers[fingerIndex].gesture == GestureType.LongTap) && !FingerInTolerance(fingers[fingerIndex]) && gesturePriority == GesturePriority.Tap))
				{
					if (fingers[fingerIndex].gesture == GestureType.LongTap)
					{
						fingers[fingerIndex].gesture = GestureType.Cancel;
						CreateGesture(fingerIndex, EvtType.On_LongTapEnd, fingers[fingerIndex], SwipeDirection.None, 0f, Vector2.zero);
						fingers[fingerIndex].gesture = GestureType.Acquisition;
					}
					else
					{
						fingers[fingerIndex].oldSwipeType = SwipeDirection.None;
						if ((bool)fingers[fingerIndex].pickedObject)
						{
							fingers[fingerIndex].gesture = GestureType.Drag;
							CreateGesture(fingerIndex, EvtType.On_DragStart, fingers[fingerIndex], SwipeDirection.None, 0f, Vector2.zero);
							if (alwaysSendSwipe)
							{
								CreateGesture(fingerIndex, EvtType.On_SwipeStart, fingers[fingerIndex], SwipeDirection.None, 0f, Vector2.zero);
							}
						}
						else
						{
							fingers[fingerIndex].gesture = GestureType.Swipe;
							CreateGesture(fingerIndex, EvtType.On_SwipeStart, fingers[fingerIndex], SwipeDirection.None, 0f, Vector2.zero);
						}
					}
				}
				EvtType evtType = EvtType.None;
				switch (fingers[fingerIndex].gesture)
				{
				case GestureType.LongTap:
					evtType = EvtType.On_LongTap;
					break;
				case GestureType.Drag:
					evtType = EvtType.On_Drag;
					break;
				case GestureType.Swipe:
					evtType = EvtType.On_Swipe;
					break;
				}
				SwipeDirection swipeDirection = SwipeDirection.None;
				swipeDirection = GetSwipe(new Vector2(0f, 0f), fingers[fingerIndex].deltaPosition);
				if (evtType != EvtType.None)
				{
					fingers[fingerIndex].oldSwipeType = swipeDirection;
					CreateGesture(fingerIndex, evtType, fingers[fingerIndex], swipeDirection, 0f, fingers[fingerIndex].deltaPosition);
					if (evtType == EvtType.On_Drag && alwaysSendSwipe)
					{
						CreateGesture(fingerIndex, EvtType.On_Swipe, fingers[fingerIndex], swipeDirection, 0f, fingers[fingerIndex].deltaPosition);
					}
				}
				CreateGesture(fingerIndex, EvtType.On_TouchDown, fingers[fingerIndex], swipeDirection, 0f, fingers[fingerIndex].deltaPosition);
				return;
			}
			switch (fingers[fingerIndex].gesture)
			{
			case GestureType.Acquisition:
				if (doubleTapDetection == DoubleTapDetection.BySystem)
				{
					if (FingerInTolerance(fingers[fingerIndex]))
					{
						if (fingers[fingerIndex].tapCount < 2)
						{
							CreateGesture(fingerIndex, EvtType.On_SimpleTap, fingers[fingerIndex], SwipeDirection.None, 0f, Vector2.zero);
						}
						else
						{
							CreateGesture(fingerIndex, EvtType.On_DoubleTap, fingers[fingerIndex], SwipeDirection.None, 0f, Vector2.zero);
						}
					}
				}
				else if (!singleDoubleTap[fingerIndex].inWait)
				{
					singleDoubleTap[fingerIndex].finger = fingers[fingerIndex];
					StartCoroutine(SingleOrDouble(fingerIndex));
				}
				else
				{
					singleDoubleTap[fingerIndex].count++;
				}
				break;
			case GestureType.LongTap:
				CreateGesture(fingerIndex, EvtType.On_LongTapEnd, fingers[fingerIndex], SwipeDirection.None, 0f, Vector2.zero);
				break;
			case GestureType.Drag:
				CreateGesture(fingerIndex, EvtType.On_DragEnd, fingers[fingerIndex], GetSwipe(fingers[fingerIndex].startPosition, fingers[fingerIndex].position), (fingers[fingerIndex].startPosition - fingers[fingerIndex].position).magnitude, fingers[fingerIndex].position - fingers[fingerIndex].startPosition);
				if (alwaysSendSwipe)
				{
					CreateGesture(fingerIndex, EvtType.On_SwipeEnd, fingers[fingerIndex], GetSwipe(fingers[fingerIndex].startPosition, fingers[fingerIndex].position), (fingers[fingerIndex].position - fingers[fingerIndex].startPosition).magnitude, fingers[fingerIndex].position - fingers[fingerIndex].startPosition);
				}
				break;
			case GestureType.Swipe:
				CreateGesture(fingerIndex, EvtType.On_SwipeEnd, fingers[fingerIndex], GetSwipe(fingers[fingerIndex].startPosition, fingers[fingerIndex].position), (fingers[fingerIndex].position - fingers[fingerIndex].startPosition).magnitude, fingers[fingerIndex].position - fingers[fingerIndex].startPosition);
				break;
			case GestureType.Cancel:
				CreateGesture(fingerIndex, EvtType.On_Cancel, fingers[fingerIndex], SwipeDirection.None, 0f, Vector2.zero);
				break;
			}
			CreateGesture(fingerIndex, EvtType.On_TouchUp, fingers[fingerIndex], SwipeDirection.None, 0f, Vector2.zero);
			fingers[fingerIndex] = null;
		}

		private IEnumerator SingleOrDouble(int fingerIndex)
		{
			singleDoubleTap[fingerIndex].inWait = true;
			float time2Wait = doubleTapTime - singleDoubleTap[fingerIndex].finger.actionTime;
			if (time2Wait < 0f)
			{
				time2Wait = 0f;
			}
			yield return new WaitForSeconds(time2Wait);
			if (singleDoubleTap[fingerIndex].count < 2)
			{
				CreateGesture(fingerIndex, EvtType.On_SimpleTap, singleDoubleTap[fingerIndex].finger, SwipeDirection.None, 0f, singleDoubleTap[fingerIndex].finger.deltaPosition);
			}
			else
			{
				CreateGesture(fingerIndex, EvtType.On_DoubleTap, singleDoubleTap[fingerIndex].finger, SwipeDirection.None, 0f, singleDoubleTap[fingerIndex].finger.deltaPosition);
			}
			singleDoubleTap[fingerIndex].Stop();
			StopCoroutine("SingleOrDouble");
		}

		private void CreateGesture(int touchIndex, EvtType message, Finger finger, SwipeDirection swipe, float swipeLength, Vector2 swipeVector)
		{
			bool flag = true;
			if (autoUpdatePickedUI && allowUIDetection)
			{
				finger.isOverGui = IsScreenPositionOverUI(finger.position);
				finger.pickedUIElement = GetFirstUIElementFromCache();
			}
			if (enabledNGuiMode && message == EvtType.On_TouchStart)
			{
				finger.isOverGui = finger.isOverGui || IsTouchOverNGui(finger.position);
			}
			if (enableUIMode || enabledNGuiMode)
			{
				flag = !finger.isOverGui;
			}
			Gesture gesture = finger.GetGesture();
			if (autoUpdatePickedObject && autoSelect && message != EvtType.On_Drag && message != EvtType.On_DragEnd && message != EvtType.On_DragStart)
			{
				if (GetPickedGameObject(finger))
				{
					gesture.pickedObject = pickedObject.pickedObj;
					gesture.pickedCamera = pickedObject.pickedCamera;
					gesture.isGuiCamera = pickedObject.isGUI;
				}
				else
				{
					gesture.pickedObject = null;
					gesture.pickedCamera = null;
					gesture.isGuiCamera = false;
				}
			}
			gesture.swipe = swipe;
			gesture.swipeLength = swipeLength;
			gesture.swipeVector = swipeVector;
			gesture.deltaPinch = 0f;
			gesture.twistAngle = 0f;
			if (flag)
			{
				RaiseEvent(message, gesture);
			}
			else if (finger.isOverGui)
			{
				if (message == EvtType.On_TouchUp)
				{
					RaiseEvent(EvtType.On_UIElementTouchUp, gesture);
				}
				else
				{
					RaiseEvent(EvtType.On_OverUIElement, gesture);
				}
			}
		}

		private void TwoFinger()
		{
			bool flag = false;
			if (twoFinger.currentGesture == GestureType.None)
			{
				if (!singleDoubleTap[99].inDoubleTap)
				{
					singleDoubleTap[99].inDoubleTap = true;
					singleDoubleTap[99].time = 0f;
					singleDoubleTap[99].count = 1;
				}
				twoFinger.finger0 = GetTwoFinger(-1);
				twoFinger.finger1 = GetTwoFinger(twoFinger.finger0);
				twoFinger.startTimeAction = Time.realtimeSinceStartup;
				twoFinger.currentGesture = GestureType.Acquisition;
				fingers[twoFinger.finger0].startPosition = fingers[twoFinger.finger0].position;
				fingers[twoFinger.finger1].startPosition = fingers[twoFinger.finger1].position;
				fingers[twoFinger.finger0].oldPosition = fingers[twoFinger.finger0].position;
				fingers[twoFinger.finger1].oldPosition = fingers[twoFinger.finger1].position;
				twoFinger.oldFingerDistance = Mathf.Abs(Vector2.Distance(fingers[twoFinger.finger0].position, fingers[twoFinger.finger1].position));
				twoFinger.startPosition = new Vector2((fingers[twoFinger.finger0].position.x + fingers[twoFinger.finger1].position.x) / 2f, (fingers[twoFinger.finger0].position.y + fingers[twoFinger.finger1].position.y) / 2f);
				twoFinger.position = twoFinger.startPosition;
				twoFinger.oldStartPosition = twoFinger.startPosition;
				twoFinger.deltaPosition = Vector2.zero;
				twoFinger.startDistance = twoFinger.oldFingerDistance;
				if (autoSelect)
				{
					if (GetTwoFingerPickedObject())
					{
						twoFinger.pickedObject = pickedObject.pickedObj;
						twoFinger.pickedCamera = pickedObject.pickedCamera;
						twoFinger.isGuiCamera = pickedObject.isGUI;
					}
					else
					{
						twoFinger.ClearPickedObjectData();
					}
				}
				if (allowUIDetection)
				{
					if (GetTwoFingerPickedUIElement())
					{
						twoFinger.pickedUIElement = pickedObject.pickedObj;
						twoFinger.isOverGui = true;
					}
					else
					{
						twoFinger.ClearPickedUIData();
					}
				}
				CreateGesture2Finger(EvtType.On_TouchStart2Fingers, twoFinger.startPosition, twoFinger.startPosition, twoFinger.deltaPosition, twoFinger.timeSinceStartAction, SwipeDirection.None, 0f, Vector2.zero, 0f, 0f, twoFinger.oldFingerDistance);
			}
			if (singleDoubleTap[99].inDoubleTap)
			{
				singleDoubleTap[99].time += Time.deltaTime;
			}
			twoFinger.timeSinceStartAction = Time.realtimeSinceStartup - twoFinger.startTimeAction;
			twoFinger.position = new Vector2((fingers[twoFinger.finger0].position.x + fingers[twoFinger.finger1].position.x) / 2f, (fingers[twoFinger.finger0].position.y + fingers[twoFinger.finger1].position.y) / 2f);
			twoFinger.deltaPosition = twoFinger.position - twoFinger.oldStartPosition;
			twoFinger.fingerDistance = Mathf.Abs(Vector2.Distance(fingers[twoFinger.finger0].position, fingers[twoFinger.finger1].position));
			if (fingers[twoFinger.finger0].phase == TouchPhase.Canceled || fingers[twoFinger.finger1].phase == TouchPhase.Canceled)
			{
				twoFinger.currentGesture = GestureType.Cancel;
			}
			if (fingers[twoFinger.finger0].phase != TouchPhase.Ended && fingers[twoFinger.finger1].phase != TouchPhase.Ended && twoFinger.currentGesture != GestureType.Cancel)
			{
				if (twoFinger.currentGesture == GestureType.Acquisition && twoFinger.timeSinceStartAction >= longTapTime && FingerInTolerance(fingers[twoFinger.finger0]) && FingerInTolerance(fingers[twoFinger.finger1]))
				{
					twoFinger.currentGesture = GestureType.LongTap;
					CreateGesture2Finger(EvtType.On_LongTapStart2Fingers, twoFinger.startPosition, twoFinger.position, twoFinger.deltaPosition, twoFinger.timeSinceStartAction, SwipeDirection.None, 0f, Vector2.zero, 0f, 0f, twoFinger.fingerDistance);
				}
				if (((!FingerInTolerance(fingers[twoFinger.finger0]) || !FingerInTolerance(fingers[twoFinger.finger1])) && gesturePriority == GesturePriority.Tap) || ((fingers[twoFinger.finger0].phase == TouchPhase.Moved || fingers[twoFinger.finger1].phase == TouchPhase.Moved) && gesturePriority == GesturePriority.Slips))
				{
					flag = true;
				}
				if (flag && twoFinger.currentGesture != GestureType.Tap)
				{
					Vector2 currentDistance = fingers[twoFinger.finger0].position - fingers[twoFinger.finger1].position;
					Vector2 previousDistance = fingers[twoFinger.finger0].oldPosition - fingers[twoFinger.finger1].oldPosition;
					float currentDelta = currentDistance.magnitude - previousDistance.magnitude;
					if (enable2FingersSwipe)
					{
						float num = Vector2.Dot(fingers[twoFinger.finger0].deltaPosition.normalized, fingers[twoFinger.finger1].deltaPosition.normalized);
						if (num > 0f)
						{
							if (twoFinger.oldGesture == GestureType.LongTap)
							{
								CreateStateEnd2Fingers(twoFinger.currentGesture, twoFinger.startPosition, twoFinger.position, twoFinger.deltaPosition, twoFinger.timeSinceStartAction, false, twoFinger.fingerDistance);
								twoFinger.startTimeAction = Time.realtimeSinceStartup;
							}
							if ((bool)twoFinger.pickedObject && !twoFinger.dragStart && !alwaysSendSwipe)
							{
								twoFinger.currentGesture = GestureType.Drag;
								CreateGesture2Finger(EvtType.On_DragStart2Fingers, twoFinger.startPosition, twoFinger.position, twoFinger.deltaPosition, twoFinger.timeSinceStartAction, SwipeDirection.None, 0f, Vector2.zero, 0f, 0f, twoFinger.fingerDistance);
								CreateGesture2Finger(EvtType.On_SwipeStart2Fingers, twoFinger.startPosition, twoFinger.position, twoFinger.deltaPosition, twoFinger.timeSinceStartAction, SwipeDirection.None, 0f, Vector2.zero, 0f, 0f, twoFinger.fingerDistance);
								twoFinger.dragStart = true;
							}
							else if (!twoFinger.pickedObject && !twoFinger.swipeStart)
							{
								twoFinger.currentGesture = GestureType.Swipe;
								CreateGesture2Finger(EvtType.On_SwipeStart2Fingers, twoFinger.startPosition, twoFinger.position, twoFinger.deltaPosition, twoFinger.timeSinceStartAction, SwipeDirection.None, 0f, Vector2.zero, 0f, 0f, twoFinger.fingerDistance);
								twoFinger.swipeStart = true;
							}
						}
						else if (num < 0f)
						{
							twoFinger.dragStart = false;
							twoFinger.swipeStart = false;
						}
						if (twoFinger.dragStart)
						{
							CreateGesture2Finger(EvtType.On_Drag2Fingers, twoFinger.startPosition, twoFinger.position, twoFinger.deltaPosition, twoFinger.timeSinceStartAction, GetSwipe(twoFinger.oldStartPosition, twoFinger.position), 0f, twoFinger.deltaPosition, 0f, 0f, twoFinger.fingerDistance);
							CreateGesture2Finger(EvtType.On_Swipe2Fingers, twoFinger.startPosition, twoFinger.position, twoFinger.deltaPosition, twoFinger.timeSinceStartAction, GetSwipe(twoFinger.oldStartPosition, twoFinger.position), 0f, twoFinger.deltaPosition, 0f, 0f, twoFinger.fingerDistance);
						}
						if (twoFinger.swipeStart)
						{
							CreateGesture2Finger(EvtType.On_Swipe2Fingers, twoFinger.startPosition, twoFinger.position, twoFinger.deltaPosition, twoFinger.timeSinceStartAction, GetSwipe(twoFinger.oldStartPosition, twoFinger.position), 0f, twoFinger.deltaPosition, 0f, 0f, twoFinger.fingerDistance);
						}
					}
					DetectPinch(currentDelta);
					DetecTwist(previousDistance, currentDistance, currentDelta);
				}
				else if (twoFinger.currentGesture == GestureType.LongTap)
				{
					CreateGesture2Finger(EvtType.On_LongTap2Fingers, twoFinger.startPosition, twoFinger.position, twoFinger.deltaPosition, twoFinger.timeSinceStartAction, SwipeDirection.None, 0f, Vector2.zero, 0f, 0f, twoFinger.fingerDistance);
				}
				CreateGesture2Finger(EvtType.On_TouchDown2Fingers, twoFinger.startPosition, twoFinger.position, twoFinger.deltaPosition, twoFinger.timeSinceStartAction, GetSwipe(twoFinger.oldStartPosition, twoFinger.position), 0f, twoFinger.deltaPosition, 0f, 0f, twoFinger.fingerDistance);
				fingers[twoFinger.finger0].oldPosition = fingers[twoFinger.finger0].position;
				fingers[twoFinger.finger1].oldPosition = fingers[twoFinger.finger1].position;
				twoFinger.oldFingerDistance = twoFinger.fingerDistance;
				twoFinger.oldStartPosition = twoFinger.position;
				twoFinger.oldGesture = twoFinger.currentGesture;
			}
			else if (twoFinger.currentGesture != GestureType.Acquisition && twoFinger.currentGesture != GestureType.Tap)
			{
				CreateStateEnd2Fingers(twoFinger.currentGesture, twoFinger.startPosition, twoFinger.position, twoFinger.deltaPosition, twoFinger.timeSinceStartAction, true, twoFinger.fingerDistance);
				twoFinger.currentGesture = GestureType.None;
				twoFinger.pickedObject = null;
				twoFinger.swipeStart = false;
				twoFinger.dragStart = false;
			}
			else
			{
				twoFinger.currentGesture = GestureType.Tap;
				CreateStateEnd2Fingers(twoFinger.currentGesture, twoFinger.startPosition, twoFinger.position, twoFinger.deltaPosition, twoFinger.timeSinceStartAction, true, twoFinger.fingerDistance);
			}
		}

		private void DetectPinch(float currentDelta)
		{
			if (!enablePinch)
			{
				return;
			}
			if ((Mathf.Abs(twoFinger.fingerDistance - twoFinger.startDistance) >= minPinchLength && twoFinger.currentGesture != GestureType.Pinch) || twoFinger.currentGesture == GestureType.Pinch)
			{
				if (currentDelta != 0f && twoFinger.oldGesture == GestureType.LongTap)
				{
					CreateStateEnd2Fingers(twoFinger.currentGesture, twoFinger.startPosition, twoFinger.position, twoFinger.deltaPosition, twoFinger.timeSinceStartAction, false, twoFinger.fingerDistance);
					twoFinger.startTimeAction = Time.realtimeSinceStartup;
				}
				twoFinger.currentGesture = GestureType.Pinch;
				if (currentDelta > 0f)
				{
					CreateGesture2Finger(EvtType.On_PinchOut, twoFinger.startPosition, twoFinger.position, twoFinger.deltaPosition, twoFinger.timeSinceStartAction, GetSwipe(twoFinger.startPosition, twoFinger.position), 0f, Vector2.zero, 0f, Mathf.Abs(twoFinger.fingerDistance - twoFinger.oldFingerDistance), twoFinger.fingerDistance);
				}
				if (currentDelta < 0f)
				{
					CreateGesture2Finger(EvtType.On_PinchIn, twoFinger.startPosition, twoFinger.position, twoFinger.deltaPosition, twoFinger.timeSinceStartAction, GetSwipe(twoFinger.startPosition, twoFinger.position), 0f, Vector2.zero, 0f, Mathf.Abs(twoFinger.fingerDistance - twoFinger.oldFingerDistance), twoFinger.fingerDistance);
				}
				if (currentDelta < 0f || currentDelta > 0f)
				{
					CreateGesture2Finger(EvtType.On_Pinch, twoFinger.startPosition, twoFinger.position, twoFinger.deltaPosition, twoFinger.timeSinceStartAction, GetSwipe(twoFinger.startPosition, twoFinger.position), 0f, Vector2.zero, 0f, currentDelta, twoFinger.fingerDistance);
				}
			}
			twoFinger.lastPinch = ((!(currentDelta > 0f)) ? twoFinger.lastPinch : currentDelta);
		}

		private void DetecTwist(Vector2 previousDistance, Vector2 currentDistance, float currentDelta)
		{
			if (!enableTwist)
			{
				return;
			}
			float num = Vector2.Angle(previousDistance, currentDistance);
			if (previousDistance == currentDistance)
			{
				num = 0f;
			}
			if ((Mathf.Abs(num) >= minTwistAngle && twoFinger.currentGesture != GestureType.Twist) || twoFinger.currentGesture == GestureType.Twist)
			{
				if (twoFinger.oldGesture == GestureType.LongTap)
				{
					CreateStateEnd2Fingers(twoFinger.currentGesture, twoFinger.startPosition, twoFinger.position, twoFinger.deltaPosition, twoFinger.timeSinceStartAction, false, twoFinger.fingerDistance);
					twoFinger.startTimeAction = Time.realtimeSinceStartup;
				}
				twoFinger.currentGesture = GestureType.Twist;
				if (num != 0f)
				{
					num *= Mathf.Sign(Vector3.Cross(previousDistance, currentDistance).z);
				}
				CreateGesture2Finger(EvtType.On_Twist, twoFinger.startPosition, twoFinger.position, twoFinger.deltaPosition, twoFinger.timeSinceStartAction, SwipeDirection.None, 0f, Vector2.zero, num, 0f, twoFinger.fingerDistance);
			}
			twoFinger.lastTwistAngle = ((num == 0f) ? twoFinger.lastTwistAngle : num);
		}

		private void CreateStateEnd2Fingers(GestureType gesture, Vector2 startPosition, Vector2 position, Vector2 deltaPosition, float time, bool realEnd, float fingerDistance, float twist = 0f, float pinch = 0f)
		{
			switch (gesture)
			{
			case GestureType.Tap:
			case GestureType.Acquisition:
				if (doubleTapDetection == DoubleTapDetection.BySystem)
				{
					if (fingers[twoFinger.finger0].tapCount < 2 && fingers[twoFinger.finger1].tapCount < 2)
					{
						CreateGesture2Finger(EvtType.On_SimpleTap2Fingers, startPosition, position, deltaPosition, time, SwipeDirection.None, 0f, Vector2.zero, 0f, 0f, fingerDistance);
					}
					else
					{
						CreateGesture2Finger(EvtType.On_DoubleTap2Fingers, startPosition, position, deltaPosition, time, SwipeDirection.None, 0f, Vector2.zero, 0f, 0f, fingerDistance);
					}
					twoFinger.currentGesture = GestureType.None;
					twoFinger.pickedObject = null;
					twoFinger.swipeStart = false;
					twoFinger.dragStart = false;
					singleDoubleTap[99].Stop();
					StopCoroutine("SingleOrDouble2Fingers");
				}
				else if (!singleDoubleTap[99].inWait)
				{
					StartCoroutine("SingleOrDouble2Fingers");
				}
				else
				{
					singleDoubleTap[99].count++;
				}
				break;
			case GestureType.LongTap:
				CreateGesture2Finger(EvtType.On_LongTapEnd2Fingers, startPosition, position, deltaPosition, time, SwipeDirection.None, 0f, Vector2.zero, 0f, 0f, fingerDistance);
				break;
			case GestureType.Pinch:
				CreateGesture2Finger(EvtType.On_PinchEnd, startPosition, position, deltaPosition, time, SwipeDirection.None, 0f, Vector2.zero, 0f, twoFinger.lastPinch, fingerDistance);
				break;
			case GestureType.Twist:
				CreateGesture2Finger(EvtType.On_TwistEnd, startPosition, position, deltaPosition, time, SwipeDirection.None, 0f, Vector2.zero, twoFinger.lastTwistAngle, 0f, fingerDistance);
				break;
			}
			if (realEnd)
			{
				if (twoFinger.dragStart)
				{
					CreateGesture2Finger(EvtType.On_DragEnd2Fingers, startPosition, position, deltaPosition, time, GetSwipe(startPosition, position), (position - startPosition).magnitude, position - startPosition, 0f, 0f, fingerDistance);
				}
				if (twoFinger.swipeStart)
				{
					CreateGesture2Finger(EvtType.On_SwipeEnd2Fingers, startPosition, position, deltaPosition, time, GetSwipe(startPosition, position), (position - startPosition).magnitude, position - startPosition, 0f, 0f, fingerDistance);
				}
				CreateGesture2Finger(EvtType.On_TouchUp2Fingers, startPosition, position, deltaPosition, time, SwipeDirection.None, 0f, Vector2.zero, 0f, 0f, fingerDistance);
			}
		}

		private IEnumerator SingleOrDouble2Fingers()
		{
			singleDoubleTap[99].inWait = true;
			yield return new WaitForSeconds(doubleTapTime);
			if (singleDoubleTap[99].count < 2)
			{
				CreateGesture2Finger(EvtType.On_SimpleTap2Fingers, twoFinger.startPosition, twoFinger.position, twoFinger.deltaPosition, twoFinger.timeSinceStartAction, SwipeDirection.None, 0f, Vector2.zero, 0f, 0f, twoFinger.fingerDistance);
			}
			else
			{
				CreateGesture2Finger(EvtType.On_DoubleTap2Fingers, twoFinger.startPosition, twoFinger.position, twoFinger.deltaPosition, twoFinger.timeSinceStartAction, SwipeDirection.None, 0f, Vector2.zero, 0f, 0f, twoFinger.fingerDistance);
			}
			twoFinger.currentGesture = GestureType.None;
			twoFinger.pickedObject = null;
			twoFinger.swipeStart = false;
			twoFinger.dragStart = false;
			singleDoubleTap[99].Stop();
			StopCoroutine("SingleOrDouble2Fingers");
		}

		private void CreateGesture2Finger(EvtType message, Vector2 startPosition, Vector2 position, Vector2 deltaPosition, float actionTime, SwipeDirection swipe, float swipeLength, Vector2 swipeVector, float twist, float pinch, float twoDistance)
		{
			bool flag = true;
			Gesture gesture = new Gesture();
			gesture.isOverGui = false;
			if (enabledNGuiMode && message == EvtType.On_TouchStart2Fingers)
			{
				gesture.isOverGui = gesture.isOverGui || (IsTouchOverNGui(twoFinger.position) && IsTouchOverNGui(twoFinger.position));
			}
			gesture.touchCount = 2;
			gesture.fingerIndex = -1;
			gesture.startPosition = startPosition;
			gesture.position = position;
			gesture.deltaPosition = deltaPosition;
			gesture.actionTime = actionTime;
			gesture.deltaTime = Time.deltaTime;
			gesture.swipe = swipe;
			gesture.swipeLength = swipeLength;
			gesture.swipeVector = swipeVector;
			gesture.deltaPinch = pinch;
			gesture.twistAngle = twist;
			gesture.twoFingerDistance = twoDistance;
			gesture.pickedObject = twoFinger.pickedObject;
			gesture.pickedCamera = twoFinger.pickedCamera;
			gesture.isGuiCamera = twoFinger.isGuiCamera;
			if (autoUpdatePickedObject && message != EvtType.On_Drag && message != EvtType.On_DragEnd && message != EvtType.On_Twist && message != EvtType.On_TwistEnd && message != EvtType.On_Pinch && message != EvtType.On_PinchEnd && message != EvtType.On_PinchIn && message != EvtType.On_PinchOut)
			{
				if (GetTwoFingerPickedObject())
				{
					gesture.pickedObject = pickedObject.pickedObj;
					gesture.pickedCamera = pickedObject.pickedCamera;
					gesture.isGuiCamera = pickedObject.isGUI;
				}
				else
				{
					twoFinger.ClearPickedObjectData();
				}
			}
			gesture.pickedUIElement = twoFinger.pickedUIElement;
			gesture.isOverGui = twoFinger.isOverGui;
			if (allowUIDetection && autoUpdatePickedUI && message != EvtType.On_Drag && message != EvtType.On_DragEnd && message != EvtType.On_Twist && message != EvtType.On_TwistEnd && message != EvtType.On_Pinch && message != EvtType.On_PinchEnd && message != EvtType.On_PinchIn && message != EvtType.On_PinchOut && message == EvtType.On_SimpleTap2Fingers)
			{
				if (GetTwoFingerPickedUIElement())
				{
					gesture.pickedUIElement = pickedObject.pickedObj;
					gesture.isOverGui = true;
				}
				else
				{
					twoFinger.ClearPickedUIData();
				}
			}
			if (enableUIMode || (enabledNGuiMode && allowUIDetection))
			{
				flag = !gesture.isOverGui;
			}
			if (flag)
			{
				RaiseEvent(message, gesture);
			}
			else if (gesture.isOverGui)
			{
				if (message == EvtType.On_TouchUp2Fingers)
				{
					RaiseEvent(EvtType.On_UIElementTouchUp, gesture);
				}
				else
				{
					RaiseEvent(EvtType.On_OverUIElement, gesture);
				}
			}
		}

		private int GetTwoFinger(int index)
		{
			int i = index + 1;
			bool flag = false;
			for (; i < 10; i++)
			{
				if (flag)
				{
					break;
				}
				if (fingers[i] != null && i >= index)
				{
					flag = true;
				}
			}
			return i - 1;
		}

		private bool GetTwoFingerPickedObject()
		{
			bool result = false;
			if (twoFingerPickMethod == TwoFingerPickMethod.Finger)
			{
				if (GetPickedGameObject(fingers[twoFinger.finger0]))
				{
					GameObject pickedObj = pickedObject.pickedObj;
					if (GetPickedGameObject(fingers[twoFinger.finger1]) && pickedObj == pickedObject.pickedObj)
					{
						result = true;
					}
				}
			}
			else if (GetPickedGameObject(fingers[twoFinger.finger0], true))
			{
				result = true;
			}
			return result;
		}

		private bool GetTwoFingerPickedUIElement()
		{
			bool result = false;
			if (fingers[twoFinger.finger0] == null)
			{
				return false;
			}
			if (twoFingerPickMethod == TwoFingerPickMethod.Finger)
			{
				if (IsScreenPositionOverUI(fingers[twoFinger.finger0].position))
				{
					GameObject firstUIElementFromCache = GetFirstUIElementFromCache();
					if (IsScreenPositionOverUI(fingers[twoFinger.finger1].position))
					{
						GameObject firstUIElementFromCache2 = GetFirstUIElementFromCache();
						if (firstUIElementFromCache2 == firstUIElementFromCache || firstUIElementFromCache2.transform.IsChildOf(firstUIElementFromCache.transform) || firstUIElementFromCache.transform.IsChildOf(firstUIElementFromCache2.transform))
						{
							pickedObject.pickedObj = firstUIElementFromCache;
							pickedObject.isGUI = true;
							result = true;
						}
					}
				}
			}
			else if (IsScreenPositionOverUI(twoFinger.position))
			{
				pickedObject.pickedObj = GetFirstUIElementFromCache();
				pickedObject.isGUI = true;
				result = true;
			}
			return result;
		}

		private void RaiseEvent(EvtType evnt, Gesture gesture)
		{
			gesture.type = evnt;
			switch (evnt)
			{
			case EvtType.On_Cancel:
				if (On_Cancel__BackingField != null)
				{
					On_Cancel__BackingField(gesture);
				}
				break;
			case EvtType.On_Cancel2Fingers:
				if (On_Cancel2Fingers__BackingField != null)
				{
					On_Cancel2Fingers__BackingField(gesture);
				}
				break;
			case EvtType.On_TouchStart:
				if (On_TouchStart__BackingField != null)
				{
					On_TouchStart__BackingField(gesture);
				}
				break;
			case EvtType.On_TouchDown:
				if (On_TouchDown__BackingField != null)
				{
					On_TouchDown__BackingField(gesture);
				}
				break;
			case EvtType.On_TouchUp:
				if (On_TouchUp__BackingField != null)
				{
					On_TouchUp__BackingField(gesture);
				}
				break;
			case EvtType.On_SimpleTap:
				if (On_SimpleTap__BackingField != null)
				{
					On_SimpleTap__BackingField(gesture);
				}
				break;
			case EvtType.On_DoubleTap:
				if (On_DoubleTap__BackingField != null)
				{
					On_DoubleTap__BackingField(gesture);
				}
				break;
			case EvtType.On_LongTapStart:
				if (On_LongTapStart__BackingField != null)
				{
					On_LongTapStart__BackingField(gesture);
				}
				break;
			case EvtType.On_LongTap:
				if (On_LongTap__BackingField != null)
				{
					On_LongTap__BackingField(gesture);
				}
				break;
			case EvtType.On_LongTapEnd:
				if (On_LongTapEnd__BackingField != null)
				{
					On_LongTapEnd__BackingField(gesture);
				}
				break;
			case EvtType.On_DragStart:
				if (On_DragStart__BackingField != null)
				{
					On_DragStart__BackingField(gesture);
				}
				break;
			case EvtType.On_Drag:
				if (On_Drag__BackingField != null)
				{
					On_Drag__BackingField(gesture);
				}
				break;
			case EvtType.On_DragEnd:
				if (On_DragEnd__BackingField != null)
				{
					On_DragEnd__BackingField(gesture);
				}
				break;
			case EvtType.On_SwipeStart:
				if (On_SwipeStart__BackingField != null)
				{
					On_SwipeStart__BackingField(gesture);
				}
				break;
			case EvtType.On_Swipe:
				if (On_Swipe__BackingField != null)
				{
					On_Swipe__BackingField(gesture);
				}
				break;
			case EvtType.On_SwipeEnd:
				if (On_SwipeEnd__BackingField != null)
				{
					On_SwipeEnd__BackingField(gesture);
				}
				break;
			case EvtType.On_TouchStart2Fingers:
				if (On_TouchStart2Fingers__BackingField != null)
				{
					On_TouchStart2Fingers__BackingField(gesture);
				}
				break;
			case EvtType.On_TouchDown2Fingers:
				if (On_TouchDown2Fingers__BackingField != null)
				{
					On_TouchDown2Fingers__BackingField(gesture);
				}
				break;
			case EvtType.On_TouchUp2Fingers:
				if (On_TouchUp2Fingers__BackingField != null)
				{
					On_TouchUp2Fingers__BackingField(gesture);
				}
				break;
			case EvtType.On_SimpleTap2Fingers:
				if (On_SimpleTap2Fingers__BackingField != null)
				{
					On_SimpleTap2Fingers__BackingField(gesture);
				}
				break;
			case EvtType.On_DoubleTap2Fingers:
				if (On_DoubleTap2Fingers__BackingField != null)
				{
					On_DoubleTap2Fingers__BackingField(gesture);
				}
				break;
			case EvtType.On_LongTapStart2Fingers:
				if (On_LongTapStart2Fingers__BackingField != null)
				{
					On_LongTapStart2Fingers__BackingField(gesture);
				}
				break;
			case EvtType.On_LongTap2Fingers:
				if (On_LongTap2Fingers__BackingField != null)
				{
					On_LongTap2Fingers__BackingField(gesture);
				}
				break;
			case EvtType.On_LongTapEnd2Fingers:
				if (On_LongTapEnd2Fingers__BackingField != null)
				{
					On_LongTapEnd2Fingers__BackingField(gesture);
				}
				break;
			case EvtType.On_Twist:
				if (On_Twist__BackingField != null)
				{
					On_Twist__BackingField(gesture);
				}
				break;
			case EvtType.On_TwistEnd:
				if (On_TwistEnd__BackingField != null)
				{
					On_TwistEnd__BackingField(gesture);
				}
				break;
			case EvtType.On_Pinch:
				if (On_Pinch__BackingField != null)
				{
					On_Pinch__BackingField(gesture);
				}
				break;
			case EvtType.On_PinchIn:
				if (On_PinchIn__BackingField != null)
				{
					On_PinchIn__BackingField(gesture);
				}
				break;
			case EvtType.On_PinchOut:
				if (On_PinchOut__BackingField != null)
				{
					On_PinchOut__BackingField(gesture);
				}
				break;
			case EvtType.On_PinchEnd:
				if (On_PinchEnd__BackingField != null)
				{
					On_PinchEnd__BackingField(gesture);
				}
				break;
			case EvtType.On_DragStart2Fingers:
				if (On_DragStart2Fingers__BackingField != null)
				{
					On_DragStart2Fingers__BackingField(gesture);
				}
				break;
			case EvtType.On_Drag2Fingers:
				if (On_Drag2Fingers__BackingField != null)
				{
					On_Drag2Fingers__BackingField(gesture);
				}
				break;
			case EvtType.On_DragEnd2Fingers:
				if (On_DragEnd2Fingers__BackingField != null)
				{
					On_DragEnd2Fingers__BackingField(gesture);
				}
				break;
			case EvtType.On_SwipeStart2Fingers:
				if (On_SwipeStart2Fingers__BackingField != null)
				{
					On_SwipeStart2Fingers__BackingField(gesture);
				}
				break;
			case EvtType.On_Swipe2Fingers:
				if (On_Swipe2Fingers__BackingField != null)
				{
					On_Swipe2Fingers__BackingField(gesture);
				}
				break;
			case EvtType.On_SwipeEnd2Fingers:
				if (On_SwipeEnd2Fingers__BackingField != null)
				{
					On_SwipeEnd2Fingers__BackingField(gesture);
				}
				break;
			case EvtType.On_OverUIElement:
				if (On_OverUIElement__BackingField != null)
				{
					On_OverUIElement__BackingField(gesture);
				}
				break;
			case EvtType.On_UIElementTouchUp:
				if (On_UIElementTouchUp__BackingField != null)
				{
					On_UIElementTouchUp__BackingField(gesture);
				}
				break;
			}
			int num = _currentGestures.FindIndex((Gesture obj) => obj.type == gesture.type && obj.fingerIndex == gesture.fingerIndex);
			if (num > -1)
			{
				_currentGestures[num].touchCount = gesture.touchCount;
				_currentGestures[num].position = gesture.position;
				_currentGestures[num].actionTime = gesture.actionTime;
				_currentGestures[num].pickedCamera = gesture.pickedCamera;
				_currentGestures[num].pickedObject = gesture.pickedObject;
				_currentGestures[num].pickedUIElement = gesture.pickedUIElement;
				_currentGestures[num].isOverGui = gesture.isOverGui;
				_currentGestures[num].isGuiCamera = gesture.isGuiCamera;
				_currentGestures[num].deltaPinch += gesture.deltaPinch;
				_currentGestures[num].deltaPosition += gesture.deltaPosition;
				_currentGestures[num].deltaTime += gesture.deltaTime;
				_currentGestures[num].twistAngle += gesture.twistAngle;
			}
			if (num == -1)
			{
				_currentGestures.Add((Gesture)gesture.Clone());
				if (_currentGestures.Count > 0)
				{
					_currentGesture = _currentGestures[0];
				}
			}
		}

		private bool GetPickedGameObject(Finger finger, bool isTowFinger = false)
		{
			if (finger == null && !isTowFinger)
			{
				return false;
			}
			pickedObject.isGUI = false;
			pickedObject.pickedObj = null;
			pickedObject.pickedCamera = null;
			if (touchCameras.Count > 0)
			{
				for (int i = 0; i < touchCameras.Count; i++)
				{
					if (touchCameras[i].camera != null && touchCameras[i].camera.enabled)
					{
						Vector2 zero = Vector2.zero;
						zero = (isTowFinger ? twoFinger.position : finger.position);
						if (GetGameObjectAt(zero, touchCameras[i].camera, touchCameras[i].guiCamera))
						{
							return true;
						}
					}
				}
			}
			else
			{
				UnityEngine.Debug.LogWarning("No camera is assigned to EasyTouch");
			}
			return false;
		}

		private bool GetGameObjectAt(Vector2 position, Camera cam, bool isGuiCam)
		{
			Ray ray = cam.ScreenPointToRay(position);
			if (enable2D)
			{
				LayerMask layerMask = pickableLayers2D;
				RaycastHit2D[] array = new RaycastHit2D[1];
				if (Physics2D.GetRayIntersectionNonAlloc(ray, array, float.PositiveInfinity, layerMask) > 0)
				{
					pickedObject.pickedCamera = cam;
					pickedObject.isGUI = isGuiCam;
					pickedObject.pickedObj = array[0].collider.gameObject;
					return true;
				}
			}
			LayerMask layerMask2 = pickableLayers3D;
			RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo, float.MaxValue, layerMask2))
			{
				pickedObject.pickedCamera = cam;
				pickedObject.isGUI = isGuiCam;
				pickedObject.pickedObj = hitInfo.collider.gameObject;
				return true;
			}
			return false;
		}

		private SwipeDirection GetSwipe(Vector2 start, Vector2 end)
		{
			Vector2 normalized = (end - start).normalized;
			if (Vector2.Dot(normalized, Vector2.up) >= swipeTolerance)
			{
				return SwipeDirection.Up;
			}
			if (Vector2.Dot(normalized, -Vector2.up) >= swipeTolerance)
			{
				return SwipeDirection.Down;
			}
			if (Vector2.Dot(normalized, Vector2.right) >= swipeTolerance)
			{
				return SwipeDirection.Right;
			}
			if (Vector2.Dot(normalized, -Vector2.right) >= swipeTolerance)
			{
				return SwipeDirection.Left;
			}
			if (Vector2.Dot(normalized, new Vector2(0.5f, 0.5f).normalized) >= swipeTolerance)
			{
				return SwipeDirection.UpRight;
			}
			if (Vector2.Dot(normalized, new Vector2(0.5f, -0.5f).normalized) >= swipeTolerance)
			{
				return SwipeDirection.DownRight;
			}
			if (Vector2.Dot(normalized, new Vector2(-0.5f, 0.5f).normalized) >= swipeTolerance)
			{
				return SwipeDirection.UpLeft;
			}
			if (Vector2.Dot(normalized, new Vector2(-0.5f, -0.5f).normalized) >= swipeTolerance)
			{
				return SwipeDirection.DownLeft;
			}
			return SwipeDirection.Other;
		}

		private bool FingerInTolerance(Finger finger)
		{
			if ((finger.position - finger.startPosition).sqrMagnitude <= StationaryTolerance * StationaryTolerance)
			{
				return true;
			}
			return false;
		}

		private bool IsTouchOverNGui(Vector2 position, bool isTwoFingers = false)
		{
			bool flag = false;
			if (enabledNGuiMode)
			{
				LayerMask layerMask = nGUILayers;
				int num = 0;
				while (!flag && num < nGUICameras.Count)
				{
					Vector2 zero = Vector2.zero;
					zero = (isTwoFingers ? twoFinger.position : position);
					Ray ray = nGUICameras[num].ScreenPointToRay(zero);
					RaycastHit hitInfo;
					flag = Physics.Raycast(ray, out hitInfo, float.MaxValue, layerMask);
					num++;
				}
			}
			return flag;
		}

		private Finger GetFinger(int finderId)
		{
			int i = 0;
			Finger finger = null;
			for (; i < 10; i++)
			{
				if (finger != null)
				{
					break;
				}
				if (fingers[i] != null && fingers[i].fingerIndex == finderId)
				{
					finger = fingers[i];
				}
			}
			return finger;
		}

		private bool IsScreenPositionOverUI(Vector2 position)
		{
			uiEventSystem = EventSystem.current;
			if (uiEventSystem != null)
			{
				uiPointerEventData = new PointerEventData(uiEventSystem);
				uiPointerEventData.position = position;
				uiEventSystem.RaycastAll(uiPointerEventData, uiRaycastResultCache);
				if (uiRaycastResultCache.Count > 0)
				{
					return true;
				}
				return false;
			}
			return false;
		}

		private GameObject GetFirstUIElementFromCache()
		{
			if (uiRaycastResultCache.Count > 0)
			{
				return uiRaycastResultCache[0].gameObject;
			}
			return null;
		}

		private GameObject GetFirstUIElement(Vector2 position)
		{
			if (IsScreenPositionOverUI(position))
			{
				return GetFirstUIElementFromCache();
			}
			return null;
		}

		public static bool IsFingerOverUIElement(int fingerIndex)
		{
			if (instance != null)
			{
				Finger finger = instance.GetFinger(fingerIndex);
				if (finger != null)
				{
					return instance.IsScreenPositionOverUI(finger.position);
				}
				return false;
			}
			return false;
		}

		public static GameObject GetCurrentPickedUIElement(int fingerIndex, bool isTwoFinger)
		{
			if (instance != null)
			{
				Finger finger = instance.GetFinger(fingerIndex);
				if (finger != null || isTwoFinger)
				{
					Vector2 zero = Vector2.zero;
					zero = (isTwoFinger ? instance.twoFinger.position : finger.position);
					return instance.GetFirstUIElement(zero);
				}
				return null;
			}
			return null;
		}

		public static GameObject GetCurrentPickedObject(int fingerIndex, bool isTwoFinger)
		{
			if (instance != null)
			{
				Finger finger = instance.GetFinger(fingerIndex);
				if ((finger != null || isTwoFinger) && instance.GetPickedGameObject(finger, isTwoFinger))
				{
					return instance.pickedObject.pickedObj;
				}
				return null;
			}
			return null;
		}

		public static GameObject GetGameObjectAt(Vector2 position, bool isTwoFinger = false)
		{
			if (instance != null)
			{
				if (isTwoFinger)
				{
					position = instance.twoFinger.position;
				}
				if (instance.touchCameras.Count > 0)
				{
					for (int i = 0; i < instance.touchCameras.Count; i++)
					{
						if (instance.touchCameras[i].camera != null && instance.touchCameras[i].camera.enabled)
						{
							if (instance.GetGameObjectAt(position, instance.touchCameras[i].camera, instance.touchCameras[i].guiCamera))
							{
								return instance.pickedObject.pickedObj;
							}
							return null;
						}
					}
				}
			}
			return null;
		}

		public static int GetTouchCount()
		{
			if ((bool)instance)
			{
				return instance.input.TouchCount();
			}
			return 0;
		}

		public static void ResetTouch(int fingerIndex)
		{
			if ((bool)instance)
			{
				instance.GetFinger(fingerIndex).gesture = GestureType.None;
			}
		}

		public static void SetEnabled(bool enable)
		{
			instance.enable = enable;
			if (enable)
			{
				instance.ResetTouches();
			}
		}

		public static bool GetEnabled()
		{
			if ((bool)instance)
			{
				return instance.enable;
			}
			return false;
		}

		public static void SetEnableUIDetection(bool enable)
		{
			if (instance != null)
			{
				instance.allowUIDetection = enable;
			}
		}

		public static bool GetEnableUIDetection()
		{
			if ((bool)instance)
			{
				return instance.allowUIDetection;
			}
			return false;
		}

		public static void SetUICompatibily(bool value)
		{
			if (instance != null)
			{
				instance.enableUIMode = value;
			}
		}

		public static bool GetUIComptability()
		{
			if (instance != null)
			{
				return instance.enableUIMode;
			}
			return false;
		}

		public static void SetAutoUpdateUI(bool value)
		{
			if ((bool)instance)
			{
				instance.autoUpdatePickedUI = value;
			}
		}

		public static bool GetAutoUpdateUI()
		{
			if ((bool)instance)
			{
				return instance.autoUpdatePickedUI;
			}
			return false;
		}

		public static void SetNGUICompatibility(bool value)
		{
			if ((bool)instance)
			{
				instance.enabledNGuiMode = value;
			}
		}

		public static bool GetNGUICompatibility()
		{
			if ((bool)instance)
			{
				return instance.enabledNGuiMode;
			}
			return false;
		}

		public static void SetEnableAutoSelect(bool value)
		{
			if ((bool)instance)
			{
				instance.autoSelect = value;
			}
		}

		public static bool GetEnableAutoSelect()
		{
			if ((bool)instance)
			{
				return instance.autoSelect;
			}
			return false;
		}

		public static void SetAutoUpdatePickedObject(bool value)
		{
			if ((bool)instance)
			{
				instance.autoUpdatePickedObject = value;
			}
		}

		public static bool GetAutoUpdatePickedObject()
		{
			if ((bool)instance)
			{
				return instance.autoUpdatePickedObject;
			}
			return false;
		}

		public static void Set3DPickableLayer(LayerMask mask)
		{
			if ((bool)instance)
			{
				instance.pickableLayers3D = mask;
			}
		}

		public static LayerMask Get3DPickableLayer()
		{
			if ((bool)instance)
			{
				return instance.pickableLayers3D;
			}
			return LayerMask.GetMask("Default");
		}

		public static void AddCamera(Camera cam, bool guiCam = false)
		{
			if ((bool)instance)
			{
				instance.touchCameras.Add(new ECamera(cam, guiCam));
			}
		}

		public static void RemoveCamera(Camera cam)
		{
			if ((bool)instance)
			{
				int num = instance.touchCameras.FindIndex((ECamera c) => c.camera == cam);
				if (num > -1)
				{
					instance.touchCameras[num] = null;
					instance.touchCameras.RemoveAt(num);
				}
			}
		}

		public static Camera GetCamera(int index = 0)
		{
			if ((bool)instance)
			{
				if (index < instance.touchCameras.Count)
				{
					return instance.touchCameras[index].camera;
				}
				return null;
			}
			return null;
		}

		public static void SetEnable2DCollider(bool value)
		{
			if ((bool)instance)
			{
				instance.enable2D = value;
			}
		}

		public static bool GetEnable2DCollider()
		{
			if ((bool)instance)
			{
				return instance.enable2D;
			}
			return false;
		}

		public static void Set2DPickableLayer(LayerMask mask)
		{
			if ((bool)instance)
			{
				instance.pickableLayers2D = mask;
			}
		}

		public static LayerMask Get2DPickableLayer()
		{
			if ((bool)instance)
			{
				return instance.pickableLayers2D;
			}
			return LayerMask.GetMask("Default");
		}

		public static void SetGesturePriority(GesturePriority value)
		{
			if ((bool)instance)
			{
				instance.gesturePriority = value;
			}
		}

		public static GesturePriority GetGesturePriority()
		{
			if ((bool)instance)
			{
				return instance.gesturePriority;
			}
			return GesturePriority.Tap;
		}

		public static void SetStationaryTolerance(float tolerance)
		{
			if ((bool)instance)
			{
				instance.StationaryTolerance = tolerance;
			}
		}

		public static float GetStationaryTolerance()
		{
			if ((bool)instance)
			{
				return instance.StationaryTolerance;
			}
			return -1f;
		}

		public static void SetLongTapTime(float time)
		{
			if ((bool)instance)
			{
				instance.longTapTime = time;
			}
		}

		public static float GetlongTapTime()
		{
			if ((bool)instance)
			{
				return instance.longTapTime;
			}
			return -1f;
		}

		public static void SetDoubleTapTime(float time)
		{
			if ((bool)instance)
			{
				instance.doubleTapTime = time;
			}
		}

		public static float GetDoubleTapTime()
		{
			if ((bool)instance)
			{
				return instance.doubleTapTime;
			}
			return -1f;
		}

		public static void SetDoubleTapMethod(DoubleTapDetection detection)
		{
			if ((bool)instance)
			{
				instance.doubleTapDetection = detection;
			}
		}

		public static DoubleTapDetection GetDoubleTapMethod()
		{
			if ((bool)instance)
			{
				return instance.doubleTapDetection;
			}
			return DoubleTapDetection.BySystem;
		}

		public static void SetSwipeTolerance(float tolerance)
		{
			if ((bool)instance)
			{
				instance.swipeTolerance = tolerance;
			}
		}

		public static float GetSwipeTolerance()
		{
			if ((bool)instance)
			{
				return instance.swipeTolerance;
			}
			return -1f;
		}

		public static void SetEnable2FingersGesture(bool enable)
		{
			if ((bool)instance)
			{
				instance.enable2FingersGesture = enable;
			}
		}

		public static bool GetEnable2FingersGesture()
		{
			if ((bool)instance)
			{
				return instance.enable2FingersGesture;
			}
			return false;
		}

		public static void SetTwoFingerPickMethod(TwoFingerPickMethod pickMethod)
		{
			if ((bool)instance)
			{
				instance.twoFingerPickMethod = pickMethod;
			}
		}

		public static TwoFingerPickMethod GetTwoFingerPickMethod()
		{
			if ((bool)instance)
			{
				return instance.twoFingerPickMethod;
			}
			return TwoFingerPickMethod.Finger;
		}

		public static void SetEnablePinch(bool enable)
		{
			if ((bool)instance)
			{
				instance.enablePinch = enable;
			}
		}

		public static bool GetEnablePinch()
		{
			if ((bool)instance)
			{
				return instance.enablePinch;
			}
			return false;
		}

		public static void SetMinPinchLength(float length)
		{
			if ((bool)instance)
			{
				instance.minPinchLength = length;
			}
		}

		public static float GetMinPinchLength()
		{
			if ((bool)instance)
			{
				return instance.minPinchLength;
			}
			return -1f;
		}

		public static void SetEnableTwist(bool enable)
		{
			if ((bool)instance)
			{
				instance.enableTwist = enable;
			}
		}

		public static bool GetEnableTwist()
		{
			if ((bool)instance)
			{
				return instance.enableTwist;
			}
			return false;
		}

		public static void SetMinTwistAngle(float angle)
		{
			if ((bool)instance)
			{
				instance.minTwistAngle = angle;
			}
		}

		public static float GetMinTwistAngle()
		{
			if ((bool)instance)
			{
				return instance.minTwistAngle;
			}
			return -1f;
		}

		public static bool GetSecondeFingerSimulation()
		{
			if (instance != null)
			{
				return instance.enableSimulation;
			}
			return false;
		}

		public static void SetSecondFingerSimulation(bool value)
		{
			if (instance != null)
			{
				instance.enableSimulation = value;
			}
		}
	}
}

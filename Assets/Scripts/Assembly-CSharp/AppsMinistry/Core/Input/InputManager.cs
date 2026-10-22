using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core.Input.Strategy;
using AppsMinistry.UI;
using Slugterra.Scene;
using UnityEngine;

namespace AppsMinistry.Core.Input
{
	public class InputManager : DonDestroyMonoSingleton<InputManager>, IInputEvents
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Vector3> OnSwipeStart__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Vector2> OnSwipe__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Vector2> OnSwipeClick__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Vector3> OnSwipeEnd__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Vector2, Vector2> OnRoll__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Vector3> OnDragStart__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Vector2> OnDrag__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Vector3> OnDragEnd__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Vector3> OnTouchStart__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Vector3, float> OnTouch__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Vector3> OnTap__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Vector3, float> OnTouchEnd__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Vector3> OnDoubleTap__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<float> OnZoom__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Vector3> OnAcceleration__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<InputEvent> OnButtonDownEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<InputEvent> OnButtonPressEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<InputEvent> OnButtonUpEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Vector2> OnJoysticOnMove_movement__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnJoysticOnMoveEnd_movement__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Vector2> OnJoysticOnMove_rotation__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnJoysticOnMoveEnd_rotation__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnNextTarget__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnPrevTarget__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnBeginAttack__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnShowShop__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OpenMiniMapEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action SkipTutorEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action PauseEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action NextSlugEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action PreviousSlugEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action UseSlugEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action NextPowerUpEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action PreviousPowerUpEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action UsePowerUpEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action BackBtnEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnShowHelpForControllerEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnSkipMissionEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnAccelMoveRight__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnAccelMoveLeft__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<float> OnAccelerometerX__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<bool> OnChangeInputState__BackingField;

		private List<int> _ignoreLayer = new List<int>();

		private List<Camera> _ignoreCameras = new List<Camera>();

		private BaseInputStrategy _inputStrategy = new KeyboardInputStrategy();

		private BaseJoysticInputStrategy _joystickInputStrategy = new ETJoysticInputStrategy();

		private List<Type> _controllDisablers = new List<Type>();

		private bool connected;

		public BaseJoysticInputStrategy JoystickInputStrategy
		{
			get
			{
				return _joystickInputStrategy;
			}
		}

		public event Action<Vector3> OnSwipeStart
		{
			add
			{
				Action<Vector3> action = OnSwipeStart__BackingField;
				Action<Vector3> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnSwipeStart__BackingField, (Action<Vector3>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<Vector3> action = OnSwipeStart__BackingField;
				Action<Vector3> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnSwipeStart__BackingField, (Action<Vector3>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action<Vector2> OnSwipe
		{
			add
			{
				Action<Vector2> action = OnSwipe__BackingField;
				Action<Vector2> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnSwipe__BackingField, (Action<Vector2>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<Vector2> action = OnSwipe__BackingField;
				Action<Vector2> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnSwipe__BackingField, (Action<Vector2>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action<Vector2> OnSwipeClick
		{
			add
			{
				Action<Vector2> action = OnSwipeClick__BackingField;
				Action<Vector2> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnSwipeClick__BackingField, (Action<Vector2>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<Vector2> action = OnSwipeClick__BackingField;
				Action<Vector2> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnSwipeClick__BackingField, (Action<Vector2>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action<Vector3> OnSwipeEnd
		{
			add
			{
				Action<Vector3> action = OnSwipeEnd__BackingField;
				Action<Vector3> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnSwipeEnd__BackingField, (Action<Vector3>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<Vector3> action = OnSwipeEnd__BackingField;
				Action<Vector3> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnSwipeEnd__BackingField, (Action<Vector3>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action<Vector2, Vector2> OnRoll
		{
			add
			{
				Action<Vector2, Vector2> action = OnRoll__BackingField;
				Action<Vector2, Vector2> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnRoll__BackingField, (Action<Vector2, Vector2>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<Vector2, Vector2> action = OnRoll__BackingField;
				Action<Vector2, Vector2> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnRoll__BackingField, (Action<Vector2, Vector2>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action<Vector3> OnDragStart
		{
			add
			{
				Action<Vector3> action = OnDragStart__BackingField;
				Action<Vector3> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnDragStart__BackingField, (Action<Vector3>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<Vector3> action = OnDragStart__BackingField;
				Action<Vector3> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnDragStart__BackingField, (Action<Vector3>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action<Vector2> OnDrag
		{
			add
			{
				Action<Vector2> action = OnDrag__BackingField;
				Action<Vector2> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnDrag__BackingField, (Action<Vector2>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<Vector2> action = OnDrag__BackingField;
				Action<Vector2> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnDrag__BackingField, (Action<Vector2>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action<Vector3> OnDragEnd
		{
			add
			{
				Action<Vector3> action = OnDragEnd__BackingField;
				Action<Vector3> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnDragEnd__BackingField, (Action<Vector3>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<Vector3> action = OnDragEnd__BackingField;
				Action<Vector3> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnDragEnd__BackingField, (Action<Vector3>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action<Vector3> OnTouchStart
		{
			add
			{
				Action<Vector3> action = OnTouchStart__BackingField;
				Action<Vector3> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnTouchStart__BackingField, (Action<Vector3>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<Vector3> action = OnTouchStart__BackingField;
				Action<Vector3> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnTouchStart__BackingField, (Action<Vector3>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action<Vector3, float> OnTouch
		{
			add
			{
				Action<Vector3, float> action = OnTouch__BackingField;
				Action<Vector3, float> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnTouch__BackingField, (Action<Vector3, float>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<Vector3, float> action = OnTouch__BackingField;
				Action<Vector3, float> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnTouch__BackingField, (Action<Vector3, float>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action<Vector3> OnTap
		{
			add
			{
				Action<Vector3> action = OnTap__BackingField;
				Action<Vector3> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnTap__BackingField, (Action<Vector3>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<Vector3> action = OnTap__BackingField;
				Action<Vector3> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnTap__BackingField, (Action<Vector3>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action<Vector3, float> OnTouchEnd
		{
			add
			{
				Action<Vector3, float> action = OnTouchEnd__BackingField;
				Action<Vector3, float> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnTouchEnd__BackingField, (Action<Vector3, float>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<Vector3, float> action = OnTouchEnd__BackingField;
				Action<Vector3, float> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnTouchEnd__BackingField, (Action<Vector3, float>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action<Vector3> OnDoubleTap
		{
			add
			{
				Action<Vector3> action = OnDoubleTap__BackingField;
				Action<Vector3> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnDoubleTap__BackingField, (Action<Vector3>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<Vector3> action = OnDoubleTap__BackingField;
				Action<Vector3> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnDoubleTap__BackingField, (Action<Vector3>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action<float> OnZoom
		{
			add
			{
				Action<float> action = OnZoom__BackingField;
				Action<float> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnZoom__BackingField, (Action<float>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<float> action = OnZoom__BackingField;
				Action<float> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnZoom__BackingField, (Action<float>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action<Vector3> OnAcceleration
		{
			add
			{
				Action<Vector3> action = OnAcceleration__BackingField;
				Action<Vector3> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnAcceleration__BackingField, (Action<Vector3>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<Vector3> action = OnAcceleration__BackingField;
				Action<Vector3> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnAcceleration__BackingField, (Action<Vector3>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action<InputEvent> OnButtonDownEvent
		{
			add
			{
				Action<InputEvent> action = OnButtonDownEvent__BackingField;
				Action<InputEvent> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnButtonDownEvent__BackingField, (Action<InputEvent>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<InputEvent> action = OnButtonDownEvent__BackingField;
				Action<InputEvent> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnButtonDownEvent__BackingField, (Action<InputEvent>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action<InputEvent> OnButtonPressEvent
		{
			add
			{
				Action<InputEvent> action = OnButtonPressEvent__BackingField;
				Action<InputEvent> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnButtonPressEvent__BackingField, (Action<InputEvent>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<InputEvent> action = OnButtonPressEvent__BackingField;
				Action<InputEvent> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnButtonPressEvent__BackingField, (Action<InputEvent>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action<InputEvent> OnButtonUpEvent
		{
			add
			{
				Action<InputEvent> action = OnButtonUpEvent__BackingField;
				Action<InputEvent> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnButtonUpEvent__BackingField, (Action<InputEvent>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<InputEvent> action = OnButtonUpEvent__BackingField;
				Action<InputEvent> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnButtonUpEvent__BackingField, (Action<InputEvent>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action<Vector2> OnJoysticOnMove_movement
		{
			add
			{
				Action<Vector2> action = OnJoysticOnMove_movement__BackingField;
				Action<Vector2> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnJoysticOnMove_movement__BackingField, (Action<Vector2>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<Vector2> action = OnJoysticOnMove_movement__BackingField;
				Action<Vector2> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnJoysticOnMove_movement__BackingField, (Action<Vector2>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnJoysticOnMoveEnd_movement
		{
			add
			{
				Action action = OnJoysticOnMoveEnd_movement__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnJoysticOnMoveEnd_movement__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnJoysticOnMoveEnd_movement__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnJoysticOnMoveEnd_movement__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action<Vector2> OnJoysticOnMove_rotation
		{
			add
			{
				Action<Vector2> action = OnJoysticOnMove_rotation__BackingField;
				Action<Vector2> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnJoysticOnMove_rotation__BackingField, (Action<Vector2>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<Vector2> action = OnJoysticOnMove_rotation__BackingField;
				Action<Vector2> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnJoysticOnMove_rotation__BackingField, (Action<Vector2>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnJoysticOnMoveEnd_rotation
		{
			add
			{
				Action action = OnJoysticOnMoveEnd_rotation__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnJoysticOnMoveEnd_rotation__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnJoysticOnMoveEnd_rotation__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnJoysticOnMoveEnd_rotation__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnNextTarget
		{
			add
			{
				Action action = OnNextTarget__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnNextTarget__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnNextTarget__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnNextTarget__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnPrevTarget
		{
			add
			{
				Action action = OnPrevTarget__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPrevTarget__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnPrevTarget__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPrevTarget__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnBeginAttack
		{
			add
			{
				Action action = OnBeginAttack__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBeginAttack__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnBeginAttack__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBeginAttack__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnShowShop
		{
			add
			{
				Action action = OnShowShop__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnShowShop__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnShowShop__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnShowShop__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OpenMiniMapEvent
		{
			add
			{
				Action action = OpenMiniMapEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OpenMiniMapEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OpenMiniMapEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OpenMiniMapEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action SkipTutorEvent
		{
			add
			{
				Action action = SkipTutorEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref SkipTutorEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = SkipTutorEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref SkipTutorEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action PauseEvent
		{
			add
			{
				Action action = PauseEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref PauseEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = PauseEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref PauseEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action NextSlugEvent
		{
			add
			{
				Action action = NextSlugEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref NextSlugEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = NextSlugEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref NextSlugEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action PreviousSlugEvent
		{
			add
			{
				Action action = PreviousSlugEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref PreviousSlugEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = PreviousSlugEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref PreviousSlugEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action UseSlugEvent
		{
			add
			{
				Action action = UseSlugEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref UseSlugEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = UseSlugEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref UseSlugEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action NextPowerUpEvent
		{
			add
			{
				Action action = NextPowerUpEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref NextPowerUpEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = NextPowerUpEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref NextPowerUpEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action PreviousPowerUpEvent
		{
			add
			{
				Action action = PreviousPowerUpEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref PreviousPowerUpEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = PreviousPowerUpEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref PreviousPowerUpEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action UsePowerUpEvent
		{
			add
			{
				Action action = UsePowerUpEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref UsePowerUpEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = UsePowerUpEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref UsePowerUpEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action BackBtnEvent
		{
			add
			{
				Action action = BackBtnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref BackBtnEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = BackBtnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref BackBtnEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnShowHelpForControllerEvent
		{
			add
			{
				Action action = OnShowHelpForControllerEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnShowHelpForControllerEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnShowHelpForControllerEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnShowHelpForControllerEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnSkipMissionEvent
		{
			add
			{
				Action action = OnSkipMissionEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnSkipMissionEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnSkipMissionEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnSkipMissionEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnAccelMoveRight
		{
			add
			{
				Action action = OnAccelMoveRight__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnAccelMoveRight__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnAccelMoveRight__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnAccelMoveRight__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnAccelMoveLeft
		{
			add
			{
				Action action = OnAccelMoveLeft__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnAccelMoveLeft__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnAccelMoveLeft__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnAccelMoveLeft__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action<float> OnAccelerometerX
		{
			add
			{
				Action<float> action = OnAccelerometerX__BackingField;
				Action<float> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnAccelerometerX__BackingField, (Action<float>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<float> action = OnAccelerometerX__BackingField;
				Action<float> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnAccelerometerX__BackingField, (Action<float>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action<bool> OnChangeInputState
		{
			add
			{
				Action<bool> action = OnChangeInputState__BackingField;
				Action<bool> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnChangeInputState__BackingField, (Action<bool>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<bool> action = OnChangeInputState__BackingField;
				Action<bool> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnChangeInputState__BackingField, (Action<bool>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		protected override void OnCreate()
		{
			base.OnCreate();
			if (DonDestroyMonoSingleton<InputManager>.Instance != this)
			{
				UnityEngine.Object.Destroy(gameObject);
				return;
			}
			_inputStrategy.SetInputController(this);
			BindEvents();
			SceneLoader.OnSceneChangedEvent += FindIgnoreCamera;
			EnableInputEvents(GetType());
			StartCoroutine(CheckForControllers());
		}

		private void OnEnable()
		{
			if (_inputStrategy != null)
			{
				_inputStrategy.Activate();
			}
			if (_joystickInputStrategy != null)
			{
				_joystickInputStrategy.Activate();
			}
		}

		private void OnDisable()
		{
			if (_inputStrategy != null)
			{
				_inputStrategy.Deactivate();
			}
			if (_joystickInputStrategy != null)
			{
				_joystickInputStrategy.Activate();
			}
		}

		public void ChangeStrategy(BaseInputStrategy newStrategy)
		{
			_inputStrategy.Deactivate();
			UnBindEvents();
			_inputStrategy = newStrategy;
			_inputStrategy.SetInputController(this).Activate();
			BindEvents();
		}

		protected override void Update()
		{
			base.Update();
			if (_inputStrategy != null)
			{
				_inputStrategy.Update();
			}
			if (_joystickInputStrategy != null)
			{
				_joystickInputStrategy.Update();
			}
		}

		public void EnableInputEvents(Type type)
		{
			if (_controllDisablers.Contains(type))
			{
				_controllDisablers.Remove(type);
			}
			if (_controllDisablers.Count == 0)
			{
				EnableInputEvent(InputEvent.Press);
				EnableInputEvent(InputEvent.Move);
				EnableInputEvent(InputEvent.Release);
				EnableInputEvent(InputEvent.Tap);
				EnableInputEvent(InputEvent.DoubleTap);
				EnableInputEvent(InputEvent.Swipe);
				EnableInputEvent(InputEvent.Zoom);
				EnableInputEvent(InputEvent.Acceleration);
				EnableInputEvent(InputEvent.Drag);
				EnableInputEvent(InputEvent.Left);
				EnableInputEvent(InputEvent.Right);
				EnableInputEvent(InputEvent.Up);
				EnableInputEvent(InputEvent.Down);
				EnableInputEvent(InputEvent.Enter);
				EnableInputEvent(InputEvent.Plus);
				EnableInputEvent(InputEvent.Minus);
				EnableInputEvent(InputEvent.Attack);
				EnableInputEvent(InputEvent.CanUseSlug);
				EnableInputEvent(InputEvent.CanUsePowerUp);
				EnableInputEvent(InputEvent.CanMove);
				EnableInputEvent(InputEvent.OnMove);
				EnableInputEvent(InputEvent.OnMoveEnd);
				if (OnChangeInputState__BackingField != null)
				{
					OnChangeInputState__BackingField(true);
				}
			}
		}

		public void DisableInputEvents(Type type)
		{
			DisableInputEvent(InputEvent.Press);
			DisableInputEvent(InputEvent.Move);
			DisableInputEvent(InputEvent.Release);
			DisableInputEvent(InputEvent.Tap);
			DisableInputEvent(InputEvent.DoubleTap);
			DisableInputEvent(InputEvent.Swipe);
			DisableInputEvent(InputEvent.Zoom);
			DisableInputEvent(InputEvent.Acceleration);
			DisableInputEvent(InputEvent.Drag);
			DisableInputEvent(InputEvent.Left);
			DisableInputEvent(InputEvent.Right);
			DisableInputEvent(InputEvent.Up);
			DisableInputEvent(InputEvent.Down);
			DisableInputEvent(InputEvent.Plus);
			DisableInputEvent(InputEvent.Minus);
			DisableInputEvent(InputEvent.Attack);
			DisableInputEvent(InputEvent.CanMove);
			DisableInputEvent(InputEvent.OnMove);
			DisableInputEvent(InputEvent.OnMoveEnd);
			if (OnChangeInputState__BackingField != null)
			{
				OnChangeInputState__BackingField(false);
			}
			if (!_controllDisablers.Contains(type))
			{
				_controllDisablers.Add(type);
			}
		}

		public void EnableInputEvent(InputEvent inputEvent)
		{
			_inputStrategy.EnableEvent(inputEvent);
			_joystickInputStrategy.EnableEvent(inputEvent);
		}

		public void DisableInputEvent(InputEvent inputEvent)
		{
			_inputStrategy.DisableEvent(inputEvent);
			_joystickInputStrategy.DisableEvent(inputEvent);
		}

		public void RegisterIgnoreLayer(int layer)
		{
			AddIgnorLayer(layer);
		}

		public void Clear()
		{
		}

		public void AddIgnorLayer(int layer)
		{
			_ignoreLayer.Add(layer);
			FindIgnoreCamera();
		}

		public void FindIgnoreCamera()
		{
			_ignoreCameras.Clear();
			Camera[] array = UnityEngine.Object.FindObjectsOfType<Camera>();
			Camera[] array2 = array;
			foreach (Camera camera in array2)
			{
				if (_ignoreLayer.Contains(camera.gameObject.layer))
				{
					_ignoreCameras.Add(camera);
				}
			}
		}

		public bool IsTapOnIngnoreLayer(Vector3 pressPosition)
		{
			foreach (Camera ignoreCamera in _ignoreCameras)
			{
				if (!(ignoreCamera == null))
				{
					Ray ray = ignoreCamera.ScreenPointToRay(new Vector3(pressPosition.x, pressPosition.y, 0f));
					RaycastHit hitInfo;
					if (Physics.Raycast(ray, out hitInfo) && _ignoreLayer.Contains(hitInfo.collider.gameObject.layer))
					{
						return true;
					}
				}
			}
			return false;
		}

		protected override void OnReleaseResource()
		{
			base.OnReleaseResource();
			SceneLoader.OnSceneChangedEvent -= FindIgnoreCamera;
		}

		private void BindEvents()
		{
			_inputStrategy.OnSwipeStart += Use_OnSwipeStart;
			_inputStrategy.OnSwipe += Use_OnSwipe;
			_inputStrategy.OnSwipeClick += Use_OnSwipeClick;
			_inputStrategy.OnSwipeEnd += Use_OnSwipeEnd;
			_inputStrategy.OnDragStart += Use_OnDragStart;
			_inputStrategy.OnDrag += Use_OnDrag;
			_inputStrategy.OnDragEnd += Use_OnDragEnd;
			_inputStrategy.OnTouchStart += Use_OnTouchStart;
			_inputStrategy.OnTouch += Use_OnTouchUpdate;
			_inputStrategy.OnTap += Use_OnTap;
			_inputStrategy.OnTouchEnd += Use_OnTouchEnd;
			_inputStrategy.OnDoubleTap += Use_OnDoubleTap;
			_inputStrategy.OnZoom += Use_OnZoom;
			_inputStrategy.OnAcceleration += Use_OnAcceleration;
			_inputStrategy.OnButtonDownEvent += Use_OnButtonDown;
			_inputStrategy.OnButtonPressEvent += Use_OnButtonPress;
			_inputStrategy.OnButtonUpEvent += Use_OnButtonUp;
			_inputStrategy.SkipTutorBtnEvent += SkipTutor;
			_inputStrategy.OnBackBtnEvent += BackBtn;
			_inputStrategy.OpenMinimapBtnEvent += OpenMinimap;
			_inputStrategy.PauseBtnEvent += PlayPause;
			_inputStrategy.UseNextSlugBtnEvent += NextSlug;
			_inputStrategy.UsePreviousSlugBtnEvent += PreviousSlug;
			_inputStrategy.UseSlugBtnEvent += UseSlug;
			_inputStrategy.UseNextPowerUpBtnEvent += NextPowerUp;
			_inputStrategy.UsePreviousPowerUpBtnEvent += PreviousPowerUp;
			_inputStrategy.UsePowerUpBtnEvent += UsePowerUp;
			_inputStrategy.OnMoveEvent += Use_OnJoystickMoveEvent_movement;
			_inputStrategy.OnMoveEndEvent += Use_OnJoystickMoveEndEvent_movement;
			_inputStrategy.OnNextTargetBtnEvent += Use_OnButtonPlus;
			_inputStrategy.OnPrevTargetBtnEvent += Use_OnButtonMinus;
			_inputStrategy.OnBegineAttack += Use_OnBeginAttack;
			_inputStrategy.OnShowShop += Use_OnShowShop;
			_inputStrategy.OnAccelMoveRightBtnEvent += Use_OnButtonAccelMoveRight;
			_inputStrategy.OnAccelMoveLeftBtnEvent += Use_OnButtonAccelMoveLeft;
			_inputStrategy.OnAccelerometerXBtnEvent += Use_OnButtonAccelerometerX;
			_joystickInputStrategy.OnMoveEvent_movement += Use_OnJoystickMoveEvent_movement;
			_joystickInputStrategy.OnMoveEndEvent_movement += Use_OnJoystickMoveEndEvent_movement;
			_joystickInputStrategy.OnMoveEvent_rotation += Use_OnJoystickMoveEvent_rotation;
			_joystickInputStrategy.OnMoveEndEvent_rotation += Use_OnJoystickMoveEndEvent_rotation;
			_joystickInputStrategy.OnRollEvent += Use_OnRollEvent;
			_inputStrategy.OnShowHelpForControllerEvent += ShowHelpForController;
			_inputStrategy.OnSkipMissionEvent += SkipMission;
		}

		private void UnBindEvents()
		{
			_inputStrategy.OnSwipeStart -= Use_OnSwipeStart;
			_inputStrategy.OnSwipe -= Use_OnSwipe;
			_inputStrategy.OnSwipeClick -= Use_OnSwipeClick;
			_inputStrategy.OnSwipeEnd -= Use_OnSwipeEnd;
			_inputStrategy.OnDragStart -= Use_OnDragStart;
			_inputStrategy.OnDrag -= Use_OnDrag;
			_inputStrategy.OnDragEnd -= Use_OnDragEnd;
			_inputStrategy.OnTouchStart -= Use_OnTouchStart;
			_inputStrategy.OnTouch -= Use_OnTouchUpdate;
			_inputStrategy.OnTap -= Use_OnTap;
			_inputStrategy.OnTouchEnd -= Use_OnTouchEnd;
			_inputStrategy.OnDoubleTap -= Use_OnDoubleTap;
			_inputStrategy.OnZoom -= Use_OnZoom;
			_inputStrategy.OnAcceleration -= Use_OnAcceleration;
			_inputStrategy.OnButtonDownEvent -= Use_OnButtonDown;
			_inputStrategy.OnButtonPressEvent -= Use_OnButtonPress;
			_inputStrategy.OnButtonUpEvent -= Use_OnButtonUp;
			_inputStrategy.SkipTutorBtnEvent -= SkipTutor;
			_inputStrategy.OnBackBtnEvent -= BackBtn;
			_inputStrategy.OpenMinimapBtnEvent -= OpenMinimap;
			_inputStrategy.PauseBtnEvent -= PlayPause;
			_inputStrategy.UseNextSlugBtnEvent -= NextSlug;
			_inputStrategy.UsePreviousSlugBtnEvent -= PreviousSlug;
			_inputStrategy.UseSlugBtnEvent -= UseSlug;
			_inputStrategy.UseNextPowerUpBtnEvent -= NextPowerUp;
			_inputStrategy.UsePreviousPowerUpBtnEvent -= PreviousPowerUp;
			_inputStrategy.UsePowerUpBtnEvent -= UsePowerUp;
			_inputStrategy.OnMoveEvent -= Use_OnJoystickMoveEvent_movement;
			_inputStrategy.OnMoveEndEvent -= Use_OnJoystickMoveEndEvent_movement;
			_inputStrategy.OnNextTargetBtnEvent -= Use_OnButtonPlus;
			_inputStrategy.OnPrevTargetBtnEvent -= Use_OnButtonMinus;
			_inputStrategy.OnBegineAttack -= Use_OnBeginAttack;
			_inputStrategy.OnAccelMoveRightBtnEvent -= Use_OnButtonAccelMoveRight;
			_inputStrategy.OnAccelMoveLeftBtnEvent -= Use_OnButtonAccelMoveLeft;
			_inputStrategy.OnAccelerometerXBtnEvent -= Use_OnButtonAccelerometerX;
			_joystickInputStrategy.OnMoveEvent_movement -= Use_OnJoystickMoveEvent_movement;
			_joystickInputStrategy.OnMoveEndEvent_movement -= Use_OnJoystickMoveEndEvent_movement;
			_joystickInputStrategy.OnMoveEvent_rotation -= Use_OnJoystickMoveEvent_rotation;
			_joystickInputStrategy.OnMoveEndEvent_rotation -= Use_OnJoystickMoveEndEvent_rotation;
			_joystickInputStrategy.OnRollEvent -= Use_OnRollEvent;
			_inputStrategy.OnShowHelpForControllerEvent -= ShowHelpForController;
			_inputStrategy.OnSkipMissionEvent -= SkipMission;
		}

		protected void Use_OnSwipeStart(Vector3 position)
		{
			if (OnSwipeStart__BackingField != null)
			{
				OnSwipeStart__BackingField(position);
			}
		}

		protected void Use_OnSwipe(Vector2 swipeDir)
		{
			if (OnSwipe__BackingField != null)
			{
				OnSwipe__BackingField(swipeDir);
			}
		}

		protected void Use_OnSwipeClick(Vector2 swipeDir)
		{
			if (OnSwipeClick__BackingField != null)
			{
				OnSwipeClick__BackingField(swipeDir);
			}
		}

		protected void Use_OnSwipeEnd(Vector3 position)
		{
			if (OnSwipeEnd__BackingField != null)
			{
				OnSwipeEnd__BackingField(position);
			}
		}

		protected void Use_OnDoubleTap(Vector3 position)
		{
			if (OnDoubleTap__BackingField != null)
			{
				OnDoubleTap__BackingField(position);
			}
		}

		protected void Use_OnDragStart(Vector3 position)
		{
			if (OnDragStart__BackingField != null)
			{
				OnDragStart__BackingField(position);
			}
		}

		protected void Use_OnDrag(Vector2 position)
		{
			if (OnDrag__BackingField != null)
			{
				OnDrag__BackingField(position);
			}
		}

		protected void Use_OnDragEnd(Vector3 position)
		{
			if (OnDragEnd__BackingField != null)
			{
				OnDragEnd__BackingField(position);
			}
		}

		protected void Use_OnTouchStart(Vector3 position)
		{
			if (OnTouchStart__BackingField != null)
			{
				OnTouchStart__BackingField(position);
			}
		}

		protected void Use_OnTouchUpdate(Vector3 position, float actionTime)
		{
			if (OnTouch__BackingField != null)
			{
				OnTouch__BackingField(position, actionTime);
			}
		}

		protected void Use_OnTouchEnd(Vector3 position, float actionTime)
		{
			if (OnTouchEnd__BackingField != null)
			{
				OnTouchEnd__BackingField(position, actionTime);
			}
		}

		protected void Use_OnTap(Vector3 position)
		{
			if (OnTap__BackingField != null)
			{
				OnTap__BackingField(position);
			}
		}

		protected void Use_OnZoom(float zoom)
		{
			if (OnZoom__BackingField != null)
			{
				OnZoom__BackingField(zoom);
			}
		}

		protected void Use_OnAcceleration(Vector3 acceleration)
		{
			if (OnAcceleration__BackingField != null)
			{
				OnAcceleration__BackingField(acceleration);
			}
		}

		protected void Use_OnButtonDown(InputEvent e)
		{
			if (OnButtonDownEvent__BackingField != null)
			{
				OnButtonDownEvent__BackingField(e);
			}
		}

		protected void Use_OnButtonPress(InputEvent e)
		{
			if (OnButtonPressEvent__BackingField != null)
			{
				OnButtonPressEvent__BackingField(e);
			}
		}

		protected void Use_OnButtonUp(InputEvent e)
		{
			if (OnButtonUpEvent__BackingField != null)
			{
				OnButtonUpEvent__BackingField(e);
			}
		}

		protected void Use_OnButtonPlus()
		{
			if (OnNextTarget__BackingField != null)
			{
				OnNextTarget__BackingField();
			}
		}

		protected void Use_OnButtonMinus()
		{
			if (OnPrevTarget__BackingField != null)
			{
				OnPrevTarget__BackingField();
			}
		}

		protected void Use_OnButtonAccelMoveRight()
		{
			if (OnAccelMoveRight__BackingField != null)
			{
				OnAccelMoveRight__BackingField();
			}
		}

		protected void Use_OnButtonAccelMoveLeft()
		{
			if (OnAccelMoveLeft__BackingField != null)
			{
				OnAccelMoveLeft__BackingField();
			}
		}

		protected void Use_OnButtonAccelerometerX(float x)
		{
			if (OnAccelerometerX__BackingField != null)
			{
				OnAccelerometerX__BackingField(x);
			}
		}

		protected void Use_OnBeginAttack()
		{
			if (OnBeginAttack__BackingField != null)
			{
				OnBeginAttack__BackingField();
			}
		}

		protected void Use_OnShowShop()
		{
			if (OnShowShop__BackingField != null)
			{
				OnShowShop__BackingField();
			}
		}

		protected void Use_OnJoystickMoveEvent_movement(Vector2 axies)
		{
			if (OnJoysticOnMove_movement__BackingField != null)
			{
				OnJoysticOnMove_movement__BackingField(axies);
			}
		}

		protected void Use_OnJoystickMoveEndEvent_movement()
		{
			if (OnJoysticOnMoveEnd_movement__BackingField != null)
			{
				OnJoysticOnMoveEnd_movement__BackingField();
			}
		}

		protected void Use_OnJoystickMoveEvent_rotation(Vector2 axies)
		{
			if (OnJoysticOnMove_rotation__BackingField != null)
			{
				OnJoysticOnMove_rotation__BackingField(axies);
			}
		}

		protected void Use_OnJoystickMoveEndEvent_rotation()
		{
			if (OnJoysticOnMoveEnd_rotation__BackingField != null)
			{
				OnJoysticOnMoveEnd_rotation__BackingField();
			}
		}

		protected void Use_OnRollEvent(Vector2 dir, Vector2 move)
		{
			if (OnRoll__BackingField != null)
			{
				OnRoll__BackingField(dir, move);
			}
		}

		protected void OpenMinimap()
		{
			if (OpenMiniMapEvent__BackingField != null)
			{
				OpenMiniMapEvent__BackingField();
			}
		}

		protected void PlayPause()
		{
			if (PauseEvent__BackingField != null)
			{
				PauseEvent__BackingField();
			}
		}

		public void SkipTutor()
		{
			if (SkipTutorEvent__BackingField != null)
			{
				SkipTutorEvent__BackingField();
			}
		}

		protected void BackBtn()
		{
			if (DonDestroyMonoSingleton<PopupManager>.Instance.IsShowing)
			{
				DonDestroyMonoSingleton<PopupManager>.Instance.CloseCurrentPopup();
			}
			else if (BackBtnEvent__BackingField != null)
			{
				BackBtnEvent__BackingField();
			}
		}

		protected void NextSlug()
		{
			if (NextSlugEvent__BackingField != null)
			{
				NextSlugEvent__BackingField();
			}
		}

		protected void PreviousSlug()
		{
			if (PreviousSlugEvent__BackingField != null)
			{
				PreviousSlugEvent__BackingField();
			}
		}

		protected void UseSlug()
		{
			if (UseSlugEvent__BackingField != null)
			{
				UseSlugEvent__BackingField();
			}
		}

		protected void NextPowerUp()
		{
			if (NextPowerUpEvent__BackingField != null)
			{
				NextPowerUpEvent__BackingField();
			}
		}

		protected void PreviousPowerUp()
		{
			if (PreviousPowerUpEvent__BackingField != null)
			{
				PreviousPowerUpEvent__BackingField();
			}
		}

		protected void UsePowerUp()
		{
			if (UsePowerUpEvent__BackingField != null)
			{
				UsePowerUpEvent__BackingField();
			}
		}

		protected void ShowHelpForController()
		{
			if (OnShowHelpForControllerEvent__BackingField != null)
			{
				OnShowHelpForControllerEvent__BackingField();
			}
		}

		protected void SkipMission()
		{
			if (OnSkipMissionEvent__BackingField != null)
			{
				OnSkipMissionEvent__BackingField();
			}
		}

		private IEnumerator CheckForControllers()
		{
			while (true)
			{
				string[] controllers = UnityEngine.Input.GetJoystickNames();
				if (!connected && controllers.Length > 0)
				{
					connected = true;
					UnityEngine.Debug.LogFormat("<b><color=green>{0}</color></b>", "Joystick Connected");
				}
				else if (connected && controllers.Length == 0)
				{
					connected = false;
					UnityEngine.Debug.LogFormat("<b><color=green>{0}</color></b>", "Joystick Disconnected");
				}
				yield return new WaitForSeconds(1f);
			}
		}
	}
}

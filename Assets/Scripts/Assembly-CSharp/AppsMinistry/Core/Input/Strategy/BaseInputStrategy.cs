using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

namespace AppsMinistry.Core.Input.Strategy
{
	public abstract class BaseInputStrategy : IInputControllerStrategy, IInputEvents
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
		private Action<Vector3> OnDragStart__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Vector2> OnDrag__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Vector3> OnDragEnd__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Vector3> OnTouchStart__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Vector3> OnTap__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Vector3, float> OnTouch__BackingField;

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
		private Action<Vector2> OnMoveEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnMoveEndEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnPlayPauseBtnEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OpenMinimapBtnEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action PauseBtnEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action SkipTutorBtnEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action UseNextSlugBtnEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action UsePreviousSlugBtnEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action UseSlugBtnEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnShowHelpForControllerEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnSkipMissionEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action UseNextPowerUpBtnEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action UsePreviousPowerUpBtnEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action UsePowerUpBtnEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnNextBoostBtnEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnPrevBoostBtnEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnUseBoostBtnEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnBackBtnEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnNextSlugBtnEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnPrevSlugBtnEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnChargeSlugBtnEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnNextTargetBtnEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnPrevTargetBtnEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnBegineAttack__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnShowShop__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnAccelMoveRightBtnEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnAccelMoveLeftBtnEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<float> OnAccelerometerXBtnEvent__BackingField;

		protected List<InputEvent> _enableEvents = new List<InputEvent>();

		protected InputManager _inputManager;

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

		public event Action<Vector2> OnMoveEvent
		{
			add
			{
				Action<Vector2> action = OnMoveEvent__BackingField;
				Action<Vector2> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnMoveEvent__BackingField, (Action<Vector2>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<Vector2> action = OnMoveEvent__BackingField;
				Action<Vector2> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnMoveEvent__BackingField, (Action<Vector2>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnMoveEndEvent
		{
			add
			{
				Action action = OnMoveEndEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnMoveEndEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnMoveEndEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnMoveEndEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnPlayPauseBtnEvent
		{
			add
			{
				Action action = OnPlayPauseBtnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPlayPauseBtnEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnPlayPauseBtnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPlayPauseBtnEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OpenMinimapBtnEvent
		{
			add
			{
				Action action = OpenMinimapBtnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OpenMinimapBtnEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OpenMinimapBtnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OpenMinimapBtnEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action PauseBtnEvent
		{
			add
			{
				Action action = PauseBtnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref PauseBtnEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = PauseBtnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref PauseBtnEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action SkipTutorBtnEvent
		{
			add
			{
				Action action = SkipTutorBtnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref SkipTutorBtnEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = SkipTutorBtnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref SkipTutorBtnEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action UseNextSlugBtnEvent
		{
			add
			{
				Action action = UseNextSlugBtnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref UseNextSlugBtnEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = UseNextSlugBtnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref UseNextSlugBtnEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action UsePreviousSlugBtnEvent
		{
			add
			{
				Action action = UsePreviousSlugBtnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref UsePreviousSlugBtnEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = UsePreviousSlugBtnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref UsePreviousSlugBtnEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action UseSlugBtnEvent
		{
			add
			{
				Action action = UseSlugBtnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref UseSlugBtnEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = UseSlugBtnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref UseSlugBtnEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
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

		public event Action UseNextPowerUpBtnEvent
		{
			add
			{
				Action action = UseNextPowerUpBtnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref UseNextPowerUpBtnEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = UseNextPowerUpBtnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref UseNextPowerUpBtnEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action UsePreviousPowerUpBtnEvent
		{
			add
			{
				Action action = UsePreviousPowerUpBtnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref UsePreviousPowerUpBtnEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = UsePreviousPowerUpBtnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref UsePreviousPowerUpBtnEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action UsePowerUpBtnEvent
		{
			add
			{
				Action action = UsePowerUpBtnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref UsePowerUpBtnEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = UsePowerUpBtnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref UsePowerUpBtnEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnNextBoostBtnEvent
		{
			add
			{
				Action action = OnNextBoostBtnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnNextBoostBtnEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnNextBoostBtnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnNextBoostBtnEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnPrevBoostBtnEvent
		{
			add
			{
				Action action = OnPrevBoostBtnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPrevBoostBtnEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnPrevBoostBtnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPrevBoostBtnEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnUseBoostBtnEvent
		{
			add
			{
				Action action = OnUseBoostBtnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnUseBoostBtnEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnUseBoostBtnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnUseBoostBtnEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnBackBtnEvent
		{
			add
			{
				Action action = OnBackBtnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBackBtnEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnBackBtnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBackBtnEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnNextSlugBtnEvent
		{
			add
			{
				Action action = OnNextSlugBtnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnNextSlugBtnEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnNextSlugBtnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnNextSlugBtnEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnPrevSlugBtnEvent
		{
			add
			{
				Action action = OnPrevSlugBtnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPrevSlugBtnEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnPrevSlugBtnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPrevSlugBtnEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnChargeSlugBtnEvent
		{
			add
			{
				Action action = OnChargeSlugBtnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnChargeSlugBtnEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnChargeSlugBtnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnChargeSlugBtnEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnNextTargetBtnEvent
		{
			add
			{
				Action action = OnNextTargetBtnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnNextTargetBtnEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnNextTargetBtnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnNextTargetBtnEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnPrevTargetBtnEvent
		{
			add
			{
				Action action = OnPrevTargetBtnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPrevTargetBtnEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnPrevTargetBtnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPrevTargetBtnEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnBegineAttack
		{
			add
			{
				Action action = OnBegineAttack__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBegineAttack__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnBegineAttack__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBegineAttack__BackingField, (Action)Delegate.Remove(action2, value), action);
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

		public event Action OnAccelMoveRightBtnEvent
		{
			add
			{
				Action action = OnAccelMoveRightBtnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnAccelMoveRightBtnEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnAccelMoveRightBtnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnAccelMoveRightBtnEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnAccelMoveLeftBtnEvent
		{
			add
			{
				Action action = OnAccelMoveLeftBtnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnAccelMoveLeftBtnEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnAccelMoveLeftBtnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnAccelMoveLeftBtnEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action<float> OnAccelerometerXBtnEvent
		{
			add
			{
				Action<float> action = OnAccelerometerXBtnEvent__BackingField;
				Action<float> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnAccelerometerXBtnEvent__BackingField, (Action<float>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<float> action = OnAccelerometerXBtnEvent__BackingField;
				Action<float> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnAccelerometerXBtnEvent__BackingField, (Action<float>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public BaseInputStrategy SetInputController(InputManager inputManager)
		{
			_inputManager = inputManager;
			return this;
		}

		public virtual void Activate()
		{
		}

		public virtual void Deactivate()
		{
		}

		public virtual void Update()
		{
		}

		public void EnableEvent(InputEvent inputEvent)
		{
			if (!_enableEvents.Contains(inputEvent))
			{
				_enableEvents.Add(inputEvent);
			}
		}

		public void DisableEvent(InputEvent inputEvent)
		{
			if (_enableEvents.Contains(inputEvent))
			{
				_enableEvents.Remove(inputEvent);
			}
		}

		public bool IsEventEnabled(InputEvent inputEvent)
		{
			return _enableEvents.Contains(inputEvent);
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

		protected void Use_OnButtonUp(InputEvent e)
		{
			if (OnButtonUpEvent__BackingField != null)
			{
				OnButtonUpEvent__BackingField(e);
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
			if (IsEventEnabled(e) && OnButtonPressEvent__BackingField != null)
			{
				OnButtonPressEvent__BackingField(e);
			}
		}

		protected void Use_OnMoveEvent(Vector2 axies)
		{
			if (IsEventEnabled(InputEvent.OnMove) && OnMoveEvent__BackingField != null)
			{
				OnMoveEvent__BackingField(axies);
			}
		}

		protected void Use_OnMoveEndEvent()
		{
			if (IsEventEnabled(InputEvent.OnMoveEnd) && OnMoveEndEvent__BackingField != null)
			{
				OnMoveEndEvent__BackingField();
			}
		}

		protected void Use_OnNextTargetBtnEvent()
		{
			if (IsEventEnabled(InputEvent.OnMoveEnd) && OnNextTargetBtnEvent__BackingField != null)
			{
				OnNextTargetBtnEvent__BackingField();
			}
		}

		protected void Use_OnPrevTargetBtnEvent()
		{
			if (IsEventEnabled(InputEvent.OnMoveEnd) && OnPrevTargetBtnEvent__BackingField != null)
			{
				OnPrevTargetBtnEvent__BackingField();
			}
		}

		protected void Use_OnAccelMoveRight()
		{
			if (OnAccelMoveRightBtnEvent__BackingField != null)
			{
				OnAccelMoveRightBtnEvent__BackingField();
			}
		}

		protected void Use_OnAccelMoveLeft()
		{
			if (OnAccelMoveLeftBtnEvent__BackingField != null)
			{
				OnAccelMoveLeftBtnEvent__BackingField();
			}
		}

		protected void Use_OnAccelerometerX(float x)
		{
			if (OnAccelerometerXBtnEvent__BackingField != null)
			{
				OnAccelerometerXBtnEvent__BackingField(x);
			}
		}

		protected void Use_OnBegineAttack()
		{
			if (IsEventEnabled(InputEvent.OnMoveEnd) && OnBegineAttack__BackingField != null)
			{
				OnBegineAttack__BackingField();
			}
		}

		protected void OnOpenMinimap()
		{
			if (OpenMinimapBtnEvent__BackingField != null)
			{
				OpenMinimapBtnEvent__BackingField();
			}
		}

		protected void OnPause()
		{
			if (PauseBtnEvent__BackingField != null)
			{
				PauseBtnEvent__BackingField();
			}
		}

		protected void OnSkipTutor()
		{
			if (SkipTutorBtnEvent__BackingField != null)
			{
				SkipTutorBtnEvent__BackingField();
			}
		}

		protected void OnBackBtn()
		{
			if (OnBackBtnEvent__BackingField != null)
			{
				OnBackBtnEvent__BackingField();
			}
		}

		protected void OnUseNextSlug()
		{
			if (IsEventEnabled(InputEvent.CanUseSlug) && UseNextSlugBtnEvent__BackingField != null)
			{
				UseNextSlugBtnEvent__BackingField();
			}
		}

		protected void OnUsePreviousSlug()
		{
			if (IsEventEnabled(InputEvent.CanUseSlug) && UsePreviousSlugBtnEvent__BackingField != null)
			{
				UsePreviousSlugBtnEvent__BackingField();
			}
		}

		protected void OnUseSlug()
		{
			if (IsEventEnabled(InputEvent.CanUseSlug) && UseSlugBtnEvent__BackingField != null)
			{
				UseSlugBtnEvent__BackingField();
			}
		}

		protected void OnSkipMission()
		{
			if (OnSkipMissionEvent__BackingField != null)
			{
				OnSkipMissionEvent__BackingField();
			}
		}

		protected void OnShowHelpForController()
		{
			if (OnShowHelpForControllerEvent__BackingField != null)
			{
				OnShowHelpForControllerEvent__BackingField();
			}
		}

		protected void OnUseNextPowerUp()
		{
			if (UseNextPowerUpBtnEvent__BackingField != null)
			{
				UseNextPowerUpBtnEvent__BackingField();
			}
		}

		protected void OnUsePreviousPowerUp()
		{
			if (UsePreviousPowerUpBtnEvent__BackingField != null)
			{
				UsePreviousPowerUpBtnEvent__BackingField();
			}
		}

		protected void OnUsePowerUp()
		{
			if (IsEventEnabled(InputEvent.CanUsePowerUp) && UsePowerUpBtnEvent__BackingField != null)
			{
				UsePowerUpBtnEvent__BackingField();
			}
		}
	}
}

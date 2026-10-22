using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

namespace AppsMinistry.Core.Input
{
	public abstract class InputController : IInputController
	{
		private const float DETECT_SWIPE_TIME = 0.1f;

		private float _lastDetectSwipe;

		protected InputEvent _currentState;

		protected List<InputEvent> _availebleEvents;

		protected float _lastTapTime;

		protected Vector3 _pressPosition;

		protected float _pressTime;

		protected Vector3 _lastSwipePosition;

		protected Vector2 _swipeVelocity;

		protected float _baseZoomDistance;

		protected Vector3 _previousDragPos = Vector3.zero;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Vector3> OnPressEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Vector3> OnMoveEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Vector3> OnReleaseEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Vector3> OnTapEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Vector3> OnDoubleTapEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Vector2> OnSwipeEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<float> OnZoomEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Vector3> OnAccelerationEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Vector3> OnDragEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<InputEvent> OnButtonDownEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<InputEvent> OnButtonPressEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<InputEvent> OnButtonUpEvent__BackingField;

		protected virtual float DoubleTapTime
		{
			get
			{
				return 0.25f;
			}
		}

		protected virtual float MinSwipeDistance
		{
			get
			{
				return 10f;
			}
		}

		protected virtual float ZoomSensetive
		{
			get
			{
				return 0.005f;
			}
		}

		protected float CurrentTime
		{
			get
			{
				return Time.unscaledTime;
			}
		}

		protected virtual float MinDragTime
		{
			get
			{
				return 0.6f;
			}
		}

		public List<InputEvent> AvailebleEvents
		{
			get
			{
				return _availebleEvents;
			}
		}

		protected float MaxDrag
		{
			get
			{
				return 15f;
			}
		}

		public event Action<Vector3> OnPressEvent
		{
			add
			{
				Action<Vector3> action = OnPressEvent__BackingField;
				Action<Vector3> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPressEvent__BackingField, (Action<Vector3>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<Vector3> action = OnPressEvent__BackingField;
				Action<Vector3> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPressEvent__BackingField, (Action<Vector3>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action<Vector3> OnMoveEvent
		{
			add
			{
				Action<Vector3> action = OnMoveEvent__BackingField;
				Action<Vector3> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnMoveEvent__BackingField, (Action<Vector3>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<Vector3> action = OnMoveEvent__BackingField;
				Action<Vector3> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnMoveEvent__BackingField, (Action<Vector3>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action<Vector3> OnReleaseEvent
		{
			add
			{
				Action<Vector3> action = OnReleaseEvent__BackingField;
				Action<Vector3> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnReleaseEvent__BackingField, (Action<Vector3>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<Vector3> action = OnReleaseEvent__BackingField;
				Action<Vector3> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnReleaseEvent__BackingField, (Action<Vector3>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action<Vector3> OnTapEvent
		{
			add
			{
				Action<Vector3> action = OnTapEvent__BackingField;
				Action<Vector3> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnTapEvent__BackingField, (Action<Vector3>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<Vector3> action = OnTapEvent__BackingField;
				Action<Vector3> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnTapEvent__BackingField, (Action<Vector3>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action<Vector3> OnDoubleTapEvent
		{
			add
			{
				Action<Vector3> action = OnDoubleTapEvent__BackingField;
				Action<Vector3> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnDoubleTapEvent__BackingField, (Action<Vector3>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<Vector3> action = OnDoubleTapEvent__BackingField;
				Action<Vector3> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnDoubleTapEvent__BackingField, (Action<Vector3>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action<Vector2> OnSwipeEvent
		{
			add
			{
				Action<Vector2> action = OnSwipeEvent__BackingField;
				Action<Vector2> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnSwipeEvent__BackingField, (Action<Vector2>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<Vector2> action = OnSwipeEvent__BackingField;
				Action<Vector2> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnSwipeEvent__BackingField, (Action<Vector2>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action<float> OnZoomEvent
		{
			add
			{
				Action<float> action = OnZoomEvent__BackingField;
				Action<float> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnZoomEvent__BackingField, (Action<float>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<float> action = OnZoomEvent__BackingField;
				Action<float> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnZoomEvent__BackingField, (Action<float>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action<Vector3> OnAccelerationEvent
		{
			add
			{
				Action<Vector3> action = OnAccelerationEvent__BackingField;
				Action<Vector3> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnAccelerationEvent__BackingField, (Action<Vector3>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<Vector3> action = OnAccelerationEvent__BackingField;
				Action<Vector3> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnAccelerationEvent__BackingField, (Action<Vector3>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action<Vector3> OnDragEvent
		{
			add
			{
				Action<Vector3> action = OnDragEvent__BackingField;
				Action<Vector3> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnDragEvent__BackingField, (Action<Vector3>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<Vector3> action = OnDragEvent__BackingField;
				Action<Vector3> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnDragEvent__BackingField, (Action<Vector3>)Delegate.Remove(action2, value), action);
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

		public InputController()
		{
			_availebleEvents = new List<InputEvent>();
			_lastTapTime = 0f;
			_currentState = InputEvent.None;
			_swipeVelocity = Vector2.zero;
		}

		public abstract void OnUpdate();

		public void EnableEvent(InputEvent inputEvent)
		{
			if (!_availebleEvents.Contains(inputEvent))
			{
				_availebleEvents.Add(inputEvent);
			}
		}

		public void DisableEvent(InputEvent inputEvent)
		{
			if (_availebleEvents.Contains(inputEvent))
			{
				_availebleEvents.Remove(inputEvent);
			}
		}

		public void Clear()
		{
			_lastTapTime = 0f;
			_currentState = InputEvent.None;
			_pressPosition = Vector3.zero;
			_lastSwipePosition = Vector3.zero;
			_lastDetectSwipe = 0f;
			_pressTime = 0f;
		}

		protected void Tap(Vector3 position)
		{
			_currentState = InputEvent.Tap;
			if (CurrentTime - _lastTapTime <= DoubleTapTime && _availebleEvents.Contains(InputEvent.DoubleTap))
			{
				_currentState = InputEvent.DoubleTap;
			}
			_lastTapTime = CurrentTime;
			_pressPosition = position;
			_lastSwipePosition = _pressPosition;
			_pressTime = CurrentTime;
			_lastDetectSwipe = CurrentTime + 0.1f;
			if (!DonDestroyMonoSingleton<InputManager>.Instance.IsTapOnIngnoreLayer(position))
			{
				OnPress(position);
			}
		}

		protected void Swipe(Vector3 position)
		{
			float magnitude = (position - _lastSwipePosition).magnitude;
			if (magnitude > MinSwipeDistance)
			{
				_currentState = InputEvent.Swipe;
				_swipeVelocity = position - _lastSwipePosition;
			}
			else
			{
				_swipeVelocity = Vector2.zero;
			}
		}

		protected void Drag(Vector3 position)
		{
			if (!(CurrentTime - _pressTime < MinDragTime))
			{
				if (_previousDragPos == Vector3.zero)
				{
					_previousDragPos = position;
				}
				Vector3 vector = position - _previousDragPos;
				Vector3 velocity = new Vector3(vector.x, 0f, vector.y);
				velocity.x = ((!(Mathf.Abs(velocity.x) < MaxDrag)) ? (MaxDrag * Mathf.Sign(velocity.x)) : velocity.x);
				velocity.z = ((!(Mathf.Abs(velocity.z) < MaxDrag)) ? (MaxDrag * Mathf.Sign(velocity.z)) : velocity.z);
				_previousDragPos = position;
				OnDrag(velocity);
			}
		}

		protected void EndInput(Vector3 position)
		{
			if (DonDestroyMonoSingleton<InputManager>.Instance.IsTapOnIngnoreLayer(position))
			{
				_currentState = InputEvent.None;
			}
			else
			{
				if (_currentState != InputEvent.None && _currentState != InputEvent.Zoom)
				{
					Swipe(position);
				}
				if (CurrentTime - _pressTime > MinDragTime && OnDragEvent__BackingField != null)
				{
					_previousDragPos = Vector3.zero;
				}
				else if (_currentState == InputEvent.Swipe)
				{
					OnSwipe(_swipeVelocity);
				}
				else if (_currentState == InputEvent.Tap)
				{
					OnTap(position);
				}
				else if (_currentState == InputEvent.DoubleTap)
				{
					OnDoubleTap(position);
				}
				else if (_currentState == InputEvent.Zoom)
				{
					_currentState = InputEvent.None;
				}
			}
			OnRelease(position);
		}

		protected void OnPress(Vector3 position)
		{
			if (OnPressEvent__BackingField != null && _availebleEvents.Contains(InputEvent.Press))
			{
				OnPressEvent__BackingField(position);
			}
		}

		protected void OnMove(Vector3 position)
		{
			if (_lastDetectSwipe <= CurrentTime)
			{
				_lastSwipePosition = position;
				_lastDetectSwipe = CurrentTime + 0.1f;
			}
			if (OnMoveEvent__BackingField != null && _availebleEvents.Contains(InputEvent.Move))
			{
				OnMoveEvent__BackingField(position);
			}
		}

		protected void OnRelease(Vector3 position)
		{
			if (OnReleaseEvent__BackingField != null && _availebleEvents.Contains(InputEvent.Release))
			{
				OnReleaseEvent__BackingField(position);
			}
		}

		protected void OnTap(Vector3 position)
		{
			if (OnTapEvent__BackingField != null && _availebleEvents.Contains(InputEvent.Tap))
			{
				OnTapEvent__BackingField(position);
			}
			_currentState = InputEvent.None;
		}

		protected void OnDoubleTap(Vector3 position)
		{
			if (OnDoubleTapEvent__BackingField != null && _availebleEvents.Contains(InputEvent.DoubleTap))
			{
				OnDoubleTapEvent__BackingField(position);
			}
			_currentState = InputEvent.None;
		}

		protected void OnSwipe(Vector2 velocity)
		{
			UnityEngine.Debug.Log("InputController::OnSwipe");
			if (OnSwipeEvent__BackingField != null && _availebleEvents.Contains(InputEvent.Swipe))
			{
				OnSwipeEvent__BackingField(velocity);
			}
			_currentState = InputEvent.None;
		}

		protected void OnZoom(float zoom)
		{
			if (OnZoomEvent__BackingField != null && _availebleEvents.Contains(InputEvent.Zoom))
			{
				OnZoomEvent__BackingField(zoom);
			}
		}

		protected void OnAcceleration(Vector3 acceleration)
		{
			if (OnAccelerationEvent__BackingField != null && _availebleEvents.Contains(InputEvent.Acceleration))
			{
				OnAccelerationEvent__BackingField(acceleration);
			}
		}

		protected void OnDrag(Vector3 velocity)
		{
			if (OnDragEvent__BackingField != null && _availebleEvents.Contains(InputEvent.Swipe))
			{
				OnDragEvent__BackingField(velocity);
			}
		}

		protected void OnButtonUp(InputEvent e)
		{
			if (OnButtonUpEvent__BackingField != null && _availebleEvents.Contains(e))
			{
				OnButtonUpEvent__BackingField(e);
			}
		}

		protected void OnButtonDown(InputEvent e)
		{
			if (OnButtonDownEvent__BackingField != null && _availebleEvents.Contains(e))
			{
				OnButtonDownEvent__BackingField(e);
			}
		}

		protected void OnButtonPress(InputEvent e)
		{
			if (OnButtonPressEvent__BackingField != null && _availebleEvents.Contains(e))
			{
				OnButtonPressEvent__BackingField(e);
			}
		}
	}
}

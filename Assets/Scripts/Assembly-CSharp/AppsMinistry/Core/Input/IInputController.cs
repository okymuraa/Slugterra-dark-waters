using System;
using System.Collections.Generic;
using UnityEngine;

namespace AppsMinistry.Core.Input
{
	public interface IInputController
	{
		List<InputEvent> AvailebleEvents { get; }

		event Action<Vector3> OnPressEvent;

		event Action<Vector3> OnMoveEvent;

		event Action<Vector3> OnReleaseEvent;

		event Action<Vector3> OnTapEvent;

		event Action<Vector3> OnDoubleTapEvent;

		event Action<Vector2> OnSwipeEvent;

		event Action<float> OnZoomEvent;

		event Action<Vector3> OnAccelerationEvent;

		event Action<Vector3> OnDragEvent;

		event Action<InputEvent> OnButtonDownEvent;

		event Action<InputEvent> OnButtonPressEvent;

		event Action<InputEvent> OnButtonUpEvent;

		void OnUpdate();

		void EnableEvent(InputEvent inputEvent);

		void DisableEvent(InputEvent inputEvent);

		void Clear();
	}
}

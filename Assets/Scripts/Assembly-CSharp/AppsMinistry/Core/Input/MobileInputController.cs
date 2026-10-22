using UnityEngine;

namespace AppsMinistry.Core.Input
{
	public class MobileInputController : InputController
	{
		public override void OnUpdate()
		{
			if (UnityEngine.Input.touchCount == 1)
			{
				Touch touch = UnityEngine.Input.touches[0];
				if (touch.phase == TouchPhase.Began)
				{
					Tap(touch.position);
				}
				else if (touch.phase == TouchPhase.Moved)
				{
					OnMove(touch.position);
					Drag(touch.position);
				}
				else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
				{
					EndInput(touch.position);
				}
			}
			else if (UnityEngine.Input.touchCount == 2)
			{
				Touch touch2 = UnityEngine.Input.touches[0];
				Touch touch3 = UnityEngine.Input.touches[1];
				if (touch2.phase == TouchPhase.Began || touch3.phase == TouchPhase.Began)
				{
					_baseZoomDistance = Mathf.Abs(Vector2.SqrMagnitude(touch2.position - touch3.position));
				}
				else if (touch2.phase == TouchPhase.Moved || touch3.phase == TouchPhase.Moved)
				{
					float num = Mathf.Abs(Vector2.SqrMagnitude(touch2.position - touch3.position));
					_currentState = InputEvent.Zoom;
					OnZoom((num - _baseZoomDistance) * ZoomSensetive);
					_baseZoomDistance = num;
				}
			}
			OnAcceleration(UnityEngine.Input.acceleration);
		}
	}
}

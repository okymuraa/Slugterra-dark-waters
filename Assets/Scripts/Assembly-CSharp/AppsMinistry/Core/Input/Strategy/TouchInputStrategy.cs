using HedgehogTeam.EasyTouch;
using UnityEngine;

namespace AppsMinistry.Core.Input.Strategy
{
	public class TouchInputStrategy : BaseInputStrategy
	{
		private EasyTouch easyTouch;

		public override void Activate()
		{
			CreateEasyTouch();
			EasyTouch.On_SwipeStart += On_SwipeStart;
			EasyTouch.On_Swipe += On_Swipe;
			EasyTouch.On_SwipeEnd += On_SwipeEnd;
			EasyTouch.On_DoubleTap += On_DoubleTap;
			EasyTouch.On_DragStart += On_DragStart;
			EasyTouch.On_Drag += On_Drag;
			EasyTouch.On_DragEnd += On_DragEnd;
			EasyTouch.On_TouchStart += On_TouchStart;
			EasyTouch.On_TouchDown += On_TouchUpdate;
			EasyTouch.On_TouchUp += On_TouchEnd;
			EasyTouch.On_TouchStart2Fingers += On_TouchStart2Fingers;
			EasyTouch.On_PinchIn += On_PinchIn;
			EasyTouch.On_PinchOut += On_PinchOut;
			EasyTouch.On_PinchEnd += On_PinchEnd;
			EasyTouch.On_Cancel2Fingers += On_Cancel2Fingers;
		}

		private void CreateEasyTouch()
		{
			easyTouch = Object.FindObjectOfType<EasyTouch>();
			if (easyTouch == null)
			{
				easyTouch = new GameObject("EasyTouch").AddComponent<EasyTouch>();
				easyTouch.transform.parent = DonDestroyMonoSingleton<InputManager>.Instance.transform;
			}
		}

		private void DestroyEasyTouch()
		{
			if (easyTouch != null)
			{
				Object.Destroy(easyTouch);
			}
		}

		public override void Deactivate()
		{
			EasyTouch.On_SwipeStart -= On_SwipeStart;
			EasyTouch.On_Swipe -= On_Swipe;
			EasyTouch.On_SwipeEnd -= On_SwipeEnd;
			EasyTouch.On_DoubleTap -= On_DoubleTap;
			EasyTouch.On_DragStart -= On_DragStart;
			EasyTouch.On_Drag -= On_Drag;
			EasyTouch.On_DragEnd -= On_DragEnd;
			EasyTouch.On_TouchStart -= On_TouchStart;
			EasyTouch.On_TouchDown -= On_TouchUpdate;
			EasyTouch.On_TouchUp -= On_TouchEnd;
			EasyTouch.On_TouchStart2Fingers -= On_TouchStart2Fingers;
			EasyTouch.On_PinchIn -= On_PinchIn;
			EasyTouch.On_PinchOut -= On_PinchOut;
			EasyTouch.On_PinchEnd -= On_PinchEnd;
			EasyTouch.On_Cancel2Fingers -= On_Cancel2Fingers;
			DestroyEasyTouch();
		}

		private void On_SwipeStart(Gesture gesture)
		{
			if (IsEventEnabled(InputEvent.Swipe))
			{
				Use_OnSwipeStart(gesture.position);
			}
		}

		private void On_Swipe(Gesture gesture)
		{
			if (IsEventEnabled(InputEvent.Swipe))
			{
				Use_OnSwipe(gesture.swipeVector);
			}
		}

		private void On_SwipeEnd(Gesture gesture)
		{
			if (IsEventEnabled(InputEvent.Swipe))
			{
				Use_OnSwipeEnd(gesture.position);
			}
		}

		private void On_DoubleTap(Gesture gesture)
		{
			if (IsEventEnabled(InputEvent.DoubleTap) && !_inputManager.IsTapOnIngnoreLayer(gesture.position))
			{
				Use_OnDoubleTap(gesture.position);
			}
		}

		private void On_DragStart(Gesture gesture)
		{
			if (IsEventEnabled(InputEvent.Drag))
			{
				Use_OnDragStart(gesture.position);
			}
		}

		private void On_Drag(Gesture gesture)
		{
			if (IsEventEnabled(InputEvent.Drag))
			{
				Use_OnDrag(gesture.position);
			}
		}

		private void On_DragEnd(Gesture gesture)
		{
			if (IsEventEnabled(InputEvent.Drag))
			{
				Use_OnDragEnd(gesture.position);
			}
		}

		private void On_TouchStart(Gesture gesture)
		{
			if ((IsEventEnabled(InputEvent.Press) || IsEventEnabled(InputEvent.Tap)) && !_inputManager.IsTapOnIngnoreLayer(gesture.position))
			{
				Use_OnTouchStart(gesture.position);
			}
		}

		private void On_TouchUpdate(Gesture gesture)
		{
			if (IsEventEnabled(InputEvent.Press) && !_inputManager.IsTapOnIngnoreLayer(gesture.position))
			{
				Use_OnTouchUpdate(gesture.position, gesture.actionTime);
			}
		}

		private void On_TouchEnd(Gesture gesture)
		{
			if (IsEventEnabled(InputEvent.Press) && !_inputManager.IsTapOnIngnoreLayer(gesture.position))
			{
				Use_OnTouchEnd(gesture.position, gesture.actionTime);
			}
			if (IsEventEnabled(InputEvent.Tap) && !_inputManager.IsTapOnIngnoreLayer(gesture.position))
			{
				Use_OnTap(gesture.position);
			}
		}

		private void On_TouchStart2Fingers(Gesture gesture)
		{
			EasyTouch.SetEnablePinch(true);
		}

		private void On_PinchIn(Gesture gesture)
		{
			if (IsEventEnabled(InputEvent.Zoom))
			{
				float zoom = -1f * Mathf.Lerp(1f, 10f, gesture.deltaPinch);
				Use_OnZoom(zoom);
			}
		}

		private void On_PinchOut(Gesture gesture)
		{
			if (IsEventEnabled(InputEvent.Zoom))
			{
				float zoom = Mathf.Lerp(1f, 10f, gesture.deltaPinch);
				Use_OnZoom(zoom);
			}
		}

		private void On_PinchEnd(Gesture gesture)
		{
			if (IsEventEnabled(InputEvent.Zoom))
			{
			}
		}

		private void On_Cancel2Fingers(Gesture gesture)
		{
		}
	}
}

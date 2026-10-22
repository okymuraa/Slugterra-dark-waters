using EasyInput.Core;
using UnityEngine;

namespace EasyInput.StandardControllers
{
	[AddComponentMenu("EasyInputHelper/Miscellaneous/BowlingFollow")]
	public class BowlingFollow : MonoBehaviour
	{
		public EasyInputConstants.AXIS axisHorizontal;

		public EasyInputConstants.AXIS axisVertical = EasyInputConstants.AXIS.YAxis;

		public EasyInputConstants.ACTION_TYPE action;

		public float sensitivity = 1f;

		public Camera followingCamera;

		public Transform aimArrow;

		public float laneRange;

		private Vector2 lastFrameTouch = EasyInputConstants.NOT_TOUCHING;

		private Vector3 actionVector3;

		private float horizontal;

		private float vertical;

		private Vector3 temp;

		private bool preventControl;

		private void OnEnable()
		{
			EasyInputHelper.On_Touch += localAxis;
			EasyInputHelper.On_TouchEnd += localAxisEnd;
		}

		private void OnDestroy()
		{
			EasyInputHelper.On_Touch -= localAxis;
			EasyInputHelper.On_TouchEnd -= localAxisEnd;
		}

		private void Update()
		{
		}

		private void localAxisEnd(InputTouch touch)
		{
			lastFrameTouch = EasyInputConstants.NOT_TOUCHING;
		}

		private void localAxis(InputTouch touch)
		{
			if (preventControl)
			{
				return;
			}
			if (lastFrameTouch == EasyInputConstants.NOT_TOUCHING)
			{
				lastFrameTouch = touch.currentTouchPosition;
				return;
			}
			horizontal = (touch.currentTouchPosition.x - lastFrameTouch.x) * sensitivity * Time.deltaTime * 100f;
			vertical = (touch.currentTouchPosition.y - lastFrameTouch.y) * sensitivity * Time.deltaTime * 100f;
			actionVector3 = EasyInputUtilities.getControllerVector3(horizontal, vertical, axisHorizontal, axisVertical);
			switch (action)
			{
			case EasyInputConstants.ACTION_TYPE.Position:
				transform.position += actionVector3;
				break;
			case EasyInputConstants.ACTION_TYPE.Rotation:
				transform.Rotate(actionVector3, Space.World);
				break;
			case EasyInputConstants.ACTION_TYPE.LocalPosition:
				transform.Translate(actionVector3);
				break;
			case EasyInputConstants.ACTION_TYPE.LocalRotation:
				transform.Rotate(actionVector3);
				break;
			case EasyInputConstants.ACTION_TYPE.LocalScale:
				transform.localScale += actionVector3;
				break;
			default:
				Debug.Log("Invalid Action");
				break;
			}
			temp = transform.position;
			if (transform.position.x > laneRange)
			{
				temp.x = laneRange;
			}
			else if (transform.position.x < 0f - laneRange)
			{
				temp.x = 0f - laneRange;
			}
			transform.position = temp;
			temp.x = transform.position.x;
			temp.y = 3.06f;
			temp.z = -6f;
			followingCamera.transform.position = temp;
			temp = aimArrow.position;
			temp.x = transform.position.x;
			aimArrow.position = temp;
			lastFrameTouch = touch.currentTouchPosition;
		}

		public void stopFollow()
		{
			aimArrow.gameObject.SetActive(false);
			preventControl = true;
		}
	}
}

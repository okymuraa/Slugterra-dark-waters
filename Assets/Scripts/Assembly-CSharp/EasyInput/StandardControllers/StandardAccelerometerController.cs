using EasyInput.Core;
using UnityEngine;

namespace EasyInput.StandardControllers
{
	[AddComponentMenu("EasyInputHelper/Standard Controllers/StandardAccelerometerController")]
	public class StandardAccelerometerController : MonoBehaviour
	{
		public EasyInputConstants.AXIS tiltHorizontal;

		public EasyInputConstants.AXIS tiltVertical = EasyInputConstants.AXIS.YAxis;

		public EasyInputConstants.ACTION_TYPE action;

		public float sensitivity = 1f;

		private Vector3 actionVector3;

		private float horizontal;

		private float vertical;

		private float normalizeDegrees = 90f;

		private void OnEnable()
		{
		}

		private void OnDestroy()
		{
		}

		private void Update()
		{
		}

		private void localAccelerometer(Vector3 accel)
		{
			horizontal = EasyInputUtilities.relativeAngleInAxis(Vector3.up, -accel, Vector3.forward) / normalizeDegrees;
			vertical = EasyInputUtilities.relativeAngleInAxis(Vector3.up, -accel, Vector3.right) / normalizeDegrees;
			horizontal *= (0f - sensitivity) * Time.deltaTime * 100f;
			vertical *= (0f - sensitivity) * Time.deltaTime * 100f;
			actionVector3 = EasyInputUtilities.getControllerVector3(horizontal, vertical, tiltHorizontal, tiltVertical);
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
		}
	}
}

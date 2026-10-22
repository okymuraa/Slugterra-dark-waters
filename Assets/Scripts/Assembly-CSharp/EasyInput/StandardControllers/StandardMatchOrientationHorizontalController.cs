using EasyInput.Core;
using UnityEngine;

namespace EasyInput.StandardControllers
{
	[AddComponentMenu("EasyInputHelper/Standard Controllers/StandardMatchOrientationHorizontalController")]
	public class StandardMatchOrientationHorizontalController : MonoBehaviour
	{
		public bool clampRotation;

		[Range(0f, 180f)]
		public float clampRotationDegreesFromZero;

		public float sensitivity = 1f;

		[Tooltip("The smallest amount in degrees that we allow a change in rotation. This helps with shakiness at the expense of precision")]
		public float precision = 1f;

		private Vector3 myRotation;

		private float lastRotation;

		private bool didRealUpdateLastFrame;

		private void OnEnable()
		{
		}

		private void OnDestroy()
		{
		}

		private void Start()
		{
			myRotation = transform.localRotation.eulerAngles;
		}

		private void Update()
		{
		}

		private void localGyro(Vector3 gravity, Vector3 userAcceleration)
		{
			if (didRealUpdateLastFrame)
			{
				didRealUpdateLastFrame = false;
				lastRotation = myRotation.y;
			}
			myRotation.y = EasyInputUtilities.relativeAngleInAxis(Vector3.up, -gravity, Vector3.forward);
			myRotation.y *= 0f - sensitivity;
			if ((double)gravity.x > 0.97 || (double)gravity.x < -0.97)
			{
				myRotation.y = lastRotation;
			}
			if (clampRotation && clampRotationDegreesFromZero != 0f && myRotation.y <= 360f - clampRotationDegreesFromZero && myRotation.y >= clampRotationDegreesFromZero)
			{
				if (myRotation.y <= 180f)
				{
					myRotation.y = clampRotationDegreesFromZero;
				}
				else if (myRotation.y > 180f)
				{
					myRotation.y = 360f - clampRotationDegreesFromZero;
				}
			}
			if (lastRotation - myRotation.y > precision || lastRotation - myRotation.y < 0f - precision)
			{
				didRealUpdateLastFrame = true;
				transform.localRotation = Quaternion.Euler(myRotation);
			}
		}
	}
}

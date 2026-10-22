using EasyInput.Core;
using UnityEngine;

namespace EasyInput.StandardControllers
{
	[AddComponentMenu("EasyInputHelper/Standard Controllers/StandardMatchOrientationVerticalController")]
	public class StandardMatchOrientationVerticalController : MonoBehaviour
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

		private void localGyro(Vector3 gravity, Vector3 userAccel)
		{
			if (didRealUpdateLastFrame)
			{
				didRealUpdateLastFrame = false;
				lastRotation = myRotation.x;
			}
			myRotation.x = EasyInputUtilities.relativeAngleInAxis(Vector3.up, -gravity, Vector3.right);
			myRotation.x *= 0f - sensitivity;
			if ((double)gravity.z > 0.97 || (double)gravity.z < -0.97)
			{
				myRotation.x = lastRotation;
			}
			if (clampRotation && clampRotationDegreesFromZero != 0f && myRotation.x <= 360f - clampRotationDegreesFromZero && myRotation.x >= clampRotationDegreesFromZero)
			{
				if (myRotation.x <= 180f)
				{
					myRotation.x = clampRotationDegreesFromZero;
				}
				else if (myRotation.x > 180f)
				{
					myRotation.x = 360f - clampRotationDegreesFromZero;
				}
			}
			if (lastRotation - myRotation.x > precision || lastRotation - myRotation.x < 0f - precision)
			{
				didRealUpdateLastFrame = true;
				transform.localRotation = Quaternion.Euler(myRotation);
			}
		}
	}
}

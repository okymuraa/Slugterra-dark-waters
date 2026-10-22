using EasyInput.Core;
using UnityEngine;

namespace EasyInput.Misc
{
	[AddComponentMenu("EasyInputHelper/Miscellaneous/Steering")]
	public class Steering : MonoBehaviour
	{
		private Rigidbody myRigidbody;

		private Vector3 myAccel = Vector3.zero;

		private bool gasPressed;

		private bool brakePressed;

		private float horizontal;

		private float vertical;

		private float normalizeDegrees = 90f;

		private float sensitivity = 10f;

		private Vector3 actionVectorPosition;

		private Vector3 computerVector;

		private void OnEnable()
		{
			EasyInputHelper.On_ClickStart += localClickStart;
			EasyInputHelper.On_ClickEnd += localClickEnd;
		}

		private void OnDestroy()
		{
			EasyInputHelper.On_Click -= localClickStart;
			EasyInputHelper.On_Click -= localClickEnd;
		}

		private void Start()
		{
			myRigidbody = GetComponent<Rigidbody>();
		}

		private void Update()
		{
			if (gasPressed)
			{
				if (myRigidbody.velocity.magnitude > 1f)
				{
					myRigidbody.AddForce(myRigidbody.velocity * sensitivity * 0.5f);
				}
				else
				{
					myRigidbody.AddForce((transform.position - Camera.main.transform.position) * sensitivity * 5f);
				}
			}
			if (brakePressed)
			{
				if (myRigidbody.velocity.magnitude > 1f)
				{
					myRigidbody.AddForce(myRigidbody.velocity * (0f - myRigidbody.velocity.magnitude));
				}
				else
				{
					myRigidbody.AddForce(myRigidbody.velocity * (0f - myRigidbody.velocity.magnitude) * 10f);
				}
			}
			steerBall(myAccel);
		}

		public void steerBall(Vector3 accel)
		{
			if (accel != Vector3.zero)
			{
				horizontal = EasyInputUtilities.relativeAngleInAxis(Vector3.up, -accel, Vector3.forward) / normalizeDegrees;
				vertical = EasyInputUtilities.relativeAngleInAxis(Vector3.up, -accel, Vector3.right) / normalizeDegrees;
				horizontal *= 0f - sensitivity;
				vertical *= 0f - sensitivity;
			}
			else
			{
				horizontal = 0f;
				vertical = 0f;
			}
			actionVectorPosition.x = horizontal;
			actionVectorPosition.y = 0f;
			actionVectorPosition.z = vertical;
			myRigidbody.AddForce(actionVectorPosition);
		}

		private void localAccelerometer(Vector3 accel)
		{
			myAccel = accel;
		}

		private void localClickStart(ButtonClick button)
		{
			if (button.button == EasyInputConstants.CONTROLLER_BUTTON.AButton)
			{
				brakePressed = true;
			}
			else if (button.button == EasyInputConstants.CONTROLLER_BUTTON.XButton || button.button == EasyInputConstants.CONTROLLER_BUTTON.BButton)
			{
				gasPressed = true;
			}
		}

		private void localClickEnd(ButtonClick button)
		{
			if (button.button == EasyInputConstants.CONTROLLER_BUTTON.AButton)
			{
				brakePressed = false;
			}
			else if (button.button == EasyInputConstants.CONTROLLER_BUTTON.XButton || button.button == EasyInputConstants.CONTROLLER_BUTTON.BButton)
			{
				gasPressed = false;
			}
		}
	}
}

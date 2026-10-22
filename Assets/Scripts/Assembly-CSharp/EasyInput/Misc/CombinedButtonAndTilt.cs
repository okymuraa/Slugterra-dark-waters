using EasyInput.Core;
using UnityEngine;

namespace EasyInput.Misc
{
	[AddComponentMenu("EasyInputHelper/Miscellaneous/CombinedButtonAndTilt")]
	public class CombinedButtonAndTilt : MonoBehaviour
	{
		public GameObject prefabBall;

		public GameObject spawn;

		public GameObject player;

		public float tiltSensitivity = 0.5f;

		[Tooltip("The smallest amount in degrees that we allow a change in rotation. This helps with shakiness at the expense of precision")]
		public float tiltPrecision = 1f;

		public float buttonSensitivity = 3f;

		private Transform myTransform;

		private Transform spawnTransform;

		private float accumulatedRotation;

		private Vector3 myRotation;

		private float lastRotation;

		private bool didRealUpdateLastFrame;

		private void OnEnable()
		{
			EasyInputHelper.On_LongClick += localOnLongClick;
			EasyInputHelper.On_QuickClickEnd += localOnQuickEnd;
			EasyInputHelper.On_Gyro += localGyro;
		}

		private void OnDestroy()
		{
			EasyInputHelper.On_LongClick -= localOnLongClick;
			EasyInputHelper.On_QuickClickEnd -= localOnQuickEnd;
			EasyInputHelper.On_Gyro -= localGyro;
		}

		private void Start()
		{
			myTransform = transform;
			spawnTransform = spawn.transform;
		}

		private void Update()
		{
		}

		private void localOnLongClick(ButtonClick click)
		{
			if (click.button == EasyInputConstants.CONTROLLER_BUTTON.AButton)
			{
				accumulatedRotation -= buttonSensitivity * Time.deltaTime * 100f;
			}
			else if (click.button == EasyInputConstants.CONTROLLER_BUTTON.XButton)
			{
				accumulatedRotation += buttonSensitivity * Time.deltaTime * 100f;
			}
		}

		private void localOnQuickEnd(ButtonClick click)
		{
			if (click.button == EasyInputConstants.CONTROLLER_BUTTON.XButton)
			{
				GameObject gameObject = Object.Instantiate(prefabBall, spawnTransform.transform.position, spawnTransform.transform.rotation);
				Rigidbody component = gameObject.GetComponent<Rigidbody>();
				component.AddForce((spawnTransform.transform.position - myTransform.transform.position) * 1000f);
			}
		}

		private void localGyro(Vector3 gravity, Vector3 userAcceleration)
		{
			if (didRealUpdateLastFrame)
			{
				didRealUpdateLastFrame = false;
				lastRotation = myRotation.y;
			}
			myRotation.y = 0f;
			myRotation.y += accumulatedRotation;
			if (lastRotation - myRotation.y > tiltPrecision || lastRotation - myRotation.y < 0f - tiltPrecision)
			{
				didRealUpdateLastFrame = true;
				player.transform.localRotation = Quaternion.Euler(myRotation);
			}
		}
	}
}

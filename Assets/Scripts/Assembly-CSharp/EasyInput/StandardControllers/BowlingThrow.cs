using EasyInput.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EasyInput.StandardControllers
{
	[AddComponentMenu("EasyInputHelper/Miscellaneous/BowlingThrow")]
	public class BowlingThrow : MonoBehaviour
	{
		public BowlingFollow follow;

		public Transform aimArrow;

		public float aimDegreesClamp = 40f;

		private bool throwInProcess;

		private bool preventThrowAgain;

		private Vector3 temp;

		private Vector3 currentThrow = new Vector3(0f, 0f, 0f);

		private Vector3 currentTorque = new Vector3(0f, 0f, 0f);

		private Rigidbody myRigidbody;

		private float degreesOff;

#if UNITY_6000_0_OR_NEWER
		private PhysicsMaterial myPhysics;
#else
		private PhysicMaterial myPhysics;
#endif

		private void OnEnable()
		{
			EasyInputHelper.On_ClickStart += initiateThrow;
			EasyInputHelper.On_ClickEnd += endThrow;
			EasyInputHelper.On_Motion += trackThrow;
		}

		private void OnDestroy()
		{
			EasyInputHelper.On_ClickStart -= initiateThrow;
			EasyInputHelper.On_ClickEnd -= endThrow;
			EasyInputHelper.On_Motion -= trackThrow;
		}

		private void Start()
		{
			myRigidbody = transform.GetComponent<Rigidbody>();
			myRigidbody.maxAngularVelocity = 70f;
			myPhysics = transform.GetComponent<SphereCollider>().material;
		}

		private void Update()
		{
			if (throwInProcess)
			{
				temp = transform.position;
				temp.y = 1.5f;
				transform.position = temp;
				myRigidbody.velocity = Vector3.zero;
			}
			else if (transform.position.z > 0f)
			{
				myPhysics.dynamicFriction = transform.position.z / 20f;
				myPhysics.staticFriction = transform.position.z / 20f;
			}
		}

		private void initiateThrow(ButtonClick click)
		{
			if (click.button == EasyInputConstants.CONTROLLER_BUTTON.AButton && !preventThrowAgain)
			{
				EasyInputHelper.resetMotion();
				throwInProcess = true;
				follow.stopFollow();
			}
		}

		private void endThrow(ButtonClick click)
		{
			if (click.button == EasyInputConstants.CONTROLLER_BUTTON.AButton && !preventThrowAgain)
			{
				EasyInputHelper.resetMotion();
				throwInProcess = false;
				preventThrowAgain = true;
				temp = transform.position;
				temp.y = 0.5f;
				transform.position = temp;
				myRigidbody.AddRelativeForce(currentThrow, ForceMode.Impulse);
				myRigidbody.AddRelativeTorque(currentTorque, ForceMode.Impulse);
				temp = myRigidbody.velocity;
				temp.y = 0f;
				myRigidbody.velocity = temp;
				Invoke("resetScene", 8f);
			}
		}

		private void trackThrow(EasyInput.Core.Motion motion)
		{
			if (throwInProcess)
			{
				if (motion.totalVelocitySinceReset.magnitude > 10f)
				{
					currentThrow.z = 365f + 85f * motion.currentVelocity.z;
				}
				else
				{
					currentThrow.z = 10f;
				}
				currentThrow.x = degreesOff * (currentThrow.z / 100f);
				if (motion.currentOrientation.x < 20f && motion.currentOrientation.x > -20f)
				{
					currentTorque.z = 0f;
				}
				else
				{
					currentTorque.z = motion.currentOrientation.x / 8f;
				}
			}
			else if (!throwInProcess && !preventThrowAgain && motion.currentOrientation.x < aimDegreesClamp && motion.currentOrientation.x > 0f - aimDegreesClamp)
			{
				temp = aimArrow.localRotation.eulerAngles;
				temp.y = 270f - motion.currentOrientation.x;
				aimArrow.localRotation = Quaternion.Euler(temp);
				degreesOff = aimArrow.localRotation.eulerAngles.y - 270f;
			}
		}

		private void resetScene()
		{
			SceneManager.LoadScene("bowlingExample");
		}
	}
}

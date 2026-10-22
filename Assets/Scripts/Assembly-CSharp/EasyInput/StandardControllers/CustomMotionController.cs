using System;
using EasyInput.Core;
using UnityEngine;
using UnityEngine.Events;

namespace EasyInput.StandardControllers
{
	[AddComponentMenu("EasyInputHelper/Custom Controllers/CustomMotionController")]
	public class CustomMotionController : MonoBehaviour
	{
		[Serializable]
		public class AccelerometerHandler : UnityEvent<Vector3>
		{
		}

		[Serializable]
		public class GyroHandler : UnityEvent<Vector3, Vector3>
		{
		}

		[SerializeField]
		public AccelerometerHandler onAccelerometer;

		[SerializeField]
		public GyroHandler onGyro;

		private void OnEnable()
		{
			EasyInputHelper.On_Accelerometer += localAcceleromter;
			EasyInputHelper.On_Gyro += localGyro;
		}

		private void OnDestroy()
		{
			EasyInputHelper.On_Accelerometer -= localAcceleromter;
			EasyInputHelper.On_Gyro -= localGyro;
		}

		private void Update()
		{
		}

		private void localAcceleromter(Vector3 accel)
		{
			onAccelerometer.Invoke(accel);
		}

		private void localGyro(Vector3 gravity, Vector3 userAccel)
		{
			onGyro.Invoke(gravity, userAccel);
		}
	}
}

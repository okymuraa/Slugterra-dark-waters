using UnityEngine;

namespace EasyInput.Core
{
	public class Motion
	{
		public float lastResetTimestamp;

		public Vector3 currentRawUserAcceleration;

		public Vector3 currentRawGravity;

		public Vector3 currentSmoothedUserAcceleration;

		public Vector3 currentSmoothedGravity;

		public Vector3 lastFrameSmoothedUserAcceleration;

		public Vector3 lastFrameSmoothedGravity;

		public Vector3 currentVelocity;

		public Vector3 lastFrameVelocity;

		public Vector3 currentRelativePositionSinceReset;

		public Vector3 currentOrientation;

		public Vector3 lastFrameOrientation;

		public Vector3 currentRotationRate;

		public Vector3 lastFrameRotationRate;

		public Vector3 totalVelocitySinceReset;

		public Vector3 totalRotationRateSinceReset;
	}
}

using UnityEngine;

namespace EasyInput.Core
{
	public static class EasyInputUtilities
	{
		public static float determineDistance(Vector2 original, Vector2 current)
		{
			return Mathf.Sqrt(Mathf.Abs(Mathf.Pow(original.x - current.x, 2f) + Mathf.Pow(original.y - current.y, 2f)));
		}

		public static EasyInputConstants.SWIPE_TYPE determineSwipeType(InputTouch myTouch)
		{
			EasyInputConstants.SWIPE_TYPE sWIPE_TYPE = EasyInputConstants.SWIPE_TYPE.None;
			Vector2 vector = myTouch.currentTouchPosition - myTouch.originalTouchPosition;
			if (Mathf.Abs(vector.x) > Mathf.Abs(vector.y))
			{
				return (!(vector.x > 0f)) ? EasyInputConstants.SWIPE_TYPE.Left : EasyInputConstants.SWIPE_TYPE.Right;
			}
			return (!(vector.y > 0f)) ? EasyInputConstants.SWIPE_TYPE.Down : EasyInputConstants.SWIPE_TYPE.Up;
		}

		public static Vector3 getControllerVector3(float horizontalMovement, float verticalMovement, EasyInputConstants.AXIS horizontalAxis, EasyInputConstants.AXIS verticalAxis)
		{
			Vector3 result = default(Vector3);
			result.x = 0f;
			result.y = 0f;
			result.z = 0f;
			if (horizontalAxis == EasyInputConstants.AXIS.XAxis)
			{
				result.x += horizontalMovement;
			}
			if (verticalAxis == EasyInputConstants.AXIS.XAxis)
			{
				result.x += verticalMovement;
			}
			if (horizontalAxis == EasyInputConstants.AXIS.NegativeXAxis)
			{
				result.x -= horizontalMovement;
			}
			if (verticalAxis == EasyInputConstants.AXIS.NegativeXAxis)
			{
				result.x -= verticalMovement;
			}
			if (horizontalAxis == EasyInputConstants.AXIS.YAxis)
			{
				result.y += horizontalMovement;
			}
			if (verticalAxis == EasyInputConstants.AXIS.YAxis)
			{
				result.y += verticalMovement;
			}
			if (horizontalAxis == EasyInputConstants.AXIS.NegativeYAxis)
			{
				result.y -= horizontalMovement;
			}
			if (verticalAxis == EasyInputConstants.AXIS.NegativeYAxis)
			{
				result.y -= verticalMovement;
			}
			if (horizontalAxis == EasyInputConstants.AXIS.ZAxis)
			{
				result.z += horizontalMovement;
			}
			if (verticalAxis == EasyInputConstants.AXIS.ZAxis)
			{
				result.z += verticalMovement;
			}
			if (horizontalAxis == EasyInputConstants.AXIS.NegativeZAxis)
			{
				result.z -= horizontalMovement;
			}
			if (verticalAxis == EasyInputConstants.AXIS.NegativeZAxis)
			{
				result.z -= verticalMovement;
			}
			return result;
		}

		public static Vector2 flipCoordinates(EasyInputConstants.REMOTE_ORIENTATION orientation, Vector2 touch)
		{
			Vector2 vector = touch;
			switch (orientation)
			{
			case EasyInputConstants.REMOTE_ORIENTATION.TouchpadLeft:
				touch.x = 0f - vector.y;
				touch.y = vector.x;
				break;
			case EasyInputConstants.REMOTE_ORIENTATION.TouchpadRight:
				touch.x = vector.y;
				touch.y = 0f - vector.x;
				break;
			}
			return touch;
		}

		public static float relativeAngleInAxis(Vector3 firstVector, Vector3 secondVector, Vector3 axisVector)
		{
			axisVector.Normalize();
			Vector3 lhs = Vector3.Cross(axisVector, firstVector);
			Vector3 vector = Vector3.Cross(lhs, axisVector);
			lhs = Vector3.Cross(axisVector, secondVector);
			Vector3 vector2 = Vector3.Cross(lhs, axisVector);
			lhs = Vector3.Cross(vector, vector2);
			float f = Vector3.Angle(lhs, axisVector) - 90f;
			float num = Mathf.Sign(f);
			return Vector3.Angle(vector, vector2) * num;
		}

		public static Vector3 dampenSensorNoise(Vector3 currentRawValue, Vector3 previouslySmoothedValue, float threshold, int framesWithinThresholdX, int framesWithinThresholdY, int framesWithinThresholdZ)
		{
			Vector3 result = currentRawValue;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			if (currentRawValue.x >= 0f - threshold && currentRawValue.x <= threshold)
			{
				result.x = 0f;
				flag = true;
			}
			if (currentRawValue.y >= 0f - threshold && currentRawValue.y <= threshold)
			{
				result.y = 0f;
				flag2 = true;
			}
			if (currentRawValue.z >= 0f - threshold && currentRawValue.z <= threshold)
			{
				result.z = 0f;
				flag3 = true;
			}
			if (currentRawValue.x >= 0f - threshold + 1f && currentRawValue.x <= threshold + 1f)
			{
				result.x = 1f;
				flag = true;
			}
			if (currentRawValue.y >= 0f - threshold + 1f && currentRawValue.y <= threshold + 1f)
			{
				result.y = 1f;
				flag2 = true;
			}
			if (currentRawValue.z >= 0f - threshold + 1f && currentRawValue.z <= threshold + 1f)
			{
				result.z = 1f;
				flag3 = true;
			}
			if (currentRawValue.x >= 0f - threshold - 1f && currentRawValue.x <= threshold - 1f)
			{
				result.x = -1f;
				flag = true;
			}
			if (currentRawValue.y >= 0f - threshold - 1f && currentRawValue.y <= threshold - 1f)
			{
				result.y = -1f;
				flag2 = true;
			}
			if (currentRawValue.z >= 0f - threshold - 1f && currentRawValue.z <= threshold - 1f)
			{
				result.z = -1f;
				flag3 = true;
			}
			if (!(previouslySmoothedValue == EasyInputConstants.NOT_VALID))
			{
				if (!flag && currentRawValue.x >= 0f - threshold + previouslySmoothedValue.x && currentRawValue.x <= threshold + previouslySmoothedValue.x)
				{
					result.x = (result.x + (float)framesWithinThresholdX * previouslySmoothedValue.x) / (float)(framesWithinThresholdX + 1);
				}
				if (!flag2 && currentRawValue.y >= 0f - threshold + previouslySmoothedValue.y && currentRawValue.y <= threshold + previouslySmoothedValue.y)
				{
					result.y = (result.y + (float)framesWithinThresholdY * previouslySmoothedValue.y) / (float)(framesWithinThresholdY + 1);
				}
				if (!flag3 && currentRawValue.z >= 0f - threshold + previouslySmoothedValue.z && currentRawValue.z <= threshold + previouslySmoothedValue.z)
				{
					result.z = (result.z + (float)framesWithinThresholdZ * previouslySmoothedValue.z) / (float)(framesWithinThresholdZ + 1);
				}
			}
			return result;
		}
	}
}

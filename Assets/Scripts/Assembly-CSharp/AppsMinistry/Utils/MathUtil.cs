using System;
using UnityEngine;

namespace AppsMinistry.Utils
{
	public static class MathUtil
	{
		public static float GetAngle(Vector2 from, Vector2 to)
		{
			float num = Vector2.Angle(from, to);
			if (Vector3.Cross(from, to).z > 0f)
			{
				num = 360f - num;
			}
			return num;
		}

		public static float GetAngle(float x, float y)
		{
			float num = Mathf.Atan2(y, x) * 57.295f;
			if (num < 0f)
			{
				num = 360f + num;
			}
			return num;
		}

		public static Vector2 Rotate(Vector2 vector, float angle)
		{
			float f = angle / 180f * (float)Math.PI;
			float x = vector.x * Mathf.Cos(f) - vector.y * Mathf.Sin(f);
			float y = vector.x * Mathf.Sin(f) + vector.y * Mathf.Cos(f);
			return new Vector2(x, y);
		}

		private static bool AreClockwise(Vector3 v1, Vector3 v2)
		{
			return (0f - v1.x) * v2.z + v1.z * v2.x > 0f;
		}

		private static bool IsWithinRadius(Vector3 v, float radius)
		{
			return v.x * v.x + v.y * v.y <= radius * radius;
		}

		public static bool IsInsideSector(Vector3 basePosition, Vector3 targetPosition, float baseAngle, float sectorAngle, float radius)
		{
			if (sectorAngle <= 180f)
			{
				float f = (baseAngle - sectorAngle / 2f) * ((float)Math.PI / 180f);
				Vector3 sectorStart = new Vector3(radius * Mathf.Sin(f), 0f, radius * Mathf.Cos(f));
				f = (baseAngle + sectorAngle / 2f) * ((float)Math.PI / 180f);
				Vector3 sectorEnd = new Vector3(radius * Mathf.Sin(f), 0f, radius * Mathf.Cos(f));
				return IsInsideSector(targetPosition, basePosition, sectorStart, sectorEnd, radius);
			}
			baseAngle = ((!(baseAngle >= 180f)) ? (baseAngle + 180f) : (baseAngle - 180f));
			return !IsInsideSector(basePosition, targetPosition, baseAngle, 360f - sectorAngle, radius);
		}

		private static bool IsInsideSector(Vector3 point, Vector3 center, Vector3 sectorStart, Vector3 sectorEnd, float radius)
		{
			Vector3 vector = point - center;
			return AreClockwise(sectorStart, vector) && !AreClockwise(sectorEnd, vector) && IsWithinRadius(vector, radius);
		}
	}
}

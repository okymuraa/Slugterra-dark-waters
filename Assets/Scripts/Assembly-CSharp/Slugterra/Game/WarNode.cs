using System;
using System.Collections.Generic;
using UnityEngine;

namespace Slugterra.Game
{
	[Serializable]
	public class WarNode : MonoBehaviour
	{
		[SerializeField]
		private Transform[] _warPoints;

		private List<Transform> _occupiedPoints;

		private Transform _cachedTransform;

		public Transform[] WarPoints
		{
			get
			{
				return _warPoints;
			}
		}

		public Transform Transform
		{
			get
			{
				return _cachedTransform;
			}
		}

		private void Start()
		{
			_cachedTransform = transform;
			_occupiedPoints = new List<Transform>();
		}

		public float GetOptimalWeight(Vector3 basePosition, Vector3 targetPosition, float distance)
		{
			float num = float.MaxValue;
			float num2 = 0f;
			float num3 = 0f;
			Transform[] warPoints = _warPoints;
			foreach (Transform transform in warPoints)
			{
				if (_occupiedPoints.Contains(transform) || transform == null)
				{
					continue;
				}
				num2 = Vector3.SqrMagnitude(targetPosition - transform.position);
				if (num2 <= distance * distance)
				{
					num3 = Vector3.SqrMagnitude(basePosition - transform.position) - num2;
					if (num > num3)
					{
						num = num3;
					}
				}
			}
			return num;
		}

		public Transform GetOptimalPoint(Vector3 basePosition, Vector3 targetPosition, float distance)
		{
			Transform transform = null;
			float num = float.MaxValue;
			float num2 = 0f;
			float num3 = 0f;
			Transform[] warPoints = _warPoints;
			foreach (Transform transform2 in warPoints)
			{
				if (_occupiedPoints.Contains(transform2) || transform2 == null)
				{
					continue;
				}
				num2 = Vector3.SqrMagnitude(targetPosition - transform2.position);
				if (num2 <= distance * distance)
				{
					num3 = Vector3.SqrMagnitude(basePosition - transform2.position) - num2;
					if (num > num3)
					{
						num = num3;
						transform = transform2;
					}
				}
			}
			if (transform != null)
			{
				_occupiedPoints.Add(transform);
			}
			return transform;
		}

		public Transform GetRandomPoint()
		{
			if (_warPoints == null || _warPoints.Length == 0)
			{
				Debug.LogWarning("GetRandomPoint: War node does not have point");
				return null;
			}
			if (_occupiedPoints.Count >= _warPoints.Length)
			{
				return null;
			}
			List<Transform> list = new List<Transform>();
			Transform[] warPoints = _warPoints;
			foreach (Transform item in warPoints)
			{
				if (!_occupiedPoints.Contains(item))
				{
					list.Add(item);
				}
			}
			if (list.Count == 0)
			{
				return null;
			}
			Transform transform = list[UnityEngine.Random.Range(0, list.Count)];
			_occupiedPoints.Add(transform);
			return transform;
		}

		public bool IsContainEmptyPoints()
		{
			if (_occupiedPoints.Count < _warPoints.Length)
			{
				return true;
			}
			return false;
		}

		public void ReleasePoint(Transform point)
		{
			if (_occupiedPoints.Contains(point))
			{
				_occupiedPoints.Remove(point);
			}
		}

		public void Rebuild()
		{
			foreach (Transform child in transform)
			{
				if (!Array.Exists(_warPoints, (Transform warPoint) => (warPoint == child) ? true : false))
				{
					Transform[] array = new Transform[_warPoints.Length + 1];
					_warPoints.CopyTo(array, 0);
					_warPoints = array;
					_warPoints[_warPoints.Length - 1] = child;
					child.gameObject.name = string.Format("WarPoint_{0}", _warPoints.Length);
				}
			}
		}
	}
}

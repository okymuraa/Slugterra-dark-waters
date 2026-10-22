using System.Collections.Generic;
using AppsMinistry.Utils;
using UnityEngine;

namespace Slugterra.Game
{
	public class CoverNode : MonoBehaviour
	{
		[SerializeField]
		private CoverType _type = CoverType.Low;

		[SerializeField]
		private Transform[] _coverPoints;

		private int curPoint;

		private List<Transform> _occupiedPoints;

		private Transform _cachedTransform;

		public Transform[] CoverPoints
		{
			get
			{
				return _coverPoints;
			}
			set
			{
				_coverPoints = value;
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

		public Transform GetNextPathPoint()
		{
			curPoint++;
			if (curPoint > _coverPoints.Length - 1)
			{
				curPoint = 0;
			}
			return _coverPoints[curPoint];
		}

		public Transform GetNextPathPoint(Vector3 position)
		{
			float num = float.MaxValue;
			for (int i = 0; i < _coverPoints.Length; i++)
			{
				float magnitude = (_coverPoints[i].transform.position - position).magnitude;
				if (magnitude < num)
				{
					num = magnitude;
					curPoint = i;
				}
			}
			return _coverPoints[curPoint];
		}

		public Transform GetNextPathPoint(Vector3 position, float radius)
		{
			List<int> list = new List<int>();
			float num = float.MaxValue;
			for (int i = 0; i < _coverPoints.Length; i++)
			{
				float magnitude = (_coverPoints[i].transform.position - position).magnitude;
				if (magnitude <= radius)
				{
					list.Add(i);
				}
				if (magnitude < num)
				{
					num = magnitude;
					curPoint = i;
				}
			}
			if (list.Count > 0)
			{
				int index = Random.Range(0, list.Count - 1);
				curPoint = list[index];
			}
			return _coverPoints[curPoint];
		}

		public Transform GetControlPoint(Transform target)
		{
			if (_coverPoints == null || _coverPoints.Length == 0)
			{
				Debug.LogWarning("ShelterController: controllPoints is null or count of controlPoints is 0");
				return null;
			}
			if (target == null)
			{
				return null;
			}
			float num = 0f;
			Transform transform = null;
			Transform[] coverPoints = _coverPoints;
			foreach (Transform transform2 in coverPoints)
			{
				if (!_occupiedPoints.Contains(transform2) && !(transform2 == null))
				{
					float num2 = Vector3.SqrMagnitude(transform2.position - target.position);
					if (num2 > num)
					{
						num = num2;
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

		public void ReleasePoint(Transform point)
		{
			if (_occupiedPoints.Contains(point))
			{
				_occupiedPoints.Remove(point);
			}
		}

		public bool IsContainEmptyPoint()
		{
			if (_occupiedPoints.Count < _coverPoints.Length)
			{
				return true;
			}
			return false;
		}

		public void OnDrawGizmos()
		{
			Gizmos.color = Color.white;
			Transform[] coverPoints = _coverPoints;
			foreach (Transform transform in coverPoints)
			{
				Vector2 vector = MathUtil.Rotate(new Vector2(0f, 1f), transform.eulerAngles.y);
				Gizmos.DrawLine(transform.position + Vector3.up * 0.1f, transform.position + new Vector3(vector.x, 0f, vector.y) * 2f + Vector3.up * 0.1f);
			}
		}

		public virtual float GetDamageRatio()
		{
			if (_type == CoverType.Low)
			{
				return 0.8f;
			}
			if (_type == CoverType.Medium)
			{
				return 0.6f;
			}
			if (_type == CoverType.High)
			{
				return 0.4f;
			}
			return 1f;
		}
	}
}

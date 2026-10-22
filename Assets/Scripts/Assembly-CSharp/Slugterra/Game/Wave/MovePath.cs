using System.Collections.Generic;
using UnityEngine;

namespace Slugterra.Game.Wave
{
	public class MovePath : MonoBehaviour
	{
		[SerializeField]
		private bool _edit;

		[SerializeField]
		private bool _ninjaMod;

		[SerializeField]
		private bool _useCustomSpeed;

		[SerializeField]
		private int _customSpeed;

		[SerializeField]
		private List<Transform> _points;

		private Transform _transform;

		public bool NinjaMod
		{
			get
			{
				return _ninjaMod;
			}
		}

		public bool UseCustomSpeed
		{
			get
			{
				return _useCustomSpeed;
			}
		}

		public int CustomSpeed
		{
			get
			{
				return _customSpeed;
			}
		}

		public List<Transform> Points
		{
			get
			{
				return _points;
			}
		}

		private void Start()
		{
			_transform = transform;
			if (_points != null && _points.Count != 0)
			{
				return;
			}
			_points = new List<Transform>(_transform.childCount);
			foreach (Transform item in _transform)
			{
				_points.Add(item);
			}
			_points.Sort(ComparePoint);
		}

		private int ComparePoint(Transform point1, Transform point2)
		{
			int result = 0;
			if (!int.TryParse(point1.name.Replace("Point", string.Empty), out result))
			{
				return 0;
			}
			int result2 = 0;
			if (!int.TryParse(point2.name.Replace("Point", string.Empty), out result2))
			{
				return 0;
			}
			return ComparePoint(result, result2);
		}

		private int ComparePoint(int point1, int point2)
		{
			if (point1 == point2)
			{
				return 0;
			}
			if (point1 > point2)
			{
				return 1;
			}
			return -1;
		}

		private void OnDrawGizmos()
		{
			if (!_edit || transform.childCount == 0)
			{
				return;
			}
			List<Transform> list = new List<Transform>(transform.childCount);
			foreach (Transform item in transform)
			{
				list.Add(item);
			}
			list.Sort(ComparePoint);
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(transform.position, 0.8f);
			Gizmos.color = Color.cyan;
			Gizmos.DrawLine(transform.position, list[0].position);
			for (int i = 0; i < list.Count - 1; i++)
			{
				Gizmos.DrawSphere(list[i].position, 0.3f);
				Gizmos.DrawLine(list[i].position, list[i + 1].position);
			}
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(list[list.Count - 1].position, 0.8f);
		}

		public int GetPointCount()
		{
			return _points.Count;
		}

		public Transform GetNextPoint(int currentIndex)
		{
			if (currentIndex < _points.Count)
			{
				return _points[currentIndex];
			}
			return null;
		}
	}
}

using AppsMinistry.Utils;
using UnityEngine;

namespace Slugterra.Game.Units
{
	public class PatrolPath : MonoBehaviour
	{
		[SerializeField]
		private Transform[] _pathPoints;

		[SerializeField]
		private bool _isClosedPath = true;

		private bool _isMoveUp;

		[SerializeField]
		private float _minWaitTime = 2f;

		[SerializeField]
		private float _maxWaitTime = 4f;

		private int _currentPointIndex;

		private int _nextPointIndex;

		public Transform[] PathPoints
		{
			get
			{
				return _pathPoints;
			}
		}

		public float MinWaitTime
		{
			get
			{
				return _minWaitTime;
			}
		}

		public float MaxWaitTime
		{
			get
			{
				return _maxWaitTime;
			}
		}

		private void Awake()
		{
			if (_pathPoints.Length < 2)
			{
				Debug.LogWarning("Path is short");
			}
			_currentPointIndex = 0;
			_nextPointIndex = _currentPointIndex + 1;
			if (!_isClosedPath)
			{
				_isMoveUp = true;
			}
		}

		private void NextPoint()
		{
			_currentPointIndex = _nextPointIndex;
			if (_isClosedPath)
			{
				_nextPointIndex = ((++_nextPointIndex < _pathPoints.Length) ? _nextPointIndex : 0);
			}
			else if (_isMoveUp)
			{
				_nextPointIndex++;
				if (_nextPointIndex >= _pathPoints.Length)
				{
					_nextPointIndex = _pathPoints.Length - 2;
					_isMoveUp = !_isMoveUp;
				}
			}
			else
			{
				_nextPointIndex--;
				if (_nextPointIndex < 0)
				{
					_nextPointIndex = 1;
					_isMoveUp = !_isMoveUp;
				}
			}
		}

		public Transform GetNextPoint()
		{
			if (_pathPoints == null || _pathPoints.Length == 0)
			{
				return null;
			}
			if (_pathPoints.Length == 1)
			{
				return _pathPoints[0];
			}
			NextPoint();
			return _pathPoints[_currentPointIndex];
		}

		public void OnDrawGizmos()
		{
			Gizmos.color = Color.white;
			Transform[] pathPoints = _pathPoints;
			foreach (Transform transform in pathPoints)
			{
				Vector2 vector = MathUtil.Rotate(new Vector2(0f, 1f), 0f - transform.eulerAngles.y);
				Gizmos.DrawLine(transform.position + Vector3.up * 0.1f, transform.position + new Vector3(vector.x, 0f, vector.y) * 2f + Vector3.up * 0.1f);
			}
		}
	}
}

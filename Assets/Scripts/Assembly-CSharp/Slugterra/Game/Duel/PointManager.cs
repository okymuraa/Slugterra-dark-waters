using System.Collections.Generic;
using UnityEngine;

namespace Slugterra.Game.Duel
{
	public class PointManager<T> where T : DuelPoint
	{
		private List<T> _points;

		private IDuelIterator _iterator;

		public PointManager(List<T> points, DuelMoveType type)
		{
			_points = points;
			switch (type)
			{
			case DuelMoveType.Circle:
				_iterator = new CircleIterator(points.Count);
				break;
			case DuelMoveType.Line:
				_iterator = new LineIterator(points.Count);
				break;
			}
		}

		public T GetNearestPoint(Vector3 position)
		{
			float num = float.MaxValue;
			int current = -1;
			for (int i = 0; i < _points.Count; i++)
			{
				DuelPoint duelPoint = _points[i];
				if (Vector3.SqrMagnitude(position - duelPoint.transform.position) < num)
				{
					current = i;
					num = Vector3.SqrMagnitude(position - duelPoint.transform.position);
				}
			}
			_iterator.Current = current;
			return _points[_iterator.Current];
		}

		public T GetRandomPoint()
		{
			int num = Random.Range(0, _points.Count);
			_iterator.Current = num;
			return _points[num];
		}

		public T GetNextPoint()
		{
			int num = _iterator.Next();
			if (num == -1)
			{
				return (T)null;
			}
			return _points[num];
		}

		public T GetPrevPoint()
		{
			int num = _iterator.Prev();
			if (num == -1)
			{
				return (T)null;
			}
			return _points[num];
		}

		public bool IsPrev()
		{
			return _iterator.IsPrev();
		}

		public bool IsNext()
		{
			return _iterator.IsNext();
		}
	}
}

using UnityEngine;

namespace Slugterra.Behavior.Data
{
	public class Movement
	{
		public float Speed;

		public bool StepBackAfterAttack;

		public bool WaitAfterLoseTarget;

		public bool IsMoveOrderRecieved;

		private Transform _movePoint;

		public Transform MovePoint
		{
			get
			{
				if (_movePoint == null)
				{
					_movePoint = new GameObject("~pointToMove").transform;
				}
				return _movePoint;
			}
			set
			{
				_movePoint = value;
			}
		}
	}
}

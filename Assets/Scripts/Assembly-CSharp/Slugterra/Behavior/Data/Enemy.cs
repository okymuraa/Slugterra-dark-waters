using Slugterra.Game;
using UnityEngine;

namespace Slugterra.Behavior.Data
{
	public class Enemy
	{
		public IDestructible Controller;

		private Transform _attackTargetPoint;

		public Transform Root;

		public bool IsReloadSlug;

		public bool IsFallDown;

		public bool IsTargetLost;

		public Vector3 LastKnownPos;

		public bool Exist
		{
			get
			{
				return Controller != null && AttackTargetPoint != null;
			}
		}

		public Transform AttackTargetPoint
		{
			get
			{
				return _attackTargetPoint;
			}
			set
			{
				_attackTargetPoint = value;
			}
		}

		public void Reset()
		{
			Controller = null;
			AttackTargetPoint = null;
			Root = null;
			IsReloadSlug = false;
		}
	}
}

using System;
using UnityEngine;

namespace Slugterra.Game.Wave
{
	[Serializable]
	public class SpawnPoint
	{
		[SerializeField]
		private Transform _position;

		[SerializeField]
		private MovePath[] _paths;

		public Vector3 Position
		{
			get
			{
				return _position.position;
			}
		}

		public MovePath[] Paths
		{
			get
			{
				return _paths;
			}
		}
	}
}

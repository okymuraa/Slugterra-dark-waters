using System;
using UnityEngine;

namespace Slugterra.Game.Wave
{
	[Serializable]
	public class Wave
	{
		[SerializeField]
		private float _delayTime;

		[SerializeField]
		private float _generalCoeficient = 1f;

		[SerializeField]
		private int _unitLimit = 2;

		[SerializeField]
		private GameObject[] _units;

		[SerializeField]
		private SpawnPoint _spawnPoint;

		public bool DefeatAll;

		public float DelayTime
		{
			get
			{
				return _delayTime;
			}
		}

		public float GeneralCoeficient
		{
			get
			{
				return _generalCoeficient;
			}
		}

		public int UnitLimit
		{
			get
			{
				return _unitLimit;
			}
		}

		public GameObject[] Units
		{
			get
			{
				return _units;
			}
		}

		public SpawnPoint SpawnPoint
		{
			get
			{
				return _spawnPoint;
			}
		}
	}
}

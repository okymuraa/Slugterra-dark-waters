using System;
using Slugterra.Game.Units;
using UnityEngine;

namespace Slugterra.Game.Wave
{
	public abstract class Spawner : MonoBehaviour
	{
		public abstract bool IsAllWaveFinish { get; }

		public abstract event Action<UnitController> OnUnitBornEvent;
	}
}

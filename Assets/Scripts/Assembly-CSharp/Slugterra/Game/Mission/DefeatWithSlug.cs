using System.Collections.Generic;
using AppsMinistry.Core;
using Slugterra.Configuration;
using Slugterra.Game.Units;
using UnityEngine;

namespace Slugterra.Game.Mission
{
	internal class DefeatWithSlug : ObjectiveTarget
	{
		[SerializeField]
		private SlugType _slugType;

		[SerializeField]
		private List<UnitController> _units;

		private bool _isFailed;

		private void OnEnable()
		{
			if (MonoSingleton<MissionStatisticsController>.Instance != null)
			{
				MonoSingleton<MissionStatisticsController>.Instance.OnEnemyKilledEvent += OnEnemyKilled;
			}
		}

		private void OnDisable()
		{
			if (MonoSingleton<MissionStatisticsController>.Instance != null)
			{
				MonoSingleton<MissionStatisticsController>.Instance.OnEnemyKilledEvent -= OnEnemyKilled;
			}
		}

		private void OnEnemyKilled(Attacker killer, UnitController corpse)
		{
			if (Completed || _isFailed)
			{
				return;
			}
			foreach (UnitController unit in _units)
			{
				if (unit.Equals(corpse))
				{
					if (killer.slugType != _slugType)
					{
						Completed = false;
						_isFailed = true;
					}
					_units.Remove(unit);
					break;
				}
			}
			if (_units.Count == 0 && !_isFailed)
			{
				Completed = true;
			}
		}
	}
}

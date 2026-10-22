using System.Collections.Generic;
using Slugterra.Game.Environment;
using Slugterra.Game.Units;
using UnityEngine;

namespace Slugterra.Game.Mission
{
	public class KillEnemyObjective : ObjectiveTarget
	{
		[SerializeField]
		private List<UnitController> _units;

		[SerializeField]
		private List<ExplosionEnvironmentController> _objects;

		[SerializeField]
		private bool _isCountable;

		[SerializeField]
		private int _count;

		protected override void OnUpdate()
		{
			if (!Completed && Active)
			{
				bool flag = true;
				if (_isCountable ? IsCompleteCountable() : IsCompleteUncountable())
				{
					Completed = true;
				}
			}
		}

		private bool IsCompleteUncountable()
		{
			foreach (UnitController unit in _units)
			{
				if (unit != null && unit.GameObject == null)
				{
					return false;
				}
				if (unit != null && unit.Model != null && !unit.Model.IsDead())
				{
					return false;
				}
			}
			foreach (ExplosionEnvironmentController @object in _objects)
			{
				if (@object != null && !@object.enabled)
				{
					return false;
				}
				if (@object != null && @object.Model != null && !@object.Model.IsDead())
				{
					return false;
				}
			}
			return true;
		}

		private bool IsCompleteCountable()
		{
			int num = 0;
			foreach (UnitController unit in _units)
			{
				if ((!(unit != null) || !(unit.GameObject == null)) && (unit == null || unit.Model == null || unit.Model.IsDead()))
				{
					num++;
				}
			}
			foreach (ExplosionEnvironmentController @object in _objects)
			{
				if ((!(@object != null) || (@object.enabled && @object.gameObject.activeSelf)) && (@object == null || @object.Model == null || @object.Model.IsDead()))
				{
					num++;
				}
			}
			return num >= _count;
		}
	}
}

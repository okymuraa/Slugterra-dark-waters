using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core;
using Slugterra.Configuration;
using Slugterra.Domain.Game.Mission;
using Slugterra.Game.Environment;
using Slugterra.Game.Units;

namespace Slugterra.Game.Mission
{
	internal class MissionStatisticsController : MonoSingleton<MissionStatisticsController>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Attacker, UnitController> OnEnemyKilledEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<SlugType> OnEliFireEvent__BackingField;

		public List<SlugType> EliFiresAll = new List<SlugType>();

		private MissionStatistics _model;

		public MissionStatistics Model
		{
			get
			{
				return _model;
			}
		}

		public event Action<Attacker, UnitController> OnEnemyKilledEvent
		{
			add
			{
				Action<Attacker, UnitController> action = OnEnemyKilledEvent__BackingField;
				Action<Attacker, UnitController> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnEnemyKilledEvent__BackingField, (Action<Attacker, UnitController>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<Attacker, UnitController> action = OnEnemyKilledEvent__BackingField;
				Action<Attacker, UnitController> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnEnemyKilledEvent__BackingField, (Action<Attacker, UnitController>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action<SlugType> OnEliFireEvent
		{
			add
			{
				Action<SlugType> action = OnEliFireEvent__BackingField;
				Action<SlugType> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnEliFireEvent__BackingField, (Action<SlugType>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<SlugType> action = OnEliFireEvent__BackingField;
				Action<SlugType> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnEliFireEvent__BackingField, (Action<SlugType>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		protected override void OnCreate()
		{
			base.OnCreate();
			_model = new MissionStatistics();
		}

		public void OnEnemyKilled(Attacker killer, UnitController corpse)
		{
			Model.KilledEnemyCount++;
			if (killer != null && killer.weaponType != null && killer.weaponType.IsSubclassOf(typeof(EnvironmentController)))
			{
				Model.KilledByEnvironment++;
			}
			if (OnEnemyKilledEvent__BackingField != null)
			{
				OnEnemyKilledEvent__BackingField(killer, corpse);
			}
		}

		public void OnEliFire(SlugType slugType)
		{
			if (OnEliFireEvent__BackingField != null)
			{
				OnEliFireEvent__BackingField(slugType);
			}
			EliFiresAll.Add(slugType);
		}
	}
}

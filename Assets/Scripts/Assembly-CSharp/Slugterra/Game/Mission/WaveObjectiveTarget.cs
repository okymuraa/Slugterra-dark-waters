using System.Collections.Generic;
using Slugterra.Domain.Game.Units;
using Slugterra.Game.Units;
using Slugterra.Game.Wave;
using UnityEngine;

namespace Slugterra.Game.Mission
{
	public class WaveObjectiveTarget : ObjectiveTarget
	{
		[SerializeField]
		private Spawner[] _waveControllers;

		private List<Unit> _enemies;

		private bool _isAllWaveFinish
		{
			get
			{
				if (_waveControllers != null && _waveControllers.Length > 0)
				{
					Spawner[] waveControllers = _waveControllers;
					foreach (Spawner spawner in waveControllers)
					{
						if (spawner != null && !spawner.IsAllWaveFinish)
						{
							return false;
						}
					}
					return true;
				}
				return false;
			}
		}

		protected override void OnCreate()
		{
			if (_waveControllers == null)
			{
				Debug.LogWarning("WaveObjectiveTarget: waveControllers is null");
			}
			if (_waveControllers.Length == 0)
			{
				Debug.LogWarning("WaveObjectiveTarget: waveControllers is empty");
			}
			_enemies = new List<Unit>();
		}

		public override void OnObjectChanged(object observable)
		{
			base.OnObjectChanged(observable);
			if (observable is Unit)
			{
				OnUnitModelChanged(observable as Unit);
			}
		}

		private void OnUnitModelChanged(Unit model)
		{
			if (model.IsDead() && _enemies.Contains(model))
			{
				model.RemoveObserver(this);
				_enemies.Remove(model);
			}
		}

		private void OnEnable()
		{
			Spawner[] waveControllers = _waveControllers;
			foreach (Spawner spawner in waveControllers)
			{
				if (spawner != null)
				{
					spawner.OnUnitBornEvent += OnEnemyBorn;
				}
			}
		}

		private void OnDisable()
		{
			Spawner[] waveControllers = _waveControllers;
			foreach (Spawner spawner in waveControllers)
			{
				if (spawner != null)
				{
					spawner.OnUnitBornEvent -= OnEnemyBorn;
				}
			}
		}

		private void OnEnemyBorn(UnitController controller)
		{
			Unit model = controller.Model;
			_enemies.Add(model);
			model.AddObserver(this);
		}

		protected override void OnUpdate()
		{
			if (_isAllWaveFinish && !Completed && Active && _enemies.Count == 0)
			{
				Completed = true;
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core.Command;
using AppsMinistry.Domain.Observer;
using Slugterra.Domain.Game.Units;
using Slugterra.Game.Units;
using UnityEngine;

namespace Slugterra.Game.Wave
{
	public class WaveController : Spawner, IObserver
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<UnitController> OnUnitBornEvent__BackingField;

		[SerializeField]
		private float _baseSpawnInterval;

		[SerializeField]
		private Wave[] _waves;

		private int _currentWaveIndex;

		private float _waveTime;

		private float _nextWaveTime;

		[SerializeField]
		private int _unitLimit = 2;

		private List<UnitController> _spawnedAliveUnits;

		private bool _isEndWave;

		public override bool IsAllWaveFinish
		{
			get
			{
				return _currentWaveIndex >= _waves.Length;
			}
		}

		public override event Action<UnitController> OnUnitBornEvent
		{
			add
			{
				Action<UnitController> action = OnUnitBornEvent__BackingField;
				Action<UnitController> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnUnitBornEvent__BackingField, (Action<UnitController>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<UnitController> action = OnUnitBornEvent__BackingField;
				Action<UnitController> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnUnitBornEvent__BackingField, (Action<UnitController>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public void ResetCurrentWaveIndex()
		{
			_currentWaveIndex = 0;
		}

		private void Start()
		{
			_waveTime = 0f;
			_currentWaveIndex = 0;
			_isEndWave = true;
			_spawnedAliveUnits = new List<UnitController>();
			if (_waves == null || _waves.Length == 0)
			{
				UnityEngine.Debug.LogWarning(string.Format("WaveController::Start {0}: waves is empty", GetType().Name));
			}
		}

		private void Update()
		{
			if (!_isEndWave)
			{
				_waveTime += Time.deltaTime;
				return;
			}
			if (_currentWaveIndex < _waves.Length)
			{
				Wave wave = _waves[_currentWaveIndex];
				if (wave.DelayTime <= _nextWaveTime)
				{
					InitializeWaveCommand initializeWaveCommand = MonoCommand.Execute<InitializeWaveCommand>(new object[3] { _baseSpawnInterval, wave, _spawnedAliveUnits });
					initializeWaveCommand.AsyncToken.AddResponder(new Responder<InitializeWaveCommand>(OnInitializeWaveCommandFinish));
					initializeWaveCommand.transform.parent = transform;
					initializeWaveCommand.OnEnemyBornEvent += OnEnemyBorn;
					_waveTime = 0f;
					_isEndWave = false;
				}
			}
			_nextWaveTime += Time.deltaTime;
		}

		private void OnEnemyBorn(UnitController enemyController)
		{
			_spawnedAliveUnits.Add(enemyController);
			enemyController.Model.AddObserver(this);
			if (OnUnitBornEvent__BackingField != null)
			{
				OnUnitBornEvent__BackingField(enemyController);
			}
		}

		private void OnInitializeWaveCommandFinish(InitializeWaveCommand command)
		{
			_isEndWave = true;
			_currentWaveIndex++;
			_nextWaveTime = 0f;
			command.OnEnemyBornEvent -= OnEnemyBorn;
		}

		public void OnObjectChanged(object observable)
		{
			if (!(observable is Unit))
			{
				return;
			}
			for (int i = 0; i < _spawnedAliveUnits.Count; i++)
			{
				if (_spawnedAliveUnits[i] == null || _spawnedAliveUnits[i].Model.IsDead())
				{
					_spawnedAliveUnits.RemoveAt(i);
					i--;
				}
			}
		}
	}
}

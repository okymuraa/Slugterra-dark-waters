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
	internal class SurvivalWaveController : Spawner, IObserver
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<UnitController> OnUnitBornEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<int> OnWaveFinishedEvent__BackingField;

		[SerializeField]
		private float _baseSpawnInterval;

		[SerializeField]
		private Wave[] _waves;

		private int _currentWaveIndex;

		private float _waveTime;

		private float _nextWaveTime;

		private List<UnitController> _spawnedAliveUnits;

		private bool _isEndWave;

		[SerializeField]
		private float _healthCoeficient;

		[SerializeField]
		private float _attackCoeficient;

		[SerializeField]
		private float _currentHealthCoef;

		[SerializeField]
		private float _currentAttackCoef;

		[SerializeField]
		private bool _isCycling;

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

		public event Action<int> OnWaveFinishedEvent
		{
			add
			{
				Action<int> action = OnWaveFinishedEvent__BackingField;
				Action<int> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnWaveFinishedEvent__BackingField, (Action<int>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<int> action = OnWaveFinishedEvent__BackingField;
				Action<int> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnWaveFinishedEvent__BackingField, (Action<int>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		private void Start()
		{
			_waveTime = 0f;
			_currentWaveIndex = 0;
			_isEndWave = true;
			_spawnedAliveUnits = new List<UnitController>();
			if (_waves == null)
			{
				UnityEngine.Debug.LogWarning(string.Format("SurvivalWaveController::Start {0}: waves is empty", GetType().Name));
			}
		}

		private void Update()
		{
			if (IsAllWaveFinish && !_isCycling)
			{
				enabled = false;
			}
			if (!_isEndWave)
			{
				_waveTime += Time.deltaTime;
			}
			else if (_waves.Length != 0)
			{
				Wave wave = _waves[_currentWaveIndex % _waves.Length];
				if (wave.DelayTime <= _nextWaveTime)
				{
					_currentHealthCoef = (float)_currentWaveIndex * _healthCoeficient + 1f;
					_currentAttackCoef = (float)_currentWaveIndex * _attackCoeficient + 1f;
					InitializeWaveCommand initializeWaveCommand = MonoCommand.Execute<InitializeWaveCommand>(new object[5] { _baseSpawnInterval, wave, _spawnedAliveUnits, _currentHealthCoef, _currentAttackCoef });
					initializeWaveCommand.AsyncToken.AddResponder(new Responder<InitializeWaveCommand>(OnInitializeWaveCommandFinish));
					initializeWaveCommand.transform.parent = transform;
					initializeWaveCommand.OnEnemyBornEvent += OnEnemyBorn;
					_waveTime = 0f;
					_isEndWave = false;
				}
				_nextWaveTime += Time.deltaTime;
			}
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
			if (OnWaveFinishedEvent__BackingField != null)
			{
				OnWaveFinishedEvent__BackingField(_currentWaveIndex);
			}
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

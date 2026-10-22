using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core.Command;
using Slugterra.Game.Units;
using UnityEngine;

namespace Slugterra.Game.Wave
{
	public class InitializeWaveCommand : MonoCommand<InitializeWaveCommand>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<UnitController> OnEnemyBornEvent__BackingField;

		private float _spawnInterval;

		private Wave _wave;

		private List<UnitController> _spawnedAliveUnits;

		private float _nextSpawn;

		private int _currentUnitIndex;

		private Camera _camera;

		private float _healthCoeficient = 1f;

		private float _attackCoeficient = 1f;

		public event Action<UnitController> OnEnemyBornEvent
		{
			add
			{
				Action<UnitController> action = OnEnemyBornEvent__BackingField;
				Action<UnitController> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnEnemyBornEvent__BackingField, (Action<UnitController>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<UnitController> action = OnEnemyBornEvent__BackingField;
				Action<UnitController> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnEnemyBornEvent__BackingField, (Action<UnitController>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		protected override void OnStart(object[] args)
		{
			_spawnInterval = (float)args[0];
			_wave = (Wave)args[1];
			_spawnedAliveUnits = (List<UnitController>)args[2];
			if (args.Length > 3)
			{
				_healthCoeficient = (float)args[3];
			}
			if (args.Length > 4)
			{
				_attackCoeficient = (float)args[4];
			}
			_nextSpawn = Time.time;
			_currentUnitIndex = 0;
			_camera = Camera.main;
			if (_wave.Units == null || _wave.Units.Length == 0)
			{
				Finish();
			}
		}

		private void Update()
		{
			if (_nextSpawn <= Time.time && _spawnedAliveUnits.Count < _wave.UnitLimit && _currentUnitIndex < _wave.Units.Length && CreateUnit())
			{
				_currentUnitIndex++;
				_nextSpawn = Time.time + _spawnInterval;
			}
			if (_currentUnitIndex >= _wave.Units.Length)
			{
				if (!_wave.DefeatAll)
				{
					Finish();
				}
				else if (_spawnedAliveUnits.Count == 0)
				{
					Finish();
				}
			}
		}

		private bool CreateUnit()
		{
			int num = UnityEngine.Random.Range(0, _wave.SpawnPoint.Paths.Length);
			MovePath movePath = _wave.SpawnPoint.Paths[num];
			if (movePath == null || movePath.Points == null || movePath.Points.Count == 0)
			{
				return false;
			}
			if (movePath.NinjaMod)
			{
				Vector3 position = movePath.transform.position;
				Vector3 vector = _camera.ScreenToViewportPoint(_camera.WorldToScreenPoint(position));
				if (0f < vector.x && vector.x < 1f && 0f < vector.y && vector.y < 1f)
				{
					return false;
				}
			}
			GameObject gameObject = UnityEngine.Object.Instantiate(_wave.Units[_currentUnitIndex], movePath.transform.position, Quaternion.identity);
			UnitController componentInChildren = gameObject.GetComponentInChildren<UnitController>();
			if (componentInChildren != null)
			{
				componentInChildren.MovePath = movePath;
				componentInChildren.Model.AddAttackCoeficient(GetType(), _attackCoeficient * _wave.GeneralCoeficient);
				componentInChildren.Model.MaxHealth = componentInChildren.Model.MaxHealth * _healthCoeficient * _wave.GeneralCoeficient;
				componentInChildren.Model.Health = componentInChildren.Model.MaxHealth;
				if (OnEnemyBornEvent__BackingField != null)
				{
					OnEnemyBornEvent__BackingField(componentInChildren);
				}
				return true;
			}
			return false;
		}
	}
}

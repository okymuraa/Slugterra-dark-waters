using System.Collections.Generic;
using AppsMinistry.Core.Sound;
using AppsMinistry.Domain.Observer;
using Slugterra.Domain.Game.Mission;
using Slugterra.UI.Game;
using UnityEngine;

namespace Slugterra.Game.Mission
{
	public class ObjectiveManager : MonoBehaviour, IObserver
	{
		[SerializeField]
		private Transform _baseTransform;

		[SerializeField]
		private float _soundPlayDelay = 1f;

		private float _nextSoundPlay;

		private bool _playSound;

		private SoundContainer _soundConteiner;

		private IList<ObjectiveTarget> _objectiveTargets;

		private IList<ObjectiveTarget> _currentObjectriveTargets;

		private RadarController _radarController;

		private void Awake()
		{
			_radarController = Object.FindObjectOfType<RadarController>();
			if (_radarController != null)
			{
				_radarController.SetBaseTransform(_baseTransform);
			}
		}

		private void Start()
		{
			ObjectiveTarget[] collection = Object.FindObjectsOfType<ObjectiveTarget>();
			_objectiveTargets = new List<ObjectiveTarget>(collection);
			_currentObjectriveTargets = new List<ObjectiveTarget>();
			foreach (ObjectiveTarget objectiveTarget in _objectiveTargets)
			{
				objectiveTarget.Model.AddObserver(this);
				objectiveTarget.Model.CommitAndNotify();
			}
		}

		public void OnObjectChanged(object observable)
		{
			if (observable is Objective)
			{
				OnObjectiveChanged(observable as Objective);
			}
		}

		private void OnObjectiveChanged(Objective objective)
		{
			bool flag = false;
			int i;
			for (i = 0; i < _currentObjectriveTargets.Count; i++)
			{
				if (_currentObjectriveTargets[i].Model.Equals(objective))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				if (!_currentObjectriveTargets[i].Model.IsActive)
				{
					_radarController.RemoveItem(_currentObjectriveTargets[i].transform);
				}
				else
				{
					_radarController.AddItem(_currentObjectriveTargets[i].transform, _currentObjectriveTargets[i].RadarMarker);
				}
				if (objective.IsComplete)
				{
					_radarController.RemoveItem(_currentObjectriveTargets[i].transform);
					_currentObjectriveTargets.RemoveAt(i);
				}
			}
			if (flag || !objective.IsActive || objective.IsComplete)
			{
				return;
			}
			foreach (ObjectiveTarget objectiveTarget in _objectiveTargets)
			{
				if (objectiveTarget.Model.Equals(objective) && objectiveTarget.ShowMarker)
				{
					_currentObjectriveTargets.Add(objectiveTarget);
					_radarController.AddItem(objectiveTarget.transform, _currentObjectriveTargets[i].RadarMarker);
					_soundConteiner = objectiveTarget.ObjectiveSound;
					_playSound = true;
					_nextSoundPlay = 0f;
				}
			}
		}

		private void Update()
		{
			if (!_playSound)
			{
				return;
			}
			if (_soundPlayDelay < _nextSoundPlay)
			{
				if (_soundConteiner != null)
				{
					_soundConteiner.Play();
				}
				_playSound = false;
			}
			_nextSoundPlay += Time.deltaTime;
		}
	}
}

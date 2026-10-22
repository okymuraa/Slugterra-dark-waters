using System.Collections.Generic;
using Slugterra.Domain.Game.Units;
using Slugterra.Domain.Game.Units.Mechanisms;
using Slugterra.Game.Units;
using Slugterra.Game.Units.Mechanisms;
using Slugterra.Game.Units.Mechanisms.Spline;
using Slugterra.UI.Game;
using UnityEngine;

namespace Slugterra.Game.Mission
{
	public class NotDieObjectiveTarget : ObjectiveTarget
	{
		[SerializeField]
		private List<UnitController> _units = new List<UnitController>();

		[SerializeField]
		private List<WagonController> _mechanisms = new List<WagonController>();

		[SerializeField]
		private List<TeamController> _teams = new List<TeamController>();

		private GameController _gameController;

		[SerializeField]
		private float _minHealthInPercent;

		private bool _failed;

		private void OnEnable()
		{
			_gameController.OnCompleteMissionEvent += OnComplete;
		}

		private void OnDisable()
		{
			_gameController.OnCompleteMissionEvent -= OnComplete;
		}

		private void OnComplete()
		{
			if (!_failed)
			{
				Completed = true;
			}
		}

		protected override void OnCreate()
		{
			base.OnCreate();
			_gameController = Object.FindObjectOfType<GameController>();
		}

		protected override void OnStart()
		{
			base.OnStart();
			foreach (UnitController unit in _units)
			{
				unit.Model.AddObserver(this);
			}
			foreach (WagonController mechanism in _mechanisms)
			{
				mechanism.Model.AddObserver(this);
			}
			foreach (TeamController team in _teams)
			{
				team.Model.AddObserver(this);
			}
			_minHealthInPercent = Mathf.Clamp(_minHealthInPercent, 0f, 99f);
		}

		public void SetTarget(WagonController controller)
		{
			_mechanisms.Add(controller);
			controller.Model.AddObserver(this);
		}

		public void SetTarget(TeamController controller)
		{
			_teams.Add(controller);
			controller.Model.AddObserver(this);
		}

		public void SetTarget(UnitController controller)
		{
			_units.Add(controller);
			controller.Model.AddObserver(this);
		}

		private void Clear()
		{
			foreach (UnitController unit in _units)
			{
				unit.Model.RemoveObserver(this);
			}
			foreach (WagonController mechanism in _mechanisms)
			{
				mechanism.Model.RemoveObserver(this);
			}
			foreach (TeamController team in _teams)
			{
				team.Model.RemoveObserver(this);
			}
		}

		public override void OnObjectChanged(object observable)
		{
			base.OnObjectChanged(observable);
			if (observable is Unit)
			{
				OnUnitChanged(observable as Unit);
			}
			else if (observable is Mechanism)
			{
				OnMechanismChanged(observable as Mechanism);
			}
		}

		private void OnUnitChanged(Unit unit)
		{
			if (_minHealthInPercent == 0f)
			{
				if (!unit.IsDead())
				{
					return;
				}
				_gameController.OnFinishGame(false);
			}
			else if (unit.Health > _minHealthInPercent * unit.MaxHealth / 100f)
			{
				return;
			}
			Clear();
			Completed = false;
			_failed = true;
		}

		private void OnMechanismChanged(Mechanism mechanism)
		{
			if (_minHealthInPercent == 0f)
			{
				if (!mechanism.IsDead())
				{
					return;
				}
				_gameController.OnFinishGame(false);
			}
			else if (mechanism.Health > _minHealthInPercent * mechanism.MaxHealth / 100f)
			{
				return;
			}
			Clear();
			Completed = false;
			_failed = true;
		}

		public void Reset()
		{
			if (!Model.IsComplete)
			{
				_failed = false;
			}
		}

		public int count()
		{
			return _units.Count;
		}
	}
}

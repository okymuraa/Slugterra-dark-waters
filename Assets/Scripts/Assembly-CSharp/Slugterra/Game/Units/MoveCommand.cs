using AppsMinistry.Core.Command;
using AppsMinistry.Domain.Observer;
using Slugterra.Domain.Game.Units;
using UnityEngine;

namespace Slugterra.Game.Units
{
	[SingletonCommand]
	public class MoveCommand : MonoCommand<MoveCommand>, IObserver
	{
		private const float HOLD_TIME = 0.2f;

		private UnitController _controller;

		private Transform _target;

		private float _distance;

		private Unit _unitModel;

		private bool _linked;

		private float _prevSqrMagnitude;

		private float _holdTime;

		protected override void OnStart(object[] args)
		{
			_controller = (UnitController)args[0];
			_target = (Transform)args[1];
			if (_target == null)
			{
				Finish();
				return;
			}
			_distance = (float)args[2];
			if (args.Length > 3)
			{
				_linked = (bool)args[3];
			}
			_unitModel = _controller.Model;
			_unitModel.AddObserver(this);
			_distance *= _distance;
			_distance += 0.1f;
			_controller.PathFinder.enabled = true;
			_controller.PathFinder.turningSpeed = 5f;
			_controller.PathFinder.target.position = _target.position;
			_controller.PathFinder.speed = _unitModel.CurrentSpeed;
			_controller.PathFinder.canMove = true;
			_controller.PathFinder.canSearch = true;
			_controller.PathFinder.SearchPath();
			_prevSqrMagnitude = Vector3.SqrMagnitude(_target.position - _controller.Transform.position);
			_holdTime = -1f;
		}

		private void Update()
		{
			if (_linked)
			{
				if (_target == null)
				{
					Finish();
				}
				else
				{
					_controller.PathFinder.target.position = _target.position;
				}
			}
			float num = Vector3.SqrMagnitude(_target.position - _controller.Transform.position);
			if (num <= _distance)
			{
				Finish();
				return;
			}
			if (Mathf.Abs(_prevSqrMagnitude - num) < 0.001f)
			{
				if (_holdTime < 0f)
				{
					_holdTime = Time.time + 0.2f;
				}
				else if (_holdTime < Time.time)
				{
					Finish();
				}
			}
			else
			{
				_holdTime = -1f;
			}
			_prevSqrMagnitude = num;
		}

		protected override void OnFinish()
		{
			if (_controller != null)
			{
				_controller.PathFinder.canMove = false;
			}
			if (_unitModel != null)
			{
				_unitModel.RemoveObserver(this);
			}
		}

		public void OnObjectChanged(object observable)
		{
			if (observable is Unit)
			{
				OnUnitChanged(observable as Unit);
			}
		}

		private void OnUnitChanged(Unit model)
		{
			_controller.PathFinder.speed = _unitModel.CurrentSpeed;
		}
	}
}

using AppsMinistry.Core.Command;
using AppsMinistry.Domain.Observer;
using Slugterra.Domain.Game.Units;

namespace Slugterra.Game.Units.Player
{
	public abstract class MoveStateBase : PlayerState, IObserver
	{
		protected UnitPathFinder _pathFinder;

		protected float _distance;

		public void OnObjectChanged(object observable)
		{
			if (observable as Unit == Controller.Model)
			{
				OnUnitModelChanged(observable as Unit);
			}
		}

		private void OnUnitModelChanged(Unit model)
		{
			if (_pathFinder != null)
			{
				_pathFinder.speed = Controller.Model.CurrentSpeed;
			}
		}

		protected override void OnStart(object[] args)
		{
			base.OnStart(args);
			_distance = (float)args[0];
			_pathFinder = Controller.PathFinder;
			Controller.Model.AddObserver(this);
			Controller.Model.CommitAndNotify();
			if (_pathFinder != null)
			{
				_pathFinder.target.gameObject.SetActive(true);
				_pathFinder.canMove = true;
			}
			MonoCommand.ExecuteOn<WalkAnimationCommand>(gameObject, new object[1] { Controller });
		}

		private void OnEnable()
		{
			WalkAnimationCommand component = GetComponent<WalkAnimationCommand>();
			if (component != null && !component.IsFinished)
			{
				component.enabled = true;
			}
		}

		private void OnDisable()
		{
			WalkAnimationCommand component = GetComponent<WalkAnimationCommand>();
			if (component != null && !component.IsFinished)
			{
				component.enabled = false;
			}
		}

		protected override void OnFinish()
		{
			base.OnFinish();
			Controller.Model.RemoveObserver(this);
		}
	}
}

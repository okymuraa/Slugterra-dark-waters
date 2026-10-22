using AppsMinistry.Core.Command;
using AppsMinistry.Domain.Observer;
using UnityEngine;

namespace Slugterra.Game.Units.Animals.SlugHound
{
	public class MoveState : SlugHoundState, IObserver
	{
		private MoveCommand _moveCommand;

		protected override void OnStart(object[] args)
		{
			base.OnStart(args);
			Controller.PathFinder.enabled = true;
			Controller.Model.AddObserver(this);
			Controller.Model.Walk();
			_moveCommand = MonoCommand.ExecuteOn<MoveCommand>(Controller.GameObject, new object[3]
			{
				Controller,
				Controller.PathFinder.target,
				Controller.PathFinder.endReachedDistance
			});
			_moveCommand.AsyncToken.AddResponder(new Responder<MoveCommand>(OnMoveFinish));
		}

		private void OnMoveFinish(MoveCommand command)
		{
			Controller.Animator.SetFloat("Speed", 0f);
			Controller.PathFinder.target.position = Controller.Transform.position;
			Controller.Model.RemoveObserver(this);
			Finish();
		}

		public void OnObjectChanged(object observable)
		{
			Controller.PathFinder.speed = Controller.Model.CurrentSpeed;
			Controller.Animator.SetFloat("Speed", Controller.Model.CurrentSpeed);
		}

		protected override void OnFinish()
		{
			if (_moveCommand != null)
			{
				Object.Destroy(_moveCommand);
			}
		}
	}
}

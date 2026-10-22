using AppsMinistry.Core.Command;
using AppsMinistry.Domain.Observer;
using UnityEngine;

namespace Slugterra.Game.Units.Animals.DinoRat
{
	public class ChaseState : DinoRatState, IObserver
	{
		private MoveCommand _moveCommand;

		protected override void OnStart(object[] args)
		{
			base.OnStart(args);
			if (Controller.AttackTarget == null)
			{
				Finish();
				return;
			}
			Controller.PathFinder.enabled = true;
			Controller.PathFinder.canSearch = true;
			Controller.PathFinder.repathRate = 0.5f;
			Controller.Model.AddObserver(this);
			Controller.Model.Run();
			Controller.Model.Notify();
			_moveCommand = MonoCommand.ExecuteOn<MoveCommand>(Controller.GameObject, new object[4] { Controller, Controller.AttackTarget, Controller.AttackDistance, true });
			_moveCommand.AsyncToken.AddResponder(new Responder<MoveCommand>(OnMoveFinish));
		}

		private void OnMoveFinish(MoveCommand command)
		{
			Controller.Animator.SetFloat("Speed", 0f);
			Controller.PathFinder.target.position = Controller.Transform.position;
			Controller.Model.RemoveObserver(this);
			Finish();
		}

		protected override void OnUpdate()
		{
			Controller.Animator.SetFloat("Speed", Controller.PathFinder.MoveVelocity.sqrMagnitude);
		}

		public void OnObjectChanged(object observable)
		{
			Controller.PathFinder.speed = Controller.Model.CurrentSpeed;
		}

		protected override void OnFinish()
		{
			if (_moveCommand != null)
			{
				Object.Destroy(_moveCommand);
			}
			Controller.PathFinder.canSearch = false;
		}
	}
}

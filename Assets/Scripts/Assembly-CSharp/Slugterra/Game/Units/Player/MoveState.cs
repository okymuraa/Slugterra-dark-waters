using UnityEngine;

namespace Slugterra.Game.Units.Player
{
	public class MoveState : MoveStateBase
	{
		protected override void OnStart(object[] args)
		{
			base.OnStart(args);
			_pathFinder.SearchPath();
		}

		protected override void OnUpdate()
		{
			if (Vector3.SqrMagnitude(_pathFinder.target.position - Controller.Transform.position) < _distance * _distance)
			{
				StateMachine.NextState<IdleState>(new object[0]);
				Finish();
			}
		}
	}
}

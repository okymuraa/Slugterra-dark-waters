using UnityEngine;

namespace Slugterra.Game.Units.Player
{
	public class ChaseState : MoveStateBase
	{
		private Transform _targetTransform;

		protected override void OnStart(object[] args)
		{
			base.OnStart(args);
			_targetTransform = (Transform)args[1];
			Controller.PathFinder.canSearch = true;
			Controller.PathFinder.repathRate = 0.5f;
		}

		protected override void OnUpdate()
		{
			_pathFinder.target.position = _targetTransform.position;
			if (Vector3.SqrMagnitude(_pathFinder.target.position - Controller.Transform.position) < _distance * _distance)
			{
				StateMachine.NextState<IdleState>(new object[0]);
				Controller.PathFinder.canSearch = false;
				Finish();
			}
		}
	}
}

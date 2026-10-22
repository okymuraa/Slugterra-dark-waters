using AppsMinistry.Core;
using AppsMinistry.Core.Command;
using Slugterra.Game.Duel;

namespace Slugterra.Game.Units.Enemy
{
	public class BossMoveState : EnemyState
	{
		protected override void OnStart(object[] args)
		{
			Move();
		}

		private void Move()
		{
			EnemyDuelPoint randomPoint = MonoSingleton<DuelPointSystem>.Instance.EnemyPointManager.GetRandomPoint();
			Controller.Model.Run();
			MoveCommand moveCommand = MonoCommand.ExecuteOn<MoveCommand>(gameObject, new object[3] { Controller, randomPoint.transform, 0.45f });
			moveCommand.AsyncToken.AddResponder(new Responder<MoveCommand>(OnMoveFinish));
		}

		private void OnMoveFinish(MoveCommand moveCommand)
		{
			if (!Controller.GetComponent<FireCommand>() && !Controller.GetComponent<DuelFireCommand>())
			{
				StateMachine.NextState<BossAttackState>(new object[0]);
			}
			Controller.Animator.SetFloat("VelocityX", 0f);
			Controller.Animator.SetFloat("VelocityY", 0f);
			Finish();
		}

		protected override void OnUpdate()
		{
			Controller.Animator.SetFloat("VelocityX", Controller.PathFinder.MoveVelocity.x);
			Controller.Animator.SetFloat("VelocityY", Controller.PathFinder.MoveVelocity.z);
		}
	}
}

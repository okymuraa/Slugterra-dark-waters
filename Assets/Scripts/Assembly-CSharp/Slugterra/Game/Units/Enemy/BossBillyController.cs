using Slugterra.Configuration;

namespace Slugterra.Game.Units.Enemy
{
	public class BossBillyController : BossController
	{
		public override EnemyType mEnemyType
		{
			get
			{
				return EnemyType.BossBilly;
			}
		}

		protected override void OnStart()
		{
			base.OnStart();
			ViewArea.SetBaseTransform(Animator.transform);
			StateMachine.ChangeState<BossIdleState>(new object[1] { WaitInWarPointTime });
		}
	}
}

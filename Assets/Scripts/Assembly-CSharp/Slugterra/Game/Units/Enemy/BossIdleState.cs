using UnityEngine;

namespace Slugterra.Game.Units.Enemy
{
	public class BossIdleState : EnemyState
	{
		private float _endWait;

		protected override void OnStart(object[] args)
		{
			float num = (float)args[0];
			_endWait = num + Time.time;
		}

		protected override void OnUpdate()
		{
			if (_endWait < Time.time)
			{
				StateMachine.NextState<BossMoveState>(new object[0]);
				Finish();
			}
		}
	}
}

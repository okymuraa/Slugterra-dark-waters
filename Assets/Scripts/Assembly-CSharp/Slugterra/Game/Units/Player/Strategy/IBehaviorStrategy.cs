using UnityEngine;

namespace Slugterra.Game.Units.Player.Strategy
{
	public interface IBehaviorStrategy
	{
		void OnUpdate();

		void OnStateChanged(PlayerState state);

		void OnEnemyKilled();

		void OnStandUp();

		void OnAttackTargetChanged(Transform target);
	}
}

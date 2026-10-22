using UnityEngine;

namespace Slugterra.Game.Units.Player.Strategy
{
	public abstract class BehaviorStrategy : IBehaviorStrategy
	{
		private PlayerController _controller;

		public PlayerController Controller
		{
			get
			{
				return _controller;
			}
		}

		public BehaviorStrategy(PlayerController controller)
		{
			_controller = controller;
		}

		public virtual void OnUpdate()
		{
		}

		public virtual void OnStateChanged(PlayerState state)
		{
		}

		public virtual void OnEnemyKilled()
		{
		}

		public virtual void OnStandUp()
		{
		}

		public virtual void OnAttackTargetChanged(Transform target)
		{
		}
	}
}

using AppsMinistry.Core.Command;
using AppsMinistry.Core.State;
using Slugterra.Configuration;
using Slugterra.Domain.Game.Units;
using Slugterra.UI.Game;
using UnityEngine;

namespace Slugterra.Game.Units.Enemy
{
	public class TwistController : EnemyController
	{
		private GameController gameController;

		public override EnemyType mEnemyType
		{
			get
			{
				return EnemyType.Twist;
			}
		}

		protected override void OnStart()
		{
			base.OnStart();
			Patrol();
			gameController = Object.FindObjectOfType<GameController>();
			if (gameController.IsPlayCutScene)
			{
				ViewArea.SetRadius(0f);
				AttackTarget = null;
			}
		}

		protected override void OnPlayerAreaEnter(Transform item)
		{
			if (!(StateMachine.CurrentState is RunAwayState) && !(StateMachine.CurrentState is DeathState) && AttackTarget == null && item != null)
			{
				AttackTarget = item;
			}
		}

		protected override void OnPlayerAreaExit(Transform item)
		{
			if (!(StateMachine.CurrentState is RunAwayState) && !(StateMachine.CurrentState is DeathState) && AttackTarget != null && AttackTarget.Equals(item))
			{
				Vector3 position = AttackTarget.position;
				if (ViewArea.Items.Count > 0)
				{
					OnPlayerAreaEnter(ViewArea.Items[0]);
					return;
				}
				if (DamageSystem.UnitLastDamage != null)
				{
					OnPlayerAreaEnter(DamageSystem.UnitLastDamage);
					return;
				}
				AttackTarget = null;
				StateMachine.ChangeState<MoveState>(new object[2] { position, Model.RunSpeed });
			}
		}

		protected override void OnAttackTargetChanged()
		{
			base.OnAttackTargetChanged();
			if (AttackTarget != null && !(StateMachine.CurrentState is HideState))
			{
				StateMachine.ChangeState<ChaseState>(new object[4] { AttackTarget, Model, MoveToWarPointChance, WaitInWarPointTime });
			}
		}

		protected override void OnModelChanged(Unit model)
		{
			base.OnModelChanged(model);
		}

		public override void OnKilled()
		{
			base.OnKilled();
			if (!(StateMachine.CurrentState is DeathState))
			{
				StateMachine.ChangeState<DeathState>(new object[0]);
				MonoCommand.ExecuteOn<DestroyUnitCommand>(GameObject, new object[2] { Transform, 0.2f });
			}
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();
			if (!(StateMachine.CurrentState is DeathState) && !(StateMachine.CurrentState is HideState) && !(StateMachine.CurrentState is RunAwayState))
			{
				if (AttackTarget == null && DamageSystem.UnitLastDamage != null)
				{
					AttackTarget = DamageSystem.UnitLastDamage;
				}
				else if (AttackTarget != null && DamageSystem.UnitMaxDamage != null && DamageSystem.UnitMaxDamage != AttackTarget)
				{
					AttackTarget = DamageSystem.UnitMaxDamage;
				}
			}
		}

		public override void Next(MonoState previousState)
		{
			if (previousState is MoveState || previousState is MovePathState)
			{
				Patrol();
			}
			if (previousState is ChaseState && AttackTarget == null)
			{
				Patrol();
			}
		}

		public void Patrol()
		{
			if (AttackTarget != null)
			{
				return;
			}
			if (MovePath != null && MovePathIndex < MovePath.GetPointCount())
			{
				Transform nextPoint = MovePath.GetNextPoint(MovePathIndex);
				if (nextPoint != null)
				{
					StateMachine.ChangeState<MovePathState>(new object[0]);
					return;
				}
			}
			if (PatrolPath == null)
			{
				StateMachine.ChangeState<IdleState>(new object[0]);
			}
			else
			{
				StateMachine.ChangeState<PatrolState>(new object[0]);
			}
		}
	}
}

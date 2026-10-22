using System.Collections;
using AppsMinistry.Core.State;
using AppsMinistry.Utils;
using Slugterra.Configuration;
using UnityEngine;

namespace Slugterra.Game.Units.Enemy.Deadweed
{
	public class DeadweedController : EnemyController
	{
		private const float ATTACK_DISTANCE = 1.5f;

		private DeadweedMecanimDelegate _mecanimDelegate;

		public ParticleSystem noAttackParticle;

		private bool triggerShow = true;

		public override EnemyType mEnemyType
		{
			get
			{
				return EnemyType.DeadWeed;
			}
		}

		public DeadweedMecanimDelegate MecanimDelegate
		{
			get
			{
				return _mecanimDelegate;
			}
		}

		public float AttackDistance
		{
			get
			{
				return 1.5f;
			}
		}

		protected override void OnStart()
		{
			base.OnStart();
			if (PatrolPath == null)
			{
				StateMachine.NextState<IdleState>(new object[0]);
			}
			else
			{
				StateMachine.NextState<PatrolState>(new object[0]);
			}
			_mecanimDelegate = Animator.GetComponent<DeadweedMecanimDelegate>();
		}

		public override void Next(MonoState state)
		{
			if (state is ChaseState)
			{
				PathFinder.target.position = Transform.position;
				PathFinder.enabled = false;
				Model.CurrentSpeed = 0f;
				Model.Notify();
			}
			if (AttackTarget != null && (state is AttackState || state is ChaseState) && !CanAttackTarget(AttackTarget))
			{
				Vector3 normalized = (AttackTarget.position - Transform.position).normalized;
				StateMachine.ChangeState<TurnState>(new object[1] { MathUtil.GetAngle(normalized.z, normalized.x) });
			}
			else if (AttackTarget != null)
			{
				if (CanAttackTarget(AttackTarget))
				{
					StateMachine.NextState<AttackState>(new object[0]);
				}
				else
				{
					StateMachine.ChangeState<ChaseState>(new object[0]);
				}
			}
			else
			{
				Patrol();
			}
		}

		protected override void OnPlayerAreaExit(Transform item)
		{
			if (!Model.IsDead() && item == AttackTarget)
			{
				AttackTarget = null;
				if (StateMachine.CurrentState != null)
				{
					StateMachine.CurrentState.Finish();
				}
			}
		}

		public void Patrol()
		{
			if (AttackTarget != null)
			{
				return;
			}
			transform.GetComponent<UnitPathFinder>().enabled = true;
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

		protected override void OnPlayerAreaEnter(Transform item)
		{
			if (AttackTarget == null && item != null)
			{
				AttackTarget = item;
			}
			if (!(StateMachine.CurrentState is ChaseState))
			{
				StateMachine.ChangeState<ChaseState>(new object[0]);
			}
		}

		public bool CanAttackTarget(Transform attackTarget)
		{
			if (attackTarget == null)
			{
				return false;
			}
			Vector3 normalized = (attackTarget.position - Transform.position).normalized;
			float f = Mathf.DeltaAngle(Transform.eulerAngles.y, MathUtil.GetAngle(normalized.z, normalized.x));
			if (Mathf.Abs(f) > 50f)
			{
				return false;
			}
			if (Vector3.SqrMagnitude(Transform.position - attackTarget.position) > 2.4750001f)
			{
				return false;
			}
			return true;
		}

		protected override void OnAttackTargetChanged()
		{
		}

		private IEnumerator IconNoAttackShow()
		{
			triggerShow = false;
			noAttackParticle.Play();
			yield return new WaitForSeconds(4f);
			triggerShow = true;
		}

		public override void OnDamage(float damage, Attacker attacker)
		{
			if (triggerShow)
			{
				StartCoroutine(IconNoAttackShow());
			}
			base.OnDamage(damage, attacker);
			if (AttackTarget == null && attacker != null && enabled)
			{
				MonoState currentState = StateMachine.CurrentState;
				StateMachine.Terminate();
				Next(currentState);
			}
		}

		public override void OnKilled()
		{
			base.OnKilled();
			Object.Destroy(GameObject);
		}
	}
}

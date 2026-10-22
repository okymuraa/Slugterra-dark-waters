using System;
using AppsMinistry.Core.Command;
using AppsMinistry.Core.State;
using AppsMinistry.Utils;
using Slugterra.Configuration;
using Slugterra.Domain.Game.Units;
using Slugterra.Game.Units.Mechanisms;
using Slugterra.Game.Units.Mechanisms.Spline;
using UnityEngine;

namespace Slugterra.Game.Units.Animals.SlugScorpion
{
	[Serializable]
	public class SlugScorpionController : AnimalController
	{
		private MecanimDelegate _mecanimDelegate;

		private float _sqrAttackDistance;

		private bool _isEvasion;

		public override AnimalType mAnimalType
		{
			get
			{
				return AnimalType.SlugScorpion;
			}
		}

		public MecanimDelegate MecanimDelegate
		{
			get
			{
				return _mecanimDelegate;
			}
		}

		public override Transform AttackTarget
		{
			get
			{
				return base.AttackTarget;
			}
			set
			{
				if (value != null)
				{
					if (value.GetComponent<TeamController>() == null && value.GetComponent<MechanismController>() == null)
					{
						base.AttackTarget = value;
					}
				}
				else
				{
					base.AttackTarget = value;
				}
			}
		}

		protected override void OnStart()
		{
			base.OnStart();
			_attackDistance = 1.3f;
			_sqrAttackDistance = _attackDistance * _attackDistance;
			Patrol();
			_isEvasion = false;
			_mecanimDelegate = Animator.GetComponent<MecanimDelegate>();
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

		private void OnEnable()
		{
			ViewArea.OnTriggerEnterEvent += OnEnemyEnter;
			ViewArea.OnTriggerExitEvent += OnEnemyExit;
		}

		private void OnDisable()
		{
			ViewArea.OnTriggerEnterEvent -= OnEnemyEnter;
			ViewArea.OnTriggerExitEvent -= OnEnemyExit;
		}

		protected virtual void OnEnemyEnter(Transform item)
		{
			if (!Model.IsDead() && Aggresion == Aggression.Aggressive && AttackTarget == null)
			{
				AttackTarget = item;
				if (StateMachine.CurrentState != null)
				{
					StateMachine.CurrentState.Finish();
				}
			}
		}

		protected virtual void OnEnemyExit(Transform item)
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

		protected override void OnUpdate()
		{
			if (!Model.IsDead() && DamageSystem.UnitMaxDamage != null && AttackTarget == null)
			{
				AttackTarget = DamageSystem.UnitMaxDamage;
			}
		}

		public override void Next(MonoState state)
		{
			if (Model.IsDead())
			{
				StateMachine.NextState<DeathState>(new object[0]);
				MonoCommand.ExecuteOn<DestroyUnitCommand>(GameObject, new object[2] { Transform, 0.2f });
				return;
			}
			if (state is MoveState || state is ChaseState)
			{
				PathFinder.target.position = Transform.position;
				PathFinder.enabled = false;
				Model.CurrentSpeed = 0f;
			}
			if (_isEvasion)
			{
				_isEvasion = false;
				StateMachine.NextState<EvasionState>(new object[0]);
			}
			else if (state is MoveState)
			{
				StateMachine.NextState<IdleState>(new object[0]);
			}
			else if (state is AttackState || state is EvasionState)
			{
				if (AttackTarget != null)
				{
					if (CanHit(AttackTarget))
					{
						StateMachine.NextState<AttackState>(new object[0]);
						return;
					}
					Vector3 normalized = (AttackTarget.position - Transform.position).normalized;
					StateMachine.NextState<TurnState>(new object[1] { MathUtil.GetAngle(normalized.z, normalized.x) });
				}
				else
				{
					Patrol();
				}
			}
			else if (state is ChaseState)
			{
				if (AttackTarget != null)
				{
					if (CanHit(AttackTarget))
					{
						StateMachine.NextState<AttackState>(new object[0]);
						return;
					}
					Vector3 normalized2 = (AttackTarget.position - Transform.position).normalized;
					StateMachine.NextState<TurnState>(new object[1] { MathUtil.GetAngle(normalized2.z, normalized2.x) });
				}
				else
				{
					Patrol();
				}
			}
			else if (state is TurnState)
			{
				if (AttackTarget != null)
				{
					if (CanHit(AttackTarget))
					{
						StateMachine.NextState<AttackState>(new object[0]);
					}
					else
					{
						StateMachine.NextState<ChaseState>(new object[0]);
					}
				}
				else
				{
					Patrol();
				}
			}
			else if (AttackTarget != null)
			{
				StateMachine.NextState<ChaseState>(new object[0]);
			}
			else
			{
				Patrol();
			}
		}

		public override void OnDamage(float damage, Attacker attackedUnit)
		{
			if (Model.IsDead() || StateMachine.CurrentState is EvasionState)
			{
				return;
			}
			float num = UnityEngine.Random.Range(0f, 1f);
			if (num <= (Model as Animal).Evasion)
			{
				MonoState currentState = StateMachine.CurrentState;
				if (currentState != null)
				{
					currentState.Finish();
				}
				_isEvasion = true;
			}
			else
			{
				(Model as Animal).Health -= damage;
				base.OnDamage(damage, attackedUnit);
				if (!Model.IsDead() && !(StateMachine.CurrentState is EvasionState))
				{
					Animator.SetTrigger("TakeHit");
				}
			}
		}

		public override void OnKilled()
		{
			base.OnKilled();
			if (!(StateMachine.CurrentState == null) && !(StateMachine.CurrentState is DeathState))
			{
				StateMachine.CurrentState.Finish();
			}
		}

		public bool CanHit(Transform attackTarget)
		{
			if (attackTarget == null)
			{
				return false;
			}
			Vector3 normalized = (attackTarget.position - Transform.position).normalized;
			float f = Mathf.DeltaAngle(Transform.eulerAngles.y, MathUtil.GetAngle(normalized.z, normalized.x));
			if (Mathf.Abs(f) > 25f)
			{
				return false;
			}
			if (Vector3.SqrMagnitude(Transform.position - attackTarget.position) > _sqrAttackDistance * 1.1f)
			{
				return false;
			}
			return true;
		}
	}
}

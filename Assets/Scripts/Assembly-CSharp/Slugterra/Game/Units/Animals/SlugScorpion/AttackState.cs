using Slugterra.Domain.Game.Units;
using UnityEngine;

namespace Slugterra.Game.Units.Animals.SlugScorpion
{
	[MecanimState(new string[] { "Attack" })]
	public class AttackState : SlugScorpionState
	{
		private Transform _attackTarget;

		private Animator _animator;

		protected override void OnStart(object[] args)
		{
			if (Controller.CanAttack)
			{
				base.OnStart(args);
				_attackTarget = Controller.AttackTarget;
				_animator = Controller.Animator;
				_animator.SetTrigger("Attack");
				Controller.PathFinder.enabled = false;
				Controller.MecanimDelegate.OnAttackEvent += OnAttack;
			}
		}

		private void OnDisable()
		{
			Controller.MecanimDelegate.OnAttackEvent -= OnAttack;
		}

		private void OnAttack()
		{
			if (Controller.CanHit(_attackTarget))
			{
				UnitController component = _attackTarget.GetComponent<UnitController>();
				component.OnDamage((Controller.Model as Animal).Damage, new Attacker(component.GetType(), component.Transform));
			}
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();
			AnimatorStateInfo currentAnimatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
			if (IsPlay && !currentAnimatorStateInfo.IsName(MecanimState.Name))
			{
				Finish();
			}
		}
	}
}

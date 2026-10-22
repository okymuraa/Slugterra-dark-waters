using Slugterra.Domain.Game.Units;
using UnityEngine;

namespace Slugterra.Game.Units.Enemy.Deadweed
{
	public class AttackState : DeadweedState
	{
		private Transform _attackTarget;

		private bool _isPlay;

		private Animator _animator;

		protected override void OnStart(object[] args)
		{
			_attackTarget = Controller.AttackTarget;
			_isPlay = false;
			_animator = Controller.Animator;
			_animator.SetTrigger("Attack");
			Controller.PathFinder.enabled = false;
			Controller.MecanimDelegate.OnAttackEvent += OnAttack;
		}

		private void OnDisable()
		{
			Controller.MecanimDelegate.OnAttackEvent -= OnAttack;
		}

		private void OnAttack()
		{
			_isPlay = true;
			if (Controller.CanAttackTarget(_attackTarget))
			{
				UnitController component = _attackTarget.GetComponent<UnitController>();
				component.OnDamage((Controller.Model as Humanoid).MeleeDamage, new Attacker(component.GetType(), component.Transform));
			}
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();
			AnimatorStateInfo currentAnimatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
			if (_isPlay && !currentAnimatorStateInfo.IsName("Attack"))
			{
				Finish();
			}
		}
	}
}

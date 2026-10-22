using AppsMinistry.Core.Command;
using Slugterra.Configuration;
using Slugterra.Domain.Game.Units;
using Slugterra.Domain.Game.Units.Slugs;
using UnityEngine;

namespace Slugterra.Game.Units.Ally.Assistant
{
	public class KordStrategy
	{
		private KordController _controller;

		private float _allyAreaRadius;

		private const float PRIVATE_ELI_RADIUS = 1f;

		private const float MAX_DISTANCE_TO_TARGET = 400f;

		public KordController Controller
		{
			get
			{
				return _controller;
			}
		}

		public KordStrategy(KordController controller)
		{
			_controller = controller;
		}

		public void OnStart()
		{
			Component allyArea = Controller.AllyArea;
			_allyAreaRadius = allyArea.GetComponent<SphereCollider>().radius;
			Controller.StateMachine.ChangeState<IdleState>(new object[0]);
		}

		public void OnUpdate()
		{
			if (Controller.AttackTarget == null)
			{
				AimAtAggressor();
			}
			if (Controller.PlayerController.AttackTarget != null && Controller.AttackTarget != Controller.PlayerController.AttackTarget)
			{
				Controller.AttackTarget = Controller.PlayerController.AttackTarget;
			}
			if (Controller.AttackTarget == null)
			{
				if (Vector3.SqrMagnitude(Controller.PlayerTransform.position - Controller.Transform.position) < Mathf.Pow(1f, 2f) && Controller.StateMachine.CurrentState is IdleState)
				{
					Controller.StateMachine.ChangeState<ManeuverState>(new object[0]);
				}
			}
			else
			{
				if (Controller.NextFire > Time.time)
				{
					return;
				}
				Attack();
				float range = (Controller.Model as Humanoid).Blaster.Range;
				float num = Vector3.SqrMagnitude(Controller.AttackTarget.position - Controller.Transform.position);
				float num2 = 0.5f;
				float num3 = Vector3.SqrMagnitude(Controller.AttackTarget.position - Controller.PlayerTransform.position);
				if (num3 > 400f && Controller.PlayerController.AttackTarget == null)
				{
					Controller.AttackTarget = null;
					return;
				}
				float num4 = FreedomCalculate();
				if (Mathf.Pow(range + num4 - num2, 2f) > num3 && Controller.StateMachine.CurrentState is IdleState && num > Mathf.Pow(range, 2f))
				{
					Controller.StateMachine.ChangeState<MoveState>(new object[3]
					{
						Controller.Model.WalkSpeed,
						range - num2,
						Controller.AttackTarget
					});
				}
			}
		}

		private void AimAtAggressor()
		{
			if (Controller.DamageSystem.UnitLastDamage != null)
			{
				Transform unitLastDamage = Controller.DamageSystem.UnitLastDamage;
				UnitController component = unitLastDamage.GetComponent<UnitController>();
				if (component != null && !component.Model.IsDead())
				{
					Controller.AttackTarget = unitLastDamage;
				}
			}
			else if (Controller.DamageSystem.UnitMaxDamage != null)
			{
				Transform unitMaxDamage = Controller.DamageSystem.UnitMaxDamage;
				UnitController component2 = unitMaxDamage.GetComponent<UnitController>();
				if (component2 != null && !component2.Model.IsDead())
				{
					Controller.AttackTarget = unitMaxDamage;
				}
			}
		}

		private float FreedomCalculate()
		{
			Vector3 vector = Controller.AttackTarget.position - Controller.Transform.position;
			Vector3 a = Controller.PlayerTransform.position - Controller.Transform.position;
			float f = Vector2.Angle(new Vector2(vector.normalized.x, vector.normalized.z), new Vector2(a.normalized.x, a.normalized.z));
			float num = Mathf.Pow(Mathf.Sin(f), 2f) * Vector3.SqrMagnitude(a);
			return Mathf.Sqrt(_allyAreaRadius * _allyAreaRadius - num);
		}

		private void Attack()
		{
			Humanoid humanoid = Controller.Model as Humanoid;
			SlugType shotSlug = Controller.GetShotSlug();
			Slug slug = humanoid.GetSlug(shotSlug);
			if (!Controller.IsCanUseSlug(slug) && slug.Available)
			{
				slug = humanoid.GetSlug(SlugType.Flopper);
				if (slug.NextFire > Time.time)
				{
					return;
				}
			}
			if (Vector3.SqrMagnitude(Controller.AttackTarget.position - Controller.Transform.position) < Mathf.Pow(humanoid.Blaster.Range, 2f))
			{
				slug.Available = false;
				MonoCommand.ExecuteOn<FireCommand>(Controller.GameObject, new object[2] { Controller, slug });
				if (Controller.StateMachine.CurrentState is IdleState)
				{
					Controller.StateMachine.ChangeState<ManeuverState>(new object[0]);
				}
				Controller.UpdateNextFire();
			}
		}
	}
}

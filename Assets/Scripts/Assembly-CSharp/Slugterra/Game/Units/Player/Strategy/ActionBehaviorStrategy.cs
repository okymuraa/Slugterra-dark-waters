using System;
using System.Collections.Generic;
using AppsMinistry.Core;
using AppsMinistry.Core.Command;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain;
using Slugterra.Domain.Game.Units;
using Slugterra.Game.Units.Enemy.Deadweed;
using Slugterra.Scene;
using UnityEngine;

namespace Slugterra.Game.Units.Player.Strategy
{
	public class ActionBehaviorStrategy : BehaviorStrategy
	{
		private Camera _camera;

		private FireCommand _fireCommand;

		public ActionBehaviorStrategy(PlayerController controller)
			: base(controller)
		{
			_camera = Camera.main;
		}

		public override void OnUpdate()
		{
			if (Controller.AttackTarget == null)
			{
				AimAtAggressor();
			}
			if (Controller.PickUpArea.Items.Count > 0)
			{
				Controller.OnPickUp();
			}
			if (Controller.NextSlug == null)
			{
				Controller.SetNextSlug(SlugType.Flopper);
			}
			if (Controller.IsTPCMode)
			{
				Type scene = MonoSingleton<AppController>.Instance.GetCurrentGameLevel().Scene;
				if (scene != typeof(ShopSceneController) && scene != typeof(BackpackSceneController) && scene != typeof(MultiplayerSceneController) && Controller.AttackTarget == null && Controller.NextReaim < Time.time && Controller.GameController.Model.State == GameState.Play && Controller.UseAutoattack)
				{
					UpdateTarget();
				}
			}
			if (Controller.AttackTarget == null)
			{
				return;
			}
			if (!Controller.AttackTarget.gameObject.activeSelf)
			{
				Controller.AttackTarget = null;
				return;
			}
			Collider component = Controller.AttackTarget.GetComponent<Collider>();
			if (component == null || (component != null && !component.enabled))
			{
				Controller.AttackTarget = null;
			}
			else if (!(Controller.NextFire > Time.time))
			{
				if (!Controller.IsReloadSlug && !Controller.IsIgnorEnemy && _fireCommand == null && Controller.IsCanUseSlug(Controller.NextSlug))
				{
					Attack();
				}
				if (!Controller.IsTPCMode)
				{
					CheckAttackTargetInFOV();
				}
			}
		}

		private void UpdateTarget()
		{
			Controller.ViewArea.UpdateTargets();
			Transform transform = null;
			List<Transform> list = Controller.BuildTargetsPriorityList();
			list.RemoveAll((Transform t) => t.GetComponent<DeadweedController>() != null);
			List<Transform> list2 = list.FindAll(Controller.CheckTargetInFOV);
			if (list2.Count > 0)
			{
				transform = list2[0];
				IDestructible component = transform.GetComponent<IDestructible>();
				if (component is UnitController)
				{
					Controller.AttackTarget = transform;
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

		private void CheckAttackTargetInFOV()
		{
			if (!(_camera == null) && Controller.AttackTarget != null)
			{
				Vector3 vector = _camera.WorldToViewportPoint(Controller.AttackTarget.position);
				if (vector.x < -0.01f || 1.01f < vector.x || vector.y < -0.01f || 1.01f < vector.y)
				{
					Controller.AttackTarget = null;
				}
			}
		}

		public override void OnStateChanged(PlayerState state)
		{
			if (state is RollState)
			{
				if (Controller.RotateToTarget)
				{
					Controller.RotateToTarget = false;
				}
			}
			else if (!(state is MoveState))
			{
				Controller.IsIgnorEnemy = false;
			}
		}

		public override void OnEnemyKilled()
		{
			base.OnEnemyKilled();
			Controller.PlayCheerSound();
		}

		protected virtual void Attack()
		{
			if (Controller.NextSlug != null && Controller.CanAttack && Vector3.SqrMagnitude(Controller.AttackTarget.position - Controller.Transform.position) < Mathf.Pow((Controller.Model as Humanoid).Blaster.Range, 2f))
			{
				bool flag = true;
				if (Controller.NextSlug.Type == SlugType.Crystalid)
				{
					flag = false;
				}
				_fireCommand = MonoCommand.ExecuteOn<FireCommand>(Controller.GameObject, new object[3] { Controller, Controller.NextSlug, flag });
			}
		}
	}
}

using System.Collections.Generic;
using AppsMinistry.Core;
using AppsMinistry.Core.Command;
using AppsMinistry.Core.State;
using Slugterra.Configuration;
using Slugterra.Game.Duel;
using Slugterra.Game.Environment;
using UnityEngine;

namespace Slugterra.Game.Units.Player.Strategy
{
	public class DuelBehaviourStrategy : BehaviorStrategy
	{
		private List<ArenaInteractiveEnvironmant> _interactiveObjects;

		private const float bossTallfix = 1f;

		public DuelBehaviourStrategy(PlayerController controller)
			: base(controller)
		{
			_interactiveObjects = new List<ArenaInteractiveEnvironmant>();
			ArenaInteractiveEnvironmant[] array = Object.FindObjectsOfType<ArenaInteractiveEnvironmant>();
			ArenaInteractiveEnvironmant[] array2 = array;
			foreach (ArenaInteractiveEnvironmant arenaInteractiveEnvironmant in array2)
			{
				if (Mathf.Abs(arenaInteractiveEnvironmant.transform.position.z - Controller.Transform.position.z) > 10f)
				{
					_interactiveObjects.Add(arenaInteractiveEnvironmant);
				}
			}
		}

		public override void OnUpdate()
		{
			if (Controller.NextFire <= Time.time)
			{
				Attack();
			}
		}

		public override void OnStateChanged(PlayerState state)
		{
			if (state is IdleState)
			{
				Controller.AttackTarget = MonoSingleton<DuelManager>.Instance.BossTransform;
			}
			else if (state is RollState)
			{
				Controller.StateMachine.CurrentState.AsyncToken.AddResponder(new Responder<MonoState>(OnRollFinish));
			}
		}

		private void Attack()
		{
			if (Controller.AttackTarget == null || !Controller.AttackTarget.gameObject.activeSelf)
			{
				return;
			}
			if (Controller.NextSlug == null)
			{
				Controller.SetNextSlug(SlugType.Flopper);
			}
			if (Controller.IsCanUseSlug(Controller.NextSlug) && Controller.NextSlug.Available)
			{
				if (Controller.AttackTarget != MonoSingleton<DuelManager>.Instance.BossTransform)
				{
					FireDefault();
				}
				else if (Controller.NextSlug.Type != SlugType.Flopper)
				{
					FireDuel();
				}
			}
		}

		protected virtual void FireDuel()
		{
			MonoCommand.ExecuteOn<DuelFireCommand>(Controller.GameObject, new object[2] { Controller, Controller.NextSlug });
			Controller.UpdateNextFire();
		}

		protected virtual void FireDefault()
		{
			FireCommand fireCommand = MonoCommand.ExecuteOn<FireCommand>(Controller.GameObject, new object[4] { Controller, Controller.NextSlug, false, 2f });
			fireCommand.AsyncToken.AddResponder(new Responder<FireCommand>(OnFireCommandFinish));
			Controller.UpdateNextFire();
		}

		protected void OnFireCommandFinish(FireCommand command)
		{
			if (Controller.enabled)
			{
				Controller.AttackTarget = MonoSingleton<DuelManager>.Instance.BossTransform;
			}
		}

		private void OnRollFinish(MonoState rollState)
		{
			Controller.Model.Walk();
			Controller.StateMachine.ChangeState<MoveState>(new object[1] { 0.5f });
		}

		public override void OnEnemyKilled()
		{
		}

		public override void OnStandUp()
		{
			Controller.AttackTarget = MonoSingleton<DuelManager>.Instance.BossTransform;
		}
	}
}

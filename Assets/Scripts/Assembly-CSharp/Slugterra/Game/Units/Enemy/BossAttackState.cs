using System.Collections.Generic;
using AppsMinistry.Core;
using AppsMinistry.Core.Command;
using Slugterra.Configuration;
using Slugterra.Domain.Game.Units;
using Slugterra.Domain.Game.Units.Slugs;
using Slugterra.Game.Duel;
using Slugterra.Game.Environment;
using UnityEngine;

namespace Slugterra.Game.Units.Enemy
{
	public class BossAttackState : EnemyState
	{
		private float _stateTime;

		private const float playerTallfix = 1f;

		protected override void OnStart(object[] args)
		{
			_stateTime = Time.time + 0.4f;
			Attack();
		}

		protected override void OnUpdate()
		{
			if (_stateTime < Time.time)
			{
				StateMachine.NextState<BossIdleState>(new object[1] { Controller.WaitInWarPointTime });
				Finish();
			}
		}

		private void Attack()
		{
			if (!(Controller as BossController).IsCanUseSlug())
			{
				StateMachine.NextState<BossIdleState>(new object[1] { Controller.WaitInWarPointTime });
				Finish();
				return;
			}
			SlugType shotSlug = Controller.GetShotSlug();
			bool flag = false;
			int num = 0;
			if (shotSlug == SlugType.Flopper)
			{
				flag = TryShootFlopper();
			}
			if (!flag)
			{
				List<SlugType> list = new List<SlugType>(Controller.AvailableSlugs);
				do
				{
					int index = Random.Range(0, list.Count);
					shotSlug = list[index];
					list.RemoveAt(index);
					flag = TryShootSlug(shotSlug);
					num++;
				}
				while (!flag && num < Controller.AvailableSlugs.Count);
			}
			StateMachine.NextState<BossIdleState>(new object[1] { Controller.WaitInWarPointTime });
			Finish();
		}

		private bool TryShootSlug(SlugType type)
		{
			Slug slug = (Controller.Model as Humanoid).GetSlug(type);
			if (Controller.IsCanUseSlug(slug) && slug.Available)
			{
				MonoCommand.ExecuteOn<DuelFireCommand>(gameObject, new object[2] { Controller, slug });
				return true;
			}
			return false;
		}

		private bool TryShootFlopper()
		{
			Slug slug = (Controller.Model as Humanoid).GetSlug(SlugType.Flopper);
			if (slug == null)
			{
				return false;
			}
			if (Controller.IsCanUseSlug(slug) || slug.Available)
			{
				List<ArenaInteractiveEnvironmant> interactiveObjects = (Controller as BossController).InteractiveObjects;
				if (interactiveObjects != null)
				{
					foreach (ArenaInteractiveEnvironmant item in interactiveObjects)
					{
						if (item == null || !item.gameObject.activeSelf || !item.IsReady || !(Vector3.SqrMagnitude(MonoSingleton<DuelManager>.Instance.PlayerController.Transform.position + Vector3.up * 1f - item.transform.position) < item.Range * item.Range))
						{
							continue;
						}
						if (Controller.AttackTarget == MonoSingleton<DuelManager>.Instance.PlayerController.Transform)
						{
							Controller.AttackTarget = item.transform;
						}
						FireCommand fireCommand = MonoCommand.ExecuteOn<FireCommand>(gameObject, new object[6] { Controller, slug, null, 0.5f, false, 2f });
						fireCommand.AsyncToken.AddResponder(new Responder<FireCommand>((Controller as BossController).OnFireCommandFinish));
						Controller.UpdateNextFire();
						return true;
					}
				}
			}
			return false;
		}
	}
}

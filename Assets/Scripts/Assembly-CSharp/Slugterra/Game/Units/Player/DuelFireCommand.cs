using AppsMinistry.Core;
using Slugterra.Configuration;
using Slugterra.Game.Duel;
using Slugterra.Game.Mission;
using Slugterra.Game.Units.Slugs;
using UnityEngine;

namespace Slugterra.Game.Units.Player
{
	public class DuelFireCommand : FireCommand
	{
		protected override void Fire()
		{
			if (_controller.AttackTarget != null)
			{
				GameObject gameObject = SpawnSlug(_slug.Type, GetShotPoint(), Quaternion.identity);
				gameObject.layer = LayerMask.NameToLayer("AllySlugLayer");
				gameObject.transform.LookAt(MonoSingleton<DuelManager>.Instance.BossTransform.position);
				SlugController component = gameObject.GetComponent<SlugController>();
				MonoSingleton<DuelManager>.Instance.StateMachine.ChangeState<SlugFlyState>(new object[1] { component });
				component.SetSlug(_slug);
				int num = LayerMask.NameToLayer("EnemyLayer");
				int num2 = LayerMask.NameToLayer("NeutralLayer");
				component.SetStrategy(new DuelAllySlugStrategy(component, new int[2] { num, num2 }, MonoSingleton<DuelManager>.Instance.BossTransform));
				component.Parent = _controller;
				_controller.Animator.SetTrigger("OnFire");
				_controller.PlaySlugShotSound(_slug.Type);
				_controller.SetNextSlug(SlugType.None);
				MonoSingleton<DuelManager>.Instance.SlugSmoothFollow.SetTarget(gameObject.transform);
				MonoSingleton<DuelManager>.Instance.AddAllySlug(component);
				_slug.Available = false;
				MonoSingleton<MissionStatisticsController>.Instance.OnEliFire(_slug.Type);
				_controller.ShootCounter--;
			}
		}
	}
}

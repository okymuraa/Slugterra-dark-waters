using System.Collections;
using AppsMinistry.Core;
using AppsMinistry.Core.Command;
using Slugterra.Domain.Game.Units.Slugs;
using Slugterra.Game.Duel;
using Slugterra.Game.Units.Slugs;
using UnityEngine;

namespace Slugterra.Game.Units.Enemy
{
	public class DuelFireCommand : MonoCommand<DuelFireCommand>
	{
		private EnemyController _controller;

		private Slug _slug;

		private GameObject _shotPrefab;

		protected override void OnStart(object[] args)
		{
			_controller = (EnemyController)args[0];
			_slug = (Slug)args[1];
			_shotPrefab = GameFactory.GetSlugPrefab(_slug.Type);
			StartCoroutine(Fire(0.2f, args));
		}

		private IEnumerator Fire(float waitTime, object[] args)
		{
			yield return new WaitForSeconds(waitTime);
			if (_controller.AttackTarget != null && _slug.Available)
			{
				GameObject gameObject = Object.Instantiate(position: (!(_controller.ShotPoint != null)) ? (_controller.Transform.position + Vector3.up * 0.5f) : _controller.ShotPoint.position, original: _shotPrefab, rotation: Quaternion.identity);
				gameObject.layer = LayerMask.NameToLayer("EnemySlugLayer");
				gameObject.transform.LookAt(_controller.AttackTarget.position);
				SlugController component = gameObject.GetComponent<SlugController>();
				component.SetSlug(_slug);
				int num = LayerMask.NameToLayer("AllyLayer");
				int num2 = LayerMask.NameToLayer("NeutralLayer");
				component.SetStrategy(new DuelEnemySlugStrategy(component, new int[2] { num, num2 }, _controller.AttackTarget));
				component.Parent = _controller;
				MonoSingleton<DuelManager>.Instance.AddEnemySlug(component);
				_controller.Animator.SetTrigger("OnFire");
				_controller.PlaySlugShotSound(_slug.Type);
				(_controller as BossController).SetUsedSlug(_slug);
			}
			Finish();
		}
	}
}

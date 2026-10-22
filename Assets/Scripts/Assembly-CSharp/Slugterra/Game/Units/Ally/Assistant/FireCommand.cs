using AppsMinistry.Core.Command;
using Slugterra.Configuration;
using Slugterra.Domain.Game.Units.Slugs;
using Slugterra.Game.Units.AreaTrigger;
using Slugterra.Game.Units.Slugs;
using UnityEngine;

namespace Slugterra.Game.Units.Ally.Assistant
{
	public class FireCommand : MonoCommand<FireCommand>
	{
		private AssistantController _controller;

		private Slug _slug;

		private float _fireTime;

		protected override void OnStart(object[] args)
		{
			_controller = (AssistantController)args[0];
			_slug = (Slug)args[1];
			_fireTime = Time.time + 0.2f;
		}

		private void Update()
		{
			if (_fireTime < Time.time)
			{
				Fire();
				Finish();
			}
		}

		private void Fire()
		{
			if (!(_controller.AttackTarget != null))
			{
				return;
			}
			Transform transform = null;
			HitMarkers currentVisibleMarkers = _controller.ViewArea.CurrentVisibleMarkers;
			if (currentVisibleMarkers.Markers.Count != 0)
			{
				transform = currentVisibleMarkers.GetRandomMarker();
				Vector3 forward = transform.position - _controller.ShotPoint.position;
				Quaternion rotation = Quaternion.LookRotation(forward);
				GameObject slugPrefab = GameFactory.GetSlugPrefab(_slug.Type);
				GameObject gameObject = Object.Instantiate(slugPrefab, _controller.ShotPoint.position, rotation);
				gameObject.layer = LayerMask.NameToLayer("AllySlugLayer");
				SlugController component = gameObject.GetComponent<SlugController>();
				component.SetVelocity(forward.normalized);
				component.SetSlug(_slug);
				int num = LayerMask.NameToLayer("EnemyLayer");
				int num2 = LayerMask.NameToLayer("NeutralLayer");
				int num3 = LayerMask.NameToLayer("PassiveLayer");
				component.SetStrategy(new AllySlugStrategy(component, new int[3] { num, num2, num3 }));
				component.Parent = _controller;
				component.Target = transform;
				_controller.Animator.SetTrigger("OnFire");
				_controller.PlaySlugShotSound(_slug.Type);
				if (_slug.Type == SlugType.Flopper)
				{
					_controller.ReloadSlug(_slug);
				}
			}
		}
	}
}

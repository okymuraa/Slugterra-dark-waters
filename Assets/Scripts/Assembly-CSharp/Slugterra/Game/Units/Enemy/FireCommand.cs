using AppsMinistry.Core.Command;
using Slugterra.Configuration;
using Slugterra.Domain.Game.Units.Slugs;
using Slugterra.Game.Units.AreaTrigger;
using Slugterra.Game.Units.Slugs;
using UnityEngine;

namespace Slugterra.Game.Units.Enemy
{
	public class FireCommand : MonoCommand<FireCommand>
	{
		private EnemyController _controller;

		private Slug _slug;

		private float _fireTime;

		private GameObject _effect;

		private Transform _hitTransform;

		protected bool _checkVisibility = true;

		protected float _projectileSpeedCoef = 1f;

		protected override void OnStart(object[] args)
		{
			_controller = (EnemyController)args[0];
			if (_controller.AttackTarget == null)
			{
				Finish();
				return;
			}
			_fireTime = Time.time + 0.5f;
			_slug = (Slug)args[1];
			GameObject gameObject = null;
			if (args.Length > 2)
			{
				gameObject = args[2] as GameObject;
			}
			if (gameObject != null)
			{
				_effect = Object.Instantiate(gameObject, _controller.ShotPoint.position, Quaternion.identity);
				_effect.transform.parent = _controller.ShotPoint;
			}
			if (args.Length > 3)
			{
				_fireTime = Time.time + (float)args[3];
			}
			if (args.Length > 4)
			{
				_checkVisibility = (bool)args[4];
			}
			if (args.Length > 5)
			{
				_projectileSpeedCoef = (float)args[5];
			}
			if (_checkVisibility)
			{
				HitMarkers currentVisibleMarkers = _controller.ViewArea.CurrentVisibleMarkers;
				if (currentVisibleMarkers.Markers.Count != 0)
				{
					_hitTransform = currentVisibleMarkers.GetRandomMarker();
				}
				else
				{
					Finish();
				}
			}
			else
			{
				HitMarker component = _controller.AttackTarget.GetComponent<HitMarker>();
				_hitTransform = component.GetRandomMarker();
			}
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
			if (_controller.AttackTarget != null && _hitTransform != null && _controller.ShotPoint != null)
			{
				Vector3 normalized = (_hitTransform.position - _controller.ShotPoint.position).normalized;
				Quaternion rotation = Quaternion.LookRotation(normalized);
				GameObject slugPrefab = GameFactory.GetSlugPrefab(_slug.Type);
				GameObject gameObject = Object.Instantiate(slugPrefab, _controller.ShotPoint.position, rotation);
				SlugController component = gameObject.GetComponent<SlugController>();
				SlugStrategy strategy = null;
				component.SetVelocity(normalized);
				component.SetSlug(_slug);
				_slug.Available = false;
				if (LayerMask.LayerToName(_controller.GameObject.layer) == "EnemyLayer")
				{
					gameObject.layer = LayerMask.NameToLayer("EnemySlugLayer");
					int num = LayerMask.NameToLayer("AllyLayer");
					int num2 = LayerMask.NameToLayer("NeutralLayer");
					int num3 = LayerMask.NameToLayer("PassiveLayer");
					strategy = new EnemySlugStrategy(component, new int[3] { num, num2, num3 });
				}
				else if (LayerMask.LayerToName(_controller.GameObject.layer) == "AllyLayer")
				{
					gameObject.layer = LayerMask.NameToLayer("AllySlugLayer");
					int num4 = LayerMask.NameToLayer("EnemyLayer");
					int num5 = LayerMask.NameToLayer("NeutralLayer");
					int num6 = LayerMask.NameToLayer("PassiveLayer");
					strategy = new AllySlugStrategy(component, new int[3] { num4, num5, num6 });
				}
				component.SetStrategy(strategy);
				component.Parent = _controller;
				component.Target = _hitTransform;
				_controller.Animator.SetTrigger("OnFire");
				_controller.PlaySlugShotSound(_slug.Type);
				if (_slug.Type == SlugType.Flopper)
				{
					_controller.ReloadSlug(_slug);
				}
			}
		}

		protected override void OnFinish()
		{
			if (_effect != null)
			{
				Object.Destroy(_effect);
			}
		}
	}
}

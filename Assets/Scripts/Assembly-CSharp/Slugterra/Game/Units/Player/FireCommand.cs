using AppsMinistry.Core;
using AppsMinistry.Core.Command;
using Slugterra.Configuration;
using Slugterra.Domain.Game.Units.Slugs;
using Slugterra.Game.Mission;
using Slugterra.Game.Units.AreaTrigger;
using Slugterra.Game.Units.Slugs;
using UnityEngine;

namespace Slugterra.Game.Units.Player
{
	public class FireCommand : MonoCommand<FireCommand>
	{
		[SerializeField]
		protected PlayerController _controller;

		protected Slug _slug;

		protected bool _checkVisibility = true;

		protected float _projectileSpeedCoef = 1f;

		protected Transform _target;

		private float _fireTime;

		public PlayerController Controller
		{
			get
			{
				return _controller;
			}
		}

		protected override void OnStart(object[] args)
		{
			_controller = (PlayerController)args[0];
			_slug = (Slug)args[1];
			if (args.Length > 2)
			{
				_checkVisibility = (bool)args[2];
			}
			if (args.Length > 3)
			{
				_projectileSpeedCoef = (float)args[3];
			}
			_fireTime = Time.time + 0.2f;
			_controller.FireCommand = this;
		}

		private void Update()
		{
			if (_fireTime < Time.time)
			{
				Fire();
				Finish();
			}
		}

		protected virtual Transform GetTarget()
		{
			Transform transform = null;
			if (_checkVisibility)
			{
				HitMarkers currentVisibleMarkers = _controller.ViewArea.CurrentVisibleMarkers;
				if (currentVisibleMarkers != null && currentVisibleMarkers.Markers.Count != 0)
				{
					transform = currentVisibleMarkers.GetRandomMarker();
					return currentVisibleMarkers.GetRandomMarker();
				}
				return null;
			}
			HitMarker component = _controller.AttackTarget.GetComponent<HitMarker>();
			return component.GetRandomMarker();
		}

		protected virtual Vector3 GetShotPoint()
		{
			if (_controller.ShotPoint != null)
			{
				return _controller.ShotPoint.position;
			}
			return _controller.Transform.position + Vector3.up;
		}

		protected virtual void Fire()
		{
			if (!(_controller.AttackTarget != null))
			{
				return;
			}
			_target = GetTarget();
			if (_target != null)
			{
				GameObject gameObject = SpawnSlug(_slug.Type, GetShotPoint(), Quaternion.identity);
				if ((bool)_controller.Animator.GetComponent<IKControl>())
				{
					_controller.Animator.GetComponent<IKControl>().ResetWeight();
				}
				Vector3 normalized = (_target.position - _controller.ShotPoint.position).normalized;
				gameObject.transform.rotation = Quaternion.LookRotation(normalized);
				gameObject.layer = LayerMask.NameToLayer("AllySlugLayer");
				SlugController component = gameObject.GetComponent<SlugController>();
				_controller.RealisedSlugs.Add(component);
				component.SetVelocity(normalized);
				component.SetSlug(_slug);
				int num = LayerMask.NameToLayer("EnemyLayer");
				int num2 = LayerMask.NameToLayer("NeutralLayer");
				int num3 = LayerMask.NameToLayer("PassiveLayer");
				component.SetStrategy(new AllySlugStrategy(component, new int[3] { num, num2, num3 }));
				component.Parent = _controller;
				component.Target = _target;
				component.ProjectileSpeedCoef = _projectileSpeedCoef;
				_controller.Animator.SetTrigger("OnFire");
				_controller.PlaySlugShotSound(_slug.Type);
				_controller.SetNextSlug(SlugType.None);
				_slug.Available = false;
				if (_slug.Type == SlugType.Flopper)
				{
					_controller.ReloadSlug(_slug);
				}
				MonoSingleton<MissionStatisticsController>.Instance.OnEliFire(_slug.Type);
				_controller.ShootCounter--;
				_controller.UpdateNextFire();
			}
		}

		protected virtual GameObject SpawnSlug(SlugType slugType, Vector3 position, Quaternion rotation)
		{
			GameObject slugPrefab = GameFactory.GetSlugPrefab(slugType);
			return Object.Instantiate(slugPrefab, position, rotation);
		}

		protected override void OnFinish()
		{
			base.OnFinish();
			_controller.FireCommand = null;
		}
	}
}

using System.Collections.Generic;
using AppsMinistry.Core.Command;
using UnityEngine;

namespace Slugterra.Game.Units.Slugs
{
	public abstract class SlugStrategy : ISlugStrategy
	{
		private SlugController _controller;

		protected int[] _layers;

		private List<SlugAbility> _abilities;

		protected int _maxCollisionCount = 1;

		public SlugController Controller
		{
			get
			{
				return _controller;
			}
		}

		public int MaxCollisionCount
		{
			set
			{
				_maxCollisionCount = value;
			}
		}

		public SlugStrategy(SlugController controller, int[] layers)
		{
			_controller = controller;
			_layers = layers;
			SlugAbility[] components = Controller.GetComponents<SlugAbility>();
			_abilities = new List<SlugAbility>(components);
		}

		public virtual bool OnTriggerEnter(Transform transform)
		{
			return false;
		}

		public virtual void OnHit(Transform transform)
		{
		}

		public void OnUseAbility(Transform transform)
		{
			foreach (SlugAbility ability in _abilities)
			{
				ability.OnUse(transform);
			}
		}

		public virtual void OnUpdate()
		{
		}

		public virtual void OnReleaseResource()
		{
		}

		protected void SplashDamage(Transform transform)
		{
			Attacker attacker = new Attacker(Controller.Parent.GetType(), Controller.Parent.Transform, Controller.GetType(), Controller.Model.Type);
			MonoCommand.Execute<AreaDamageCommand>(new object[6]
			{
				Controller.Transform.position,
				Controller.Model.HittingArea,
				Controller.Model.Damage,
				_layers,
				attacker,
				transform
			});
		}

		public bool CollisionWithLevelItem(Transform transform)
		{
			if (transform.gameObject.layer == LayerMask.NameToLayer("Terrain") || transform.gameObject.layer == LayerMask.NameToLayer("PropLayer"))
			{
				return true;
			}
			return false;
		}
	}
}

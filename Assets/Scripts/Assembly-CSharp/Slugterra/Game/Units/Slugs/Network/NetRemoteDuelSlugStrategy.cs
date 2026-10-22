using UnityEngine;

namespace Slugterra.Game.Units.Slugs.Network
{
	public class NetRemoteDuelSlugStrategy : SlugStrategy
	{
		public NetRemoteDuelSlugStrategy(SlugController controller, int[] layers)
			: base(controller, layers)
		{
		}

		public override bool OnTriggerEnter(Transform transform)
		{
			int[] layers = _layers;
			foreach (int num in layers)
			{
				if (transform.gameObject.layer == num)
				{
					Controller.OnExplosion();
					OnUseAbility(transform);
					if (Controller.IsSplash)
					{
						SplashDamage(transform);
					}
					else
					{
						OnHit(transform);
					}
					return true;
				}
			}
			if (CollisionWithLevelItem(transform))
			{
				Controller.OnExplosion();
				OnUseAbility(transform);
				if (Controller.IsSplash)
				{
					SplashDamage(transform);
				}
				return true;
			}
			return false;
		}

		public override void OnHit(Transform transform)
		{
			IDestructible component = transform.GetComponent<IDestructible>();
			if (component != null)
			{
				component.OnDamage(Controller.Damage, new Attacker(Controller.Parent.GetType(), Controller.Parent.Transform, Controller.GetType(), Controller.Model.Type));
			}
		}
	}
}

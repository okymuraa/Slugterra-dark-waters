using UnityEngine;

namespace Slugterra.Game.Units.Slugs.Network
{
	public class NetSlugController : SlugController
	{
		private Collider _collider;

		public override void CheckCollision(Collider collider)
		{
			IDestructible component = collider.transform.GetComponent<IDestructible>();
			if (component != null)
			{
				_collider = collider;
			}
		}

		public void NetSelfDestruction()
		{
			float num = Mathf.Abs((_collider.transform.position - transform.position).magnitude);
			if (num < 1.6f && _collider != null && (_strategy == null || _strategy.OnTriggerEnter(_collider.transform)))
			{
				Target = null;
			}
			SelfDestruction();
		}

		public override void SelfDestruction(float timer = 0f)
		{
			Object.Destroy(gameObject, timer);
		}
	}
}

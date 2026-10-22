using UnityEngine;

namespace Slugterra.Game.Units.Slugs
{
	public interface ISlugStrategy
	{
		bool OnTriggerEnter(Transform transform);

		void OnHit(Transform transform);

		void OnUseAbility(Transform transform);

		void OnUpdate();

		void OnReleaseResource();
	}
}

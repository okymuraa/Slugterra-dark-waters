using UnityEngine;

namespace Slugterra.Game
{
	public interface IDestructible
	{
		void OnDamage(float damage, Attacker attacker = null);

		Renderer GetRenderer();
	}
}

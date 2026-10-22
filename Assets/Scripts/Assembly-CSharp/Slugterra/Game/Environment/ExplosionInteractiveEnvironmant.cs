using Slugterra.Game.Units;
using UnityEngine;

namespace Slugterra.Game.Environment
{
	internal class ExplosionInteractiveEnvironmant : EnvironmentAbility
	{
		[SerializeField]
		private float _damage = 1f;

		public override void ApplyEffect(UnitController unitController, Attacker parent)
		{
			unitController.OnDamage(_damage, parent);
		}
	}
}

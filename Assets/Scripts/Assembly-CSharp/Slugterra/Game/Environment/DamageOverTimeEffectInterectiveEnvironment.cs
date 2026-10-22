using AppsMinistry.Core.Command;
using Slugterra.Game.Units;
using Slugterra.Game.Units.Slugs;
using UnityEngine;

namespace Slugterra.Game.Environment
{
	public class DamageOverTimeEffectInterectiveEnvironment : EnvironmentAbility
	{
		[SerializeField]
		private float _effectTimes = 1f;

		public float _damagePerSeconds = 20f;

		public GameObject ParticleEffect;

		[SerializeField]
		private float _chance = 1f;

		public override void ApplyEffect(UnitController unitController, Attacker parent)
		{
			float num = Random.Range(0f, 1f);
			if (!(num >= _chance))
			{
				MonoCommand.ExecuteOn<DamageOverTimeEffect>(unitController.GameObject, new object[5] { unitController, _effectTimes, _damagePerSeconds, parent, ParticleEffect });
			}
		}
	}
}

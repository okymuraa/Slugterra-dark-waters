using AppsMinistry.Core.Command;
using Slugterra.Game.Units;
using Slugterra.Game.Units.Slugs;
using UnityEngine;

namespace Slugterra.Game.Environment
{
	internal class SlowSpeedInteractiveEnvironmant : EnvironmentAbility
	{
		[SerializeField]
		private float _effectTimes = 1f;

		[SerializeField]
		private float _effectDeceys = 1f;

		[SerializeField]
		private GameObject _particleEffect;

		protected GameObject ParticleEffect
		{
			get
			{
				return _particleEffect;
			}
		}

		public override void ApplyEffect(UnitController unitController, Attacker parent)
		{
			MonoCommand.ExecuteOn<SlowSpeedEffect>(unitController.GameObject, new object[4] { unitController.Model, _effectTimes, _effectDeceys, ParticleEffect });
		}
	}
}

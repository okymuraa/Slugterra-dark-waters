using AppsMinistry.Core.Command;
using Slugterra.Game.Units;
using Slugterra.Game.Units.Slugs;
using UnityEngine;

namespace Slugterra.Game.Environment
{
	public class FearInterectiveEnvironment : EnvironmentAbility
	{
		[SerializeField]
		private float _effectTimes = 1f;

		public GameObject ParticleEffect;

		public override void ApplyEffect(UnitController unitController, Attacker parent)
		{
			MonoCommand.ExecuteOn<FearEffect>(unitController.GameObject, new object[3] { unitController, _effectTimes, ParticleEffect });
			if (unitController is HumanoidController)
			{
				MonoCommand.ExecuteOn<AmmoFearEffect>(unitController.GameObject, new object[2] { unitController, _effectTimes });
			}
		}
	}
}

using AppsMinistry.Core.Command;
using Slugterra.Game.Units;
using Slugterra.Game.Units.Slugs;
using UnityEngine;

namespace Slugterra.Game.Environment
{
	public class KnockBackInterectiveEnvironment : EnvironmentAbility
	{
		[SerializeField]
		private float _effectTimes = 1f;

		[SerializeField]
		private float _effectDeceys = 1f;

		public override void ApplyEffect(UnitController unitController, Attacker parent)
		{
			MonoCommand.ExecuteOn<KnockBackEffect>(unitController.GameObject, new object[4] { unitController, _effectTimes, _effectDeceys, null });
		}
	}
}

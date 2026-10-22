using AppsMinistry.Core;
using Slugterra.Game.Units;

namespace Slugterra.Game.Environment
{
	public abstract class EnvironmentAbility : BaseMonoBehaviour
	{
		public abstract void ApplyEffect(UnitController unitController, Attacker parent);
	}
}

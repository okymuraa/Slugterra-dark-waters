using Slugterra.Game.Units.Slugs;

namespace Slugterra.Game.Units
{
	public class AntiNegativeAbility : BoostEffect
	{
		protected override void OnStart(object[] args)
		{
			base.OnStart(args);
			UnitEffect[] components = gameObject.GetComponents<UnitEffect>();
			UnitEffect[] array = components;
			foreach (UnitEffect unitEffect in array)
			{
				unitEffect.Finish();
			}
			Finish();
		}
	}
}

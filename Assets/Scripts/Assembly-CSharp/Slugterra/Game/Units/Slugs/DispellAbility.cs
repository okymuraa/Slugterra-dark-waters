using UnityEngine;

namespace Slugterra.Game.Units.Slugs
{
	public class DispellAbility : SlugAbility
	{
		protected override void OnUsePositiveEffect(Transform unitTransform)
		{
			base.OnUsePositiveEffect(unitTransform);
			UnitController component = unitTransform.GetComponent<UnitController>();
			if (component != null && component.Model != null)
			{
				DamageOverTimeEffect component2 = component.GetComponent<DamageOverTimeEffect>();
				if (component2 != null)
				{
					component2.Finish();
				}
				StunEffect component3 = component.GetComponent<StunEffect>();
				if (component3 != null)
				{
					component3.Finish();
				}
				SlowSpeedEffect component4 = component.GetComponent<SlowSpeedEffect>();
				if (component4 != null)
				{
					component4.Finish();
				}
				AmmoFearEffect component5 = component.GetComponent<AmmoFearEffect>();
				if (component5 != null)
				{
					component5.Finish();
				}
				KnockBackEffect component6 = component.GetComponent<KnockBackEffect>();
				if (component6 != null)
				{
					component6.Finish();
				}
				BlindEffect component7 = component.GetComponent<BlindEffect>();
				if (component7 != null)
				{
					component7.Finish();
				}
			}
		}
	}
}

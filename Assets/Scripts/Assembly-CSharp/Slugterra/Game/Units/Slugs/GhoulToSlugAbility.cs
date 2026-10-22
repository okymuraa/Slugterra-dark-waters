using System.Collections.Generic;
using System.Linq;
using Slugterra.Domain.Game.Units;
using Slugterra.Domain.Game.Units.Slugs;
using UnityEngine;

namespace Slugterra.Game.Units.Slugs
{
	public class GhoulToSlugAbility : SlugAbility
	{
		protected override void OnUseNegativeEffect(Transform unitTransform)
		{
			base.OnUseNegativeEffect(unitTransform);
			UnitController component = unitTransform.GetComponent<UnitController>();
			if (!(component != null))
			{
				return;
			}
			HumanoidController humanoidController = component as HumanoidController;
			if (!(humanoidController != null))
			{
				return;
			}
			Humanoid humanoid = humanoidController.Model as Humanoid;
			List<Slug> list = humanoid.Slugs.Values.Where((Slug slug) => slug.IsGhoul).ToList();
			for (int num = 0; num < list.Count; num++)
			{
				for (int num2 = 0; num2 < humanoidController.AvailableSlugs.Count; num2++)
				{
					if (humanoidController.AvailableSlugs[num2] == list[num].GhoulType)
					{
						humanoidController.AvailableSlugs[num2] = list[num].SlugType;
					}
				}
				humanoid.Slugs.Remove(list[num].GhoulType);
				humanoid.Slugs.Add(list[num].SlugType, new Slug(list[num].SlugType, list[num].Level));
				humanoid.CommitAndNotify();
			}
		}
	}
}

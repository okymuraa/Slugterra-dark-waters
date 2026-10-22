using Slugterra.Configuration;
using Slugterra.Game.Units.Player;
using UnityEngine;

namespace Slugterra.Game.Units.Ally
{
	public class EliView : PlayerView
	{
		public override void SetArmor(ArmorType type)
		{
			if (PlayerRenderer != null)
			{
				Material armorMaterial = GameFactory.GetArmorMaterial(type);
				if (armorMaterial != null)
				{
					PlayerRenderer.material = armorMaterial;
				}
			}
		}

		public void UseMaskGhouled()
		{
			Material material = Resources.Load("Materials/Units/Ally/Eli/EliGhouledMaterial") as Material;
			if (material != null)
			{
				PlayerRenderer.material = material;
			}
		}
	}
}

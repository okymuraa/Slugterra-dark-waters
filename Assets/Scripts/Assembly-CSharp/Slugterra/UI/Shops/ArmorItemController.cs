using Slugterra.Domain.Shops;
using Slugterra.Game;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class ArmorItemController : ShopItemController
	{
		private ArmorItem armorItem;

		protected override GameObject CreatePreview(object model)
		{
			armorItem = model as ArmorItem;
			if (armorItem != null)
			{
				GameObject gameObject = Object.Instantiate(GameFactory.GetShopArmor());
				SkinnedMeshRenderer componentInChildren = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
				if (componentInChildren != null)
				{
					componentInChildren.material = GameFactory.GetArmorMaterial(armorItem.Type);
				}
				if (gameObject != null)
				{
					shopItemShaderProxy = gameObject.AddComponent<ShopItemShaderProxy>();
					shopItemShaderProxy.Initialize();
					if (armorItem.IsLocked && !armorItem.IsBought)
					{
						shopItemShaderProxy.SetGraySclae(0f);
					}
				}
				return gameObject;
			}
			return null;
		}

		protected override void OnApplyModel(object model)
		{
			ObjectChanged(model);
		}

		protected override void ObjectChanged(object observable)
		{
			armorItem = observable as ArmorItem;
			if (armorItem != null && shopItemShaderProxy != null)
			{
				if (armorItem.IsLocked)
				{
					shopItemShaderProxy.SetGraySclae(0f);
				}
				else
				{
					shopItemShaderProxy.SetGraySclae(1f);
				}
			}
		}
	}
}

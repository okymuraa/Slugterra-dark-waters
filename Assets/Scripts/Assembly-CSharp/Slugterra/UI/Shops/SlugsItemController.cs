using Slugterra.Domain.Shops;
using Slugterra.Game;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class SlugsItemController : ShopItemController
	{
		private SlugItem slugItem;

		protected override GameObject CreatePreview(object model)
		{
			slugItem = model as SlugItem;
			if (slugItem != null)
			{
				GameObject gameObject = GameFactory.GetSlugPrefabUI(slugItem.Type);
				if (gameObject != null)
				{
					gameObject = Object.Instantiate(gameObject);
					shopItemShaderProxy = gameObject.AddComponent<ShopItemShaderProxy>();
					shopItemShaderProxy.Initialize();
					if (slugItem.IsLocked && !slugItem.IsBought)
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
			slugItem = observable as SlugItem;
			if (slugItem != null && shopItemShaderProxy != null)
			{
				if (slugItem.IsLocked)
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

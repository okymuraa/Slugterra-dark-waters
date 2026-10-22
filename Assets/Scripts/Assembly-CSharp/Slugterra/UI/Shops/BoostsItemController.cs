using Slugterra.Domain.Shops;
using Slugterra.Game;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class BoostsItemController : ShopItemController
	{
		private ConsumableItem consumableItem;

		protected override GameObject CreatePreview(object model)
		{
			consumableItem = model as ConsumableItem;
			if (consumableItem != null)
			{
				GameObject gameObject = GameFactory.GetConsumable(consumableItem.Type);
				if (gameObject != null)
				{
					gameObject = Object.Instantiate(gameObject);
					shopItemShaderProxy = gameObject.AddComponent<ShopItemShaderProxy>();
					shopItemShaderProxy.Initialize();
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
			consumableItem = observable as ConsumableItem;
			if (consumableItem != null && !(shopItemShaderProxy != null))
			{
			}
		}
	}
}

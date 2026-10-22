using Slugterra.Domain.Shops;
using Slugterra.Game;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class BlasterItemController : ShopItemController
	{
		private BlasterItem blasterItem;

		protected override GameObject CreatePreview(object model)
		{
			blasterItem = model as BlasterItem;
			if (blasterItem != null)
			{
				GameObject blasterPrefabUI = GameFactory.GetBlasterPrefabUI(blasterItem.Type);
				GameObject gameObject = Object.Instantiate(blasterPrefabUI);
				gameObject.transform.localScale = Vector3.one * 2f;
				if (gameObject != null)
				{
					shopItemShaderProxy = gameObject.AddComponent<ShopItemShaderProxy>();
					shopItemShaderProxy.Initialize();
					if (blasterItem.IsLocked)
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
			blasterItem = observable as BlasterItem;
			if (blasterItem != null && shopItemShaderProxy != null)
			{
				if (blasterItem.IsLocked)
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

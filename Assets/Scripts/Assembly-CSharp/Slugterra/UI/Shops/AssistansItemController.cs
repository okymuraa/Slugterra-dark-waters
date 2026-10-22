using Slugterra.Domain.Shops;
using Slugterra.Game;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class AssistansItemController : ShopItemController
	{
		private AssistantItem assistantItem;

		protected override GameObject CreatePreview(object model)
		{
			assistantItem = model as AssistantItem;
			if (assistantItem != null)
			{
				GameObject gameObject = GameFactory.GetAssistantUI(assistantItem.Type);
				if (gameObject != null)
				{
					gameObject = Object.Instantiate(gameObject);
					shopItemShaderProxy = gameObject.AddComponent<ShopItemShaderProxy>();
					shopItemShaderProxy.Initialize();
					if (assistantItem.IsLocked && !assistantItem.IsBought)
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
			assistantItem = observable as AssistantItem;
			if (assistantItem != null && shopItemShaderProxy != null)
			{
				if (assistantItem.IsLocked)
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

using Slugterra.Configuration;
using Slugterra.Game;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class EliItemController : ShopItemController
	{
		public static readonly Vector3 PlatformScale = new Vector3(0.6f, 0.6f, 0.6f);

		public static readonly Vector3 PlatformRotation = new Vector3(8f, 180f, 0f);

		public static readonly Vector3 EliScale = new Vector3(1.6f, 1.6f, 1.6f);

		protected override GameObject CreatePreview(object model)
		{
			GameObject eliShopPlatform = GameFactory.GetEliShopPlatform();
			GameObject assistantUI = GameFactory.GetAssistantUI(AllyType.Eli);
			if (eliShopPlatform != null)
			{
				eliShopPlatform = Object.Instantiate(eliShopPlatform);
				EliPlatformView component = eliShopPlatform.GetComponent<EliPlatformView>();
				if (assistantUI != null)
				{
					assistantUI = Object.Instantiate(assistantUI);
					assistantUI.transform.localScale = EliScale;
					EliShopView component2 = assistantUI.GetComponent<EliShopView>();
					component2.ApplyModel(model);
					component.Eli = component2;
				}
				eliShopPlatform.transform.localScale = PlatformScale;
				eliShopPlatform.transform.eulerAngles = PlatformRotation;
				return eliShopPlatform;
			}
			return null;
		}
	}
}

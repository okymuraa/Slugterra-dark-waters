using Slugterra.Domain.Shops;
using Slugterra.Game;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class FoeItemController : ShopItemController
	{
		private FoeItem _foeItem;

		protected override GameObject CreatePreview(object model)
		{
			_foeItem = model as FoeItem;
			if (_foeItem != null)
			{
				GameObject gameObject = GameFactory.GetFoeUI(_foeItem.Type);
				if (gameObject != null)
				{
					gameObject = Object.Instantiate(gameObject);
					shopItemShaderProxy = gameObject.AddComponent<ShopItemShaderProxy>();
					shopItemShaderProxy.Initialize();
					if (_foeItem.IsLocked && !_foeItem.IsBought)
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
			_foeItem = observable as FoeItem;
			if (_foeItem != null && shopItemShaderProxy != null)
			{
				if (_foeItem.IsLocked)
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

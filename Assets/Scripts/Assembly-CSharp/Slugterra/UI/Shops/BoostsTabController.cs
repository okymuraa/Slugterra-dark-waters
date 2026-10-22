using System.Collections.Generic;
using Slugterra.Domain.Shops;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class BoostsTabController : ShopTabController
	{
		[SerializeField]
		private BoostItemFacade _itemFacade;

		protected override void _OnEnable()
		{
			base._OnEnable();
			_itemFacade.gameObject.SetActive(true);
		}

		protected override void _OnDisable()
		{
			base._OnDisable();
			_itemFacade.gameObject.SetActive(false);
		}

		public override void ApplyModel(object model)
		{
			base.ApplyModel(model);
			if (model is ShopModel)
			{
				List<ConsumableItem> consumables = (model as ShopModel).Consumables;
				Transform parent = View.ShopItemsScrollScontroller.transform;
				View.ShopItemsScrollScontroller.ApplyModel(ShopItemController.CreateList<BoostsItemController, ConsumableItem>(consumables, parent));
			}
		}

		protected override void OnObjectSelected(ShopItemController item)
		{
			base.OnObjectSelected(item);
			if (item != null)
			{
				_itemFacade.ApplyModel(item.Model);
			}
		}
	}
}

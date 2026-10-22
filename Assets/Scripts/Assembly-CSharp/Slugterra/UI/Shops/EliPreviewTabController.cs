using System.Collections.Generic;
using AppsMinistry.UI;
using Slugterra.Domain.App.Settings;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class EliPreviewTabController : ShopTabController
	{
		[SerializeField]
		private EliItemFacade _itemFacade;

		[SerializeField]
		private ScrollController _scrollController;

		[SerializeField]
		private TabIconController _anotherTabIconController;

		private bool allowInputTemp;

		protected override void _OnEnable()
		{
			base._OnEnable();
			_itemFacade.gameObject.SetActive(true);
			allowInputTemp = _scrollController.allowInput;
			_scrollController.allowInput = false;
			if (_anotherTabIconController != null)
			{
				_anotherTabIconController.Active = true;
			}
		}

		protected override void _OnDisable()
		{
			base._OnDisable();
			_itemFacade.gameObject.SetActive(false);
			_scrollController.allowInput = allowInputTemp;
			if (_anotherTabIconController != null)
			{
				_anotherTabIconController.Active = false;
			}
		}

		public override void ApplyModel(object model)
		{
			base.ApplyModel(model);
			if (model is MissionSetting)
			{
				EliItemController item = ShopItemController.Create<EliItemController>(model as MissionSetting, View.ShopItemsScrollScontroller.transform);
				List<ShopItemController> list = new List<ShopItemController>();
				list.Add(item);
				View.ShopItemsScrollScontroller.ApplyModel(list);
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

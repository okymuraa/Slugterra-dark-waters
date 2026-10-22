using System;
using System.Collections.Generic;
using AppsMinistry.UI;
using Slugterra.Domain.Shops;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class ShopGameView : UIView
	{
		public Action<List<GameConsumableController>> listAlreadyFilled;

		[SerializeField]
		private tk2dUIScrollableArea _scrollArea;

		[SerializeField]
		private GameObject _gameShopItemPrefab;

		[SerializeField]
		private List<GameConsumableController> _consumablesItemUI = new List<GameConsumableController>();

		public List<GameConsumableController> ConsumablesItemUI
		{
			get
			{
				return _consumablesItemUI;
			}
		}

		protected override void OnStart()
		{
			base.OnStart();
		}

		protected override void OnApplyModel(object model)
		{
			base.OnApplyModel(model);
			if (!(model is ShopModel))
			{
				return;
			}
			_consumablesItemUI.Clear();
			ShopModel shopModel = model as ShopModel;
			if (_scrollArea.contentContainer.transform.childCount == 0)
			{
				List<ConsumableItem> gameShopConsumables = shopModel.GetGameShopConsumables();
				for (int i = 0; i < gameShopConsumables.Count; i++)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(_gameShopItemPrefab);
					gameObject.transform.parent = _scrollArea.contentContainer.transform;
					gameObject.transform.localPosition = new Vector3(0f, (float)i * -2.86f, -0.01f);
					GameConsumableController component = gameObject.GetComponent<GameConsumableController>();
					component.ApplyModel(gameShopConsumables[i]);
					_consumablesItemUI.Add(component);
				}
				if (listAlreadyFilled != null)
				{
					listAlreadyFilled(_consumablesItemUI);
				}
				_scrollArea.ContentLength = (float)gameShopConsumables.Count * 2.86f;
			}
		}
	}
}

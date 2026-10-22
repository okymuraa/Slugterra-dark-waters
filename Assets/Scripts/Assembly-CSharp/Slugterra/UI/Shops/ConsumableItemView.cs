using System;
using System.Collections.Generic;
using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.Shops;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class ConsumableItemView : UIView
	{
		protected ConsumableType _itemValue;

		public Action OnBtnBuyItem;

		[SerializeField]
		private GameObject _information;

		[SerializeField]
		private GameObject _menu;

		private bool _curInfoState;

		[SerializeField]
		private TextMesh _consumableCountCurrent;

		[SerializeField]
		private TextMesh _consumableCountExtra;

		[SerializeField]
		private TextMesh _consumableCountTitle;

		[SerializeField]
		private TextMesh _title;

		[SerializeField]
		private TextMesh _price;

		[SerializeField]
		private TextMesh _description;

		[SerializeField]
		private tk2dSprite _itemConsumablePreview;

		[SerializeField]
		private tk2dButton _smallPackButtonBtnBuy;

		public tk2dButton SmallPackButtonBtnBuy
		{
			get
			{
				return _smallPackButtonBtnBuy;
			}
		}

		public object ItemValue
		{
			get
			{
				return _itemValue;
			}
		}

		public void ShowConsumable(ConsumableType type)
		{
			_itemValue = type;
			ConsumableProperties consumableProperties = MonoSingleton<GameConfiguration>.Instance.ConsumableConfigurationManager.GetConsumableProperties(type);
			ConsumableItem consumable = MonoSingleton<AppController>.Instance.Model.ShopModel.GetConsumable(type);
			_itemValue = type;
			string sprite = "item_consumable_health";
			switch (type)
			{
			case ConsumableType.First:
				sprite = "item_consumable_health";
				break;
			case ConsumableType.ArmorBoost:
				sprite = "item_consumable_armor";
				break;
			case ConsumableType.SpeedBoost:
				sprite = "item_consumable_speed";
				break;
			case ConsumableType.SlugFood:
				sprite = "item_consumable_slugfood";
				break;
			}
			_itemConsumablePreview.SetSprite(sprite);
			List<ConsumableItem> consumables = MonoSingleton<AppController>.Instance.Model.ShopModel.Consumables;
			foreach (ConsumableItem item in consumables)
			{
				if (item.Type == type)
				{
					_consumableCountCurrent.text = item.Count.ToString();
					break;
				}
			}
			_consumableCountTitle.text = MonoSingleton<Localization>.Instance.GetValueByKey("menu_title_your_balance");
			_consumableCountExtra.text = "+5";
			_title.text = MonoSingleton<Localization>.Instance.GetValueByKey(consumableProperties.baseData.title);
			_description.text = MonoSingleton<Localization>.Instance.GetValueByKey(consumableProperties.baseData.description);
			_price.text = consumableProperties.priceData.smallPackPrice.ToString();
		}

		public void UpdateConsumable()
		{
			List<ConsumableItem> consumables = MonoSingleton<AppController>.Instance.Model.ShopModel.Consumables;
			foreach (ConsumableItem item in consumables)
			{
				if (item.Type == _itemValue)
				{
					_consumableCountCurrent.text = item.Count.ToString();
					break;
				}
			}
		}

		public void OnBtnBuyClick()
		{
			if (OnBtnBuyItem != null)
			{
				OnBtnBuyItem();
			}
			UpdateConsumable();
		}

		public void OnInfoBtnClick()
		{
			_curInfoState = !_curInfoState;
			_information.SetActive(_curInfoState);
			_menu.SetActive(!_curInfoState);
		}
	}
}

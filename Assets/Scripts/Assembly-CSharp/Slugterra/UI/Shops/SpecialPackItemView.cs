using System;
using System.Collections.Generic;
using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.Configuration;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class SpecialPackItemView : UIView
	{
		[SerializeField]
		private TextMesh _title;

		[SerializeField]
		private TextMesh _titleSh;

		[SerializeField]
		private TextMesh _price;

		[SerializeField]
		private SpecialPackItemsList _ItemsList;

		[SerializeField]
		private GameObject _btnBuy;

		[SerializeField]
		private GameObject _btnAcquired;

		[SerializeField]
		private GameObject _newBadge;

		private bool _acquired;

		public Action OnBtnBuyItem;

		public string Title
		{
			set
			{
				_title.text = value;
				_titleSh.text = value;
			}
		}

		public string Price
		{
			set
			{
				_price.text = value;
			}
		}

		public bool Acquired
		{
			get
			{
				return _acquired;
			}
			set
			{
				_acquired = value;
			}
		}

		public void ShowItems(List<SpecialPacksItem> list)
		{
			if (list.Count > 10)
			{
				return;
			}
			for (int i = 0; i < _ItemsList.list.Count; i++)
			{
				_ItemsList.list[i].gameObject.SetActive(false);
			}
			for (int j = 0; j < list.Count; j++)
			{
				SpecialPackItemElementView specialPackItemElementView = _ItemsList.list[j];
				specialPackItemElementView.gameObject.SetActive(true);
				SpecialPacksItem specialPacksItem = list[j];
				if (specialPacksItem.Type == SpecialItemType.Slug)
				{
					SlugType slugTypeByName = MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugTypeByName(specialPacksItem.ItemValue);
					specialPackItemElementView.ShowSlug(slugTypeByName, specialPacksItem.ItemLevel);
				}
				else if (specialPacksItem.Type == SpecialItemType.Blaster)
				{
					BlasterType blasterTypeByName = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterTypeByName(specialPacksItem.ItemValue);
					specialPackItemElementView.ShowBlaster(blasterTypeByName);
				}
				else if (specialPacksItem.Type == SpecialItemType.Armor)
				{
					ArmorType armorTypeByName = MonoSingleton<GameConfiguration>.Instance.ArmorConfigManager.GetArmorTypeByName(specialPacksItem.ItemValue);
					specialPackItemElementView.ShowArmor(armorTypeByName);
				}
				else if (specialPacksItem.Type == SpecialItemType.Ally)
				{
					AllyType assistantTypeByName = MonoSingleton<GameConfiguration>.Instance.UnitConfigurationManager.GetAssistantTypeByName(specialPacksItem.ItemValue);
					specialPackItemElementView.ShowAlly(assistantTypeByName, specialPacksItem.ItemLevel);
				}
				else if (specialPacksItem.Type == SpecialItemType.Consumable)
				{
					ConsumableType consumableTypeByName = MonoSingleton<GameConfiguration>.Instance.ConsumableConfigurationManager.GetConsumableTypeByName(specialPacksItem.ItemValue);
					specialPackItemElementView.ShowConsumable(consumableTypeByName, specialPacksItem.Count);
				}
				else if (specialPacksItem.Type == SpecialItemType.Gold)
				{
					specialPackItemElementView.ShowGold(specialPacksItem.Count);
				}
			}
			_btnBuy.gameObject.SetActive(!_acquired);
			_btnAcquired.gameObject.SetActive(_acquired);
		}

		public void ShowBadge(bool state)
		{
			_newBadge.SetActive(state);
		}

		public void OnBtnBuyClick()
		{
			if (OnBtnBuyItem != null)
			{
				OnBtnBuyItem();
			}
		}
	}
}

using System;
using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.Shops;
using Slugterra.UI.Utils;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class SpecialPackItemElementView : UIView
	{
		[SerializeField]
		private GameObject _consumable;

		[SerializeField]
		private GameObject _force;

		[SerializeField]
		private GameObject _currency;

		[SerializeField]
		private TextMesh _consumableCount;

		[SerializeField]
		private TextMesh _forceCount;

		[SerializeField]
		private TextMesh _currencyCount;

		[SerializeField]
		private tk2dSprite _itemBlasterPreview;

		[SerializeField]
		private tk2dSprite _itemOtherPreview;

		[SerializeField]
		private tk2dSprite _itemConsumablePreview;

		[SerializeField]
		private tk2dSprite _itemCurrencyPreview;

		public Action OnBtnBuyItem;

		protected object _itemValue;

		private SlugType _slugType;

		private BlasterType _blasterType;

		private ArmorType _armorType;

		private AllyType _allyType;

		public object ItemValue
		{
			get
			{
				return _itemValue;
			}
		}

		public void ShowArmor(ArmorType type)
		{
			_armorType = type;
			_itemValue = type;
			ArmorConfig armorConfiguration = MonoSingleton<GameConfiguration>.Instance.ArmorConfigManager.GetArmorConfiguration(type);
			ArmorItem armor = MonoSingleton<AppController>.Instance.Model.ShopModel.GetArmor(type);
			_itemOtherPreview.SetSprite(armorConfiguration.uiIcon);
			_forceCount.text = MonoSingleton<GameConfiguration>.Instance.ArmorConfigManager.GetArmorParams(type, armor.Level).forceRate.ToString();
			_consumable.SetActive(false);
			_force.SetActive(true);
			_currency.SetActive(false);
			_itemOtherPreview.gameObject.SetActive(true);
			_itemBlasterPreview.gameObject.SetActive(false);
		}

		public void ShowBlaster(BlasterType type)
		{
			_blasterType = type;
			_itemValue = type;
			BlasterConfig blusterConfiguration = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlusterConfiguration(type);
			BlasterItem blaster = MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(type);
			_itemBlasterPreview.SetSprite(blusterConfiguration.uiIcon);
			_forceCount.text = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterParams(type, blaster.Level).forceRate.ToString();
			_consumable.SetActive(false);
			_force.SetActive(true);
			_currency.SetActive(false);
			_itemOtherPreview.gameObject.SetActive(false);
			_itemBlasterPreview.gameObject.SetActive(true);
		}

		public void ShowSlug(SlugType type, int level)
		{
			_slugType = type;
			_itemValue = type;
			SlugConfig slugConfiguration = MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugConfiguration(type);
			if (slugConfiguration == null)
			{
				Debug.LogWarning(type);
			}
			SlugItem slug = MonoSingleton<AppController>.Instance.Model.ShopModel.GetSlug(type);
			string sprite = SpriteNamesHelper.GetSlugSpriteName(type).Replace("icon", "item");
			_forceCount.text = MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugParams(type, level).forceRate.ToString();
			_itemOtherPreview.SetSprite(sprite);
			_consumable.SetActive(false);
			_force.SetActive(true);
			_currency.SetActive(false);
			_itemOtherPreview.gameObject.SetActive(true);
			_itemBlasterPreview.gameObject.SetActive(false);
		}

		public void ShowAlly(AllyType type, int level)
		{
			_allyType = type;
			_itemValue = type;
			HumanoidConfig alliesConfig = MonoSingleton<GameConfiguration>.Instance.UnitConfigurationManager.GetAlliesConfig(type);
			AssistantItem assistantItem = MonoSingleton<AppController>.Instance.Model.ShopModel.Assistants.Find((AssistantItem assistant) => assistant.Type == type);
			string sprite = SpriteNamesHelper.GetAssistantSpriteName(type).Replace("icon", "item");
			_itemOtherPreview.SetSprite(sprite);
			_forceCount.text = MonoSingleton<GameConfiguration>.Instance.UnitConfigurationManager.GetAlliesConfigurationLevel(type, level).forceRate.ToString();
			_consumable.SetActive(false);
			_force.SetActive(true);
			_currency.SetActive(false);
			_itemOtherPreview.gameObject.SetActive(true);
			_itemBlasterPreview.gameObject.SetActive(false);
		}

		public void ShowConsumable(ConsumableType type, int count)
		{
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
			_consumableCount.text = "+" + count;
			_consumable.SetActive(true);
			_force.SetActive(false);
			_currency.SetActive(false);
		}

		public void ShowGold(int count)
		{
			_itemValue = null;
			string sprite = "item_coins_01";
			if (count > 0)
			{
				sprite = "item_coins_02";
			}
			else if (count >= 2500)
			{
				sprite = "item_coins_03";
			}
			else if (count >= 5000)
			{
				sprite = "item_coins_04";
			}
			else if (count >= 10000)
			{
				sprite = "item_coins_08";
			}
			_itemCurrencyPreview.SetSprite(sprite);
			_currencyCount.text = "+" + count;
			_consumable.SetActive(false);
			_force.SetActive(false);
			_currency.SetActive(true);
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

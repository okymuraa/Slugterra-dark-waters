using System;
using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Shops;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class SpecialPackItemController : UIController<SpecialPackItemView>
	{
		private SpecialPacksConfig _config;

		private void OnEnable()
		{
			SpecialPackItemView view = View;
			view.OnBtnBuyItem = (Action)Delegate.Combine(view.OnBtnBuyItem, new Action(OnBtnBuy));
		}

		private void OnDisable()
		{
			SpecialPackItemView view = View;
			view.OnBtnBuyItem = (Action)Delegate.Remove(view.OnBtnBuyItem, new Action(OnBtnBuy));
		}

		public void ShowItems(SpecialPacksConfig config)
		{
			_config = config;
			View.Title = Localization.GetValue(config.Name, string.Empty);
			View.Price = config.Price.ToString();
			View.Acquired = config.IsBought;
			View.ShowItems(config.Items);
		}

		private void Update()
		{
		}

		private void OnBtnBuy()
		{
			ShopModel shopModel = MonoSingleton<AppController>.Instance.Model.ShopModel;
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			if (shopModel.OnBuySpecialPack(shopModel.GetSpecialPack(_config.Id)))
			{
				foreach (SpecialPacksItem item in _config.Items)
				{
					SpecialItemType type = item.Type;
					switch (type)
					{
					case SpecialItemType.Armor:
					{
						ArmorType armorTypeByName = MonoSingleton<GameConfiguration>.Instance.ArmorConfigManager.GetArmorTypeByName(item.ItemValue);
						ArmorItem armor = MonoSingleton<AppController>.Instance.Model.ShopModel.GetArmor(armorTypeByName);
						shopModel.OnTakeArmorFree(shopModel.GetArmor(armorTypeByName));
						shopModel.OnUpgradeArmorFree(shopModel.GetArmor(armorTypeByName), item.ItemLevel);
						missionSetting.Armor = armor;
						break;
					}
					case SpecialItemType.Ally:
					{
						AllyType allyType = MonoSingleton<GameConfiguration>.Instance.UnitConfigurationManager.GetAssistantTypeByName(item.ItemValue);
						AssistantItem assistant = MonoSingleton<AppController>.Instance.Model.ShopModel.Assistants.Find((AssistantItem assistantItem) => assistantItem.Type == allyType);
						shopModel.OnTakeAssistantFree(shopModel.GetAssistant(allyType));
						shopModel.OnUpgradeAssistantFree(shopModel.GetAssistant(allyType), item.ItemLevel);
						missionSetting.Assistant = assistant;
						break;
					}
					case SpecialItemType.Blaster:
					{
						BlasterType blasterTypeByName = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterTypeByName(item.ItemValue);
						BlasterItem blaster = MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(blasterTypeByName);
						shopModel.OnTakeBlasterFree(shopModel.GetBlaster(blasterTypeByName));
						missionSetting.Blaster = blaster;
						break;
					}
					case SpecialItemType.Slug:
					{
						SlugType slugTypeByName = MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugTypeByName(item.ItemValue);
						SlugItem slug = MonoSingleton<AppController>.Instance.Model.ShopModel.GetSlug(slugTypeByName);
						shopModel.OnTakeSlugFree(shopModel.GetSlug(slugTypeByName));
						shopModel.OnUpgradeSlugFree(shopModel.GetSlug(slugTypeByName), item.ItemLevel);
						missionSetting.SetSlug(slug);
						break;
					}
					case SpecialItemType.Gold:
						MonoSingleton<AppController>.Instance.Model.Wallet.Gold += item.Count;
						break;
					case SpecialItemType.Consumable:
					{
						ConsumableType consumableTypeByName = MonoSingleton<GameConfiguration>.Instance.ConsumableConfigurationManager.GetConsumableTypeByName(item.ItemValue);
						ConsumableItem consumable = shopModel.GetConsumable(consumableTypeByName);
						Debug.Log("SpecialPackItemController::OnBtnBuy consumableType = " + consumableTypeByName);
						Debug.Log("SpecialPackItemController::OnBtnBuy consumableItem != null" + consumable != null);
						if (consumable != null)
						{
							shopModel.OnTakeConsumableFree(consumable, item.Count);
						}
						break;
					}
					}
				}
				_config.IsBought = true;
			}
			else
			{
				_config.IsBought = shopModel.GetSpecialPack(_config.Id).IsBought;
			}
			ShowItems(_config);
		}
	}
}

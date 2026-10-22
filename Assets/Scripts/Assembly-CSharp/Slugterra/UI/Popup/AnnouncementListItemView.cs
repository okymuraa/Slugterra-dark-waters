using System;
using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.Shops;
using Slugterra.InAppPurchases;
using Slugterra.UI.Shops;
using Slugterra.UI.Shops.PaymentStates;
using Slugterra.UI.Utils;
using UnityEngine;

namespace Slugterra.UI.Popup
{
	public class AnnouncementListItemView : UIView
	{
		[SerializeField]
		public TextMesh _title;

		[SerializeField]
		public TextMesh _price;

		[SerializeField]
		public tk2dSprite _itemPreview;

		[SerializeField]
		public GameObject _priceView;

		[SerializeField]
		public GameObject _detailsView;

		[SerializeField]
		public GameObject _buyView;

		[SerializeField]
		public GameObject _acquiredView;

		[SerializeField]
		public GameObject _acquiredTestView;

		[SerializeField]
		public AbstractPaymentState _priceState;

		[SerializeField]
		private AnnounceItemViewType _viewType = AnnounceItemViewType.Buy;

		protected object _itemValue;

		public Action OnBtnBuyItem;

		public Action OnBtnDetails;

		public AnnounceItemViewType CurrentViewType
		{
			get
			{
				return _viewType;
			}
			set
			{
				_viewType = value;
			}
		}

		public virtual void ShowArmor(ArmorType type, AnnounceItemViewType viewType)
		{
			_itemValue = type;
			ArmorConfig armorConfiguration = MonoSingleton<GameConfiguration>.Instance.ArmorConfigManager.GetArmorConfiguration(type);
			ArmorItem armor = MonoSingleton<AppController>.Instance.Model.ShopModel.GetArmor(type);
			if (viewType != AnnounceItemViewType.Price && viewType != AnnounceItemViewType.AcquiredNeedEquip && viewType != AnnounceItemViewType.AcquiredNeedUpgrade && viewType != AnnounceItemViewType.AcquiredNeedRecover && armor.IsBought)
			{
				viewType = AnnounceItemViewType.Acquired;
			}
			_title.text = Localization.GetValue(armorConfiguration.title, string.Empty);
			_itemPreview.SetSprite(armorConfiguration.uiIcon);
			float armorPriceGold = MonoSingleton<GameConfiguration>.Instance.ArmorConfigManager.GetArmorPriceGold(armor.Type, 0);
			_priceState.SetPrice(PaymentButtonCurrency.Coins, armorPriceGold.ToString());
			UpdateViewType(viewType);
		}

		public virtual void ShowBlaster(BlasterType type, AnnounceItemViewType viewType)
		{
			_itemValue = type;
			BlasterConfig blusterConfiguration = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlusterConfiguration(type);
			BlasterItem blaster = MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(type);
			if (viewType != AnnounceItemViewType.Price && viewType != AnnounceItemViewType.AcquiredNeedEquip && viewType != AnnounceItemViewType.AcquiredNeedUpgrade && viewType != AnnounceItemViewType.AcquiredNeedRecover && blaster.IsBought)
			{
				viewType = AnnounceItemViewType.Acquired;
			}
			_title.text = Localization.GetValue(blusterConfiguration.title, string.Empty);
			_itemPreview.SetSprite(blusterConfiguration.uiIcon);
			string blasterInApp = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterInApp(blaster.Type, 0);
			if (blasterInApp != string.Empty)
			{
				string formattedPrice = DonDestroyMonoSingleton<SlugterraPurchaseContext>.Instance.GetFormattedPrice(blasterInApp);
				_priceState.SetPrice(PaymentButtonCurrency.Money, formattedPrice);
			}
			else
			{
				float blasterPriceGold = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterPriceGold(blaster.Type, 0);
				float blasterPriceGems = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterPriceGems(blaster.Type, 0);
				if (blasterPriceGems > 0f)
				{
					_priceState.SetPrice(PaymentButtonCurrency.Gems, blasterPriceGems.ToString());
				}
				else
				{
					_priceState.SetPrice(PaymentButtonCurrency.Coins, blasterPriceGold.ToString());
				}
			}
			UpdateViewType(viewType);
		}

		public virtual void ShowSlug(SlugType type, AnnounceItemViewType viewType)
		{
			_itemValue = type;
			SlugConfig slugConfiguration = MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugConfiguration(type);
			if (slugConfiguration == null)
			{
				Debug.LogWarning(type);
			}
			SlugItem slug = MonoSingleton<AppController>.Instance.Model.ShopModel.GetSlug(type);
			if (viewType != AnnounceItemViewType.Price && viewType != AnnounceItemViewType.AcquiredNeedEquip && viewType != AnnounceItemViewType.AcquiredNeedUpgrade && viewType != AnnounceItemViewType.AcquiredNeedRecover && viewType != AnnounceItemViewType.AcquiredForTest && slug.IsBought)
			{
				viewType = AnnounceItemViewType.Acquired;
			}
			_title.text = Localization.GetValue(slugConfiguration.title, string.Empty);
			string sprite = SpriteNamesHelper.GetSlugSpriteName(type).Replace("icon", "item");
			_itemPreview.SetSprite(sprite);
			float slugPriceGold = MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugPriceGold(slug.Type, 0);
			_priceState.SetPrice(PaymentButtonCurrency.Coins, slugPriceGold.ToString());
			UpdateViewType(viewType);
		}

		public virtual void ShowAlly(AllyType type, AnnounceItemViewType viewType)
		{
			_itemValue = type;
			HumanoidConfig alliesConfig = MonoSingleton<GameConfiguration>.Instance.UnitConfigurationManager.GetAlliesConfig(type);
			AssistantItem assistantItem = MonoSingleton<AppController>.Instance.Model.ShopModel.Assistants.Find((AssistantItem assistant) => assistant.Type == type);
			if (viewType != AnnounceItemViewType.Price && viewType != AnnounceItemViewType.AcquiredNeedEquip && viewType != AnnounceItemViewType.AcquiredNeedUpgrade && viewType != AnnounceItemViewType.AcquiredNeedRecover && assistantItem.IsBought)
			{
				viewType = AnnounceItemViewType.Acquired;
			}
			_title.text = Localization.GetValue(alliesConfig.title, string.Empty);
			string sprite = SpriteNamesHelper.GetAssistantSpriteName(type).Replace("icon", "item");
			_itemPreview.SetSprite(sprite);
			ShopItemConfiguration assistantConfig = MonoSingleton<AppConfiguration>.Instance.ShopConfig.GetAssistantConfig(assistantItem.Type, 0);
			_priceState.SetPrice(PaymentButtonCurrency.Coins, assistantConfig.GamePrice.Value.ToString());
			UpdateViewType(viewType);
		}

		public virtual void ShowConsumable(ConsumableType type, AnnounceItemViewType viewType, bool islarge = false)
		{
			_itemValue = type;
			if (viewType != AnnounceItemViewType.Price && viewType != AnnounceItemViewType.AcquiredNeedEquip && viewType != AnnounceItemViewType.AcquiredNeedUpgrade && viewType != AnnounceItemViewType.AcquiredNeedRecover)
			{
				ConsumableItem consumable = MonoSingleton<AppController>.Instance.Model.ShopModel.GetConsumable(type);
			}
			ConsumableProperties consumableProperties = MonoSingleton<GameConfiguration>.Instance.ConsumableConfigurationManager.GetConsumableProperties(type);
			if (consumableProperties != null)
			{
				_title.text = Localization.GetValue(consumableProperties.baseData.title, string.Empty);
				string consumableSpriteName = SpriteNamesHelper.GetConsumableSpriteName(type);
				_itemPreview.SetSprite(consumableSpriteName);
				float num = ((!islarge) ? consumableProperties.priceData.smallPackPrice : consumableProperties.priceData.largePackPrice);
				_priceState.SetPrice(PaymentButtonCurrency.Coins, num.ToString());
			}
			UpdateViewType(viewType);
		}

		protected override void OnApplyModel(object model)
		{
			base.OnApplyModel(model);
		}

		public void UpdateViewType(AnnounceItemViewType viewType)
		{
			_priceView.SetActive(false);
			if (_detailsView != null)
			{
				_detailsView.SetActive(false);
			}
			_acquiredView.SetActive(false);
			if (_acquiredTestView != null)
			{
				_acquiredTestView.SetActive(false);
			}
			_buyView.SetActive(false);
			_priceState.Hide();
			switch (viewType)
			{
			case AnnounceItemViewType.Price:
				_priceView.SetActive(true);
				_priceState.transform.parent = _priceView.transform;
				_priceState.Show();
				break;
			case AnnounceItemViewType.Details:
				if (_detailsView != null)
				{
					_detailsView.SetActive(true);
				}
				break;
			case AnnounceItemViewType.Buy:
				_buyView.SetActive(true);
				_priceState.transform.parent = _buyView.transform;
				_priceState.Show();
				break;
			case AnnounceItemViewType.Acquired:
				_acquiredView.SetActive(true);
				break;
			case AnnounceItemViewType.AcquiredForTest:
				if (_acquiredTestView != null)
				{
					_acquiredTestView.SetActive(true);
				}
				break;
			case AnnounceItemViewType.AcquiredNeedUpgrade:
				_acquiredView.SetActive(false);
				_priceState.Show();
				break;
			case AnnounceItemViewType.AcquiredNeedRecover:
				_acquiredView.SetActive(false);
				break;
			case AnnounceItemViewType.AcquiredNeedEquip:
				_acquiredView.SetActive(false);
				break;
			}
			CurrentViewType = viewType;
		}

		private void OnBtnDetailsClick()
		{
			if (OnBtnDetails != null)
			{
				OnBtnDetails();
			}
		}

		public virtual void OnBtnBuyClick()
		{
			if (OnBtnBuyItem != null)
			{
				OnBtnBuyItem();
			}
		}
	}
}

using System;
using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.Game.HealthRestoration;
using Slugterra.Domain.Shops;
using Slugterra.UI.Popup;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class RecommendedItemController : UIController<RecommendedItemView>
	{
		private ItemType _modelType;

		private PopupGuideAnnouncementView _popupContainer;

		private object _modelValue;

		private RestoreHealthConfiguration healthRestoreManager;

		private ShopModel _shopModel;

		private int _сonsumableCount;

		private int _upgradeLevel;

		private bool _isRequired;

		private AnnounceItemViewType _viewType;

		protected override void OnCreate()
		{
			base.OnCreate();
			healthRestoreManager = MonoSingleton<GameConfiguration>.Instance.RestoreHealthConfigurationManager;
			_shopModel = MonoSingleton<AppController>.Instance.Model.ShopModel;
		}

		private void OnEnable()
		{
			RecommendedItemView view = View;
			view.OnBtnBuyItem = (Action)Delegate.Combine(view.OnBtnBuyItem, new Action(OnBtnBuy));
			RecommendedItemView view2 = View;
			view2.OnBtnDetails = (Action)Delegate.Combine(view2.OnBtnDetails, new Action(OnBtnDetails));
			View.PaymentButton.OnRecoverEvent += OnRecoverHandler;
			healthRestoreManager.OnUpdateValues += OnRecoverValuesUpdate;
		}

		private void OnDisable()
		{
			RecommendedItemView view = View;
			view.OnBtnBuyItem = (Action)Delegate.Remove(view.OnBtnBuyItem, new Action(OnBtnBuy));
			RecommendedItemView view2 = View;
			view2.OnBtnDetails = (Action)Delegate.Remove(view2.OnBtnDetails, new Action(OnBtnDetails));
			View.PaymentButton.OnRecoverEvent -= OnRecoverHandler;
			healthRestoreManager.OnUpdateValues -= OnRecoverValuesUpdate;
		}

		public void ShowArmor(ArmorType type, AnnounceItemViewType viewType, PopupGuideAnnouncementView popup, bool isRequired)
		{
			_modelType = ItemType.Armor;
			_modelValue = type;
			_popupContainer = popup;
			View.IsRequired = isRequired;
			View.ShowArmor(type, viewType);
		}

		public void ShowBlaster(BlasterType type, AnnounceItemViewType viewType, PopupGuideAnnouncementView popup, bool isRequired)
		{
			_modelType = ItemType.Blaster;
			_modelValue = type;
			_popupContainer = popup;
			View.IsRequired = isRequired;
			View.ShowBlaster(type, viewType);
		}

		public void ShowSlug(SlugType type, AnnounceItemViewType viewType, PopupGuideAnnouncementView popup, int upgradeLevel, bool isRequired)
		{
			_modelType = ItemType.Slug;
			_modelValue = type;
			_popupContainer = popup;
			View.IsRequired = isRequired;
			View.UpgradeLevel = upgradeLevel;
			View.ShowSlug(type, viewType);
		}

		public void ShowAlly(AllyType type, AnnounceItemViewType viewType, PopupGuideAnnouncementView popup, int upgradeLevel, bool isRequired)
		{
			_modelType = ItemType.Ally;
			_modelValue = type;
			_popupContainer = popup;
			View.IsRequired = isRequired;
			View.UpgradeLevel = upgradeLevel;
			View.ShowAlly(type, viewType);
		}

		public void ShowConsumable(ConsumableType type, AnnounceItemViewType viewType, PopupGuideAnnouncementView popup, int upgradeLevel, bool isRequired)
		{
			_modelType = ItemType.Consumable;
			_modelValue = type;
			_popupContainer = popup;
			_upgradeLevel = upgradeLevel;
			_isRequired = isRequired;
			_viewType = viewType;
			ConsumableItem consumable = MonoSingleton<AppController>.Instance.Model.ShopModel.GetConsumable(type);
			_сonsumableCount = upgradeLevel - consumable.Count;
			_сonsumableCount = ((_сonsumableCount > 0) ? ((_сonsumableCount <= 5) ? 5 : 10) : 0);
			View.IsRequired = isRequired;
			View.UpgradeLevel = upgradeLevel;
			View.ShowConsumable(type, viewType, _сonsumableCount > 5);
		}

		public void UpdateConsumable()
		{
			ConsumableItem consumable = MonoSingleton<AppController>.Instance.Model.ShopModel.GetConsumable((ConsumableType)_modelValue);
			_сonsumableCount = _upgradeLevel - consumable.Count;
			_сonsumableCount = ((_сonsumableCount > 0) ? ((_сonsumableCount <= 5) ? 5 : 10) : 0);
			View.IsRequired = _isRequired;
			View.UpgradeLevel = _upgradeLevel;
			View.ShowConsumable((ConsumableType)_modelValue, _viewType, _сonsumableCount > 5);
		}

		private void Update()
		{
			if (View.CurrentViewType != AnnounceItemViewType.Buy)
			{
				return;
			}
			switch (_modelType)
			{
			case ItemType.Armor:
				if (_shopModel.GetArmor(View.ArmorType).IsBought)
				{
					View.UpdateViewType(AnnounceItemViewType.Acquired);
				}
				break;
			case ItemType.Ally:
				if (_shopModel.GetAssistant(View.AllyType).IsBought)
				{
					View.UpdateViewType(AnnounceItemViewType.Acquired);
				}
				break;
			case ItemType.Blaster:
				if (_shopModel.GetBlaster(View.BlasterType).IsBought)
				{
					View.UpdateViewType(AnnounceItemViewType.Acquired);
				}
				break;
			case ItemType.Slug:
				if (_shopModel.GetSlug(View.SlugType).IsBought)
				{
					View.UpdateViewType(AnnounceItemViewType.Acquired);
				}
				break;
			case ItemType.Consumable:
			{
				ConsumableItem consumable = MonoSingleton<AppController>.Instance.Model.ShopModel.GetConsumable((ConsumableType)_modelValue);
				if (consumable != null && consumable.Count > View.UpgradeLevel)
				{
					View.UpdateViewType(AnnounceItemViewType.Acquired);
				}
				break;
			}
			}
		}

		private void OnBtnBuy()
		{
			switch (_modelType)
			{
			case ItemType.Armor:
				_shopModel.OnBuyArmor(_shopModel.GetArmor(View.ArmorType));
				break;
			case ItemType.Ally:
				_shopModel.OnBuyAssistant(_shopModel.GetAssistant(View.AllyType));
				break;
			case ItemType.Blaster:
				_shopModel.OnBuyBlaster(_shopModel.GetBlaster(View.BlasterType));
				break;
			case ItemType.Slug:
				_shopModel.OnBuySlug(_shopModel.GetSlug(View.SlugType));
				break;
			case ItemType.Consumable:
				UpdateConsumable();
				_shopModel.OnBuyConsumable(_shopModel.GetConsumable((ConsumableType)_modelValue), _сonsumableCount);
				break;
			}
		}

		private void OnRecoverHandler()
		{
			switch (_modelType)
			{
			case ItemType.Ally:
				RecoverAssistant();
				break;
			case ItemType.Slug:
				RecoverSlug();
				break;
			}
			View.ShowInfo();
			View.ReDrawWindow();
		}

		private void OnRecoverValuesUpdate()
		{
			View.ShowInfo();
			View.ReDrawWindow();
		}

		private void OnBtnDetails()
		{
			_popupContainer.ItemDetailsClick();
			ShopTask.ShowItem(_modelValue);
		}

		private void RecoverAssistant()
		{
			AllyType allyType = (AllyType)_modelValue;
			AssistantItem assistant = _shopModel.GetAssistant(allyType);
			if (assistant == null)
			{
				Debug.LogError("Can't find assistant with type:" + allyType);
				return;
			}
			switch (View.PaymentButton.State)
			{
			case PaymentButtonState.Recover:
				_shopModel.OnRecoverAssistant(assistant);
				break;
			case PaymentButtonState.RecoverByAd:
				RecoverItemByAd(allyType);
				break;
			}
		}

		private void RecoverSlug()
		{
			SlugType slugType = (SlugType)_modelValue;
			SlugItem slug = _shopModel.GetSlug(slugType);
			if (slug == null)
			{
				Debug.LogError("Can't find slug with type:" + slugType);
				return;
			}
			switch (View.PaymentButton.State)
			{
			case PaymentButtonState.Recover:
				_shopModel.OnRecoverSlug(slug);
				break;
			case PaymentButtonState.RecoverByAd:
				RecoverItemByAd(slugType);
				break;
			}
		}

		private void RecoverItemByAd(Enum itemType)
		{
			if (healthRestoreManager != null)
			{
				healthRestoreManager.GetHealthConfiguration(itemType).Heal();
				healthRestoreManager.Save();
			}
			else
			{
				Debug.LogError("Missing health restoration manager!");
			}
		}
	}
}

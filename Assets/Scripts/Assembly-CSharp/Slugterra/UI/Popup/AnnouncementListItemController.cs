using System;
using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.Shops;
using Slugterra.UI.Shops;

namespace Slugterra.UI.Popup
{
	public class AnnouncementListItemController : UIController<AnnouncementListItemView>
	{
		private ItemType _modelType;

		private PopupGuideAnnouncementView _popupContainer;

		private object _modelValue;

		private void OnEnable()
		{
			AnnouncementListItemView view = View;
			view.OnBtnBuyItem = (Action)Delegate.Combine(view.OnBtnBuyItem, new Action(OnBtnBuy));
			AnnouncementListItemView view2 = View;
			view2.OnBtnDetails = (Action)Delegate.Combine(view2.OnBtnDetails, new Action(OnBtnDetails));
		}

		private void OnDisable()
		{
			AnnouncementListItemView view = View;
			view.OnBtnBuyItem = (Action)Delegate.Remove(view.OnBtnBuyItem, new Action(OnBtnBuy));
			AnnouncementListItemView view2 = View;
			view2.OnBtnDetails = (Action)Delegate.Remove(view2.OnBtnDetails, new Action(OnBtnDetails));
		}

		public void ShowArmor(ArmorType type, AnnounceItemViewType viewType, PopupGuideAnnouncementView popup)
		{
			_modelType = ItemType.Armor;
			_modelValue = type;
			_popupContainer = popup;
			View.ShowArmor(type, viewType);
		}

		public void ShowBlaster(BlasterType type, AnnounceItemViewType viewType, PopupGuideAnnouncementView popup)
		{
			_modelType = ItemType.Blaster;
			_modelValue = type;
			_popupContainer = popup;
			View.ShowBlaster(type, viewType);
		}

		public void ShowSlug(SlugType type, AnnounceItemViewType viewType, PopupGuideAnnouncementView popup)
		{
			_modelType = ItemType.Slug;
			_modelValue = type;
			_popupContainer = popup;
			View.ShowSlug(type, viewType);
		}

		public void ShowAlly(AllyType type, AnnounceItemViewType viewType, PopupGuideAnnouncementView popup)
		{
			_modelType = ItemType.Ally;
			_modelValue = type;
			_popupContainer = popup;
			View.ShowAlly(type, viewType);
		}

		private void Update()
		{
			if (View.CurrentViewType != AnnounceItemViewType.Buy)
			{
				return;
			}
			ShopModel shopModel = MonoSingleton<AppController>.Instance.Model.ShopModel;
			switch (_modelType)
			{
			case ItemType.Armor:
				if (shopModel.GetArmor((ArmorType)_modelValue).IsBought)
				{
					View.UpdateViewType(AnnounceItemViewType.Acquired);
				}
				break;
			case ItemType.Ally:
				if (shopModel.Assistants.Find((AssistantItem assistant) => assistant.Type == (AllyType)_modelValue).IsBought)
				{
					View.UpdateViewType(AnnounceItemViewType.Acquired);
				}
				break;
			case ItemType.Blaster:
				if (shopModel.GetBlaster((BlasterType)_modelValue).IsBought)
				{
					View.UpdateViewType(AnnounceItemViewType.Acquired);
				}
				break;
			case ItemType.Slug:
				if (shopModel.GetSlug((SlugType)_modelValue).IsBought)
				{
					View.UpdateViewType(AnnounceItemViewType.Acquired);
				}
				break;
			}
		}

		private void OnBtnBuy()
		{
			ShopModel shopModel = MonoSingleton<AppController>.Instance.Model.ShopModel;
			switch (_modelType)
			{
			case ItemType.Armor:
				shopModel.OnBuyArmor(shopModel.GetArmor((ArmorType)_modelValue));
				break;
			case ItemType.Ally:
				shopModel.OnBuyAssistant(shopModel.Assistants.Find((AssistantItem assistant) => assistant.Type == (AllyType)_modelValue));
				break;
			case ItemType.Blaster:
				shopModel.OnBuyBlaster(shopModel.GetBlaster((BlasterType)_modelValue));
				break;
			case ItemType.Slug:
				shopModel.OnBuySlug(shopModel.GetSlug((SlugType)_modelValue));
				break;
			}
		}

		private void OnBtnDetails()
		{
			_popupContainer.ItemDetailsClick();
			ShopTask.ShowItem(_modelValue);
		}
	}
}

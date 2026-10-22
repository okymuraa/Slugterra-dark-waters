using System;
using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.Game.HealthRestoration;
using Slugterra.Domain.Shops;
using Slugterra.UI.Popup;

namespace Slugterra.UI.Shops
{
	public class ConsumableItemController : UIController<ConsumableItemView>
	{
		private PopupGuideAnnouncementView _popupContainer;

		private ConsumableType _modelValue = ConsumableType.None;

		private RestoreHealthConfiguration healthRestoreManager;

		private ShopModel _shopModel;

		protected override void OnCreate()
		{
			base.OnCreate();
			healthRestoreManager = MonoSingleton<GameConfiguration>.Instance.RestoreHealthConfigurationManager;
			_shopModel = MonoSingleton<AppController>.Instance.Model.ShopModel;
		}

		private void OnEnable()
		{
			ConsumableItemView view = View;
			view.OnBtnBuyItem = (Action)Delegate.Combine(view.OnBtnBuyItem, new Action(OnBtnBuy));
			if (_modelValue != ConsumableType.None)
			{
				View.UpdateConsumable();
			}
		}

		private void OnDisable()
		{
			ConsumableItemView view = View;
			view.OnBtnBuyItem = (Action)Delegate.Remove(view.OnBtnBuyItem, new Action(OnBtnBuy));
		}

		public void ShowConsumable(ConsumableType type)
		{
			_modelValue = type;
			View.ShowConsumable(type);
		}

		private void Update()
		{
		}

		private void OnBtnBuy()
		{
			ShopModel shopModel = MonoSingleton<AppController>.Instance.Model.ShopModel;
			ConsumableItem consumable = shopModel.GetConsumable(_modelValue);
			shopModel.OnBuyConsumable(consumable, 5);
		}
	}
}

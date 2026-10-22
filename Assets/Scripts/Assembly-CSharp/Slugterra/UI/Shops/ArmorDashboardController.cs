using AppsMinistry.Core;
using AppsMinistry.Domain.Observer;
using Slugterra.App;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Shops;

namespace Slugterra.UI.Shops
{
	public class ArmorDashboardController : ItemDashboardController, IObserver
	{
		private ArmorItem _model;

		protected new ArmorDashboardView View
		{
			get
			{
				return base.View as ArmorDashboardView;
			}
		}

		public void OnObjectChanged(object observable)
		{
			if (observable is ArmorItem)
			{
				View.ApplyModel(observable as ArmorItem);
			}
		}

		private void OnEnable()
		{
			View.PaymentButton.OnBuyEvent += OnBuy;
			View.PaymentButton.OnUpgradeEvent += OnUpgrade;
		}

		private void OnDisable()
		{
			View.PaymentButton.OnBuyEvent -= OnBuy;
			View.PaymentButton.OnUpgradeEvent -= OnUpgrade;
		}

		private void OnBuy()
		{
			ShopModel shopModel = MonoSingleton<AppController>.Instance.Model.ShopModel;
			if (shopModel.OnBuyArmor(_model))
			{
				OnEquip();
			}
		}

		private void OnEquip()
		{
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			missionSetting.Armor = _model;
		}

		private void OnUnequip()
		{
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			missionSetting.SetDefaultArmor();
		}

		private void OnUpgrade()
		{
			ShopModel shopModel = MonoSingleton<AppController>.Instance.Model.ShopModel;
			shopModel.OnUpgradeArmor(_model);
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			if (missionSetting.Armor == _model)
			{
				missionSetting.CommitAndNotify();
			}
		}

		public override void ApplyModel(object model)
		{
			if (model is ArmorItem)
			{
				if (_model != null)
				{
					_model.RemoveObserver(this);
				}
				_model = model as ArmorItem;
				_model.AddObserver(this);
				_model.IsBackpack = false;
				View.ApplyModel(_model);
			}
		}

		protected override void OnReleaseResource()
		{
			base.OnReleaseResource();
			if (_model != null)
			{
				_model.RemoveObserver(this);
			}
		}
	}
}

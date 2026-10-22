using AppsMinistry.Core;
using AppsMinistry.Domain.Observer;
using Slugterra.App;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Shops;

namespace Slugterra.UI.Shops
{
	public class BlasterDashboardController : ItemDashboardController, IObserver
	{
		private BlasterItem _model;

		protected new BlasterDashboardView View
		{
			get
			{
				return base.View as BlasterDashboardView;
			}
		}

		public void OnObjectChanged(object observable)
		{
			if (observable is BlasterItem)
			{
				View.ApplyModel(observable as BlasterItem);
			}
		}

		private void OnEnable()
		{
			View.PaymentButton.OnBuyEvent += OnBuy;
			View.PaymentButton.OnUpgradeEvent += OnUpgrade;
			View.PaymentButton.OnRecoverEvent += OnRecover;
		}

		private void OnDisable()
		{
			View.PaymentButton.OnBuyEvent -= OnBuy;
			View.PaymentButton.OnUpgradeEvent -= OnUpgrade;
			View.PaymentButton.OnRecoverEvent -= OnRecover;
		}

		private void OnBuy()
		{
			ShopModel shopModel = MonoSingleton<AppController>.Instance.Model.ShopModel;
			if (shopModel.OnBuyBlaster(_model))
			{
				OnEquip();
			}
		}

		private void OnUpgrade()
		{
			ShopModel shopModel = MonoSingleton<AppController>.Instance.Model.ShopModel;
			shopModel.OnUpgradeBlaster(_model);
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			if (missionSetting.Blaster == _model)
			{
				missionSetting.CommitAndNotify();
			}
		}

		private void OnRecover()
		{
			ShopModel shopModel = MonoSingleton<AppController>.Instance.Model.ShopModel;
			switch (View.PaymentButton.State)
			{
			case PaymentButtonState.Recover:
				shopModel.OnRecoverBlaster(_model);
				break;
			case PaymentButtonState.RecoverByAd:
				shopModel.OnRecoverBlasterByAD(_model);
				break;
			}
		}

		private void OnEquip()
		{
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			if (missionSetting != null && missionSetting.Blaster != null)
			{
				missionSetting.Blaster = _model;
			}
		}

		private void OnUnequip()
		{
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			missionSetting.SetDefaultBlaster();
		}

		public override void ApplyModel(object model)
		{
			if (model is BlasterItem)
			{
				if (_model != null)
				{
					_model.RemoveObserver(this);
				}
				_model = model as BlasterItem;
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

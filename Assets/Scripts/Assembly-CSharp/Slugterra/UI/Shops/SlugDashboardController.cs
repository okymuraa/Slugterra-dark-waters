using AppsMinistry.Core;
using AppsMinistry.Domain.Observer;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Game.HealthRestoration;
using Slugterra.Domain.Shops;

namespace Slugterra.UI.Shops
{
	public class SlugDashboardController : ItemDashboardController, IObserver
	{
		private SlugItem _model;

		protected new SlugDashboardView View
		{
			get
			{
				return base.View as SlugDashboardView;
			}
		}

		private RestoreHealthConfiguration healthRestoreManager
		{
			get
			{
				return MonoSingleton<GameConfiguration>.Instance.RestoreHealthConfigurationManager;
			}
		}

		public void OnObjectChanged(object observable)
		{
			if (observable is SlugItem)
			{
				View.ApplyModel(observable as SlugItem);
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
			if (shopModel.OnBuySlug(_model))
			{
				OnEquip();
			}
		}

		private void OnEquip()
		{
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			missionSetting.SetSlug(_model);
		}

		private void OnUnequip()
		{
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			if (MonoSingleton<AppController>.Instance.Model.ShopModel.AttackSlugs.FindIndex((SlugItem obj) => obj.Type == _model.Type) >= 0)
			{
				missionSetting.SetDefaultAttackSlug();
			}
			else if (MonoSingleton<AppController>.Instance.Model.ShopModel.DefenceSlugs.FindIndex((SlugItem obj) => obj.Type == _model.Type) >= 0)
			{
				missionSetting.SetDefaultDefenceSlug();
			}
			else if (MonoSingleton<AppController>.Instance.Model.ShopModel.SpecialSlugs.FindIndex((SlugItem obj) => obj.Type == _model.Type) >= 0)
			{
				missionSetting.SetDefaultSpecialSlug();
			}
		}

		private void OnUpgrade()
		{
			ShopModel shopModel = MonoSingleton<AppController>.Instance.Model.ShopModel;
			shopModel.OnUpgradeSlug(_model);
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			if (shopModel.AttackSlugs.Contains(_model))
			{
				missionSetting.AttackSlug = _model;
			}
			else if (shopModel.DefenceSlugs.Contains(_model))
			{
				missionSetting.DefenceSlug = _model;
			}
			else if (shopModel.SpecialSlugs.Contains(_model))
			{
				missionSetting.SpecialSlug = _model;
			}
		}

		private void OnRecover()
		{
			switch (View.PaymentButton.State)
			{
			case PaymentButtonState.Recover:
			{
				ShopModel shopModel = MonoSingleton<AppController>.Instance.Model.ShopModel;
				shopModel.OnRecoverSlug(_model);
				break;
			}
			case PaymentButtonState.RecoverByAd:
				healthRestoreManager.GetHealthConfiguration(_model.Type).Heal();
				healthRestoreManager.Save();
				break;
			}
		}

		public override void ApplyModel(object model)
		{
			if (model is SlugItem)
			{
				if (_model != null)
				{
					_model.RemoveObserver(this);
				}
				_model = model as SlugItem;
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

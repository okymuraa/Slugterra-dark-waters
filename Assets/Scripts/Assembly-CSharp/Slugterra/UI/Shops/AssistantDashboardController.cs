using AppsMinistry.Core;
using AppsMinistry.Domain.Observer;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Game.HealthRestoration;
using Slugterra.Domain.Shops;

namespace Slugterra.UI.Shops
{
	public class AssistantDashboardController : ItemDashboardController, IObserver
	{
		private AssistantItem _model;

		protected new AssistantDashboardView View
		{
			get
			{
				return base.View as AssistantDashboardView;
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
			if (observable is AssistantItem)
			{
				View.ApplyModel(observable as AssistantItem);
			}
		}

		private void OnEnable()
		{
			View.PaymentButton.OnBuyEvent += OnBuy;
			View.PaymentButton.OnUpgradeEvent += OnUpgrade;
			View.PaymentButton.OnRecoverEvent += OnHeal;
		}

		private void OnDisable()
		{
			View.PaymentButton.OnBuyEvent -= OnBuy;
			View.PaymentButton.OnUpgradeEvent -= OnUpgrade;
			View.PaymentButton.OnRecoverEvent -= OnHeal;
		}

		private void OnBuy()
		{
			ShopModel shopModel = MonoSingleton<AppController>.Instance.Model.ShopModel;
			if (shopModel.OnBuyAssistant(_model))
			{
				OnEquip();
			}
		}

		private void OnUpgrade()
		{
			ShopModel shopModel = MonoSingleton<AppController>.Instance.Model.ShopModel;
			shopModel.OnUpgradeAssistant(_model);
		}

		private void OnHeal()
		{
			switch (View.PaymentButton.State)
			{
			case PaymentButtonState.Recover:
				MonoSingleton<AppController>.Instance.Model.ShopModel.OnRecoverAssistant(_model);
				break;
			case PaymentButtonState.RecoverByAd:
				healthRestoreManager.GetHealthConfiguration(_model.Type).Heal();
				healthRestoreManager.Save();
				break;
			}
		}

		private void OnEquip()
		{
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			if (missionSetting.Assistant.Type != _model.Type)
			{
				missionSetting.Assistant = _model;
			}
		}

		private void OnUnequip()
		{
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			missionSetting.SetDefaultAssistant();
		}

		public override void ApplyModel(object model)
		{
			if (model is AssistantItem)
			{
				if (_model != null)
				{
					_model.RemoveObserver(this);
				}
				_model = model as AssistantItem;
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

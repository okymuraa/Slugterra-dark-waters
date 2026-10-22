using AppsMinistry.Core;
using Slugterra.App;
using Slugterra.Domain.Shops;

namespace Slugterra.UI.Shops
{
	public class BoostDashboardController : ItemDashboardController
	{
		private ConsumableItem _model;

		protected new BoostDashboardView View
		{
			get
			{
				return base.View as BoostDashboardView;
			}
		}

		private void OnEnable()
		{
			View.OnBuyEvent += OnBuyPack;
		}

		private void OnDisable()
		{
			View.OnBuyEvent -= OnBuyPack;
		}

		private void OnBuyPack(int count)
		{
			ShopModel shopModel = MonoSingleton<AppController>.Instance.Model.ShopModel;
			shopModel.OnBuyConsumable(_model, count);
		}

		public override void ApplyModel(object model)
		{
			if (model is ConsumableItem)
			{
				_model = model as ConsumableItem;
				View.ApplyModel(_model);
			}
		}
	}
}

using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.Domain.Shops;
using Slugterra.InAppPurchases;

namespace Slugterra.UI.Shops
{
	public class TicketItemController : UIController<TicketItemView>
	{
		private TicketItem _model;

		protected override void _OnEnable()
		{
			View.OnBuyEvent += OnBuy;
		}

		protected override void _OnDisable()
		{
			View.OnBuyEvent -= OnBuy;
		}

		public void ApplyModel(TicketItem model)
		{
			View.ApplyModel(model);
			_model = model;
		}

		private void OnBuy()
		{
			DonDestroyMonoSingleton<SlugterraPurchaseContext>.Instance.PurchaseProduct(_model.SKU);
		}
	}
}

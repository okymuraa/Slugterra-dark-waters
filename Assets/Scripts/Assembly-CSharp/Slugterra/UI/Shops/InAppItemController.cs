using AppsMinistry.Core;
using AppsMinistry.InAppPurchases;
using AppsMinistry.UI;
using Slugterra.InAppPurchases;

namespace Slugterra.UI.Shops
{
	public class InAppItemController : UIController<InAppItemView>
	{
		private InAppPurchase _model;

		public void ApplyModel(InAppPurchase model)
		{
			View.ApplyModel(model);
			_model = model;
		}

		private void OnBuy()
		{
			DonDestroyMonoSingleton<SlugterraPurchaseContext>.Instance.PurchaseProduct(_model.ProductID);
		}
	}
}

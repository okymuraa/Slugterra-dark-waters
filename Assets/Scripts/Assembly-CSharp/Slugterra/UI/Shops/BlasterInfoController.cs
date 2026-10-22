using AppsMinistry.UI;
using Slugterra.Domain.Shops;

namespace Slugterra.UI.Shops
{
	public class BlasterInfoController : UIController<BlasterInfoView>
	{
		private BlasterItem _model;

		private void OnEnable()
		{
		}

		private void OnDisable()
		{
		}

		public void ApplyModel(object model)
		{
			if (model is BlasterItem)
			{
				_model = model as BlasterItem;
				_model.IsBackpack = false;
				View.ApplyModel(_model);
			}
		}
	}
}

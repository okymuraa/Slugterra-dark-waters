using AppsMinistry.UI;
using Slugterra.Domain.Shops;

namespace Slugterra.UI.Shops
{
	public class ArmorInfoController : UIController<ArmorInfoView>
	{
		private ArmorItem _model;

		private void OnEnable()
		{
		}

		private void OnDisable()
		{
		}

		public void ApplyModel(object model)
		{
			if (model is ArmorItem)
			{
				_model = model as ArmorItem;
				View.ApplyModel(_model);
			}
		}
	}
}

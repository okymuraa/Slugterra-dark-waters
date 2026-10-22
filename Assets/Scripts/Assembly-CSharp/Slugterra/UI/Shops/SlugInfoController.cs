using AppsMinistry.UI;
using Slugterra.Domain.Shops;

namespace Slugterra.UI.Shops
{
	public class SlugInfoController : UIController<SlugInfoView>
	{
		private SlugItem _model;

		private void OnEnable()
		{
		}

		private void OnDisable()
		{
		}

		public void ApplyModel(object model)
		{
			if (model is SlugItem)
			{
				_model = model as SlugItem;
				View.ApplyModel(_model);
			}
		}
	}
}

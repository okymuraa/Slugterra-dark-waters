using AppsMinistry.UI;
using Slugterra.Domain.Shops;

namespace Slugterra.UI.Shops
{
	public class AllyInfoController : UIController<AllyInfoView>
	{
		private AssistantItem _model;

		private void OnEnable()
		{
		}

		private void OnDisable()
		{
		}

		public void ApplyModel(object model)
		{
			if (model is AssistantItem)
			{
				_model = model as AssistantItem;
				_model.IsBackpack = false;
				View.ApplyModel(_model);
			}
		}
	}
}

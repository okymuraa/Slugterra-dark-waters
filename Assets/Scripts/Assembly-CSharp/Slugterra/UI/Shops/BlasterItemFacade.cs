using Slugterra.Domain.Shops;

namespace Slugterra.UI.Shops
{
	public class BlasterItemFacade : ItemFacade
	{
		private BlasterItem _model;

		public override void OnObjectChanged(object observable)
		{
			base.OnObjectChanged(observable);
		}

		protected override void OnDisable()
		{
			if (_model != null)
			{
				_model.RemoveObserver(this);
			}
			base.OnDisable();
		}

		protected override void OnApplyModel(object model)
		{
			if (model is BlasterItem)
			{
				if (_model != null)
				{
					_model.RemoveObserver(this);
				}
				_model = model as BlasterItem;
				_model.AddObserver(this);
			}
		}
	}
}

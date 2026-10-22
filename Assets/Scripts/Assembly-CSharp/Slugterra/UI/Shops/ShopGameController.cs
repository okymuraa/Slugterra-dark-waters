using AppsMinistry.Core;
using AppsMinistry.Domain.Observer;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Domain.Shops;

namespace Slugterra.UI.Shops
{
	public class ShopGameController : UIController<ShopGameView>, IObserver
	{
		private ShopModel _model;

		protected override void OnStart()
		{
			_model = MonoSingleton<AppController>.Instance.Model.ShopModel;
			_model.AddObserver(this);
			View.ApplyModel(_model);
		}

		public void OnObjectChanged(object observable)
		{
			if (observable is ShopModel)
			{
				View.ApplyModel(observable);
			}
		}

		protected override void OnReleaseResource()
		{
			if (_model != null)
			{
				_model.RemoveObserver(this);
			}
			base.OnReleaseResource();
		}
	}
}

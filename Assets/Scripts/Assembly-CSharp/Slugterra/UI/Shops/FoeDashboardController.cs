using AppsMinistry.Core;
using AppsMinistry.Domain.Observer;
using Slugterra.App;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Shops;

namespace Slugterra.UI.Shops
{
	public class FoeDashboardController : ItemDashboardController, IObserver
	{
		private FoeItem _model;

		protected new FoeDashboardView View
		{
			get
			{
				return base.View as FoeDashboardView;
			}
		}

		public void OnObjectChanged(object observable)
		{
			if (observable is FoeItem)
			{
				View.ApplyModel(observable as FoeItem);
			}
		}

		private void OnEquip()
		{
			DuelSetting duelSetting = MonoSingleton<AppController>.Instance.Model.DuelSetting;
			if (_model != null && duelSetting.Foe != null)
			{
				duelSetting.Foe = _model;
			}
		}

		public override void ApplyModel(object model)
		{
			if (model is FoeItem)
			{
				if (_model != null)
				{
					_model.RemoveObserver(this);
				}
				_model = model as FoeItem;
				_model.AddObserver(this);
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

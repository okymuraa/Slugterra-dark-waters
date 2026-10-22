using AppsMinistry.Core;
using AppsMinistry.Domain.Observer;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Shops;

namespace Slugterra.UI.Mission
{
	public class BlasterItemController : UIController<BlasterItemView>, IObserver
	{
		private BlasterItem _model;

		public void OnObjectChanged(object observable)
		{
			if (observable is BlasterItem)
			{
				View.ApplyModel(observable as BlasterItem);
			}
		}

		protected override void _OnEnable()
		{
			View.OnUseEvent += OnUse;
		}

		protected override void _OnDisable()
		{
			View.OnUseEvent -= OnUse;
		}

		private void OnUse()
		{
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			missionSetting.Blaster = _model;
		}

		public void ApplyModel(BlasterItem item)
		{
			if (_model != null)
			{
				_model.RemoveObserver(this);
			}
			_model = item;
			_model.AddObserver(this);
			View.ApplyModel(_model);
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

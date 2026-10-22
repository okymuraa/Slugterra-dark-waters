using AppsMinistry.Core;
using AppsMinistry.Domain.Observer;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Shops;

namespace Slugterra.UI.Mission
{
	public class ArmorItemController : UIController<ArmorItemView>, IObserver
	{
		private ArmorItem _model;

		public void OnObjectChanged(object observable)
		{
			if (observable is ArmorItem)
			{
				View.ApplyModel(observable as ArmorItem);
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
			missionSetting.Armor = _model;
		}

		public void ApplyModel(ArmorItem item)
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

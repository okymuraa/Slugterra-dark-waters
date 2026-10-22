using AppsMinistry.Core;
using AppsMinistry.Domain.Observer;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Domain.App.Settings;

namespace Slugterra.UI.Shops
{
	public class EliSpecsController : UIController<EliSpecsView>, IObserver
	{
		private MissionSetting _model;

		protected override void OnCreate()
		{
			_model = MonoSingleton<AppController>.Instance.Model.MissionSetting;
		}

		public void OnObjectChanged(object observable)
		{
			if (observable is MissionSetting)
			{
				View.ApplyModel(observable as MissionSetting);
			}
		}

		protected override void _OnEnable()
		{
			if (_model != null)
			{
				_model.AddObserver(this);
				View.ApplyModel(_model);
			}
		}

		protected override void _OnDisable()
		{
			if (_model != null)
			{
				_model.RemoveObserver(this);
			}
		}
	}
}

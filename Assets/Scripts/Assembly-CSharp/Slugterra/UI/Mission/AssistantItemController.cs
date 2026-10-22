using AppsMinistry.Core;
using AppsMinistry.Domain.Observer;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Shops;

namespace Slugterra.UI.Mission
{
	public class AssistantItemController : UIController<AssistantItemView>, IObserver
	{
		private AssistantItem _model;

		public void OnObjectChanged(object observable)
		{
			if (observable is AssistantItem)
			{
				View.ApplyModel(observable as AssistantItem);
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
			ShopModel shopModel = MonoSingleton<AppController>.Instance.Model.ShopModel;
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			if (shopModel.Assistants.Contains(_model))
			{
				missionSetting.Assistant = _model;
			}
		}

		public void ApplyModel(AssistantItem item)
		{
			if (_model != null)
			{
				_model.RemoveObserver(this);
			}
			_model = item;
			_model.AddObserver(this);
			View.ApplyModel(item);
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

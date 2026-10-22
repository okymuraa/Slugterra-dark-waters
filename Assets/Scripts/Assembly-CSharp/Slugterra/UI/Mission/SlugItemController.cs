using AppsMinistry.Core;
using AppsMinistry.Domain.Observer;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Shops;

namespace Slugterra.UI.Mission
{
	public class SlugItemController : UIController<SlugItemView>, IObserver
	{
		private SlugItem _model;

		public void OnObjectChanged(object observable)
		{
			if (observable is SlugItem)
			{
				View.ApplyModel(observable as SlugItem);
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
			if (shopModel.AttackSlugs.Contains(_model))
			{
				missionSetting.AttackSlug = _model;
			}
			else if (shopModel.DefenceSlugs.Contains(_model))
			{
				missionSetting.DefenceSlug = _model;
			}
			else if (shopModel.SpecialSlugs.Contains(_model))
			{
				missionSetting.SpecialSlug = _model;
			}
		}

		public void ApplyModel(SlugItem item)
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

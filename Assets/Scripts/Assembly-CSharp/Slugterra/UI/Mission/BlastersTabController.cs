using AppsMinistry.Core;
using AppsMinistry.Domain.Observer;
using Slugterra.App;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Shops;
using UnityEngine;

namespace Slugterra.UI.Mission
{
	public class BlastersTabController : MissionTabController, IObserver
	{
		[SerializeField]
		private BlasterScrollViewController _scrollView;

		private MissionSetting _setting;

		public void OnObjectChanged(object observable)
		{
			if (observable is MissionSetting)
			{
				_scrollView.Rebuild();
			}
		}

		protected override void OnCreate()
		{
			base.OnCreate();
			_setting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
		}

		protected override void _OnEnable()
		{
			base._OnEnable();
			_setting.AddObserver(this);
		}

		protected override void _OnDisable()
		{
			base._OnDisable();
			if (_setting != null)
			{
				_setting.RemoveObserver(this);
			}
		}

		public override void ApplyModel(object model)
		{
			base.ApplyModel(model);
			if (model is ShopModel)
			{
				_scrollView.ApplyModel((model as ShopModel).GetAvailebleBlasters());
			}
		}
	}
}

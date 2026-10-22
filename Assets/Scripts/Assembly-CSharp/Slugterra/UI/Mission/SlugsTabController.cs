using AppsMinistry.Core;
using AppsMinistry.Domain.Observer;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Shops;
using UnityEngine;

namespace Slugterra.UI.Mission
{
	public class SlugsTabController : MissionTabController, IObserver
	{
		[SerializeField]
		private SlugScrollViewController _scrollView;

		private SlugGroup _activeTab;

		private MissionSetting _setting;

		protected new SlugsTabView View
		{
			get
			{
				return base.View as SlugsTabView;
			}
		}

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
			View.OnSlugGroupChangedEvent += OnSlugGroupChanged;
		}

		protected override void _OnDisable()
		{
			base._OnDisable();
			if (_setting != null)
			{
				_setting.RemoveObserver(this);
			}
			View.OnSlugGroupChangedEvent -= OnSlugGroupChanged;
		}

		public override void ApplyModel(object model)
		{
			base.ApplyModel(model);
			if (model is ShopModel)
			{
				OnSlugGroupChanged(SlugGroup.Attack);
			}
		}

		private void OnSlugGroupChanged(SlugGroup slugGroup)
		{
			if (_tabModel != null)
			{
				ShopModel shopModel = _tabModel as ShopModel;
				_activeTab = slugGroup;
				if (_activeTab == SlugGroup.Attack)
				{
					_scrollView.ApplyModel(shopModel.GetAvailebleAttackSlugs());
				}
				else if (_activeTab == SlugGroup.Control)
				{
					_scrollView.ApplyModel(shopModel.GetAvailebleDefenceSlugs());
				}
				else if (_activeTab == SlugGroup.Special)
				{
					_scrollView.ApplyModel(shopModel.GetAvailebleSpecialSlugs());
				}
				_scrollView.SetCurrentItem(0);
				_scrollView.Rebuild();
				View.ApplyModel(_activeTab);
			}
		}
	}
}

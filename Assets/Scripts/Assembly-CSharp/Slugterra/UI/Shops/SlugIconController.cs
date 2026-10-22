using AppsMinistry.Core;
using AppsMinistry.Domain.Observer;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class SlugIconController : UIController<SlugIconView>, IObserver
	{
		[SerializeField]
		private SlugGroup _slugGroup;

		private MissionSetting _setting;

		public void OnObjectChanged(object observable)
		{
			if (observable is MissionSetting)
			{
				View.ApplyModel((observable as MissionSetting).GetSlugByGroup(_slugGroup));
			}
		}

		protected override void OnCreate()
		{
			base.OnCreate();
			_setting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
		}

		protected override void _OnEnable()
		{
			if (_setting != null)
			{
				_setting.AddObserver(this);
				View.ApplyModel(_setting.GetSlugByGroup(_slugGroup));
			}
		}

		protected override void _OnDisable()
		{
			if (_setting != null)
			{
				_setting.RemoveObserver(this);
			}
		}
	}
}

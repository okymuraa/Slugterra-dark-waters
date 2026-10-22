using System;
using AppsMinistry.Core;
using AppsMinistry.Domain.Observer;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Domain.App;
using Slugterra.Domain.App.Settings;
using Slugterra.Scene;

namespace Slugterra.UI.Menu
{
	public class ChoseBossMenuController : UIController<ChoseBossMenuView>, IObserver
	{
		private DuelSetting _model;

		protected override void OnCreate()
		{
			AppModel model = MonoSingleton<AppController>.Instance.Model;
			_model = model.DuelSetting;
		}

		protected override void OnStart()
		{
			View.OnPlayEvent += OnPlay;
			View.OnBackEvent += OnBack;
			View.OnChoseBossEvent += OnChangeBoss;
			_model.AddObserver(this);
			View.ApplyModel(_model);
		}

		protected override void OnReleaseResource()
		{
			_model.RemoveObserver(this);
		}

		public void OnObjectChanged(object observable)
		{
			if (observable is DuelSetting)
			{
				OnDuelSettingChanged(observable as DuelSetting);
			}
		}

		private void OnDuelSettingChanged(DuelSetting duelSetting)
		{
			View.ApplyModel(duelSetting);
		}

		private void OnPlay()
		{
		}

		private void OnBack()
		{
			SceneLoader.LoadPrevScene();
		}

		private void OnChangeBoss(Type bossType)
		{
		}
	}
}

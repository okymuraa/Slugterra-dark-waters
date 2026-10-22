using System;
using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Domain.App;

namespace Slugterra.UI.Survival
{
	public class SurvivalMapItemController : UIController<SurvivalMapItemView>
	{
		protected override void _OnEnable()
		{
			SurvivalMapItemView view = View;
			view.OnSurvivalSelectBtnClickEvent = (Action<Type>)Delegate.Combine(view.OnSurvivalSelectBtnClickEvent, new Action<Type>(OnPlaySurvivalBtnClick));
		}

		private void OnPlaySurvivalBtnClick(Type scene)
		{
			AppModel model = MonoSingleton<AppController>.Instance.Model;
			model.MissionSetting.Mission = scene;
		}
	}
}

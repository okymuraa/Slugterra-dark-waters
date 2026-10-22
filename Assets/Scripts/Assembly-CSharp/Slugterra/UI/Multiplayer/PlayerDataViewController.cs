using AppsMinistry.Core;
using Slugterra.App;
using UnityEngine;

namespace Slugterra.UI.Multiplayer
{
	public class PlayerDataViewController : PlayerInfoView
	{
		[SerializeField]
		private tk2dSprite[] _stars;

		protected override void OnStart()
		{
			base.OnStart();
			OnApplyModel(MonoSingleton<AppController>.Instance.Model.MissionSetting);
		}
	}
}

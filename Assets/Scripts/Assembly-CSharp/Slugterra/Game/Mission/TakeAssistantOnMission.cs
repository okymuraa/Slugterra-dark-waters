using AppsMinistry.Core;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using UnityEngine;

namespace Slugterra.Game.Mission
{
	internal class TakeAssistantOnMission : ObjectiveTarget
	{
		[SerializeField]
		private AllyType _assistantTyep;

		protected override void OnStart()
		{
			base.OnStart();
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			if (missionSetting.Assistant.Type == _assistantTyep)
			{
				Completed = true;
			}
			else
			{
				Completed = false;
			}
		}
	}
}

using AppsMinistry.Core;
using AppsMinistry.Domain.Observer;
using Slugterra.Domain.Game.Mission;
using UnityEngine;

namespace Slugterra.Game.Mission
{
	internal class DefeatWithExploding : ObjectiveTarget, IObserver
	{
		[SerializeField]
		private int enemyesCount = 1;

		protected override void OnCreate()
		{
			base.OnCreate();
			MissionStatistics model = MonoSingleton<MissionStatisticsController>.Instance.Model;
			model.AddObserver(this);
		}

		public new void OnObjectChanged(object observable)
		{
			if (observable is MissionStatistics)
			{
				MissionStatistics missionStatistics = observable as MissionStatistics;
				if (missionStatistics.KilledByEnvironment == enemyesCount)
				{
					Completed = true;
					missionStatistics.RemoveObserver(this);
				}
			}
		}
	}
}

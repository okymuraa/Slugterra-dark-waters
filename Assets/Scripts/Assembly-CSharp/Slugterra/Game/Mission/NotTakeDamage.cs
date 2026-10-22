using AppsMinistry.Core;
using AppsMinistry.Domain.Observer;
using Slugterra.Domain.Game.Mission;
using Slugterra.UI.Game;
using UnityEngine;

namespace Slugterra.Game.Mission
{
	internal class NotTakeDamage : ObjectiveTarget, IObserver
	{
		private GameController _gameController;

		protected override void OnCreate()
		{
			base.OnCreate();
			MissionStatistics model = MonoSingleton<MissionStatisticsController>.Instance.Model;
			model.AddObserver(this);
			_gameController = Object.FindObjectOfType<GameController>();
		}

		private void OnEnable()
		{
			_gameController.OnCompleteMissionEvent += OnComplete;
		}

		private void OnDisable()
		{
			_gameController.OnCompleteMissionEvent -= OnComplete;
		}

		private void OnComplete()
		{
			MissionStatistics model = MonoSingleton<MissionStatisticsController>.Instance.Model;
			if (model.DamageEliTook == 0f)
			{
				Completed = true;
			}
		}

		public new void OnObjectChanged(object observable)
		{
			if (observable is MissionStatistics)
			{
				MissionStatistics missionStatistics = observable as MissionStatistics;
				if (missionStatistics.DamageEliTook > 0f)
				{
					Completed = false;
					missionStatistics.RemoveObserver(this);
				}
			}
		}
	}
}

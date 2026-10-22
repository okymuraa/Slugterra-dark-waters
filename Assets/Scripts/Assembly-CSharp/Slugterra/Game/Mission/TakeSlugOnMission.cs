using AppsMinistry.Core;
using AppsMinistry.Domain.Observer;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using UnityEngine;

namespace Slugterra.Game.Mission
{
	internal class TakeSlugOnMission : ObjectiveTarget, IObserver
	{
		[SerializeField]
		private SlugType _slugType;

		[SerializeField]
		private bool _needToUse;

		[SerializeField]
		private int _countOfUse = 1;

		private int _currentCountOfUse;

		private bool _activated;

		private void OnEnable()
		{
			if (MonoSingleton<MissionStatisticsController>.Instance != null)
			{
				MonoSingleton<MissionStatisticsController>.Instance.OnEliFireEvent += OnEliFire;
			}
		}

		private void OnDisable()
		{
			if (MonoSingleton<MissionStatisticsController>.Instance != null)
			{
				MonoSingleton<MissionStatisticsController>.Instance.OnEliFireEvent -= OnEliFire;
			}
		}

		protected override void OnStart()
		{
			base.OnStart();
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			if (_slugType == SlugType.None || missionSetting.AttackSlug.Type == _slugType || missionSetting.DefenceSlug.Type == _slugType || missionSetting.SpecialSlug.Type == _slugType)
			{
				if (!_needToUse)
				{
					Completed = true;
				}
			}
			else
			{
				Completed = false;
			}
		}

		protected override void OnActivate()
		{
			base.OnActivate();
			_activated = true;
		}

		private void OnEliFire(SlugType slugType)
		{
			if (Model.IsActive)
			{
				if ((_slugType == SlugType.None && slugType != SlugType.Flopper) || slugType == _slugType)
				{
					_currentCountOfUse++;
				}
				if (_currentCountOfUse == _countOfUse)
				{
					Completed = true;
				}
			}
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();
			if (!Model.IsActive && _activated)
			{
				Completed = false;
			}
		}
	}
}

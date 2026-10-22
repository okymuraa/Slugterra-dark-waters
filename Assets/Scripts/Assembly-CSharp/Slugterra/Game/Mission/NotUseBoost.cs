using AppsMinistry.Core;
using Slugterra.Configuration;
using Slugterra.Domain.Game.Mission;
using Slugterra.UI.Game;
using UnityEngine;

namespace Slugterra.Game.Mission
{
	internal class NotUseBoost : ObjectiveTarget
	{
		[SerializeField]
		private bool _cantUseAllBoosts;

		[SerializeField]
		private ConsumableType _boostType;

		private GameController _gameController;

		private bool _activated;

		private int[] _countsOfUsedBoosts = new int[4];

		private bool _failed;

		protected override void OnCreate()
		{
			base.OnCreate();
			MissionStatistics model = MonoSingleton<MissionStatisticsController>.Instance.Model;
			model.AddObserver(this);
			_gameController = Object.FindObjectOfType<GameController>();
		}

		protected override void OnActivate()
		{
			base.OnActivate();
			_activated = true;
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
			if (!_cantUseAllBoosts)
			{
				if (GetCountOfUsing(_boostType) == 0)
				{
					Completed = true;
				}
				return;
			}
			for (ConsumableType consumableType = ConsumableType.First; consumableType < ConsumableType.Last; consumableType++)
			{
				if (GetCountOfUsing(consumableType) == 0)
				{
					Completed = true;
				}
			}
		}

		public override void OnObjectChanged(object observable)
		{
			base.OnObjectChanged(observable);
			if (!(observable is MissionStatistics))
			{
				return;
			}
			MissionStatistics missionStatistics = observable as MissionStatistics;
			if (!_cantUseAllBoosts)
			{
				if (Model.IsActive && GetCountOfUsing(_boostType) - _countsOfUsedBoosts[(int)_boostType] > 0)
				{
					_failed = true;
				}
			}
			else
			{
				for (ConsumableType consumableType = ConsumableType.First; consumableType < ConsumableType.Last; consumableType++)
				{
					if (Model.IsActive && GetCountOfUsing(consumableType) - _countsOfUsedBoosts[(int)consumableType] > 0)
					{
						_failed = true;
					}
				}
			}
			if (_failed)
			{
				Completed = false;
				missionStatistics.RemoveObserver(this);
			}
			else
			{
				SetCountsOfUsingBoosts();
			}
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();
			if (!Model.IsActive && _activated && !_failed)
			{
				Completed = true;
			}
		}

		private void SetCountsOfUsingBoosts()
		{
			for (ConsumableType consumableType = ConsumableType.First; consumableType < ConsumableType.Last; consumableType++)
			{
				_countsOfUsedBoosts[(int)consumableType] = GetCountOfUsing(consumableType);
			}
		}

		private int GetCountOfUsing(ConsumableType type)
		{
			int result = 0;
			MissionStatistics model = MonoSingleton<MissionStatisticsController>.Instance.Model;
			switch (type)
			{
			case ConsumableType.First:
				result = model.UsedMedKitCount;
				break;
			case ConsumableType.SpeedBoost:
				result = model.UsedSpeedBoostCount;
				break;
			case ConsumableType.ArmorBoost:
				result = model.UsedArmorBoostCount;
				break;
			case ConsumableType.SlugFood:
				result = model.UsedSlugFoodCount;
				break;
			}
			return result;
		}
	}
}

using System;
using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.Game.UI;
using Slugterra.Scene.Daily;
using Slugterra.UI.MapMenu;
using UnityEngine;

namespace Slugterra.UI.Game
{
	public class ObjectivesView : UIView
	{
		[SerializeField]
		private TextMesh _missionNumber;

		[SerializeField]
		private TextMesh _primaryObjective;

		[SerializeField]
		private TextMesh _secondaryObjective;

		[SerializeField]
		private ObjectiveCheckboxController _mainObjective;

		[SerializeField]
		private ObjectiveCheckboxController _bonusObjectiveFirst;

		[SerializeField]
		private ObjectiveCheckboxController _bonusObjectiveSecond;

		public ObjectiveCheckboxController MainObjective
		{
			get
			{
				return _mainObjective;
			}
		}

		public ObjectiveCheckboxController BonusObjectiveFirst
		{
			get
			{
				return _bonusObjectiveFirst;
			}
		}

		public ObjectiveCheckboxController BonusObjectiveSecond
		{
			get
			{
				return _bonusObjectiveSecond;
			}
		}

		private void Awake()
		{
			LevelItem.NewScores.Clear();
		}

		protected override void OnStart()
		{
			base.OnStart();
			MonoSingleton<Localization>.Instance.ConfigTextMesh(_missionNumber);
			MonoSingleton<Localization>.Instance.ConfigTextMesh(_primaryObjective);
			MonoSingleton<Localization>.Instance.ConfigTextMesh(_secondaryObjective);
		}

		private void OnEnable()
		{
			LevelItemController.OnSelectlevelItem = (Action<LevelItemController>)Delegate.Combine(LevelItemController.OnSelectlevelItem, new Action<LevelItemController>(SelectLevelItem));
			if (Application.loadedLevelName != "MapScene" && Application.loadedLevelName != "MapScene_HD")
			{
				SelectLevelItem(null);
			}
		}

		private void OnDisable()
		{
			LevelItemController.OnSelectlevelItem = (Action<LevelItemController>)Delegate.Remove(LevelItemController.OnSelectlevelItem, new Action<LevelItemController>(SelectLevelItem));
		}

		private void SelectLevelItem(LevelItemController item)
		{
			LevelItem levelItem = ((!(item != null)) ? MonoSingleton<AppController>.Instance.GetCurrentGameLevel() : item.Model);
			LevelItem levelItem2 = MonoSingleton<AppController>.Instance.Model.GetLevelItem(DailySceneManager.GetDaily().Logic);
			_primaryObjective.text = MonoSingleton<Localization>.Instance["main_obj_title"];
			_secondaryObjective.text = MonoSingleton<Localization>.Instance["secondary_obj_title"];
			_missionNumber.text = MonoSingleton<Localization>.Instance[MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetLevelDesc(levelItem.Scene.Name)];
			MainObjective.View.Label.text = MonoSingleton<Localization>.Instance[MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetObjectiveDescription(levelItem.Scene.Name, 0)];
			BonusObjectiveFirst.View.Label.text = MonoSingleton<Localization>.Instance[MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetObjectiveDescription(levelItem.Scene.Name, 1)];
			BonusObjectiveSecond.View.Label.text = MonoSingleton<Localization>.Instance[MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetObjectiveDescription(levelItem.Scene.Name, 2)];
			if (levelItem.Name.Contains("daily"))
			{
				MainObjective.View.Label.text = MonoSingleton<Localization>.Instance[MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetObjectiveDescription(levelItem.Scene.Name, 0)];
				BonusObjectiveFirst.View.Label.text = MonoSingleton<Localization>.Instance[MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetObjectiveDescription(levelItem.Scene.Name, 1)];
				BonusObjectiveSecond.View.Label.text = MonoSingleton<Localization>.Instance[MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetObjectiveDescription(levelItem.Scene.Name, 2)];
				MainObjective.State = ((levelItem2.GetObjectiveStatus(0) == 1) ? ObjectiveCheckboxState.Success : ObjectiveCheckboxState.Unset);
				BonusObjectiveFirst.State = ((levelItem2.GetObjectiveStatus(1) == 1) ? ObjectiveCheckboxState.Success : ObjectiveCheckboxState.Unset);
				BonusObjectiveSecond.State = ((levelItem2.GetObjectiveStatus(2) == 1) ? ObjectiveCheckboxState.Success : ObjectiveCheckboxState.Unset);
			}
			if (Application.loadedLevelName == "MapScene" || Application.loadedLevelName == "MapScene_HD")
			{
				MainObjective.State = ((levelItem.GetObjectiveStatus(0) == 1) ? ObjectiveCheckboxState.Success : ObjectiveCheckboxState.Unset);
				BonusObjectiveFirst.State = ((levelItem.GetObjectiveStatus(1) == 1) ? ObjectiveCheckboxState.Success : ObjectiveCheckboxState.Unset);
				BonusObjectiveSecond.State = ((levelItem.GetObjectiveStatus(2) == 1) ? ObjectiveCheckboxState.Success : ObjectiveCheckboxState.Unset);
			}
			else
			{
				MainObjective.State = (ObjectiveCheckboxState)LevelItem.GetNewObjectiveStatus(0);
				BonusObjectiveFirst.State = (ObjectiveCheckboxState)LevelItem.GetNewObjectiveStatus(1);
				BonusObjectiveSecond.State = (ObjectiveCheckboxState)LevelItem.GetNewObjectiveStatus(2);
			}
		}
	}
}

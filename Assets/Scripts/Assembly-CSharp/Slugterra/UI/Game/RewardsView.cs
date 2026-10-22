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
	public class RewardsView : UIView
	{
		[SerializeField]
		private TextMesh _coins;

		[SerializeField]
		private TextMesh _gems;

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
			LevelItem levelItem = ((!(item != null) || item.Model == null) ? MonoSingleton<AppController>.Instance.GetCurrentGameLevel() : item.Model);
			if (levelItem != null)
			{
				LevelItem levelItem2 = MonoSingleton<AppController>.Instance.Model.GetLevelItem(DailySceneManager.GetDaily().Logic);
				LevelProperties levelProperties = MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetLevelProperties(levelItem.Scene.Name);
				if (levelItem.GetObjectiveStatus(0) == 1)
				{
					_coins.text = ((int)(levelProperties.mainObjectiveRepeatRewardGold + levelProperties.firstObjectiveRepeatRewardGold + levelProperties.secondaryObjectiveRepeatRewardGold)/*cast due to constrained. prefix*/).ToString();
					_gems.text = ((int)(levelProperties.mainObjectiveRepeatRewardGems + levelProperties.firstObjectiveRepeatRewardGems + levelProperties.secondaryObjectiveRepeatRewardGems)/*cast due to constrained. prefix*/).ToString();
				}
				else
				{
					_coins.text = ((int)(levelProperties.mainObjectiveRewardGold + levelProperties.firstObjectiveRewardGold + levelProperties.secondaryObjectiveRewardGold)/*cast due to constrained. prefix*/).ToString();
					_gems.text = ((int)(levelProperties.mainObjectiveRewardGems + levelProperties.firstObjectiveRewardGems + levelProperties.secondaryObjectiveRewardGems)/*cast due to constrained. prefix*/).ToString();
				}
			}
		}
	}
}

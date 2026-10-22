using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Scene;
using Slugterra.UI.MapMenu;
using UnityEngine;

namespace Slugterra.UI.Popup
{
	public class PopupTutorialCheckOutController : UIController<PopupTutorialCheckOutView>
	{
		protected override void _OnEnable()
		{
			View.OnArenaEvent += OnBtnArenaClick;
			View.OnSurvivalEvent += OnSurvival;
		}

		protected override void _OnDisable()
		{
			View.OnArenaEvent -= OnBtnArenaClick;
			View.OnSurvivalEvent -= OnSurvival;
		}

		private void OnBtnArenaClick()
		{
			LevelItemController levelItemController = null;
			MapMenuView mapMenuView = Object.FindObjectOfType<MapMenuView>();
			if (mapMenuView != null)
			{
				foreach (LevelItemController levelItem in mapMenuView.LevelItems)
				{
					if (levelItem.Model.Scene.Name == "ScorchedSparksTutorial_1_1")
					{
						levelItemController = levelItem;
						break;
					}
				}
			}
			if (levelItemController != null)
			{
				LevelItemController.SelectLevelItem(levelItemController);
				MonoSingleton<AppController>.Instance.Model.MissionSetting.Mission = levelItemController.Model.Scene;
				SceneLoader.LoadScene<BackpackSceneController>();
			}
		}

		private void OnSurvival()
		{
			SceneLoader.LoadScene<SurvivalMapSceneController>();
		}
	}
}

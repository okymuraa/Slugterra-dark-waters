using System.Collections;
using AppsMinistry.Core;
using Slugterra.App;
using Slugterra.Domain.Game.UI;
using Slugterra.Scene.Game;
using UnityEngine;

public class CutScene_Multiplayer_MainScene : MonoBehaviour
{
	private bool TutorialMapSceneComplete2 = true;

	private bool TutorialMapSceneComplete3 = true;

	public GameObject hand2dFlip;

	public Vector3 offsetflip;

	private tk2dButton btmCurrent;

	private tk2dButton[] btnMission;

	private int i;

	private void Start()
	{
		TutorialMapSceneComplete3 = true;
		TutorialMapSceneComplete2 = true;
		if (PlayerPrefs.GetInt("Tutor_CutMegaShopScene") == 0 && MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(QuietLawn_2_3)).Status != LevelStatus.Completed)
		{
			TutorialMapSceneComplete2 = false;
		}
		if (PlayerPrefs.GetInt("Tutor_Multiplayer") == 0)
		{
			TutorialMapSceneComplete3 = false;
		}
		if (TutorialMapSceneComplete2 && !TutorialMapSceneComplete3)
		{
			AfterStart();
		}
	}

	private void AfterStart()
	{
		btnMission = Object.FindObjectsOfType<tk2dButton>();
		for (i = 0; i < btnMission.Length; i++)
		{
			if (btnMission[i].transform.parent.name == "btnMultiplayer")
			{
				btmCurrent = btnMission[i];
				StartCoroutine(ShowHandFlip(1f));
			}
			else
			{
				btnMission[i].enabled = false;
			}
		}
		for (i = 0; i < btnMission.Length; i++)
		{
			if ((btnMission[i].targetObject != null && btnMission[i].targetObject.name == "PopupDailyBonus") || (btnMission[i].targetObject != null && btnMission[i].targetObject.name == "PopupConfirm") || (btnMission[i].targetObject != null && btnMission[i].targetObject.name == "PopupAdFeedback") || (btnMission[i].targetObject != null && btnMission[i].targetObject.name == "PopupFacebookGoals"))
			{
				btnMission[i].enabled = true;
			}
		}
	}

	private IEnumerator ShowHandFlip(float weit)
	{
		yield return new WaitForSeconds(weit);
		hand2dFlip.transform.position = btmCurrent.transform.position + offsetflip;
		hand2dFlip.SetActive(true);
	}
}

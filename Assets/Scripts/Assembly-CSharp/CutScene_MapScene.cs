using System.Collections;
using AppsMinistry.Core;
using AppsMinistry.Core.Input.Window;
using DG.Tweening;
using Slugterra.App;
using Slugterra.Domain.Game.UI;
using Slugterra.Scene.Game;
using Slugterra.UI.Game.Camera;
using Slugterra.UI.MapMenu;
using Slugterra.UI.Popup;
using UnityEngine;

public class CutScene_MapScene : MonoBehaviour
{
	private bool TutorialMapSceneComplete = true;

	private bool TutorialMapSceneComplete0 = true;

	private bool fix_2_2;

	private bool TutorialMapSceneComplete1 = true;

	private bool TutorialMapSceneComplete2 = true;

	private bool TutorialMapSceneComplete3 = true;

	public Transform Level_2_1;

	public Transform Level_2_2;

	public Transform Level_Arena;

	public Transform Level_2_3;

	public GameObject Equip;

	public GameObject EquipFalse;

	public GameObject EquipText;

	public GameObject btnBack;

	public GameObject hand2d;

	public GameObject hand2dFlip;

	public Vector3 offset;

	public Vector3 offsetflip;

	public Vector3 offsetbtnMission;

	public GameObject hand3d;

	public TextMesh help;

	public string keyHelp;

	public GameObject AdFeedBack;

	public GameObject PopupConfirm;

	public GameObject PopupMultiplayer;

	private GameObject FX_ChosenItem;

	private tk2dButton btmCurrent;

	private tk2dButton[] btnMission;

	private LevelItemController[] LevelItem;

	private LevelItemController LevelItemCurrent;

	private int i;

	private tk2dButton[] popupButtons;

	private RaycastHit hit;

	private MapCamera _mapCamera;

	private Transform _camera;

	private Transform _cameraUi;

	public Renderer shadow;

	private Color black;

	private float missionWeit;

	private bool missionTap;

	private int indexShadow;

	public Vector3 CameraPosition;

	public Vector3 CameraPosition0;

	public Vector3 CameraPosition1;

	public Vector3 CameraPosition2;

	private MapMenuView mapMenuView;

	private PopupMultiplayerAvailabilityView popupMultiplayerAvailabilityView;

	private bool AfterStart3Lock;

	private Transform BtnMultiplayer;

	private void OnEnable()
	{
		mapMenuView = Object.FindObjectOfType<MapMenuView>();
		mapMenuView.OnLevelItemCreatedEvent += StartTutor;
	}

	private void OnDisable()
	{
		if (mapMenuView != null)
		{
			mapMenuView.OnLevelItemCreatedEvent -= StartTutor;
		}
	}

	private void StartTutor()
	{
		hand2d.SetActive(false);
		hand3d.SetActive(false);
		CameraPosition = new Vector3(-15.752f, 13f, -7f);
		CameraPosition0 = new Vector3(-14.5f, 13f, -7f);
		CameraPosition1 = new Vector3(-17f, 13f, -7f);
		CameraPosition2 = new Vector3(-4.782f, 13f, -7f);
		TutorialMapSceneComplete = true;
		if (MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(QuietLawn_2_1)).Status != LevelStatus.Locked && MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(QuietLawn_2_1)).Status != LevelStatus.Completed)
		{
			TutorialMapSceneComplete = false;
		}
		TutorialMapSceneComplete0 = true;
		if (MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(QuietLawn_2_2)).Status != LevelStatus.Locked && MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(QuietLawn_2_2)).Status != LevelStatus.Completed)
		{
			TutorialMapSceneComplete0 = false;
			fix_2_2 = true;
		}
		TutorialMapSceneComplete1 = true;
		if (MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(ScorchedSparksTutorial_1_1)).Status != LevelStatus.Locked && MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(ScorchedSparksTutorial_1_1)).Status != LevelStatus.Completed)
		{
			TutorialMapSceneComplete1 = false;
		}
		TutorialMapSceneComplete1 = true;
		TutorialMapSceneComplete2 = true;
		if (PlayerPrefs.GetInt("Tutor_CutMegaShopScene") == 0 && MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(QuietLawn_2_2)).Status == LevelStatus.Completed && MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(QuietLawn_2_3)).Status != LevelStatus.Completed && TutorialMapSceneComplete1)
		{
			TutorialMapSceneComplete2 = false;
		}
		TutorialMapSceneComplete3 = true;
		if (PlayerPrefs.GetInt("Tutor_Multiplayer") == 0 && MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(QuietLawn_2_3)).Status == LevelStatus.Completed && TutorialMapSceneComplete2)
		{
			TutorialMapSceneComplete3 = false;
		}
		if (!TutorialMapSceneComplete)
		{
			shadow.gameObject.SetActive(true);
			black = Color.black;
			black.a = 0f;
			shadow.material.color = black;
			_mapCamera = Object.FindObjectOfType<MapCamera>();
			_mapCamera.enabled = false;
			_cameraUi = Object.FindObjectOfType<tk2dCamera>().transform;
			btnBack.transform.position -= Vector3.forward * 0.5f;
			_camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
			shadow.transform.parent = _cameraUi.transform;
			shadow.transform.localPosition = Vector3.zero;
			_camera.position = CameraPosition;
			AfterStart();
		}
		else
		{
			shadow.gameObject.SetActive(false);
		}
		if (!TutorialMapSceneComplete0)
		{
			black = Color.black;
			black.a = 0f;
			shadow.material.color = black;
			_mapCamera = Object.FindObjectOfType<MapCamera>();
			_mapCamera.enabled = false;
			_cameraUi = Object.FindObjectOfType<tk2dCamera>().transform;
			btnBack.transform.position -= Vector3.forward * 0.5f;
			_camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
			shadow.transform.parent = _cameraUi.transform;
			shadow.transform.localPosition = Vector3.zero;
			_camera.position = CameraPosition0;
			AfterStart0();
		}
		else
		{
			shadow.gameObject.SetActive(false);
		}
		if (!TutorialMapSceneComplete1)
		{
			black = Color.black;
			black.a = 0f;
			shadow.material.color = black;
			_mapCamera = Object.FindObjectOfType<MapCamera>();
			_mapCamera.enabled = false;
			_cameraUi = Object.FindObjectOfType<tk2dCamera>().transform;
			btnBack.transform.position -= Vector3.forward * 0.5f;
			_camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
			shadow.transform.parent = _cameraUi.transform;
			shadow.transform.localPosition = Vector3.zero;
			_camera.position = CameraPosition1;
			AfterStart1();
		}
		else
		{
			shadow.gameObject.SetActive(false);
		}
		if (!TutorialMapSceneComplete2)
		{
			black = Color.black;
			black.a = 0f;
			shadow.material.color = black;
			_mapCamera = Object.FindObjectOfType<MapCamera>();
			_mapCamera.enabled = false;
			_cameraUi = Object.FindObjectOfType<tk2dCamera>().transform;
			btnBack.transform.position -= Vector3.forward * 0.5f;
			_camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
			shadow.transform.parent = _cameraUi.transform;
			shadow.transform.localPosition = Vector3.zero;
			_camera.position = CameraPosition2;
			AfterStart2();
		}
		else
		{
			shadow.gameObject.SetActive(false);
		}
	}

	private void AfterStart()
	{
		LevelItem = Object.FindObjectsOfType<LevelItemController>();
		for (i = 0; i < LevelItem.Length; i++)
		{
			if (LevelItem[i].Model.Scene == typeof(QuietLawn_2_1))
			{
				LevelItemCurrent = LevelItem[i];
				btmCurrent = LevelItem[i].View.LevelButton;
			}
		}
		if (!(LevelItemCurrent != null) || LevelItemCurrent.Model.Status != LevelStatus.Unlocked)
		{
			return;
		}
		btnMission = Object.FindObjectsOfType<tk2dButton>();
		FX_ChosenItem = GameObject.Find("FX_ChosenItem(Clone)");
		for (i = 0; i < LevelItem.Length; i++)
		{
			if (LevelItem[i] != LevelItemCurrent)
			{
				LevelItem[i].enabled = false;
				LevelItem[i].transform.GetComponent<InputWindowView>().enabled = false;
			}
		}
		for (i = 0; i < btnMission.Length; i++)
		{
			if (btnMission[i].transform.parent.name != "btnBack")
			{
				btnMission[i].enabled = false;
			}
		}
		for (i = 0; i < btnMission.Length; i++)
		{
			if ((btnMission[i].targetObject != null && btnMission[i].targetObject.transform.parent != null && btnMission[i].targetObject.transform.parent.name == "InGame") || (btnMission[i].targetObject != null && btnMission[i].targetObject.name == "AnnouncementListItem(Clone)") || (btnMission[i].targetObject != null && btnMission[i].targetObject.name == "PopupConfirm") || (btnMission[i].targetObject != null && btnMission[i].targetObject.name == "PopupAdFeedback") || (btnMission[i].targetObject != null && btnMission[i].targetObject.name == "PopupAreYouSure"))
			{
				btnMission[i].enabled = true;
			}
		}
		btmCurrent.enabled = true;
		btmCurrent.targetObject = gameObject;
		btmCurrent.messageName = "BtmClik";
		if (FX_ChosenItem != null)
		{
			FX_ChosenItem.transform.position = btmCurrent.transform.position;
		}
	}

	private void AfterStart0()
	{
		LevelItem = Object.FindObjectsOfType<LevelItemController>();
		for (i = 0; i < LevelItem.Length; i++)
		{
			if (LevelItem[i].Model.Scene == typeof(QuietLawn_2_2))
			{
				LevelItemCurrent = LevelItem[i];
				btmCurrent = LevelItem[i].View.LevelButton;
			}
		}
		if (!(LevelItemCurrent != null) || LevelItemCurrent.Model.Status != LevelStatus.Unlocked)
		{
			return;
		}
		btnMission = Object.FindObjectsOfType<tk2dButton>();
		FX_ChosenItem = GameObject.Find("FX_ChosenItem(Clone)");
		for (i = 0; i < LevelItem.Length; i++)
		{
			if (LevelItem[i] != LevelItemCurrent)
			{
				LevelItem[i].enabled = false;
				LevelItem[i].transform.GetComponent<InputWindowView>().enabled = false;
			}
		}
		for (i = 0; i < btnMission.Length; i++)
		{
			if (btnMission[i].transform.parent.name != "btnBack")
			{
				btnMission[i].enabled = false;
			}
		}
		for (i = 0; i < btnMission.Length; i++)
		{
			if ((btnMission[i].targetObject != null && btnMission[i].targetObject.transform.parent != null && btnMission[i].targetObject.transform.parent.name == "InGame") || (btnMission[i].targetObject != null && btnMission[i].targetObject.name == "AnnouncementListItem(Clone)") || (btnMission[i].targetObject != null && btnMission[i].targetObject.name == "PopupConfirm") || (btnMission[i].targetObject != null && btnMission[i].targetObject.name == "PopupAdFeedback") || (btnMission[i].targetObject != null && btnMission[i].targetObject.name == "PopupAreYouSure"))
			{
				btnMission[i].enabled = true;
			}
		}
		btmCurrent.enabled = true;
		btmCurrent.targetObject = gameObject;
		btmCurrent.messageName = "BtmClik";
		if (FX_ChosenItem != null)
		{
			FX_ChosenItem.transform.position = btmCurrent.transform.position;
		}
	}

	private void AfterStart1()
	{
		LevelItem = Object.FindObjectsOfType<LevelItemController>();
		for (i = 0; i < LevelItem.Length; i++)
		{
			if (LevelItem[i].Model.Scene == typeof(ScorchedSparksTutorial_1_1))
			{
				LevelItemCurrent = LevelItem[i];
				btmCurrent = LevelItem[i].View.LevelButton;
			}
		}
		if (!(LevelItemCurrent != null) || LevelItemCurrent.Model.Status != LevelStatus.Unlocked)
		{
			return;
		}
		btnMission = Object.FindObjectsOfType<tk2dButton>();
		FX_ChosenItem = GameObject.Find("FX_ChosenItem(Clone)");
		for (i = 0; i < LevelItem.Length; i++)
		{
			if (LevelItem[i] != LevelItemCurrent)
			{
				LevelItem[i].enabled = false;
				LevelItem[i].transform.GetComponent<InputWindowView>().enabled = false;
			}
		}
		for (i = 0; i < btnMission.Length; i++)
		{
			if (btnMission[i].transform.parent.name != "btnBack")
			{
				btnMission[i].enabled = false;
			}
		}
		for (i = 0; i < btnMission.Length; i++)
		{
			if ((btnMission[i].targetObject != null && btnMission[i].targetObject.transform.parent != null && btnMission[i].targetObject.transform.parent.name == "InGame") || (btnMission[i].targetObject != null && btnMission[i].targetObject.name == "AnnouncementListItem(Clone)") || (btnMission[i].targetObject != null && btnMission[i].targetObject.name == "PopupConfirm") || (btnMission[i].targetObject != null && btnMission[i].targetObject.name == "PopupAdFeedback") || (btnMission[i].targetObject != null && btnMission[i].targetObject.name == "PopupAreYouSure"))
			{
				btnMission[i].enabled = true;
			}
		}
		btmCurrent.enabled = true;
		btmCurrent.targetObject = gameObject;
		btmCurrent.messageName = "BtmClik";
		if (FX_ChosenItem != null)
		{
			FX_ChosenItem.transform.position = btmCurrent.transform.position;
		}
	}

	private void AfterStart2()
	{
		LevelItem = Object.FindObjectsOfType<LevelItemController>();
		for (i = 0; i < LevelItem.Length; i++)
		{
			if (LevelItem[i].Model.Scene == typeof(QuietLawn_2_3))
			{
				LevelItemCurrent = LevelItem[i];
				btmCurrent = LevelItem[i].View.LevelButton;
			}
		}
		if (!(LevelItemCurrent != null) || LevelItemCurrent.Model.Status != LevelStatus.Unlocked)
		{
			return;
		}
		btnMission = Object.FindObjectsOfType<tk2dButton>();
		FX_ChosenItem = GameObject.Find("FX_ChosenItem(Clone)");
		for (i = 0; i < LevelItem.Length; i++)
		{
			if (LevelItem[i] != LevelItemCurrent)
			{
				LevelItem[i].enabled = false;
				LevelItem[i].transform.GetComponent<InputWindowView>().enabled = false;
			}
		}
		for (i = 0; i < btnMission.Length; i++)
		{
			if (btnMission[i].transform.parent.name != "btnBack")
			{
				btnMission[i].enabled = false;
			}
		}
		for (i = 0; i < btnMission.Length; i++)
		{
			if ((btnMission[i].targetObject != null && btnMission[i].targetObject.transform.parent != null && btnMission[i].targetObject.transform.parent.name == "InGame") || (btnMission[i].targetObject != null && btnMission[i].targetObject.name == "AnnouncementListItem(Clone)") || (btnMission[i].targetObject != null && btnMission[i].targetObject.name == "PopupConfirm") || (btnMission[i].targetObject != null && btnMission[i].targetObject.name == "PopupAdFeedback") || (btnMission[i].targetObject != null && btnMission[i].targetObject.name == "PopupAreYouSure"))
			{
				btnMission[i].enabled = true;
			}
		}
		btmCurrent.enabled = true;
		btmCurrent.targetObject = gameObject;
		btmCurrent.messageName = "BtmClik";
		if (FX_ChosenItem != null)
		{
			FX_ChosenItem.transform.position = btmCurrent.transform.position;
		}
	}

	private void AfterStart3()
	{
		popupButtons = PopupMultiplayer.GetComponentsInChildren<tk2dButton>();
		popupMultiplayerAvailabilityView = PopupMultiplayer.GetComponentInChildren<PopupMultiplayerAvailabilityView>();
		bool flag = false;
		for (int i = 0; i < popupButtons.Length; i++)
		{
			if (popupButtons[i].messageName == "OnBtnMultiplayerClick")
			{
				flag = true;
			}
		}
		if (!flag || !(popupMultiplayerAvailabilityView != null))
		{
			return;
		}
		for (int j = 0; j < popupButtons.Length; j++)
		{
			if (popupButtons[j].messageName == "OnBtnMultiplayerClick")
			{
				popupMultiplayerAvailabilityView.LockBack = true;
				BtnMultiplayer = popupButtons[j].transform;
				StartCoroutine(ShowHand(0.8f));
			}
			else
			{
				popupButtons[j].enabled = false;
			}
		}
	}

	private IEnumerator Scale(float weit, float scale)
	{
		yield return new WaitForSeconds(weit);
		EquipText.transform.DOScale(Vector3.one * scale, 0.3f);
	}

	public void OnBtnBackpackClick()
	{
		hand2d.SetActive(false);
		Equip.transform.parent.parent.parent.SendMessage("OnBtnBackpackClick");
		PlayerPrefs.SetInt("Tutor_CutSceneMap", 1);
	}

	public void BtmClik()
	{
		missionTap = true;
		btmCurrent.enabled = false;
		hand3d.SetActive(false);
		hand2d.transform.position = Equip.transform.position + offset;
		hand2d.SetActive(true);
		indexShadow = 1;
		Equip.GetComponent<tk2dButton>().enabled = true;
		Equip.GetComponent<tk2dButton>().targetObject = gameObject;
		StartCoroutine(Scale(0.3f, 0.9f));
		StartCoroutine(Scale(0.6f, 1f));
		StartCoroutine(Scale(1.2f, 0.9f));
		StartCoroutine(Scale(1.5f, 1f));
		StartCoroutine(Scale(2.1f, 0.9f));
		StartCoroutine(Scale(2.4f, 1f));
		StartCoroutine(Scale(3f, 0.9f));
		StartCoroutine(Scale(3.3f, 1f));
		StartCoroutine(Scale(3.9f, 0.9f));
		StartCoroutine(Scale(4.2f, 1f));
	}

	private IEnumerator ShowHand(float weit)
	{
		yield return new WaitForSeconds(weit);
		hand2dFlip.transform.position = BtnMultiplayer.position + offsetflip;
		hand2dFlip.SetActive(true);
	}

	private void LateUpdate()
	{
		if (!AfterStart3Lock && !TutorialMapSceneComplete3 && PopupMultiplayer.activeSelf)
		{
			AfterStart3Lock = true;
			AfterStart3();
		}
		if (AdFeedBack.activeSelf || PopupConfirm.activeSelf)
		{
			if (hand2d.GetComponent<MeshRenderer>().enabled)
			{
				hand2d.GetComponent<MeshRenderer>().enabled = false;
			}
		}
		else if (!hand2d.GetComponent<MeshRenderer>().enabled)
		{
			hand2d.GetComponent<MeshRenderer>().enabled = true;
		}
		missionWeit += Time.deltaTime;
		if (btmCurrent != null && !missionTap && missionWeit > 0.5f)
		{
			missionTap = true;
			if (help != null)
			{
				MonoSingleton<Localization>.Instance.LocalizeTextMesh(help, keyHelp, string.Empty);
			}
			hand3d.transform.parent = btmCurrent.transform.parent.transform;
			if (fix_2_2)
			{
				offsetbtnMission.x = 1f;
				offsetbtnMission.y = 0.62f;
				offsetbtnMission.z = -0.03f;
			}
			hand3d.transform.localPosition = offsetbtnMission;
			hand3d.transform.localEulerAngles = Vector3.zero;
			if (fix_2_2)
			{
				hand3d.transform.localEulerAngles = Vector3.forward * 88.6f;
			}
			else
			{
				hand3d.transform.localEulerAngles = Vector3.forward * 54f;
			}
			hand3d.SetActive(true);
		}
		if (indexShadow == 1 && black.a < 0.8f)
		{
			black.a += 0.05f;
			shadow.material.color = black;
		}
		if (indexShadow == 2 && black.a > 0f)
		{
			black.a -= 0.05f;
			shadow.material.color = black;
		}
	}
}

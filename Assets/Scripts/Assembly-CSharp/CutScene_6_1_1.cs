using System.Collections;
using AppsMinistry.Core;
using AppsMinistry.Core.Input;
using AppsMinistry.UI.Camera;
using DG.Tweening;
using Slugterra.Game.Units;
using Slugterra.Game.Units.Ally;
using Slugterra.Game.Units.Enemy;
using Slugterra.Scene;
using Slugterra.UI.Game;
using Slugterra.UI.Game.Camera;
using UnityEngine;

public class CutScene_6_1_1 : SlugterraCutScene
{
	[SerializeField]
	private GameController gameController;

	public GameObject HUDPanel;

	private GameObject MainCamera;

	private GameObject UICamera;

	private CameraFade _cameraFade;

	public GameObject _fade;

	private EliController _eliController;

	private CameraSmoothFollow _cameraSmoothFollow;

	public string text;

	public GameObject dialogueWindow;

	[SerializeField]
	private Transform endElliPoint;

	[SerializeField]
	private GameObject trainSwitchTarget;

	[SerializeField]
	private Transform trainSwitch;

	private bool updateTrigger = true;

	public Transform train1Platforms;

	public Transform train2;

	public GameObject mobGroupRun;

	public GameObject[] mobsRun;

	public Transform[] mobsRunEndPoint;

	public GameObject EnemyGroup_AB_1;

	public GameObject EnemyGroup_AB_2;

	public GameObject EnemyGroup_AB_3;

	public GameObject EnemyGroup_AB_4;

	private void Start()
	{
		_eliController = Object.FindObjectOfType<EliController>();
		if (_eliController.Model.Health > 0f)
		{
			StartCoroutine(StartDelay());
		}
	}

	private IEnumerator StartDelay()
	{
		yield return new WaitForSeconds(1f);
		gameController.PlayCutScene(true);
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		MainCamera = GameObject.Find("MainCamera");
		UICamera = GameObject.Find("UICamera");
		UICamera.GetComponent<Camera>().enabled = false;
		HUDPanel.SetActive(false);
		_eliController = Object.FindObjectOfType<EliController>();
		_cutSceneView.SkipButton.targetObject = gameObject;
		_cameraFade = MainCamera.GetComponent<CameraFade>();
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 1f), 0.25f, 0.5f, 0.5f);
		_cameraSmoothFollow = MainCamera.GetComponent<CameraSmoothFollow>();
		_cameraSmoothFollow.enabled = false;
		dialogueWindow.SetActive(false);
		mobGroupRun.SetActive(true);
		StartCoroutine(StartCameraTrack());
	}

	private IEnumerator StartCameraTrack()
	{
		yield return new WaitForSeconds(0.7f);
		GameObject[] allObjects = (GameObject[])Object.FindObjectsOfType(typeof(GameObject));
		GameObject[] array = allObjects;
		foreach (GameObject gameObject in array)
		{
			if (gameObject.name == "prfCoins(Clone)" || gameObject.name == "prfCoins_5(Clone)" || gameObject.name == "prfCoins_15(Clone)" || gameObject.name == "prfCoins_25(Clone)" || gameObject.name == "prfCoins_50(Clone)" || gameObject.name == "ScrapHeap_Mob_Sys")
			{
				Object.Destroy(gameObject);
			}
		}
		for (int j = 0; j < mobsRun.Length; j++)
		{
			if (mobsRun[j] != null)
			{
				mobsRun[j].GetComponent<SentinelController>().MoveTo(mobsRunEndPoint[j].position, 5f);
			}
		}
		MainCamera.GetComponent<Animation>().Play("CameraTrack_2");
		StartCoroutine(FadeMiddle());
		StartCoroutine(TextHint_0());
	}

	private IEnumerator TextHint_0()
	{
		yield return new WaitForSeconds(0.1f);
		_cutSceneView.SkipButton.gameObject.SetActive(true);
		_cutSceneView.ShowScreens(true);
		_cutSceneView.SpeakerIcon = "icon_eli";
		_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance[text];
		_isSkipEnable = true;
		_eliController.MoveTarget.position = endElliPoint.position;
		_eliController.OnMove(1f);
		_eliController.transform.position = endElliPoint.position;
		StartCoroutine(HideTextHint_0());
	}

	private IEnumerator HideTextHint_0()
	{
		yield return new WaitForSeconds(10f);
		if (_isSkipEnable)
		{
			ShowScreens(false);
			_isSkipEnable = false;
			_cutSceneView.SkipButton.gameObject.SetActive(false);
			Fade();
		}
	}

	private IEnumerator FadeMiddle()
	{
		yield return new WaitForSeconds(3.8f);
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 0f), 0.1f, 0.1f, 0.1f);
		_eliController.transform.eulerAngles = endElliPoint.eulerAngles;
		EnemyGroup_AB_1.SetActive(false);
		EnemyGroup_AB_2.SetActive(false);
		EnemyGroup_AB_3.SetActive(false);
		EnemyGroup_AB_4.SetActive(false);
		for (int i = 0; i < mobsRun.Length; i++)
		{
			if (mobsRun[i] != null)
			{
				mobsRun[i].GetComponent<SentinelController>().MoveTo(mobsRunEndPoint[i].position, 9.5f);
			}
		}
		StartCoroutine(Attack());
	}

	private IEnumerator Attack()
	{
		yield return new WaitForSeconds(1.2f);
		_eliController.AttackTarget = trainSwitchTarget.transform;
	}

	private void FixedUpdate()
	{
		if (!trainSwitchTarget.activeSelf && updateTrigger)
		{
			trainSwitch.transform.DOLocalRotate(new Vector3(-25f, 0f, 0f), 0.5f);
			StartCoroutine(TrainsGoBack());
			updateTrigger = false;
		}
	}

	private IEnumerator TrainsGoBack()
	{
		yield return new WaitForSeconds(0.5f);
		for (int i = 0; i < mobsRun.Length; i++)
		{
			if (mobsRun[i] != null)
			{
				mobsRun[i].GetComponent<SentinelController>().MyBehavior.CanMove = false;
				mobsRun[i].GetComponent<UnitPathFinder>().enabled = false;
				mobsRun[i].transform.eulerAngles = mobsRunEndPoint[i].eulerAngles;
				mobsRun[i].GetComponent<SentinelController>().AttackTarget = _eliController.transform;
			}
		}
		train1Platforms.DOLocalMoveZ(80f, 20f);
		train2.DOLocalMoveZ(80f, 20f);
	}

	private void Fade()
	{
		ShowScreens(false);
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 0f), 1f, 0.5f, 2f);
		_cutSceneView.SkipButton.gameObject.SetActive(false);
		_isSkipEnable = false;
		StartCoroutine(End());
	}

	private IEnumerator End()
	{
		yield return new WaitForSeconds(0.5f);
		UICamera.GetComponent<Camera>().enabled = true;
		HUDPanel.SetActive(true);
		gameController.OnFinishGame();
		RisePlayCutSceneEvent(false);
		gameObject.SetActive(false);
	}
}

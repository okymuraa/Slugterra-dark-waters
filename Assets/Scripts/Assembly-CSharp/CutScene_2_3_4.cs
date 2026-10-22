using System.Collections;
using AppsMinistry.Core;
using AppsMinistry.Core.Input;
using AppsMinistry.UI.Camera;
using Slugterra.Game.Units.Ally;
using Slugterra.Game.Units.Ally.Assistant;
using Slugterra.Game.Units.Enemy;
using Slugterra.Scene;
using Slugterra.UI.Game;
using Slugterra.UI.Game.Camera;
using UnityEngine;

public class CutScene_2_3_4 : SlugterraCutScene
{
	[SerializeField]
	private GameController gameController;

	[SerializeField]
	private GameObject MoveTargetEli;

	[SerializeField]
	private GameObject MoveTargetAlly;

	[SerializeField]
	private Transform EliGoTo_AB_3;

	[SerializeField]
	private Transform EliGoTo_AB_3_1;

	[SerializeField]
	private Transform AllyGoTo_AB_3;

	[SerializeField]
	private GameObject ScrapHeap_1;

	[SerializeField]
	private Transform ScrapHeap_1_1;

	[SerializeField]
	private Transform ScrapHeap_1_2;

	[SerializeField]
	private GameObject ScrapHeap_2;

	[SerializeField]
	private Transform ScrapHeap_2_1;

	[SerializeField]
	private Transform ScrapHeap_2_2;

	[SerializeField]
	private GameObject Billy;

	[SerializeField]
	private Transform Billy_1;

	[SerializeField]
	private Transform Billy_2;

	[SerializeField]
	private GameObject billy2;

	[SerializeField]
	private GameObject enemyGroup_AB_4;

	private GameObject MainCamera;

	private GameObject UICamera;

	private EliController _eliController;

	private AssistantController _assistantController;

	private CameraFade _cameraFade;

	private CameraSmoothFollow _cameraSmoothFollow;

	private Animation _camerAnimation;

	private BillyController _billyController;

	private ScrapHeapController _scrapHeapController1;

	private ScrapHeapController _scrapHeapController2;

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
		yield return new WaitForSeconds(3f);
		RisePlayCutSceneEvent(true);
		MainCamera = GameObject.Find("MainCamera");
		UICamera = GameObject.Find("UICamera");
		_cameraFade = MainCamera.GetComponent<CameraFade>();
		_cameraSmoothFollow = MainCamera.GetComponent<CameraSmoothFollow>();
		_camerAnimation = MainCamera.GetComponent<Animation>();
		_assistantController = Object.FindObjectOfType<AssistantController>();
		_billyController = Billy.GetComponent<BillyController>();
		_scrapHeapController1 = ScrapHeap_1.GetComponentInChildren<ScrapHeapController>();
		_scrapHeapController2 = ScrapHeap_2.GetComponentInChildren<ScrapHeapController>();
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 1f), 1f, 0.1f, 2f);
		_cutSceneView.SkipButton.targetObject = gameObject;
		gameController.PlayCutScene(true);
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		billy2.SetActive(false);
		float time = 3f;
		StartCoroutine(TextHint_9(time));
		float _time = 1f;
		StartCoroutine(StartCameraTrack(_time));
	}

	private IEnumerator DelaySkip(float a)
	{
		yield return new WaitForSeconds(a);
		Skip();
	}

	private void Skip()
	{
		_camerAnimation.Stop();
		_cameraSmoothFollow.enabled = true;
		UICamera.SetActive(true);
		ShowScreens(false);
		DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
		gameController.PlayCutScene(false);
		RisePlayCutSceneEvent(false);
		enemyGroup_AB_4.SetActive(true);
		float a = 6f;
		StartCoroutine(CutSceneTurnOff(a));
	}

	private IEnumerator StartCameraTrack(float a)
	{
		yield return new WaitForSeconds(a);
		ProntoEliGoTo();
		UICamera.SetActive(false);
		_cutSceneView.SkipButton.targetObject = gameObject;
		_cameraSmoothFollow.enabled = false;
		_camerAnimation.Play("CameraTrack_4");
	}

	private IEnumerator TextHint_9(float a)
	{
		yield return new WaitForSeconds(a);
		_cutSceneView.SpeakerIcon = "icon_eli";
		ShowScreens(true);
		_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["2_5_cutscene_4_text_01"];
		float _time = 4f;
		StartCoroutine(HideTextHint_9(_time));
		StartCoroutine(EnemySetActive());
		StartCoroutine(BillyGroupGoTo());
		_isSkipEnable = true;
		_cutSceneView.SkipButton.gameObject.SetActive(true);
	}

	private IEnumerator HideTextHint_9(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			float a2 = 1f;
			StartCoroutine(TextHint_10(a2));
		}
	}

	private IEnumerator TextHint_10(float a)
	{
		yield return new WaitForSeconds(a);
		float time = 0.1f;
		StartCoroutine(RotateEly(time, EliGoTo_AB_3.eulerAngles));
		if (_isSkipEnable)
		{
			_cutSceneView.SpeakerIcon = "icon_billy";
			_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["2_5_cutscene_4_text_02"];
			time = 4f;
			StartCoroutine(HideTextHint_10(time));
		}
	}

	private IEnumerator HideTextHint_10(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			float a2 = 1f;
			StartCoroutine(TextHint_11(a2));
		}
	}

	private IEnumerator TextHint_11(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["2_5_cutscene_4_text_03"];
			float a2 = 4f;
			StartCoroutine(HideTextHint_11(a2));
			StartCoroutine(BillyGroupGoToOnEnd(a2));
			StartCoroutine(BillyGoToOnEnd(a2));
		}
	}

	private IEnumerator HideTextHint_11(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			ShowScreens(false);
			Fade();
		}
	}

	private void Fade()
	{
		_isSkipEnable = false;
		if (!Billy.activeSelf)
		{
			StartCoroutine(EnemySetActive());
		}
		float a = 0.5f;
		StartCoroutine(BillyGroupGoToOnEnd(a));
		StartCoroutine(BillyGoToOnEnd(a));
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 1f), 1f, 1f, 1f);
		_cutSceneView.SkipButton.gameObject.SetActive(false);
		_cutSceneView.SkipButton.targetObject = null;
		float a2 = 1f;
		StartCoroutine(DelaySkip(a2));
	}

	private IEnumerator BillyGroupGoTo()
	{
		yield return new WaitForSeconds(0.2f);
		_billyController.MoveTarget.position = Billy_1.position;
		_scrapHeapController1.MoveTo(ScrapHeap_1_1, 7f);
		_scrapHeapController2.MoveTo(ScrapHeap_2_1, 7f);
		_billyController.OnMove(7f);
	}

	private IEnumerator BillyGoToOnEnd(float a)
	{
		yield return new WaitForSeconds(a);
		_billyController.MoveTarget.position = Billy_2.position;
		_billyController.OnMove(5f);
	}

	private IEnumerator BillyGroupGoToOnEnd(float a)
	{
		yield return new WaitForSeconds(a);
		_scrapHeapController1.MoveTo(ScrapHeap_1_2, 3f);
		_scrapHeapController2.MoveTo(ScrapHeap_2_2, 3f);
	}

	private void ProntoEliGoTo()
	{
		_eliController.MoveTarget.transform.position = EliGoTo_AB_3.position;
		_eliController.OnMove(0f);
		_eliController.transform.position = EliGoTo_AB_3.position;
		float a = 0.1f;
		StartCoroutine(RotateEly(a, EliGoTo_AB_3_1.eulerAngles));
		if (_assistantController != null && !(_assistantController.StateMachine.CurrentState is Slugterra.Game.Units.Ally.Assistant.DeathState))
		{
			_assistantController.MoveTarget.position = AllyGoTo_AB_3.transform.position;
			_assistantController.transform.position = AllyGoTo_AB_3.position;
		}
	}

	private IEnumerator RotateEly(float a, Vector3 angle)
	{
		yield return new WaitForSeconds(a);
		_eliController.transform.eulerAngles = angle;
	}

	private IEnumerator EnemySetActive()
	{
		yield return new WaitForSeconds(0.1f);
		ScrapHeap_1.SetActive(true);
		ScrapHeap_2.SetActive(true);
		Billy.SetActive(true);
	}

	private IEnumerator CutSceneTurnOff(float a)
	{
		_scrapHeapController1.SetViewRadius(15f);
		_scrapHeapController2.SetViewRadius(15f);
		yield return new WaitForSeconds(a);
		Billy.SetActive(false);
		gameObject.SetActive(false);
	}
}

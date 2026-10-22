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

public class CutScene_2_3_2 : SlugterraCutScene
{
	[SerializeField]
	private GameController gameController;

	[SerializeField]
	private GameObject MoveTargetEli;

	[SerializeField]
	private GameObject MoveTargetAlly;

	[SerializeField]
	private Transform EliGoTo_AB_1;

	[SerializeField]
	private Transform EliGoTo_AB_1_1;

	[SerializeField]
	private Transform AllyGoTo_AB_1;

	[SerializeField]
	private GameObject ScrapHeap_1;

	[SerializeField]
	private Transform ScrapHeap_1_1;

	[SerializeField]
	private Transform ScrapHeap_1_1a;

	[SerializeField]
	private Transform ScrapHeap_1_2;

	[SerializeField]
	private GameObject ScrapHeap_2;

	[SerializeField]
	private Transform ScrapHeap_2_1;

	[SerializeField]
	private Transform ScrapHeap_2_1a;

	[SerializeField]
	private Transform ScrapHeap_2_2;

	[SerializeField]
	private GameObject Shorty;

	[SerializeField]
	private Transform Shorty_1;

	[SerializeField]
	private Transform Shorty_2;

	[SerializeField]
	private GameObject EnemyGroup_AB_2;

	private GameObject MainCamera;

	private GameObject UICamera;

	private EliController _eliController;

	private AssistantController _assistantController;

	private CameraFade _cameraFade;

	private CameraSmoothFollow _cameraSmoothFollow;

	private Animation _camerAnimation;

	private ShortyController _shortyController;

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
		yield return new WaitForSeconds(2f);
		RisePlayCutSceneEvent(true);
		MainCamera = GameObject.Find("MainCamera");
		UICamera = GameObject.Find("UICamera");
		_cameraFade = MainCamera.GetComponent<CameraFade>();
		_cameraSmoothFollow = MainCamera.GetComponent<CameraSmoothFollow>();
		_camerAnimation = MainCamera.GetComponent<Animation>();
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 1f), 1f, 0.1f, 1.5f);
		_cutSceneView.SkipButton.targetObject = gameObject;
		gameController.PlayCutScene(true);
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		_assistantController = Object.FindObjectOfType<AssistantController>();
		_shortyController = Shorty.GetComponent<ShortyController>();
		_scrapHeapController1 = ScrapHeap_1.GetComponentInChildren<ScrapHeapController>();
		_scrapHeapController2 = ScrapHeap_2.GetComponentInChildren<ScrapHeapController>();
		float time = 3f;
		StartCoroutine(TextHint_3(time));
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
		_scrapHeapController1.transform.position = ScrapHeap_1_1a.position;
		_scrapHeapController2.transform.position = ScrapHeap_2_1a.position;
		StartCoroutine(ShortyGroupGoToOnEnd(0f, 7f));
		StartCoroutine(ShortyGoToOnEnd(0f));
		_camerAnimation.Stop();
		_cameraSmoothFollow.enabled = true;
		UICamera.SetActive(true);
		ShowScreens(false);
		DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
		gameController.PlayCutScene(false);
		RisePlayCutSceneEvent(false);
		EnemyGroup_AB_2.SetActive(true);
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
		_camerAnimation.Play("CameraTrack_2");
	}

	private IEnumerator TextHint_3(float a)
	{
		yield return new WaitForSeconds(a);
		ShowScreens(true);
		_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["2_5_cutscene_2_text_01"];
		float _time = 4f;
		StartCoroutine(HideTextHint_3(_time));
		_isSkipEnable = true;
		_cutSceneView.SkipButton.gameObject.SetActive(true);
	}

	private IEnumerator HideTextHint_3(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			float a2 = 1f;
			StartCoroutine(TextHint_4(a2));
		}
	}

	private IEnumerator TextHint_4(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["2_5_cutscene_2_text_02"];
			StartCoroutine(EnemySetActive());
			StartCoroutine(ShortyGroupGoTo());
			float a2 = 8f;
			StartCoroutine(ShortyGroupGoToOnEnd(a2, 7f));
			float a3 = 4f;
			StartCoroutine(HideTextHint_4(a3));
		}
	}

	private IEnumerator HideTextHint_4(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			float a2 = 1f;
			StartCoroutine(TextHint_5(a2));
		}
	}

	private IEnumerator TextHint_5(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			_cutSceneView.SpeakerIcon = "icon_shorty";
			_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["2_5_cutscene_2_text_03"];
			float a2 = 4f;
			StartCoroutine(HideTextHint_5(a2));
			float a3 = 6f;
			StartCoroutine(ShortyGoToOnEnd(a3));
		}
	}

	private IEnumerator HideTextHint_5(float a)
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
		if (!Shorty.activeSelf)
		{
			StartCoroutine(EnemySetActive());
		}
		float a = 0.5f;
		StartCoroutine(ShortyGroupGoToOnEnd(a, 7f));
		StartCoroutine(ShortyGoToOnEnd(a));
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 1f), 1f, 1f, 1f);
		_cutSceneView.SkipButton.gameObject.SetActive(false);
		_cutSceneView.SkipButton.targetObject = null;
		float a2 = 1f;
		StartCoroutine(DelaySkip(a2));
	}

	private IEnumerator ShortyGroupGoTo()
	{
		yield return new WaitForSeconds(1f);
		_shortyController.MoveTarget.position = Shorty_1.position;
		_scrapHeapController1.MoveTo(ScrapHeap_1_1, 7f);
		_scrapHeapController2.MoveTo(ScrapHeap_2_1, 7f);
		_shortyController.OnMove(7f);
	}

	private IEnumerator ShortyGoToOnEnd(float a)
	{
		yield return new WaitForSeconds(a);
		_shortyController.MoveTarget.position = Shorty_2.position;
		_shortyController.OnMove(5f);
	}

	private IEnumerator ShortyGroupGoToOnEnd(float a, float b)
	{
		yield return new WaitForSeconds(a);
		_scrapHeapController1.MoveTo(ScrapHeap_1_2, b);
		_scrapHeapController2.MoveTo(ScrapHeap_2_2, b);
	}

	private void ProntoEliGoTo()
	{
		_eliController.MoveTarget.transform.position = EliGoTo_AB_1.position;
		_eliController.OnMove(0f);
		_eliController.transform.position = EliGoTo_AB_1.position;
		float num = 0.1f;
		StartCoroutine(RotateEly(num, EliGoTo_AB_1_1.eulerAngles));
		StartCoroutine(RotateEly(num + 5f, EliGoTo_AB_1.eulerAngles));
		if (_assistantController != null && !(_assistantController.StateMachine.CurrentState is Slugterra.Game.Units.Ally.Assistant.DeathState))
		{
			_assistantController.MoveTarget.position = AllyGoTo_AB_1.transform.position;
			_assistantController.transform.position = AllyGoTo_AB_1.position;
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
		Shorty.SetActive(true);
	}

	private IEnumerator CutSceneTurnOff(float a)
	{
		_scrapHeapController1.SetViewRadius(15f);
		_scrapHeapController2.SetViewRadius(15f);
		yield return new WaitForSeconds(a);
		Shorty.SetActive(false);
		gameObject.SetActive(false);
	}
}

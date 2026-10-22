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

public class CutScene_2_3_3 : SlugterraCutScene
{
	[SerializeField]
	private GameController gameController;

	[SerializeField]
	private GameObject MoveTargetEli;

	[SerializeField]
	private GameObject MoveTargetAlly;

	[SerializeField]
	private Transform EliGoTo_AB_2;

	[SerializeField]
	private Transform EliGoTo_AB_2_1;

	[SerializeField]
	private Transform AllyGoTo_AB_2;

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
	private GameObject Glasses;

	[SerializeField]
	private Transform Glasses_1;

	[SerializeField]
	private Transform Glasses_2;

	[SerializeField]
	private GameObject enemyGroup_AB_3;

	private GameObject MainCamera;

	private GameObject UICamera;

	private EliController _eliController;

	private AssistantController _assistantController;

	private CameraFade _cameraFade;

	private CameraSmoothFollow _cameraSmoothFollow;

	private Animation _camerAnimation;

	private GlassesController _glassesController;

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
		_glassesController = Glasses.GetComponent<GlassesController>();
		_scrapHeapController1 = ScrapHeap_1.GetComponentInChildren<ScrapHeapController>();
		_scrapHeapController2 = ScrapHeap_2.GetComponentInChildren<ScrapHeapController>();
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 1f), 1f, 0.1f, 1.5f);
		_cutSceneView.SkipButton.targetObject = gameObject;
		gameController.PlayCutScene(true);
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		float time = 3f;
		StartCoroutine(TextHint_6(time));
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
		enemyGroup_AB_3.SetActive(true);
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
		_camerAnimation.Play("CameraTrack_3");
	}

	private IEnumerator TextHint_6(float a)
	{
		yield return new WaitForSeconds(a);
		_cutSceneView.SpeakerIcon = "icon_eli";
		ShowScreens(true);
		_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["2_5_cutscene_3_text_01"];
		float time = 4f;
		StartCoroutine(HideTextHint_6(time));
		_isSkipEnable = true;
		_cutSceneView.SkipButton.gameObject.SetActive(true);
	}

	private IEnumerator HideTextHint_6(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			float a2 = 1f;
			StartCoroutine(TextHint_7(a2));
		}
	}

	private IEnumerator TextHint_7(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			_cutSceneView.SpeakerIcon = "icon_eli";
			_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["2_5_cutscene_3_text_02"];
			StartCoroutine(EnemySetActive());
			StartCoroutine(GlassesGroupGoTo());
			float a2 = 12f;
			StartCoroutine(GlassesGroupGoToOnEnd(a2));
			a2 = 4f;
			StartCoroutine(HideTextHint_7(a2));
		}
	}

	private IEnumerator HideTextHint_7(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			float a2 = 1f;
			StartCoroutine(TextHint_8(a2));
		}
	}

	private IEnumerator TextHint_8(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			_cutSceneView.SpeakerIcon = "icon_glasses";
			_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["2_5_cutscene_3_text_03"];
			float a2 = 4f;
			StartCoroutine(HideTextHint_8(a2));
			StartCoroutine(GlassesGoToOnEnd(a2));
			StartCoroutine(GlassesGroupGoToOnEnd(a2));
		}
	}

	private IEnumerator HideTextHint_8(float a)
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
		if (!Glasses.activeSelf)
		{
			StartCoroutine(EnemySetActive());
		}
		float a = 0.5f;
		StartCoroutine(GlassesGroupGoToOnEnd(a));
		StartCoroutine(GlassesGoToOnEnd(a));
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 1f), 1f, 1f, 1f);
		_cutSceneView.SkipButton.gameObject.SetActive(false);
		_cutSceneView.SkipButton.targetObject = null;
		float a2 = 1f;
		StartCoroutine(DelaySkip(a2));
	}

	private IEnumerator GlassesGroupGoTo()
	{
		yield return new WaitForSeconds(0.2f);
		_glassesController.MoveTarget.position = Glasses_1.position;
		_scrapHeapController1.MoveTo(ScrapHeap_1_1, 7f);
		_scrapHeapController2.MoveTo(ScrapHeap_2_1, 7f);
		_glassesController.OnMove(7f);
	}

	private IEnumerator GlassesGoToOnEnd(float a)
	{
		yield return new WaitForSeconds(a);
		_glassesController.MoveTarget.position = Glasses_2.position;
		_glassesController.OnMove(6f);
	}

	private IEnumerator GlassesGroupGoToOnEnd(float a)
	{
		yield return new WaitForSeconds(a);
		_scrapHeapController1.MoveTo(ScrapHeap_1_2, 5f);
		_scrapHeapController2.MoveTo(ScrapHeap_2_2, 5f);
	}

	private void ProntoEliGoTo()
	{
		_eliController.MoveTarget.transform.position = EliGoTo_AB_2.position;
		_eliController.OnMove(0f);
		_eliController.transform.position = EliGoTo_AB_2.position;
		float num = 0.1f;
		StartCoroutine(RotateEly(num, EliGoTo_AB_2_1.eulerAngles));
		StartCoroutine(RotateEly(num + 6.2f, EliGoTo_AB_2.eulerAngles));
		if (_assistantController != null && !(_assistantController.StateMachine.CurrentState is Slugterra.Game.Units.Ally.Assistant.DeathState))
		{
			_assistantController.MoveTarget.position = AllyGoTo_AB_2.transform.position;
			_assistantController.transform.position = AllyGoTo_AB_2.position;
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
		Glasses.SetActive(true);
	}

	private IEnumerator CutSceneTurnOff(float a)
	{
		_scrapHeapController1.SetViewRadius(15f);
		_scrapHeapController2.SetViewRadius(15f);
		yield return new WaitForSeconds(a);
		Glasses.SetActive(false);
		gameObject.SetActive(false);
	}
}

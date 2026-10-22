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

public class CutScene_3_1_1 : SlugterraCutScene
{
	[SerializeField]
	private GameController gameController;

	[SerializeField]
	private Transform EliGoTo_AB_3;

	[SerializeField]
	private Transform AllyGoTo_AB_3;

	[SerializeField]
	private GameObject ScrapHeap_1;

	[SerializeField]
	private GameObject ScrapHeap_2;

	[SerializeField]
	private Transform ScrapHeapWP_1;

	[SerializeField]
	private Transform ScrapHeapWP_2;

	[SerializeField]
	private SphereCollider ScrapHeapView_1;

	[SerializeField]
	private SphereCollider ScrapHeapView_2;

	public float _time_StartCameraTrack = 1f;

	public float _time_textHint_3 = 0.5f;

	public float _time_HideTextHint_3 = 4f;

	public float _time_Fade = 1f;

	public float _time_DelaySkip = 1f;

	public float _time_CutSceneTurnOff = 1f;

	private GameObject MainCamera;

	private GameObject UICamera;

	private EliController _eliController;

	private AssistantController _assistantController;

	private CameraFade _cameraFade;

	private CameraSmoothFollow _cameraSmoothFollow;

	private Animation _camerAnimation;

	private ScrapHeapController _scrapHeapController1;

	private ScrapHeapController _scrapHeapController2;

	public GameObject _holder_AB34;

	private void Start()
	{
		RisePlayCutSceneEvent(true);
		MainCamera = GameObject.Find("MainCamera");
		UICamera = GameObject.Find("UICamera");
		_eliController = Object.FindObjectOfType<EliController>();
		_assistantController = Object.FindObjectOfType<AssistantController>();
		_cameraFade = MainCamera.GetComponent<CameraFade>();
		_cameraSmoothFollow = MainCamera.GetComponent<CameraSmoothFollow>();
		_camerAnimation = MainCamera.GetComponent<Animation>();
		_scrapHeapController1 = ScrapHeap_1.GetComponentInChildren<ScrapHeapController>();
		_scrapHeapController2 = ScrapHeap_2.GetComponentInChildren<ScrapHeapController>();
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 1f), 1f, 0.1f, 2f);
		_cutSceneView.SkipButton.targetObject = gameObject;
		gameController.PlayCutScene(true);
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		StartCoroutine(TextHint_3(_time_textHint_3));
		StartCoroutine(StartCameraTrack(_time_StartCameraTrack));
	}

	private IEnumerator DelaySkip(float a)
	{
		yield return new WaitForSeconds(a);
		Skip();
	}

	private void Skip()
	{
		_holder_AB34.SetActive(false);
		_camerAnimation.Stop();
		_cameraSmoothFollow.enabled = true;
		UICamera.SetActive(true);
		ShowScreens(false);
		DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
		gameController.PlayCutScene(false);
		RisePlayCutSceneEvent(false);
		StartCoroutine(CutSceneTurnOff(_time_CutSceneTurnOff));
	}

	private IEnumerator StartCameraTrack(float a)
	{
		yield return new WaitForSeconds(a);
		UICamera.SetActive(false);
		_cutSceneView.SkipButton.targetObject = gameObject;
		_cameraSmoothFollow.enabled = false;
		_camerAnimation.Play("CameraTrack_2");
		StartCoroutine(EnemySetActive());
		StartCoroutine(EnemyGroupGoTo());
	}

	private IEnumerator TextHint_3(float a)
	{
		yield return new WaitForSeconds(a);
		ProntoEliGoTo();
		_cutSceneView.SpeakerIcon = "icon_eli";
		ShowScreens(true);
		_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["3_1_cutscene_2_text_01"];
		StartCoroutine(HideTextHint_3(_time_HideTextHint_3));
		_isSkipEnable = true;
		_cutSceneView.SkipButton.gameObject.SetActive(true);
	}

	private IEnumerator HideTextHint_3(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			ShowScreens(false);
			yield return new WaitForSeconds(_time_Fade);
			Fade();
		}
	}

	private void Fade()
	{
		_isSkipEnable = false;
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 1f), 2f, 1f, 1f);
		_cutSceneView.SkipButton.gameObject.SetActive(false);
		_cutSceneView.SkipButton.targetObject = null;
		StartCoroutine(DelaySkip(_time_DelaySkip));
	}

	private IEnumerator EnemySetActive()
	{
		yield return new WaitForSeconds(0.1f);
		ScrapHeap_1.SetActive(true);
		ScrapHeap_2.SetActive(true);
		ScrapHeapView_1.radius = 1f;
		ScrapHeapView_2.radius = 1f;
	}

	private IEnumerator EnemyGroupGoTo()
	{
		yield return new WaitForSeconds(0.2f);
		_scrapHeapController1.MoveTo(ScrapHeapWP_1, 5f);
		_scrapHeapController2.MoveTo(ScrapHeapWP_2, 5f);
	}

	private void ProntoEliGoTo()
	{
		_eliController.MoveTarget.transform.position = EliGoTo_AB_3.position;
		_eliController.transform.position = EliGoTo_AB_3.position;
		float a = 1f;
		StartCoroutine(RotateEly(a));
		if (_assistantController != null && _assistantController.StateMachine.CurrentState is Slugterra.Game.Units.Ally.Assistant.DeathState)
		{
		}
	}

	private IEnumerator RotateEly(float a)
	{
		yield return new WaitForSeconds(a);
		_eliController.transform.eulerAngles = EliGoTo_AB_3.eulerAngles;
	}

	private IEnumerator CutSceneTurnOff(float a)
	{
		yield return new WaitForSeconds(a);
		ScrapHeap_1.SetActive(false);
		ScrapHeap_2.SetActive(false);
		gameObject.SetActive(false);
	}
}

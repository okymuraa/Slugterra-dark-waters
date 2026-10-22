using System.Collections;
using AppsMinistry.Core;
using AppsMinistry.Core.Input;
using AppsMinistry.UI.Camera;
using Slugterra.Game.Units.Ally;
using Slugterra.Game.Units.Ally.Assistant;
using Slugterra.Game.Units.Mechanisms.Spline;
using Slugterra.Game.Wave;
using Slugterra.Scene;
using Slugterra.UI.Game;
using Slugterra.UI.Game.Camera;
using UnityEngine;

public class CutScene_3_2_0 : SlugterraCutScene
{
	[SerializeField]
	private GameController gameController;

	[SerializeField]
	private GameObject startBlackPlane;

	public float _time_StartCameraTrack = 1f;

	public float _time_textHint_0 = 3f;

	public float _time_HideTextHint_0 = 3f;

	public float _time_textHint_1 = 3f;

	public float _time_HideTextHint_1 = 3f;

	public float _time_DelaySkip = 1f;

	[SerializeField]
	private Transform ElyGoTo_AB_0;

	[SerializeField]
	private Transform AllyGoTo_AB_0;

	[SerializeField]
	private MovePath OnStartPath;

	[SerializeField]
	private MovePath AB_1_Path;

	[SerializeField]
	private GameObject _comeBackObjective;

	private GameObject MainCamera;

	private GameObject UICamera;

	private EliController _eliController;

	private AssistantController _assistantController;

	private CameraFade _cameraFade;

	private CameraSmoothFollow _cameraSmoothFollow;

	private Animation _camerAnimation;

	private TeamController _trailer;

	private GameObject _allyArea;

	public GameObject stopTeam;

	private void Start()
	{
		RisePlayCutSceneEvent(true);
		MainCamera = GameObject.Find("MainCamera");
		UICamera = GameObject.Find("UICamera");
		_camerAnimation = MainCamera.GetComponent<Animation>();
		_cameraFade = MainCamera.GetComponent<CameraFade>();
		_cameraSmoothFollow = MainCamera.GetComponent<CameraSmoothFollow>();
		_eliController = Object.FindObjectOfType<EliController>();
		_trailer = Object.FindObjectOfType<TeamController>();
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 1f), 2f, 0.1f, 2f);
		_cutSceneView.SkipButton.targetObject = gameObject;
		gameController.PlayCutScene(true);
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		StartCoroutine(TextHint_0(_time_textHint_0));
		StartCoroutine(StartCameraTrack(_time_StartCameraTrack));
		_trailer.SetSpeed(0.45f, 0.9f, 3f);
		_trailer.OnMove();
	}

	private IEnumerator StartCameraTrack(float a)
	{
		yield return new WaitForSeconds(a);
		UICamera.SetActive(false);
		_cutSceneView.SkipButton.targetObject = gameObject;
		_cameraSmoothFollow.enabled = false;
		_camerAnimation.Play("CameraTrack_1");
		ElyGoToOnStart();
		startBlackPlane.SetActive(false);
	}

	private IEnumerator DelaySkip(float a)
	{
		yield return new WaitForSeconds(a);
		_cameraSmoothFollow.enabled = true;
		Skip();
	}

	private void Skip()
	{
		_camerAnimation.Stop();
		UICamera.SetActive(true);
		ShowScreens(false);
		_comeBackObjective.SetActive(true);
		stopTeam.SetActive(false);
		_trailer.SetSpeed(0.3f, 0.9f, 1.9f);
		_trailer.OnMove();
		_eliController.SetAttackTarget(null);
		DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
		gameController.PlayCutScene(false);
		RisePlayCutSceneEvent(false);
		gameObject.SetActive(false);
	}

	private IEnumerator TextHint_0(float a)
	{
		yield return new WaitForSeconds(a);
		ShowScreens(true);
		_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["3_2_cutscene_1_text_01"];
		StartCoroutine(HideTextHint_0(_time_HideTextHint_0));
		_isSkipEnable = true;
		_cutSceneView.SkipButton.gameObject.SetActive(true);
	}

	private IEnumerator HideTextHint_0(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			StartCoroutine(TextHint_1(_time_textHint_1));
		}
	}

	private IEnumerator TextHint_1(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["3_2_cutscene_1_text_02"];
			StartCoroutine(HideTextHint_1(_time_HideTextHint_1));
		}
	}

	private IEnumerator HideTextHint_1(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			ShowScreens(false);
			Fade();
		}
	}

	public void Fade()
	{
		_isSkipEnable = false;
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 1f), 1f, 1f, 1f);
		_cutSceneView.SkipButton.gameObject.SetActive(false);
		_cutSceneView.SkipButton.targetObject = null;
		StartCoroutine(DelaySkip(_time_DelaySkip));
	}

	private void ElyGoToOnStart()
	{
		_eliController.MoveTarget.position = ElyGoTo_AB_0.transform.position;
		_eliController.OnMove(8f);
	}
}

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

public class CutScene_3_1_2 : SlugterraCutScene
{
	[SerializeField]
	private GameController gameController;

	public GameObject Lock_Lode;

	[SerializeField]
	private SkinnedMeshRenderer Lock;

	[SerializeField]
	private SkinnedMeshRenderer Lode;

	[SerializeField]
	private LodeController _lodeController;

	[SerializeField]
	private LockController _lockController;

	public float _time_StartCameraTrack = 1f;

	public float _time_Txt_Cut_4 = 1f;

	public float _time_HideTxt_Cut_4 = 3f;

	public float _time_Txt_Cut_5 = 1f;

	public float _time_HideTxt_Cut_5 = 3f;

	public float _time_DelayFade = 1f;

	[SerializeField]
	private Transform EliGoTo_AB_4;

	[SerializeField]
	private Transform AllyGoTo_AB_4;

	[SerializeField]
	private Transform LockGoToOnStart;

	[SerializeField]
	private Transform LockGoToOnEnd;

	[SerializeField]
	private Transform LodeGoToOnStart;

	[SerializeField]
	private Transform LodeGoToOnEnd;

	private GameObject MainCamera;

	private GameObject UICamera;

	private GameObject cutSceneCamera;

	private EliController _eliController;

	private AssistantController _assistantController;

	private CameraFade _cameraFade;

	private CameraSmoothFollow _cameraSmoothFollow;

	private Animation _camerAnimation;

	public float _timeEndLevel;

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
		gameController.PlayCutScene(true);
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		RisePlayCutSceneEvent(true);
		MainCamera = GameObject.Find("MainCamera");
		UICamera = GameObject.Find("UICamera");
		cutSceneCamera = GameObject.Find("CameraCutSceneText");
		_assistantController = Object.FindObjectOfType<AssistantController>();
		_cameraFade = MainCamera.GetComponent<CameraFade>();
		_cameraSmoothFollow = MainCamera.GetComponent<CameraSmoothFollow>();
		_camerAnimation = MainCamera.GetComponent<Animation>();
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 1f), 1f, 0.1f, 1f);
		_cutSceneView.SkipButton.targetObject = gameObject;
		Lock_Lode.SetActive(true);
		Lock.enabled = true;
		Lode.enabled = true;
		_lockController.MoveTarget.position = LockGoToOnStart.transform.position;
		_lodeController.MoveTarget.position = LodeGoToOnStart.transform.position;
		_lodeController.OnMove(8f);
		_lockController.enabled = false;
		_lockController.transform.eulerAngles = LockGoToOnStart.eulerAngles;
		StartCoroutine(TextHint_4(_time_Txt_Cut_4));
		StartCoroutine(StartCameraTrack(_time_StartCameraTrack));
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
		cutSceneCamera.SetActive(false);
		UICamera.SetActive(true);
		_cameraFade.StopFade();
		ShowScreens(false);
		GoToOnEnd();
		gameController.OnFinishGame();
		DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
		gameController.PlayCutScene(false);
		RisePlayCutSceneEvent(false);
		gameObject.SetActive(false);
	}

	private IEnumerator StartCameraTrack(float a)
	{
		yield return new WaitForSeconds(a);
		UICamera.SetActive(false);
		_cutSceneView.SkipButton.targetObject = gameObject;
		_cameraSmoothFollow.enabled = false;
		_camerAnimation.Play("CameraTrack_3");
		GoToOnStart();
	}

	private IEnumerator TextHint_4(float a)
	{
		StartCoroutine(RotateEly(0.5f));
		yield return new WaitForSeconds(a);
		_lockController.transform.eulerAngles = LockGoToOnStart.eulerAngles;
		_cutSceneView.SpeakerIcon = "icon_lode";
		ShowScreens(true);
		_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["3_1_cutscene_3_text_01"];
		StartCoroutine(HideTextHint_4(_time_HideTxt_Cut_4));
		_isSkipEnable = true;
		_cutSceneView.SkipButton.gameObject.SetActive(true);
		float _time = 1f;
		StartCoroutine(RotateEly(_time));
	}

	private IEnumerator HideTextHint_4(float a)
	{
		StartCoroutine(RotateEly(0.1f));
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			StartCoroutine(TextHint_5(_time_Txt_Cut_5));
		}
	}

	private IEnumerator TextHint_5(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			_cutSceneView.SpeakerIcon = "icon_locke";
			_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["3_1_cutscene_3_text_02"];
			StartCoroutine(HideTextHint_5(_time_HideTxt_Cut_5));
		}
	}

	private IEnumerator HideTextHint_5(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			_cutSceneView.SkipButton.gameObject.SetActive(false);
			_cutSceneView.SkipButton.targetObject = null;
			ShowScreens(false);
			GoToOnEnd();
			StartCoroutine(DelayFade());
		}
	}

	private IEnumerator DelayFade()
	{
		yield return new WaitForSeconds(_time_DelayFade);
		Fade();
	}

	private void Fade()
	{
		_isSkipEnable = false;
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 1f), 1f, 1f, 0f);
		_cutSceneView.SkipButton.gameObject.SetActive(false);
		_cutSceneView.SkipButton.targetObject = null;
		float a = 2f;
		StartCoroutine(DelaySkip(a));
	}

	private void GoToOnStart()
	{
		_eliController.MoveTarget.transform.position = EliGoTo_AB_4.position;
		_eliController.transform.position = EliGoTo_AB_4.position;
		float a = 2f;
		StartCoroutine(RotateEly(a));
		if (_assistantController != null && !(_assistantController.StateMachine.CurrentState is Slugterra.Game.Units.Ally.Assistant.DeathState))
		{
			_assistantController.MoveTarget.position = AllyGoTo_AB_4.position;
			_assistantController.transform.position = AllyGoTo_AB_4.position;
			float a2 = 4f;
			StartCoroutine(RotateAlly(a2));
		}
	}

	private IEnumerator RotateEly(float a)
	{
		yield return new WaitForSeconds(a);
		_eliController.transform.eulerAngles = EliGoTo_AB_4.eulerAngles;
	}

	private IEnumerator RotateAlly(float a)
	{
		yield return new WaitForSeconds(a);
		_assistantController.transform.eulerAngles = AllyGoTo_AB_4.eulerAngles;
	}

	private void GoToOnEnd()
	{
		_lockController.MoveTarget.position = LockGoToOnEnd.transform.position;
		_lodeController.MoveTarget.position = LodeGoToOnEnd.transform.position;
		_lockController.OnMove(8f);
		_lodeController.OnMove(8f);
	}
}

using System.Collections;
using AppsMinistry.Core;
using AppsMinistry.Core.Input;
using AppsMinistry.UI.Camera;
using Slugterra.Game.Units.Ally;
using Slugterra.Game.Units.Ally.Assistant;
using Slugterra.Scene;
using Slugterra.UI.Game;
using Slugterra.UI.Game.Camera;
using UnityEngine;

public class CutScene_2_1_6 : SlugterraCutScene
{
	[SerializeField]
	private GameController gameController;

	[SerializeField]
	private Transform EliOnStart;

	[SerializeField]
	private Transform ProntoStart;

	private GameObject MainCamera;

	private GameObject UICamera;

	private GameObject cutSceneCamera;

	private EliController _eliController;

	private AssistantController _assistantController;

	private CameraFade _cameraFade;

	private CameraSmoothFollow _cameraSmoothFollow;

	private Animation _camerAnimation;

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
		yield return new WaitForSeconds(1.5f);
		RisePlayCutSceneEvent(true);
		MainCamera = GameObject.Find("MainCamera");
		UICamera = GameObject.Find("UICamera");
		cutSceneCamera = GameObject.Find("CameraCutSceneText");
		_assistantController = Object.FindObjectOfType<AssistantController>();
		_camerAnimation = MainCamera.GetComponent<Animation>();
		_cameraFade = MainCamera.GetComponent<CameraFade>();
		_cameraSmoothFollow = MainCamera.GetComponent<CameraSmoothFollow>();
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 1f), 1f, 0.1f, 2f);
		_cutSceneView.SkipButton.targetObject = gameObject;
		gameController.PlayCutScene(true);
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		float time = 3f;
		StartCoroutine(TextHint_13(time));
		float _time = 1f;
		StartCoroutine(StartCameraTrack(_time));
	}

	private IEnumerator StartCameraTrack(float a)
	{
		yield return new WaitForSeconds(a);
		UICamera.SetActive(false);
		_cutSceneView.SkipButton.targetObject = gameObject;
		_cameraSmoothFollow.enabled = false;
		_camerAnimation.Play("CameraTrack_6");
		EliProntoOnStart();
	}

	private IEnumerator DelaySkip(float a)
	{
		yield return new WaitForSeconds(a);
		Skip(null);
	}

	private void Skip(tk2dButton source)
	{
		_camerAnimation.Stop();
		_cameraSmoothFollow.enabled = true;
		cutSceneCamera.SetActive(false);
		UICamera.SetActive(true);
		_cameraFade.StopFade();
		gameController.OnFinishGame();
		DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
		gameController.PlayCutScene(false);
		RisePlayCutSceneEvent(false);
		gameObject.SetActive(false);
	}

	private IEnumerator TextHint_13(float a)
	{
		yield return new WaitForSeconds(a);
		_cutSceneView.SpeakerIcon = "icon_pronto";
		ShowScreens(true);
		_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["2_2_cutscene_2_text_01"];
		float time = 5f;
		StartCoroutine(HideTextHint_13(time));
		_isSkipEnable = true;
		_cutSceneView.SkipButton.gameObject.SetActive(true);
	}

	private IEnumerator HideTextHint_13(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			float a2 = 1f;
			StartCoroutine(TextHint_14(a2));
		}
	}

	private IEnumerator TextHint_14(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["2_2_cutscene_2_text_02"];
			float a2 = 5f;
			StartCoroutine(HideTextHint_14(a2));
		}
	}

	private IEnumerator HideTextHint_14(float a)
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
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 1f), 1f, 1f, 0f);
		_cutSceneView.SkipButton.gameObject.SetActive(false);
		_cutSceneView.SkipButton.targetObject = null;
		float a = 2f;
		StartCoroutine(DelaySkip(a));
	}

	private void EliProntoOnStart()
	{
		_eliController.MoveTarget.transform.position = EliOnStart.position;
		_eliController.transform.position = EliOnStart.position;
		_eliController.transform.eulerAngles = EliOnStart.eulerAngles;
		if (_assistantController != null && !(_assistantController.StateMachine.CurrentState is DeathState))
		{
			_assistantController.MoveTarget.transform.position = ProntoStart.position;
			_assistantController.transform.position = ProntoStart.position;
			_assistantController.transform.eulerAngles = ProntoStart.eulerAngles;
		}
	}
}

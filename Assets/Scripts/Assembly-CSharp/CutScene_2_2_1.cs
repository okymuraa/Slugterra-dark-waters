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

public class CutScene_2_2_1 : SlugterraCutScene
{
	[SerializeField]
	private GameController gameController;

	[SerializeField]
	private Transform ElyGoToOnEnd;

	[SerializeField]
	private Transform AllyGoToOnEnd;

	private GameObject MainCamera;

	private GameObject UICamera;

	private GameObject cutSceneCamera;

	private EliController _eliController;

	private AssistantController _assistantController;

	private CameraSmoothFollow _cameraSmoothFollow;

	private CameraFade _cameraFade;

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
		_cameraSmoothFollow = MainCamera.GetComponent<CameraSmoothFollow>();
		_cameraFade = MainCamera.GetComponent<CameraFade>();
		_camerAnimation = MainCamera.GetComponent<Animation>();
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 1f), 1f, 0.1f, 2f);
		_cutSceneView.SkipButton.targetObject = gameObject;
		gameController.PlayCutScene(true);
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		float time = 3f;
		StartCoroutine(TextHint_3(time));
		float _time = 1f;
		StartCoroutine(StartCameraTrack(_time));
	}

	private IEnumerator StartCameraTrack(float a)
	{
		yield return new WaitForSeconds(a);
		UICamera.SetActive(false);
		_cutSceneView.SkipButton.targetObject = gameObject;
		_cameraSmoothFollow.enabled = false;
		_camerAnimation.Play("CameraTrack_3");
		EliAllyGoTo();
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
		gameController.OnFinishGame();
		DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
		gameController.PlayCutScene(false);
		RisePlayCutSceneEvent(false);
		gameObject.SetActive(false);
	}

	private IEnumerator TextHint_3(float a)
	{
		yield return new WaitForSeconds(a);
		ShowScreens(true);
		_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["2_3_cutscene_2_text_01"];
		float time = 4f;
		StartCoroutine(HideTextHint_3(time));
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
			_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["2_3_cutscene_2_text_02"];
			float a2 = 4f;
			StartCoroutine(HideTextHint_4(a2));
		}
	}

	private IEnumerator HideTextHint_4(float a)
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

	private void EliAllyGoTo()
	{
		_eliController.MoveTarget.transform.position = ElyGoToOnEnd.position;
		_eliController.transform.position = ElyGoToOnEnd.position;
		float a = 4.5f;
		StartCoroutine(RotateEly(a));
		if (_assistantController != null && !(_assistantController.StateMachine.CurrentState is DeathState))
		{
			_assistantController.MoveTarget.transform.position = AllyGoToOnEnd.position;
			_assistantController.transform.position = AllyGoToOnEnd.position;
			float a2 = 4.5f;
			StartCoroutine(RotateAlly(a2));
		}
	}

	private IEnumerator RotateEly(float a)
	{
		yield return new WaitForSeconds(a);
		_eliController.transform.eulerAngles = ElyGoToOnEnd.eulerAngles;
	}

	private IEnumerator RotateAlly(float a)
	{
		yield return new WaitForSeconds(a);
		_assistantController.transform.eulerAngles = AllyGoToOnEnd.eulerAngles;
	}
}

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

public class CutScene_4_2_1 : SlugterraCutScene
{
	[SerializeField]
	private GameController gameController;

	[SerializeField]
	private Transform EliGoTo;

	[SerializeField]
	private Transform AllyGoTo;

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
		yield return new WaitForSeconds(3f);
		RisePlayCutSceneEvent(true);
		MainCamera = GameObject.Find("MainCamera");
		UICamera = GameObject.Find("UICamera");
		cutSceneCamera = GameObject.Find("CameraCutSceneText");
		_assistantController = Object.FindObjectOfType<AssistantController>();
		_cameraFade = MainCamera.GetComponent<CameraFade>();
		_cameraSmoothFollow = MainCamera.GetComponent<CameraSmoothFollow>();
		_camerAnimation = MainCamera.GetComponent<Animation>();
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 1f), 1f, 0.1f, 2f);
		_cutSceneView.SkipButton.targetObject = gameObject;
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		gameController.PlayCutScene(true);
		float time = 3f;
		StartCoroutine(TextHint_3(time));
		float _time = 1f;
		StartCoroutine(StartCameraTrack(_time));
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
		gameController.PlayCutScene(false);
		DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
		RisePlayCutSceneEvent(false);
		gameObject.SetActive(false);
	}

	private IEnumerator StartCameraTrack(float a)
	{
		yield return new WaitForSeconds(a);
		UICamera.SetActive(false);
		_cutSceneView.SkipButton.targetObject = gameObject;
		_cameraSmoothFollow.enabled = false;
		MainCamera.GetComponent<Animation>().Play("CameraTrack_2");
	}

	private IEnumerator TextHint_3(float a)
	{
		yield return new WaitForSeconds(a);
		ShowScreens(true);
		_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["4_2_cutscene_2_text_01"];
		float time = 3f;
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
			_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["4_2_cutscene_2_text_02"];
			float a2 = 5f;
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
		_eliController.MoveTarget.position = EliGoTo.position;
		_eliController.transform.position = EliGoTo.position;
		if (_assistantController != null && !(_assistantController.StateMachine.CurrentState is DeathState))
		{
			_assistantController.MoveTarget.position = AllyGoTo.position;
			_assistantController.transform.position = AllyGoTo.position;
		}
	}
}

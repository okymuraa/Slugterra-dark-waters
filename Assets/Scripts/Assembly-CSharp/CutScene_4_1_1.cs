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

public class CutScene_4_1_1 : SlugterraCutScene
{
	[SerializeField]
	private GameController gameController;

	[SerializeField]
	private GameObject Txt_Cut_2;

	[SerializeField]
	private GameObject Txt_Cut_3;

	[SerializeField]
	private GameObject Txt_Cut_4;

	[SerializeField]
	private Transform EliGoTo;

	[SerializeField]
	private Transform AllyGoTo;

	[SerializeField]
	private Transform EliGoTo1;

	[SerializeField]
	private Transform AllyGoTo1;

	[SerializeField]
	private GameObject allMobs;

	private GameObject MainCamera;

	private GameObject UICamera;

	private GameObject cutSceneCamera;

	private EliController _eliController;

	private AssistantController _assistantController;

	private CameraFade _cameraFade;

	private CameraSmoothFollow _cameraSmoothFollow;

	private Animation _camerAnimation;

	private tk2dButton _skip;

	private bool _skipEnable;

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
		gameController.PlayCutScene(true);
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		float time = 4f;
		StartCoroutine(TextHint_2(time));
		float _time = 1f;
		StartCoroutine(StartCameraTrack(_time));
		StartCoroutine(EliAllyGoTo(0.5f));
	}

	private IEnumerator DelaySkip(float a)
	{
		yield return new WaitForSeconds(a);
		Skip();
	}

	private void Skip()
	{
		EliAllyGoToOnEnd();
		_camerAnimation.Stop();
		_cameraSmoothFollow.enabled = true;
		cutSceneCamera.SetActive(false);
		UICamera.SetActive(true);
		_cameraFade.StopFade();
		Txt_Cut_2.SetActive(false);
		Txt_Cut_3.SetActive(false);
		Txt_Cut_4.SetActive(false);
		ShowScreens(false);
		gameController.OnFinishGame();
		DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
		gameController.PlayCutScene(false);
		RisePlayCutSceneEvent(false);
		gameObject.SetActive(false);
	}

	private IEnumerator StartCameraTrack(float a)
	{
		yield return new WaitForSeconds(a);
		allMobs.SetActive(false);
		UICamera.SetActive(false);
		_cutSceneView.SkipButton.targetObject = gameObject;
		_cameraSmoothFollow.enabled = false;
		MainCamera.GetComponent<Animation>().Play("CameraTrack_2");
	}

	private IEnumerator TextHint_2(float a)
	{
		yield return new WaitForSeconds(a);
		ShowScreens(true);
		_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["4_1_cutscene_2_text_01"];
		float time = 4f;
		StartCoroutine(HideTextHint_2(time));
		_skipEnable = true;
		_cutSceneView.SkipButton.gameObject.SetActive(true);
	}

	private IEnumerator HideTextHint_2(float a)
	{
		yield return new WaitForSeconds(a);
		if (_skipEnable)
		{
			float a2 = 1f;
			StartCoroutine(TextHint_3(a2));
		}
	}

	private IEnumerator TextHint_3(float a)
	{
		yield return new WaitForSeconds(a);
		if (_skipEnable)
		{
			_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["4_1_cutscene_2_text_02"];
			float a2 = 4f;
			StartCoroutine(HideTextHint_3(a2));
		}
	}

	private IEnumerator HideTextHint_3(float a)
	{
		yield return new WaitForSeconds(a);
		if (_skipEnable)
		{
			float a2 = 1f;
			StartCoroutine(TextHint_4(a2));
			EliAllyGoToOnEnd();
		}
	}

	private IEnumerator TextHint_4(float a)
	{
		yield return new WaitForSeconds(a);
		if (_skipEnable)
		{
			_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["4_1_cutscene_2_text_03"];
			float a2 = 4f;
			StartCoroutine(HideTextHint_4(a2));
		}
	}

	private IEnumerator HideTextHint_4(float a)
	{
		yield return new WaitForSeconds(a);
		if (_skipEnable)
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

	private IEnumerator EliAllyGoTo(float a)
	{
		yield return new WaitForSeconds(a);
		_eliController.MoveTarget.position = EliGoTo.position;
		_eliController.transform.position = EliGoTo.position;
		float time = 0.7f;
		StartCoroutine(RotateEly(time));
		if (_assistantController != null && !(_assistantController.StateMachine.CurrentState is DeathState))
		{
			_assistantController.MoveTarget.position = AllyGoTo.position;
			_assistantController.transform.position = AllyGoTo.position;
		}
	}

	private IEnumerator RotateEly(float a)
	{
		yield return new WaitForSeconds(a);
		_eliController.transform.eulerAngles = EliGoTo.eulerAngles;
	}

	private void EliAllyGoToOnEnd()
	{
		_eliController.MoveTarget.position = EliGoTo1.position;
		_eliController.OnMove(5f);
		if (_assistantController != null && !(_assistantController.StateMachine.CurrentState is DeathState))
		{
			_assistantController.MoveTarget.position = AllyGoTo1.position;
			_assistantController.OnMove(5f);
		}
	}
}

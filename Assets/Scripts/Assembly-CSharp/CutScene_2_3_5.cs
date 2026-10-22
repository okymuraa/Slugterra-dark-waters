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

public class CutScene_2_3_5 : SlugterraCutScene
{
	[SerializeField]
	private GameController gameController;

	[SerializeField]
	private GameObject MoveTargetEli;

	[SerializeField]
	private GameObject MoveTargetAlly;

	[SerializeField]
	private GameObject Billy;

	[SerializeField]
	private Transform EliGoTo_AB_4;

	[SerializeField]
	private Transform AllyGoTo_AB_4;

	[SerializeField]
	private Transform BillySpawnPoint_AB_4;

	[SerializeField]
	private Transform BillyGoTo_AB_4;

	private GameObject MainCamera;

	private GameObject UICamera;

	private GameObject cutSceneCamera;

	private EliController _eliController;

	private AssistantController _assistantController;

	private BillyController _billyController;

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
		_cameraFade = MainCamera.GetComponent<CameraFade>();
		_cameraSmoothFollow = MainCamera.GetComponent<CameraSmoothFollow>();
		_camerAnimation = MainCamera.GetComponent<Animation>();
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 1f), 1f, 0.1f, 2f);
		_cutSceneView.SkipButton.targetObject = gameObject;
		_assistantController = Object.FindObjectOfType<AssistantController>();
		_billyController = Billy.GetComponent<BillyController>();
		gameController.PlayCutScene(true);
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		float time = 3f;
		StartCoroutine(TextHint_12(time));
		float _time = 1f;
		StartCoroutine(StartCameraTrack(_time));
		ProntoEliGoTo();
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
		BillyGoTo();
		gameController.OnFinishGame();
		DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
		gameController.PlayCutScene(false);
		RisePlayCutSceneEvent(false);
		gameObject.SetActive(false);
	}

	private IEnumerator StartCameraTrack(float a)
	{
		yield return new WaitForSeconds(a);
		_eliController.transform.eulerAngles = EliGoTo_AB_4.eulerAngles;
		UICamera.SetActive(false);
		_cutSceneView.SkipButton.targetObject = gameObject;
		_cameraSmoothFollow.enabled = false;
		_camerAnimation.Play("CameraTrack_5");
	}

	private IEnumerator TextHint_12(float a)
	{
		yield return new WaitForSeconds(a);
		_eliController.transform.eulerAngles = EliGoTo_AB_4.eulerAngles;
		_cutSceneView.SpeakerIcon = "icon_billy";
		ShowScreens(true);
		_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["2_5_cutscene_5_text_01"];
		float time = 4f;
		StartCoroutine(HideTextHint_12(time));
		_isSkipEnable = true;
		_cutSceneView.SkipButton.gameObject.SetActive(true);
	}

	private IEnumerator HideTextHint_12(float a)
	{
		yield return new WaitForSeconds(a);
		_eliController.transform.eulerAngles = EliGoTo_AB_4.eulerAngles;
		if (_isSkipEnable)
		{
			BillyGoTo();
			float a2 = 1f;
			StartCoroutine(TextHint_13(a2));
		}
	}

	private IEnumerator TextHint_13(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			_cutSceneView.SpeakerIcon = "icon_eli";
			_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["2_5_cutscene_5_text_02"];
			float a2 = 4f;
			StartCoroutine(HideTextHint_13(a2));
		}
	}

	private IEnumerator HideTextHint_13(float a)
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

	private void ProntoEliGoTo()
	{
		Billy.SetActive(true);
		Billy.transform.position = BillySpawnPoint_AB_4.position;
		Billy.transform.eulerAngles = BillySpawnPoint_AB_4.eulerAngles;
		_eliController.enabled = false;
		_eliController.MoveTarget.transform.position = EliGoTo_AB_4.position;
		_eliController.OnMove(0f);
		_eliController.transform.position = EliGoTo_AB_4.position;
		if (_assistantController != null && !(_assistantController.StateMachine.CurrentState is Slugterra.Game.Units.Ally.Assistant.DeathState))
		{
			_assistantController.MoveTarget.position = AllyGoTo_AB_4.transform.position;
			_assistantController.transform.position = AllyGoTo_AB_4.position;
		}
	}

	private void BillyGoTo()
	{
		_billyController.MoveTarget.position = BillyGoTo_AB_4.transform.position;
		_billyController.OnMove(7f);
	}
}

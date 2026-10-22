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

public class CutScene_4_3_1 : SlugterraCutScene
{
	[SerializeField]
	private GameController gameController;

	[SerializeField]
	private Transform EliGoTo;

	[SerializeField]
	private Transform AllyGoTo;

	[SerializeField]
	private GameObject nacho;

	[SerializeField]
	private Transform nachoGoToOnStart;

	private GameObject MainCamera;

	private GameObject UICamera;

	[SerializeField]
	private NachoController _nachoController;

	private EliController _eliController;

	private AssistantController _assistantController;

	private CameraFade _cameraFade;

	private CameraSmoothFollow _cameraSmoothFollow;

	private Animation _camerAnimation;

	private GameObject backgroundMusicManager;

	private AudioSource aSource;

	private AudioClip aClip;

	[SerializeField]
	private GameObject missionObjectives;

	private void Start()
	{
		aSource = GetComponent<AudioSource>();
		backgroundMusicManager = GameObject.Find("BackgroundMusicManager");
		if (backgroundMusicManager != null)
		{
			backgroundMusicManager.GetComponent<AudioSource>().Stop();
		}
		aSource.Play();
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
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 1f), 1f, 0.1f, 2f);
		_assistantController = Object.FindObjectOfType<AssistantController>();
		_cutSceneView.SkipButton.targetObject = gameObject;
		gameController.PlayCutScene(true);
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		float time = 3f;
		StartCoroutine(TextHint_2(time));
		float _time = 1f;
		StartCoroutine(StartCameraTrack(_time));
		nacho.SetActive(true);
		_nachoController.enabled = false;
	}

	private IEnumerator DelaySkip(float a)
	{
		yield return new WaitForSeconds(a);
		Skip();
	}

	private void Skip()
	{
		missionObjectives.SetActive(true);
		_camerAnimation.Stop();
		_cameraSmoothFollow.enabled = true;
		UICamera.SetActive(true);
		ShowScreens(false);
		_eliController.SetAttackTarget(null);
		DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
		_nachoController.enabled = true;
		gameController.PlayCutScene(false);
		RisePlayCutSceneEvent(false);
	}

	private IEnumerator StartCameraTrack(float a)
	{
		yield return new WaitForSeconds(a);
		UICamera.SetActive(false);
		_cutSceneView.SkipButton.targetObject = gameObject;
		_cameraSmoothFollow.enabled = false;
		MainCamera.GetComponent<Animation>().Play("CameraTrack_1a");
		EliAllyGoTo();
	}

	private IEnumerator TextHint_2(float a)
	{
		yield return new WaitForSeconds(a);
		float time = 0.1f;
		StartCoroutine(RotateEly(time));
		ShowScreens(true);
		_cutSceneView.SpeakerIcon = "icon_nacho";
		_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["4_3_cutscene_2_text_01"];
		float _time = 4f;
		StartCoroutine(HideTextHint_2(_time));
		_isSkipEnable = true;
		_cutSceneView.SkipButton.gameObject.SetActive(true);
	}

	private IEnumerator HideTextHint_2(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			float a2 = 1f;
			StartCoroutine(TextHint_3(a2));
		}
	}

	private IEnumerator TextHint_3(float a)
	{
		yield return new WaitForSeconds(a);
		float _time = 0.1f;
		StartCoroutine(RotateEly(_time));
		_cutSceneView.SpeakerIcon = "icon_eli";
		_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["4_3_cutscene_2_text_02"];
		float time = 4f;
		StartCoroutine(HideTextHint_3(time));
	}

	private IEnumerator HideTextHint_3(float a)
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
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 1f), 1f, 0.5f, 0.5f);
		_cutSceneView.SkipButton.gameObject.SetActive(false);
		_cutSceneView.SkipButton.targetObject = null;
		float a = 1f;
		StartCoroutine(DelaySkip(a));
	}

	private void EliAllyGoTo()
	{
		_eliController.MoveTarget.position = EliGoTo.position;
		_eliController.transform.position = EliGoTo.position;
		float a = 1.5f;
		StartCoroutine(RotateEly(a));
		if (_assistantController != null && !(_assistantController.StateMachine.CurrentState is Slugterra.Game.Units.Ally.Assistant.DeathState))
		{
			_assistantController.MoveTarget.position = AllyGoTo.position;
			_assistantController.transform.position = AllyGoTo.position;
			RotateAlly();
		}
	}

	private IEnumerator RotateEly(float a)
	{
		yield return new WaitForSeconds(a);
		_eliController.transform.rotation = Quaternion.Euler(EliGoTo.eulerAngles);
	}

	private void RotateAlly()
	{
		_assistantController.transform.eulerAngles = AllyGoTo.eulerAngles;
	}
}

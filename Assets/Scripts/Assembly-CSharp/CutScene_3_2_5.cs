using System.Collections;
using AppsMinistry.Core;
using AppsMinistry.Core.Input;
using AppsMinistry.UI.Camera;
using Slugterra.Game.Units.Ally;
using Slugterra.Scene;
using Slugterra.UI.Game;
using Slugterra.UI.Game.Camera;
using UnityEngine;

public class CutScene_3_2_5 : SlugterraCutScene
{
	[SerializeField]
	private GameController gameController;

	public float _time_StartCameraTrack = 1f;

	public float _time_textHint_2 = 3f;

	public float _time_HideTextHint_2 = 3f;

	public float _time_textHint_3 = 3f;

	public float _time_HideTextHint_3 = 3f;

	public float _time_DelaySkip = 1f;

	[SerializeField]
	private Transform EliGoTo_AB_5;

	[SerializeField]
	private Transform AllyGoTo_AB_5;

	private GameObject MainCamera;

	private GameObject UICamera;

	private GameObject cutSceneCamera;

	private EliController _eliController;

	private CameraFade _cameraFade;

	private CameraSmoothFollow _cameraSmoothFollow;

	private Animation _camerAnimation;

	private void Start()
	{
		RisePlayCutSceneEvent(true);
		MainCamera = GameObject.Find("MainCamera");
		UICamera = GameObject.Find("UICamera");
		cutSceneCamera = GameObject.Find("CameraCutSceneText");
		_eliController = Object.FindObjectOfType<EliController>();
		_cameraFade = MainCamera.GetComponent<CameraFade>();
		_cameraSmoothFollow = MainCamera.GetComponent<CameraSmoothFollow>();
		_camerAnimation = MainCamera.GetComponent<Animation>();
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 1f), 1f, 0.1f, 2f);
		_cutSceneView.SkipButton.targetObject = gameObject;
		gameController.PlayCutScene(true);
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		StartCoroutine(TextHint_2(_time_textHint_2));
		StartCoroutine(StartCameraTrack(_time_StartCameraTrack));
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
		_camerAnimation.Play("CameraTrack_2");
	}

	private IEnumerator TextHint_2(float a)
	{
		yield return new WaitForSeconds(a);
		ShowScreens(true);
		_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["3_2_cutscene_2_text_01"];
		StartCoroutine(HideTextHint_2(_time_HideTextHint_2));
		_isSkipEnable = true;
		_cutSceneView.SkipButton.gameObject.SetActive(true);
	}

	private IEnumerator HideTextHint_2(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			StartCoroutine(TextHint_3(_time_textHint_3));
		}
	}

	private IEnumerator TextHint_3(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			_cutSceneView.SpeakerIcon = "icon_eli";
			_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["3_2_cutscene_2_text_02"];
			StartCoroutine(HideTextHint_13(_time_HideTextHint_3));
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
		_eliController.MoveTarget.transform.position = EliGoTo_AB_5.position;
		_eliController.transform.position = EliGoTo_AB_5.position;
	}
}

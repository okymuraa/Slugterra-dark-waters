using System.Collections;
using AppsMinistry.Core;
using AppsMinistry.Core.Input;
using AppsMinistry.UI.Camera;
using Slugterra.Scene;
using Slugterra.UI.Game;
using Slugterra.UI.Game.Camera;
using UnityEngine;

public class CutScene_2_2_0 : SlugterraCutScene
{
	[SerializeField]
	private GameController gameController;

	[SerializeField]
	private GameObject startBlackPlane;

	private GameObject MainCamera;

	private GameObject UICamera;

	private CameraSmoothFollow _cameraSmoothFollow;

	private CameraFade _cameraFade;

	private Animation _camerAnimation;

	private void Start()
	{
		RisePlayCutSceneEvent(true);
		MainCamera = GameObject.Find("MainCamera");
		UICamera = GameObject.Find("UICamera");
		_cameraSmoothFollow = MainCamera.GetComponent<CameraSmoothFollow>();
		_cameraFade = MainCamera.GetComponent<CameraFade>();
		_camerAnimation = MainCamera.GetComponent<Animation>();
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 1f), 2f, 0.1f, 2f);
		_cutSceneView.SkipButton.targetObject = gameObject;
		gameController.PlayCutScene(true);
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		float a = 3f;
		StartCoroutine(TextHint_0(a));
		float a2 = 1f;
		StartCoroutine(StartCameraTrack(a2));
	}

	private IEnumerator StartCameraTrack(float a)
	{
		yield return new WaitForSeconds(a);
		UICamera.SetActive(false);
		_cameraSmoothFollow.enabled = false;
		_camerAnimation.Play("CameraTrack_1");
		startBlackPlane.SetActive(false);
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
		UICamera.SetActive(true);
		ShowScreens(false);
		DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
		gameController.PlayCutScene(false);
		RisePlayCutSceneEvent(false);
		gameObject.SetActive(false);
	}

	private IEnumerator TextHint_0(float a)
	{
		yield return new WaitForSeconds(a);
		ShowScreens(true);
		_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["2_3_cutscene_1_text_01"];
		float _time = 4f;
		StartCoroutine(HideTextHint_0(_time));
		_isSkipEnable = true;
		_cutSceneView.SkipButton.gameObject.SetActive(true);
	}

	private IEnumerator HideTextHint_0(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			float a2 = 1f;
			StartCoroutine(TextHint_1(a2));
		}
	}

	private IEnumerator TextHint_1(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["2_3_cutscene_1_text_02"];
			float a2 = 4f;
			StartCoroutine(HideTextHint_1(a2));
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

	private void Fade()
	{
		_isSkipEnable = false;
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 1f), 2f, 1f, 1f);
		_cutSceneView.SkipButton.gameObject.SetActive(false);
		_cutSceneView.SkipButton.targetObject = null;
		float a = 1f;
		StartCoroutine(DelaySkip(a));
	}
}

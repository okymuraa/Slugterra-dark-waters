using System.Collections;
using AppsMinistry.Core;
using AppsMinistry.Core.Input;
using AppsMinistry.UI.Camera;
using Slugterra.Game.Units.Ally;
using Slugterra.Scene;
using Slugterra.UI.Game;
using Slugterra.UI.Game.Camera;
using UnityEngine;

public class CutScene_2_1_5 : SlugterraCutScene
{
	[SerializeField]
	private GameController gameController;

	[SerializeField]
	private GameObject startBlackPlane;

	private GameObject MainCamera;

	private GameObject UICamera;

	private EliController _eliController;

	private CameraFade _cameraFade;

	private CameraSmoothFollow _cameraSmoothFollow;

	private Animation _camerAnimation;

	public GameObject DamageBlaster;

	private GameObject clone;

	private void Start()
	{
		RisePlayCutSceneEvent(true);
		MainCamera = GameObject.Find("MainCamera");
		UICamera = GameObject.Find("UICamera");
		_camerAnimation = MainCamera.GetComponent<Animation>();
		_cameraFade = MainCamera.GetComponent<CameraFade>();
		_cameraSmoothFollow = MainCamera.GetComponent<CameraSmoothFollow>();
		_eliController = Object.FindObjectOfType<EliController>();
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 1f), 2f, 0.1f, 2f);
		_DisableInput();
		StartCoroutine(AfterStart(0.001f));
		float a = 4f;
		StartCoroutine(TextHint_0(a));
		float a2 = 1f;
		StartCoroutine(StartCameraTrack(a2));
		StartCoroutine(StartIntro(2.5f));
	}

	private void _DisableInput()
	{
		Debug.Log("Disable in cut scene");
		gameController.PlayCutScene(true);
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
	}

	private IEnumerator AfterStart(float t)
	{
		yield return new WaitForSeconds(t);
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
	}

	private IEnumerator StartCameraTrack(float a)
	{
		yield return new WaitForSeconds(a);
		UICamera.SetActive(false);
		_cutSceneView.SkipButton.targetObject = gameObject;
		_cameraSmoothFollow.enabled = false;
		_camerAnimation.Play("CameraTrack_7");
		startBlackPlane.SetActive(false);
		GameObject oldblaster = _eliController.ShotPoint.transform.Find("SlugBlaster(Clone)").gameObject;
		if (oldblaster != null)
		{
			clone = Object.Instantiate(DamageBlaster, oldblaster.transform.position, Quaternion.identity);
			clone.transform.parent = oldblaster.transform.parent;
			Object.Destroy(oldblaster);
		}
	}

	private IEnumerator StartIntro(float a)
	{
		yield return new WaitForSeconds(a);
		_eliController.Animator.SetTrigger("Intro");
		clone.GetComponent<Animation>().enabled = true;
		yield return new WaitForSeconds(1f);
		clone.GetComponent<AudioSource>().Play();
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
		_eliController.SetCanAttack(false);
		DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
		gameController.PlayCutScene(false);
		RisePlayCutSceneEvent(false);
		gameObject.SetActive(false);
		_eliController.Animator.SetTrigger("IntroEnd");
	}

	private IEnumerator TextHint_0(float a)
	{
		yield return new WaitForSeconds(a);
		ShowScreens(true);
		_cutSceneView.SpeakerIcon = "icon_eli";
		_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["2_2_cutscene_1_text_01"];
		float time = 4f;
		StartCoroutine(HideTextHint_0(time));
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
			_cutSceneView.SpeakerIcon = "icon_pronto";
			_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["2_2_cutscene_1_text_02"];
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
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 1f), 1f, 1f, 1f);
		_cutSceneView.SkipButton.gameObject.SetActive(false);
		_cutSceneView.SkipButton.targetObject = null;
		float a = 1f;
		StartCoroutine(DelaySkip(a));
	}
}

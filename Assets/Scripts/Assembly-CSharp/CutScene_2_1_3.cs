using System.Collections;
using AppsMinistry.Core;
using AppsMinistry.Core.Input;
using AppsMinistry.UI.Camera;
using Slugterra.Game.Units.Ally;
using Slugterra.Game.Units.Enemy;
using Slugterra.Scene;
using Slugterra.UI.Game;
using Slugterra.UI.Game.Camera;
using UnityEngine;

public class CutScene_2_1_3 : SlugterraCutScene
{
	[SerializeField]
	private GameController gameController;

	[SerializeField]
	private Transform GoToOnStart;

	[SerializeField]
	private GameObject Arrow1;

	[SerializeField]
	private GameObject wagon1;

	[SerializeField]
	private float scrapHeapViewRadius = 20f;

	[SerializeField]
	private ScrapHeapController[] scrapHeap;

	private GameObject MainCamera;

	private GameObject UICamera;

	private EliController _eliController;

	private CameraFade _cameraFade;

	private CameraSmoothFollow _cameraSmoothFollow;

	private Animation _camerAnimation;

	private void Start()
	{
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		gameController.PlayCutScene(true);
		RisePlayCutSceneEvent(true);
		MainCamera = GameObject.Find("MainCamera");
		UICamera = GameObject.Find("UICamera");
		_eliController = Object.FindObjectOfType<EliController>();
		_camerAnimation = MainCamera.GetComponent<Animation>();
		_cameraFade = MainCamera.GetComponent<CameraFade>();
		_cameraSmoothFollow = MainCamera.GetComponent<CameraSmoothFollow>();
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 1f), 1f, 0.1f, 2f);
		float a = 3f;
		StartCoroutine(TextHint_8(a));
		float a2 = 1f;
		StartCoroutine(StartCameraTrack(a2));
	}

	private IEnumerator StartCameraTrack(float a)
	{
		yield return new WaitForSeconds(a);
		UICamera.SetActive(false);
		_cutSceneView.SkipButton.targetObject = gameObject;
		_cameraSmoothFollow.enabled = false;
		_camerAnimation.Play("CameraTrack_4");
		EliGoTo();
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
		Arrow1.SetActive(true);
		DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
		gameController.PlayCutScene(false);
		RisePlayCutSceneEvent(false);
	}

	private void FixedUpdate()
	{
		if (!(wagon1 == null))
		{
			return;
		}
		for (int i = 0; i < scrapHeap.Length; i++)
		{
			if (scrapHeap[i] != null)
			{
				scrapHeap[i].ViewArea.SetRadius(scrapHeapViewRadius);
			}
		}
		gameObject.SetActive(false);
	}

	private IEnumerator TextHint_8(float a)
	{
		yield return new WaitForSeconds(a);
		_cutSceneView.SpeakerIcon = "icon_eli";
		ShowScreens(true);
		_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["2_1_cutscene_4_text_01"];
		float time = 4f;
		StartCoroutine(HideTextHint_8(time));
		_isSkipEnable = true;
		_cutSceneView.SkipButton.gameObject.SetActive(true);
	}

	private IEnumerator HideTextHint_8(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			float a2 = 1f;
			StartCoroutine(TextHint_9(a2));
		}
	}

	private IEnumerator TextHint_9(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["2_1_cutscene_4_text_02"];
			float a2 = 4f;
			StartCoroutine(HideTextHint_9(a2));
		}
	}

	private IEnumerator HideTextHint_9(float a)
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

	private void EliGoTo()
	{
		_eliController.MoveTarget.transform.position = GoToOnStart.position;
		_eliController.OnMove(0f);
		_eliController.transform.position = GoToOnStart.position;
		_eliController.transform.eulerAngles = GoToOnStart.eulerAngles;
	}
}

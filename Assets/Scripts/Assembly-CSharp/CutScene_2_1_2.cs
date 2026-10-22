using System.Collections;
using AppsMinistry.Core;
using AppsMinistry.Core.Input;
using AppsMinistry.UI.Camera;
using Slugterra.Game.Units.Ally;
using Slugterra.Scene;
using Slugterra.UI.Game;
using Slugterra.UI.Game.Camera;
using UnityEngine;

public class CutScene_2_1_2 : SlugterraCutScene
{
	[SerializeField]
	private GameController gameController;

	[SerializeField]
	private Transform GoToOnStart;

	public GameObject barricadeCollision;

	[SerializeField]
	private SphereCollider[] MobView;

	[SerializeField]
	private GameObject Arrow_1;

	[SerializeField]
	private GameObject Arrow_2;

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
		StartCoroutine(TextHint_6(a));
		float a2 = 1f;
		StartCoroutine(StartCameraTrack(a2));
	}

	private IEnumerator StartCameraTrack(float a)
	{
		yield return new WaitForSeconds(a);
		for (int i = 0; i < MobView.Length; i++)
		{
			if (MobView[i] != null)
			{
				MobView[i].radius = 0f;
			}
			else
			{
				Debug.Log("###  No mobView ref in:" + gameObject.name.ToString());
			}
		}
		UICamera.SetActive(false);
		_cutSceneView.SkipButton.targetObject = gameObject;
		_cameraSmoothFollow.enabled = false;
		_camerAnimation.Play("CameraTrack_3");
		EliGoTo();
	}

	private IEnumerator DelaySkip(float a)
	{
		yield return new WaitForSeconds(a);
		Skip();
	}

	private void Skip()
	{
		Arrow_1.SetActive(false);
		Arrow_2.SetActive(false);
		for (int i = 0; i < MobView.Length; i++)
		{
			if (MobView[i] != null)
			{
				MobView[i].radius = 12f;
			}
			else
			{
				Debug.Log("###  No mobView ref in:" + gameObject.name.ToString());
			}
		}
		barricadeCollision.SetActive(false);
		_camerAnimation.Stop();
		_cameraSmoothFollow.enabled = true;
		UICamera.SetActive(true);
		ShowScreens(false);
		DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
		gameController.PlayCutScene(false);
		RisePlayCutSceneEvent(false);
		gameObject.SetActive(false);
	}

	private IEnumerator TextHint_6(float a)
	{
		yield return new WaitForSeconds(a);
		RotateEly();
		_cutSceneView.SpeakerIcon = "icon_eli";
		ShowScreens(true);
		_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["2_1_cutscene_3_text_01"];
		_isSkipEnable = true;
		_cutSceneView.SkipButton.gameObject.SetActive(true);
		float time = 5f;
		StartCoroutine(HideTextHint_6(time));
	}

	private IEnumerator HideTextHint_6(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			float a2 = 1f;
			StartCoroutine(TextHint_7(a2));
		}
	}

	private IEnumerator TextHint_7(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			Arrow_1.SetActive(true);
			Arrow_2.SetActive(true);
			_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["2_1_cutscene_3_text_02"];
			float a2 = 5f;
			StartCoroutine(HideTextHint_7(a2));
		}
	}

	private IEnumerator HideTextHint_7(float a)
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

	private void RotateEly()
	{
		_eliController.transform.rotation = Quaternion.Euler(GoToOnStart.eulerAngles);
	}
}

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

public class CutScene_2_1_4 : SlugterraCutScene
{
	[SerializeField]
	private GameController gameController;

	[SerializeField]
	private GameObject Pronto;

	[SerializeField]
	private GameObject InfurnusButton;

	[SerializeField]
	private Transform GoToOnStart;

	[SerializeField]
	private Transform ProntoStart;

	[SerializeField]
	private Transform ProntoOnEnd;

	[SerializeField]
	private AvatarInfoView AssistantHUD;

	private GameObject MainCamera;

	private GameObject UICamera;

	private GameObject cutSceneCamera;

	private EliController _eliController;

	private AssistantController _assistantController;

	private CameraFade _cameraFade;

	private CameraSmoothFollow _cameraSmoothFollow;

	private Animation _camerAnimation;

	[SerializeField]
	private GameObject HandFinish;

	[SerializeField]
	private GameObject PopupConfirm;

	[SerializeField]
	private GameObject PopupAppStore;

	private float temp = 2f;

	private void Start()
	{
		_eliController = Object.FindObjectOfType<EliController>();
		if (_eliController.Model.Health > 0f)
		{
			StartCoroutine(StartDelay());
		}
		HandFinish.SetActive(false);
	}

	private IEnumerator StartDelay()
	{
		yield return new WaitForSeconds(3f);
		RisePlayCutSceneEvent(true);
		Pronto.SetActive(true);
		MainCamera = GameObject.Find("MainCamera");
		UICamera = GameObject.Find("UICamera");
		cutSceneCamera = GameObject.Find("CameraCutSceneText");
		_assistantController = Object.FindObjectOfType<AssistantController>();
		_camerAnimation = MainCamera.GetComponent<Animation>();
		_cameraFade = MainCamera.GetComponent<CameraFade>();
		_cameraSmoothFollow = MainCamera.GetComponent<CameraSmoothFollow>();
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 1f), 1f, 0.1f, 2f);
		gameController.PlayCutScene(true);
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		float time = 3f;
		StartCoroutine(TextHint_10(time));
		float _time = 1f;
		StartCoroutine(StartCameraTrack(_time));
	}

	private IEnumerator StartCameraTrack(float a)
	{
		GameObject[] _ActiveEnemies = GameObject.FindGameObjectsWithTag("Enemy");
		for (int i = 0; i < _ActiveEnemies.Length; i++)
		{
			_ActiveEnemies[i].SetActive(false);
		}
		yield return new WaitForSeconds(a);
		Pronto.SetActive(true);
		UICamera.SetActive(false);
		_cutSceneView.SkipButton.targetObject = gameObject;
		_cameraSmoothFollow.enabled = false;
		_camerAnimation.Play("CameraTrack_5");
		EliGoTo();
		ProntoGoTo();
	}

	private IEnumerator DelaySkip(float a)
	{
		yield return new WaitForSeconds(a);
		Skip();
	}

	private void Skip()
	{
		_camerAnimation.Stop();
		cutSceneCamera.SetActive(false);
		UICamera.SetActive(true);
		_cameraFade.StopFade();
		AssistantUITurnOn();
		_eliController.SetCanAttack(false);
		gameController.OnFinishGame();
		DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
		gameController.PlayCutScene(false);
		RisePlayCutSceneEvent(false);
	}

	private void LateUpdate()
	{
		if (HandFinish != null)
		{
			if (PopupConfirm.activeSelf || PopupAppStore.activeSelf)
			{
				HandFinish.GetComponent<MeshRenderer>().enabled = false;
			}
			else
			{
				HandFinish.GetComponent<MeshRenderer>().enabled = true;
			}
		}
		if (Time.timeScale == 0f)
		{
			temp -= Time.unscaledDeltaTime;
			if (temp <= 0f)
			{
				HandFinish.SetActive(true);
			}
		}
	}

	private IEnumerator TextHint_10(float a)
	{
		yield return new WaitForSeconds(a);
		_cutSceneView.SpeakerIcon = "icon_pronto";
		ShowScreens(true);
		_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["2_1_cutscene_5_text_01"];
		float time = 4f;
		StartCoroutine(HideTextHint_10(time));
		_isSkipEnable = true;
		_cutSceneView.SkipButton.gameObject.SetActive(true);
	}

	private IEnumerator HideTextHint_10(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			float a2 = 1f;
			StartCoroutine(TextHint_11(a2));
		}
	}

	private IEnumerator TextHint_11(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			float a2 = 0.1f;
			StartCoroutine(RotateEly(a2));
			_cutSceneView.SpeakerIcon = "icon_eli";
			_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["2_1_cutscene_5_text_02"];
			float a3 = 4f;
			StartCoroutine(HideTextHint_11(a3));
		}
	}

	private IEnumerator HideTextHint_11(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			float a2 = 1f;
			StartCoroutine(TextHint_12(a2));
		}
	}

	private IEnumerator TextHint_12(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			_cutSceneView.SpeakerIcon = "icon_pronto";
			_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["2_1_cutscene_5_text_03"];
			float a2 = 4f;
			StartCoroutine(HideTextHint_12(a2));
		}
	}

	private IEnumerator HideTextHint_12(float a)
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

	private void EliGoTo()
	{
		_eliController.MoveTarget.transform.position = GoToOnStart.position;
		_eliController.OnMove(0f);
		_eliController.transform.position = GoToOnStart.position;
		float a = 1.5f;
		StartCoroutine(RotateEly(a));
	}

	private void ProntoGoTo()
	{
		if (_assistantController != null && !(_assistantController.StateMachine.CurrentState is DeathState))
		{
			_assistantController.MoveTarget.position = ProntoOnEnd.transform.position;
			_assistantController.OnMove(6f);
			float a = 6f;
			StartCoroutine(RotatePronto(a));
		}
	}

	private void AssistantUITurnOn()
	{
		AssistantHUD.gameObject.SetActive(true);
		InfurnusButton.SetActive(false);
	}

	private IEnumerator RotateEly(float a)
	{
		yield return new WaitForSeconds(a);
		_eliController.transform.rotation = Quaternion.Euler(GoToOnStart.eulerAngles);
	}

	private IEnumerator RotatePronto(float a)
	{
		yield return new WaitForSeconds(a);
		_assistantController.transform.eulerAngles = ProntoOnEnd.eulerAngles;
	}
}

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

public class CutScene_3_3_1 : SlugterraCutScene
{
	[SerializeField]
	private GameController gameController;

	[SerializeField]
	private Transform EliGoTo_AB_3;

	[SerializeField]
	private Transform AllyGoTo_AB_3;

	[SerializeField]
	private Transform Lock_1;

	[SerializeField]
	private Transform Lock_2;

	[SerializeField]
	private Transform Lode_1;

	[SerializeField]
	private Transform Lode_2;

	[SerializeField]
	private GameObject Lock;

	[SerializeField]
	private GameObject Lode;

	[SerializeField]
	private SphereCollider lockSphere;

	[SerializeField]
	private SphereCollider lodeSphere;

	[SerializeField]
	private Transform lock_Animation;

	[SerializeField]
	private Transform EllyDummy;

	private GameObject MainCamera;

	private GameObject UICamera;

	private EliController _eliController;

	private AssistantController _assistantController;

	private LockController _lockController;

	private LodeController _lodeController;

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

	private void Update()
	{
		lock_Animation.LookAt(EllyDummy.position);
	}

	private IEnumerator StartDelay()
	{
		yield return new WaitForSeconds(3f);
		RisePlayCutSceneEvent(true);
		MainCamera = GameObject.Find("MainCamera");
		UICamera = GameObject.Find("UICamera");
		_assistantController = Object.FindObjectOfType<AssistantController>();
		_cameraFade = MainCamera.GetComponent<CameraFade>();
		_cameraSmoothFollow = MainCamera.GetComponent<CameraSmoothFollow>();
		_camerAnimation = MainCamera.GetComponent<Animation>();
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 1f), 1f, 0.1f, 2f);
		_cutSceneView.SkipButton.targetObject = gameObject;
		gameController.PlayCutScene(true);
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		float time = 3f;
		StartCoroutine(TextHint_3(time));
		float _time = 1f;
		StartCoroutine(StartCameraTrack(_time));
		EliAllyGoTo();
		StartCoroutine(EnemySetActive());
		StartCoroutine(EnemyGoToOnStart());
	}

	private IEnumerator DelaySkip(float a)
	{
		yield return new WaitForSeconds(a);
		Skip();
	}

	private void Skip()
	{
		lock_Animation.rotation = Quaternion.identity;
		EnemyGoToOnEnd();
		_camerAnimation.Stop();
		_cameraSmoothFollow.enabled = true;
		UICamera.SetActive(true);
		ShowScreens(false);
		DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
		StartCoroutine(CutsceneTurnOff());
	}

	private IEnumerator CutsceneTurnOff()
	{
		yield return new WaitForSeconds(3f);
		lockSphere.radius = 15f;
		lodeSphere.radius = 15f;
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
		MainCamera.GetComponent<Animation>().Play("CameraTrack_2");
	}

	private IEnumerator TextHint_3(float a)
	{
		yield return new WaitForSeconds(a);
		EliAllyGoTo();
		ShowScreens(true);
		_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["3_3_cutscene_2_text_01"];
		float time = 4f;
		StartCoroutine(HideTextHint_3(time));
		_isSkipEnable = true;
		_cutSceneView.SkipButton.gameObject.SetActive(true);
	}

	private IEnumerator HideTextHint_3(float a)
	{
		yield return new WaitForSeconds(a);
		EliAllyGoTo();
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
			_cutSceneView.SpeakerIcon = "icon_locke";
			_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["3_3_cutscene_2_text_02"];
			float a2 = 4f;
			StartCoroutine(HideTextHint_4(a2));
		}
	}

	private IEnumerator HideTextHint_4(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			float a2 = 1f;
			StartCoroutine(TextHint_5(a2));
		}
	}

	private IEnumerator TextHint_5(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			_cutSceneView.SpeakerIcon = "icon_lode";
			_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["3_3_cutscene_2_text_03"];
			float a2 = 4f;
			StartCoroutine(HideTextHint_5(a2));
		}
	}

	private IEnumerator HideTextHint_5(float a)
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

	private IEnumerator EnemyGoToOnStart()
	{
		yield return new WaitForSeconds(1f);
		_lockController.MoveTarget.position = Lock_1.position;
		_lockController.OnMove(10f);
		_lodeController.MoveTarget.position = Lode_1.position;
		_lodeController.OnMove(10f);
	}

	private void EliAllyGoTo()
	{
		_eliController.MoveTarget.position = EliGoTo_AB_3.position;
		_eliController.transform.position = EliGoTo_AB_3.position;
		float a = 1f;
		StartCoroutine(RotateEly(a));
	}

	private IEnumerator RotateEly(float a)
	{
		yield return new WaitForSeconds(a);
		_eliController.transform.eulerAngles = EliGoTo_AB_3.eulerAngles;
	}

	private IEnumerator EnemySetActive()
	{
		yield return new WaitForSeconds(0.1f);
		Lock.SetActive(true);
		Lode.SetActive(true);
		_lockController = Lock.GetComponent<LockController>();
		_lodeController = Lode.GetComponent<LodeController>();
		lockSphere.radius = 0f;
		lodeSphere.radius = 0f;
	}

	private void EnemyGoToOnEnd()
	{
		_lockController.MoveTarget.position = Lock_2.position;
		_lockController.OnMove(5f);
		_lodeController.MoveTarget.position = Lode_2.position;
		_lodeController.OnMove(5f);
	}
}

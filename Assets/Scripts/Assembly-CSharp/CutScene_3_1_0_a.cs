using System.Collections;
using AppsMinistry.Core;
using AppsMinistry.Core.Input;
using AppsMinistry.UI.Camera;
using Slugterra.Game.Mission;
using Slugterra.Game.Units.Ally;
using Slugterra.Game.Units.Enemy;
using Slugterra.Scene;
using Slugterra.UI.Game;
using Slugterra.UI.Game.Camera;
using UnityEngine;

public class CutScene_3_1_0_a : SlugterraCutScene
{
	[SerializeField]
	private GameController gameController;

	[SerializeField]
	private Transform goPointWagon;

	[SerializeField]
	private ObjectiveTarget _endPoint;

	[SerializeField]
	private ScrapHeapController Mob_1;

	[SerializeField]
	private ScrapHeapController Mob_2;

	[SerializeField]
	private ScrapHeapController AllyMob_1;

	[SerializeField]
	private ScrapHeapController AllyMob_2;

	[SerializeField]
	private ObjectiveTarget enabledAB_2;

	private bool trigerDelay;

	private GameObject MainCamera;

	private GameObject UICamera;

	private EliController _eliController;

	private CameraFade _cameraFade;

	private CameraSmoothFollow _cameraSmoothFollow;

	private void Start()
	{
		_eliController = Object.FindObjectOfType<EliController>();
		_eliController.MoveTarget.position = _eliController.gameObject.transform.position;
		if (_eliController.Model.Health > 0f)
		{
			StartCoroutine(StartDelay());
		}
	}

	private IEnumerator StartDelay()
	{
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		yield return new WaitForSeconds(0.1f);
		RisePlayCutSceneEvent(true);
		MainCamera = GameObject.Find("MainCamera");
		UICamera = GameObject.Find("UICamera");
		_cameraFade = MainCamera.GetComponent<CameraFade>();
		_cameraSmoothFollow = MainCamera.GetComponent<CameraSmoothFollow>();
		_cameraSmoothFollow._posMixer = 2f;
		_cameraSmoothFollow._damping = 0.01f;
		gameController.PlayCutScene(true);
		UICamera.SetActive(false);
		StartCoroutine(GoPointWagon());
		StartCoroutine(TextHint_0());
	}

	private IEnumerator TextHint_0()
	{
		yield return new WaitForSeconds(0f);
		ShowScreens(true);
		_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["3_1_additional_text_02"];
	}

	private void OnEnable()
	{
		_endPoint.OnObjectiveCompleteEvent += EndPoint;
		enabledAB_2.OnObjectiveCompleteEvent += SettAttack;
	}

	private void OnDisable()
	{
		_endPoint.OnObjectiveCompleteEvent -= EndPoint;
		enabledAB_2.OnObjectiveCompleteEvent -= SettAttack;
	}

	private void Fade()
	{
		StartCoroutine(DelaySkip(0f));
	}

	private IEnumerator DelaySkip(float a)
	{
		yield return new WaitForSeconds(a);
		Skip();
	}

	private void Skip()
	{
		ShowScreens(false);
		_cameraSmoothFollow._posMixer = 0.25f;
		_cameraSmoothFollow._damping = 6f;
		_cameraFade.StopFade();
		UICamera.SetActive(true);
		DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
		gameController.PlayCutScene(false);
		RisePlayCutSceneEvent(false);
	}

	private void EndPoint()
	{
		Fade();
	}

	private IEnumerator GoPointWagon()
	{
		yield return new WaitForSeconds(2f);
		_eliController.MoveTarget.position = goPointWagon.position;
		_eliController.OnMove(8f);
	}

	private void SettAttack()
	{
		if (Mob_1 != null && AllyMob_1 != null)
		{
			Mob_1.SetAttackTarget(AllyMob_1.Transform);
		}
		if (Mob_2 != null && AllyMob_2 != null)
		{
			Mob_2.SetAttackTarget(AllyMob_2.Transform);
		}
		if (Mob_1 != null && AllyMob_1 != null)
		{
			AllyMob_1.SetAttackTarget(Mob_1.Transform);
		}
		if (Mob_2 != null && AllyMob_2 != null)
		{
			AllyMob_2.SetAttackTarget(Mob_2.Transform);
		}
	}
}

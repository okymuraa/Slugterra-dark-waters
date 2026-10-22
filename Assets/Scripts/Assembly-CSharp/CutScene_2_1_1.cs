using System.Collections;
using AppsMinistry.Core;
using AppsMinistry.Core.Input;
using AppsMinistry.UI.Camera;
using Slugterra.Game.Environment;
using Slugterra.Game.Units.Ally;
using Slugterra.Game.Units.Ally.Assistant;
using Slugterra.Game.Units.Enemy;
using Slugterra.Scene;
using Slugterra.UI.Game;
using Slugterra.UI.Game.Camera;
using UnityEngine;

public class CutScene_2_1_1 : SlugterraCutScene
{
	[SerializeField]
	private GameController gameController;

	[SerializeField]
	private Transform ElyGoTo_AB_1;

	[SerializeField]
	private Transform AllyGoTo_AB_1_1;

	[SerializeField]
	private Transform AllyGoTo_AB_1_2;

	[SerializeField]
	private Transform GoToEnemy1;

	[SerializeField]
	private Transform GoToEnemy2;

	[SerializeField]
	private ScrapHeapController Enemy1;

	[SerializeField]
	private ScrapHeapController Enemy2;

	[SerializeField]
	private GameObject AllyRadius;

	[SerializeField]
	private GameObject Coins;

	[SerializeField]
	private GameObject Pronto;

	[SerializeField]
	private AvatarInfoView AssistantHUD;

	private GameObject MainCamera;

	private GameObject UICamera;

	private EliController _eliController;

	private AssistantController _assistantController;

	private CameraFade _cameraFade;

	private CameraSmoothFollow _cameraSmoothFollow;

	private Animation _camerAnimation;

	private tk2dButton _skip;

	public ExplosionEnvironmentController barrel;

	private void Start()
	{
		RisePlayCutSceneEvent(true);
		MainCamera = GameObject.Find("MainCamera");
		UICamera = GameObject.Find("UICamera");
		_eliController = Object.FindObjectOfType<EliController>();
		_assistantController = Object.FindObjectOfType<AssistantController>();
		_camerAnimation = MainCamera.GetComponent<Animation>();
		_cameraFade = MainCamera.GetComponent<CameraFade>();
		_cameraSmoothFollow = MainCamera.GetComponent<CameraSmoothFollow>();
		AllyRadius.GetComponent<SphereCollider>().radius = 50f;
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 1f), 1f, 0.1f, 2f);
		gameController.PlayCutScene(true);
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		barrel.enabled = false;
		float a = 2f;
		StartCoroutine(TextHint_2(a));
		float a2 = 1f;
		StartCoroutine(StartCameraTrack(a2));
	}

	private IEnumerator StartCameraTrack(float a)
	{
		yield return new WaitForSeconds(a);
		UICamera.SetActive(false);
		_cutSceneView.SkipButton.targetObject = gameObject;
		_cameraSmoothFollow.enabled = false;
		_camerAnimation.Play("CameraTrack_2");
		EliGoTo();
		float time = 2.8f;
		StartCoroutine(CoinTurnOff(time));
	}

	private IEnumerator CoinTurnOff(float a)
	{
		yield return new WaitForSeconds(a);
		Coins.SetActive(false);
	}

	private IEnumerator DelaySkip(float a)
	{
		yield return new WaitForSeconds(a);
		Skip();
	}

	private void Skip()
	{
		PronroScriptTurnOff();
		EnemyMoveTo();
		AssistantHUD.gameObject.SetActive(false);
		_camerAnimation.Stop();
		_cameraSmoothFollow.enabled = true;
		UICamera.SetActive(true);
		Coins.SetActive(false);
		ShowScreens(false);
		DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
		gameController.PlayCutScene(false);
		RisePlayCutSceneEvent(false);
	}

	private IEnumerator TextHint_2(float a)
	{
		yield return new WaitForSeconds(a);
		ShowScreens(true);
		_cutSceneView.SpeakerIcon = "icon_pronto";
		_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["2_1_cutscene_2_text_01"];
		float time = 2f;
		StartCoroutine(HideTextHint_2(time));
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
		if (_isSkipEnable)
		{
			_cutSceneView.SpeakerIcon = "icon_eli";
			_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["2_1_cutscene_2_text_02"];
			EnemyGoTo();
			float a2 = 2f;
			StartCoroutine(HideTextHint_3(a2));
		}
	}

	private IEnumerator HideTextHint_3(float a)
	{
		yield return new WaitForSeconds(a);
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
			_cutSceneView.SpeakerIcon = "icon_pronto";
			_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["2_1_cutscene_2_text_03"];
			ProntoGoToOnEnd();
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
			_cutSceneView.SpeakerIcon = "icon_eli";
			_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["2_1_cutscene_2_text_04"];
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
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 1f), 1f, 1f, 1f);
		_cutSceneView.SkipButton.gameObject.SetActive(false);
		_cutSceneView.SkipButton.targetObject = null;
		float a = 1f;
		StartCoroutine(DelaySkip(a));
	}

	private void EnemyGoTo()
	{
		Enemy1.MoveTo(GoToEnemy1, 5f);
		Enemy2.MoveTo(GoToEnemy2, 5f);
	}

	private void EnemyMoveTo()
	{
		Enemy1.transform.position = GoToEnemy1.transform.position;
		Enemy1.transform.eulerAngles = GoToEnemy1.eulerAngles;
		Enemy2.transform.position = GoToEnemy2.transform.position;
		Enemy2.transform.eulerAngles = GoToEnemy2.eulerAngles;
		Enemy1.MoveTo(GoToEnemy1, 5f);
		Enemy2.MoveTo(GoToEnemy2, 5f);
	}

	private void ProntoGoToOnEnd()
	{
		if (_assistantController != null && !(_assistantController.StateMachine.CurrentState is Slugterra.Game.Units.Ally.Assistant.DeathState))
		{
			_assistantController.MoveTarget.position = AllyGoTo_AB_1_2.transform.position;
			_assistantController.OnMove(8f);
			_assistantController.enabled = false;
		}
	}

	private void PronroScriptTurnOff()
	{
		AllyRadius.GetComponent<SphereCollider>().radius = 4f;
		Pronto.SetActive(false);
		barrel.enabled = true;
		gameObject.SetActive(false);
	}

	private void EliGoTo()
	{
		_eliController.MoveTarget.transform.position = ElyGoTo_AB_1.position;
		_eliController.transform.position = ElyGoTo_AB_1.position;
		_eliController.transform.eulerAngles = ElyGoTo_AB_1.eulerAngles;
		if (_assistantController != null && !(_assistantController.StateMachine.CurrentState is Slugterra.Game.Units.Ally.Assistant.DeathState))
		{
			_assistantController.MoveTarget.position = AllyGoTo_AB_1_1.transform.position;
			_assistantController.OnMove(7f);
			_assistantController.enabled = false;
		}
	}
}

using System.Collections;
using AppsMinistry.Core;
using AppsMinistry.Core.Input;
using AppsMinistry.UI.Camera;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.Game.Units;
using Slugterra.Game.Environment;
using Slugterra.Game.Units.Ally;
using Slugterra.Game.Units.Ally.Assistant;
using Slugterra.Game.Units.Enemy;
using Slugterra.Scene;
using Slugterra.UI.Game;
using Slugterra.UI.Game.Camera;
using UnityEngine;

public class CutScene_DailyMission_01_A : SlugterraCutScene
{
	[SerializeField]
	private GameController gameController;

	public GameObject HUDPanel;

	public Transform noWay_0;

	public Transform finishVeiw;

	public Transform firePos;

	public Transform goHome;

	public GameObject fade;

	public GameObject wagon;

	public GameObject DestroyFX;

	private GameObject MainCamera;

	private GameObject UICamera;

	private CameraFade _cameraFade;

	private CameraSmoothFollow _cameraSmoothFollow;

	private ProntoController _prontoController;

	public ScrapHeapController _scrapHeapController;

	public ScrapHeapController _scrapHeapController1;

	private EliController _eliController;

	public Transform prontoPoint;

	public GameObject FinishTrigger;

	public string[] text;

	private void Start()
	{
		RisePlayCutSceneEvent(true);
		gameController.PlayCutScene(true);
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		MainCamera = GameObject.Find("MainCamera");
		UICamera = GameObject.Find("UICamera");
		UICamera.GetComponent<Camera>().enabled = false;
		HUDPanel.SetActive(false);
		if (MonoSingleton<AppController>.Instance.Model.MissionSetting.Assistant.Type == AllyType.Pronto)
		{
			_prontoController = Object.FindObjectOfType<ProntoController>();
		}
		_eliController = Object.FindObjectOfType<EliController>();
		_cutSceneView.SkipButton.targetObject = gameObject;
		_cameraFade = MainCamera.GetComponent<CameraFade>();
		_cameraSmoothFollow = MainCamera.GetComponent<CameraSmoothFollow>();
		_cameraSmoothFollow.enabled = false;
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 1f), 1f, 0.1f, 2f);
		wagon.GetComponent<ExplosionEnvironmentController>().enabled = false;
		if (MonoSingleton<AppController>.Instance.Model.MissionSetting.Assistant.Type == AllyType.None)
		{
			StartCoroutine("TextHint_eli");
		}
		else if (_prontoController != null)
		{
			_prontoController.transform.position = prontoPoint.position;
			StartCoroutine("TextHint_add");
		}
		else
		{
			StartCoroutine("TextHint_base");
		}
		StartCoroutine(AfterStart(0.1f));
	}

	private IEnumerator AfterStart(float t)
	{
		yield return new WaitForSeconds(t);
		fade.SetActive(false);
	}

	public void Fade()
	{
		ShowScreens(false);
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 0f), 1f, 0.5f, 2f);
		_cutSceneView.SkipButton.gameObject.SetActive(false);
		_isSkipEnable = false;
		StartCoroutine(_StartGame(0.5f));
	}

	public void NoWay()
	{
		gameController.PlayCutScene(true);
		_eliController.SetAttackTarget(null);
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 0f), 1f, 0.5f, 2f);
		UICamera.GetComponent<Camera>().enabled = false;
		HUDPanel.SetActive(false);
		StartCoroutine(_noWay_0(0.5f));
	}

	public void FixedUpdate()
	{
		if (FinishTrigger.activeSelf)
		{
			FinishTrigger.SetActive(false);
			_eliController.SetAttackTarget(null);
			_eliController.MoveTarget.position = _eliController.transform.position;
			gameController.PlayCutScene(true);
			DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
			_cameraFade.StartFade(new Color(0f, 0f, 0f, 0f), 1f, 0.5f, 2f);
			UICamera.GetComponent<Camera>().enabled = false;
			HUDPanel.SetActive(false);
			StartCoroutine(_finish(0.5f));
		}
	}

	private IEnumerator _noWay_0(float t)
	{
		yield return new WaitForSeconds(t);
		_cameraSmoothFollow.enabled = false;
		MainCamera.transform.position = noWay_0.position;
		MainCamera.transform.eulerAngles = noWay_0.eulerAngles;
		StartCoroutine(TextHint(2f, "icon_eli", text[3]));
	}

	private IEnumerator _finish(float t)
	{
		yield return new WaitForSeconds(t);
		_cameraSmoothFollow.enabled = false;
		MainCamera.transform.position = finishVeiw.position;
		MainCamera.transform.eulerAngles = finishVeiw.eulerAngles;
		StartCoroutine(_finish_1(0.5f));
	}

	private IEnumerator _finish_1(float t)
	{
		yield return new WaitForSeconds(t);
		_eliController.MoveTarget.position = firePos.position;
		_eliController.OnMove(5f);
		StartCoroutine(TextHint_finish(2f));
	}

	public void GoHome()
	{
		if (wagon != null)
		{
			wagon.GetComponent<ExplosionEnvironmentController>().OnDamage(10f);
			Object.Destroy(wagon);
		}
		ShowScreens(false);
		_eliController.MoveTarget.position = goHome.position;
		_scrapHeapController.MoveTarget.position = goHome.position;
		_scrapHeapController1.MoveTarget.position = goHome.position;
		_eliController.OnMove(5f);
		_scrapHeapController.OnMove(5f);
		_scrapHeapController1.OnMove(7f);
		_isSkipEnable = false;
		_cutSceneView.SkipButton.gameObject.SetActive(false);
		StartCoroutine(GoHomeFade(2.5f));
	}

	private IEnumerator GoHomeFade(float t)
	{
		yield return new WaitForSeconds(t);
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 0f), 1f, 1f, 2f);
		StartCoroutine(GoHomeFade1(1f));
	}

	private IEnumerator GoHomeFade1(float t)
	{
		yield return new WaitForSeconds(t);
		DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
		fade.SetActive(true);
		UICamera.GetComponent<Camera>().enabled = true;
		HUDPanel.SetActive(true);
		gameController.OnFinishGame();
		gameController.PlayCutScene(false);
		RisePlayCutSceneEvent(false);
		gameObject.SetActive(false);
	}

	private IEnumerator _StartGame(float t)
	{
		yield return new WaitForSeconds(t);
		_cameraSmoothFollow.enabled = true;
		DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
		gameController.PlayCutScene(false);
		RisePlayCutSceneEvent(false);
		UICamera.GetComponent<Camera>().enabled = true;
		HUDPanel.SetActive(true);
	}

	private IEnumerator TextHint_eli()
	{
		yield return new WaitForSeconds(2f);
		ShowScreens(true);
		_cutSceneView.SpeakerIcon = "icon_eli";
		_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance[text[0]];
		StartCoroutine(HideTextHint_0(4f));
		_isSkipEnable = true;
		_cutSceneView.SkipButton.gameObject.SetActive(true);
	}

	private IEnumerator TextHint_base()
	{
		yield return new WaitForSeconds(2f);
		ShowScreens(true);
		if (MonoSingleton<AppController>.Instance.Model.MissionSetting.Assistant.Type == AllyType.Kord)
		{
			_cutSceneView.SpeakerIcon = "ico_kord";
		}
		if (MonoSingleton<AppController>.Instance.Model.MissionSetting.Assistant.Type == AllyType.Trixie)
		{
			_cutSceneView.SpeakerIcon = "icon_trixie";
		}
		_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance[text[1]];
		StartCoroutine(HideTextHint_0(4f));
		_isSkipEnable = true;
		_cutSceneView.SkipButton.gameObject.SetActive(true);
	}

	private IEnumerator TextHint_add()
	{
		yield return new WaitForSeconds(2f);
		ShowScreens(true);
		_cutSceneView.SpeakerIcon = "icon_pronto";
		_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance[text[2]];
		StartCoroutine(HideTextHint_0(4f));
		_isSkipEnable = true;
		_cutSceneView.SkipButton.gameObject.SetActive(true);
	}

	private IEnumerator TextHint_finish(float t)
	{
		yield return new WaitForSeconds(t);
		_cutSceneView.SkipButton.messageName = "GoHome";
		ShowScreens(true);
		_cutSceneView.SpeakerIcon = "icon_eli";
		_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance[text[4]];
		wagon.GetComponent<ExplosionEnvironmentController>().enabled = true;
		SlugType type = MonoSingleton<AppController>.Instance.Model.MissionSetting.AttackSlug.Type;
		if (type != SlugType.None)
		{
			Humanoid humanoid = _eliController.Model as Humanoid;
			humanoid.Clear(Time.time);
			_eliController.OnUseSlug(type);
			_eliController.AttackTarget = wagon.transform;
		}
		StartCoroutine(HideTextHint_1(4f));
		_isSkipEnable = true;
		_cutSceneView.SkipButton.gameObject.SetActive(true);
	}

	private IEnumerator TextHint(float t, string icon, string key)
	{
		yield return new WaitForSeconds(t);
		gameController.PlayCutScene(true);
		UICamera.GetComponent<Camera>().enabled = false;
		HUDPanel.SetActive(false);
		_cutSceneView.ShowScreens(true);
		_cutSceneView.SpeakerIcon = icon;
		_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance[key];
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		_cutSceneView.SkipButton.gameObject.SetActive(true);
		_isSkipEnable = true;
		StartCoroutine(HideTextHint_0(4f));
	}

	private IEnumerator HideTextHint_0(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			ShowScreens(false);
			_isSkipEnable = false;
			_cutSceneView.SkipButton.gameObject.SetActive(false);
			Fade();
		}
	}

	private IEnumerator HideTextHint_1(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			if (!wagon.activeSelf)
			{
				Object.Destroy(wagon);
			}
			ShowScreens(false);
			_isSkipEnable = false;
			_cutSceneView.SkipButton.gameObject.SetActive(false);
			GoHome();
		}
	}
}

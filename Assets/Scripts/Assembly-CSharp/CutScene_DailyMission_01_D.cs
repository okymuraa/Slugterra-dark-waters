using System.Collections;
using AppsMinistry.Core;
using AppsMinistry.Core.Input;
using AppsMinistry.UI.Camera;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Game.Units.Ally;
using Slugterra.Game.Units.Ally.Assistant;
using Slugterra.Game.Units.Enemy;
using Slugterra.Scene;
using Slugterra.UI.Game;
using Slugterra.UI.Game.Camera;
using UnityEngine;

public class CutScene_DailyMission_01_D : SlugterraCutScene
{
	private InGameView _inGameView;

	private CountdownWarningMessageView _countdownMessage;

	[SerializeField]
	private GameController gameController;

	public GameObject HUDPanel;

	public Transform view_part_1;

	public Transform eli_part_1;

	public Transform ass_part_1;

	private GameObject MainCamera;

	private GameObject UICamera;

	private CameraFade _cameraFade;

	public GameObject _fade;

	private CameraSmoothFollow _cameraSmoothFollow;

	public GameObject finish;

	public GameObject _waveController;

	private EliController _eliController;

	public AssistantController _assController;

	public Transform _start;

	public ScrapHeapController[] _scrapHeapController;

	public ScrapHeapController[] _scrapHeapSocial;

	public Transform[] pointCutScene_01;

	public Transform[] node;

	public string[] text;

	private float timer;

	private bool part_0;

	private bool part_1;

	private bool part_1_notTarget;

	private bool part_2;

	private void Start()
	{
		_inGameView = Object.FindObjectOfType<InGameView>();
		_countdownMessage = _inGameView.CountdownWarningMessage;
		gameController.PlayCutScene(true);
		_waveController.SetActive(false);
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		MainCamera = GameObject.Find("MainCamera");
		UICamera = GameObject.Find("UICamera");
		UICamera.GetComponent<Camera>().enabled = false;
		HUDPanel.SetActive(false);
		_eliController = Object.FindObjectOfType<EliController>();
		_assController = Object.FindObjectOfType<AssistantController>();
		_cutSceneView.SkipButton.targetObject = gameObject;
		_cameraFade = MainCamera.GetComponent<CameraFade>();
		_cameraSmoothFollow = MainCamera.GetComponent<CameraSmoothFollow>();
		_cameraSmoothFollow.enabled = false;
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 1f), 1f, 0.1f, 2f);
		part_0 = true;
		if (MonoSingleton<AppController>.Instance.Model.MissionSetting.Assistant.Type == AllyType.None)
		{
			StartCoroutine(TextHint(2f, "icon_eli", text[1]));
		}
		else
		{
			StartCoroutine(TextHint(2f, "icon_eli", text[2]));
		}
		StartCoroutine(AfterStart(0.1f));
	}

	private void FixedUpdate()
	{
		if (finish.activeSelf)
		{
			finish.SetActive(false);
			part_1_notTarget = false;
			StartCoroutine(_part_2(3f));
		}
		if (part_1)
		{
			_eliController.transform.eulerAngles = Vector3.up * 0f;
		}
		if (!part_1_notTarget)
		{
			return;
		}
		timer -= Time.deltaTime;
		if (timer < 0f && _scrapHeapSocial[0] == null && _scrapHeapSocial[1] == null)
		{
			for (int i = 0; i < node.Length; i++)
			{
				node[i].position = _eliController.transform.position;
			}
			timer = 5f;
		}
	}

	private IEnumerator AfterStart(float t)
	{
		yield return new WaitForSeconds(t);
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		_fade.SetActive(false);
		for (int i = 0; i < _scrapHeapController.Length; i++)
		{
			_scrapHeapController[i].MoveTarget.position = pointCutScene_01[i].position;
			_scrapHeapController[i].OnMove(5f);
		}
		for (int j = 0; j < _scrapHeapSocial.Length; j++)
		{
			_scrapHeapSocial[j].gameObject.SetActive(false);
		}
	}

	public void Fade()
	{
		ShowScreens(false);
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 0f), 1f, 0.5f, 2f);
		_cutSceneView.SkipButton.gameObject.SetActive(false);
		_isSkipEnable = false;
		if (part_0)
		{
			for (int i = 0; i < _scrapHeapController.Length; i++)
			{
				_scrapHeapController[i].MoveTarget.position = _start.position;
				_scrapHeapController[i].OnMove(5f);
				Object.Destroy(_scrapHeapController[i].gameObject, 2f + (float)i / 10f);
			}
			StartCoroutine(_part_0(0.5f));
			part_0 = false;
		}
		if (part_1)
		{
			StartCoroutine(_part_1(0.5f));
			part_1 = false;
			part_1_notTarget = true;
		}
		if (part_2)
		{
			part_2 = false;
			StartCoroutine(_part_2_End(3f));
		}
	}

	public void MessageStart()
	{
		StartCoroutine(MessageShowDeffend());
	}

	private IEnumerator MessageShowDeffend()
	{
		_countdownMessage.Show(true);
		_countdownMessage.Description = MonoSingleton<Localization>.Instance[text[0]];
		yield return new WaitForSeconds(5f);
		_countdownMessage.Show(false);
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

	private IEnumerator _part_0(float t)
	{
		yield return new WaitForSeconds(t);
		_cameraSmoothFollow.enabled = true;
		DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
		gameController.PlayCutScene(false);
		UICamera.GetComponent<Camera>().enabled = true;
		HUDPanel.SetActive(true);
	}

	public void Part_1()
	{
		for (int i = 0; i < _scrapHeapSocial.Length; i++)
		{
			_scrapHeapSocial[i].gameObject.SetActive(true);
		}
		_waveController.SetActive(true);
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 0f), 1f, 0.5f, 2f);
		StartCoroutine(_add_0_part_1(0.5f));
	}

	private IEnumerator _add_0_part_1(float t)
	{
		yield return new WaitForSeconds(t);
		gameController.PlayCutScene(true);
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		_cameraSmoothFollow.enabled = false;
		MainCamera.transform.position = view_part_1.position;
		MainCamera.transform.eulerAngles = view_part_1.eulerAngles;
		UICamera.GetComponent<Camera>().enabled = false;
		HUDPanel.SetActive(false);
		_eliController.MoveTarget.position = eli_part_1.position;
		_eliController.transform.position = eli_part_1.position;
		if ((bool)_assController)
		{
			_assController.MoveTarget.position = ass_part_1.position;
			_assController.transform.position = ass_part_1.position;
		}
		part_1 = true;
		StartCoroutine(TextHint(0.5f, "icon_eli", text[3]));
		yield return new WaitForSeconds(0.1f);
		if ((bool)_scrapHeapSocial[0] && (bool)_scrapHeapSocial[2])
		{
			_scrapHeapSocial[0].SetViewRadius(12f);
			_scrapHeapSocial[2].SetViewRadius(12f);
		}
	}

	private IEnumerator _part_1(float t)
	{
		yield return new WaitForSeconds(t);
		_cameraSmoothFollow.enabled = true;
		DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
		gameController.PlayCutScene(false);
		if ((bool)_scrapHeapSocial[0] && (bool)_scrapHeapSocial[2])
		{
			_scrapHeapSocial[2].SetAttackTarget(_scrapHeapSocial[0].gameObject.transform);
			_scrapHeapSocial[0].SetAttackTarget(_scrapHeapSocial[2].gameObject.transform);
		}
		UICamera.GetComponent<Camera>().enabled = true;
		HUDPanel.SetActive(true);
	}

	private IEnumerator _part_2(float t)
	{
		yield return new WaitForSeconds(t);
		part_2 = true;
		if (_scrapHeapSocial[0] == null && _scrapHeapSocial[1] == null)
		{
			StartCoroutine(TextHint(0.5f, "icon_eli", text[5]));
		}
		else
		{
			StartCoroutine(TextHint(0.5f, "icon_eli", text[4]));
		}
	}

	private IEnumerator _part_2_End(float t)
	{
		yield return new WaitForSeconds(t);
		UICamera.GetComponent<Camera>().enabled = true;
		HUDPanel.SetActive(true);
		gameController.OnFinishGame();
		gameController.PlayCutScene(false);
		gameObject.SetActive(false);
	}
}

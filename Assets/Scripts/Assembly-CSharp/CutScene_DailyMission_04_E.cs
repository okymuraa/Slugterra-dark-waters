using System.Collections;
using AppsMinistry.Core;
using AppsMinistry.Core.Input;
using AppsMinistry.UI.Camera;
using Slugterra.Game.Units.Ally.Assistant;
using Slugterra.Scene;
using Slugterra.UI.Game;
using Slugterra.UI.Game.Camera;
using UnityEngine;

public class CutScene_DailyMission_04_E : SlugterraCutScene
{
	[SerializeField]
	private GameController gameController;

	public GameObject HUDPanel;

	private GameObject MainCamera;

	private GameObject UICamera;

	private CameraFade _cameraFade;

	public GameObject _fade;

	private CameraSmoothFollow _cameraSmoothFollow;

	public GameObject finish;

	private bool _completedMain;

	public string[] text;

	public Transform finishPoint;

	public float radius;

	public FollowerController[] UnitsEscort;

	private byte u;

	private bool part_0;

	private void Start()
	{
		gameController.PlayCutScene(true);
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		MainCamera = GameObject.Find("MainCamera");
		UICamera = GameObject.Find("UICamera");
		UICamera.GetComponent<Camera>().enabled = false;
		HUDPanel.SetActive(false);
		_cutSceneView.SkipButton.targetObject = gameObject;
		_cameraFade = MainCamera.GetComponent<CameraFade>();
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 1f), 1f, 0.1f, 2f);
		_cameraSmoothFollow = MainCamera.GetComponent<CameraSmoothFollow>();
		_cameraSmoothFollow.enabled = false;
		part_0 = true;
		StartCoroutine(TextHint(2f, "icon_eli", text[0]));
		StartCoroutine(AfterStart(0.1f));
	}

	public void FindAfterRes(FollowerController[] _unitsEscort)
	{
		UnitsEscort = _unitsEscort;
	}

	private void FixedUpdate()
	{
		if (finish.activeSelf)
		{
			_completedMain = true;
			for (u = 0; u < UnitsEscort.Length; u++)
			{
				if (UnitsEscort[u] != null && Vector3.Distance(UnitsEscort[u].transform.position, finishPoint.position) > radius)
				{
					_completedMain = false;
				}
			}
		}
		if (finish.activeSelf && _completedMain)
		{
			finish.SetActive(false);
			StartCoroutine(_part_End(2.5f));
		}
	}

	private IEnumerator AfterStart(float t)
	{
		yield return new WaitForSeconds(t);
		_fade.SetActive(false);
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
	}

	public void Fade()
	{
		ShowScreens(false);
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 0f), 1f, 0.5f, 2f);
		_cutSceneView.SkipButton.gameObject.SetActive(false);
		_isSkipEnable = false;
		if (part_0)
		{
			StartCoroutine(_part_0(0.5f));
			part_0 = false;
		}
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

	private IEnumerator _part_End(float t)
	{
		yield return new WaitForSeconds(t);
		UICamera.GetComponent<Camera>().enabled = true;
		HUDPanel.SetActive(true);
		gameController.OnFinishGame();
		gameController.PlayCutScene(false);
		gameObject.SetActive(false);
	}
}

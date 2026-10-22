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

public class CutScene_DailyMission_02_D : SlugterraCutScene
{
	private InGameView _inGameView;

	private CountdownWarningMessageView _countdownMessage;

	[SerializeField]
	private GameController gameController;

	public GameObject HUDPanel;

	private GameObject MainCamera;

	private GameObject UICamera;

	private CameraFade _cameraFade;

	public GameObject _fade;

	private CameraSmoothFollow _cameraSmoothFollow;

	public GameObject finish;

	public Transform Objective_100;

	public Transform Objective_50;

	private EliController _eliController;

	public ScrapHeapController[] FirstBot;

	public Transform _BotFinish_1;

	public Transform _BotFinish_2;

	public GameObject[] Box;

	private int boxCount;

	private int i;

	public Transform[] node;

	public string[] text;

	private float timer;

	private bool part_0;

	private void Start()
	{
		_inGameView = Object.FindObjectOfType<InGameView>();
		_countdownMessage = _inGameView.CountdownWarningMessage;
		gameController.PlayCutScene(true);
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		MainCamera = GameObject.Find("MainCamera");
		UICamera = GameObject.Find("UICamera");
		UICamera.GetComponent<Camera>().enabled = false;
		HUDPanel.SetActive(false);
		_eliController = Object.FindObjectOfType<EliController>();
		_cutSceneView.SkipButton.targetObject = gameObject;
		_cameraFade = MainCamera.GetComponent<CameraFade>();
		_cameraSmoothFollow = MainCamera.GetComponent<CameraSmoothFollow>();
		_cameraSmoothFollow.enabled = false;
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 1f), 1f, 0.1f, 2f);
		part_0 = true;
		StartCoroutine(TextHint(2f, "icon_eli", text[1]));
		StartCoroutine(AfterStart(0.1f));
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

	private void FixedUpdate()
	{
		if (finish.activeSelf)
		{
			finish.SetActive(false);
			boxCount = 0;
			for (i = 0; i < Box.Length; i++)
			{
				if (Box[i].activeSelf)
				{
					boxCount++;
				}
			}
			if (boxCount >= 10)
			{
				Object.DestroyImmediate(Objective_50.gameObject);
				Objective_100.position = _eliController.transform.position;
			}
			else if (boxCount != 0)
			{
				Object.DestroyImmediate(Objective_100.gameObject);
				Objective_50.position = _eliController.transform.position;
			}
			else
			{
				Object.DestroyImmediate(Objective_100.gameObject);
			}
			StartCoroutine(_part_End(2.5f));
		}
		if (Box.Length <= 0)
		{
			return;
		}
		boxCount = 0;
		for (i = 0; i < Box.Length; i++)
		{
			if (!Box[i].activeSelf)
			{
				boxCount++;
			}
		}
		if (boxCount != Box.Length)
		{
			return;
		}
		timer -= Time.deltaTime;
		if (timer < 0f)
		{
			for (i = 0; i < node.Length; i++)
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
		for (i = 0; i < FirstBot.Length; i++)
		{
			if (i == 0)
			{
				FirstBot[i].MoveTarget.position = _BotFinish_1.position;
			}
			else
			{
				FirstBot[i].MoveTarget.position = _BotFinish_2.position;
			}
			FirstBot[i].OnMove(4f);
		}
		_fade.SetActive(false);
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

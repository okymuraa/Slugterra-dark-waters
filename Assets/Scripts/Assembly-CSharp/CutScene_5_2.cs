using System.Collections;
using AppsMinistry.Core;
using AppsMinistry.Core.Input;
using AppsMinistry.UI.Camera;
using Slugterra.Game.Units.Ally;
using Slugterra.Game.Units.Enemy;
using Slugterra.Game.Units.Mechanisms.Spline;
using Slugterra.Scene;
using Slugterra.UI.Game;
using Slugterra.UI.Game.Camera;
using UnityEngine;

public class CutScene_5_2 : SlugterraCutScene
{
	public enum CutScenePart
	{
		Part_0 = 0,
		Part_1 = 1,
		Part_2 = 2,
		Part_3 = 3,
		Part_4 = 4,
		Part_5 = 5,
		Part_6 = 6,
		Part_7 = 7
	}

	public bool CutSceneUse;

	public CutScenePart _cutScenePart;

	private bool Part_0;

	private bool Part_1;

	private bool Part_2;

	private bool Part_3;

	private bool Part_4;

	private bool Part_5;

	private bool Part_6;

	private bool Part_7;

	[SerializeField]
	private GameController gameController;

	public GameObject HUDPanel;

	private GameObject MainCamera;

	private GameObject UICamera;

	private CameraFade _cameraFade;

	public GameObject _fade;

	private CameraSmoothFollow _cameraSmoothFollow;

	private TeamController _trailer;

	private EliController _eliController;

	public GameObject finish;

	public AnimationClip AninationStartScene;

	public Transform[] Objective;

	public string[] text;

	private bool part_0;

	private bool part_1;

	private bool part_2;

	private bool part_3;

	private bool part_4;

	private bool part_5;

	public GameObject zone;

	public GameObject WaveController_AB_1;

	public GameObject TowerDoor;

	public GameObject TowerGate;

	public SentinelController[] Bandit;

	private int g;

	private int count;

	private float StartHealh;

	public AnimationClip BanditView;

	public AnimationClip FinishView;

	public AnimationClip TowerDoorOpen;

	public AnimationClip TowerDoorClose;

	public AnimationClip TowerGateOpen;

	public AnimationClip TowerGateClose;

	private void Start()
	{
		gameController.PlayCutScene(true);
		CutSceneUse = true;
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
		if (_cutScenePart == CutScenePart.Part_0)
		{
			part_0 = true;
			if ((bool)AninationStartScene)
			{
				MainCamera.GetComponent<Animation>().Play(AninationStartScene.name);
				StartCoroutine(TextHint(2f, AninationStartScene.length - 3f, "icon_eli", text[0]));
			}
			else
			{
				StartCoroutine(TextHint(2f, 4f, "icon_eli", text[0]));
			}
			StartCoroutine(AnimationPlay(TowerDoor, 2f, TowerDoorOpen.name));
			StartCoroutine(AnimationPlay(TowerGate, 1f, TowerGateOpen.name));
			StartCoroutine(AnimationPlay(TowerDoor, 0.45f, TowerDoorClose.name, 1.25f));
			StartCoroutine(AnimationPlay(TowerGate, 1f, TowerGateClose.name, 3.5f));
		}
		_trailer = Object.FindObjectOfType<TeamController>();
		_trailer.SetSpeed(0.45f, 0.8f, 1.8f);
		_trailer.OnMove();
		WaveController_AB_1.SetActive(false);
		StartHealh = Bandit[0].Model.Health;
		StartCoroutine(AfterStart(0.1f));
	}

	private void FixedUpdate()
	{
		if (finish.activeSelf)
		{
			finish.SetActive(false);
			StartCoroutine(_part_End_new(3f));
		}
	}

	private IEnumerator AfterStart(float t)
	{
		yield return new WaitForSeconds(t);
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		for (int i = 0; i < Objective.Length; i++)
		{
			if (Objective[i] != null)
			{
				Objective[i].gameObject.SetActive(false);
			}
		}
		_fade.SetActive(false);
	}

	public void Fade()
	{
		ShowScreens(false);
		if (!_cameraSmoothFollow.enabled)
		{
			_cameraFade.StartFade(new Color(0f, 0f, 0f, 0f), 1f, 0.5f, 2f);
		}
		_cutSceneView.SkipButton.gameObject.SetActive(false);
		_isSkipEnable = false;
		if (part_0)
		{
			StartCoroutine(_part_0(0.5f));
			_cutScenePart = CutScenePart.Part_1;
			part_0 = false;
		}
		if (part_1)
		{
			StopAllCoroutines();
			StartCoroutine(_part_1_skip(0.5f));
			StartCoroutine(_part_0(0.5f));
			part_1 = false;
		}
		if (part_2)
		{
			StopAllCoroutines();
			StartCoroutine(_part_2_skip(0.5f));
			StartCoroutine(_part_0(0.5f));
			part_2 = false;
		}
	}

	private IEnumerator TextHint(float t, float d, string icon, string key)
	{
		yield return new WaitForSeconds(t);
		gameController.PlayCutScene(true);
		CutSceneUse = true;
		UICamera.GetComponent<Camera>().enabled = false;
		HUDPanel.SetActive(false);
		_cutSceneView.ShowScreens(true);
		_cutSceneView.SpeakerIcon = icon;
		_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance[key];
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		_cutSceneView.SkipButton.gameObject.SetActive(true);
		_isSkipEnable = true;
		if (d != 0f)
		{
			StartCoroutine(HideTextHint_0(d));
		}
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

	private IEnumerator ShowUIAuto(float weit)
	{
		yield return new WaitForSeconds(weit);
		gameController.PlayCutScene(false);
		CutSceneUse = false;
		HUDPanel.SetActive(true);
		UICamera.GetComponent<Camera>().enabled = true;
		if (!zone.activeSelf && _cutScenePart != CutScenePart.Part_3)
		{
			zone.SetActive(true);
		}
	}

	private IEnumerator AnimationPlay(GameObject obj, float speed = 1f, string name = null, float weit = 0f)
	{
		yield return new WaitForSeconds(weit);
		if (speed != 1f)
		{
			obj.GetComponent<Animation>()[name].speed = speed;
		}
		obj.GetComponent<Animation>().Play(name);
	}

	private IEnumerator _part_0(float t, float s = 1.5f)
	{
		yield return new WaitForSeconds(t);
		if ((bool)MainCamera.GetComponent<Animation>())
		{
			MainCamera.GetComponent<Animation>().Stop();
		}
		_cameraSmoothFollow.enabled = true;
		DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
		StartCoroutine(ShowUIAuto(s));
	}

	private IEnumerator _part_End(float t)
	{
		yield return new WaitForSeconds(t);
		DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
		UICamera.GetComponent<Camera>().enabled = true;
		HUDPanel.SetActive(true);
		gameController.OnFinishGame();
		gameController.PlayCutScene(false);
		CutSceneUse = false;
		gameObject.SetActive(false);
	}

	public void BanditWeit()
	{
		if (_cutScenePart != CutScenePart.Part_2)
		{
			_cutScenePart = CutScenePart.Part_2;
			gameController.PlayCutScene(true);
			CutSceneUse = true;
			DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
			_cameraSmoothFollow.enabled = false;
			UICamera.GetComponent<Camera>().enabled = false;
			HUDPanel.SetActive(false);
			_eliController.MoveTarget.position = _eliController.transform.position;
			_eliController.AttackTarget = null;
			_cameraFade.StartFade(new Color(0f, 0f, 0f, 0f), 1f, 0.5f, 2f);
			StartCoroutine(_part_1_start(0.5f));
			part_1 = true;
		}
	}

	private void BanditAttack()
	{
		if (_cutScenePart != CutScenePart.Part_2)
		{
			_cutScenePart = CutScenePart.Part_2;
			gameController.PlayCutScene(true);
			CutSceneUse = true;
			DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
			_cameraSmoothFollow.enabled = false;
			UICamera.GetComponent<Camera>().enabled = false;
			HUDPanel.SetActive(false);
			_eliController.MoveTarget.position = _eliController.transform.position;
			_eliController.AttackTarget = null;
			_cameraFade.StartFade(new Color(0f, 0f, 0f, 0f), 1f, 0.5f, 2f);
			StartCoroutine(_part_1_start(0.5f));
			part_1 = true;
		}
	}

	private IEnumerator _part_End_new(float t)
	{
		yield return new WaitForSeconds(t);
		gameController.PlayCutScene(true);
		CutSceneUse = true;
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		_cameraSmoothFollow.enabled = false;
		UICamera.GetComponent<Camera>().enabled = false;
		HUDPanel.SetActive(false);
		_eliController.MoveTarget.position = _eliController.transform.position;
		_eliController.AttackTarget = null;
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 0f), 1f, 0.5f, 2f);
		StartCoroutine(_part_2_start(0.5f));
		_cutScenePart = CutScenePart.Part_3;
		zone.SetActive(false);
		part_2 = true;
	}

	private IEnumerator _part_1_start(float t)
	{
		yield return new WaitForSeconds(t);
		MainCamera.GetComponent<Animation>().Play(BanditView.name);
		StartCoroutine(TextHint(1f, BanditView.length - 3f, "icon_sentinel", text[1]));
	}

	private IEnumerator _part_1_skip(float t)
	{
		yield return new WaitForSeconds(t);
		WaveController_AB_1.SetActive(true);
	}

	private IEnumerator _part_2_start(float t)
	{
		yield return new WaitForSeconds(t);
		MainCamera.GetComponent<Animation>().Play(FinishView.name);
		StartCoroutine(TextHint(3f, FinishView.length - 2f, "icon_eli", text[2]));
	}

	private IEnumerator _part_2_skip(float t)
	{
		yield return new WaitForSeconds(t);
		StartCoroutine(_part_End(5.5f));
	}

	private void Update()
	{
		if (_cutScenePart != CutScenePart.Part_1)
		{
			return;
		}
		count = 0;
		for (g = 0; g < Bandit.Length; g++)
		{
			if (Bandit[g].Model.Health < StartHealh)
			{
				count++;
			}
		}
		if (count > 0)
		{
			BanditAttack();
			_cutScenePart = CutScenePart.Part_2;
		}
	}
}

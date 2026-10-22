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

public class CutScene_6_2 : SlugterraCutScene
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

	private EliController _eliController;

	private TwistController _twistController;

	private NachoController _nachoController;

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

	private bool part_6;

	public Transform wist_gate_point_0;

	public SentinelController[] bandit_part_0;

	public GameObject wave_part_0_1;

	public GameObject wave_part_0_2;

	public GameObject wave_part_0_3;

	public GameObject _wave_part_0_1;

	public GameObject _wave_part_0_2;

	public GameObject _wave_part_0_3;

	public Transform wist_go_point_0;

	public GameObject TowerGate_1;

	public GameObject TowerGate_2;

	public GameObject TowerGate_3;

	public AnimationClip OpenDoorView_1;

	public AnimationClip GoGate_2;

	public Transform wist_gate_point_1;

	public Transform wist_go_point_1;

	public AnimationClip GoGate_3;

	public Transform wist_gate_point_2;

	public AnimationClip GoGate_4;

	public Transform nacho_gate_point_0;

	public Transform wist_go_point_2;

	public AnimationClip TowerGateOpen;

	public AnimationClip TowerGateClose;

	public SentinelController[] bandit_part_1;

	public Transform[] bandit_part_1_point;

	public Transform[] WavePointEnd;

	private float timer;

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
		_twistController = Object.FindObjectOfType<TwistController>();
		_nachoController = Object.FindObjectOfType<NachoController>();
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
				StartCoroutine(TextHint(2f, AninationStartScene.length - 2f, "icon_twist", text[0]));
				StartCoroutine(MoveTargetEnemy(_twistController, 4f, wist_gate_point_0.position, 6f));
				StartCoroutine(TextHint(8f, 0f, "icon_sentinel", text[1]));
				StartCoroutine(TextHint(14f, 0f, "icon_sentinel", text[2]));
				StartCoroutine(SetActiveTrue(wave_part_0_1, 16f));
				for (int i = 0; i < bandit_part_0.Length; i++)
				{
					StartCoroutine(MoveTargetEnemy(bandit_part_0[i], 4f, _eliController.transform.position, 16.5f));
				}
			}
			else
			{
				StartCoroutine(TextHint(2f, 4f, "icon_eli", text[0]));
			}
		}
		if (_cutScenePart == CutScenePart.Part_2)
		{
			for (int j = 0; j < bandit_part_0.Length; j++)
			{
				bandit_part_0[j].OnDamage(10000f, null);
			}
			_twistController.transform.position = wist_gate_point_0.position;
			StartCoroutine(_part_0(0.11f));
		}
		if (_cutScenePart == CutScenePart.Part_5)
		{
			_twistController.transform.position = wist_go_point_1.position;
			_eliController.transform.position = wist_gate_point_1.position;
			StartCoroutine(AnimationPlay(TowerGate_1, 1f, TowerGateOpen.name));
			StartCoroutine(AnimationPlay(TowerGate_2, 1f, TowerGateOpen.name));
			StartCoroutine(_part_0(0.11f));
		}
		StartCoroutine(AfterStart(0.1f));
	}

	private void FixedUpdate()
	{
		if (finish.activeSelf)
		{
			finish.SetActive(false);
			StartCoroutine(_part_End(0.5f));
		}
	}

	private IEnumerator AfterStart(float t)
	{
		yield return new WaitForSeconds(t);
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
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
			StopAllCoroutines();
			StartCoroutine(_part_0_skip(0.5f));
			StartCoroutine(_part_0(0.5f));
			part_0 = false;
		}
		if (part_1)
		{
			StopAllCoroutines();
			StartCoroutine(_part_1_skip(0.5f));
			StartCoroutine(_part_0(0.5f, 0f));
			part_1 = false;
		}
		if (part_2)
		{
			StopAllCoroutines();
			StartCoroutine(_part_2_skip(0.5f));
			StartCoroutine(_part_0(0.5f));
			part_2 = false;
		}
		if (part_3)
		{
			StopAllCoroutines();
			StartCoroutine(_part_3_skip(0.5f));
			StartCoroutine(_part_0(0.5f));
			part_3 = false;
		}
		if (part_4)
		{
			StopAllCoroutines();
			StartCoroutine(_part_4_skip(0.5f));
			StartCoroutine(_part_0(0.5f, 0f));
			part_4 = false;
		}
		if (part_5)
		{
			StopAllCoroutines();
			StartCoroutine(_part_5_skip(0.5f));
			StartCoroutine(_part_0(0.5f));
			part_5 = false;
		}
		if (part_6)
		{
			StopAllCoroutines();
			StartCoroutine(_part_6_skip(0.5f));
			StartCoroutine(_part_0(0.5f));
			part_6 = false;
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

	private IEnumerator MoveTargetEnemy(EnemyController indexController, float speed, Vector3 target, float weit = 0f)
	{
		yield return new WaitForSeconds(weit);
		if (indexController is TwistController || indexController is NachoController)
		{
			indexController.MoveTarget.position = target;
			indexController.OnMove(speed);
		}
		else
		{
			indexController.MoveTo(target, speed);
		}
	}

	private IEnumerator SetActiveTrue(GameObject obj, float weit)
	{
		yield return new WaitForSeconds(weit);
		obj.SetActive(true);
	}

	private IEnumerator SetActiveFalse(GameObject obj, float weit)
	{
		yield return new WaitForSeconds(weit);
		obj.SetActive(false);
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

	public void Gate_2()
	{
		gameController.PlayCutScene(true);
		CutSceneUse = true;
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		_cameraSmoothFollow.enabled = false;
		UICamera.GetComponent<Camera>().enabled = false;
		HUDPanel.SetActive(false);
		_eliController.MoveTarget.position = _eliController.transform.position;
		_eliController.AttackTarget = null;
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 0f), 1f, 0.5f, 2f);
		StartCoroutine(_part_3_start(0.5f));
		part_3 = true;
	}

	public void Gate_3()
	{
		gameController.PlayCutScene(true);
		CutSceneUse = true;
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		_cameraSmoothFollow.enabled = false;
		UICamera.GetComponent<Camera>().enabled = false;
		HUDPanel.SetActive(false);
		_eliController.MoveTarget.position = _eliController.transform.position;
		_eliController.AttackTarget = null;
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 0f), 1f, 0.5f, 2f);
		StartCoroutine(_part_5_start(0.5f));
		part_5 = true;
	}

	public void Gate_4()
	{
		gameController.PlayCutScene(true);
		CutSceneUse = true;
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		_cameraSmoothFollow.enabled = false;
		UICamera.GetComponent<Camera>().enabled = false;
		HUDPanel.SetActive(false);
		_eliController.MoveTarget.position = _eliController.transform.position;
		_eliController.AttackTarget = null;
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 0f), 1f, 0.5f, 2f);
		StartCoroutine(_part_6_start(0.5f));
		part_6 = true;
	}

	private IEnumerator _part_0_skip(float t)
	{
		yield return new WaitForSeconds(t);
		for (int i = 0; i < bandit_part_0.Length; i++)
		{
			StartCoroutine(MoveTargetEnemy(bandit_part_0[i], 5f, wist_gate_point_0.position));
		}
		StartCoroutine(MoveTargetEnemy(_twistController, 5f, wist_gate_point_0.position));
		StartCoroutine(SetActiveTrue(wave_part_0_1, 0f));
	}

	private IEnumerator _part_1_start(float t)
	{
		yield return new WaitForSeconds(t);
		gameController.PlayCutScene(true);
		CutSceneUse = true;
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		UICamera.GetComponent<Camera>().enabled = false;
		HUDPanel.SetActive(false);
		_eliController.MoveTarget.position = _eliController.transform.position;
		_eliController.AttackTarget = null;
		StartCoroutine(TextHint(0.2f, 0f, "icon_eli", text[3]));
		StartCoroutine(TextHint(2.2f, 2.5f, "icon_twist", text[4]));
		wave_part_0_1.SetActive(false);
		wave_part_0_2.SetActive(true);
		part_1 = true;
	}

	private IEnumerator _part_1_skip(float t)
	{
		yield return new WaitForSeconds(t);
		wave_part_0_1.SetActive(false);
		wave_part_0_2.SetActive(true);
	}

	private IEnumerator _part_2_start(float t)
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
		StartCoroutine(AnimationPlay(MainCamera, 1f, OpenDoorView_1.name, 1f));
		StartCoroutine(AnimationPlay(TowerGate_1, 0.1f, TowerGateOpen.name, 2f));
		StartCoroutine(TextHint(3f, OpenDoorView_1.length - 2f, "icon_twist", text[5]));
		StartCoroutine(MoveTargetEnemy(_twistController, 4f, wist_go_point_0.position, 7f));
		part_2 = true;
	}

	private IEnumerator _part_2_skip(float t)
	{
		yield return new WaitForSeconds(t);
		StartCoroutine(AnimationPlay(TowerGate_1, 1f, TowerGateOpen.name));
		StartCoroutine(MoveTargetEnemy(_twistController, 5f, wist_go_point_0.position));
	}

	private IEnumerator _part_3_start(float t)
	{
		yield return new WaitForSeconds(t);
		StartCoroutine(AnimationPlay(MainCamera, 1f, GoGate_2.name, 1f));
		StartCoroutine(TextHint(2f, GoGate_2.length - 2f, "icon_twist", text[6]));
		StartCoroutine(MoveTargetEnemy(_twistController, 4f, wist_gate_point_1.position, 4f));
		StartCoroutine(SetActiveTrue(wave_part_0_3, 3f));
		StartCoroutine(AnimationPlay(TowerGate_1, 0.5f, TowerGateClose.name, 7f));
		_cutScenePart = CutScenePart.Part_3;
	}

	private IEnumerator _part_3_skip(float t)
	{
		yield return new WaitForSeconds(t);
		StartCoroutine(MoveTargetEnemy(_twistController, 6f, wist_gate_point_1.position));
		StartCoroutine(AnimationPlay(TowerGate_1, 2f, TowerGateClose.name));
		StartCoroutine(SetActiveTrue(wave_part_0_3, 0f));
	}

	private IEnumerator _part_4_start(float t)
	{
		yield return new WaitForSeconds(t);
		gameController.PlayCutScene(true);
		CutSceneUse = true;
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		UICamera.GetComponent<Camera>().enabled = false;
		HUDPanel.SetActive(false);
		_eliController.MoveTarget.position = _eliController.transform.position;
		_eliController.AttackTarget = null;
		StartCoroutine(AnimationPlay(TowerGate_2, 0.1f, TowerGateOpen.name, 2f));
		StartCoroutine(TextHint(0.2f, 4f, "icon_twist", text[5]));
		StartCoroutine(MoveTargetEnemy(_twistController, 4f, wist_go_point_1.position, 7f));
		part_4 = true;
	}

	private IEnumerator _part_4_skip(float t)
	{
		yield return new WaitForSeconds(t);
		StartCoroutine(AnimationPlay(TowerGate_2, 1f, TowerGateOpen.name));
		StartCoroutine(MoveTargetEnemy(_twistController, 5f, wist_go_point_1.position));
	}

	private IEnumerator _part_5_start(float t)
	{
		yield return new WaitForSeconds(t);
		StartCoroutine(AnimationPlay(MainCamera, 1f, GoGate_3.name, 1f));
		StartCoroutine(AnimationPlay(TowerGate_2, 0.5f, TowerGateClose.name, 1.25f));
		StartCoroutine(TextHint(2f, GoGate_3.length - 2f, "icon_twist", text[6]));
		StartCoroutine(TextHint(6f, 0f, "icon_twist", text[9]));
		StartCoroutine(MoveTargetEnemy(_twistController, 6f, wist_gate_point_2.position));
		_cutScenePart = CutScenePart.Part_5;
	}

	private IEnumerator _part_5_skip(float t)
	{
		yield return new WaitForSeconds(t);
		_twistController.transform.position = wist_gate_point_2.position;
		StartCoroutine(AnimationPlay(TowerGate_2, 2f, TowerGateClose.name));
	}

	private IEnumerator _part_6_start(float t)
	{
		yield return new WaitForSeconds(t);
		_cutScenePart = CutScenePart.Part_6;
		_twistController.transform.eulerAngles = wist_gate_point_2.eulerAngles;
		StartCoroutine(AnimationPlay(MainCamera, 1f, GoGate_4.name, 1f));
		StartCoroutine(AnimationPlay(TowerGate_3, 0.5f, TowerGateOpen.name, 2f));
		StartCoroutine(MoveTargetEnemy(_nachoController, 2f, nacho_gate_point_0.position, 1.85f));
		for (int i = 0; i < bandit_part_1.Length; i++)
		{
			StartCoroutine(MoveTargetEnemy(bandit_part_1[i], 2f, bandit_part_1_point[i].position, 1.9f + (float)i / 10f));
		}
		StartCoroutine(MoveTargetEnemy(_twistController, 6f, wist_go_point_2.position, 6f));
		StartCoroutine(TextHint(4f, 0f, "icon_nacho", text[7]));
		StartCoroutine(TextHint(7f, GoGate_4.length, "icon_nacho", text[8]));
		StartCoroutine(AnimationPlay(TowerGate_3, 0.5f, TowerGateClose.name, 10f));
	}

	private IEnumerator _part_6_skip(float t)
	{
		yield return new WaitForSeconds(t);
		_twistController.transform.position = wist_go_point_2.position;
		_nachoController.transform.position = nacho_gate_point_0.position;
		for (int i = 0; i < bandit_part_1.Length; i++)
		{
			bandit_part_1[i].transform.position = bandit_part_1_point[i].position;
		}
		StartCoroutine(AnimationPlay(TowerGate_3, 2f, TowerGateClose.name));
	}

	private void Update()
	{
		if (_cutScenePart == CutScenePart.Part_0)
		{
			if (_wave_part_0_1.activeSelf)
			{
				_wave_part_0_1.SetActive(false);
				_cutScenePart = CutScenePart.Part_1;
			}
			timer += Time.deltaTime;
			if (timer > 1f)
			{
				timer = 0f;
				for (int i = 0; i < WavePointEnd.Length; i++)
				{
					WavePointEnd[i].transform.position = _eliController.transform.position;
				}
			}
		}
		if (_cutScenePart == CutScenePart.Part_1)
		{
			if (!Part_1)
			{
				Part_1 = true;
				StartCoroutine(_part_1_start(1.5f));
			}
			if (_wave_part_0_2.activeSelf)
			{
				_wave_part_0_2.SetActive(false);
				_cutScenePart = CutScenePart.Part_2;
			}
			timer += Time.deltaTime;
			if (timer > 1f)
			{
				timer = 0f;
				for (int j = 0; j < WavePointEnd.Length; j++)
				{
					WavePointEnd[j].transform.position = _eliController.transform.position;
				}
			}
		}
		if (_cutScenePart == CutScenePart.Part_2 && !Part_2)
		{
			Part_2 = true;
			StartCoroutine(_part_2_start(2.5f));
		}
		if (_cutScenePart == CutScenePart.Part_3)
		{
			if (_wave_part_0_3.activeSelf)
			{
				_wave_part_0_3.SetActive(false);
				_cutScenePart = CutScenePart.Part_4;
			}
			timer += Time.deltaTime;
			if (timer > 1f)
			{
				timer = 0f;
				for (int k = 0; k < WavePointEnd.Length; k++)
				{
					WavePointEnd[k].transform.position = _eliController.transform.position;
				}
			}
		}
		if (_cutScenePart == CutScenePart.Part_4 && !Part_4)
		{
			Part_4 = true;
			StartCoroutine(_part_4_start(1.5f));
		}
		if (_cutScenePart == CutScenePart.Part_6 && !Part_6 && _nachoController == null)
		{
			Part_6 = true;
			StartCoroutine(AnimationPlay(TowerGate_3, 0.5f, TowerGateOpen.name, 2f));
		}
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using AppsMinistry.Core;
using AppsMinistry.Core.Input;
using AppsMinistry.UI.Camera;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.Shops;
using Slugterra.Game.Mission;
using Slugterra.Game.Units;
using Slugterra.Game.Units.Ally;
using Slugterra.Game.Units.Ally.Assistant;
using Slugterra.Game.Units.Enemy;
using Slugterra.Scene;
using Slugterra.UI.Game;
using Slugterra.UI.Popup;
using UnityEngine;
using UnityStandardAssets.Cameras;

public class CutScene_Multiplayer : SlugterraCutScene
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

	private enum RotationSide
	{
		none = 0,
		left = 1,
		right = 2
	}

	public bool CutSceneUse;

	private bool _skip;

	[SerializeField]
	private CutSceneView _filmSceneView;

	[SerializeField]
	private bool InputEnable;

	private TutorHelpInput Help;

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

	private InGameController inGameController;

	public GameObject HUDPanel;

	public GameObject SlugPanel;

	public GameObject BoostPanel;

	private GameObject MainCamera;

	private GameObject UICamera;

	private CameraFade _cameraFade;

	public GameObject _fade;

	private AutoCam _cameraAutoCam;

	private ProtectCameraFromWallClip _cameraWallClip;

	private EliController _eliController;

	private TwistController _twistController;

	private NachoController _nachoController;

	public GameObject finish;

	private RotationSide side;

	public string[] text;

	private bool part_0;

	private bool part_1;

	private bool part_2;

	private bool part_3;

	private bool part_4;

	private bool part_5;

	private bool part_6;

	public Color Fog_Level_1;

	public Color Fog_Level_2;

	public Color Fog_Level_3;

	private byte indexMesh;

	public GameObject Mesh_Level_1;

	public GameObject Mesh_Level_2;

	public GameObject Mesh_Level_3;

	public float ActiveHeight_level_2;

	public float ActiveHeight_level_3;

	public GameObject FanceBlok;

	public GameObject EasyTouchControlsCanvas;

	public GameObject EasyTouchControlsButton_Back;

	public GameObject[] EasyTouchControlsMove;

	public GameObject EasyTouchControlsMoveDummy;

	public GameObject[] EasyTouchControlsRotation;

	public GameObject[] Crosshair;

	public OffScreenTarget OffScreenTarget;

	public OnScreenTarget OnScreenTarget;

	public OnScreenTargetCanvas OnScreenCanvasTarget;

	public GameObject Elevator_Platform;

	public float Elevator_Speed;

	public Transform Elevator_KordPoint;

	public Transform Elevator_EliPoint;

	public Transform Elevator_KordEndPoint;

	public Transform Elevator_EliEndPoint;

	public Transform Elevator_CameraPoint;

	public GameObject Part_0_Gate_1;

	public AnimationClip Part_0_OpenGate;

	public Transform Part_1_KordWeitPoint;

	public float Part_1_KordSpeed = 5f;

	public Transform Part_2_KordWeitPoint;

	public float Part_2_KordSpeed = 5f;

	public Transform Part_3_StartRunPoint;

	public Transform Part_3_CameraPoint;

	public List<Transform> Part_4_RandomPoints = new List<Transform>();

	private Transform Part_4_curPoints;

	public float Part_4_KordSpeed = 5f;

	private bool Part_4_KordGo;

	private bool Part_4_KordMoreDamage;

	private float oldHeatlh;

	private float maxHeatlh;

	public GameObject AssistHUD;

	public GameObject EnemyHUD;

	public float Part_5_KordSpeed = 5f;

	private bool Part_5_StartFight;

	private bool FixKord;

	public GameObject TwistBoss;

	public GameObject TwistClone;

	public GameObject TwistFXSpawn;

	public Transform TwistRandomPointRoot;

	public bool TwistCloneUse;

	public int TwistCount;

	private int _randomPoint;

	public Transform[] clonePoint;

	public List<GameObject> TwistCloneArray = new List<GameObject>();

	private GameObject clone;

	private Transform cloneRotation;

	private Vector3 Rotation;

	private GameObject cloneFx;

	public LayerMask ScanLayer;

	private AssistantController _assistantController;

	public ObjectiveTarget TakeSlugObjective;

	[SerializeField]
	private SlugButtonView _ButtonTemp0;

	[SerializeField]
	private GameObject _ButtonTemp0_cd;

	[SerializeField]
	private SlugButtonView _ButtonTemp1;

	[SerializeField]
	private GameObject _ButtonTemp1_cd;

	[SerializeField]
	private SlugButtonView _ButtonTemp2;

	[SerializeField]
	private GameObject _ButtonTemp2_cd;

	private bool getsgug;

	private bool selectsgug;

	private bool fireReagy;

	private bool fireSlug;

	private float timer1;

	private float timer2;

	private bool needUseBoost;

	private bool cd;

	private List<ConsumableItem> items = new List<ConsumableItem>();

	public PopupConfirmView _popup;

	private RaycastHit hit;

	private bool cloneInstantiate;

	private int s;

	private int finishCount;

	private Transform newpoint
	{
		get
		{
			int num = UnityEngine.Random.Range(0, Part_4_RandomPoints.Count);
			int num2 = 80;
			while (true)
			{
				num = UnityEngine.Random.Range(0, Part_4_RandomPoints.Count);
				Vector3 rhs = _eliController.Transform.position - Part_4_RandomPoints[num].position;
				float num3 = Vector3.Dot(_eliController.Transform.forward, rhs);
				if (Vector3.Dot(_eliController.Transform.forward, rhs) > 3f)
				{
					break;
				}
				num2--;
			}
			return Part_4_RandomPoints[num];
		}
	}

	private void Start()
	{
		gameController.PlayCutScene(true);
		CutSceneUse = true;
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		Help = GetComponents<TutorHelpInput>()[0];
		MainCamera = GameObject.Find("MainCamera");
		UICamera = GameObject.Find("UICamera");
		_assistantController = UnityEngine.Object.FindObjectOfType<AssistantController>();
		inGameController = UnityEngine.Object.FindObjectOfType<InGameController>();
		_assistantController.enabled = false;
		UICamera.GetComponent<Camera>().enabled = false;
		HUDPanel.SetActive(false);
		SlugPanel.SetActive(false);
		BoostPanel.SetActive(false);
		_eliController = UnityEngine.Object.FindObjectOfType<EliController>();
		_cutSceneView.SkipButton.targetObject = gameObject;
		_cutSceneView.SkipButton.messageName = "Skip";
		_cutSceneView.SkipButton.gameObject.SetActive(false);
		_skip = false;
		_cameraFade = MainCamera.GetComponent<CameraFade>();
		_cameraAutoCam = MainCamera.transform.parent.parent.parent.GetComponent<AutoCam>();
		_cameraWallClip = MainCamera.transform.parent.parent.parent.GetComponent<ProtectCameraFromWallClip>();
		for (int i = 0; i < EasyTouchControlsMove.Length; i++)
		{
			EasyTouchControlsMove[i].SetActive(false);
		}
		for (int j = 0; j < EasyTouchControlsRotation.Length; j++)
		{
			EasyTouchControlsRotation[j].SetActive(false);
		}
		for (int k = 0; k < Crosshair.Length; k++)
		{
			Crosshair[k].SetActive(false);
		}
		EasyTouchControlsButton_Back.SetActive(false);
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 1f), 1f, 0.1f, 2f);
		ReplaceScene(1);
		FanceBlok.SetActive(false);
		StartCoroutine(AfterStart(0.1f));
	}

	private void FixedUpdate()
	{
		if (finish.activeSelf)
		{
			finish.SetActive(false);
			StartCoroutine(_part_End(0.5f));
		}
		if (_cutScenePart == CutScenePart.Part_4)
		{
			timer1 += Time.deltaTime;
			if (Part_4_KordMoreDamage)
			{
				if (_ButtonTemp0_cd.activeSelf && _ButtonTemp1_cd.activeSelf && _ButtonTemp2_cd.activeSelf)
				{
					cd = true;
				}
				else if (cd)
				{
					cd = false;
				}
				if (!fireSlug)
				{
					if (!selectsgug && !cd)
					{
						if (!getsgug)
						{
							getsgug = true;
							StartCoroutine(SetActiveHelpTrue("selectslug", null, Vector3.zero, 1.5f));
							Help.DisableHelp("aim");
						}
					}
					else if (getsgug)
					{
						getsgug = false;
						Help.DisableHelp("selectslug");
						Help.ShowHelp("aim", null, Vector3.zero);
					}
				}
				else if (getsgug)
				{
					getsgug = false;
					Help.DisableHelp("selectslug");
					Help.ShowHelp("aim", null, Vector3.zero);
				}
				timer1 = 0f;
			}
		}
		if (_cutScenePart != CutScenePart.Part_5 || !Part_5_StartFight)
		{
			return;
		}
		timer2 += Time.deltaTime;
		if (!(timer2 > 20f))
		{
			return;
		}
		if (!needUseBoost && _eliController.Model.Health < _eliController.Model.MaxHealth * 0.4f)
		{
			items = MonoSingleton<AppController>.Instance.Model.ShopModel.Consumables;
			foreach (ConsumableItem item in items)
			{
				if (item.Type == ConsumableType.First && item.Count > 0)
				{
					needUseBoost = true;
					Help.ShowHelp("useboost", null, Vector3.zero);
					return;
				}
			}
		}
		if (needUseBoost && _eliController.Model.Health > _eliController.Model.MaxHealth * 0.4f)
		{
			needUseBoost = false;
			Help.DisableHelp("useboost");
			timer2 = 0f;
		}
	}

	private IEnumerator AfterStart(float t)
	{
		yield return new WaitForSeconds(t);
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		_fade.SetActive(false);
		AssistHUD.SetActive(false);
		EnemyHUD.SetActive(false);
		OffScreenTarget.gameObject.SetActive(true);
	}

	public void Skip()
	{
		_skip = true;
		Help.DisableHelp("skip");
		Fade();
	}

	public void Fade()
	{
		_cutSceneView.ShowScreens(false);
		_filmSceneView.ShowScreens(false);
		if (!_cameraAutoCam.enabled)
		{
			_cameraFade.StartFade(new Color(0f, 0f, 0f, 0f), 1f, 0.5f, 2f);
		}
		_cutSceneView.SkipButton.gameObject.SetActive(false);
		_isSkipEnable = false;
		if (part_0)
		{
			StopAllCoroutines();
			cloneInstantiate = false;
			StartCoroutine(_part_default(0.5f));
			if (_skip)
			{
				StartCoroutine(_part_0_skip(0.5f));
				_skip = false;
			}
			part_0 = false;
		}
		if (part_1)
		{
			StopAllCoroutines();
			cloneInstantiate = false;
			StartCoroutine(_part_default(0.5f));
			if (_skip)
			{
				_skip = false;
			}
			part_1 = false;
		}
		if (part_2)
		{
			StopAllCoroutines();
			cloneInstantiate = false;
			StartCoroutine(_part_default(0.5f));
			if (_skip)
			{
				_skip = false;
			}
			part_2 = false;
		}
		if (part_3)
		{
			StopAllCoroutines();
			cloneInstantiate = false;
			StartCoroutine(_part_default(0.5f));
			if (_skip)
			{
				StartCoroutine(_part_3_skip(0.5f));
				_skip = false;
			}
			part_3 = false;
		}
		if (part_5)
		{
			StopAllCoroutines();
			cloneInstantiate = false;
			StartCoroutine(_part_default(0.5f));
			if (_skip)
			{
				StartCoroutine(_part_5_skip(0.5f));
				_skip = false;
			}
			part_5 = false;
		}
		if (part_6)
		{
			StopAllCoroutines();
			cloneInstantiate = false;
			StartCoroutine(_part_End(1.5f));
			if (_skip)
			{
				_skip = false;
			}
			part_6 = false;
		}
	}

	private IEnumerator TextHint(float t, float d, float s, string icon, string key)
	{
		if (d > s)
		{
			s = d;
			Debug.Log("TimeCUT");
		}
		yield return new WaitForSeconds(t);
		gameController.PlayCutScene(true);
		CutSceneUse = true;
		UICamera.GetComponent<Camera>().enabled = false;
		HUDPanel.SetActive(false);
		_cutSceneView.ShowScreens(true);
		_cutSceneView.SpeakerIcon = icon;
		_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance[key];
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		InputEnable = false;
		_isSkipEnable = true;
		if (d != 0f)
		{
			StartCoroutine(HideTextHint_0(d));
		}
		StartCoroutine(SetActiveHelpTrue("skip", null, Vector3.zero, s + 1.25f));
		yield return new WaitForSeconds(s);
		_cutSceneView.SkipButton.gameObject.SetActive(true);
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
		MainCamera.transform.localEulerAngles = Vector3.zero;
		MainCamera.transform.localPosition = Vector3.zero;
	}

	private IEnumerator FilmHint(float t, float d, float s)
	{
		if (d > s)
		{
			s = d;
			Debug.Log("TimeCUT");
		}
		yield return new WaitForSeconds(t);
		gameController.PlayCutScene(true);
		CutSceneUse = true;
		UICamera.GetComponent<Camera>().enabled = false;
		HUDPanel.SetActive(false);
		_filmSceneView.ShowScreens(true);
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		InputEnable = false;
		_isSkipEnable = true;
		if (d != 0f)
		{
			StartCoroutine(HideTextHint_0(d));
		}
		yield return new WaitForSeconds(s);
		_cutSceneView.SkipButton.gameObject.SetActive(true);
	}

	private IEnumerator HideFilmHint_0(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			_filmSceneView.ShowScreens(false);
			_isSkipEnable = false;
			_cutSceneView.SkipButton.gameObject.SetActive(false);
			Fade();
		}
		MainCamera.transform.localEulerAngles = Vector3.zero;
		MainCamera.transform.localPosition = Vector3.zero;
	}

	private IEnumerator SetActiveHelpTrue(string helpkey, Transform point, Vector3 offset, float weit)
	{
		yield return new WaitForSeconds(weit);
		Help.ShowHelp(helpkey, point, offset);
	}

	private IEnumerator SetActiveHelpFalse(string helpkey, float weit)
	{
		yield return new WaitForSeconds(weit);
		Help.DisableHelp(helpkey);
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
		if (indexController is TwistController)
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

	private IEnumerator SetActiveMove(bool state, float weit)
	{
		yield return new WaitForSeconds(weit);
		for (int i = 0; i < EasyTouchControlsMove.Length; i++)
		{
			EasyTouchControlsMove[i].SetActive(state);
		}
	}

	private IEnumerator SetActiveRotation(bool state, float weit)
	{
		yield return new WaitForSeconds(weit);
		for (int i = 0; i < EasyTouchControlsRotation.Length; i++)
		{
			EasyTouchControlsRotation[i].SetActive(state);
		}
	}

	private IEnumerator SetActiveCrosshair(bool state, float weit)
	{
		yield return new WaitForSeconds(weit);
		for (int i = 0; i < Crosshair.Length; i++)
		{
			Crosshair[i].SetActive(state);
		}
	}

	private IEnumerator _part_default(float t, float s = 1.5f)
	{
		yield return new WaitForSeconds(t);
		if ((bool)MainCamera.GetComponent<Animation>())
		{
			MainCamera.GetComponent<Animation>().Stop();
		}
		_cameraAutoCam.enabled = true;
		_cameraWallClip.enabled = true;
		MainCamera.transform.localEulerAngles = Vector3.zero;
		MainCamera.transform.localPosition = Vector3.zero;
		DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
		StartCoroutine(ShowUIAuto(s));
	}

	private IEnumerator _part_End(float t)
	{
		gameController.IsNotMultiplayer();
		yield return new WaitForSeconds(t);
		PlayerPrefs.SetInt("Tutor_Multiplayer", 1);
		DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
		UICamera.GetComponent<Camera>().enabled = true;
		HUDPanel.SetActive(true);
		gameController.OnFinishGame();
		gameController.PlayCutScene(false);
		CutSceneUse = false;
		gameObject.SetActive(false);
	}

	public void SkipTutor()
	{
		_popup.Show("defeat_skip_desc", () =>
		{
			PlayerPrefs.SetInt("Tutor_Multiplayer", 1);
			SceneLoader.LoadFromPrev<MultiplayerSceneController>(true);
		});
		Time.timeScale = 0f;
	}

	private void No(Action action)
	{
		Time.timeScale = 1f;
	}

	private void OnEnable()
	{
		_ButtonTemp0.OnUseEvent += SelectSlug;
		_ButtonTemp0.OnEndEvent += FireSlug;
		_ButtonTemp1.OnUseEvent += SelectSlug;
		_ButtonTemp1.OnEndEvent += FireSlug;
		_ButtonTemp2.OnUseEvent += SelectSlug;
		_ButtonTemp2.OnEndEvent += FireSlug;
		_popup.OnDeclineEvent += No;
		TakeSlugObjective.OnObjectiveCompleteEvent += ObjectiveComplete;
		DonDestroyMonoSingleton<InputManager>.Instance.BackBtnEvent += SkipTutor;
	}

	private void OnDisable()
	{
		_ButtonTemp0.OnUseEvent -= SelectSlug;
		_ButtonTemp0.OnEndEvent -= FireSlug;
		_ButtonTemp1.OnUseEvent -= SelectSlug;
		_ButtonTemp1.OnEndEvent -= FireSlug;
		_ButtonTemp2.OnUseEvent -= SelectSlug;
		_ButtonTemp2.OnEndEvent -= FireSlug;
		_popup.OnDeclineEvent -= No;
		TakeSlugObjective.OnObjectiveCompleteEvent -= ObjectiveComplete;
		DonDestroyMonoSingleton<InputManager>.Instance.BackBtnEvent -= SkipTutor;
	}

	private void SelectSlug()
	{
		selectsgug = true;
	}

	private void FireSlug()
	{
		selectsgug = false;
	}

	private void ObjectiveComplete()
	{
		fireSlug = true;
	}

	private IEnumerator _part_0_start(float t)
	{
		_cameraAutoCam.enabled = false;
		_cameraWallClip.enabled = false;
		MainCamera.GetComponent<Camera>().fieldOfView = 70f;
		yield return new WaitForSeconds(t);
		StartCoroutine(FilmHint(0f, 0f, 3f));
		StartCoroutine(_part_0_start_0(5f));
		part_0 = true;
	}

	private IEnumerator _part_0_start_0(float t)
	{
		yield return new WaitForSeconds(t);
		StartCoroutine(FilmHint(0f, 0f, 3f));
		StartCoroutine(TextHint(10f, 0f, 1f, "icon_kord", text[0]));
		StartCoroutine(SetActiveHelpTrue("skip", null, Vector3.zero, 15f));
	}

	private IEnumerator _part_0_skip(float t)
	{
		yield return new WaitForSeconds(t);
		Elevator_Platform.transform.position = Vector3.zero;
		StartCoroutine(AnimationPlay(Part_0_Gate_1, 0.5f, Part_0_OpenGate.name));
		_assistantController.transform.position = Elevator_KordEndPoint.position;
		_assistantController.transform.rotation = Elevator_KordEndPoint.rotation;
		_assistantController.enabled = true;
		_eliController.transform.position = Elevator_EliEndPoint.position;
		_eliController.transform.rotation = Elevator_EliEndPoint.rotation;
		StartCoroutine(SetActiveHelpFalse("skip", 0f));
		StartCoroutine(HideFilmHint_0(0f));
		ReplaceScene(3);
		MainCamera.GetComponent<Camera>().fieldOfView = 45f;
		_cutScenePart = CutScenePart.Part_1;
	}

	private IEnumerator _part_1_start(float t)
	{
		yield return new WaitForSeconds(t);
		_assistantController.MoveTarget.position = Part_1_KordWeitPoint.position;
		_assistantController.OnMove(Part_1_KordSpeed);
		StartCoroutine(SetActiveMove(true, 0.4f));
		StartCoroutine(SetActiveHelpTrue("move", null, Vector3.zero, 0f));
		StartCoroutine(SetActiveHelpTrue("movehand", null, Vector3.zero, 0.1f));
	}

	private IEnumerator _part_2_start(float t)
	{
		yield return new WaitForSeconds(t);
		_assistantController.MoveTarget.position = Part_2_KordWeitPoint.position;
		_assistantController.OnMove(Part_2_KordSpeed);
		Help.DelHelp("movehand");
	}

	private IEnumerator _part_3_start(float t)
	{
		Help.DelHelp("move");
		yield return new WaitForSeconds(t);
		gameController.PlayCutScene(true);
		CutSceneUse = true;
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		_cameraAutoCam.enabled = false;
		_cameraWallClip.enabled = false;
		UICamera.GetComponent<Camera>().enabled = false;
		HUDPanel.SetActive(false);
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 0f), 1f, 0.5f, 2f);
		StartCoroutine(TextHint(3f, 0f, 2f, "icon_kord", text[1]));
		_eliController.OnMoveEnd();
		StartCoroutine(_part_3_temp(0.5f));
		part_3 = true;
	}

	private IEnumerator _part_3_temp(float t)
	{
		yield return new WaitForSeconds(t);
		_assistantController.MoveTarget.position = Part_3_StartRunPoint.position;
		_assistantController.transform.position = Part_3_StartRunPoint.position;
		_assistantController.transform.rotation = Part_3_StartRunPoint.rotation;
		_eliController.transform.position = Part_2_KordWeitPoint.position;
		_eliController.transform.rotation = Part_2_KordWeitPoint.rotation;
		MainCamera.transform.position = Part_3_CameraPoint.position;
		MainCamera.transform.rotation = Part_3_CameraPoint.rotation;
	}

	private IEnumerator _part_3_skip(float t)
	{
		yield return new WaitForSeconds(t);
		_assistantController.MoveTarget.position = Part_3_StartRunPoint.position;
		_assistantController.transform.position = Part_3_StartRunPoint.position;
		_assistantController.transform.rotation = Part_3_StartRunPoint.rotation;
		_cutScenePart = CutScenePart.Part_4;
	}

	private IEnumerator _part_4_start(float t)
	{
		yield return new WaitForSeconds(t);
		FanceBlok.SetActive(true);
		StartCoroutine(SetActiveMove(false, 0f));
		_eliController.OnMoveEnd();
		StartCoroutine(SetActiveCrosshair(true, 1.5f));
		StartCoroutine(_part_4_temp(2f));
		StartCoroutine(SetActiveRotation(true, 2.5f));
		StartCoroutine(SetActiveHelpTrue("aim", null, Vector3.zero, 3f));
		yield return new WaitForSeconds(3f);
		Part_4_KordGo = true;
	}

	private IEnumerator _part_4_temp(float t)
	{
		maxHeatlh = _assistantController.Model.Health;
		oldHeatlh = _assistantController.Model.Health;
		yield return new WaitForSeconds(t);
		_assistantController.Transform.gameObject.layer = LayerMask.NameToLayer("EnemyLayer");
		_assistantController.CanAttack = false;
	}

	private IEnumerator _part_5_start(float t)
	{
		_assistantController.Transform.gameObject.layer = LayerMask.NameToLayer("AllyLayer");
		OnScreenTarget.SwitchTarget(null);
		OnScreenCanvasTarget.SwitchTarget(null);
		OnScreenTarget.gameObject.SetActive(false);
		OnScreenCanvasTarget.gameObject.SetActive(false);
		_eliController.AttackTarget = null;
		_assistantController.SetAttackTarget(null);
		_assistantController.PathFinder.repathRate = 0.5f;
		Help.DelHelp("aim");
		OffScreenTarget.SwitchTarget(null);
		yield return new WaitForSeconds(t);
		gameController.PlayCutScene(true);
		CutSceneUse = true;
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		_cameraAutoCam.enabled = false;
		_cameraWallClip.enabled = false;
		UICamera.GetComponent<Camera>().enabled = false;
		HUDPanel.SetActive(false);
		_eliController.AttackTarget = null;
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 0f), 1f, 0.5f, 2f);
		StartCoroutine(_part_5_temp(0.5f));
		StartCoroutine(TextHint(2.5f, 0f, 1f, "icon_kord", text[2]));
		part_5 = true;
	}

	private IEnumerator _part_5_temp(float t)
	{
		yield return new WaitForSeconds(t);
		FixKord = true;
		_eliController.transform.position = Part_2_KordWeitPoint.position;
		_eliController.transform.rotation = Part_2_KordWeitPoint.rotation;
		_eliController.OnMoveEnd();
		MainCamera.transform.position = Part_3_CameraPoint.position;
		MainCamera.transform.rotation = Part_3_CameraPoint.rotation;
	}

	private IEnumerator _part_5_skip(float t)
	{
		yield return new WaitForSeconds(t);
		_assistantController.Transform.gameObject.layer = LayerMask.NameToLayer("Default");
		_assistantController.enabled = false;
		StartCoroutine(SetActiveCrosshair(true, 1.5f));
		StartCoroutine(SetActiveMove(true, 1.5f));
		StartCoroutine(SetActiveRotation(true, 1.5f));
		StartCoroutine(_part_5_temp_1(2f));
	}

	private IEnumerator _part_5_temp_1(float t)
	{
		yield return new WaitForSeconds(t);
		EnemyHUD.SetActive(true);
		Part_5_StartFight = true;
		OnScreenTarget.gameObject.SetActive(true);
		OnScreenCanvasTarget.gameObject.SetActive(true);
		BoostPanel.SetActive(true);
	}

	private void ArenaFight()
	{
		TwistRespawn();
	}

	private IEnumerator _part_6_start(float t)
	{
		yield return new WaitForSeconds(t);
		OnScreenTarget.gameObject.SetActive(false);
		OnScreenCanvasTarget.gameObject.SetActive(false);
		StartCoroutine(_part_End(0.5f));
	}

	private void TwistRespawn()
	{
		if (TwistCloneArray.Count > 0)
		{
			for (s = 0; s < TwistCloneArray.Count; s++)
			{
				if (TwistCloneArray[s] == null)
				{
					TwistCloneArray.RemoveAt(s);
					finishCount++;
				}
				else if (Vector3.Distance(TwistCloneArray[s].transform.position, _eliController.Transform.position) < 1f)
				{
					TwistCloneArray[s].GetComponent<EnemyController>().OnDamage(1000f, null);
				}
			}
		}
		if (cloneInstantiate || finishCount >= 10 || TwistCloneArray.Count >= TwistCount)
		{
			return;
		}
		_randomPoint = UnityEngine.Random.Range(0, clonePoint.Length);
		bool flag = false;
		for (int i = 0; i < clonePoint.Length; i++)
		{
			Vector3 rhs = _eliController.Transform.position - clonePoint[i].position;
			float num = Vector3.Dot(_eliController.Transform.forward, rhs) * -1f;
			if (num < 0f)
			{
				flag = true;
			}
		}
		if (flag)
		{
			Debug.DrawRay(clonePoint[_randomPoint].position + -Vector3.up * 50f, transform.up * 100f, Color.gray, 1f);
			if (Physics.SphereCast(clonePoint[_randomPoint].position + Vector3.up * 50f, 1f, -transform.up, out hit, 100f, ScanLayer))
			{
				Vector3 rhs2 = _eliController.Transform.position - clonePoint[_randomPoint].position;
				float num2 = Vector3.Dot(_eliController.Transform.forward, rhs2) * -1f;
				if (num2 < 0f)
				{
					StartCoroutine(_InstantiateFx(0f));
					cloneInstantiate = true;
				}
			}
		}
		else
		{
			Debug.DrawRay(clonePoint[_randomPoint].position + -Vector3.up * 50f, transform.up * 100f, Color.blue, 1f);
			if (Physics.SphereCast(clonePoint[_randomPoint].position + Vector3.up * 50f, 1f, -transform.up, out hit, 100f, ScanLayer))
			{
				StartCoroutine(_InstantiateFx(0f));
				cloneInstantiate = true;
			}
		}
	}

	private IEnumerator _InstantiateFx(float t)
	{
		yield return new WaitForSeconds(t);
		if (TwistCloneUse)
		{
			cloneFx = UnityEngine.Object.Instantiate(TwistFXSpawn, hit.point, Quaternion.identity);
			StartCoroutine(_InstantiateClone(0.333f));
		}
		else
		{
			cloneInstantiate = false;
		}
	}

	private IEnumerator _InstantiateClone(float t)
	{
		yield return new WaitForSeconds(t);
		clone = UnityEngine.Object.Instantiate(TwistClone, cloneFx.transform.position, Quaternion.identity);
		cloneRotation = clone.transform.Find("MobController");
		cloneRotation.transform.forward = -(cloneFx.transform.position - _eliController.transform.position);
		Rotation.y = cloneRotation.transform.eulerAngles.y;
		cloneRotation.transform.eulerAngles = Rotation;
		inGameController.NetPlayerAvatarInfo.OnApplyModel(cloneRotation.GetComponent<EnemyController>().Model);
		TwistCloneArray.Add(cloneRotation.gameObject);
		UnityEngine.Object.Destroy(cloneRotation.GetComponent<EnemyController>().PathFinder);
		OffScreenTarget.SwitchTarget(cloneRotation);
		OnScreenTarget.SwitchTarget(cloneRotation);
		OnScreenCanvasTarget.SwitchTarget(cloneRotation);
		cloneInstantiate = false;
	}

	private void ReplaceScene(byte index)
	{
		if (index == 1)
		{
			indexMesh = 1;
			RenderSettings.fogColor = Fog_Level_1;
			StartCoroutine(SetActiveTrue(Mesh_Level_1, 0f));
			if (Mesh_Level_2.activeSelf)
			{
				StartCoroutine(SetActiveFalse(Mesh_Level_2, 0f));
			}
			if (Mesh_Level_3.activeSelf)
			{
				StartCoroutine(SetActiveFalse(Mesh_Level_3, 0f));
			}
		}
		if (index == 2)
		{
			indexMesh = 2;
			RenderSettings.fogColor = Fog_Level_2;
			StartCoroutine(SetActiveTrue(Mesh_Level_2, 0f));
			if (Mesh_Level_1.activeSelf)
			{
				StartCoroutine(SetActiveFalse(Mesh_Level_1, 0f));
			}
			if (Mesh_Level_3.activeSelf)
			{
				StartCoroutine(SetActiveFalse(Mesh_Level_3, 0f));
			}
		}
		if (index == 3)
		{
			indexMesh = 3;
			RenderSettings.fogColor = Fog_Level_3;
			StartCoroutine(SetActiveTrue(Mesh_Level_3, 0f));
			if (Mesh_Level_2.activeSelf)
			{
				StartCoroutine(SetActiveFalse(Mesh_Level_2, 0f));
			}
			if (Mesh_Level_1.activeSelf)
			{
				StartCoroutine(SetActiveFalse(Mesh_Level_1, 0f));
			}
		}
	}

	private void Update()
	{
		if (_cutScenePart == CutScenePart.Part_0)
		{
			if (!Part_0)
			{
				Part_0 = true;
				StartCoroutine(_part_0_start(0f));
			}
			Elevator_Platform.transform.position = Vector3.MoveTowards(Elevator_Platform.transform.position, Vector3.zero, Time.deltaTime * Elevator_Speed);
			if (indexMesh < 2 && Elevator_Platform.transform.position.y <= ActiveHeight_level_2)
			{
				ReplaceScene(2);
			}
			if (indexMesh < 3 && Elevator_Platform.transform.position.y <= ActiveHeight_level_3)
			{
				ReplaceScene(3);
			}
			_eliController.transform.position = Elevator_EliPoint.position;
			_assistantController.transform.position = Elevator_KordPoint.position;
			MainCamera.transform.rotation = Elevator_CameraPoint.rotation;
			MainCamera.transform.position = Elevator_CameraPoint.position;
		}
		if (_cutScenePart == CutScenePart.Part_1)
		{
			if (!Part_1)
			{
				Part_1 = true;
				StartCoroutine(_part_1_start(3f));
			}
			if (!EasyTouchControlsMoveDummy.activeSelf)
			{
				Help.HideScaleLayer();
			}
			else
			{
				Help.ShowScaleLayer();
			}
			if (Vector3.Distance(_eliController.transform.position, Part_1_KordWeitPoint.position) < 8f)
			{
				_cutScenePart = CutScenePart.Part_2;
			}
		}
		if (_cutScenePart == CutScenePart.Part_2)
		{
			if (!Part_2)
			{
				Part_2 = true;
				StartCoroutine(_part_2_start(0f));
			}
			if (!EasyTouchControlsMoveDummy.activeSelf)
			{
				Help.HideScaleLayer();
			}
			else
			{
				Help.ShowScaleLayer();
			}
			if (Vector3.Distance(_eliController.transform.position, Part_2_KordWeitPoint.position) < 10f)
			{
				_cutScenePart = CutScenePart.Part_3;
				Help.ShowScaleLayer();
			}
		}
		if (_cutScenePart == CutScenePart.Part_3 && !Part_3)
		{
			Part_3 = true;
			StartCoroutine(_part_3_start(0.5f));
		}
		if (_cutScenePart == CutScenePart.Part_4)
		{
			if (!Part_4)
			{
				Part_4 = true;
				StartCoroutine(_part_4_start(0.5f));
			}
			if ((bool)_assistantController.GetComponent<RotateToCommand>())
			{
				_assistantController.GetComponent<RotateToCommand>().ReplaseTarget(_eliController.Transform);
				_assistantController.Animator.SetBool("EnemyInSight", false);
			}
			if (Part_4_KordGo && Part_4_curPoints != newpoint && Vector3.Distance(_assistantController.MoveTarget.position, _assistantController.Transform.position) < 4f)
			{
				Vector3 rhs = _eliController.Transform.position - _assistantController.Transform.position;
				float num = Vector3.Dot(_eliController.Transform.forward, rhs) * -1f;
				if (oldHeatlh != _assistantController.Model.Health)
				{
					Part_4_curPoints = newpoint;
					_assistantController.MoveTarget.position = Part_4_curPoints.position;
					_assistantController.OnMove(Part_4_KordSpeed);
					oldHeatlh = _assistantController.Model.Health;
				}
			}
			if (Part_4_KordGo)
			{
				Vector3 rhs2 = _eliController.Transform.position - _assistantController.Transform.position;
				float num2 = Vector3.Dot(_eliController.Transform.forward, rhs2) * -1f;
				side = RotationSide.none;
				if ((OffScreenTarget.GetComponent<MeshRenderer>().enabled && OffScreenTarget.transform.localPosition.x >= 0f) || (OnScreenTarget.GetComponent<MeshRenderer>().enabled && OnScreenTarget.transform.localPosition.x >= 0f))
				{
					side = RotationSide.left;
				}
				else
				{
					side = RotationSide.right;
				}
				if (_eliController.AttackTarget != null)
				{
					side = RotationSide.none;
				}
				if (side == RotationSide.none || (Part_4_KordMoreDamage && !cd && !selectsgug))
				{
					Help.DisableHelp("slide_l_r");
					Help.DisableHelp("slide_r_l");
				}
				else if (side == RotationSide.left)
				{
					Help.DisableHelp("slide_l_r");
					Help.ShowHelp("slide_r_l", null, Vector3.zero);
				}
				else
				{
					Help.DisableHelp("slide_r_l");
					Help.ShowHelp("slide_l_r", null, Vector3.zero);
				}
			}
			if (_assistantController.Model.Health < maxHeatlh - maxHeatlh / 40f)
			{
				if (!Part_4_KordMoreDamage)
				{
					Part_4_KordMoreDamage = true;
					SlugPanel.SetActive(true);
				}
				_assistantController.Model.AddHealth(1f);
			}
			if (Part_4_KordMoreDamage && fireSlug)
			{
				_cutScenePart = CutScenePart.Part_5;
				Help.DelHelp("slide_l_r");
				Help.DelHelp("slide_r_l");
			}
			if (_popup.gameObject.activeSelf)
			{
				Help.HideLayer();
			}
			else
			{
				Help.ShowLayer();
			}
		}
		if (_cutScenePart == CutScenePart.Part_5)
		{
			if (!Part_5)
			{
				Part_5 = true;
				_assistantController.MoveTarget.position = Part_3_StartRunPoint.position;
				_assistantController.OnMove(Part_5_KordSpeed);
				_assistantController.Animator.SetBool("EnemyInSight", false);
				Help.DelHelp("slide_l_r");
				Help.DelHelp("slide_r_l");
				StartCoroutine(_part_5_start(3.5f));
				StartCoroutine(SetActiveCrosshair(false, 0f));
			}
			if (FixKord)
			{
				_assistantController.MoveTarget.position = Part_3_StartRunPoint.position;
				_assistantController.transform.position = Part_3_StartRunPoint.position;
				_assistantController.transform.rotation = Part_3_StartRunPoint.rotation;
				_assistantController.Animator.SetBool("EnemyInSight", false);
			}
			if (Part_5_StartFight)
			{
				ArenaFight();
				if (_eliController.Model.Health < 800f)
				{
					_eliController.Model.AddHealth(10f);
					if (_eliController.AttackTarget == null)
					{
						_eliController.OnBeginAttack();
					}
				}
				if (finishCount >= 10)
				{
					_cutScenePart = CutScenePart.Part_6;
				}
			}
			if (_popup.gameObject.activeSelf)
			{
				Help.HideLayer();
			}
			else
			{
				Help.ShowLayer();
			}
		}
		if (_cutScenePart == CutScenePart.Part_6 && !Part_6)
		{
			Part_6 = true;
			StartCoroutine(_part_6_start(1f));
		}
	}
}

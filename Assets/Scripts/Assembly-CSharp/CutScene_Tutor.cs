using System;
using System.Collections;
using AppsMinistry.Core;
using AppsMinistry.Core.Input;
using AppsMinistry.UI.Camera;
using DG.Tweening;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain;
using Slugterra.Domain.Game.Units;
using Slugterra.Domain.Game.Units.Slugs;
using Slugterra.Game.Environment;
using Slugterra.Game.Input;
using Slugterra.Game.Units;
using Slugterra.Game.Units.Ally;
using Slugterra.Game.Units.Ally.Assistant;
using Slugterra.Game.Units.AreaTrigger;
using Slugterra.Game.Units.Enemy;
using Slugterra.Game.Units.Slugs;
using Slugterra.Scene;
using Slugterra.Scene.Game;
using Slugterra.UI.Game;
using Slugterra.UI.Game.Camera;
using UnityEngine;

public class CutScene_Tutor : SlugterraCutScene
{
	public enum CutScenePart
	{
		Part_Start = 0,
		Part_0 = 1,
		Part_1 = 2,
		Part_2 = 3,
		Part_3 = 4,
		Part_4 = 5,
		Part_5 = 6,
		Part_6 = 7,
		Part_7 = 8
	}

	[Serializable]
	public class _sourceCommon
	{
		public TrixieController _trixieController;

		public KordController _kordController;

		public SentinelController _sentinelController;

		public CapsuleCollider _sentinelTouch;

		public GameObject _eliAttackTarget;

		public EnemyController[] SentinelEnemy;

		public GameObject[] TerrainElements;

		public GameObject[] TerrainElementsDisable;

		public AudioClip mostOpen;

		public AudioClip mostClose;

		public GameObject Most1;

		public BoxCollider Most1_Coll;

		public GameObject Most2;

		public BoxCollider[] Most2_Coll;

		public float newSpeed = 3.6f;

		public UnitPathFinder[] CustomSpeed;
	}

	[Serializable]
	public class _part_0
	{
		public AnimationClip ShowHacho;

		public AnimationClip FlyForSlug;

		public AnimationClip wowClip;

		public Transform MovePoint_0;

		public GameObject MovePoint_cross;

		public AudioSource NachoShoot;

		public AudioSource SlugFly;

		public ParticleSystem Fly;

		public Animator Nacho;

		public GameObject wallAdded;

		public float nachoActive = 5f;

		public float weitFire = 5f;

		public SlugController Slug;

		public SplineAnimator SplineCamera;

		public float weitAfterFire = 0.2f;

		public float speedUp = 0.5f;

		public float MaxSpeedUp = 0.16f;

		public bool lockNachoFire;

		public bool doorExp;

		public float weitWow = 5f;

		public Animation wow;

		public Transform Pronto_point_0;

		public Transform Eli_point_0;
	}

	[Serializable]
	public class _part_1
	{
		public GameObject MiniMapPanel;

		[HideInInspector]
		public Color black = Color.black;

		public Renderer shadow;

		public bool showShadowPlane;

		public GameObject MiniMapCamera;

		public GameObject Radar_Root;

		[HideInInspector]
		public bool MiniMapOpen;

		[HideInInspector]
		public bool MiniMapClose;

		[HideInInspector]
		public bool MiniMapOpenStoreOpen;

		public GameObject Marker_1_Loop;

		public Transform Pronto_point_1;

		public Transform[] objective;
	}

	[Serializable]
	public class _part_2
	{
		public GameObject Marker_ChekPoint_0;

		public GameObject Marker_2_Loop;

		public Transform Pronto_point_2;

		public Transform Eli_point_1;

		public GameObject FixBarrel;

		public GameObject BoxEli;

		public BoxCollider BoxEliColl;

		public GameObject FixBarrelBorder;

		public GameObject BarrelEli;

		public BoxCollider BarrelEliColl;

		[HideInInspector]
		public bool KickParent;

		public Transform ExposionParent;

		public GameObject prf_Health;

		public Transform Pronto_point_3;

		public Transform Pronto_point_4;

		public Transform PlayerHUD;

		public Transform PlayerHP;

		public Transform MoneyHUD;

		public Transform NewPlayerHP;

		[HideInInspector]
		public bool GetMoney;

		[HideInInspector]
		public float OldMoneyCount;
	}

	[Serializable]
	public class _part_3
	{
		public AnimationClip CutScene1;

		public float CameraSpeed = 1f;

		public float ProntoSpeed = 4f;

		public float EliSpeed = 4f;

		public float TimeUp = 1f;

		public Transform Pronto_point_5;

		public Transform Eli_point_3;

		public Transform Pronto_point_6;

		public Transform Eli_point_4;

		public Transform Eli_point_5;

		public Transform Sentinel_point_0;
	}

	[Serializable]
	public class _part_4
	{
		public Animation Motor;

		public SphereCollider Area;

		public SphereCollider ProntoArea;

		public AreaTrigger ProntoAreaTrigger;

		public SphereCollider SentilelArea;

		public BoxCollider MostTouch;

		public AnimationClip TrixiIntro;

		public Transform SentilelReadyFire;

		public Transform SentilelGoHome;

		public Transform TrixieEnd;

		public GameObject Hand_DT;

		[HideInInspector]
		public bool EnemyKick;

		[HideInInspector]
		public bool TargetFalse;

		public Transform TrixiePoint_0;

		public float DestroyFence = 10f;

		public GameObject EnemyGroup;

		public EnemyController[] Enemy;

		public CapsuleCollider[] _sphereEnemyColliderTouch;

		public SphereCollider[] _sphereEnemyArea;

		public Transform[] EnemyPoint;

		public Transform EliEnemyPoint;

		public Transform EliEnemyPoint_07;

		public ExplosionEnvironmentController Fence;

		[HideInInspector]
		public bool FixDtap;

		[HideInInspector]
		public bool SwipeGo;
	}

	[Serializable]
	public class _part_5
	{
		public AnimationClip EpicEnd;

		public float EliSpeed;

		public Transform Eli_startPoint;

		public Transform Eli_endPoint;

		public Transform Kord_Point;

		public Transform Pronto_Point;

		public GameObject Explosion;

		public float weitExp = 0.25f;

		public ExplosionEnvironmentController[] Box;

		[HideInInspector]
		public bool Part_5_Go;
	}

	[SerializeField]
	private CutSceneView _filmSceneView;

	private TutorHelpInput Help;

	private TutorHelpInput HelpLayer2;

	[SerializeField]
	private bool CutSceneUse;

	[SerializeField]
	private bool InputEnable;

	private InGameView _inGameView;

	[SerializeField]
	private MinimapView _minimapView;

	private bool _skip;

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

	public GameObject FadePlane;

	private GameObject MainCamera;

	private GameObject UICamera;

	private CameraFade _cameraFade;

	private CameraSmoothFollow _cameraSmoothFollow;

	private EliController _eliController;

	private ProntoController _prontoController;

	public GameObject finish;

	public OffScreenTarget _offScreenTarget;

	public string[] text;

	private bool part_0;

	private bool part_1;

	private bool part_2;

	private bool part_3;

	private bool part_4;

	private bool part_5;

	private bool part_6;

	public GameObject SlugPanel;

	public GameObject PopupConfirm;

	public TextMesh PopupConfirmText;

	public bool PopupConfirmShow;

	public GameObject PopupAppStore;

	public tk2dButton btnYes;

	public tk2dButton btnNo;

	public GameObject Arrow;

	public GameObject LoadingScreen;

	public _sourceCommon SourceCommon;

	public _part_0 SourcePart_0;

	public _part_1 SourcePart_1;

	public _part_2 SourcePart_2;

	public _part_3 SourcePart_3;

	public _part_4 SourcePart_4;

	public _part_5 SourcePart_5;

	[HideInInspector]
	public bool MiniMapOpen;

	[HideInInspector]
	public bool MiniMapClse;

	private float weitHand;

	private Vector3 ProntoKickTemp = new Vector3(-11.285f, 0.231f, 22.162f);

	private int coutFireMost;

	private float timer = 0.4f;

	public float RadiusShake = 0.1f;

	public float TimeShake = 0.5f;

	private float timerShake;

	private Vector3 Shake = Vector3.zero;

	public bool SmoothShake;

	public float ReduceSmooth = 1f;

	private Vector3 newCameraPos;

	private Vector3 oldCameraPos;

	private bool checkSkip;

	private void Start()
	{
		_inGameView = HUDPanel.GetComponent<InGameView>();
		_minimapView = UnityEngine.Object.FindObjectOfType<MinimapView>();
		Help = GetComponents<TutorHelpInput>()[0];
		HelpLayer2 = GetComponents<TutorHelpInput>()[1];
		MainCamera = GameObject.Find("MainCamera");
		UICamera = GameObject.Find("UICamera");
		_eliController = UnityEngine.Object.FindObjectOfType<EliController>();
		_prontoController = UnityEngine.Object.FindObjectOfType<ProntoController>();
		_cutSceneView.SkipButton.targetObject = gameObject;
		_cutSceneView.SkipButton.messageName = "Skip";
		_cutSceneView.SkipButton.gameObject.SetActive(false);
		_skip = false;
		_cameraFade = MainCamera.GetComponent<CameraFade>();
		_cameraSmoothFollow = MainCamera.GetComponent<CameraSmoothFollow>();
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 1f), 1.5f, 0.1f, 2f);
		StartCoroutine(SetActiveFalse(FadePlane, 0.1f));
		AfterStart();
		if (_cutScenePart == CutScenePart.Part_0)
		{
			UICamera.GetComponent<Camera>().enabled = false;
			HUDPanel.SetActive(false);
			DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
			InputEnable = false;
			_cameraSmoothFollow.enabled = false;
		}
		DonDestroyMonoSingleton<InputManager>.Instance.BackBtnEvent += SkipTutor;
		if (_inGameView != null)
		{
			_inGameView.OnMinimapEvent += OnMinimapOpen;
		}
		if (_minimapView != null)
		{
			_minimapView.OnBackEvent += OnMinimapClose;
		}
	}

	private void OnMinimapOpen()
	{
		if (gameController.Model.State == GameState.Minimap)
		{
			DonDestroyMonoSingleton<InputManager>.Instance.BackBtnEvent -= SkipTutor;
		}
	}

	private void OnMinimapClose()
	{
		DonDestroyMonoSingleton<InputManager>.Instance.BackBtnEvent += SkipTutor;
	}

	private void FixedUpdate()
	{
		if (finish.activeSelf)
		{
			finish.SetActive(false);
			StartCoroutine(_part_End(0.5f));
		}
	}

	public void Skip()
	{
		_skip = true;
		Help.DisableHelp("skip");
		Fade();
	}

	private void Fade()
	{
		_cutSceneView.ShowScreens(false);
		_filmSceneView.ShowScreens(false);
		if (!_cameraSmoothFollow.enabled)
		{
			_cameraFade.StartFade(new Color(0f, 0f, 0f, 0f), 1f, 0.5f, 1.5f);
		}
		_cutSceneView.SkipButton.gameObject.SetActive(false);
		_isSkipEnable = false;
		if (part_0)
		{
			StopAllCoroutines();
			SourcePart_0.lockNachoFire = true;
			StartCoroutine(_part_default(0.5f, 2f));
			if (_skip)
			{
				StartCoroutine(_part_0_skip(0.5f));
				_skip = false;
			}
			_cutScenePart = CutScenePart.Part_1;
			part_0 = false;
		}
		if (part_1)
		{
			StopAllCoroutines();
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
			StartCoroutine(_part_default(0.5f, 0f));
			if (_skip)
			{
				StartCoroutine(_part_2_skip(0.5f));
				_skip = false;
			}
			part_2 = false;
		}
		if (part_3)
		{
			StopAllCoroutines();
			StartCoroutine(_part_default(0.5f));
			if (_skip)
			{
				StartCoroutine(_part_3_skip(0.5f));
				_skip = false;
			}
			part_3 = false;
		}
		if (part_4)
		{
			StopAllCoroutines();
			StartCoroutine(_part_default(0.5f));
			if (_skip)
			{
				StartCoroutine(_part_4_skip(0.5f));
				_skip = false;
			}
			part_4 = false;
		}
		if (part_5)
		{
			StopAllCoroutines();
			StartCoroutine(_part_default(0.5f));
			if (_skip)
			{
				StartCoroutine(_part_5_skip(0.5f));
				_skip = false;
			}
			part_5 = false;
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

	private IEnumerator SoundPlay(GameObject sudioSource, AudioClip sound, float weit = 0f)
	{
		yield return new WaitForSeconds(weit);
		sudioSource.GetComponent<AudioSource>().clip = sound;
		sudioSource.GetComponent<AudioSource>().Play();
	}

	private IEnumerator MoveTargetUnit(EnemyController Controller, float speed, Vector3 target, float weit = 0f)
	{
		yield return new WaitForSeconds(weit);
		Controller.MoveTo(target, speed);
	}

	private IEnumerator MoveTargetUnit(AssistantController Controller, float speed, Vector3 target, float weit = 0f)
	{
		yield return new WaitForSeconds(weit);
		Controller.MoveTarget.position = target;
		Controller.OnMove(speed);
	}

	private IEnumerator MoveTargetUnit(EliController Controller, float speed, Vector3 target, float weit = 0f)
	{
		yield return new WaitForSeconds(weit);
		Controller.MoveTarget.position = target;
		Controller.OnMove(speed);
	}

	private IEnumerator ReplaceCurentTarget(Transform target = null, float weit = 0f)
	{
		yield return new WaitForSeconds(weit);
		if (_offScreenTarget != null)
		{
			if (target != null)
			{
				_offScreenTarget.SwitchTarget(target);
			}
			else
			{
				_offScreenTarget.SwitchTarget(null);
			}
		}
	}

	private IEnumerator SetActiveTrue(GameObject obj, float weit)
	{
		yield return new WaitForSeconds(weit);
		if (obj != null)
		{
			obj.SetActive(true);
		}
	}

	private IEnumerator SetActiveFalse(GameObject obj, float weit)
	{
		yield return new WaitForSeconds(weit);
		if (obj != null)
		{
			obj.SetActive(false);
		}
	}

	private IEnumerator SetActiveFalseScale(GameObject obj, float weit)
	{
		yield return new WaitForSeconds(weit);
		if (!(obj != null))
		{
			yield break;
		}
		bool scale = true;
		Vector3 oldScale = obj.transform.localScale;
		int i = 50;
		while (scale && i > 0)
		{
			yield return new WaitForSeconds(0.01f);
			obj.transform.localScale = Vector3.MoveTowards(obj.transform.localScale, Vector3.zero, 0.075f);
			i--;
			if (obj.transform.localScale == Vector3.zero || i <= 0)
			{
				scale = false;
				obj.SetActive(false);
				obj.transform.localScale = oldScale;
			}
		}
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

	private IEnumerator _part_default(float t, float s = 1.5f)
	{
		yield return new WaitForSeconds(t);
		if ((bool)MainCamera.GetComponent<Animation>())
		{
			MainCamera.GetComponent<Animation>().Stop();
		}
		_cameraSmoothFollow.enabled = true;
		DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
		InputEnable = true;
		StartCoroutine(ShowUIAuto(s));
	}

	private IEnumerator _part_End(float t)
	{
		PlayerPrefs.SetInt(typeof(ShaneTutor_0_1).Name, 1);
		yield return new WaitForSeconds(t);
		DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
		InputEnable = true;
		UICamera.GetComponent<Camera>().enabled = true;
		HUDPanel.SetActive(true);
		gameController.OnFinishGame();
		gameController.PlayCutScene(false);
		CutSceneUse = false;
		_eliController.CanSwipe = true;
		DonDestroyMonoSingleton<InputManager>.Instance.BackBtnEvent -= SkipTutor;
		if (_inGameView != null)
		{
			_inGameView.OnMinimapEvent += OnMinimapOpen;
		}
		if (_minimapView != null)
		{
			_minimapView.OnBackEvent -= OnMinimapClose;
		}
	}

	private void AfterStart()
	{
		MonoSingleton<GameCollisionMap>.Instance.isTutorial = true;
		gameController.Model.Assistant = _prontoController.Model as Humanoid;
		SourceCommon.TerrainElementsDisable = new GameObject[SourceCommon.TerrainElements.Length];
		SourceCommon._trixieController = UnityEngine.Object.FindObjectOfType<TrixieController>();
		SourceCommon._kordController = UnityEngine.Object.FindObjectOfType<KordController>();
		SourceCommon._trixieController.gameObject.SetActive(false);
		SourceCommon._kordController.gameObject.SetActive(false);
		LoadingScreen.SetActive(false);
		SlugPanel.SetActive(false);
		Arrow.SetActive(false);
		MoveTerrainElements(7, false);
		SourcePart_0.SplineCamera.enabled = false;
		SourcePart_0.SplineCamera.speed = 0f;
		SourcePart_0.MovePoint_cross.SetActive(false);
		SourcePart_1.MiniMapPanel.SetActive(false);
		SourcePart_1.black = Color.black;
		SourcePart_1.black.a = 0f;
		SourcePart_1.shadow.material.color = SourcePart_1.black;
		SourcePart_1.shadow.gameObject.GetComponent<BoxCollider>().enabled = false;
		SourcePart_1.Marker_1_Loop.SetActive(false);
		SourcePart_2.BarrelEliColl.enabled = false;
		SourcePart_2.BoxEliColl.enabled = false;
		SourcePart_2.ExposionParent.GetComponent<Animator>().enabled = false;
		SourcePart_2.prf_Health.SetActive(false);
		SourcePart_2.Marker_2_Loop.SetActive(false);
		SourcePart_2.Marker_ChekPoint_0.SetActive(false);
		SourcePart_2.OldMoneyCount = MonoSingleton<AppController>.Instance.Model.Wallet.Gold;
		SourcePart_2.PlayerHP.parent = SourcePart_2.NewPlayerHP.transform;
		for (int i = 0; i < SourcePart_4._sphereEnemyColliderTouch.Length; i++)
		{
			SourcePart_4._sphereEnemyColliderTouch[i].enabled = false;
		}
		SourcePart_4.EnemyGroup.SetActive(false);
		SourcePart_4.Hand_DT.SetActive(false);
		_eliController.CanSwipe = false;
		HelpLayer2.ShowHelp("continue", null, Vector3.zero);
	}

	private IEnumerator ProntoSetTarget(GameObject target, float weit)
	{
		yield return new WaitForSeconds(weit);
		_prontoController.SetAttackTarget(target.transform);
	}

	private IEnumerator ProntoEnableDisable(bool state, float weit)
	{
		yield return new WaitForSeconds(weit);
		_prontoController.enabled = state;
	}

	private IEnumerator ProntoKick(float weit)
	{
		yield return new WaitForSeconds(weit);
		SourcePart_2.KickParent = true;
		_prontoController.OnFallDown();
	}

	private IEnumerator ProntoUp(float weit)
	{
		yield return new WaitForSeconds(weit);
		_prontoController.OnStandUp();
		SourcePart_2.KickParent = false;
	}

	private void DisableMove()
	{
		for (int i = 0; i < SourceCommon.TerrainElements.Length; i++)
		{
			if (SourceCommon.TerrainElements[i] != null)
			{
				SourceCommon.TerrainElements[i].layer = LayerMask.NameToLayer("Default");
			}
		}
	}

	private void EnableMove()
	{
		for (int i = 0; i < SourceCommon.TerrainElements.Length; i++)
		{
			if (SourceCommon.TerrainElements[i] != null)
			{
				SourceCommon.TerrainElements[i].layer = LayerMask.NameToLayer("Terrain");
			}
		}
	}

	private void MoveTerrainElements(int index, bool state)
	{
		if (!state)
		{
			if (SourceCommon.TerrainElements[index] != null)
			{
				SourceCommon.TerrainElements[index].layer = LayerMask.NameToLayer("Default");
				SourceCommon.TerrainElementsDisable[index] = SourceCommon.TerrainElements[index];
				SourceCommon.TerrainElements[index] = null;
			}
		}
		else if (SourceCommon.TerrainElementsDisable[index] != null)
		{
			SourceCommon.TerrainElementsDisable[index].layer = LayerMask.NameToLayer("Terrain");
			SourceCommon.TerrainElements[index] = SourceCommon.TerrainElementsDisable[index];
			SourceCommon.TerrainElementsDisable[index] = null;
		}
	}

	private IEnumerator EliReadyFire(float weit)
	{
		yield return new WaitForSeconds(weit);
		_eliController.SetAttackTarget(null);
	}

	private IEnumerator ShowArrow(Vector3 pos, float weit)
	{
		yield return new WaitForSeconds(weit);
		Arrow.transform.position = pos;
		Arrow.SetActive(true);
	}

	private IEnumerator HideArrow(float weit)
	{
		yield return new WaitForSeconds(weit);
		Arrow.SetActive(false);
	}

	private IEnumerator _part_0_start(float t)
	{
		yield return new WaitForSeconds(t);
		gameController.PlayCutScene(true);
		CutSceneUse = true;
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		InputEnable = false;
		_cameraSmoothFollow.enabled = false;
		StartCoroutine(AnimationPlay(MainCamera, 1f, SourcePart_0.ShowHacho.name));
		UICamera.GetComponent<Camera>().enabled = false;
		HUDPanel.SetActive(false);
		_eliController.MoveTarget.position = _eliController.transform.position;
		_eliController.AttackTarget = null;
		_eliController.gameObject.SetActive(false);
		_prontoController.gameObject.SetActive(false);
		StartCoroutine(_part_0_start_0(SourcePart_0.nachoActive));
		StartCoroutine(AnimationPlay(SourceCommon.Most2.gameObject, 1f, "up"));
		StartCoroutine(FilmHint(0f, 0f, 1f));
		part_0 = true;
	}

	private IEnumerator _part_0_start_0(float t)
	{
		yield return new WaitForSeconds(t);
		SourcePart_0.Nacho.SetTrigger("IntroStart");
	}

	public void NachoFireEvent()
	{
		if (!SourcePart_0.lockNachoFire)
		{
			SourcePart_0.NachoShoot.Play();
			SourcePart_0.Fly.Play();
			StartCoroutine(_part_0_start_1(0f));
		}
	}

	private IEnumerator _part_0_start_1(float t)
	{
		yield return new WaitForSeconds(t);
		StartCoroutine(AnimationPlay(MainCamera, 1f, SourcePart_0.FlyForSlug.name));
		SourcePart_0.Slug.gameObject.SetActive(true);
		int allyLayer = LayerMask.NameToLayer("AllyLayer");
		int neutralLayer = LayerMask.NameToLayer("NeutralLayer");
		SourcePart_0.Slug.SetSlug(new Slug(SlugType.InfurnusGhoul, 1));
		SourcePart_0.Slug.Parent = SourceCommon._sentinelController;
		SlugStrategy slugStrategy = new EnemySlugStrategy(SourcePart_0.Slug, new int[2] { allyLayer, neutralLayer });
		SourcePart_0.Slug.SetStrategy(slugStrategy);
		yield return new WaitForSeconds(SourcePart_0.weitAfterFire);
		SourcePart_0.SplineCamera.enabled = true;
		SourcePart_0.Nacho.SetTrigger("IntroEnd");
		StartCoroutine(_part_0_start_2(SourcePart_0.FlyForSlug.length + SourcePart_0.weitWow));
	}

	private IEnumerator _part_0_start_2(float t)
	{
		yield return new WaitForSeconds(t);
		StartCoroutine(AnimationPlay(SourcePart_0.wow.gameObject, 1f, SourcePart_0.wowClip.name));
		StartCoroutine(TextHint(1f, 0f, 0f, "icon_pronto", text[0]));
		SourcePart_0.SplineCamera.enabled = false;
		StartCoroutine(_part_0_start_3(SourcePart_0.wowClip.length));
	}

	private IEnumerator _part_0_start_3(float t)
	{
		yield return new WaitForSeconds(t);
		_eliController.gameObject.SetActive(true);
		_prontoController.gameObject.SetActive(true);
		SourcePart_0.wow.gameObject.SetActive(false);
		StartCoroutine(MoveTargetUnit(_prontoController, 4f, SourcePart_0.Pronto_point_0.position, 0.5f));
		StartCoroutine(MoveTargetUnit(_eliController, 4f, SourcePart_0.Eli_point_0.position, 1f));
		StartCoroutine(SetActiveHelpTrue("skip", null, Vector3.zero, 1f));
		StartCoroutine(SetActiveTrue(SourcePart_0.wallAdded, 2f));
	}

	private IEnumerator _part_0_skip(float t)
	{
		yield return new WaitForSeconds(t);
		_eliController.gameObject.SetActive(true);
		_eliController.transform.position = SourcePart_0.Eli_point_0.position;
		_eliController.transform.rotation = SourcePart_0.Eli_point_0.rotation;
		_eliController.Animator.transform.localEulerAngles = Vector3.zero;
		_prontoController.gameObject.SetActive(true);
		_prontoController.transform.position = SourcePart_0.Pronto_point_0.position;
		_prontoController.transform.rotation = SourcePart_0.Pronto_point_0.rotation;
		_prontoController.Animator.transform.localEulerAngles = Vector3.zero;
		SourcePart_0.wow.gameObject.SetActive(false);
		StartCoroutine(AnimationPlay(SourceCommon.Most1.gameObject, 1f, "up"));
		StartCoroutine(AnimationPlay(SourceCommon.Most2.gameObject, 1f, "down"));
		SourcePart_2.Marker_ChekPoint_0.SetActive(true);
		StartCoroutine(SetActiveHelpFalse("skip", 1f));
		SourcePart_0.SplineCamera.enabled = false;
		StartCoroutine(SetActiveTrue(SourcePart_0.wallAdded, 0f));
		if (SourcePart_0.Slug != null)
		{
			SourcePart_0.Slug.gameObject.SetActive(false);
		}
		SourcePart_0.Fly.gameObject.SetActive(false);
		StartCoroutine(HideFilmHint_0(0f));
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		InputEnable = false;
	}

	private IEnumerator _part_1_start(float t)
	{
		yield return new WaitForSeconds(t);
		gameController.PlayCutScene(true);
		CutSceneUse = true;
		StartCoroutine(SetActiveTrue(SourcePart_1.MiniMapPanel, 2.3f));
		StartCoroutine(TransformScale(SourcePart_1.Radar_Root.transform, 1.85f, 0.5f, 0.5f, 3.5f));
		StartCoroutine(SetActiveHelpTrue("radar", null, Vector3.zero, 4.5f));
		SourcePart_1.showShadowPlane = true;
		SourcePart_1.shadow.gameObject.GetComponent<BoxCollider>().enabled = true;
		part_1 = false;
	}

	private IEnumerator _part_1_start_1(float t)
	{
		yield return new WaitForSeconds(t);
		SourcePart_1.showShadowPlane = false;
		SourcePart_1.shadow.gameObject.GetComponent<BoxCollider>().enabled = false;
		SourcePart_1.shadow.gameObject.SetActive(false);
		Time.timeScale = 1f;
		StartCoroutine(SetActiveTrue(SourcePart_1.Marker_1_Loop, 0.75f));
		StartCoroutine(SetActiveHelpTrue("minimapback", null, Vector3.zero, 2.75f));
		Help.DelHelp("radar");
		for (int i = 0; i < SourcePart_1.objective.Length; i++)
		{
			if (i < 3)
			{
				StartCoroutine(TransformScale(SourcePart_1.objective[i], 1.3f, 0.3f, 0.3f, 1f + (float)i * 0.5f, 0.4f));
			}
			else
			{
				StartCoroutine(TransformScale(SourcePart_1.objective[i], 1.3f, 0.3f, 0.3f, 1f + (float)i * 0.4f, 0.333f));
			}
		}
	}

	private IEnumerator _part_1_start_2(float t)
	{
		yield return new WaitForSeconds(t);
		gameController.PlayCutScene(false);
		CutSceneUse = false;
		DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
		InputEnable = true;
		StartCoroutine(SetActiveTrue(SourcePart_0.MovePoint_cross, 0.5f));
		StartCoroutine(SetActiveHelpTrue("move", SourcePart_0.MovePoint_0, Vector3.zero, 0.5f));
		Help.DelHelp("minimapback");
		Help.DelHelp("radar");
		StartCoroutine(MoveTargetUnit(_prontoController, 7f, SourcePart_1.Pronto_point_1.position, 0.25f));
	}

	private IEnumerator TransformScale(Transform obj, float scale, float speedUp, float speedDown, float weit, float weitAfter = 0.5f)
	{
		TextMesh tempText = obj.GetComponent<TextMesh>();
		Color startColor = Color.white;
		if (tempText != null)
		{
			startColor = tempText.color;
		}
		yield return new WaitForSeconds(weit);
		Vector3 oldScale = obj.localScale;
		obj.DOScale(Vector3.one * scale, speedUp);
		if (tempText != null)
		{
			tempText.DOColor(Color.green, 1f);
		}
		yield return new WaitForSeconds(weitAfter);
		obj.DOScale(oldScale, speedDown);
		if (tempText != null)
		{
			tempText.DOColor(startColor, 1f);
		}
	}

	public void SetPart2()
	{
		_cutScenePart = CutScenePart.Part_2;
		MoveTerrainElements(0, false);
		MoveTerrainElements(1, false);
	}

	private IEnumerator _part_2_start(float t)
	{
		yield return new WaitForSeconds(t);
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvent(InputEvent.DoubleTap);
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		InputEnable = false;
		StartCoroutine(MoveTargetUnit(_eliController, 7f, SourcePart_2.Eli_point_1.position));
		StartCoroutine(MoveTargetUnit(_eliController, 7f, SourcePart_2.Eli_point_1.position, 0.1f));
		StartCoroutine(_part_2_start_0(1.2f));
		DisableMove();
		StartCoroutine(ProntoEnableDisable(false, 0f));
	}

	private IEnumerator _part_2_start_0(float t)
	{
		yield return new WaitForSeconds(t);
		_eliController.Model.AddSpeedCoeficient(GetType(), 0f);
		_eliController.MoveTarget.position = _eliController.transform.position;
		_eliController.UseAutoattack = true;
		DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
		InputEnable = true;
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvent(InputEvent.DoubleTap);
		weitHand = 2.5f;
		Help.DisableHelp("fire");
		StartCoroutine(ShowArrow(SourcePart_2.BoxEli.transform.position + Vector3.up * 0.25f, 0f));
		SourcePart_2.BoxEliColl.enabled = true;
		StartCoroutine(SetActiveFalseScale(SourcePart_2.Marker_ChekPoint_0, 0f));
	}

	private IEnumerator _part_2_start_1(float t)
	{
		yield return new WaitForSeconds(t);
		_eliController.Model.AddHealth((0f - _eliController.Model.Health) * 0.5f);
		_prontoController.OnDamage(_prontoController.Model.Health * 0.5f, null);
		gameController.PlayCutScene(true);
		CutSceneUse = true;
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		InputEnable = false;
		StartCoroutine(TextHint(0f, 0f, 1f, "icon_pronto", text[1]));
		SourcePart_2.ExposionParent.GetComponent<Animator>().enabled = true;
		_prontoController.transform.rotation = SourcePart_2.ExposionParent.rotation;
		_prontoController.Animator.transform.localEulerAngles = Vector3.zero;
		StartCoroutine(ProntoKick(0f));
		_eliController.OnFallDown();
		EnableMove();
		part_2 = true;
	}

	private IEnumerator _part_2_skip(float t)
	{
		yield return new WaitForSeconds(t);
		_eliController.Model.RemoveSpeedCoeficient(GetType());
		StartCoroutine(ProntoEnableDisable(true, 0f));
		StartCoroutine(ProntoUp(0f));
		_eliController.OnStandUp();
		SourcePart_2.FixBarrelBorder.SetActive(false);
		StartCoroutine(ShowArrow(SourcePart_2.prf_Health.transform.position + Vector3.up * 0.5f, 0.1f));
		StartCoroutine(MoveTargetUnit(_prontoController, 6f, SourcePart_2.Pronto_point_3.position, 1.5f));
		StartCoroutine(HideFilmHint_0(0f));
		StartCoroutine(SetActiveFalseScale(SourcePart_1.Marker_1_Loop, 0f));
		StartCoroutine(SetActiveTrue(SourcePart_2.Marker_2_Loop, 3f));
		_offScreenTarget.SwitchTarget(SourcePart_2.Marker_2_Loop.transform);
		StartCoroutine(AnimationPlay(SourceCommon.Most1.gameObject, 1f, "down"));
	}

	public void ProntoPart1()
	{
		_eliController.UseAutoattack = false;
		Help.DisableHelp("move");
		StartCoroutine(SetActiveFalseScale(SourcePart_0.MovePoint_cross, 0f));
		StartCoroutine(MoveTargetUnit(_prontoController, 7f, SourcePart_2.Pronto_point_2.position));
		StartCoroutine(ReplaceCurentTarget(SourcePart_1.Marker_1_Loop.transform));
		StartCoroutine(ProntoEnableDisable(false, 0f));
	}

	public void ProntoPart3()
	{
		StartCoroutine(MoveTargetUnit(_prontoController, 7f, SourcePart_2.Pronto_point_4.position));
		_eliController.UseAutoattack = false;
	}

	public void SetPart3()
	{
		_cutScenePart = CutScenePart.Part_3;
		MoveTerrainElements(2, false);
		MoveTerrainElements(3, false);
		MoveTerrainElements(4, false);
		MoveTerrainElements(5, false);
	}

	private IEnumerator _part_3_start(float t)
	{
		yield return new WaitForSeconds(t);
		gameController.PlayCutScene(true);
		CutSceneUse = true;
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		InputEnable = false;
		UICamera.GetComponent<Camera>().enabled = false;
		HUDPanel.SetActive(false);
		part_3 = true;
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 0f), 0.2f, 0.5f, 0.5f);
		StartCoroutine(_part_3_start_0(0.5f));
	}

	private IEnumerator _part_3_start_0(float t)
	{
		yield return new WaitForSeconds(t);
		_cameraSmoothFollow.enabled = false;
		StartCoroutine(AnimationPlay(MainCamera, SourcePart_3.CameraSpeed, SourcePart_3.CutScene1.name));
		_eliController.transform.position = SourcePart_3.Eli_point_3.position;
		_eliController.MoveTarget.position = _eliController.transform.position;
		_eliController.AttackTarget = null;
		StartCoroutine(MoveTargetUnit(_eliController, SourcePart_3.EliSpeed, SourcePart_3.Eli_point_4.position));
		_prontoController.transform.position = SourcePart_3.Pronto_point_5.position;
		_prontoController.MoveTarget.position = _prontoController.transform.position;
		StartCoroutine(MoveTargetUnit(_prontoController, SourcePart_3.ProntoSpeed, SourcePart_3.Pronto_point_6.position));
		StartCoroutine(AnimationPlay(SourceCommon.Most1, 0.8f, "up", 1.5f));
		StartCoroutine(SoundPlay(SourceCommon.Most1, SourceCommon.mostOpen, 1.5f));
		StartCoroutine(AnimationPlay(SourceCommon.Most2.gameObject, 0.8f, "up", SourcePart_3.TimeUp));
		StartCoroutine(SoundPlay(SourceCommon.Most2, SourceCommon.mostOpen, SourcePart_3.TimeUp));
		StartCoroutine(ProntoKick(SourcePart_3.TimeUp));
		StartCoroutine(ProntoUp(SourcePart_3.TimeUp + 3f));
		StartCoroutine(FilmHint(0f, 0f, 1f));
		StartCoroutine(TextHint(SourcePart_3.CutScene1.length - 0.3f * SourcePart_3.CutScene1.length, 0f, 0f, "icon_sentinel", text[2]));
		yield return new WaitForSeconds(SourcePart_3.TimeUp);
		for (int time = 0; time < 50; time++)
		{
			_prontoController.transform.localPosition = Vector3.MoveTowards(_prontoController.transform.localPosition, ProntoKickTemp, Time.deltaTime * 4f);
			yield return new WaitForSeconds(Time.deltaTime);
		}
	}

	private IEnumerator _part_3_skip(float t)
	{
		yield return new WaitForSeconds(t);
		_eliController.transform.position = SourcePart_3.Eli_point_5.position;
		_eliController.MoveTarget.position = _eliController.transform.position;
		_eliController.transform.rotation = SourcePart_3.Eli_point_5.rotation;
		_eliController.Animator.transform.localEulerAngles = Vector3.zero;
		_eliController.AttackTarget = null;
		_prontoController.MoveTarget.position = _prontoController.transform.position;
		StartCoroutine(ProntoUp(0f));
		_offScreenTarget.SwitchTarget(null);
		SourceCommon._sentinelController.transform.position = SourcePart_3.Sentinel_point_0.position;
		SourceCommon._sentinelController.transform.rotation = SourcePart_3.Sentinel_point_0.rotation;
		SourceCommon._sentinelController.Animator.transform.localEulerAngles = Vector3.zero;
		StartCoroutine(AnimationPlay(SourceCommon.Most1.gameObject, 1f, "up"));
		StartCoroutine(AnimationPlay(SourceCommon.Most2.gameObject, 1f, "up"));
		Help.ShowHelp("select", SourceCommon._sentinelController.transform, Vector3.up * 1.9f);
		weitHand = 2.5f;
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvent(InputEvent.DoubleTap);
	}

	public void Temp()
	{
		if (SourcePart_4.EnemyKick)
		{
			coutFireMost++;
		}
		if (coutFireMost >= 1)
		{
			StartCoroutine(AnimationPlay(SourceCommon.Most2.gameObject, 1f, "down"));
			StartCoroutine(SoundPlay(SourceCommon.Most2, SourceCommon.mostClose));
			if (_cutScenePart != CutScenePart.Part_5 && Vector3.Distance(_prontoController.transform.position, SourcePart_2.Pronto_point_4.position) > 5f)
			{
				_prontoController.transform.position = SourcePart_2.Pronto_point_4.position;
				StartCoroutine(ProntoEnableDisable(true, 0.5f));
				StartCoroutine(MoveTargetUnit(_prontoController, 7f, SourcePart_3.Pronto_point_6.position, 0.5f));
			}
			SourcePart_4.Motor.Play("down");
			SourcePart_4.ProntoArea.enabled = true;
			SourcePart_4.ProntoAreaTrigger.enabled = true;
		}
		MoveTerrainElements(4, true);
		MoveTerrainElements(5, true);
	}

	private IEnumerator _part_4_start(float t)
	{
		yield return new WaitForSeconds(t);
		SourcePart_4.Area.center = Vector3.zero;
		_cameraSmoothFollow.SetTarget(SourceCommon._sentinelController.transform);
		SourceCommon._trixieController.gameObject.SetActive(true);
		SourceCommon._trixieController.SetAttackTarget(SourceCommon._sentinelController.transform);
		StartCoroutine(MoveTargetUnit(SourceCommon._trixieController, 8f, _cameraSmoothFollow.transform.position - _cameraSmoothFollow.cameraOffset));
	}

	private IEnumerator _part_4_start_1(float t)
	{
		yield return new WaitForSeconds(t);
		SourcePart_4.TargetFalse = true;
		DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
		InputEnable = true;
		SourcePart_4.Hand_DT.transform.position = SourceCommon._sentinelController.transform.position;
		SourcePart_4.Hand_DT.SetActive(true);
	}

	private IEnumerator _part_4_start_1_fixDT(float t)
	{
		yield return new WaitForSeconds(t);
		SourcePart_4.TargetFalse = true;
		DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
		InputEnable = true;
	}

	private IEnumerator _part_4_start_cutScene(float t)
	{
		yield return new WaitForSeconds(t);
		gameController.PlayCutScene(true);
		CutSceneUse = true;
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		InputEnable = false;
		UICamera.GetComponent<Camera>().enabled = false;
		HUDPanel.SetActive(false);
		part_4 = true;
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 0f), 0.2f, 0.5f, 0.5f);
		StartCoroutine(_part_4_start_cutScene_0(0.5f));
	}

	private IEnumerator _part_4_start_cutScene_0(float t)
	{
		yield return new WaitForSeconds(t);
		StartCoroutine(MoveTargetUnit(SourceCommon._trixieController, 6.5f, SourcePart_4.TrixieEnd.position));
		SourceCommon._trixieController.AttackTarget = null;
		SourceCommon._trixieController.transform.position = SourcePart_4.TrixieEnd.position;
		SourceCommon._trixieController.transform.rotation = SourcePart_4.TrixieEnd.rotation;
		SourceCommon._trixieController.Animator.transform.localEulerAngles = Vector3.zero;
		_eliController.transform.position = SourcePart_4.EliEnemyPoint_07.position;
		_eliController.transform.rotation = SourcePart_4.EliEnemyPoint_07.rotation;
		_eliController.Animator.transform.localEulerAngles = Vector3.zero;
		_eliController.MoveTarget.position = _eliController.transform.position;
		Temp();
		_cameraSmoothFollow.enabled = false;
		StartCoroutine(AnimationPlay(MainCamera, 1f, SourcePart_4.TrixiIntro.name));
		_cameraSmoothFollow.SetTarget(_eliController.MoveTarget);
		_cameraSmoothFollow.SetPlayer(_eliController.transform);
		StartCoroutine(FilmHint(0f, 0f, 1f));
		StartCoroutine(TextHint(5f, 0f, 0f, "icon_trixie", text[3]));
		StartCoroutine(MoveTargetUnit(SourceCommon._trixieController, 7f, SourcePart_4.TrixiePoint_0.position, 8f));
		StartCoroutine(SetActiveTrue(SourcePart_4.EnemyGroup, SourcePart_4.TrixiIntro.length - 5f));
		StartCoroutine(_part_4_start_2(SourcePart_4.DestroyFence));
	}

	private IEnumerator _part_4_start_2(float t)
	{
		yield return new WaitForSeconds(t);
		SourceCommon._sentinelController.gameObject.SetActive(false);
		SourcePart_4.Fence.OnDamage(10000f);
		for (int i = 0; i < SourcePart_4.Enemy.Length; i++)
		{
			StartCoroutine(MoveTargetUnit(SourcePart_4.Enemy[i], 7f, SourcePart_4.EnemyPoint[i].position, 0.5f + (float)i / 10f));
		}
	}

	private IEnumerator _part_4_skip(float t)
	{
		yield return new WaitForSeconds(t);
		SourceCommon._trixieController.gameObject.SetActive(false);
		_eliController.transform.position = SourcePart_4.EliEnemyPoint.position;
		_eliController.transform.rotation = SourcePart_4.EliEnemyPoint.rotation;
		_eliController.Animator.transform.localEulerAngles = Vector3.zero;
		_eliController.MoveTarget.position = _eliController.transform.position;
		SourceCommon._sentinelController.gameObject.SetActive(false);
		StartCoroutine(AnimationPlay(SourceCommon.Most2.gameObject, 1f, "up"));
		if (SourcePart_4.Fence.gameObject.activeSelf)
		{
			SourcePart_4.Fence.gameObject.SetActive(false);
		}
		DisableMove();
		SourcePart_4.SwipeGo = true;
		SourcePart_4.EnemyGroup.SetActive(true);
		StartCoroutine(SetActiveHelpTrue("slide", null, Vector3.zero, 2f));
		_eliController.Model.AddSpeedCoeficient(GetType(), 0f);
		for (int i = 0; i < SourcePart_4.Enemy.Length; i++)
		{
			SourcePart_4.Enemy[i].transform.position = SourcePart_4.EnemyPoint[i].position;
			SourcePart_4.Enemy[i].transform.rotation = SourcePart_4.EnemyPoint[i].rotation;
			SourcePart_4.Enemy[i].Animator.transform.localEulerAngles = Vector3.zero;
		}
		yield return new WaitForSeconds(2f);
		_eliController.CanSwipe = true;
	}

	private IEnumerator _part_5_start(float t)
	{
		_eliController.Model.RemoveSpeedCoeficient(GetType());
		yield return new WaitForSeconds(t);
		Help.DisableHelp("slide");
		for (int i = 0; i < SourcePart_4._sphereEnemyColliderTouch.Length; i++)
		{
			SourcePart_4._sphereEnemyColliderTouch[i].enabled = true;
			SourcePart_4._sphereEnemyArea[i].center = Vector3.zero;
		}
		_eliController.UseAutoattack = true;
		_eliController.CanAttack = true;
		_prontoController.enabled = true;
		SourcePart_4.Motor.SendMessage("Respawn");
		SourceCommon._kordController.gameObject.SetActive(true);
		StartCoroutine(MoveTargetUnit(SourceCommon._kordController, 5f, _cameraSmoothFollow.transform.position - _cameraSmoothFollow.cameraOffset, 1f));
		EnableMove();
	}

	private IEnumerator _part_5_start_CutScene(float t)
	{
		yield return new WaitForSeconds(t);
		gameController.PlayCutScene(true);
		CutSceneUse = true;
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		InputEnable = false;
		UICamera.GetComponent<Camera>().enabled = false;
		HUDPanel.SetActive(false);
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 0f), 0.2f, 0.5f, 0.5f);
		StartCoroutine(_part_5_start_CutScene_0(0.5f));
		part_5 = true;
	}

	private IEnumerator _part_5_start_CutScene_0(float t)
	{
		RisePlayCutSceneEvent(true);
		yield return new WaitForSeconds(t);
		_cameraSmoothFollow.enabled = false;
		StartCoroutine(AnimationPlay(MainCamera, 1f, SourcePart_5.EpicEnd.name));
		_eliController.transform.position = SourcePart_5.Eli_startPoint.position;
		_eliController.transform.rotation = SourcePart_5.Eli_startPoint.rotation;
		_eliController.Animator.transform.localEulerAngles = Vector3.zero;
		StartCoroutine(MoveTargetUnit(_eliController, 1f, SourcePart_5.Eli_endPoint.position));
		StartCoroutine(MoveTargetUnit(_prontoController, 8f, SourcePart_5.Pronto_Point.position));
		_prontoController.AllyArea.GetComponent<SphereCollider>().enabled = false;
		_prontoController.AllyArea.enabled = false;
		_prontoController.enabled = false;
		_prontoController.transform.position = SourcePart_5.Pronto_Point.position;
		_prontoController.transform.rotation = SourcePart_5.Pronto_Point.rotation;
		_prontoController.Animator.transform.localEulerAngles = Vector3.zero;
		StartCoroutine(MoveTargetUnit(SourceCommon._kordController, 8f, SourcePart_5.Kord_Point.position));
		SourceCommon._kordController.AllyArea.GetComponent<SphereCollider>().enabled = false;
		SourceCommon._kordController.AllyArea.enabled = false;
		SourceCommon._kordController.enabled = false;
		SourceCommon._kordController.transform.position = SourcePart_5.Kord_Point.position;
		SourceCommon._kordController.transform.rotation = SourcePart_5.Kord_Point.rotation;
		SourceCommon._kordController.Animator.transform.localEulerAngles = Vector3.zero;
		StartCoroutine(_part_5_start_1(3f));
		StartCoroutine(FilmHint(0f, 0f, 1f));
		StartCoroutine(TextHint(6.5f, 0f, 0f, "icon_kord", text[4]));
	}

	private IEnumerator _part_5_start_1(float t)
	{
		yield return new WaitForSeconds(t);
		StartCoroutine(ProntoKick(0.6f));
		StartCoroutine(SetActiveTrue(SourcePart_5.Explosion, 0.55f));
		StartCoroutine(_part_5_start_2(0f));
	}

	private IEnumerator _part_5_start_2(float t)
	{
		yield return new WaitForSeconds(t);
		for (int i = 0; i < SourcePart_5.Box.Length; i++)
		{
			SourcePart_5.Box[i].OnDamage(100f);
			yield return new WaitForSeconds(SourcePart_5.weitExp);
		}
	}

	private IEnumerator _part_5_skip(float t)
	{
		RisePlayCutSceneEvent(false);
		yield return new WaitForSeconds(t);
		finish.SetActive(true);
	}

	private void Update()
	{
		if (_cutScenePart == CutScenePart.Part_0)
		{
			if (SourcePart_0.Slug != null && SourcePart_0.Slug.gameObject.activeSelf && SourcePart_0.SplineCamera.enabled && SourcePart_0.SplineCamera.speed < SourcePart_0.MaxSpeedUp)
			{
				SourcePart_0.SplineCamera.speed += SourcePart_0.speedUp * Time.deltaTime;
			}
			if (SourcePart_0.Slug == null && !SourcePart_0.doorExp)
			{
				SourcePart_0.doorExp = true;
				SourcePart_0.SplineCamera.enabled = false;
				DoShake(2f);
			}
			if (!Part_0)
			{
				Part_0 = true;
				StartCoroutine(_part_0_start(0f));
			}
		}
		if (_cutScenePart == CutScenePart.Part_1)
		{
			if (!Part_1)
			{
				Part_1 = true;
				StartCoroutine(_part_1_start(0f));
			}
			if (SourcePart_1.MiniMapCamera.activeSelf)
			{
				_eliController.MoveTarget.position = _eliController.transform.position;
			}
			if (SourcePart_1.showShadowPlane && HUDPanel.activeSelf)
			{
				if (!CutSceneUse)
				{
					gameController.PlayCutScene(true);
					CutSceneUse = true;
				}
				if (SourcePart_1.black.a < 0.75f)
				{
					SourcePart_1.black.a += Time.deltaTime * 2f;
					SourcePart_1.shadow.material.color = SourcePart_1.black;
				}
			}
			if (SourcePart_1.MiniMapCamera.activeSelf && !SourcePart_1.MiniMapOpen)
			{
				SourcePart_1.MiniMapOpen = true;
				StartCoroutine(_part_1_start_1(0f));
			}
			if (SourcePart_1.MiniMapOpen && PopupAppStore.activeSelf)
			{
				if (!SourcePart_1.MiniMapOpenStoreOpen)
				{
					SourcePart_1.MiniMapOpenStoreOpen = true;
					Help.HideLayer();
				}
			}
			else if (SourcePart_1.MiniMapOpenStoreOpen)
			{
				SourcePart_1.MiniMapOpenStoreOpen = false;
				Help.ShowLayer();
			}
			if (SourcePart_1.MiniMapOpen && !SourcePart_1.MiniMapClose && !SourcePart_1.MiniMapCamera.activeSelf)
			{
				SourcePart_1.MiniMapClose = true;
				StartCoroutine(_part_1_start_2(0f));
			}
		}
		if (_cutScenePart == CutScenePart.Part_2)
		{
			if (!Part_2)
			{
				Part_2 = true;
				StartCoroutine(_part_2_start(0f));
			}
			if (SourcePart_2.BoxEli != null && !SourcePart_2.BoxEli.activeSelf)
			{
				weitHand = 2.5f;
				Help.DisableHelp("fire");
				SourcePart_2.BoxEli = null;
				StartCoroutine(ShowArrow(SourcePart_2.BarrelEli.transform.position + Vector3.up * 0.5f, 0f));
				SourcePart_2.BarrelEliColl.enabled = true;
			}
			if (SourcePart_2.BarrelEli != null && !SourcePart_2.BarrelEli.activeSelf)
			{
				SourcePart_2.BarrelEli = null;
				StartCoroutine(HideArrow(0f));
				SourcePart_2.prf_Health.SetActive(true);
				weitHand = 2.5f;
				Help.DisableHelp("fire");
				StartCoroutine(_part_2_start_1(0f));
			}
			if (SourcePart_2.prf_Health != null && !SourcePart_2.prf_Health.activeSelf && SourcePart_2.BarrelEli == null && SourcePart_2.BoxEli == null)
			{
				SourcePart_2.prf_Health = null;
				StartCoroutine(TransformScale(SourcePart_2.NewPlayerHP, 1.4f, 0.66f, 0.66f, 0f, 0.75f));
				StartCoroutine(HideArrow(0f));
			}
			if (SourcePart_2.KickParent)
			{
				_prontoController.transform.position = Vector3.Slerp(_prontoController.transform.position, SourcePart_2.ExposionParent.position, Time.deltaTime * 6f);
				_prontoController.transform.rotation = SourcePart_2.ExposionParent.rotation;
				_prontoController.Animator.transform.localEulerAngles = Vector3.zero;
			}
			if (!SourcePart_2.GetMoney && SourcePart_2.OldMoneyCount != MonoSingleton<AppController>.Instance.Model.Wallet.Gold)
			{
				SourcePart_2.GetMoney = true;
				StartCoroutine(TransformScale(SourcePart_2.MoneyHUD, 1.2f, 0.66f, 0.66f, 0f, 0.75f));
			}
			if (weitHand >= 0f)
			{
				weitHand -= Time.deltaTime;
			}
			if (weitHand < 0f && SourcePart_2.BarrelEli != null && Arrow.activeSelf)
			{
				Help.ShowHelp("fire", Arrow.transform, Vector3.zero);
			}
		}
		if (_cutScenePart == CutScenePart.Part_3)
		{
			if (!Part_3)
			{
				Part_3 = true;
				StartCoroutine(_part_3_start(0f));
				weitHand = 10f;
			}
			if (weitHand >= 0f)
			{
				weitHand -= Time.deltaTime;
			}
			if (_cutScenePart != CutScenePart.Part_4 && SourceCommon._sentinelController.AttackTarget != null)
			{
				_cutScenePart = CutScenePart.Part_4;
				Help.DisableHelp("select");
			}
		}
		if (_cutScenePart == CutScenePart.Part_4)
		{
			if (!Part_4)
			{
				Part_4 = true;
				DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvent(InputEvent.DoubleTap);
				StartCoroutine(_part_4_start(0f));
				Help.DisableHelp("select");
			}
			if (true)
			{
				if (!SourcePart_4.EnemyKick && SourceCommon._sentinelController.Model.Health <= 500f)
				{
					SourcePart_4.EnemyKick = true;
					SourceCommon._sentinelController.Model.Invulnerability = true;
					SourceCommon._sentinelController.SetViewRadius(0f);
					SourceCommon._sentinelController.AttackTarget = null;
					SourceCommon._sentinelController.SetAttackTarget(null);
					SourcePart_4.SentilelArea.center += Vector3.up * 25f;
					SourceCommon._sentinelController.OnFallDown();
					_eliController.UseAutoattack = false;
					_eliController.CanAttack = false;
					_eliController.DamageSystem.Clear();
					_eliController.AttackTarget = null;
					_eliController.SetAttackTarget(null);
					SourceCommon._sentinelController.enabled = false;
					_prontoController.SetAttackTarget(null);
					_prontoController.enabled = false;
					StartCoroutine(MoveTargetUnit(SourceCommon._trixieController, 8f, SourceCommon._trixieController.transform.position, 0.3f));
					SourceCommon._trixieController.SetAttackTarget(null);
					SourceCommon._trixieController.enabled = false;
					SourcePart_4.MostTouch.enabled = false;
					SourceCommon._sentinelTouch.enabled = false;
					Help.DisableHelp("select");
					StartCoroutine(_part_4_start_1_fixDT(1f));
				}
			}
			else if (!SourcePart_4.EnemyKick && SourceCommon._sentinelController.Model.Health <= 500f)
			{
				SourcePart_4.EnemyKick = true;
				SourceCommon._sentinelController.Model.Invulnerability = true;
				SourceCommon._sentinelController.SetViewRadius(0f);
				SourceCommon._sentinelController.SetAttackTarget(null);
				SourcePart_4.SentilelArea.center += Vector3.up * 25f;
				_eliController.AttackTarget = SourceCommon._sentinelController.transform;
				_eliController.CanAttack = false;
				StartCoroutine(MoveTargetUnit(_eliController, 4f, _cameraSmoothFollow.transform.position - _cameraSmoothFollow.cameraOffset));
				SourceCommon._sentinelController.enabled = false;
				_prontoController.SetAttackTarget(null);
				_prontoController.enabled = false;
				StartCoroutine(MoveTargetUnit(SourceCommon._trixieController, 8f, SourceCommon._trixieController.transform.position, 0.3f));
				SourceCommon._trixieController.SetAttackTarget(null);
				SourceCommon._trixieController.enabled = false;
				SourceCommon._sentinelController.OnFallDown();
				DisableMove();
				SourcePart_4.MostTouch.enabled = false;
				_cameraSmoothFollow.SetPlayer(SourceCommon._sentinelController.transform);
				Help.DisableHelp("select");
				StartCoroutine(_part_4_start_1(2f));
			}
			timer += Time.deltaTime;
			if (!SourcePart_4.EnemyKick && SourceCommon._trixieController.AttackTarget != SourceCommon._sentinelController.transform && timer > 0.5f)
			{
				SourceCommon._trixieController.SetAttackTarget(SourceCommon._sentinelController.transform);
				timer += Time.deltaTime;
				if (timer > 0.5f)
				{
					timer = 0f;
				}
			}
			if (SourcePart_4.EnemyKick && SourceCommon._trixieController.AttackTarget != null)
			{
				SourceCommon._trixieController.SetAttackTarget(null);
			}
			if (!CutSceneUse)
			{
				for (int i = 0; i < SourceCommon.CustomSpeed.Length; i++)
				{
					if (SourceCommon.CustomSpeed[i] != null && SourceCommon.CustomSpeed[i].speed > SourceCommon.newSpeed)
					{
						SourceCommon.CustomSpeed[i].speed = SourceCommon.newSpeed;
					}
				}
			}
			if (SourcePart_4.TargetFalse)
			{
				if (SourceCommon._eliAttackTarget.activeSelf)
				{
					if (!SourcePart_4.FixDtap)
					{
						SourcePart_4.FixDtap = true;
						StartCoroutine(EliReadyFire(5f));
					}
				}
				else
				{
					SourcePart_4.TargetFalse = false;
					StartCoroutine(SetActiveFalse(SourcePart_4.Hand_DT, 0f));
					DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
					InputEnable = false;
					EnableMove();
					SourceCommon._sentinelController.OnStandUp();
					StartCoroutine(MoveTargetUnit(SourceCommon._sentinelController, 5f, SourcePart_4.SentilelGoHome.position, 1.6f));
					StartCoroutine(MoveTargetUnit(SourceCommon._sentinelController, 5f, SourcePart_4.SentilelGoHome.position, 2f));
					StartCoroutine(MoveTargetUnit(SourceCommon._sentinelController, 5f, SourcePart_4.SentilelGoHome.position, 2.4f));
					StartCoroutine(MoveTargetUnit(SourceCommon._sentinelController, 5f, SourcePart_4.SentilelGoHome.position, 2.8f));
					StartCoroutine(MoveTargetUnit(SourceCommon._sentinelController, 5f, SourcePart_4.SentilelGoHome.position, 3f));
					StartCoroutine(MoveTargetUnit(SourceCommon._sentinelController, 5f, SourcePart_4.SentilelGoHome.position, 3.8f));
					StartCoroutine(_part_4_start_cutScene(3f));
				}
			}
			if (weitHand >= 0f)
			{
				weitHand -= Time.deltaTime;
			}
			if (weitHand < 0f && !SourcePart_4.EnemyKick)
			{
				if (!SourceCommon._eliAttackTarget.activeSelf || Vector3.Distance(SourceCommon._eliAttackTarget.transform.position, SourceCommon._sentinelController.transform.position) > 1f)
				{
					Help.ShowHelp("select", SourceCommon._sentinelController.transform, Vector3.up * 1.9f);
				}
				else
				{
					Help.DisableHelp("select");
				}
			}
			if (_eliController.CanSwipe && SourcePart_4.SwipeGo && Vector3.Distance(_eliController.transform.position, SourcePart_4.EliEnemyPoint.position) > 1f)
			{
				_cutScenePart = CutScenePart.Part_5;
			}
			if (SourcePart_4.EnemyKick)
			{
				if (PopupAppStore.activeSelf)
				{
					Help.HideLayer();
				}
				else
				{
					Help.ShowLayer();
				}
			}
		}
		if (_cutScenePart == CutScenePart.Part_5)
		{
			if (!Part_5)
			{
				Part_5 = true;
				StartCoroutine(_part_5_start(0f));
			}
			if (_eliController.Model.Health < 800f)
			{
				_eliController.Model.AddHealth(10f);
				if (_eliController.AttackTarget == null)
				{
					_eliController.OnBeginAttack();
				}
			}
			if (_prontoController.Model.Health < 250f)
			{
				_prontoController.Model.AddHealth(10f);
			}
			timer += Time.deltaTime;
			if (!SourcePart_5.Part_5_Go)
			{
				int num = 0;
				for (int j = 0; j < SourcePart_4.Enemy.Length; j++)
				{
					if (SourcePart_4.Enemy[j] == null)
					{
						num++;
					}
					if (num == SourcePart_4.Enemy.Length)
					{
						SourcePart_5.Part_5_Go = true;
						_eliController.UseAutoattack = false;
						_eliController.SetAttackTarget(null);
						_eliController.CanSwipe = false;
						StartCoroutine(_part_5_start_CutScene(3f));
					}
				}
				timer += Time.deltaTime;
				if (timer > 0.5f)
				{
					timer = 0f;
				}
			}
			if (Time.timeScale < 1f)
			{
				if (PopupConfirm.activeSelf || PopupAppStore.activeSelf)
				{
					Help.DisableHelp("handfinish");
				}
				else
				{
					Help.ShowHelp("handfinish", null, Vector3.zero);
				}
			}
			if (!CutSceneUse)
			{
				for (int k = 0; k < SourceCommon.CustomSpeed.Length; k++)
				{
					if (SourceCommon.CustomSpeed[k] != null && SourceCommon.CustomSpeed[k].speed > SourceCommon.newSpeed)
					{
						SourceCommon.CustomSpeed[k].speed = SourceCommon.newSpeed;
					}
				}
			}
		}
		if (!_cameraSmoothFollow.enabled && timerShake > 0f)
		{
			newCameraPos = oldCameraPos;
			if (timerShake > 0f)
			{
				timerShake -= Time.deltaTime;
				if (Time.timeScale > 0f)
				{
					if (!SmoothShake)
					{
						Shake = UnityEngine.Random.insideUnitSphere * RadiusShake;
					}
					else
					{
						Shake = Vector3.Lerp(Shake, UnityEngine.Random.insideUnitSphere * RadiusShake, Time.deltaTime * ReduceSmooth);
					}
					newCameraPos += Shake;
				}
			}
			else if (Shake != Vector3.zero)
			{
				Shake = Vector3.zero;
				newCameraPos = oldCameraPos;
			}
			MainCamera.transform.position = newCameraPos;
		}
		if (PopupConfirm.activeSelf && !PopupConfirmShow)
		{
			if (!PopupConfirmShow)
			{
				PopupConfirmShow = true;
			}
		}
		else if (!PopupConfirm.activeSelf && PopupConfirmShow)
		{
			PopupConfirmShow = false;
			No();
		}
	}

	public void SkipTutor()
	{
		if (UICamera.GetComponent<Camera>().enabled)
		{
			checkSkip = true;
			Time.timeScale = 0f;
			PopupConfirm.SetActive(true);
			HUDPanel.SetActive(false);
			Help.DisableHelp("radar");
			btnYes.targetObject = gameObject;
			btnYes.messageName = "Yes";
			btnNo.targetObject = gameObject;
			btnNo.messageName = "No";
			PopupConfirmText.text = MonoSingleton<Localization>.Instance["defeat_skip_desc"];
			gameController.PlayCutScene(true);
			CutSceneUse = true;
			Time.timeScale = 0f;
		}
	}

	public void Yes()
	{
		PlayerPrefs.SetInt(typeof(ShaneTutor_0_1).Name, 1);
		DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
		InputEnable = true;
		DonDestroyMonoSingleton<InputManager>.Instance.BackBtnEvent -= SkipTutor;
		gameController.PlayCutScene(false);
		CutSceneUse = false;
		RisePlayCutSceneEvent(false);
		Time.timeScale = 0f;
		LoadingScreen.SetActive(true);
		UICamera.GetComponent<Camera>().enabled = false;
		MainCamera.GetComponent<Camera>().enabled = false;
		SourcePart_1.shadow.enabled = false;
		_eliController.CanSwipe = true;
		gameController.SkipMission();
		if (_inGameView != null)
		{
			_inGameView.OnMinimapEvent -= OnMinimapOpen;
		}
		if (_minimapView != null)
		{
			_minimapView.OnBackEvent -= OnMinimapClose;
		}
	}

	public void No()
	{
		if (checkSkip)
		{
			checkSkip = false;
			Time.timeScale = 1f;
			PopupConfirmShow = false;
			btnYes.targetObject = PopupConfirm;
			btnYes.messageName = "OnBtnAcceptClick";
			btnNo.targetObject = PopupConfirm;
			btnNo.messageName = "OnBtnDeclineClick";
			StartCoroutine(SetActiveHelpTrue("radar", null, Vector3.zero, 0.5f));
			PopupConfirm.SetActive(false);
			gameController.PlayCutScene(false);
			CutSceneUse = false;
			Time.timeScale = 1f;
		}
	}

	public void End()
	{
		Help.DelHelp("handfinish");
	}

	public void DoShake(float timer = 0f)
	{
		oldCameraPos = MainCamera.transform.position;
		if (timer == 0f)
		{
			timerShake = TimeShake;
		}
		else
		{
			timerShake = timer;
		}
	}
}

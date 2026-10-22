using System;
using System.Collections;
using System.Collections.Generic;
using AppsMinistry.Core;
using AppsMinistry.Core.Input;
using AppsMinistry.Core.State;
using AppsMinistry.UI.Camera;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.Shops;
using Slugterra.Game.Duel;
using Slugterra.Game.Environment;
using Slugterra.Game.Units;
using Slugterra.Game.Units.Ally;
using Slugterra.Game.Units.Enemy;
using Slugterra.Game.Units.Slugs;
using Slugterra.Scene;
using Slugterra.Scene.Game;
using Slugterra.UI.Game;
using UnityEngine;

public class CutScene_0_1_2_Interactive : SlugterraCutScene
{
	public enum CutScenePart
	{
		Part_0 = 0,
		Part_1 = 1,
		Part_2 = 2,
		Part_2_1 = 3,
		Part_2_2 = 4,
		Part_3 = 5,
		Part_4 = 6,
		Part_Restart = 7
	}

	[Serializable]
	public class _part_0
	{
		[HideInInspector]
		public bool close;

		public GameObject rightTrigger;

		public GameObject leftTrigger;

		public Transform LeftArrow;

		public Transform RightArrow;

		[HideInInspector]
		public bool QuestGoRight;

		[HideInInspector]
		public bool QuestGoLeft;
	}

	[Serializable]
	public class _part_1
	{
		[HideInInspector]
		public bool close;

		public float EliHealthCurent;

		public bool FixDamage;

		public Transform SlugMedicine;

		public tk2dButton BtnMedicine;

		public bool QuestEliDamage;

		public bool QuestAddHealth;
	}

	[Serializable]
	public class _part_2
	{
		[HideInInspector]
		public bool close;

		public bool close_2_1;

		public bool close_2_2;

		public Transform SlugFire;

		public GameObject SlugKord;

		public bool FindSlugKord;

		public Transform ArrowRight;

		public Transform ArrowLeft;

		[HideInInspector]
		public bool QuestFirstFire;

		[HideInInspector]
		public bool QuestFireComplite;

		[HideInInspector]
		public bool QuestMoveSlug;
	}

	[Serializable]
	public class _part_3
	{
		[HideInInspector]
		public bool close;

		public Transform SlugFood;

		public tk2dButton BtnSlugFood;

		public tk2dButton BtnSlugRun;

		public tk2dButton BtnSlugArmor;

		[HideInInspector]
		public bool QuestSlugRestart;

		[HideInInspector]
		public bool QuestFoot;

		[HideInInspector]
		public bool UseSlagFoot;
	}

	[Serializable]
	public class _part_4
	{
		[HideInInspector]
		public bool close;

		public float KordHelthStart;

		public float EliHealthCurent;

		public float MinEliHeath = 200f;

		[HideInInspector]
		public bool QuestFight;

		[HideInInspector]
		public bool QuestGoBattle;

		[HideInInspector]
		public bool Part_0_4_fix;
	}

	public bool CutSceneUse;

	public bool InputEnable;

	private TutorHelpInput Help;

	private TutorHelpInput HelpLayer2;

	[SerializeField]
	private GameController gameController;

	public GameObject _SlugButtonOn;

	public GameObject _SlugButtonOff;

	private InGameView _inGameView;

	private EliController _eliController;

	private BossKordController _kordController;

	public OnScreenTarget ScreenHand;

	public Transform StartHintHandSlice;

	public TextMesh help;

	private GameObject MainCamera;

	private GameObject UICamera;

	private CameraFade _cameraFade;

	public GameObject DuelCamera;

	private GameObject FightCamera;

	public GameObject[] Text;

	public TextMesh[] AdditionalText;

	public string[] NewText;

	public GameObject[] TopPanel;

	private int t;

	public GameObject BoostPanel;

	private bool BoostActive;

	public GameObject SlugPanel;

	private bool SlugActive;

	public GameObject SlugButton;

	private tk2dUIItem useSlugButton;

	private GameObject ParantSlugButton;

	private bool SlugButtonDown;

	public GameObject FadeStart;

	public GameObject ButomNext;

	public GameObject PopupAppStore;

	public GameObject PopupConfirm;

	public TextMesh PopupConfirmText;

	public bool PopupConfirmShow;

	public bool PopupAppStoreShow;

	public GameObject PausePanel;

	public tk2dButton btnYes;

	public tk2dButton btnNo;

	public GameObject HUDPanel;

	public GameObject ReliseSlug;

	private tk2dButton useReliseSlug;

	public bool ReliseTutor;

	public Transform[] warNode;

	private Vector3[] warNodeStartPos;

	public CutScenePart _cutScenePart;

	public _part_0 Part_0;

	public _part_1 Part_1;

	public _part_2 Part_2;

	public _part_3 Part_3;

	public _part_4 Part_4;

	private float prontoWeit;

	private float prontoWeitHate;

	private int CountProntotext;

	private bool ActiveInput;

	private bool ShowText;

	private bool Duelhelp;

	private bool part_0_rightTrigger;

	private bool part_0_leftTrigger;

	public GameObject Eli;

	public GameObject Eli_Target;

	public GameObject Eli_Move_Target;

	public GameObject Eli_models;

	public float EliHealthStart;

	public GameObject Kord;

	public GameObject Kord_Move_Target;

	public bool kord_stoped;

	private Vector3 onlyY;

	private bool notReady;

	private Vector3 onlyB;

	private Vector3 offsetHand;

	private List<ConsumableItem> items;

	private bool death;

	private bool finish;

	private float fixSpline;

	private Vector3 SplinePos;

	private float fixSplineOld;

	public GameObject Fx_info;

	public GameObject FxBarrels;

	public GameObject[] barrels;

	private bool onMuveSlug;

	public float fix;

	public InGameView reliseAdd;

	private bool reliseCompleted;

	private bool FireCompleted;

	private bool reliseCompletedFinish;

	public Camera IO_Camera;

	private GameObject Infernus;

	private bool InfernusFind;

	private bool TimeBrake;

	public BoxCollider[] Touch;

	public GameObject[] Glow;

	public ArenaInteractiveEnvironmant[] Iobjects;

	public Transform pipe_1;

	public Transform pipe_2;

	public Transform bomb_1;

	public Transform bomb_2;

	private void Start()
	{
		ActiveInput = false;
		MainCamera = GameObject.Find("MainCamera");
		UICamera = GameObject.Find("UICamera");
		_inGameView = UnityEngine.Object.FindObjectOfType<InGameView>();
		Help = GetComponents<TutorHelpInput>()[0];
		HelpLayer2 = GetComponents<TutorHelpInput>()[1];
		useSlugButton = SlugButton.GetComponent<tk2dUIItem>();
		useSlugButton.OnClick += OnBtnUseClick;
		ReliseSlug.SetActive(false);
		useReliseSlug = ReliseSlug.GetComponentInChildren<tk2dButton>();
		useReliseSlug.ButtonPressedEvent += OnBtnReleaseClick;
		DonDestroyMonoSingleton<InputManager>.Instance.OnButtonDownEvent += OnButtonDownEvent;
		DonDestroyMonoSingleton<InputManager>.Instance.OnButtonPressEvent += OnButtonDownEvent;
		Part_3.BtnSlugFood.ButtonPressedEvent += OnBtnUseFood;
		gameController = UnityEngine.Object.FindObjectOfType<GameController>();
		gameController.PlayCutScene(true);
		CutSceneUse = true;
		_cameraFade = MainCamera.GetComponent<CameraFade>();
		_eliController = UnityEngine.Object.FindObjectOfType<EliController>();
		_kordController = UnityEngine.Object.FindObjectOfType<BossKordController>();
		Kord = _kordController.gameObject;
		Kord_Move_Target = _kordController.transform.parent.transform.Find("MoveTarget").gameObject;
		_kordController.GetComponent<UnitPathFinder>().enabled = false;
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 1f), 1f, 0.1f, 2f);
		_kordController.WaitInWarPointTime = 0f;
		warNodeStartPos = new Vector3[warNode.Length];
		for (int i = 0; i < warNodeStartPos.Length; i++)
		{
			warNodeStartPos[i] = warNode[i].position;
			warNode[i].position = _kordController.transform.position;
		}
		ScreenHand.SwitchTarget(_kordController.transform);
		ParantSlugButton = SlugButton.GetComponent<tk2dUIItem>().sendMessageTarget;
		for (int j = 0; j < Touch.Length; j++)
		{
			Touch[j].enabled = false;
		}
		for (int k = 0; k < Glow.Length; k++)
		{
			Glow[k].SetActive(false);
		}
		items = MonoSingleton<AppController>.Instance.Model.ShopModel.Consumables;
		foreach (ConsumableItem item in items)
		{
			if (item.Type == ConsumableType.First && item.Count < 1)
			{
				item.Count = 1;
				break;
			}
		}
		foreach (ConsumableItem item2 in items)
		{
			if (item2.Type == ConsumableType.SlugFood && item2.Count < 1)
			{
				item2.Count = 1;
				break;
			}
		}
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		InputEnable = false;
		StartCoroutine(SetActiveFalse(FadeStart, 0.5f));
		ButomNext.SetActive(false);
		SlugPanel.SetActive(false);
		BoostPanel.SetActive(false);
		UICamera.GetComponent<Camera>().enabled = false;
		HUDPanel.SetActive(false);
		Part_0.rightTrigger.SetActive(false);
		Part_0.leftTrigger.SetActive(false);
		notReady = true;
		for (int l = 0; l < barrels.Length; l++)
		{
			barrels[l].SetActive(false);
		}
		DonDestroyMonoSingleton<InputManager>.Instance.OnAccelerometerX += MoveSlugAccelerometer;
		DonDestroyMonoSingleton<InputManager>.Instance.OnTouch += MoveSlugTouch;
		DonDestroyMonoSingleton<InputManager>.Instance.OnJoysticOnMove_movement += MoveSlugJoystic;
		_kordController.SetAttackTarget(null);
		Eli_Target.SetActive(false);
		StartCoroutine(AfterStart(0.1f));
		DonDestroyMonoSingleton<InputManager>.Instance.BackBtnEvent += SkipTutor;
	}

	private void MoveSlugAccelerometer(float temp)
	{
		if (Mathf.Abs(temp) > 0.25f)
		{
			OnMoveSlug();
		}
	}

	private void MoveSlugTouch(Vector3 temp, float actionTime)
	{
		OnMoveSlug();
	}

	private void MoveSlugJoystic(Vector2 temp)
	{
		OnMoveSlug();
	}

	private void OnMoveSlug()
	{
		if (MonoSingleton<DuelManager>.Instance.StateMachine.CurrentState is SlugFlyState)
		{
			onMuveSlug = true;
		}
	}

	private void OnDisable()
	{
		useSlugButton.OnClick -= OnBtnUseClick;
		Part_3.BtnSlugFood.ButtonPressedEvent -= OnBtnUseFood;
		useReliseSlug.ButtonPressedEvent -= OnBtnReleaseClick;
		DonDestroyMonoSingleton<InputManager>.Instance.OnAccelerometerX -= MoveSlugAccelerometer;
		DonDestroyMonoSingleton<InputManager>.Instance.OnTouch -= MoveSlugTouch;
		DonDestroyMonoSingleton<InputManager>.Instance.OnJoysticOnMove_movement -= MoveSlugJoystic;
		DonDestroyMonoSingleton<InputManager>.Instance.OnButtonDownEvent -= OnButtonDownEvent;
		DonDestroyMonoSingleton<InputManager>.Instance.OnButtonPressEvent -= OnButtonDownEvent;
	}

	private IEnumerator AfterStart(float t)
	{
		yield return new WaitForSeconds(t);
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		InputEnable = false;
	}

	private IEnumerator ShowTextHint(string ico, string text, float weit)
	{
		yield return new WaitForSeconds(weit);
		gameController.PlayCutScene(true);
		CutSceneUse = true;
		if (!ShowText)
		{
			UICamera.GetComponent<Camera>().enabled = false;
			StartCoroutine(SetActiveFalse(HUDPanel, 0f));
			_cutSceneView.ShowScreens(true);
		}
		Help.DisableHelp("useslug");
		Help.DisableHelp("useboost");
		BoostPanel.SetActive(false);
		SlugPanel.SetActive(false);
		_cutSceneView.SpeakerIcon = ico;
		_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance[text];
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		InputEnable = false;
		ShowText = true;
		StartCoroutine(SetActiveHelpTrue("skip", null, Vector3.zero, 0.45f));
		StartCoroutine(TimeZero(0.5f));
	}

	private IEnumerator HideTextHint(float weit)
	{
		yield return new WaitForSeconds(weit);
		_cutSceneView.ShowScreens(false);
		if (ActiveInput)
		{
			DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
			InputEnable = true;
		}
		else
		{
			DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
			InputEnable = false;
		}
		Help.DisableHelp("skip");
		StartCoroutine(ShowUIAuto(0.5f));
		ShowText = false;
	}

	private IEnumerator HideTextHintFade(float weit)
	{
		yield return new WaitForSeconds(weit);
		_cutSceneView.ShowScreens(false);
		gameController.PlayCutScene(false);
		CutSceneUse = false;
		if (ActiveInput)
		{
			DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
			InputEnable = true;
		}
		else
		{
			DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
			InputEnable = false;
		}
		ShowText = false;
	}

	private IEnumerator ShowUIAuto(float weit)
	{
		yield return new WaitForSeconds(weit);
		gameController.PlayCutScene(false);
		CutSceneUse = false;
		if (BoostActive)
		{
			BoostPanel.SetActive(true);
		}
		if (SlugActive)
		{
			SlugPanel.SetActive(true);
		}
		StartCoroutine(SetActiveTrue(HUDPanel, 0f));
		UICamera.GetComponent<Camera>().enabled = true;
	}

	private IEnumerator TimeZero(float weit)
	{
		yield return new WaitForSeconds(weit);
		ButomNext.SetActive(true);
		ButomNext.GetComponent<BoxCollider>().enabled = true;
		Time.timeScale = 0f;
	}

	private IEnumerator BoostOn(float weit)
	{
		yield return new WaitForSeconds(weit);
		BoostActive = true;
	}

	private IEnumerator SlugOn(float weit)
	{
		yield return new WaitForSeconds(weit);
		SlugActive = true;
	}

	private IEnumerator DisableInput(float weit)
	{
		yield return new WaitForSeconds(weit);
		ActiveInput = false;
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		InputEnable = false;
	}

	private IEnumerator EnableInput(float weit)
	{
		yield return new WaitForSeconds(weit);
		ActiveInput = true;
		DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
		InputEnable = true;
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

	private IEnumerator Fade_Maincamera(float weit)
	{
		yield return new WaitForSeconds(weit);
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 0f), 1f, 1f, 2f);
	}

	private IEnumerator Find_Camera(float weit)
	{
		yield return new WaitForSeconds(weit);
		if (Part_2.QuestFireComplite && DuelCamera == null)
		{
			DuelCamera = MonoSingleton<DuelManager>.Instance.SlugSmoothFollow.gameObject;
			if (DuelCamera == null)
			{
				DuelCamera = GameObject.Find("DuelSlugCamera(Clone)");
			}
		}
		if (_cutScenePart == CutScenePart.Part_4 && FightCamera == null)
		{
			FightCamera = GameObject.Find("SlugFightCamera(Clone)");
		}
	}

	private IEnumerator Find_KordSlug(float weit)
	{
		yield return new WaitForSeconds(weit);
		SlugController temp = UnityEngine.Object.FindObjectOfType<SlugController>();
		if (temp != null)
		{
			Part_2.SlugKord = temp.gameObject;
		}
		else
		{
			StartCoroutine(Find_KordSlug(0.5f));
		}
		if ((bool)Part_2.SlugKord)
		{
			Part_2.FindSlugKord = true;
		}
	}

	private IEnumerator MoveTarget(int index, float speed, Vector3 target, float weit = 0f)
	{
		yield return new WaitForSeconds(weit);
		if (index == 0)
		{
			_eliController.MoveTarget.position = target;
			_eliController.OnMove(speed);
		}
		if (index == 1)
		{
			_kordController.MoveTarget.position = target;
			_kordController.OnMove(speed);
		}
	}

	private IEnumerator KordReadyFire(bool state, float weit)
	{
		yield return new WaitForSeconds(weit);
		if (state)
		{
			_kordController.AttackTarget = _eliController.Transform;
		}
		else
		{
			_kordController.SetAttackTarget(null);
		}
	}

	private IEnumerator KordReadyMove(bool state, float weit)
	{
		yield return new WaitForSeconds(weit);
		if (state)
		{
			kord_stoped = false;
			Kord.GetComponent<UnitPathFinder>().enabled = true;
			Kord.GetComponent<StateMachine>().enabled = true;
		}
		else
		{
			Kord.GetComponent<UnitPathFinder>().enabled = false;
			Kord.GetComponent<StateMachine>().enabled = false;
		}
	}

	private IEnumerator EliReadyFire(bool state, float weit)
	{
		yield return new WaitForSeconds(weit);
		if (state)
		{
			notReady = false;
			_eliController.SetAttackTarget(Kord.transform);
			Eli_Target.SetActive(true);
		}
		else
		{
			notReady = true;
			_eliController.enabled = false;
		}
	}

	private IEnumerator ShowHand(string key, Transform obj, Vector3 offset, float weit)
	{
		yield return new WaitForSeconds(weit);
		Help.ShowHelp(key, obj, offset);
	}

	private IEnumerator ShowHand_R(string key, Transform obj, Vector3 offset, float weit)
	{
		yield return new WaitForSeconds(weit);
		Help.ShowHelp(key, obj, offset);
	}

	private IEnumerator ShowHand_L(string key, Transform obj, Vector3 offset, float weit)
	{
		yield return new WaitForSeconds(weit);
		Help.ShowHelp(key, obj, offset);
	}

	private IEnumerator HideHand(float weit)
	{
		yield return new WaitForSeconds(weit);
		Help.DisableHelp("goleft");
		Help.DisableHelp("goright", StartHintHandSlice);
		Help.DisableHelp("useslug");
		Help.DisableHelp("useboost");
		Help.DisableHelp("accelerometer");
	}

	private IEnumerator Mission(int index, bool state, float weit)
	{
		yield return new WaitForSeconds(weit);
		if (index == 0)
		{
			Part_0.QuestGoRight = state;
			if (Part_0.rightTrigger != null)
			{
				Part_0.rightTrigger.SetActive(true);
			}
		}
		if (index == 1)
		{
			Part_0.QuestGoLeft = state;
			if (Part_0.leftTrigger != null)
			{
				Part_0.leftTrigger.SetActive(true);
			}
		}
		if (index == 2)
		{
			Part_1.QuestAddHealth = state;
		}
		if (index == 3)
		{
			Part_2.QuestFirstFire = state;
		}
		if (index == 4)
		{
			Part_2.QuestMoveSlug = state;
		}
		if (index == 5)
		{
			Part_3.QuestSlugRestart = state;
		}
		if (index == 6)
		{
			Part_4.QuestFight = state;
		}
	}

	private void LookNode(Vector3 GetNodeLook)
	{
		Eli.transform.forward = Vector3.Slerp(Eli.transform.forward, -(Eli.transform.position - GetNodeLook), Time.deltaTime * 8f);
		onlyB.y = Eli.transform.eulerAngles.y;
		Eli.transform.eulerAngles = onlyB;
	}

	public void Right_part()
	{
		part_0_rightTrigger = true;
	}

	public void Left_part()
	{
		part_0_leftTrigger = true;
		StartCoroutine(HideHand(0f));
	}

	private void OnBtnUseClick()
	{
		if (!SlugButtonDown && !_SlugButtonOff.activeSelf)
		{
			SlugButtonDown = true;
			Part_2.QuestFireComplite = true;
			StartCoroutine(Find_Camera(0.4f));
		}
	}

	private void OnBtnUseFood(tk2dButton source)
	{
		if (!SlugButtonDown)
		{
			SlugButtonDown = true;
			Part_3.UseSlagFoot = true;
		}
	}

	private void Next()
	{
		ButomNext.SetActive(false);
		ButomNext.GetComponent<BoxCollider>().enabled = false;
		Time.timeScale = 1f;
	}

	public void OnBtnReleaseClick(tk2dButton source)
	{
		reliseCompleted = true;
	}

	private void OnButtonDownEvent(InputEvent e)
	{
		if (MonoSingleton<DuelManager>.Instance.StateMachine.CurrentState is SlugFlyState && _inGameView.ReleaseBar != null && e == InputEvent.Enter)
		{
			reliseCompleted = true;
		}
	}

	private void KordWeit()
	{
		if (ShowText)
		{
			return;
		}
		if (_cutScenePart == CutScenePart.Part_0 && ((Part_0.QuestGoRight && !part_0_rightTrigger) || (Part_0.QuestGoLeft && !part_0_leftTrigger)))
		{
			if (Part_0.QuestGoRight && (bool)Part_0.rightTrigger)
			{
				fixSpline = (int)Vector3.Distance(Eli.transform.position, Part_0.rightTrigger.transform.position);
				SplinePos = Part_0.rightTrigger.transform.position;
			}
			if (Part_0.QuestGoLeft && (bool)Part_0.leftTrigger)
			{
				fixSpline = (int)Vector3.Distance(Eli.transform.position, Part_0.leftTrigger.transform.position);
				SplinePos = Part_0.leftTrigger.transform.position;
			}
			if (_eliController.Animator.GetFloat("VelocityX") == 0f && _eliController.Animator.GetFloat("RollDirectionX") == 0f)
			{
				prontoWeit += Time.deltaTime;
				prontoWeitHate += Time.deltaTime;
				if (prontoWeit > 10f)
				{
				}
				if (prontoWeitHate > 0.4f)
				{
					if (Part_0.QuestGoRight)
					{
						offsetHand.x = -0.2f;
						offsetHand.y = -0.76f;
						offsetHand.z = 0f;
						StartCoroutine(ShowHand("goleft", Part_0.RightArrow, offsetHand, 0f));
					}
					if (Part_0.QuestGoLeft)
					{
						offsetHand.x = 0.2f;
						offsetHand.y = -0.76f;
						offsetHand.z = 0f;
						StartCoroutine(ShowHand("goright", null, Vector3.zero, 0f));
					}
				}
			}
			else if (prontoWeit != 0f)
			{
				prontoWeit = 0f;
				prontoWeitHate = 0f;
				CountProntotext = UnityEngine.Random.Range(0, 3);
				StartCoroutine(HideHand(0f));
			}
			fixSplineOld = (int)Vector3.Distance(Eli.transform.position, SplinePos);
		}
		if (_cutScenePart == CutScenePart.Part_1 || _cutScenePart == CutScenePart.Part_4)
		{
			if (Part_1.QuestAddHealth && BoostPanel.activeSelf)
			{
				prontoWeit += Time.deltaTime;
				prontoWeitHate += Time.deltaTime;
				if (prontoWeit > 10f)
				{
					prontoWeit = 0f;
				}
				if (prontoWeitHate > 1f)
				{
					offsetHand.x = 0.215f;
					offsetHand.y = -0.7335f;
					offsetHand.z = 0f;
					offsetHand.x = 0.4f;
					offsetHand.y = 0f;
					offsetHand.z = 0f;
					StartCoroutine(ShowHand_L("useboost", Part_1.SlugMedicine, offsetHand, 0f));
				}
			}
			else if (prontoWeit != 0f)
			{
				prontoWeit = 0f;
				prontoWeitHate = 0f;
				CountProntotext = UnityEngine.Random.Range(0, 3);
				StartCoroutine(HideHand(0f));
			}
		}
		if (_cutScenePart == CutScenePart.Part_2 && DuelCamera == null)
		{
			if (Part_2.QuestFirstFire && !Part_2.QuestFireComplite)
			{
				prontoWeit += Time.deltaTime;
				prontoWeitHate += Time.deltaTime;
				if (prontoWeit > 10f)
				{
					prontoWeit = 0f;
				}
				if (prontoWeitHate > 2f)
				{
					offsetHand.x = 0.215f;
					offsetHand.y = -0.3f;
					offsetHand.z = 0f;
					offsetHand.x = -0.4f;
					offsetHand.y = 0.8f;
					offsetHand.z = 0f;
					StartCoroutine(ShowHand_R("useslug", Part_2.SlugFire, offsetHand, 0f));
				}
			}
			else if (prontoWeit != 0f)
			{
				prontoWeit = 0f;
				prontoWeitHate = 0f;
				CountProntotext = UnityEngine.Random.Range(0, 3);
				StartCoroutine(HideHand(0f));
			}
		}
		if (_cutScenePart == CutScenePart.Part_2_1 && !FireCompleted)
		{
			if (!FireCompleted)
			{
				prontoWeit += Time.deltaTime;
				prontoWeitHate += Time.deltaTime;
				if (prontoWeit > 10f)
				{
					prontoWeit = 0f;
				}
				if (prontoWeitHate > 2f)
				{
					offsetHand.x = 0.215f;
					offsetHand.y = -0.3f;
					offsetHand.z = 0f;
					offsetHand.x = -0.4f;
					offsetHand.y = 0.8f;
					offsetHand.z = 0f;
					StartCoroutine(ShowHand_R("useslug", Part_2.SlugFire, offsetHand, 0f));
				}
			}
			else if (prontoWeit != 0f)
			{
				prontoWeit = 0f;
				prontoWeitHate = 0f;
				CountProntotext = UnityEngine.Random.Range(0, 3);
				StartCoroutine(HideHand(0f));
			}
		}
		if (Part_2.QuestFireComplite && _cutScenePart == CutScenePart.Part_2)
		{
			if (Part_2.QuestMoveSlug && DuelCamera != null && DuelCamera.activeSelf)
			{
				prontoWeit += Time.deltaTime;
				prontoWeitHate += Time.deltaTime;
				if (prontoWeit > 0.333f)
				{
					if (CountProntotext % 2 == 0)
					{
						offsetHand.x = -0.2f;
						offsetHand.y = -0.76f;
						offsetHand.z = 0f;
						StartCoroutine(ShowHand("accelerometer", Part_2.ArrowRight, offsetHand, 0f));
					}
					else
					{
						offsetHand.x = 0.2f;
						offsetHand.y = -0.76f;
						offsetHand.z = 0f;
						StartCoroutine(ShowHand("accelerometer", Part_2.ArrowLeft, offsetHand, 0f));
					}
					prontoWeit = 0f;
					CountProntotext++;
				}
			}
			else if (prontoWeit != 0f)
			{
				prontoWeit = 0f;
				prontoWeitHate = 0f;
				CountProntotext = UnityEngine.Random.Range(0, 3);
				StartCoroutine(HideHand(0f));
			}
		}
		if (!Part_3.QuestSlugRestart || _cutScenePart != CutScenePart.Part_3)
		{
			return;
		}
		if (!Part_3.UseSlagFoot && BoostPanel.activeSelf)
		{
			prontoWeit += Time.deltaTime;
			prontoWeitHate += Time.deltaTime;
			if (prontoWeit > 10f)
			{
				prontoWeit = 0f;
			}
			if (prontoWeitHate > 2f)
			{
				offsetHand.x = 0.215f;
				offsetHand.y = -0.7335f;
				offsetHand.z = 0f;
				offsetHand.x = 0.4f;
				offsetHand.y = 0f;
				offsetHand.z = 0f;
				StartCoroutine(ShowHand_L("useboost", Part_3.SlugFood, offsetHand, 0f));
			}
		}
		else if (prontoWeit != 0f)
		{
			prontoWeit = 0f;
			prontoWeitHate = 0f;
			CountProntotext = UnityEngine.Random.Range(0, 3);
			StartCoroutine(HideHand(0f));
		}
	}

	private void Update()
	{
		if (PopupConfirmShow && Time.timeScale == 0f && !PopupConfirm.activeSelf)
		{
			Time.timeScale = 1f;
			PopupConfirmShow = false;
			gameController.PlayCutScene(false);
			CutSceneUse = false;
			No();
		}
		if (PopupAppStore.activeSelf)
		{
			if (!PopupAppStoreShow)
			{
				PopupAppStoreShow = true;
				Help.HideLayer();
			}
		}
		else if (PopupAppStoreShow)
		{
			PopupAppStoreShow = false;
			StartCoroutine(ShowLayerHelp());
		}
		if (Time.timeScale == 0f)
		{
			if (IO_Camera.enabled)
			{
				IO_Camera.enabled = false;
			}
		}
		else if (!IO_Camera.enabled)
		{
			IO_Camera.enabled = true;
		}
		if (_kordController.Model.Health <= 1f && !death)
		{
			DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
			InputEnable = true;
			_eliController.Animator.updateMode = AnimatorUpdateMode.Normal;
			death = true;
			StartCoroutine(EliReadyFire(false, 0f));
			_kordController.OnFallDown();
			gameController.PlayCutScene(false);
			CutSceneUse = false;
			PlayerPrefs.SetInt(typeof(ScorchedSparksTutorial_1_1).Name, 1);
			DonDestroyMonoSingleton<InputManager>.Instance.BackBtnEvent -= SkipTutor;
		}
		if (_kordController.Model.Health < 150f && _cutScenePart != CutScenePart.Part_4 && _cutScenePart != CutScenePart.Part_Restart && !Part_4.QuestFight)
		{
			_kordController.Model.AddHealth(151f - _kordController.Model.Health);
		}
		if (_cutScenePart == CutScenePart.Part_0)
		{
			if (!Part_0.close)
			{
				DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
				InputEnable = false;
				StartCoroutine(Part_0_0());
				Part_0.close = true;
			}
			else
			{
				if (Part_0.QuestGoRight && part_0_rightTrigger)
				{
					part_0_rightTrigger = false;
					StartCoroutine(Mission(0, false, 0f));
					StartCoroutine(ShowTextHint("icon_kord", Text[5].GetComponent<TextMesh>().text, 0f));
					StartCoroutine(HideTextHint(1f));
					StartCoroutine(Mission(1, true, 1f));
				}
				if (Part_0.QuestGoLeft && part_0_leftTrigger)
				{
					part_0_leftTrigger = false;
					StartCoroutine(Mission(1, false, 0f));
					StartCoroutine(HideHand(0f));
					_cutScenePart = CutScenePart.Part_1;
				}
				KordWeit();
			}
		}
		if (_cutScenePart == CutScenePart.Part_1)
		{
			if (!Part_1.close)
			{
				StartCoroutine("Part_0_1");
				Part_1.close = true;
			}
			else
			{
				if ((Part_1.QuestEliDamage && EliHealthStart > _eliController.Model.Health) || Part_1.FixDamage)
				{
					Part_1.FixDamage = false;
					_eliController.Model.AddHealth(-100f);
					StartCoroutine(DisableInput(0f));
					Part_1.QuestEliDamage = false;
					StartCoroutine(KordReadyFire(false, 0f));
					StartCoroutine(Part_0_1_1(3f));
				}
				if (Part_1.QuestAddHealth && Part_1.EliHealthCurent < _eliController.Model.Health)
				{
					StartCoroutine(EnableInput(0f));
					Part_1.QuestAddHealth = false;
					Part_1.BtnMedicine.enabled = false;
					StartCoroutine(Part_0_1_2(1f));
				}
				KordWeit();
				if ((Part_2.FindSlugKord && EliHealthStart == _eliController.Model.Health && Part_2.SlugKord == null) || (Part_2.FindSlugKord && EliHealthStart == _eliController.Model.Health && Part_2.SlugKord != null && !Part_2.SlugKord.activeSelf))
				{
					Part_2.FindSlugKord = false;
					StartCoroutine(KordReadyFire(false, 0f));
					Part_1.EliHealthCurent = _eliController.Model.Health;
					Part_3.BtnSlugFood.enabled = false;
					Part_3.BtnSlugArmor.enabled = false;
					Part_3.BtnSlugRun.enabled = false;
					StartCoroutine(Part_0_1_2(0f));
				}
			}
		}
		if (_cutScenePart == CutScenePart.Part_2)
		{
			if (!Part_2.close)
			{
				StartCoroutine("Part_0_2");
				Part_2.close = true;
			}
			else
			{
				if (Part_2.QuestFireComplite && DuelCamera == null)
				{
					StartCoroutine(Mission(4, true, 0f));
				}
				if (DuelCamera != null && DuelCamera.activeSelf && Part_2.QuestMoveSlug && onMuveSlug)
				{
					Part_2.QuestMoveSlug = false;
					Part_2.QuestFireComplite = false;
					StartCoroutine(HideHand(0f));
					_cutScenePart = CutScenePart.Part_3;
				}
				if (DuelCamera != null && !DuelCamera.activeSelf && Part_2.QuestMoveSlug)
				{
					StartCoroutine(HideHand(0f));
					StartCoroutine(Mission(4, false, 0f));
					SlugButtonDown = false;
					Part_2.QuestFireComplite = false;
					Part_2.QuestMoveSlug = false;
					DuelCamera = null;
				}
				KordWeit();
			}
		}
		if (_cutScenePart == CutScenePart.Part_3)
		{
			if (!Part_3.close)
			{
				Part_3.close = true;
				SlugButtonDown = false;
				StartCoroutine(BoostOn(0f));
				if (!BoostPanel.activeSelf)
				{
					BoostPanel.SetActive(true);
				}
				_SlugButtonOn.SetActive(false);
				_SlugButtonOff.SetActive(true);
			}
			else
			{
				if (!DuelCamera.activeSelf && MainCamera.activeSelf && !Part_3.QuestSlugRestart && !Part_3.QuestFoot)
				{
					Part_3.QuestFoot = true;
					StartCoroutine(DisableInput(0f));
					Part_3.BtnSlugFood.enabled = true;
					StartCoroutine(EliReadyFire(false, 1f));
					StartCoroutine(ShowTextHint("icon_kord", Text[12].GetComponent<TextMesh>().text, 1f));
					StartCoroutine(HideTextHint(2f));
					StartCoroutine(Mission(5, true, 2f));
				}
				if (!kord_stoped && Vector3.Distance(Kord.transform.position, Kord_Move_Target.transform.position) < 0.6f && Part_3.QuestFoot)
				{
					kord_stoped = true;
					StartCoroutine(KordReadyMove(false, 0f));
					Kord.transform.forward = Vector3.Lerp(Kord.transform.forward, -(Kord.transform.position - Eli.transform.position), Time.deltaTime * 80f);
					onlyB.y = Kord.transform.eulerAngles.y;
					Kord.transform.eulerAngles = onlyB;
				}
				KordWeit();
				if (Part_3.QuestSlugRestart && Part_3.UseSlagFoot)
				{
					Part_3.UseSlagFoot = false;
					_SlugButtonOn.SetActive(true);
					_SlugButtonOff.SetActive(false);
					SlugButton.GetComponent<tk2dUIItem>().enabled = false;
					StartCoroutine(Part_0_2_relise_fix());
					if (ReliseTutor)
					{
						_cutScenePart = CutScenePart.Part_2_1;
					}
					else
					{
						_cutScenePart = CutScenePart.Part_4;
					}
				}
			}
		}
		if (_cutScenePart == CutScenePart.Part_2_1)
		{
			if (!Part_2.close_2_1)
			{
				StartCoroutine(Part_0_2_relise());
				Part_2.close_2_1 = true;
			}
			else
			{
				if (!FireCompleted && DuelCamera.activeSelf)
				{
					FireCompleted = true;
					prontoWeit = 0f;
					prontoWeitHate = 0f;
					InfernusFind = false;
					Infernus = GameObject.Find("InfurnusSlug(Clone)");
					if (Infernus != null)
					{
						InfernusFind = true;
					}
					StartCoroutine(SetActiveHelpFalse("useslug", 0f));
					StartCoroutine(SetActiveHelpTrue("reliseslug", null, Vector3.zero, 0.85f));
					StartCoroutine(Part_0_2_relise_TimeScaleZero());
				}
				else if (!FireCompleted)
				{
					KordWeit();
				}
				if (!FireCompleted || !ReliseSlug.activeSelf || !reliseCompleted)
				{
				}
				if (reliseCompleted && Infernus == null)
				{
					BoostPanel.SetActive(false);
					SlugPanel.SetActive(false);
					Help.DisableHelp("reliseslug");
					TimeBrake = true;
					Time.timeScale = 1f;
					prontoWeit = 0f;
					prontoWeitHate = 0f;
					_cutScenePart = CutScenePart.Part_2_2;
				}
				else if (FireCompleted && !DuelCamera.activeSelf)
				{
					if (!reliseCompleted)
					{
						FireCompleted = false;
					}
					prontoWeit = 0f;
					prontoWeitHate = 0f;
					Help.DisableHelp("reliseslug");
					TimeBrake = true;
					Time.timeScale = 1f;
				}
			}
		}
		if (_cutScenePart == CutScenePart.Part_2_2)
		{
			if (!Part_2.close_2_2)
			{
				if (InfernusFind)
				{
					StartCoroutine(Part_0_2_interactive(0.5f));
				}
				else
				{
					StartCoroutine(Part_0_2_interactive(5f));
				}
				Part_2.close_2_2 = true;
			}
			else
			{
				fix += Time.deltaTime;
				if ((!Iobjects[0].IsReady || !Iobjects[1].IsReady || !Iobjects[2].IsReady || !Iobjects[3].IsReady || fix > 15f) && !reliseCompletedFinish)
				{
					reliseCompletedFinish = true;
					StartCoroutine(Part_0_2_reliseFinish());
					ParticleSystem[] componentsInChildren = transform.GetComponentsInChildren<ParticleSystem>();
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						componentsInChildren[i].loop = false;
					}
				}
			}
		}
		if (_cutScenePart == CutScenePart.Part_4)
		{
			if (!Part_4.close)
			{
				if (!death)
				{
					BoostActive = true;
					SlugActive = true;
					StartCoroutine(Part_0_4(0f));
				}
				else
				{
					StartCoroutine(Part_0_4(2f));
					death = false;
				}
				Part_4.close = true;
			}
			else
			{
				if (Part_4.QuestFight)
				{
					if (_eliController.Model.Health < Part_4.MinEliHeath)
					{
						Part_4.QuestFight = false;
						_cutScenePart = CutScenePart.Part_Restart;
						death = true;
						StartCoroutine("Part_RestartFight");
					}
					else if (FightCamera == null && !MainCamera.activeSelf && DuelCamera != null && !DuelCamera.activeSelf && !Part_4.QuestGoBattle)
					{
						Part_4.QuestGoBattle = true;
						StartCoroutine(Find_Camera(0.4f));
					}
					else
					{
						if (FightCamera != null)
						{
							if (!Duelhelp)
							{
								Help.ShowHelp("duel", null, Vector3.zero);
								MonoSingleton<Localization>.Instance.LocalizeTextMesh(help, AdditionalText[12].text, string.Empty);
								Duelhelp = true;
							}
						}
						else if (Duelhelp)
						{
							Help.DisableHelp("duel");
							Duelhelp = false;
						}
						Part_4.QuestGoBattle = false;
					}
				}
				if (EliHealthStart > _eliController.Model.Health && !Part_4.Part_0_4_fix && DuelCamera != null && !DuelCamera.activeSelf)
				{
					if (Part_1.QuestEliDamage && EliHealthStart > _eliController.Model.Health)
					{
						StartCoroutine(DisableInput(0f));
						Part_1.QuestEliDamage = false;
						StartCoroutine(KordReadyFire(false, 0f));
						StartCoroutine(KordReadyMove(false, 0f));
						StartCoroutine(EliReadyFire(false, 0f));
						SlugButton.GetComponent<tk2dUIItem>().sendMessageTarget = null;
						Part_1.EliHealthCurent = _eliController.Model.Health;
						EliHealthStart = Part_1.EliHealthCurent + 10000f;
						StartCoroutine(Part_0_4_fix(3f));
					}
					if (Part_1.QuestAddHealth && Part_1.EliHealthCurent < _eliController.Model.Health)
					{
						Part_4.Part_0_4_fix = true;
						Part_1.QuestAddHealth = false;
						StartCoroutine(EnableInput(0f));
						SlugButton.GetComponent<tk2dUIItem>().sendMessageTarget = ParantSlugButton;
						StartCoroutine(KordReadyMove(true, 0f));
						StartCoroutine(KordReadyFire(true, 0f));
						StartCoroutine(EliReadyFire(true, 0f));
						StartCoroutine(EliReadyFire(true, 0.01f));
					}
					KordWeit();
				}
			}
		}
		if (_kordController.Model.CurrentSpeed > 3f && _cutScenePart != CutScenePart.Part_2_2)
		{
			_kordController.Model.CurrentSpeed = 1.25f;
		}
		if (!PopupConfirmShow)
		{
		}
		if (notReady && Eli_Target.activeSelf)
		{
			Eli_Target.SetActive(false);
		}
		if (!Eli_Move_Target.activeSelf && _cutScenePart != CutScenePart.Part_0 && _cutScenePart != CutScenePart.Part_1 && _cutScenePart != CutScenePart.Part_Restart)
		{
		}
	}

	private IEnumerator Part_0_0()
	{
		yield return new WaitForSeconds(0f);
		StartCoroutine(ShowTextHint("icon_kord", Text[0].GetComponent<TextMesh>().text, 2f));
		StartCoroutine(HideTextHint(3f));
		StartCoroutine(EnableInput(4f));
		StartCoroutine(Mission(0, true, 4f));
	}

	private IEnumerator Part_0_1()
	{
		yield return new WaitForSeconds(0f);
		EliHealthStart = _eliController.Model.Health;
		StartCoroutine(DisableInput(0f));
		StartCoroutine(ShowTextHint("icon_kord", Text[6].GetComponent<TextMesh>().text, 0f));
		StartCoroutine(HideTextHint(1f));
		StartCoroutine(KordReadyFire(true, 1f));
		StartCoroutine(KordReadyMove(true, 1f));
		StartCoroutine(EnableInput(1f));
		Part_1.QuestEliDamage = true;
		StartCoroutine(Find_KordSlug(3.5f));
	}

	private IEnumerator Part_0_1_1(float weit)
	{
		yield return new WaitForSeconds(weit);
		Part_4.Part_0_4_fix = true;
		StartCoroutine(ShowTextHint("icon_kord", Text[7].GetComponent<TextMesh>().text, 1f));
		StartCoroutine(HideTextHint(2f));
		StartCoroutine(SetActiveTrue(BoostPanel, 2.5f));
		StartCoroutine(BoostOn(2.5f));
		StartCoroutine(Mission(2, true, 2.5f));
		Part_1.EliHealthCurent = _eliController.Model.Health;
		StartCoroutine(Part_0_1_1_0(2.6f));
	}

	private IEnumerator Part_0_1_1_0(float weit)
	{
		yield return new WaitForSeconds(weit);
		Part_3.BtnSlugFood.enabled = false;
		Part_3.BtnSlugArmor.enabled = false;
		Part_3.BtnSlugRun.enabled = false;
	}

	private IEnumerator Part_0_1_2(float weit)
	{
		yield return new WaitForSeconds(weit);
		_cutScenePart = CutScenePart.Part_2;
		for (int i = 0; i < warNodeStartPos.Length; i++)
		{
			warNode[i].position = warNodeStartPos[i];
		}
	}

	private IEnumerator Part_0_2_relise_fix()
	{
		SlugButton.GetComponent<tk2dUIItem>().enabled = false;
		yield return new WaitForSeconds(0.1f);
		SlugButton.GetComponent<tk2dUIItem>().enabled = false;
	}

	private IEnumerator Part_0_2_relise()
	{
		yield return new WaitForSeconds(2f);
		reliseAdd.ReleaseBar = ReliseSlug;
		StartCoroutine(ShowTextHint("icon_kord", NewText[0], 0f));
		SlugButton.GetComponent<tk2dUIItem>().enabled = true;
		StartCoroutine(HideTextHint(1f));
		StartCoroutine(EnableInput(1.5f));
		StartCoroutine(KordReadyMove(false, 0f));
		StartCoroutine(KordReadyMove(true, 2.5f));
		StartCoroutine(EliReadyFire(true, 2f));
	}

	private IEnumerator Part_0_2_relise_TimeScaleZero()
	{
		float tempTime = 1f;
		while (tempTime > 0.2f && !TimeBrake)
		{
			yield return new WaitForSeconds(Time.unscaledDeltaTime);
			tempTime -= 0.02f;
			if (tempTime < 0.2f)
			{
				tempTime = 0.2f;
				break;
			}
			Time.timeScale = tempTime;
		}
		yield return new WaitForSeconds(2f);
		while (tempTime < 1f && !TimeBrake)
		{
			yield return new WaitForSeconds(Time.unscaledDeltaTime);
			tempTime += Time.unscaledDeltaTime;
			if (tempTime > 1f)
			{
				tempTime = 1f;
				TimeBrake = true;
				break;
			}
			Time.timeScale = tempTime;
		}
	}

	private IEnumerator Part_0_2_interactive(float t)
	{
		yield return new WaitForSeconds(t);
		BoostActive = false;
		SlugActive = false;
		StartCoroutine(Part_0_2_interactive_On());
		StartCoroutine(ShowTextHint("icon_kord", NewText[1], 0f));
		StartCoroutine(HideTextHint(1f));
		StartCoroutine(EnableInput(1.5f));
		StartCoroutine(KordReadyMove(false, 0f));
		StartCoroutine(KordReadyMove(true, 2.5f));
		StartCoroutine(EliReadyFire(true, 2f));
	}

	private IEnumerator Part_0_2_interactive_On()
	{
		yield return new WaitForSeconds(0.6f);
		for (int i = 0; i < barrels.Length; i++)
		{
			barrels[i].SetActive(true);
			UnityEngine.Object.Instantiate(FxBarrels, barrels[i].transform.position, Quaternion.identity);
		}
		for (int j = 0; j < 4; j++)
		{
			Glow[j].SetActive(true);
		}
		yield return new WaitForSeconds(2f);
		for (int k = 0; k < Touch.Length; k++)
		{
			Touch[k].enabled = true;
		}
		StartCoroutine(SetActiveHelpTrue("useio", null, Vector3.zero, 0.6f));
		HelpLayer2.ShowHelp("useio1", null, Vector3.zero);
		StartCoroutine(SetActiveTrue(Glow[5], 1.2f));
		StartCoroutine(SetActiveTrue(Glow[6], 1.2f));
		StartCoroutine(SetActiveTrue(Glow[7], 1.2f));
		StartCoroutine(SetActiveTrue(Glow[8], 1.2f));
		StartCoroutine(SetActiveTrue(Fx_info, 0.2f));
	}

	private IEnumerator Part_0_2_reliseFinish()
	{
		Help.DisableHelp("useio");
		HelpLayer2.DisableHelp("useio1");
		yield return new WaitForSeconds(2f);
		StartCoroutine(EnableInput(1.5f));
		StartCoroutine(KordReadyMove(false, 0f));
		StartCoroutine(KordReadyMove(true, 6f));
		StartCoroutine(EliReadyFire(true, 6f));
		StartCoroutine(Part_0_2_reliseFinish2());
	}

	private IEnumerator Part_0_2_reliseFinish2()
	{
		yield return new WaitForSeconds(4f);
		_cutScenePart = CutScenePart.Part_4;
		for (int i = 0; i < Touch.Length; i++)
		{
			if ((bool)Touch[i].transform.parent.GetComponent<ArenaInteractiveEnvironmant>())
			{
				if (Touch[i].transform.parent.GetComponent<ArenaInteractiveEnvironmant>().IsReady)
				{
					Touch[i].enabled = true;
				}
			}
			else
			{
				Touch[i].enabled = true;
			}
		}
	}

	private IEnumerator Part_0_2()
	{
		yield return new WaitForSeconds(2.5f);
		StartCoroutine(ShowTextHint("icon_kord", Text[10].GetComponent<TextMesh>().text, 0f));
		StartCoroutine(HideTextHint(1f));
		StartCoroutine(SetActiveTrue(SlugPanel, 1.5f));
		StartCoroutine(SlugOn(1.5f));
		StartCoroutine(EnableInput(1.5f));
		StartCoroutine(KordReadyMove(true, 1.55f));
		StartCoroutine(EliReadyFire(true, 2f));
		StartCoroutine(Mission(3, true, 2f));
		StartCoroutine(KordReadyMove(true, 2f));
	}

	private IEnumerator Part_0_4(float weit)
	{
		yield return new WaitForSeconds(weit);
		StartCoroutine(ShowTextHint("icon_kord", Text[14].GetComponent<TextMesh>().text, 0f));
		StartCoroutine(ShowTextHint("icon_kord", Text[15].GetComponent<TextMesh>().text, 1f));
		StartCoroutine(HideTextHint(2f));
		StartCoroutine(EnableInput(2f));
		StartCoroutine(KordReadyMove(true, 2f));
		StartCoroutine(KordReadyFire(true, 2f));
		StartCoroutine(EliReadyFire(true, 2.5f));
		StartCoroutine(Mission(6, true, 2.6f));
		Part_4.KordHelthStart = _kordController.Model.Health;
		Part_4.EliHealthCurent = _eliController.Model.Health;
		Part_3.BtnSlugArmor.enabled = true;
		Part_3.BtnSlugRun.enabled = true;
		if (Part_4.Part_0_4_fix)
		{
			Part_1.BtnMedicine.enabled = true;
			Part_1.BtnMedicine.enabled = true;
		}
		else
		{
			Part_1.BtnMedicine.enabled = false;
		}
	}

	private IEnumerator Part_0_4_fix(float weit)
	{
		yield return new WaitForSeconds(weit);
		StartCoroutine(ShowTextHint("icon_kord", Text[7].GetComponent<TextMesh>().text, 1f));
		StartCoroutine(HideTextHint(2f));
		StartCoroutine(Mission(2, true, 2f));
		StartCoroutine(Part_0_4_fix_0(2.1f));
	}

	private IEnumerator Part_0_4_fix_0(float weit)
	{
		yield return new WaitForSeconds(weit);
		Part_1.BtnMedicine.enabled = true;
		Part_1.BtnMedicine.enabled = true;
	}

	public void SkipTutorOld()
	{
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		InputEnable = false;
		UICamera.GetComponent<Camera>().enabled = false;
		HUDPanel.SetActive(false);
		StopAllCoroutines();
		gameController.PlayCutScene(true);
		CutSceneUse = true;
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 0f), 1f, 0.5f, 1f);
		StartCoroutine(Skip_Part_0_4_2(0.5f));
	}

	public void SkipTutor()
	{
		if (UICamera.GetComponent<Camera>().enabled)
		{
			Time.timeScale = 0f;
			PopupConfirmShow = true;
			PopupConfirm.SetActive(true);
			btnYes.targetObject = gameObject;
			btnYes.messageName = "Yes";
			btnNo.targetObject = gameObject;
			btnNo.messageName = "No";
			PopupConfirmText.text = MonoSingleton<Localization>.Instance["defeat_skip_desc"];
			_eliController.Animator.updateMode = AnimatorUpdateMode.Normal;
			Help.HideLayer();
			gameController.PlayCutScene(true);
			CutSceneUse = true;
			Time.timeScale = 0f;
		}
	}

	public void Yes()
	{
		PlayerPrefs.SetInt(typeof(ScorchedSparksTutorial_1_1).Name, 1);
		DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
		InputEnable = true;
		gameController.PlayCutScene(false);
		CutSceneUse = false;
		RisePlayCutSceneEvent(false);
		Time.timeScale = 0f;
		gameController.SkipMission();
		DonDestroyMonoSingleton<InputManager>.Instance.BackBtnEvent -= SkipTutor;
	}

	public void No()
	{
		Time.timeScale = 1f;
		PopupConfirmShow = false;
		btnYes.targetObject = PopupConfirm;
		btnYes.messageName = "OnBtnAcceptClick";
		btnNo.targetObject = PopupConfirm;
		btnNo.messageName = "OnBtnDeclineClick";
		PopupConfirm.SetActive(false);
		gameController.PlayCutScene(false);
		CutSceneUse = false;
		StartCoroutine(ShowLayerHelp());
		_eliController.Animator.updateMode = AnimatorUpdateMode.UnscaledTime;
		Time.timeScale = 1f;
	}

	private IEnumerator ShowLayerHelp()
	{
		yield return new WaitForSeconds(0.7f);
		Help.ShowLayer();
	}

	public void Pipe1()
	{
		ScreenHand.SwitchTarget(pipe_1);
	}

	public void Pipe2()
	{
		ScreenHand.SwitchTarget(pipe_2);
	}

	public void Bomb1()
	{
		ScreenHand.SwitchTarget(bomb_1);
	}

	public void Bomb2()
	{
		ScreenHand.SwitchTarget(bomb_2);
	}

	private IEnumerator Skip_Part_0_4_2(float weit)
	{
		yield return new WaitForSeconds(weit);
		DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
		InputEnable = true;
		PlayerPrefs.SetInt(typeof(ScorchedSparksTutorial_1_1).Name, 1);
		gameController.PlayCutScene(false);
		CutSceneUse = false;
		RisePlayCutSceneEvent(false);
		DonDestroyMonoSingleton<InputManager>.Instance.BackBtnEvent -= SkipTutor;
		gameController.SkipMission();
	}

	private IEnumerator Part_RestartFight()
	{
		yield return new WaitForSeconds(0f);
		StartCoroutine(DisableInput(0f));
		StartCoroutine(KordReadyMove(false, 0f));
		StartCoroutine(KordReadyFire(false, 0f));
		StartCoroutine(EliReadyFire(false, 0f));
		StartCoroutine(ShowTextHint("icon_kord", AdditionalText[10].text, 4.5f));
		StartCoroutine(HideTextHintFade(5.5f));
		StartCoroutine(Fade_Maincamera(5.5f));
		StartCoroutine(Part_RestartFight_Scene(7.5f));
	}

	private IEnumerator Part_RestartFight_Scene(float weit)
	{
		yield return new WaitForSeconds(weit);
		_eliController.Model.AddHealth(Part_4.EliHealthCurent);
		_kordController.Model.AddHealth(Part_4.KordHelthStart);
		Eli_models.transform.forward = Vector3.Lerp(Eli_models.transform.forward, -(Eli_models.transform.position - Kord.transform.position), Time.deltaTime * 80f);
		onlyB.y = Eli_models.transform.eulerAngles.y;
		Eli_models.transform.eulerAngles = onlyB;
		Part_4.close = false;
		_cutScenePart = CutScenePart.Part_4;
	}
}

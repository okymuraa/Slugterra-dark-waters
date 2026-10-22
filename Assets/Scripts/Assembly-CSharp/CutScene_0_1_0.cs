using System;
using System.Collections;
using AppsMinistry.Core;
using AppsMinistry.Core.Input;
using AppsMinistry.UI.Camera;
using Slugterra.Domain.Game.Units;
using Slugterra.Game.Input;
using Slugterra.Game.Units;
using Slugterra.Game.Units.Ally;
using Slugterra.Game.Units.Ally.Assistant;
using Slugterra.Game.Units.Enemy;
using Slugterra.Scene;
using Slugterra.Scene.Game;
using Slugterra.UI.Game;
using Slugterra.UI.Game.Camera;
using UnityEngine;

public class CutScene_0_1_0 : SlugterraCutScene
{
	public enum CutScenePart
	{
		Part_0 = 0,
		Part_1 = 1,
		Part_2 = 2,
		Part_3 = 3,
		Part_4 = 4
	}

	[Serializable]
	public class _part_0
	{
		[HideInInspector]
		public bool close;

		public GameObject CutSceneCamera;

		public GameObject House_Round_Door_L;

		public GameObject House_Round_Door_R;

		public Transform EliPoint_0_1;

		public GameObject EliCheckPoint_0_1;

		public GameObject EliCheckPoint_0_2;

		public GameObject EliCheckPoint_0_3;

		public GameObject EliCheckPoint_0_4;

		public Transform ProntoPoint_0_1;

		public Transform ProntoPoint_0_2;

		public Transform ProntoPoint_0_3;

		public Transform ProntoPoint_0_4;

		public Transform ProntoPoint_0_5;

		public GameObject Wall;
	}

	[Serializable]
	public class _part_1
	{
		[HideInInspector]
		public bool close;

		public GameObject CutSceneCamera;

		public GameObject CrateBoxPronto;

		public GameObject CrateBoxEli;

		public GameObject MiniMapCamera;

		[HideInInspector]
		public bool CrateBoxEliComplite;

		[HideInInspector]
		public bool CrateBoxEliComplite_1;

		[HideInInspector]
		public bool CrateBoxEliComplite_2;

		[HideInInspector]
		public bool CrateBoxEliComplite_3;

		[HideInInspector]
		public bool CrateBoxEliComplite_4;

		[HideInInspector]
		public bool CoinsQuestComplite;

		[HideInInspector]
		public bool MiniMapOpen;

		[HideInInspector]
		public bool MiniMapClse;

		[HideInInspector]
		public bool MissionBoxStart;

		[HideInInspector]
		public bool MissionGoProntoToBarrels;

		public GameObject[] CrateBox;

		public GameObject[] TouchColl;

		public Transform ProntoPoint_0_6;

		public Transform ProntoPoint_0_7;

		public Transform ProntoPoint_0_8;

		public Transform EliPoint_CointsQuest;

		public Transform EliPoint_0_2;

		[HideInInspector]
		public bool CheckPointComplite_06;

		[HideInInspector]
		public bool EliFix_06;

		[HideInInspector]
		public bool CheckPointComplite_07;

		[HideInInspector]
		public bool CheckPointComplite_08;

		[HideInInspector]
		public bool ProntoFix_08;
	}

	[Serializable]
	public class _part_2
	{
		[HideInInspector]
		public bool close;

		public GameObject CutSceneCamera;

		public GameObject Arena;

		public GameObject Medical;

		public GameObject Barrel;

		[HideInInspector]
		public bool BarrelEliComplite;

		public Transform ProntoPoint_0_9;

		public Transform ProntoPoint_0_10;

		public Transform ProntoPoint_0_11;

		public Transform ProntoPoint_0_12;

		[HideInInspector]
		public bool CheckPointComplite_10;

		[HideInInspector]
		public bool CheckPointComplite_11;

		[HideInInspector]
		public bool MissionBarrels;

		public Transform EliPoint_0_3;

		public Transform EliPoint_0_4;

		public Transform EliPoint_0_5;

		[HideInInspector]
		public bool CheckPointComplite_05;

		public Transform EnemyPoint_0_1;

		public Transform[] EnemyPatrolPoints;

		[HideInInspector]
		public bool EnemyPatrolLock;

		[HideInInspector]
		public GameObject EnemyTempPoint;

		public Transform ExposionParent;

		[HideInInspector]
		public bool KickParent;

		[HideInInspector]
		public bool battleEnemy;

		[HideInInspector]
		public bool battleEnemyWeit;
	}

	[Serializable]
	public class _part_3
	{
		[HideInInspector]
		public bool close;

		public GameObject CutSceneCamera;

		public CapsuleCollider TouchLayer;

		public GameObject Arena;

		[HideInInspector]
		public bool EnemyKick;

		[HideInInspector]
		public bool TrixieHelp;

		[HideInInspector]
		public bool TargetFalse;

		[HideInInspector]
		public bool EnemyWeit;

		[HideInInspector]
		public bool TrixieStop;

		[HideInInspector]
		public Vector3 EnemyWeitPos;

		public Transform TrixiePoint_0_1;

		public Transform TrixiePoint_0_2;

		public Transform ProntoPoint_0_13;

		[HideInInspector]
		public bool CheckPointComplite_13;

		[HideInInspector]
		public bool FixDtap;

		public Transform EliPoint_0_6;
	}

	[Serializable]
	public class _part_4
	{
		[HideInInspector]
		public bool close;

		public GameObject CutSceneCamera;

		public Transform ProntoPoint_0_14;

		[HideInInspector]
		public bool CheckPointComplite_14;

		public Transform ProntoPoint_0_15;

		[HideInInspector]
		public bool CheckPointComplite_15;

		public Transform ProntoPoint_0_16;

		[HideInInspector]
		public bool CheckPointComplite_16;

		public Transform ProntoPoint_0_17;

		[HideInInspector]
		public bool CheckPointComplite_17;

		public Transform ProntoPoint_0_18;

		[HideInInspector]
		public bool CheckPointComplite_18;

		public Transform TrixiePoint_0_3;

		public Transform TrixiePoint_0_4;

		public Transform EliPoint_0_7;

		public Transform KordPoint_0_1;
	}

	[SerializeField]
	private GameController gameController;

	public bool CutSceneUse;

	public GameObject HintHand;

	public GameObject HintHandDT;

	public GameObject Hand2D;

	public GameObject Hand2D2;

	public GameObject Hand2D2_Skip;

	public GameObject Arrow;

	public GameObject FadeStart;

	public GameObject ButomNext;

	public GameObject PopupConfirm;

	public TextMesh PopupConfirmText;

	public bool PopupConfirmShow;

	public GameObject PopupAppStore;

	public tk2dButton btnYes;

	public tk2dButton btnNo;

	public GameObject DialogueWindowNew;

	public GameObject DialogueWindowOld;

	public Transform point_new;

	public Transform point_old;

	public Transform Dialoguetext;

	public GameObject[] Text;

	public TextMesh[] AdditionalText;

	public GameObject SlugPanel;

	public GameObject MiniMapPanel;

	public GameObject HUDPanel;

	public bool CoinsQuest;

	public CutScenePart _cutScenePart;

	public GameObject Eli;

	public GameObject Eli_Target;

	public GameObject Eli_Move_Target;

	public BoxCollider brige;

	public GameObject[] TerrainElements;

	private int i;

	private int f;

	public GameObject Pronto;

	public GameObject Pronto_Move_Target;

	public GameObject Enemy_1;

	public SphereCollider Enemy_1_Area;

	public GameObject Trixie;

	public GameObject Kord;

	private GameObject MainCamera;

	private GameObject UICamera;

	private CameraFade _cameraFade;

	private CameraFade _cameraFade_cut;

	[SerializeField]
	private CameraSmoothFollow _cameraSmoothFollow;

	private LookSlerpObj[] _lookSlerpObj;

	public _part_0 Part_0;

	public _part_1 Part_1;

	public _part_2 Part_2;

	public _part_3 Part_3;

	public _part_4 Part_4;

	private EliController _eliController;

	private ProntoController _prontoController;

	private KordController _kordController;

	private TrixieController _trixieController;

	public Transform TrixieAnimations;

	private SentinelController _enemy_1_Controller;

	private float prontoWeit;

	private float prontoWeitHate;

	private int CountProntotext;

	private bool ActiveInput;

	private bool ShowText;

	private Color black = Color.black;

	public Renderer shadow;

	public bool showShadowPlane;

	public GameObject arrowScreen;

	private void Start()
	{
		MainCamera = GameObject.Find("MainCamera");
		UICamera = GameObject.Find("UICamera");
		black = Color.black;
		black.a = 0f;
		shadow.material.color = black;
		shadow.gameObject.GetComponent<BoxCollider>().enabled = false;
		gameController = UnityEngine.Object.FindObjectOfType<GameController>();
		_cameraFade = MainCamera.GetComponent<CameraFade>();
		_cameraFade_cut = Part_0.CutSceneCamera.GetComponent<CameraFade>();
		_cameraSmoothFollow = MainCamera.GetComponent<CameraSmoothFollow>();
		_lookSlerpObj = UnityEngine.Object.FindObjectsOfType<LookSlerpObj>();
		_eliController = UnityEngine.Object.FindObjectOfType<EliController>();
		_prontoController = UnityEngine.Object.FindObjectOfType<ProntoController>();
		_trixieController = UnityEngine.Object.FindObjectOfType<TrixieController>();
		_kordController = UnityEngine.Object.FindObjectOfType<KordController>();
		_enemy_1_Controller = UnityEngine.Object.FindObjectOfType<SentinelController>();
		gameController.Model.Assistant = _prontoController.Model as Humanoid;
		_cameraFade_cut.StartFade(new Color(0f, 0f, 0f, 1f), 1f, 0.1f, 2f);
		MonoSingleton<GameCollisionMap>.Instance.isTutorial = true;
		gameController.PlayCutScene(true);
		CutSceneUse = true;
		DisableMove();
		brige.enabled = false;
		ActiveInput = true;
		ShowText = false;
		Eli_Target.SetActive(false);
		Enemy_1_Area.radius = 0.25f;
		Kord.SetActive(false);
		_trixieController.gameObject.SetActive(false);
		UICamera.GetComponent<Camera>().enabled = false;
		HUDPanel.SetActive(false);
		MainCamera.GetComponent<Camera>().enabled = false;
		_cameraSmoothFollow.enabled = false;
		HintHand.SetActive(false);
		HintHandDT.SetActive(false);
		Arrow.SetActive(false);
		MiniMapPanel.SetActive(false);
		Hand2D.SetActive(false);
		DialogueWindowNew.SetActive(false);
		DialogueWindowOld.SetActive(false);
		Part_0.EliCheckPoint_0_1.SetActive(false);
		Part_0.EliCheckPoint_0_2.SetActive(false);
		Part_0.EliCheckPoint_0_3.SetActive(false);
		Part_0.EliCheckPoint_0_4.SetActive(false);
		Part_0.Wall.SetActive(false);
		SlugPanel.SetActive(false);
		StartCoroutine(SetActiveFalse(FadeStart, 0.2f));
		Part_1.CrateBoxEliComplite = false;
		for (f = 0; f < Part_1.TouchColl.Length; f++)
		{
			Part_1.TouchColl[f].GetComponent<Collider>().enabled = false;
		}
		Part_2.ExposionParent.GetComponent<Animator>().enabled = false;
		Part_3.TouchLayer.enabled = false;
		if (_cutScenePart == CutScenePart.Part_1 && !Part_0.close)
		{
			Part_0.close = true;
			Eli.transform.position = Part_0.EliCheckPoint_0_4.transform.position;
			Pronto.transform.position = Part_0.ProntoPoint_0_5.transform.position;
			Part_0.CutSceneCamera.SetActive(false);
			UICamera.GetComponent<Camera>().enabled = true;
			MainCamera.GetComponent<Camera>().enabled = true;
			_cameraSmoothFollow.enabled = true;
		}
		if (_cutScenePart == CutScenePart.Part_2 && !Part_1.close)
		{
			Part_0.close = true;
			Part_1.close = true;
			Eli.transform.position = Part_1.EliPoint_0_2.position;
			Eli.transform.localEulerAngles = Vector3.up * 186f;
			Pronto.transform.position = Part_1.ProntoPoint_0_8.position;
			Pronto.transform.localEulerAngles = Vector3.up * 186f;
			Part_1.CutSceneCamera.SetActive(false);
			UICamera.GetComponent<Camera>().enabled = true;
			MainCamera.GetComponent<Camera>().enabled = true;
			_cameraSmoothFollow.enabled = true;
		}
		if (_cutScenePart == CutScenePart.Part_3 && !Part_2.close)
		{
			Part_0.close = true;
			Part_1.close = true;
			Part_2.close = true;
			Eli.transform.position = Part_2.EliPoint_0_5.position;
			Eli.transform.localEulerAngles = Vector3.up * 127f;
			Pronto.transform.position = Part_2.ProntoPoint_0_12.position;
			Pronto.transform.localEulerAngles = Vector3.up * 129f;
			StartCoroutine(EnableMoveIndex(2, 0f));
			Part_3.TouchLayer.enabled = true;
			Part_2.CutSceneCamera.SetActive(false);
			UICamera.GetComponent<Camera>().enabled = true;
			MainCamera.GetComponent<Camera>().enabled = true;
			_cameraSmoothFollow.enabled = true;
			StartCoroutine(SetActiveTrue(Part_3.Arena, 4f));
		}
		if (_cutScenePart == CutScenePart.Part_4 && !Part_3.close)
		{
			Part_0.close = true;
			Part_1.close = true;
			Part_2.close = true;
			Part_3.close = true;
			Eli.transform.position = Part_3.EliPoint_0_6.position;
			Pronto.transform.position = Part_3.ProntoPoint_0_13.position;
			Part_3.CutSceneCamera.SetActive(false);
			UICamera.GetComponent<Camera>().enabled = true;
			MainCamera.GetComponent<Camera>().enabled = true;
			_cameraSmoothFollow.enabled = true;
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
			_prontoController.MoveTarget.position = target;
			_prontoController.OnMove(speed);
		}
		if (index == 2)
		{
			_enemy_1_Controller.MoveTarget.position = target;
			_enemy_1_Controller.OnMove(speed);
		}
		if (index == 3)
		{
			_trixieController.MoveTarget.position = target;
			_trixieController.OnMove(speed);
		}
		if (index == 4)
		{
			_kordController.MoveTarget.position = target;
			_kordController.OnMove(speed);
		}
	}

	private IEnumerator LookAtTarget(GameObject obj, int Angle, float weit = 0f)
	{
		yield return new WaitForSeconds(weit);
		_trixieController.gameObject.GetComponent<UnitPathFinder>().enabled = true;
		TrixieAnimations.localEulerAngles = Vector3.zero;
		obj.transform.localEulerAngles = Vector3.up * Angle;
	}

	private IEnumerator ShowTextHint(string ico, string text, float weit)
	{
		yield return new WaitForSeconds(weit);
		gameController.PlayCutScene(true);
		CutSceneUse = true;
		DialogueWindowOld.SetActive(false);
		DialogueWindowNew.SetActive(true);
		Dialoguetext.position = point_new.position;
		if (!ShowText)
		{
			UICamera.GetComponent<Camera>().enabled = false;
			StartCoroutine(SetActiveFalse(HUDPanel, 0f));
			_cutSceneView.ShowScreens(true);
		}
		if (HintHand.activeSelf)
		{
			HintHand.SetActive(false);
			HintHandDT.SetActive(false);
		}
		if (!Hand2D2_Skip.activeSelf)
		{
			StartCoroutine(SetActiveTrue(Hand2D2_Skip, 0.49f));
		}
		_cutSceneView.SpeakerIcon = ico;
		_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance[text];
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		ShowText = true;
		StartCoroutine(TimeZero(0.5f));
	}

	private IEnumerator HideTextHint(float weit)
	{
		yield return new WaitForSeconds(weit);
		_cutSceneView.ShowScreens(false);
		if (ActiveInput)
		{
			DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
		}
		else
		{
			DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		}
		if (!Part_1.CutSceneCamera.activeSelf)
		{
			StartCoroutine(ShowUIAuto(0.5f));
		}
		ShowText = false;
	}

	private IEnumerator ShowUIAuto(float weit)
	{
		yield return new WaitForSeconds(weit);
		gameController.PlayCutScene(false);
		CutSceneUse = false;
		HUDPanel.SetActive(true);
		UICamera.GetComponent<Camera>().enabled = true;
	}

	private IEnumerator TimeZero(float weit)
	{
		yield return new WaitForSeconds(weit);
		ButomNext.SetActive(true);
		ButomNext.GetComponent<BoxCollider>().enabled = true;
		Time.timeScale = 0f;
	}

	private IEnumerator ShowTextHintAdd(string ico, string text, float weit)
	{
		yield return new WaitForSeconds(weit);
		gameController.PlayCutScene(true);
		CutSceneUse = true;
		DialogueWindowOld.SetActive(true);
		DialogueWindowNew.SetActive(false);
		Dialoguetext.position = point_old.position;
		if (!ShowText)
		{
			UICamera.GetComponent<Camera>().enabled = false;
			StartCoroutine(SetActiveFalse(HUDPanel, 0f));
			_cutSceneView.ShowScreens(true);
		}
		if (HintHand.activeSelf)
		{
			HintHand.SetActive(false);
		}
		if (HintHandDT.activeSelf)
		{
			HintHandDT.SetActive(false);
		}
		SlugPanel.SetActive(false);
		_cutSceneView.SpeakerIcon = ico;
		_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance[text];
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		ShowText = true;
	}

	private IEnumerator HideTextHintAdd(float weit)
	{
		yield return new WaitForSeconds(weit);
		_cutSceneView.ShowScreens(false);
		if (HintHandDT.activeSelf)
		{
			HintHandDT.SetActive(false);
		}
		if (ActiveInput)
		{
			DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
		}
		else
		{
			DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		}
		StartCoroutine(ShowUIAuto(0.5f));
		ShowText = false;
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

	private IEnumerator DestroyObj(GameObject obj, float weit)
	{
		yield return new WaitForSeconds(weit);
		UnityEngine.Object.Destroy(obj);
	}

	private IEnumerator HideMeinCamera(float weit)
	{
		yield return new WaitForSeconds(weit);
		MainCamera.GetComponent<Camera>().cullingMask = 0;
		_cameraFade.enabled = false;
	}

	private IEnumerator ShowArrow(Vector3 pos, float weit)
	{
		yield return new WaitForSeconds(weit);
		Arrow.transform.position = pos;
		Arrow.SetActive(true);
		HintHand.transform.position = pos;
	}

	private IEnumerator HideArrow(float weit)
	{
		yield return new WaitForSeconds(weit);
		Arrow.SetActive(false);
	}

	private IEnumerator Fade_Maincamera(float weit)
	{
		yield return new WaitForSeconds(weit);
		gameController.PlayCutScene(true);
		CutSceneUse = true;
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 0f), 1f, 1f, 2f);
	}

	private IEnumerator ReplaceCamers(float weit)
	{
		yield return new WaitForSeconds(weit);
		if (Part_0.CutSceneCamera.activeSelf)
		{
			Part_0.CutSceneCamera.SetActive(false);
			MainCamera.GetComponent<Camera>().enabled = true;
			_cameraSmoothFollow.enabled = true;
			StartCoroutine(ShowUIAuto(2f));
		}
		else
		{
			gameController.PlayCutScene(true);
			CutSceneUse = true;
			Part_0.CutSceneCamera.SetActive(true);
			UICamera.GetComponent<Camera>().enabled = false;
			MainCamera.GetComponent<Camera>().enabled = false;
			_cameraSmoothFollow.enabled = false;
			StartCoroutine(SetActiveFalse(HUDPanel, 0f));
		}
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
		Part_2.KickParent = true;
		_prontoController.OnFallDown();
	}

	private IEnumerator ProntoUp(float weit)
	{
		yield return new WaitForSeconds(weit);
		_prontoController.OnStandUp();
		Part_2.KickParent = false;
	}

	private IEnumerator DisableInput(float weit)
	{
		yield return new WaitForSeconds(weit);
		ActiveInput = false;
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
	}

	private IEnumerator EnableInput(float weit)
	{
		yield return new WaitForSeconds(weit);
		ActiveInput = true;
		DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
	}

	private void DisableMove()
	{
		for (i = 0; i < TerrainElements.Length; i++)
		{
			TerrainElements[i].layer = LayerMask.NameToLayer("Default");
		}
	}

	private void EnableMove()
	{
		for (i = 0; i < TerrainElements.Length; i++)
		{
			TerrainElements[i].layer = LayerMask.NameToLayer("Terrain");
		}
	}

	private void Next()
	{
		Hand2D2_Skip.SetActive(false);
		ButomNext.GetComponent<BoxCollider>().enabled = false;
		Time.timeScale = 1f;
	}

	private IEnumerator EnableMoveIndex(int index, float weit)
	{
		yield return new WaitForSeconds(weit);
		TerrainElements[index].layer = LayerMask.NameToLayer("Terrain");
	}

	private IEnumerator DisableMoveIndex(int index, float weit)
	{
		yield return new WaitForSeconds(weit);
		TerrainElements[index].layer = LayerMask.NameToLayer("Default");
	}

	private IEnumerator EliReadyFire(float weit)
	{
		yield return new WaitForSeconds(weit);
		_eliController.SetAttackTarget(null);
	}

	private IEnumerator Mission(int index, bool state, float weit)
	{
		yield return new WaitForSeconds(weit);
		if (index == 1)
		{
			Part_1.MissionBoxStart = state;
		}
		if (index == 2)
		{
			Part_1.MissionGoProntoToBarrels = state;
		}
		if (index == 3)
		{
			Part_2.MissionBarrels = state;
		}
		prontoWeitHate = 0f;
	}

	private void WeitPronto()
	{
		if (!Part_0.CutSceneCamera.activeSelf)
		{
			if (ShowText)
			{
				return;
			}
			if (_cutScenePart == CutScenePart.Part_0)
			{
				if (Vector3.Distance(Eli.transform.position, Eli_Move_Target.transform.position) < 1f)
				{
					prontoWeit += Time.deltaTime;
					prontoWeitHate += Time.deltaTime;
					if (prontoWeit > 10f)
					{
						if (CountProntotext < 5)
						{
							StartCoroutine(ShowTextHintAdd("icon_pronto", Text[0].GetComponent<TextMesh>().text, 0f));
						}
						else
						{
							StartCoroutine(ShowTextHintAdd("icon_pronto", AdditionalText[4].text, 0f));
							CountProntotext = UnityEngine.Random.Range(0, 3);
						}
						StartCoroutine(HideTextHintAdd(3f));
						prontoWeit = 0f;
						CountProntotext++;
					}
				}
				else if (prontoWeit != 0f)
				{
					prontoWeit = 0f;
					prontoWeitHate = 0f;
					CountProntotext = UnityEngine.Random.Range(0, 3);
				}
				if (prontoWeitHate > 5f && !ShowText)
				{
					if (Part_0.EliCheckPoint_0_1.activeSelf)
					{
						HintHand.transform.position = Part_0.EliCheckPoint_0_1.transform.position;
						for (int i = 0; i < _lookSlerpObj.Length; i++)
						{
							_lookSlerpObj[i].NewText(MonoSingleton<Localization>.Instance[AdditionalText[0].text]);
						}
						HintHand.SetActive(true);
					}
					if (Part_0.EliCheckPoint_0_2.activeSelf)
					{
						HintHand.transform.position = Part_0.EliCheckPoint_0_2.transform.position;
						for (int j = 0; j < _lookSlerpObj.Length; j++)
						{
							_lookSlerpObj[j].NewText(MonoSingleton<Localization>.Instance[AdditionalText[0].text]);
						}
						HintHand.SetActive(true);
					}
					if (Part_0.EliCheckPoint_0_3.activeSelf)
					{
						HintHand.transform.position = Part_0.EliCheckPoint_0_3.transform.position;
						for (int k = 0; k < _lookSlerpObj.Length; k++)
						{
							_lookSlerpObj[k].NewText(MonoSingleton<Localization>.Instance[AdditionalText[0].text]);
						}
						HintHand.SetActive(true);
					}
					if (Part_0.EliCheckPoint_0_4.activeSelf)
					{
						HintHand.transform.position = Part_0.EliCheckPoint_0_4.transform.position;
						for (int l = 0; l < _lookSlerpObj.Length; l++)
						{
							_lookSlerpObj[l].NewText(MonoSingleton<Localization>.Instance[AdditionalText[0].text]);
						}
						HintHand.SetActive(true);
					}
				}
				else if (HintHand.activeSelf)
				{
					HintHand.SetActive(false);
				}
			}
			if (_cutScenePart == CutScenePart.Part_1 && Part_1.MissionBoxStart)
			{
				if (!Eli_Target.activeSelf)
				{
					if (!Part_1.CrateBoxEliComplite || !Part_1.CrateBoxEliComplite_1 || !Part_1.CrateBoxEliComplite_2 || !Part_1.CrateBoxEliComplite_3 || !Part_1.CrateBoxEliComplite_4)
					{
						prontoWeit += Time.deltaTime;
						prontoWeitHate += Time.deltaTime;
						if (prontoWeit > 10f)
						{
							if (CountProntotext < 5)
							{
								StartCoroutine(ShowTextHintAdd("icon_pronto", Text[15].GetComponent<TextMesh>().text, 0f));
							}
							else
							{
								StartCoroutine(ShowTextHintAdd("icon_pronto", AdditionalText[5].text, 0f));
								CountProntotext = UnityEngine.Random.Range(0, 3);
							}
							StartCoroutine(HideTextHintAdd(3f));
							prontoWeit = 0f;
							CountProntotext++;
						}
					}
					if (prontoWeitHate > 5f && !ShowText)
					{
						for (int m = 0; m < _lookSlerpObj.Length; m++)
						{
							_lookSlerpObj[m].NewText(MonoSingleton<Localization>.Instance[AdditionalText[1].text]);
						}
						HintHand.SetActive(true);
					}
					else if (HintHand.activeSelf)
					{
						HintHand.SetActive(false);
					}
				}
				else if (prontoWeit != 0f)
				{
					prontoWeit = 0f;
					prontoWeitHate = 0f;
					CountProntotext = UnityEngine.Random.Range(0, 3);
				}
			}
			if (_cutScenePart == CutScenePart.Part_1 && Part_1.MissionGoProntoToBarrels && (!Part_1.CheckPointComplite_06 || !Part_1.CheckPointComplite_07 || !Part_1.CheckPointComplite_08))
			{
				prontoWeit += Time.deltaTime;
				prontoWeitHate += Time.deltaTime;
				if (prontoWeitHate < 55f)
				{
					if (prontoWeit > 10f)
					{
						StartCoroutine(ShowTextHintAdd("icon_pronto", Text[4].GetComponent<TextMesh>().text, 0f));
						StartCoroutine(HideTextHintAdd(2f));
						prontoWeit = 0f;
					}
				}
				else if (prontoWeit > 10f)
				{
					StartCoroutine(ShowTextHintAdd("icon_pronto", AdditionalText[4].text, 0f));
					StartCoroutine(HideTextHintAdd(2f));
					prontoWeit = 0f;
					prontoWeitHate = 0f;
				}
			}
			if (_cutScenePart == CutScenePart.Part_2 && Part_2.Barrel != null && Part_2.MissionBarrels && !Eli_Target.activeSelf)
			{
				prontoWeit += Time.deltaTime;
				prontoWeitHate += Time.deltaTime;
				if (prontoWeit > 10f)
				{
					if (CountProntotext < 5)
					{
						StartCoroutine(ShowTextHintAdd("icon_pronto", Text[58].GetComponent<TextMesh>().text, 0f));
					}
					else
					{
						StartCoroutine(ShowTextHintAdd("icon_pronto", AdditionalText[5].text, 0f));
						CountProntotext = UnityEngine.Random.Range(0, 3);
					}
					StartCoroutine(HideTextHintAdd(3f));
					prontoWeit = 0f;
					CountProntotext++;
				}
				if (prontoWeitHate > 5f && !ShowText)
				{
					for (int n = 0; n < _lookSlerpObj.Length; n++)
					{
						_lookSlerpObj[n].NewText(MonoSingleton<Localization>.Instance[AdditionalText[2].text]);
					}
					HintHand.SetActive(true);
				}
				else if (HintHand.activeSelf)
				{
					HintHand.SetActive(false);
				}
			}
			if (_cutScenePart == CutScenePart.Part_3)
			{
				if (Part_3.TargetFalse && Eli_Target.activeSelf)
				{
					prontoWeit += Time.deltaTime;
					prontoWeitHate += Time.deltaTime;
					if (prontoWeit > 10f)
					{
						if (CountProntotext < 2)
						{
							StartCoroutine(ShowTextHintAdd("icon_pronto", Text[61].GetComponent<TextMesh>().text, 0f));
							StartCoroutine(ShowTextHintAdd("icon_pronto", Text[6].GetComponent<TextMesh>().text, 4f));
							StartCoroutine(HideTextHintAdd(8f));
						}
						else
						{
							StartCoroutine(ShowTextHintAdd("icon_pronto", Text[61].GetComponent<TextMesh>().text, 0f));
							StartCoroutine(HideTextHintAdd(4f));
							CountProntotext = 0;
						}
						CountProntotext++;
						prontoWeit = 0f;
					}
					if (prontoWeitHate > 5f)
					{
						HintHandDT.transform.position = Enemy_1.transform.position + Vector3.up * 0.5f;
						for (int num = 0; num < _lookSlerpObj.Length; num++)
						{
							_lookSlerpObj[num].NewText(MonoSingleton<Localization>.Instance[AdditionalText[3].text]);
						}
						HintHandDT.SetActive(true);
					}
					else if (HintHandDT.activeSelf)
					{
						HintHandDT.SetActive(false);
					}
				}
				if (!Eli_Target.activeSelf && !Part_3.EnemyKick)
				{
					prontoWeit += Time.deltaTime;
					prontoWeitHate += Time.deltaTime;
					if (prontoWeit > 10f)
					{
						if (CountProntotext < 5)
						{
							StartCoroutine(ShowTextHintAdd("icon_pronto", Text[2].GetComponent<TextMesh>().text, 0f));
							StartCoroutine(HideTextHintAdd(4f));
						}
						else
						{
							StartCoroutine(ShowTextHintAdd("icon_pronto", AdditionalText[5].text, 0f));
							StartCoroutine(HideTextHintAdd(4f));
							CountProntotext = UnityEngine.Random.Range(0, 3);
						}
						CountProntotext++;
						prontoWeit = 0f;
					}
					if (prontoWeitHate > 5f)
					{
						HintHand.transform.position = Enemy_1.transform.position + Vector3.up * 0.5f;
						for (int num2 = 0; num2 < _lookSlerpObj.Length; num2++)
						{
							_lookSlerpObj[num2].NewText(MonoSingleton<Localization>.Instance[AdditionalText[2].text]);
						}
						HintHand.SetActive(true);
					}
					else if (HintHand.activeSelf)
					{
						HintHand.SetActive(false);
					}
				}
			}
			if (_cutScenePart == CutScenePart.Part_2 && !Part_2.CheckPointComplite_11 && Vector3.Distance(Pronto.transform.position, Part_2.ProntoPoint_0_11.position) < 1f)
			{
				prontoWeit += Time.deltaTime;
				prontoWeitHate += Time.deltaTime;
				if (prontoWeitHate < 55f)
				{
					if (prontoWeit > 10f)
					{
						StartCoroutine(ShowTextHintAdd("icon_pronto", Text[4].GetComponent<TextMesh>().text, 0f));
						StartCoroutine(HideTextHintAdd(2f));
						prontoWeit = 0f;
					}
				}
				else if (prontoWeit > 10f)
				{
					StartCoroutine(ShowTextHintAdd("icon_pronto", AdditionalText[4].text, 0f));
					StartCoroutine(HideTextHintAdd(2f));
					prontoWeit = 0f;
					prontoWeitHate = 0f;
				}
			}
			if (_cutScenePart == CutScenePart.Part_4 && !Part_4.CheckPointComplite_14 && Vector3.Distance(Pronto.transform.position, Part_4.ProntoPoint_0_14.position) < 1f)
			{
				prontoWeit += Time.deltaTime;
				prontoWeitHate += Time.deltaTime;
				if (prontoWeitHate < 55f)
				{
					if (prontoWeit > 10f)
					{
						StartCoroutine(ShowTextHintAdd("icon_pronto", Text[4].GetComponent<TextMesh>().text, 0f));
						StartCoroutine(HideTextHintAdd(2f));
						prontoWeit = 0f;
					}
				}
				else if (prontoWeit > 10f)
				{
					StartCoroutine(ShowTextHintAdd("icon_pronto", AdditionalText[4].text, 0f));
					StartCoroutine(HideTextHintAdd(2f));
					prontoWeit = 0f;
					prontoWeitHate = 0f;
				}
			}
			if (_cutScenePart == CutScenePart.Part_4 && !Part_4.CheckPointComplite_15 && Vector3.Distance(Pronto.transform.position, Part_4.ProntoPoint_0_15.position) < 1f)
			{
				prontoWeit += Time.deltaTime;
				prontoWeitHate += Time.deltaTime;
				if (prontoWeitHate < 55f)
				{
					if (prontoWeit > 10f)
					{
						StartCoroutine(ShowTextHintAdd("icon_pronto", Text[4].GetComponent<TextMesh>().text, 0f));
						StartCoroutine(HideTextHintAdd(2f));
						prontoWeit = 0f;
					}
				}
				else if (prontoWeit > 10f)
				{
					StartCoroutine(ShowTextHintAdd("icon_pronto", AdditionalText[4].text, 0f));
					StartCoroutine(HideTextHintAdd(2f));
					prontoWeit = 0f;
					prontoWeitHate = 0f;
				}
			}
			if (_cutScenePart == CutScenePart.Part_4 && !Part_4.CheckPointComplite_16 && Vector3.Distance(Pronto.transform.position, Part_4.ProntoPoint_0_16.position) < 1f)
			{
				prontoWeit += Time.deltaTime;
				prontoWeitHate += Time.deltaTime;
				if (prontoWeitHate < 55f)
				{
					if (prontoWeit > 10f)
					{
						StartCoroutine(ShowTextHintAdd("icon_pronto", Text[4].GetComponent<TextMesh>().text, 0f));
						StartCoroutine(HideTextHintAdd(2f));
						prontoWeit = 0f;
					}
				}
				else if (prontoWeit > 10f)
				{
					StartCoroutine(ShowTextHintAdd("icon_pronto", AdditionalText[4].text, 0f));
					StartCoroutine(HideTextHintAdd(2f));
					prontoWeit = 0f;
					prontoWeitHate = 0f;
				}
			}
			if (_cutScenePart != CutScenePart.Part_4 || Part_4.CheckPointComplite_17 || !(Vector3.Distance(Pronto.transform.position, Part_4.ProntoPoint_0_17.position) < 1f))
			{
				return;
			}
			prontoWeit += Time.deltaTime;
			prontoWeitHate += Time.deltaTime;
			if (prontoWeitHate < 55f)
			{
				if (prontoWeit > 10f)
				{
					StartCoroutine(ShowTextHintAdd("icon_pronto", Text[4].GetComponent<TextMesh>().text, 0f));
					StartCoroutine(HideTextHintAdd(2f));
					prontoWeit = 0f;
				}
			}
			else if (prontoWeit > 10f)
			{
				StartCoroutine(ShowTextHintAdd("icon_pronto", AdditionalText[4].text, 0f));
				StartCoroutine(HideTextHintAdd(2f));
				prontoWeit = 0f;
				prontoWeitHate = 0f;
			}
		}
		else if (prontoWeit != 0f)
		{
			prontoWeit = 0f;
			prontoWeitHate = 0f;
		}
	}

	public void battleEnemy()
	{
		Part_2.battleEnemy = true;
		prontoWeit = 0f;
		prontoWeitHate = 0f;
		_prontoController.Model.Invulnerability = true;
		StartCoroutine(DisableMoveIndex(2, 0f));
		brige.enabled = false;
	}

	public void CheckPoint_14()
	{
		StartCoroutine(MoveTarget(1, 8f, Part_4.ProntoPoint_0_15.position));
		StartCoroutine(DisableMoveIndex(2, 0f));
		Part_4.CheckPointComplite_14 = true;
		prontoWeit = 0f;
		prontoWeitHate = 0f;
	}

	public void CheckPoint_15()
	{
		StartCoroutine(MoveTarget(1, 9f, Part_4.ProntoPoint_0_16.position));
		Part_4.CheckPointComplite_15 = true;
		prontoWeit = 0f;
		prontoWeitHate = 0f;
	}

	public void CheckPoint_16()
	{
		StartCoroutine(MoveTarget(1, 10f, Part_4.ProntoPoint_0_17.position));
		Part_4.CheckPointComplite_16 = true;
		prontoWeit = 0f;
		prontoWeitHate = 0f;
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
		if (_cutScenePart == CutScenePart.Part_0)
		{
			if (!Part_0.close)
			{
				StartCoroutine("Part_0_0");
				Part_0.close = true;
			}
			else
			{
				if (Part_0.EliCheckPoint_0_1.activeSelf)
				{
					if (Vector3.Distance(Part_0.EliCheckPoint_0_1.transform.position, Eli.transform.position) < 2f)
					{
						Part_0.EliCheckPoint_0_1.SetActive(false);
						StartCoroutine(Fade_Maincamera(0f));
						StartCoroutine(ReplaceCamers(1f));
						StartCoroutine(AnimationPlay(Part_0.CutSceneCamera, 1f, "LVL_Tutorial_Follow_Pronto_Run", 1f));
						StartCoroutine(Fade_Maincamera(7f));
						StartCoroutine(ReplaceCamers(9f));
						StartCoroutine(MoveTarget(1, 6f, Part_0.ProntoPoint_0_3.position, 8.5f));
						StartCoroutine(SetActiveTrue(Part_0.EliCheckPoint_0_2, 8f));
						prontoWeit = 0f;
						prontoWeitHate = 0f;
					}
					else
					{
						WeitPronto();
					}
				}
				if (Part_0.EliCheckPoint_0_2.activeSelf)
				{
					if (Vector3.Distance(Part_0.EliCheckPoint_0_2.transform.position, Eli.transform.position) < 2f)
					{
						Part_0.EliCheckPoint_0_2.SetActive(false);
						StartCoroutine(SetActiveTrue(Part_0.EliCheckPoint_0_3, 0.4f));
						StartCoroutine(MoveTarget(1, 5f, Part_0.ProntoPoint_0_4.position));
						prontoWeit = 0f;
						prontoWeitHate = 0f;
					}
					else
					{
						WeitPronto();
					}
				}
				if (Part_0.EliCheckPoint_0_3.activeSelf)
				{
					if (Vector3.Distance(Part_0.EliCheckPoint_0_3.transform.position, Eli.transform.position) < 2f)
					{
						Part_0.EliCheckPoint_0_3.SetActive(false);
						StartCoroutine(SetActiveTrue(Part_0.EliCheckPoint_0_4, 0.4f));
						StartCoroutine(MoveTarget(1, 5f, Part_0.ProntoPoint_0_5.position));
						prontoWeit = 0f;
						prontoWeitHate = 0f;
					}
					else
					{
						WeitPronto();
					}
				}
				if (Part_0.EliCheckPoint_0_4.activeSelf)
				{
					if (Vector3.Distance(Part_0.EliCheckPoint_0_4.transform.position, Eli.transform.position) < 2f)
					{
						Part_0.EliCheckPoint_0_4.SetActive(false);
						StartCoroutine(DisableInput(0f));
						_cutScenePart = CutScenePart.Part_1;
						prontoWeit = 0f;
						prontoWeitHate = 0f;
					}
					else
					{
						WeitPronto();
					}
				}
			}
		}
		if (_cutScenePart == CutScenePart.Part_1)
		{
			if (!Part_1.close)
			{
				StartCoroutine(Part_0_1());
				Part_1.close = true;
			}
			else
			{
				if (Part_1.TouchColl.Length > 0)
				{
					if ((Part_1.CrateBoxEli == null && !Part_1.CrateBoxEliComplite) || (Part_1.CrateBoxEli != null && !Part_1.CrateBoxEliComplite && !Part_1.CrateBoxEli.activeSelf))
					{
						Part_1.CrateBoxEliComplite = true;
						StartCoroutine(HideArrow(0f));
						StartCoroutine(Mission(1, false, 0f));
					}
					else
					{
						WeitPronto();
					}
					if ((Part_1.CrateBox[0] == null && !Part_1.CrateBoxEliComplite_1) || (Part_1.CrateBox[0] != null && !Part_1.CrateBoxEliComplite_1 && !Part_1.CrateBox[0].activeSelf))
					{
						Part_1.CrateBoxEliComplite_1 = true;
						Part_1.TouchColl[2].GetComponent<Collider>().enabled = true;
					}
					if ((Part_1.CrateBox[1] == null && !Part_1.CrateBoxEliComplite_2) || (Part_1.CrateBox[1] != null && !Part_1.CrateBoxEliComplite_2 && !Part_1.CrateBox[1].activeSelf))
					{
						Part_1.CrateBoxEliComplite_2 = true;
					}
					if ((Part_1.CrateBox[2] == null && !Part_1.CrateBoxEliComplite_3) || (Part_1.CrateBox[2] != null && !Part_1.CrateBoxEliComplite_3 && !Part_1.CrateBox[2].activeSelf))
					{
						Part_1.CrateBoxEliComplite_3 = true;
						StartCoroutine(HideArrow(0f));
						Part_1.TouchColl[4].GetComponent<Collider>().enabled = true;
						StartCoroutine("Part_0_1_2");
						StartCoroutine(Mission(1, false, 0f));
					}
					if ((Part_1.CrateBox[3] == null && !Part_1.CrateBoxEliComplite_4) || (Part_1.CrateBox[3] != null && !Part_1.CrateBoxEliComplite_4 && !Part_1.CrateBox[3].activeSelf))
					{
						Part_1.CrateBoxEliComplite_4 = true;
						StartCoroutine(HideArrow(0f));
						HintHand.SetActive(false);
						Part_1.TouchColl = new GameObject[0];
						if (CoinsQuest)
						{
							StartCoroutine("Part_0_1_3_0");
						}
						else
						{
							StartCoroutine("Part_0_1_3");
						}
						StartCoroutine(Mission(1, false, 0f));
					}
				}
				if (Part_1.MiniMapCamera.activeSelf && !Part_1.MiniMapOpen)
				{
					Part_1.MiniMapOpen = true;
					StartCoroutine(SetActiveFalse(Hand2D, 0f));
					showShadowPlane = false;
					shadow.gameObject.GetComponent<BoxCollider>().enabled = false;
					shadow.gameObject.SetActive(false);
				}
				if (Part_1.MiniMapOpen && !Part_1.MiniMapClse && !Part_1.MiniMapCamera.activeSelf)
				{
					Part_1.MiniMapClse = true;
					prontoWeit = 0f;
					prontoWeitHate = 0f;
					StartCoroutine(Part_0_1_3_2(0.333f));
				}
				if (Time.timeScale == 0f && Hand2D2.activeSelf && PopupAppStore.activeSelf)
				{
					Hand2D2.GetComponent<MeshRenderer>().enabled = false;
				}
				else
				{
					Hand2D2.GetComponent<MeshRenderer>().enabled = true;
				}
				if (!Part_1.CheckPointComplite_06 && Vector3.Distance(Pronto.transform.position, Part_1.ProntoPoint_0_6.position) < 1f && Part_1.CoinsQuestComplite)
				{
					if (Vector3.Distance(Pronto.transform.position, Eli.transform.position) < 4f)
					{
						Part_1.CheckPointComplite_06 = true;
						StartCoroutine(MoveTarget(1, 5f, Part_1.ProntoPoint_0_7.position));
					}
					if (Vector3.Distance(Pronto.transform.position, Part_1.ProntoPoint_0_7.position) > Vector3.Distance(Eli.transform.position, Part_1.ProntoPoint_0_7.position))
					{
						Part_1.CheckPointComplite_06 = true;
						StartCoroutine(MoveTarget(1, 5f, Part_1.ProntoPoint_0_7.position));
					}
					WeitPronto();
				}
				else if (Part_1.CrateBoxEliComplite_4 && !Part_1.MiniMapOpen && !Part_1.CoinsQuestComplite)
				{
					if (!ShowText)
					{
						prontoWeit += Time.deltaTime;
						prontoWeitHate += Time.deltaTime;
					}
					if (prontoWeit > 10f)
					{
						if (CountProntotext < 5)
						{
							StartCoroutine(ShowTextHintAdd("icon_pronto", AdditionalText[6].text, 0f));
						}
						else
						{
							StartCoroutine(ShowTextHintAdd("icon_pronto", AdditionalText[7].text, 0f));
							CountProntotext = UnityEngine.Random.Range(0, 3);
						}
						StartCoroutine(HideTextHintAdd(3f));
						prontoWeit = 0f;
						CountProntotext++;
					}
				}
				if (!Part_1.CheckPointComplite_07 && Vector3.Distance(Pronto.transform.position, Part_1.ProntoPoint_0_7.position) < 1f)
				{
					if (Vector3.Distance(Pronto.transform.position, Eli.transform.position) < 4f)
					{
						Part_1.CheckPointComplite_07 = true;
						Part_1.CheckPointComplite_06 = true;
						StartCoroutine(MoveTarget(1, 5f, Part_1.ProntoPoint_0_8.position));
					}
					WeitPronto();
				}
				if (Part_1.EliFix_06)
				{
				}
				if (!Part_1.EliFix_06 && Vector3.Distance(Eli.transform.position, Part_1.ProntoPoint_0_8.position) < 8f)
				{
					Part_1.EliFix_06 = true;
					Part_1.CheckPointComplite_07 = true;
					Part_1.CheckPointComplite_06 = true;
					StartCoroutine(DisableInput(0f));
					StartCoroutine(DisableMoveIndex(1, 0f));
					StartCoroutine(DisableMoveIndex(2, 0f));
					StartCoroutine(MoveTarget(1, 5f, Part_1.ProntoPoint_0_8.position, 0.01f));
					StartCoroutine(MoveTarget(0, 5f, Part_1.EliPoint_0_2.position, 0.01f));
				}
				else if (Vector3.Distance(Pronto.transform.position, Part_1.ProntoPoint_0_8.position) < 1f)
				{
					WeitPronto();
				}
				if (Part_1.EliFix_06)
				{
				}
				if (!Part_1.ProntoFix_08 && Part_1.EliFix_06 && Vector3.Distance(Pronto.transform.position, Part_1.ProntoPoint_0_8.position) <= 1f && Vector3.Distance(Eli.transform.position, Part_1.EliPoint_0_2.position) < 1f)
				{
					Part_1.ProntoFix_08 = true;
					prontoWeit = 0f;
					StartCoroutine(Mission(2, false, 6f));
					_cutScenePart = CutScenePart.Part_2;
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
				if ((!Part_2.BarrelEliComplite && Part_2.Barrel == null) || (!Part_2.BarrelEliComplite && Part_2.Barrel != null && !Part_2.Barrel.activeSelf))
				{
					Part_2.BarrelEliComplite = true;
					StartCoroutine(DisableInput(0f));
					StartCoroutine(HideArrow(0f));
					StartCoroutine(ProntoEnableDisable(true, 0f));
					StartCoroutine(Part_0_2_1(0.1f));
					StartCoroutine(Mission(3, false, 0f));
					HintHand.SetActive(false);
				}
				else if (Part_2.Barrel != null)
				{
					WeitPronto();
				}
				if (Part_2.KickParent)
				{
					Pronto.transform.position = Vector3.Slerp(Pronto.transform.position, Part_2.ExposionParent.position, Time.deltaTime * 6f);
				}
				if (Part_2.Medical != null && Vector3.Distance(Part_2.Medical.transform.position, Pronto.transform.position) < 1f)
				{
					UnityEngine.Object.Destroy(Part_2.Medical);
					_prontoController.Model.AddHealth(_prontoController.Model.Health);
				}
				if (!Part_2.EnemyPatrolLock)
				{
					if (Part_2.EnemyPatrolPoints.Length > 0 && Part_2.EnemyTempPoint == null)
					{
						Part_2.EnemyTempPoint = Part_2.EnemyPatrolPoints[UnityEngine.Random.Range(0, Part_2.EnemyPatrolPoints.Length)].gameObject;
						if (Vector3.Distance(Part_2.EnemyTempPoint.transform.position, Enemy_1.transform.position) >= 1f)
						{
							StartCoroutine(MoveTarget(2, 3f, Part_2.EnemyTempPoint.transform.position));
						}
						else
						{
							Part_2.EnemyTempPoint = null;
						}
					}
					if (Part_2.EnemyTempPoint != null && Vector3.Distance(Part_2.EnemyTempPoint.transform.position, Enemy_1.transform.position) <= 1f)
					{
						Part_2.EnemyTempPoint = null;
					}
				}
				if (!Part_2.CheckPointComplite_10 && Vector3.Distance(Pronto.transform.position, Part_2.ProntoPoint_0_10.position) < 1f && Vector3.Distance(Pronto.transform.position, Eli.transform.position) < 3f)
				{
					Part_2.CheckPointComplite_10 = true;
					StartCoroutine(ShowTextHint("icon_pronto", Text[4].GetComponent<TextMesh>().text, 1f));
					StartCoroutine(HideTextHint(2f));
					StartCoroutine(MoveTarget(1, 5f, Part_2.ProntoPoint_0_11.position, 2f));
					StartCoroutine(EnableMoveIndex(2, 2.1f));
					StartCoroutine(EnableInput(2.2f));
				}
				if (!Part_2.CheckPointComplite_11 && Vector3.Distance(Pronto.transform.position, Eli.transform.position) < 4f && Vector3.Distance(Pronto.transform.position, Part_2.ProntoPoint_0_11.position) < 4f)
				{
					Part_2.CheckPointComplite_11 = true;
					StartCoroutine(MoveTarget(1, 5f, Part_2.ProntoPoint_0_12.position));
				}
				else if (!Part_2.CheckPointComplite_11 && Vector3.Distance(Pronto.transform.position, Part_2.ProntoPoint_0_11.position) < 1f)
				{
					WeitPronto();
				}
				if (Part_2.battleEnemy)
				{
					StartCoroutine(DisableInput(0f));
					Part_2.EnemyPatrolLock = true;
					StartCoroutine(MoveTarget(0, 3f, Part_2.EliPoint_0_5.position, 0.01f));
					StartCoroutine(MoveTarget(2, 6f, Part_2.EnemyPoint_0_1.position, 0.1f));
					Part_2.battleEnemy = false;
					Part_3.TouchLayer.enabled = true;
					Part_2.battleEnemyWeit = true;
				}
				if (Part_2.EnemyPatrolLock)
				{
				}
				if (Part_2.battleEnemyWeit && Vector3.Distance(Eli.transform.position, Part_2.EliPoint_0_5.position) < 0.6f)
				{
					Part_2.battleEnemyWeit = false;
					StartCoroutine(ShowTextHint("icon_sentinel", Text[31].GetComponent<TextMesh>().text, 1f));
					StartCoroutine(HideTextHint(2f));
					StartCoroutine(SetActiveTrue(Part_2.Arena, 2f));
					StartCoroutine(Part_0_2_2(3f));
				}
			}
		}
		if (_cutScenePart == CutScenePart.Part_3)
		{
			if (!Part_3.close)
			{
				StartCoroutine("Part_0_3");
				Part_3.close = true;
			}
			else
			{
				if (Eli_Target.activeSelf)
				{
					if (Part_3.TouchLayer != null && !Part_3.EnemyKick && Part_3.TouchLayer.enabled)
					{
						Part_3.TouchLayer.enabled = false;
						_cameraSmoothFollow.SetTarget(Enemy_1.transform);
						HintHand.SetActive(false);
						prontoWeit = 0f;
						prontoWeitHate = 0f;
					}
					if (_eliController.Model.Health < 300f)
					{
						_eliController.Model.AddHealth(2f);
						StartCoroutine(MoveTarget(0, 5f, _cameraSmoothFollow.transform.position - _cameraSmoothFollow.cameraOffset));
					}
					if (!Part_3.TrixieHelp && _enemy_1_Controller.Model.Health <= 350f)
					{
						Part_3.TrixieHelp = true;
						_trixieController.gameObject.SetActive(true);
						StartCoroutine(MoveTarget(3, 5f, Part_3.TrixiePoint_0_1.position));
						_trixieController.SetAttackTarget(Enemy_1.transform);
					}
					if (Vector3.Distance(Part_3.TrixiePoint_0_1.position, _trixieController.transform.position) < 1f)
					{
						_trixieController.MoveTarget.position = _trixieController.transform.position;
						if (!Part_3.TrixieStop)
						{
							Part_3.TrixieStop = true;
						}
					}
					if (!Part_3.EnemyKick && _enemy_1_Controller.Model.Health <= 175f)
					{
						Part_3.EnemyKick = true;
						_enemy_1_Controller.Model.Invulnerability = true;
						_enemy_1_Controller.SetAttackTarget(null);
						_enemy_1_Controller.AttackTarget = null;
						_eliController.CanAttack = false;
						_enemy_1_Controller.enabled = false;
						_prontoController.AttackTarget = null;
						_trixieController.AttackTarget = null;
						_trixieController.enabled = false;
						_enemy_1_Controller.OnFallDown();
						StartCoroutine(DisableMoveIndex(2, 0f));
						Part_3.Arena.SetActive(false);
						if (Vector3.Distance(Eli.transform.position, Enemy_1.transform.position) > 4f)
						{
							Part_3.EnemyWeitPos = _cameraSmoothFollow.transform.position - _cameraSmoothFollow.cameraOffset;
							StartCoroutine(MoveTarget(0, 3f, Part_3.EnemyWeitPos, 1f));
							Part_3.EnemyWeit = true;
						}
						else
						{
							_cameraSmoothFollow.SetTarget(Eli_Move_Target.transform);
							StartCoroutine(ShowTextHint("icon_pronto", Text[38].GetComponent<TextMesh>().text, 1f));
							StartCoroutine(Part_0_3_1(2f));
						}
					}
					if (Vector3.Distance(Eli.transform.position, Part_3.EnemyWeitPos) < 0.6f && Part_3.EnemyWeit)
					{
						_cameraSmoothFollow.SetTarget(Eli_Move_Target.transform);
						StartCoroutine(ShowTextHint("icon_pronto", Text[38].GetComponent<TextMesh>().text, 0f));
						StartCoroutine(Part_0_3_1(1f));
						Part_3.EnemyWeit = false;
					}
				}
				else
				{
					if (Part_3.TouchLayer != null && !Eli_Target.activeSelf && !Part_3.TouchLayer.enabled && !Part_3.EnemyKick)
					{
						_eliController.SetAttackTarget(Enemy_1.transform);
						Eli_Target.SetActive(true);
						return;
					}
					if (!Part_3.EnemyKick)
					{
						WeitPronto();
					}
				}
				if (Part_3.TargetFalse)
				{
					if (Eli_Target.activeSelf)
					{
						WeitPronto();
						if (!Part_3.FixDtap)
						{
							Part_3.FixDtap = true;
							StartCoroutine(EliReadyFire(21f));
						}
					}
					else
					{
						Part_3.TargetFalse = false;
						HintHand.SetActive(false);
						HintHandDT.SetActive(false);
						ActiveInput = false;
						DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
						Part_3.TouchLayer.enabled = false;
						StartCoroutine(EnableMoveIndex(2, 0f));
						StartCoroutine(ShowTextHint("icon_sentinel", Text[35].GetComponent<TextMesh>().text, 1f));
						StartCoroutine(HideTextHint(2f));
						StartCoroutine(MoveTarget(2, 10f, Part_2.EliPoint_0_4.position, 2.5f));
						StartCoroutine(MoveTarget(0, 3f, Part_2.EnemyPoint_0_1.position, 3.25f));
						StartCoroutine(Fade_Maincamera(5f));
						StartCoroutine(LookAtTarget(_trixieController.gameObject, 300, 7f));
						StartCoroutine(ReplaceCamers(6f));
						StartCoroutine(AnimationPlay(Part_3.CutSceneCamera, 1f, "TutorialTrixieFirstMeeting", 6.1f));
						StartCoroutine(ShowTextHint("icon_trixie", Text[39].GetComponent<TextMesh>().text, 9f));
						StartCoroutine(HideTextHint(10f));
						StartCoroutine(MoveTarget(3, 5f, Part_3.TrixiePoint_0_2.position, 10.5f));
						StartCoroutine(Fade_Maincamera(10f));
						StartCoroutine(ReplaceCamers(11f));
						StartCoroutine(SetActiveFalse(Enemy_1, 11f));
						StartCoroutine(DestroyObj(Enemy_1, 11f));
						StartCoroutine(ShowTextHint("icon_pronto", Text[41].GetComponent<TextMesh>().text, 14f));
						StartCoroutine(HideTextHint(15f));
						StartCoroutine(ProntoEnableDisable(true, 15f));
						StartCoroutine(MoveTarget(1, 6.5f, Part_4.ProntoPoint_0_14.position, 15f));
						StartCoroutine(MoveTarget(0, 6.5f, Part_4.EliPoint_0_7.position, 15.4f));
						StartCoroutine(Part_0_3_2(16f));
					}
				}
				if (Part_3.CheckPointComplite_13)
				{
					Part_3.CheckPointComplite_13 = false;
					_cutScenePart = CutScenePart.Part_4;
				}
			}
		}
		if (_cutScenePart == CutScenePart.Part_4)
		{
			if (!Part_4.close)
			{
				StartCoroutine(Part_0_4());
				Part_4.close = true;
			}
			else
			{
				if (!Part_4.CheckPointComplite_17 && Vector3.Distance(Pronto.transform.position, Eli.transform.position) < 8f && Vector3.Distance(Pronto.transform.position, Part_4.ProntoPoint_0_17.position) < 1f)
				{
					Part_4.CheckPointComplite_17 = true;
					StartCoroutine(DisableInput(0f));
					StartCoroutine(Part_0_4_1(1f));
				}
				if (!Part_4.CheckPointComplite_17)
				{
					WeitPronto();
				}
			}
		}
		if (showShadowPlane && HUDPanel.activeSelf)
		{
			if (!CutSceneUse)
			{
				gameController.PlayCutScene(true);
				CutSceneUse = true;
			}
			if (black.a < 0.75f)
			{
				black.a += Time.deltaTime;
				shadow.material.color = black;
			}
		}
	}

	private IEnumerator Part_0_0()
	{
		yield return new WaitForSeconds(0f);
		StartCoroutine(AnimationPlay(Part_0.CutSceneCamera, 1f, "CameraAnimationLegacyTest"));
		StartCoroutine(AnimationPlay(Part_0.House_Round_Door_L, 1f, "Eli_Home_Door_L_Open", 17.5f));
		StartCoroutine(AnimationPlay(Part_0.House_Round_Door_R, 1f, "Eli_Home_Door_R_Open", 17.75f));
		StartCoroutine(MoveTarget(0, 5f, Part_0.EliPoint_0_1.position, 19f));
		StartCoroutine(MoveTarget(1, 5f, Part_0.ProntoPoint_0_1.position, 18.5f));
		StartCoroutine(ShowTextHint("icon_pronto", Text[0].GetComponent<TextMesh>().text, 21f));
		StartCoroutine(HideTextHint(22f));
		StartCoroutine(Fade_Maincamera(22f));
		StartCoroutine(Part_0_0_1(23f));
		StartCoroutine(ReplaceCamers(23f));
	}

	private IEnumerator Part_0_0_1(float weit)
	{
		yield return new WaitForSeconds(weit);
		StartCoroutine(SetActiveTrue(Part_0.EliCheckPoint_0_1, 0.1f));
		Part_0.Wall.SetActive(true);
		StartCoroutine(MoveTarget(1, 5f, Part_0.ProntoPoint_0_2.position, 1.5f));
	}

	private IEnumerator Part_0_1()
	{
		yield return new WaitForSeconds(0f);
		StartCoroutine(DisableInput(0f));
		StartCoroutine(ShowTextHint("icon_pronto", Text[8].GetComponent<TextMesh>().text, 1f));
		StartCoroutine(HideTextHint(2f));
		StartCoroutine(ShowArrow(Part_1.CrateBoxPronto.transform.position + Vector3.up * 0.55f, 2f));
		StartCoroutine(ProntoSetTarget(Part_1.CrateBoxPronto, 2f));
		StartCoroutine(HideArrow(2.5f));
		StartCoroutine(ShowTextHint("icon_pronto", Text[15].GetComponent<TextMesh>().text, 4f));
		StartCoroutine(HideTextHint(5f));
		StartCoroutine(ShowArrow(Part_1.CrateBox[2].transform.position + Vector3.up * 0.55f, 5f));
		StartCoroutine(ProntoEnableDisable(false, 5f));
		StartCoroutine(Part_0_1_1(5.6f));
		StartCoroutine(Mission(1, true, 5f));
	}

	private IEnumerator Part_0_1_1(float t)
	{
		yield return new WaitForSeconds(t);
		Part_1.TouchColl[1].GetComponent<Collider>().enabled = true;
		Part_1.TouchColl[3].GetComponent<Collider>().enabled = true;
		StartCoroutine(EnableInput(0f));
		StartCoroutine(Mission(1, true, 0f));
	}

	private IEnumerator Part_0_1_2()
	{
		yield return new WaitForSeconds(0f);
		StartCoroutine(ShowArrow(Part_1.CrateBox[3].transform.position + Vector3.up * 0.55f, 0f));
		StartCoroutine(Mission(1, true, 0f));
	}

	private IEnumerator Part_0_1_3()
	{
		yield return new WaitForSeconds(0f);
		StartCoroutine(ShowTextHint("icon_pronto", Text[23].GetComponent<TextMesh>().text, 1f));
		StartCoroutine(HideTextHint(2f));
		StartCoroutine(ProntoEnableDisable(true, 2f));
		Part_1.CoinsQuestComplite = true;
		StartCoroutine(EnableMoveIndex(1, 2f));
		StartCoroutine(MoveTarget(1, 5f, Part_1.ProntoPoint_0_6.position, 3f));
		StartCoroutine(Mission(2, true, 4f));
	}

	private IEnumerator Part_0_1_3_0()
	{
		yield return new WaitForSeconds(0f);
		StartCoroutine(ShowTextHint("icon_pronto", Text[23].GetComponent<TextMesh>().text, 1f));
		StartCoroutine(HideTextHint(2f));
		StartCoroutine(ProntoEnableDisable(true, 2f));
		StartCoroutine(MoveTarget(0, 5f, Part_1.EliPoint_CointsQuest.position, 2f));
		StartCoroutine(MoveTarget(1, 5f, Part_1.ProntoPoint_0_6.position, 1.75f));
		StartCoroutine(Part_0_1_3_1(4f));
	}

	private IEnumerator Part_0_1_3_1(float weit)
	{
		yield return new WaitForSeconds(weit);
		StartCoroutine(ShowTextHint("icon_pronto", Text[63].GetComponent<TextMesh>().text, 0f));
		StartCoroutine(HideTextHint(1f));
		StartCoroutine(SetActiveTrue(MiniMapPanel, 0.75f));
		StartCoroutine(SetActiveTrue(Hand2D, 2f));
		showShadowPlane = true;
		shadow.gameObject.GetComponent<BoxCollider>().enabled = true;
	}

	private IEnumerator Part_0_1_3_2(float weit)
	{
		yield return new WaitForSeconds(weit);
		StartCoroutine(ShowTextHint("icon_pronto", Text[4].GetComponent<TextMesh>().text, 0f));
		StartCoroutine(HideTextHint(1f));
		Part_1.CoinsQuestComplite = true;
		StartCoroutine(SetActiveFalse(Hand2D2, 0f));
		StartCoroutine(EnableMoveIndex(1, 1f));
		StartCoroutine(Mission(2, true, 1f));
		prontoWeit = 0f;
		prontoWeitHate = 0f;
	}

	private IEnumerator Part_0_2()
	{
		yield return new WaitForSeconds(0f);
		StartCoroutine(ProntoEnableDisable(false, 0f));
		StartCoroutine(ShowTextHint("icon_pronto", Text[24].GetComponent<TextMesh>().text, 1f));
		StartCoroutine(HideTextHint(2f));
		StartCoroutine(ShowArrow(Part_2.Barrel.transform.position, 2f));
		StartCoroutine(EnableInput(2f));
		StartCoroutine(Mission(3, true, 2f));
	}

	private IEnumerator Part_0_2_1(float weit)
	{
		yield return new WaitForSeconds(weit);
		_prontoController.Model.AddHealth((0f - _prontoController.Model.Health) * 0.5f);
		StartCoroutine(ProntoKick(0f));
		Part_2.ExposionParent.GetComponent<Animator>().enabled = true;
		brige.enabled = true;
		StartCoroutine(ShowTextHint("icon_pronto", Text[25].GetComponent<TextMesh>().text, 1f));
		StartCoroutine(HideTextHint(2f));
		StartCoroutine(ProntoUp(2f));
		StartCoroutine(MoveTarget(1, 5f, Part_2.ProntoPoint_0_9.position, 4f));
		StartCoroutine(MoveTarget(0, 4f, Part_2.EliPoint_0_3.position, 5.25f));
		StartCoroutine(ShowTextHint("icon_pronto", Text[30].GetComponent<TextMesh>().text, 8f));
		StartCoroutine(HideTextHint(9f));
		StartCoroutine(MoveTarget(1, 5f, Part_2.ProntoPoint_0_10.position, 10f));
		StartCoroutine(MoveTarget(0, 4f, Part_2.EliPoint_0_4.position, 10.5f));
	}

	private IEnumerator Part_0_2_2(float weit)
	{
		yield return new WaitForSeconds(weit);
		_cutScenePart = CutScenePart.Part_3;
	}

	private IEnumerator Part_0_3()
	{
		yield return new WaitForSeconds(0f);
		StartCoroutine(ProntoEnableDisable(false, 0f));
		StartCoroutine(EnableInput(0f));
	}

	private IEnumerator Part_0_3_1(float weit)
	{
		yield return new WaitForSeconds(weit);
		StartCoroutine(HideTextHint(0f));
		_enemy_1_Controller.OnStandUp();
		Part_3.Arena.SetActive(false);
		Part_3.TouchLayer.enabled = true;
		Part_3.TargetFalse = true;
		prontoWeitHate = 3f;
	}

	private IEnumerator Part_0_3_2(float weit)
	{
		yield return new WaitForSeconds(weit);
		Part_3.CheckPointComplite_13 = true;
	}

	private IEnumerator Part_0_4()
	{
		yield return new WaitForSeconds(0f);
		StartCoroutine(EnableMoveIndex(2, 0f));
		StartCoroutine(EnableMoveIndex(3, 1f));
	}

	private IEnumerator Part_0_4_1(float weit)
	{
		yield return new WaitForSeconds(weit);
		Kord.SetActive(true);
		arrowScreen.SetActive(false);
		StartCoroutine(SetActiveFalse(HUDPanel, 0f));
		StartCoroutine(ShowTextHint("icon_pronto", Text[44].GetComponent<TextMesh>().text, 0f));
		StartCoroutine(HideTextHint(1f));
		StartCoroutine(MoveTarget(1, 5f, Part_4.ProntoPoint_0_18.position, 1f));
		StartCoroutine(MoveTarget(4, 5f, Part_4.KordPoint_0_1.position, 4f));
		StartCoroutine(ShowTextHint("icon_kord", Text[52].GetComponent<TextMesh>().text, 9f));
		StartCoroutine(HideTextHint(10f));
		StartCoroutine(Fade_Maincamera(10f));
		StartCoroutine(HideMeinCamera(11.5f));
		StartCoroutine(Part_0_4_2(11.5f));
	}

	private IEnumerator Part_0_4_2(float weit)
	{
		yield return new WaitForSeconds(weit);
		PlayerPrefs.SetInt(typeof(ShaneTutor_0_1).Name, 1);
		gameController.PlayCutScene(false);
		CutSceneUse = false;
		RisePlayCutSceneEvent(false);
		gameController.OnFinishGame();
	}

	public void SkipTutor()
	{
		Time.timeScale = 0f;
		PopupConfirmShow = true;
		PopupConfirm.SetActive(true);
		btnYes.targetObject = gameObject;
		btnYes.messageName = "Yes";
		btnNo.targetObject = gameObject;
		btnNo.messageName = "No";
		PopupConfirmText.text = MonoSingleton<Localization>.Instance["defeat_skip_desc"];
		gameController.PlayCutScene(true);
		CutSceneUse = true;
		Time.timeScale = 0f;
	}

	public void Yes()
	{
		PlayerPrefs.SetInt(typeof(ShaneTutor_0_1).Name, 1);
		gameController.PlayCutScene(false);
		CutSceneUse = false;
		RisePlayCutSceneEvent(false);
		Time.timeScale = 0f;
		gameController.SkipMission();
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
		Time.timeScale = 1f;
	}

	private IEnumerator Skip_Part_0_4_2(float weit)
	{
		yield return new WaitForSeconds(weit);
		DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
		PlayerPrefs.SetInt(typeof(ShaneTutor_0_1).Name, 1);
		gameController.PlayCutScene(false);
		CutSceneUse = false;
		RisePlayCutSceneEvent(false);
		gameController.SkipMission();
	}
}

using System.Collections;
using System.Collections.Generic;
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

public class CutScene_6_3 : SlugterraCutScene
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

	private bool _skip;

	public AnimationClip AninationStartScene;

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

	public string[] text;

	private bool part_0;

	private bool part_1;

	private bool part_2;

	private bool part_3;

	private bool part_4;

	private bool part_5;

	private bool part_6;

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

	public TwistController _twistControllerBoss_1;

	public TwistController _twistControllerBoss_2;

	public TwistController _twistControllerBoss_3;

	public SphereCollider AreaBoss1;

	public SphereCollider AreaBoss3;

	public TwistController _twistControllerFriend;

	public GameObject Part_0_Gate_1;

	public Transform Part_0_Twist_Point;

	public Transform Part_0_Eli_Point;

	public Transform Part_0_Eli_Point_speak;

	public Transform Part_0_Twist_Point_Hide;

	public Transform Part_1_Twist_Point_Hide;

	public Transform Part_1_Eli_Point_speak;

	public AnimationClip Part_1_AninationTwistCry;

	public AnimationClip Part_1_AninationTwistEnemy;

	public Transform Part_1Go_2;

	public GameObject Part_3_Gate_2;

	public AnimationClip Part_3_OpenGateView_1;

	public GameObject Part_4_Gate_3;

	public Transform Part_4_Twist_Boss3;

	public Transform Part_4_Eli_Point_speak;

	public Transform Part_5Go_6;

	public AnimationClip Part_5_AninationTwistEnemy;

	public Transform Part_Go_end;

	public GameObject FixEndRoom;

	public AnimationClip Part_End_AninationView;

	public GameObject Part_6_Gate_1;

	public GameObject Part_6_twist_hide;

	public GameObject Part_6_twist_end;

	public GameObject Pronto;

	public ProntoController _pronto;

	public GameObject Kord;

	public KordController _kord;

	public GameObject Trixie;

	public TrixieController _trixie;

	public Transform Part_6_end_point_eli;

	public Transform Part_6_end_point_pronto;

	public Transform Part_6_end_point_kord;

	public Transform Part_6_end_point_trixie;

	public GameObject Part_6_AI;

	public AnimationClip TowerGateOpen;

	public AnimationClip TowerGateClose;

	public LayerMask ScanLayer;

	private AssistantController _assistantController;

	private RaycastHit hit;

	private bool cloneInstantiate;

	private int s;

	private void Start()
	{
		gameController.PlayCutScene(true);
		CutSceneUse = true;
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		MainCamera = GameObject.Find("MainCamera");
		UICamera = GameObject.Find("UICamera");
		_assistantController = Object.FindObjectOfType<AssistantController>();
		if (_assistantController != null && _assistantController is KordController)
		{
			_assistantController.Model.AddSpeedCoeficient(GetType(), 1.3f);
		}
		UICamera.GetComponent<Camera>().enabled = false;
		HUDPanel.SetActive(false);
		AreaBoss1.enabled = false;
		AreaBoss3.enabled = false;
		_twistControllerBoss_1.transform.parent.gameObject.SetActive(false);
		_twistControllerBoss_3.transform.parent.gameObject.SetActive(false);
		_eliController = Object.FindObjectOfType<EliController>();
		_twistController = Object.FindObjectOfType<TwistController>();
		_nachoController = Object.FindObjectOfType<NachoController>();
		_cutSceneView.SkipButton.targetObject = gameObject;
		_cutSceneView.SkipButton.messageName = "Skip";
		_skip = false;
		_cameraFade = MainCamera.GetComponent<CameraFade>();
		_cameraSmoothFollow = MainCamera.GetComponent<CameraSmoothFollow>();
		_cameraSmoothFollow.enabled = false;
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 1f), 1f, 0.1f, 2f);
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

	public void Skip()
	{
		_skip = true;
		Fade();
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
				StartCoroutine(_part_1_skip(0.5f));
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
				StartCoroutine(_part_2_skip(0.5f));
				_skip = false;
			}
			AreaBoss1.enabled = true;
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
			AreaBoss3.enabled = true;
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

	private IEnumerator _part_default(float t, float s = 1.5f)
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

	private IEnumerator _part_0_start(float t)
	{
		StartCoroutine(AnimationPlay(Part_0_Gate_1, 1f, TowerGateOpen.name));
		yield return new WaitForSeconds(t);
		if ((bool)AninationStartScene)
		{
			MainCamera.GetComponent<Animation>().Play(AninationStartScene.name);
			StartCoroutine(MoveTargetEnemy(_twistControllerFriend, 2.8f, Part_0_Twist_Point.position));
			_eliController.MoveTarget.position = Part_0_Eli_Point.position;
			_eliController.OnMove(2.8f);
			StartCoroutine(AnimationPlay(Part_0_Gate_1, 0.15f, TowerGateClose.name, 6f));
			StartCoroutine(TextHint(4f, 0f, "icon_twist", text[0]));
			StartCoroutine(TextHint(7f, 0f, "icon_eli", text[1]));
			StartCoroutine(TextHint(10f, 4.1f, "icon_twist", text[2]));
			StartCoroutine(MoveTargetEnemy(_twistControllerFriend, 8f, Part_0_Twist_Point_Hide.position, 13f));
		}
		part_0 = true;
	}

	private IEnumerator _part_0_skip(float t)
	{
		yield return new WaitForSeconds(t);
		if (_assistantController != null)
		{
			if (Vector3.Distance(_assistantController.transform.position, _eliController.transform.position) > 5f)
			{
				_assistantController.transform.position = _eliController.transform.position;
			}
			_assistantController.Model.AddSpeedCoeficient(GetType(), 1f);
		}
		StartCoroutine(MoveTargetEnemy(_twistControllerFriend, 9f, Part_0_Twist_Point_Hide.position));
		StartCoroutine(AnimationPlay(Part_0_Gate_1, 10f, TowerGateClose.name));
	}

	private IEnumerator _part_1_start(float t)
	{
		yield return new WaitForSeconds(t);
		gameController.PlayCutScene(true);
		CutSceneUse = true;
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		_cameraSmoothFollow.enabled = false;
		StartCoroutine(AnimationPlay(MainCamera, 1f, Part_1_AninationTwistCry.name, 0.5f));
		UICamera.GetComponent<Camera>().enabled = false;
		HUDPanel.SetActive(false);
		_eliController.MoveTarget.position = _eliController.transform.position;
		_eliController.AttackTarget = null;
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 0f), 1f, 0.5f, 2f);
		StartCoroutine(_part_1_temp(0.5f));
		StartCoroutine(TextHint(2.5f, 0f, "icon_eli", text[3]));
		StartCoroutine(TextHint(5f, 0f, "icon_twist", text[4]));
		StartCoroutine(TextHint(7f, 0f, "icon_eli", text[5]));
		StartCoroutine(TextHint(10f, 0f, "icon_twist", text[6]));
		StartCoroutine(TextHint(14f, 4f, "icon_twist", text[7]));
		StartCoroutine(MoveTargetEnemy(_twistControllerFriend, 8f, Part_1_Twist_Point_Hide.position, 17f));
		part_1 = true;
	}

	private IEnumerator _part_1_temp(float t)
	{
		yield return new WaitForSeconds(t);
		_eliController.transform.position = Part_0_Eli_Point_speak.position;
		_eliController.MoveTarget.position = Part_0_Eli_Point_speak.position;
		_eliController.transform.localEulerAngles = Part_0_Eli_Point_speak.localEulerAngles;
		_twistControllerFriend.transform.localEulerAngles = Part_0_Twist_Point_Hide.localEulerAngles;
	}

	private IEnumerator _part_1_skip(float t)
	{
		yield return new WaitForSeconds(t);
		StartCoroutine(MoveTargetEnemy(_twistControllerFriend, 7f, Part_1_Twist_Point_Hide.position));
	}

	private IEnumerator _part_2_start(float t)
	{
		yield return new WaitForSeconds(t);
		gameController.PlayCutScene(true);
		CutSceneUse = true;
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		_cameraSmoothFollow.enabled = false;
		StartCoroutine(AnimationPlay(MainCamera, 1f, Part_1_AninationTwistEnemy.name, 0.5f));
		UICamera.GetComponent<Camera>().enabled = false;
		HUDPanel.SetActive(false);
		_eliController.MoveTarget.position = _eliController.transform.position;
		_eliController.AttackTarget = null;
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 0f), 1f, 0.5f, 2f);
		StartCoroutine(_part_2_temp(0.5f));
		StartCoroutine(TextHint(2.5f, 8f, "icon_twist", text[8]));
		StartCoroutine(SetActiveFalse(_twistControllerFriend.transform.parent.gameObject, 0.5f));
		StartCoroutine(SetActiveTrue(_twistControllerBoss_1.transform.parent.gameObject, 0.5f));
		StartCoroutine(_part_2_Show(6f));
		part_2 = true;
	}

	private IEnumerator _part_2_temp(float t)
	{
		yield return new WaitForSeconds(t);
		_eliController.transform.position = Part_1_Eli_Point_speak.position;
		_eliController.MoveTarget.position = Part_1_Eli_Point_speak.position;
		_eliController.transform.localEulerAngles = Part_1_Eli_Point_speak.localEulerAngles;
	}

	private IEnumerator _part_2_Show(float t)
	{
		yield return new WaitForSeconds(t);
		_twistControllerFriend.transform.position = Part_4_Twist_Boss3.position;
		_twistControllerFriend.transform.eulerAngles = Part_4_Twist_Boss3.eulerAngles;
		_twistControllerFriend.transform.parent.gameObject.SetActive(true);
		cloneInstantiate = false;
		TwistCount = 2;
		TwistCloneUse = true;
	}

	private IEnumerator _part_2_skip(float t)
	{
		yield return new WaitForSeconds(t);
		_twistControllerFriend.transform.position = Part_4_Twist_Boss3.position;
		_twistControllerFriend.transform.eulerAngles = Part_4_Twist_Boss3.eulerAngles;
		_twistControllerFriend.transform.parent.gameObject.SetActive(true);
		TwistCount = 2;
		TwistCloneUse = true;
	}

	private IEnumerator _part_3_start(float t)
	{
		yield return new WaitForSeconds(t);
		gameController.PlayCutScene(true);
		CutSceneUse = true;
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		_cameraSmoothFollow.enabled = false;
		StartCoroutine(AnimationPlay(MainCamera, 1f, Part_3_OpenGateView_1.name, 0.5f));
		UICamera.GetComponent<Camera>().enabled = false;
		HUDPanel.SetActive(false);
		_eliController.MoveTarget.position = _eliController.transform.position;
		_eliController.AttackTarget = null;
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 0f), 1f, 0.5f, 2f);
		StartCoroutine(TextHint(2.5f, 6f, "icon_eli", text[9]));
		StartCoroutine(AnimationPlay(Part_3_Gate_2, 0.3f, TowerGateOpen.name, 2f));
		TwistCount = 3;
		part_3 = true;
	}

	private IEnumerator _part_3_skip(float t)
	{
		yield return new WaitForSeconds(t);
		StartCoroutine(AnimationPlay(Part_3_Gate_2, 0.3f, TowerGateOpen.name));
	}

	private IEnumerator _part_4_start(float t)
	{
		yield return new WaitForSeconds(t);
		StartCoroutine(AnimationPlay(Part_4_Gate_3, 0.3f, TowerGateOpen.name, 2f));
	}

	private IEnumerator _part_5_start(float t)
	{
		yield return new WaitForSeconds(t);
		gameController.PlayCutScene(true);
		CutSceneUse = true;
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		_cameraSmoothFollow.enabled = false;
		StartCoroutine(AnimationPlay(MainCamera, 1f, Part_5_AninationTwistEnemy.name, 0.5f));
		UICamera.GetComponent<Camera>().enabled = false;
		HUDPanel.SetActive(false);
		_eliController.MoveTarget.position = _eliController.transform.position;
		_eliController.AttackTarget = null;
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 0f), 1f, 0.5f, 2f);
		StartCoroutine(_part_5_temp(0.5f));
		StartCoroutine(TextHint(2.5f, 0f, "icon_twist", text[10]));
		StartCoroutine(TextHint(6.5f, 0f, "icon_twist", text[11]));
		StartCoroutine(TextHint(10.5f, 4f, "icon_eli", text[12]));
		StartCoroutine(SetActiveFalse(_twistControllerFriend.transform.parent.gameObject, 0.5f));
		StartCoroutine(SetActiveTrue(_twistControllerBoss_3.transform.parent.gameObject, 0.5f));
		StartCoroutine(_part_5_Show(12.5f));
		part_5 = true;
	}

	private IEnumerator _part_5_temp(float t)
	{
		yield return new WaitForSeconds(t);
		_eliController.transform.position = Part_4_Eli_Point_speak.position;
		_eliController.MoveTarget.position = Part_4_Eli_Point_speak.position;
		_eliController.transform.localEulerAngles = Part_4_Eli_Point_speak.localEulerAngles;
	}

	private IEnumerator _part_5_Show(float t)
	{
		yield return new WaitForSeconds(t);
		cloneInstantiate = false;
		TwistCount = 4;
		TwistCloneUse = true;
	}

	private IEnumerator _part_5_skip(float t)
	{
		yield return new WaitForSeconds(t);
		TwistCount = 4;
		TwistCloneUse = true;
	}

	private IEnumerator _part_6_start(float t)
	{
		yield return new WaitForSeconds(t);
		gameController.PlayCutScene(true);
		CutSceneUse = true;
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		_cameraSmoothFollow.enabled = false;
		StartCoroutine(AnimationPlay(MainCamera, 1f, Part_End_AninationView.name, 0.5f));
		StartCoroutine(AnimationPlay(Part_6_Gate_1, 0.15f, TowerGateOpen.name, 5f));
		StartCoroutine(MoveTargetEnemy(_twistControllerFriend, 10f, Part_6_twist_end.transform.position, 7f));
		UICamera.GetComponent<Camera>().enabled = false;
		HUDPanel.SetActive(false);
		_eliController.MoveTarget.position = _eliController.transform.position;
		_eliController.AttackTarget = null;
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 0f), 1f, 0.5f, 2f);
		StartCoroutine(_part_6_temp(0.5f));
		StartCoroutine(TextHint(2.5f, 0f, "icon_pronto", text[13]));
		StartCoroutine(TextHint(5.5f, 0f, "icon_trixie", text[14]));
		StartCoroutine(TextHint(8.5f, 0f, "icon_kord", text[15]));
		StartCoroutine(TextHint(11.5f, 3f, "icon_eli", text[16]));
		part_6 = true;
	}

	private IEnumerator _part_6_temp(float t)
	{
		yield return new WaitForSeconds(t);
		Part_6_AI.SetActive(false);
		if (MonoSingleton<AppController>.Instance.Model.MissionSetting.Assistant.Type == AllyType.Kord || MonoSingleton<AppController>.Instance.Model.MissionSetting.Assistant.Type == AllyType.Pronto || MonoSingleton<AppController>.Instance.Model.MissionSetting.Assistant.Type == AllyType.Trixie)
		{
			AssistantController assistantController = Object.FindObjectOfType<AssistantController>();
			assistantController.transform.parent.gameObject.SetActive(false);
		}
		_twistControllerFriend.transform.position = Part_6_twist_hide.transform.position;
		StartCoroutine(SetActiveTrue(_twistControllerFriend.transform.parent.gameObject, 0.5f));
		Pronto.SetActive(true);
		Kord.SetActive(true);
		Trixie.SetActive(true);
		_pronto.MoveTarget.position = Part_6_end_point_pronto.position;
		_pronto.OnMove(2.5f);
		_pronto.enabled = false;
		_kord.MoveTarget.position = Part_6_end_point_kord.position;
		_kord.OnMove(2.5f);
		_kord.enabled = false;
		_trixie.MoveTarget.position = Part_6_end_point_trixie.position;
		_trixie.OnMove(2.5f);
		_trixie.enabled = false;
		_eliController.transform.position = Part_4_Twist_Boss3.position;
		_eliController.MoveTarget.position = Part_4_Twist_Boss3.position;
		_eliController.transform.localEulerAngles = Part_4_Twist_Boss3.localEulerAngles;
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
				}
			}
		}
		if (!cloneInstantiate && TwistCloneArray.Count < TwistCount)
		{
			if (_cutScenePart == CutScenePart.Part_2 && _twistControllerBoss_1 != null)
			{
				TwistRandomPointRoot.position = _twistControllerBoss_1.transform.position;
			}
			if (_cutScenePart == CutScenePart.Part_3 && _twistControllerBoss_2 != null)
			{
				TwistRandomPointRoot.position = _twistControllerBoss_2.transform.position;
			}
			if (_cutScenePart == CutScenePart.Part_5 && _twistControllerBoss_3 != null)
			{
				TwistRandomPointRoot.position = _twistControllerBoss_3.transform.position;
			}
			_randomPoint = Random.Range(0, clonePoint.Length);
			Debug.DrawRay(clonePoint[_randomPoint].position + -Vector3.up * 50f, transform.up * 100f, Color.gray, 5f);
			if (Physics.SphereCast(clonePoint[_randomPoint].position + Vector3.up * 50f, 1f, -transform.up, out hit, 100f, ScanLayer) && hit.collider.CompareTag("currentLevel"))
			{
				StartCoroutine(_InstantiateFx(0.333f));
				cloneInstantiate = true;
			}
		}
	}

	private IEnumerator _InstantiateFx(float t)
	{
		yield return new WaitForSeconds(t);
		if (TwistCloneUse)
		{
			if (_cutScenePart == CutScenePart.Part_2 && _twistControllerBoss_1 != null)
			{
				cloneFx = Object.Instantiate(TwistFXSpawn, hit.point, Quaternion.identity);
				StartCoroutine(_InstantiateClone(0.333f));
			}
			if (_cutScenePart == CutScenePart.Part_3 && _twistControllerBoss_2 != null)
			{
				cloneFx = Object.Instantiate(TwistFXSpawn, hit.point, Quaternion.identity);
				StartCoroutine(_InstantiateClone(0.4f));
			}
			if (_cutScenePart == CutScenePart.Part_5 && _twistControllerBoss_3 != null)
			{
				cloneFx = Object.Instantiate(TwistFXSpawn, hit.point, Quaternion.identity);
				StartCoroutine(_InstantiateClone(0.4f));
			}
		}
		else
		{
			cloneInstantiate = false;
		}
	}

	private IEnumerator _InstantiateClone(float t)
	{
		yield return new WaitForSeconds(t);
		clone = Object.Instantiate(TwistClone, cloneFx.transform.position, Quaternion.identity);
		cloneRotation = clone.transform.Find("TwistController");
		cloneRotation.transform.forward = -(cloneFx.transform.position - _eliController.transform.position);
		Rotation.y = cloneRotation.transform.eulerAngles.y;
		cloneRotation.transform.eulerAngles = Rotation;
		TwistCloneArray.Add(cloneRotation.gameObject);
		cloneInstantiate = false;
	}

	private void Update()
	{
		if (TwistCloneUse)
		{
			TwistRespawn();
		}
		if (_cutScenePart == CutScenePart.Part_0)
		{
			if (!Part_0)
			{
				Part_0 = true;
				StartCoroutine(_part_0_start(0f));
			}
			if (Vector3.Distance(_eliController.transform.position, Part_0_Twist_Point_Hide.position) < 6f)
			{
				_cutScenePart = CutScenePart.Part_1;
			}
		}
		if (_cutScenePart == CutScenePart.Part_1)
		{
			if (!Part_1)
			{
				Part_1 = true;
				StartCoroutine(_part_1_start(0f));
			}
			if (Vector3.Distance(_eliController.transform.position, Part_1_Twist_Point_Hide.position) < 6f)
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
			if (_twistControllerBoss_1 == null && TwistCloneArray.Count == 0 && _eliController.AttackTarget == null)
			{
				TwistCloneUse = false;
				Part_1Go_2.position = _eliController.transform.position;
				_cutScenePart = CutScenePart.Part_3;
			}
		}
		if (_cutScenePart == CutScenePart.Part_3)
		{
			if (!Part_3)
			{
				Part_3 = true;
				StartCoroutine(_part_3_start(1.5f));
			}
			if (!TwistCloneUse && _twistControllerBoss_2.AttackTarget != null)
			{
				TwistCloneUse = true;
			}
			if (_twistControllerBoss_2 == null && TwistCloneArray.Count == 0 && _eliController.AttackTarget == null)
			{
				TwistCloneUse = false;
				Part_5Go_6.position = _eliController.transform.position;
				_cutScenePart = CutScenePart.Part_4;
			}
		}
		if (_cutScenePart == CutScenePart.Part_4)
		{
			if (!Part_4)
			{
				Part_4 = true;
				StartCoroutine(_part_4_start(1.5f));
			}
			if (Vector3.Distance(_eliController.transform.position, Part_4_Twist_Boss3.position) < 7f)
			{
				_cutScenePart = CutScenePart.Part_5;
			}
		}
		if (_cutScenePart == CutScenePart.Part_5)
		{
			if (!Part_5)
			{
				Part_5 = true;
				StartCoroutine(_part_5_start(0f));
			}
			if (_twistControllerBoss_3 == null && TwistCloneArray.Count == 0)
			{
				TwistCloneUse = false;
				Part_Go_end.position = _eliController.transform.position;
				FixEndRoom.SetActive(false);
				_cutScenePart = CutScenePart.Part_6;
			}
		}
		if (_cutScenePart == CutScenePart.Part_6 && !Part_6)
		{
			Part_6 = true;
			StartCoroutine(_part_6_start(6f));
		}
	}
}

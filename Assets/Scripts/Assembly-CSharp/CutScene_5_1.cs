using System.Collections;
using AppsMinistry.Core;
using AppsMinistry.Core.Input;
using AppsMinistry.UI.Camera;
using Slugterra.Game.Units.Ally;
using Slugterra.Game.Units.Ally.Assistant;
using Slugterra.Game.Units.Enemy;
using Slugterra.Scene;
using Slugterra.UI.Game;
using Slugterra.UI.Game.Camera;
using UnityEngine;

public class CutScene_5_1 : SlugterraCutScene
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

	public GameObject finish;

	public AnimationClip AninationStartScene;

	public Transform[] Objective;

	public string[] text;

	public Transform[] WarningRadius;

	public Transform WarningRadiusPivot;

	private Transform ClearPoint;

	public Transform CoverPoint2;

	private bool ClearPointStop;

	private bool part_0;

	private bool part_1;

	private bool part_2;

	private bool part_3;

	private bool part_4;

	private bool part_5;

	public SentinelController[] guard;

	private int g;

	private int count;

	public GameObject TrigerPart_0;

	public GameObject TrigerPart_3;

	public Transform TrigerMessage;

	public GameObject FirstFindTriger;

	private bool _firstFindTriger;

	public Transform FinishTarget;

	private bool Scan;

	private float timer;

	public GameObject TowerDoor;

	public GameObject TowerGate;

	public GameObject TowerRoof;

	public GameObject AddGuardParent;

	public SentinelController[] AddGuard;

	public GameObject AddGuard2Parent;

	public SentinelController[] AddGuard2;

	public SentinelController[] AddGuard3;

	public Transform[] AddGuardPoint;

	public Transform Faza_2;

	public AnimationClip AninationDoorView;

	public AnimationClip GuardHelpView;

	public AnimationClip TowerDoorOpen;

	public AnimationClip TowerDoorClose;

	public AnimationClip TowerGateOpen;

	public AnimationClip TowerGateClose;

	public LayerMask RoofScan;

	public float tempScan;

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
				StartCoroutine(TextHint(2f, AninationStartScene.length - 2f, "icon_eli", text[0]));
				StartCoroutine(TextHint(8f, 0f, "icon_eli", text[1]));
				StartCoroutine(TextHint(13f, 0f, "icon_eli", text[2]));
			}
			else
			{
				StartCoroutine(TextHint(2f, 4f, "icon_eli", text[0]));
			}
			AddGuardParent.SetActive(false);
			AddGuard2Parent.SetActive(false);
		}
		if (_cutScenePart == CutScenePart.Part_3)
		{
			_eliController.transform.position = AddGuardPoint[0].transform.parent.transform.position;
		}
		if (_cutScenePart == CutScenePart.Part_4)
		{
			TrigerPart_0.SetActive(false);
			_eliController.transform.position = AddGuardPoint[0].transform.parent.transform.position;
			for (g = 0; g < AddGuard2.Length; g++)
			{
				AddGuard2[g].MoveTo(AddGuard[g].transform.parent.transform.position, 2.5f);
			}
			Object.Destroy(AddGuard[0].transform.parent.transform.parent.transform.gameObject);
			Object.Destroy(guard[0].transform.parent.transform.gameObject);
			Object.Destroy(guard[1].transform.parent.transform.gameObject);
			Objective[0].position = _eliController.transform.position;
			Objective[1].position = _eliController.transform.position;
			StartCoroutine(_part_0(0.11f));
		}
		if (_cutScenePart == CutScenePart.Part_5)
		{
			TrigerPart_0.SetActive(false);
			TrigerPart_3.SetActive(false);
			_eliController.transform.position = Faza_2.position;
			Object.Destroy(AddGuard[0].transform.parent.transform.parent.transform.gameObject);
			Object.Destroy(AddGuard2[0].transform.parent.transform.parent.transform.gameObject);
			Object.Destroy(guard[0].transform.parent.transform.gameObject);
			Object.Destroy(guard[1].transform.parent.transform.gameObject);
			Objective[0].position = _eliController.transform.position;
			Objective[1].position = _eliController.transform.position;
			StartCoroutine(_part_0(0.11f));
		}
		if (_cutScenePart == CutScenePart.Part_6)
		{
			TrigerPart_0.SetActive(false);
			TrigerPart_3.SetActive(false);
			_eliController.transform.position = Faza_2.position;
			Object.Destroy(AddGuard[0].transform.parent.transform.parent.transform.gameObject);
			Object.Destroy(AddGuard2[0].transform.parent.transform.parent.transform.gameObject);
			Object.Destroy(AddGuard3[0].transform.parent.transform.parent.transform.gameObject);
			Object.Destroy(guard[0].transform.parent.transform.gameObject);
			Object.Destroy(guard[1].transform.parent.transform.gameObject);
			StartCoroutine(AnimationPlay(TowerGate, 1f, TowerGateOpen.name, 4f));
			StartCoroutine(AnimationPlay(TowerDoor, 1f, TowerDoorOpen.name, 3f));
			if ((bool)FirstFindTriger)
			{
				FirstFindTriger.SetActive(false);
			}
			Objective[0].position = _eliController.transform.position;
			Objective[1].position = _eliController.transform.position;
			StartCoroutine(_part_0(0.11f, 0f));
		}
		StartCoroutine(AfterStart(0.1f));
	}

	private void FixedUpdate()
	{
		if (finish.activeSelf)
		{
			finish.SetActive(false);
			StartCoroutine(_part_6_end(0.5f));
		}
		if (timer > 0f)
		{
			timer -= Time.deltaTime;
		}
		if (_cutScenePart != CutScenePart.Part_5 && _cutScenePart != CutScenePart.Part_6)
		{
			return;
		}
		tempScan -= Time.deltaTime;
		if (!(tempScan <= 0f))
		{
			return;
		}
		if (Physics.Raycast(_eliController.transform.position + Vector3.up * 40f, -Vector3.up, 10f, RoofScan))
		{
			if (TowerRoof.activeSelf)
			{
				TowerRoof.SetActive(false);
			}
			tempScan = 0.25f;
		}
		else
		{
			if (!TowerRoof.activeSelf)
			{
				TowerRoof.SetActive(true);
			}
			tempScan = 0.25f;
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
			StartCoroutine(_part_0(0.5f));
			part_0 = false;
		}
		if (part_1)
		{
			StopAllCoroutines();
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
			StartCoroutine(_part_0(0.5f, 0f));
			part_4 = false;
		}
		if (part_5)
		{
			StopAllCoroutines();
			StartCoroutine(_part_0(0.5f, 0f));
			StartCoroutine(_part_6_message(0.6f));
			StartCoroutine(_part_End(4f));
			part_5 = false;
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
	}

	public void FindDoor()
	{
		count = 0;
		for (g = 0; g < guard.Length; g++)
		{
			if (guard[g] == null)
			{
				count++;
			}
		}
		AddGuardParent.SetActive(true);
		AddGuard2Parent.SetActive(true);
		if (count == guard.Length)
		{
			_cutScenePart = CutScenePart.Part_2;
			ClearPointStop = true;
			return;
		}
		gameController.PlayCutScene(true);
		CutSceneUse = true;
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		_cameraSmoothFollow.enabled = false;
		UICamera.GetComponent<Camera>().enabled = false;
		HUDPanel.SetActive(false);
		TrigerPart_0.SetActive(false);
		_eliController.MoveTarget.position = _eliController.transform.position;
		_eliController.AttackTarget = null;
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 0f), 1f, 0.5f, 2f);
		StartCoroutine(_part_1_start(0.5f));
		part_1 = true;
	}

	public void KillHim()
	{
		gameController.PlayCutScene(true);
		CutSceneUse = true;
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		_cameraSmoothFollow.enabled = false;
		UICamera.GetComponent<Camera>().enabled = false;
		HUDPanel.SetActive(false);
		TrigerPart_0.SetActive(false);
		TrigerPart_3.SetActive(false);
		_eliController.MoveTarget.position = _eliController.transform.position;
		_eliController.AttackTarget = null;
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 0f), 1f, 0.5f, 2f);
		StartCoroutine(_part_3_start(0.5f));
		part_3 = true;
	}

	public void On()
	{
		TowerRoof.SetActive(true);
	}

	public void Off()
	{
		TowerRoof.SetActive(false);
	}

	public void FirstFindSlug()
	{
		if (timer > 0f)
		{
			return;
		}
		timer = 10 + Random.Range(0, 3);
		if (_eliController.AttackTarget != null)
		{
			return;
		}
		gameController.PlayCutScene(true);
		CutSceneUse = true;
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		UICamera.GetComponent<Camera>().enabled = false;
		HUDPanel.SetActive(false);
		TrigerPart_0.SetActive(false);
		TrigerPart_3.SetActive(false);
		_firstFindTriger = true;
		_eliController.MoveTarget.position = _eliController.transform.position;
		_eliController.AttackTarget = null;
		count = 0;
		for (g = 0; g < AddGuard3.Length; g++)
		{
			if (AddGuard3[g] == null)
			{
				count++;
			}
		}
		if (count >= 6)
		{
			StartCoroutine(TextHint(0.1f, 4f, "icon_eli", text[6]));
		}
		else
		{
			StartCoroutine(TextHint(0.1f, 4f, "icon_eli", text[7]));
		}
		part_4 = true;
	}

	private void KillGuard()
	{
		gameController.PlayCutScene(true);
		CutSceneUse = true;
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		_cameraSmoothFollow.enabled = false;
		UICamera.GetComponent<Camera>().enabled = false;
		HUDPanel.SetActive(false);
		TrigerPart_0.SetActive(false);
		_eliController.MoveTarget.position = _eliController.transform.position;
		_eliController.AttackTarget = null;
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 0f), 1f, 0.5f, 2f);
		StartCoroutine(_part_2_start(0.5f));
		part_2 = true;
	}

	private IEnumerator _part_1_start(float t)
	{
		yield return new WaitForSeconds(t);
		MainCamera.GetComponent<Animation>().Play(AninationDoorView.name);
		StartCoroutine(TextHint(2f, AninationDoorView.length - 2f, "icon_eli", text[3]));
		_cutScenePart = CutScenePart.Part_2;
	}

	private IEnumerator _part_2_start(float t)
	{
		yield return new WaitForSeconds(t);
		StartCoroutine(AnimationPlay(MainCamera, 1f, GuardHelpView.name, 1f));
		StartCoroutine(AnimationPlay(TowerDoor, 1f, TowerDoorOpen.name, 2.5f));
		StartCoroutine(_part_2_goGuard(2.5f));
		if (!ClearPointStop && Vector3.Magnitude(_eliController.transform.position - WarningRadiusPivot.position) < 14f)
		{
			_eliController.transform.position = WarningRadiusPivot.position;
			_eliController.transform.eulerAngles = WarningRadiusPivot.eulerAngles;
			ClearPoint = WarningRadius[Random.Range(0, WarningRadius.Length)];
		}
		StartCoroutine(AnimationPlay(TowerGate, 1f, TowerGateOpen.name, 4.5f));
		StartCoroutine(AnimationPlay(TowerDoor, 1f, TowerDoorClose.name, 7f));
		StartCoroutine(AnimationPlay(TowerGate, 1f, TowerGateClose.name, 8f));
		StartCoroutine(TextHint(2.5f, GuardHelpView.length - 2f, "icon_sentinel", text[4]));
		yield return new WaitForSeconds(6.2f);
		if (!ClearPointStop && Vector3.Magnitude(_eliController.transform.position - WarningRadiusPivot.position) < 14f)
		{
			_eliController.MoveTarget.position = ClearPoint.position;
			_eliController.OnMove(6f);
		}
	}

	private IEnumerator _part_2_skip(float t)
	{
		yield return new WaitForSeconds(t);
		for (g = 0; g < AddGuard.Length; g++)
		{
			AddGuard[g].MoveTo(AddGuardPoint[g].position, 7f);
			AddGuard[g].transform.position = AddGuardPoint[g].position;
		}
		for (g = 0; g < AddGuard2.Length; g++)
		{
			AddGuard2[g].MoveTo(AddGuard[g].transform.parent.transform.position, 2.5f);
		}
		StartCoroutine(AnimationPlay(TowerDoor, 1f, TowerDoorClose.name));
		StartCoroutine(AnimationPlay(TowerGate, 1f, TowerGateClose.name));
		if (ClearPoint != null)
		{
			_eliController.transform.position = ClearPoint.position;
			_eliController.transform.eulerAngles = ClearPoint.eulerAngles;
		}
	}

	private IEnumerator _part_2_goGuard(float t)
	{
		yield return new WaitForSeconds(t);
		for (g = 0; g < AddGuard.Length; g++)
		{
			AddGuard[g].MoveTo(AddGuardPoint[g].position, 7f);
		}
		for (g = 0; g < AddGuard2.Length; g++)
		{
			AddGuard2[g].MoveTo(AddGuard[g].transform.parent.transform.position, 2.5f);
		}
	}

	private IEnumerator _part_3_start(float t)
	{
		yield return new WaitForSeconds(t);
		_eliController.transform.position = Faza_2.position;
		StartCoroutine(AnimationPlay(MainCamera, 0.55f, GuardHelpView.name, 1f));
		StartCoroutine(AnimationPlay(TowerDoor, 1f, TowerDoorOpen.name, 2f));
		StartCoroutine(_part_3_goGuard(2f));
		StartCoroutine(AnimationPlay(TowerDoor, 1f, TowerDoorClose.name, 5f));
		StartCoroutine(AnimationPlay(TowerGate, 1f, TowerGateClose.name, 6.5f));
		StartCoroutine(TextHint(3f, 4.5f, "icon_sentinel", text[5]));
		yield return new WaitForSeconds(3f);
		_eliController.MoveTarget.position = CoverPoint2.position;
		_eliController.OnMove(6f);
	}

	private IEnumerator _part_3_skip(float t)
	{
		yield return new WaitForSeconds(t);
		AddGuardPoint[0].transform.parent.transform.position = Faza_2.position;
		for (g = 0; g < AddGuard2.Length; g++)
		{
			AddGuard2[g].MoveTo(AddGuardPoint[g].position, 7f);
			AddGuard2[g].transform.position = AddGuardPoint[g].position;
		}
		StartCoroutine(AnimationPlay(TowerDoor, 1f, TowerDoorClose.name));
		StartCoroutine(AnimationPlay(TowerGate, 5f, TowerGateClose.name));
		_eliController.transform.position = CoverPoint2.position;
		AssistantController _assistant = Object.FindObjectOfType<AssistantController>();
		if (_assistant != null && !_assistant.Model.IsDead())
		{
			_assistant.transform.position = _eliController.transform.position;
		}
	}

	private IEnumerator _part_3_goGuard(float t)
	{
		yield return new WaitForSeconds(t);
		AddGuardPoint[0].transform.parent.transform.position = Faza_2.position;
		for (g = 0; g < AddGuard2.Length; g++)
		{
			AddGuard2[g].MoveTo(AddGuardPoint[g].position, 7f);
		}
	}

	private IEnumerator _part_6_end(float t)
	{
		yield return new WaitForSeconds(t);
		_eliController.MoveTarget.position = _eliController.transform.position;
		_eliController.AttackTarget = null;
		if (_firstFindTriger)
		{
			StartCoroutine(TextHint(0f, 4f, "icon_eli", text[8]));
		}
		else
		{
			StartCoroutine(TextHint(0f, 4f, "icon_eli", text[9]));
		}
		StartCoroutine(_part_6_message(5f));
		StartCoroutine(_part_End(7.5f));
		part_5 = true;
	}

	private IEnumerator _part_6_message(float t)
	{
		yield return new WaitForSeconds(t);
		TrigerMessage.position = _eliController.transform.position;
	}

	private void Update()
	{
		if (_cutScenePart == CutScenePart.Part_2)
		{
			count = 0;
			for (g = 0; g < guard.Length; g++)
			{
				if (guard[g] == null)
				{
					count++;
				}
			}
			if (count > 1)
			{
				_cutScenePart = CutScenePart.Part_3;
				Part_2 = true;
			}
		}
		if (_cutScenePart == CutScenePart.Part_3)
		{
			if (!Part_3)
			{
				KillGuard();
				Part_3 = true;
			}
			count = 0;
			for (g = 0; g < AddGuard.Length; g++)
			{
				if (AddGuard[g] == null)
				{
					count++;
				}
			}
			for (g = 0; g < guard.Length; g++)
			{
				if (guard[g] == null)
				{
					count++;
				}
			}
			if (count >= 8)
			{
				_cutScenePart = CutScenePart.Part_4;
			}
		}
		if (_cutScenePart == CutScenePart.Part_4)
		{
			if (!Part_4)
			{
				StartCoroutine(AnimationPlay(TowerGate, 1f, TowerGateOpen.name, 1f));
				Part_4 = true;
			}
			count = 0;
			for (g = 0; g < AddGuard2.Length; g++)
			{
				if (AddGuard2[g] == null)
				{
					count++;
				}
			}
			if (count >= 6)
			{
				_cutScenePart = CutScenePart.Part_5;
			}
		}
		if (_cutScenePart == CutScenePart.Part_5)
		{
			if (!Part_5)
			{
				StartCoroutine(AnimationPlay(TowerDoor, 1f, TowerDoorOpen.name, 3f));
				Part_5 = true;
			}
			count = 0;
			for (g = 0; g < AddGuard3.Length; g++)
			{
				if (AddGuard3[g] == null)
				{
					count++;
				}
			}
			if (count >= 6)
			{
				_cutScenePart = CutScenePart.Part_6;
				if ((bool)FirstFindTriger)
				{
					FirstFindTriger.SetActive(false);
				}
				Scan = true;
			}
		}
		if (_cutScenePart == CutScenePart.Part_6 && Scan && Vector3.Distance(_eliController.transform.position, FinishTarget.position) < 9f)
		{
			finish.SetActive(true);
			Scan = false;
		}
	}
}

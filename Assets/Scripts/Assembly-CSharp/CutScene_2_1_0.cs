using System;
using System.Collections;
using System.Collections.Generic;
using AppsMinistry.Core;
using AppsMinistry.Core.Input;
using AppsMinistry.UI.Camera;
using DG.Tweening;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.Shops;
using Slugterra.Game.Units.Ally;
using Slugterra.Game.Units.Ally.Assistant;
using Slugterra.Scene;
using Slugterra.UI.Game;
using Slugterra.UI.Game.Camera;
using Slugterra.UI.Shops;
using UnityEngine;

public class CutScene_2_1_0 : SlugterraCutScene
{
	[SerializeField]
	private GameController gameController;

	[SerializeField]
	private TutorHelpInput Help;

	[SerializeField]
	private Transform ElyGoTo_AB_0;

	[SerializeField]
	private Transform AllyGoTo_AB_0;

	[SerializeField]
	private GameObject Obstacle;

	[SerializeField]
	private GameObject startBlackPlane;

	private GameObject MainCamera;

	private GameObject UICamera;

	private EliController _eliController;

	private AssistantController _assistantController;

	private CameraFade _cameraFade;

	private CameraSmoothFollow _cameraSmoothFollow;

	private Animation _camerAnimation;

	[SerializeField]
	private int AddGold;

	public Transform text_gold;

	private bool TutorialComplete;

	private int _goldAdd;

	private int _buyHealth;

	private int _buySpeed;

	private int _buyArmor;

	private int _buyKorm;

	private bool UseTutor;

	private bool WeitPauseOff;

	public GameObject PausePanel;

	private bool ShowPausePanel;

	public Transform ContentRoot;

	[SerializeField]
	private PauseController _shopGameView;

	private ConsumableItemController[] gameConsumableControllers;

	public tk2dUIScrollableArea ScrollableArea;

	private bool UseScroll;

	private bool LockEli;

	public float[] PointforBuy;

	public Vector3 CurrentPointforBuy;

	private float weitTime = 0.8f;

	private float timer;

	private Vector3 offset = Vector3.zero;

	private Transform Tempbtn;

	public GameObject SlugPanel;

	public GameObject PopupConfirm;

	public GameObject InAppStore;

	public GameObject Map;

	private List<ConsumableItem> items = new List<ConsumableItem>();

	private void OnEnable()
	{
		_buyHealth = PlayerPrefs.GetInt("buyHealth_2_1");
		_buySpeed = PlayerPrefs.GetInt("buySpeed_2_1");
		_buyArmor = PlayerPrefs.GetInt("buyArmor_2_1");
		_buyKorm = PlayerPrefs.GetInt("buyKorm_2_1");
		UseTutor = true;
		if (_buyHealth == 1 && _buySpeed == 1 && _buyArmor == 1 && _buyKorm == 1)
		{
			TutorialComplete = true;
			UseTutor = false;
		}
		if (UseTutor)
		{
			PauseController shopGameView = _shopGameView;
			shopGameView.listAlreadyFilled = (Action<List<ConsumableItemController>>)Delegate.Combine(shopGameView.listAlreadyFilled, new Action<List<ConsumableItemController>>(StartTutor));
		}
	}

	private void OnDisable()
	{
		if (UseTutor)
		{
			PauseController shopGameView = _shopGameView;
			shopGameView.listAlreadyFilled = (Action<List<ConsumableItemController>>)Delegate.Remove(shopGameView.listAlreadyFilled, new Action<List<ConsumableItemController>>(StartTutor));
		}
		if (DonDestroyMonoSingleton<InputManager>.IsAlive)
		{
			DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
		}
	}

	private void Start()
	{
		RisePlayCutSceneEvent(true);
		MainCamera = GameObject.Find("MainCamera");
		UICamera = GameObject.Find("UICamera");
		_camerAnimation = MainCamera.GetComponent<Animation>();
		_cameraFade = MainCamera.GetComponent<CameraFade>();
		_cameraSmoothFollow = MainCamera.GetComponent<CameraSmoothFollow>();
		_eliController = UnityEngine.Object.FindObjectOfType<EliController>();
		_assistantController = UnityEngine.Object.FindObjectOfType<AssistantController>();
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 1f), 2f, 0.1f, 2f);
		_cutSceneView.SkipButton.targetObject = gameObject;
		gameController.PlayCutScene(true);
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		AddGold = (int)MonoSingleton<GameConfiguration>.Instance.ConsumableConfigurationManager.GetConsumablePrice(ConsumableType.First, 5) + (int)MonoSingleton<GameConfiguration>.Instance.ConsumableConfigurationManager.GetConsumablePrice(ConsumableType.ArmorBoost, 5) + (int)MonoSingleton<GameConfiguration>.Instance.ConsumableConfigurationManager.GetConsumablePrice(ConsumableType.SlugFood, 5) + (int)MonoSingleton<GameConfiguration>.Instance.ConsumableConfigurationManager.GetConsumablePrice(ConsumableType.SpeedBoost, 5);
		if (!UseTutor)
		{
			float a = 6f;
			StartCoroutine(TextHint_0(a));
		}
		else
		{
			StartCoroutine(Pause(5f));
			timer = weitTime;
			_goldAdd = PlayerPrefs.GetInt("bonusGold_2_1");
		}
		float a2 = 1f;
		StartCoroutine(StartCameraTrack(a2));
	}

	private void StartTutor(List<ConsumableItemController> items)
	{
		gameConsumableControllers = items.ToArray();
		if (UseTutor)
		{
			for (byte b = 0; b < gameConsumableControllers.Length; b++)
			{
				gameConsumableControllers[b].View.SmallPackButtonBtnBuy.targetObject = gameObject;
				gameConsumableControllers[b].View.SmallPackButtonBtnBuy.enabled = false;
				gameConsumableControllers[b].transform.Find("btnInfo").GetComponent<tk2dUIItem>().enabled = false;
			}
			if (_buyHealth == 1)
			{
				gameConsumableControllers[0].View.SmallPackButtonBtnBuy.enabled = false;
				gameConsumableControllers[0].View.SmallPackButtonBtnBuy.targetObject = gameConsumableControllers[0].View.SmallPackButtonBtnBuy.transform.parent.parent.parent.parent.parent.gameObject;
			}
			if (_buySpeed == 1)
			{
				gameConsumableControllers[1].View.SmallPackButtonBtnBuy.enabled = false;
				gameConsumableControllers[1].View.SmallPackButtonBtnBuy.targetObject = gameConsumableControllers[1].View.SmallPackButtonBtnBuy.transform.parent.parent.parent.parent.parent.gameObject;
			}
			if (_buyArmor == 1)
			{
				gameConsumableControllers[2].View.SmallPackButtonBtnBuy.enabled = false;
				gameConsumableControllers[2].View.SmallPackButtonBtnBuy.targetObject = gameConsumableControllers[2].View.SmallPackButtonBtnBuy.transform.parent.parent.parent.parent.parent.gameObject;
			}
		}
	}

	private IEnumerator Pause(float a)
	{
		yield return new WaitForSeconds(a);
		float t = 1f;
		while (t > 0f)
		{
			t -= Time.deltaTime;
			_camerAnimation["CameraTrack_1"].speed = t;
			yield return new WaitForSeconds(Time.deltaTime);
			if (t <= 0f)
			{
				t = 0f;
				_camerAnimation["CameraTrack_1"].speed = 0f;
			}
		}
		LockEli = true;
		SlugPanel.SetActive(false);
		UICamera.SetActive(true);
		gameController.PlayCutScene(false);
		yield return new WaitForSeconds(1f);
		StartCoroutine(SetActiveHelpTrue("pause", null, Vector3.zero, 0.4f));
	}

	private IEnumerator PauseOff()
	{
		float t = 0f;
		while (t < 1f)
		{
			t += Time.deltaTime;
			_camerAnimation["CameraTrack_1"].speed = t;
			yield return new WaitForSeconds(Time.deltaTime);
			if (t >= 1f)
			{
				t = 1f;
				_camerAnimation["CameraTrack_1"].speed = 1f;
			}
		}
	}

	private IEnumerator AfterCutScene(float t)
	{
		yield return new WaitForSeconds(t);
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
	}

	private IEnumerator MoveLocalX(Transform obj, float NewPosX, float speed)
	{
		Vector3 tempPos = obj.localPosition;
		tempPos.x = NewPosX;
		while (Vector3.Distance(tempPos, obj.localPosition) > 0.01f)
		{
			yield return new WaitForSeconds(Time.smoothDeltaTime);
			obj.localPosition = Vector3.MoveTowards(obj.localPosition, tempPos, speed * Time.smoothDeltaTime);
		}
		ScrollableArea.allowSwipeScrolling = true;
		ScrollableArea.allowScrollWheel = true;
	}

	private void LateUpdate()
	{
		if (!UseTutor)
		{
			return;
		}
		if (!TutorialComplete)
		{
			if (PausePanel.activeSelf && Time.timeScale == 0f && !ShowPausePanel)
			{
				ShowPausePanel = true;
				ScrollableArea.allowSwipeScrolling = false;
				ScrollableArea.allowScrollWheel = false;
				StartCoroutine(AddMegaGoldPack(0.5f));
				StartCoroutine(SetActiveHelpFalse("pause", 0f));
				Time.timeScale = 1f;
			}
			if (ShowPausePanel && !PausePanel.activeSelf)
			{
				ShowPausePanel = false;
				if (!WeitPauseOff)
				{
					StartCoroutine(SetActiveHelpTrue("pause", null, Vector3.zero, 0.6f));
				}
				StartCoroutine(AfterCutScene(0f));
				StartCoroutine(SetActiveHelpFalse("pause", 0f));
				timer = weitTime;
			}
			if (ShowPausePanel)
			{
				if (timer > 0f)
				{
					timer -= Time.deltaTime;
					if (timer <= 0f)
					{
						timer = 0f;
					}
				}
				if (timer <= 0f && !WeitPauseOff)
				{
					for (byte b = 0; b < gameConsumableControllers.Length; b++)
					{
						gameConsumableControllers[b].View.SmallPackButtonBtnBuy.enabled = false;
						Tempbtn = null;
					}
					if (_buyHealth == 0)
					{
						CurrentPointforBuy = ContentRoot.localPosition;
						gameConsumableControllers[0].View.SmallPackButtonBtnBuy.enabled = true;
						CurrentPointforBuy.x = PointforBuy[0];
						Tempbtn = gameConsumableControllers[0].View.SmallPackButtonBtnBuy.transform;
					}
					if (_buyHealth == 1 && _buySpeed == 0)
					{
						CurrentPointforBuy = ContentRoot.localPosition;
						gameConsumableControllers[1].View.SmallPackButtonBtnBuy.enabled = true;
						CurrentPointforBuy.x = PointforBuy[1];
						Tempbtn = gameConsumableControllers[1].View.SmallPackButtonBtnBuy.transform;
					}
					if (_buySpeed == 1 && _buyArmor == 0)
					{
						CurrentPointforBuy = ContentRoot.localPosition;
						gameConsumableControllers[2].View.SmallPackButtonBtnBuy.enabled = true;
						CurrentPointforBuy.x = PointforBuy[2];
						Tempbtn = gameConsumableControllers[2].View.SmallPackButtonBtnBuy.transform;
					}
					if (_buyArmor == 1 && _buyKorm == 0)
					{
						CurrentPointforBuy = ContentRoot.localPosition;
						gameConsumableControllers[3].View.SmallPackButtonBtnBuy.enabled = true;
						CurrentPointforBuy.x = PointforBuy[3];
						Tempbtn = gameConsumableControllers[3].View.SmallPackButtonBtnBuy.transform;
					}
					if (!UseScroll)
					{
						ContentRoot.localPosition = Vector3.MoveTowards(ContentRoot.localPosition, CurrentPointforBuy, Time.deltaTime * 8f);
					}
					if (!WeitPauseOff)
					{
						if (Vector3.Distance(ContentRoot.transform.localPosition, CurrentPointforBuy) < 0.3f)
						{
							Help.ShowHelp("buy", null, Vector3.zero);
						}
						else
						{
							Help.DisableHelp("buy");
						}
					}
				}
				if (!TutorialComplete && _buyHealth == 1 && _buySpeed == 1 && _buyArmor == 1 && _buyKorm == 1 && !WeitPauseOff)
				{
					Tempbtn = null;
					Help.DisableHelp("buy");
					Help.DisableHelp("pause");
					Help.ShowHelp("resume", null, Vector3.zero);
					ScrollableArea.allowSwipeScrolling = true;
					ScrollableArea.allowScrollWheel = true;
					for (byte b2 = 0; b2 < gameConsumableControllers.Length; b2++)
					{
						gameConsumableControllers[b2].View.SmallPackButtonBtnBuy.enabled = true;
						gameConsumableControllers[b2].transform.Find("btnInfo").GetComponent<tk2dUIItem>().enabled = true;
					}
					WeitPauseOff = true;
				}
			}
			if (WeitPauseOff && !TutorialComplete && !ShowPausePanel)
			{
				SlugPanel.SetActive(true);
				TutorialComplete = true;
				float a = 1f;
				StartCoroutine(TextHint_0(a));
				UICamera.SetActive(false);
				StartCoroutine(AfterCutScene(0f));
				gameController.PlayCutScene(true);
				Help.DisableHelp("resume");
				StartCoroutine(PauseOff());
				WeitPauseOff = false;
				LockEli = false;
			}
			if (LockEli && _eliController.MoveTarget.position != _eliController.transform.position)
			{
				_eliController.MoveTarget.position = _eliController.transform.position;
				DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
			}
		}
		if (PopupConfirm.activeSelf || InAppStore.activeSelf || Map.activeSelf)
		{
			Help.HideLayer();
		}
		else
		{
			Help.ShowLayer();
		}
	}

	private IEnumerator AddMegaGoldPack(float weit)
	{
		yield return new WaitForSeconds(weit);
		if (_goldAdd == 0)
		{
			StartCoroutine(ScaleUp(text_gold, 1.8f, 0f));
			StartCoroutine(ScaleDown(text_gold, 1f, 0.75f));
			SetGold();
		}
	}

	private void SetGold()
	{
		MonoSingleton<AppController>.Instance.Model.Wallet.Gold += AddGold;
		_goldAdd = 1;
		PlayerPrefs.SetInt("bonusGold_2_1", 1);
	}

	private IEnumerator ScaleUp(Transform obj, float scale, float weit)
	{
		yield return new WaitForSeconds(weit);
		obj.DOScale(Vector3.one * scale, 0.25f);
	}

	private IEnumerator ScaleDown(Transform obj, float scale, float weit)
	{
		yield return new WaitForSeconds(weit);
		obj.DOScale(Vector3.one * scale, 0.25f);
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

	public void OnBtnBuyClick()
	{
		if (_buyHealth == 0)
		{
			gameConsumableControllers[0].transform.SendMessage("OnBtnBuyClick");
			_buyHealth = 1;
			PlayerPrefs.SetInt("buyHealth_2_1", 1);
			gameConsumableControllers[0].View.SmallPackButtonBtnBuy.enabled = false;
			gameConsumableControllers[0].View.SmallPackButtonBtnBuy.targetObject = gameConsumableControllers[0].View.SmallPackButtonBtnBuy.transform.parent.parent.parent.parent.parent.gameObject;
		}
		else if (_buyHealth == 1 && _buySpeed == 0)
		{
			gameConsumableControllers[1].transform.SendMessage("OnBtnBuyClick");
			_buySpeed = 1;
			PlayerPrefs.SetInt("buySpeed_2_1", 1);
			gameConsumableControllers[1].View.SmallPackButtonBtnBuy.enabled = false;
			gameConsumableControllers[1].View.SmallPackButtonBtnBuy.targetObject = gameConsumableControllers[1].View.SmallPackButtonBtnBuy.transform.parent.parent.parent.parent.parent.gameObject;
		}
		else if (_buySpeed == 1 && _buyArmor == 0)
		{
			gameConsumableControllers[2].transform.SendMessage("OnBtnBuyClick");
			_buyArmor = 1;
			PlayerPrefs.SetInt("buyArmor_2_1", 1);
			gameConsumableControllers[2].View.SmallPackButtonBtnBuy.enabled = false;
			gameConsumableControllers[2].View.SmallPackButtonBtnBuy.targetObject = gameConsumableControllers[2].View.SmallPackButtonBtnBuy.transform.parent.parent.parent.parent.parent.gameObject;
		}
		else if (_buyArmor == 1 && _buyKorm == 0)
		{
			gameConsumableControllers[3].transform.SendMessage("OnBtnBuyClick");
			_buyKorm = 1;
			PlayerPrefs.SetInt("buyKorm_2_1", 1);
			gameConsumableControllers[3].View.SmallPackButtonBtnBuy.enabled = false;
			gameConsumableControllers[3].View.SmallPackButtonBtnBuy.targetObject = gameConsumableControllers[3].View.SmallPackButtonBtnBuy.transform.parent.parent.parent.parent.parent.gameObject;
			UseScroll = true;
		}
	}

	private IEnumerator StartCameraTrack(float a)
	{
		yield return new WaitForSeconds(a);
		UICamera.SetActive(false);
		_cameraSmoothFollow.enabled = false;
		_camerAnimation.Play("CameraTrack_1");
		ElyGoToOnStart();
		startBlackPlane.SetActive(false);
	}

	private IEnumerator DelaySkip(float a)
	{
		yield return new WaitForSeconds(a);
		Skip();
	}

	private void Skip()
	{
		_camerAnimation.Stop();
		_cameraSmoothFollow.enabled = true;
		UICamera.SetActive(true);
		Help.ShowHelp("selectslug", null, Vector3.zero);
		ShowScreens(false);
		_eliController.SetAttackTarget(null);
		_assistantController.SetAttackTarget(null);
		DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
		gameController.PlayCutScene(false);
		RisePlayCutSceneEvent(false);
		gameObject.SetActive(false);
	}

	private IEnumerator TextHint_0(float a)
	{
		yield return new WaitForSeconds(a);
		_cutSceneView.SpeakerIcon = "icon_pronto";
		ShowScreens(true);
		_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["2_1_cutscene_1_text_01"];
		float time = 4f;
		StartCoroutine(HideTextHint_0(time));
		_isSkipEnable = true;
		_cutSceneView.SkipButton.gameObject.SetActive(true);
	}

	private IEnumerator HideTextHint_0(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			_eliController.SetAttackTarget(Obstacle.transform);
			float a2 = 4f;
			StartCoroutine(StopShooting(a2));
		}
	}

	private IEnumerator StopShooting(float a)
	{
		yield return new WaitForSeconds(a);
		_eliController.SetAttackTarget(null);
		_assistantController.SetAttackTarget(null);
		float time = 1f;
		StartCoroutine(TextHint_1(time));
	}

	private IEnumerator TextHint_1(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["2_1_cutscene_1_text_02"];
			float a2 = 4f;
			StartCoroutine(HideTextHint_1(a2));
		}
	}

	private IEnumerator HideTextHint_1(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			ShowScreens(false);
			Fade();
		}
	}

	private void Fade()
	{
		_isSkipEnable = false;
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 1f), 1f, 1f, 1f);
		_cutSceneView.SkipButton.gameObject.SetActive(false);
		_cutSceneView.SkipButton.targetObject = null;
		float a = 1f;
		StartCoroutine(DelaySkip(a));
	}

	private void ElyGoToOnStart()
	{
		_eliController.MoveTarget.position = ElyGoTo_AB_0.transform.position;
		_eliController.OnMove(5f);
		float a = 5f;
		StartCoroutine(RotateEly(a));
		if (_assistantController != null && !(_assistantController.StateMachine.CurrentState is DeathState))
		{
			_assistantController.MoveTarget.position = AllyGoTo_AB_0.transform.position;
			_assistantController.OnMove(7f);
			float a2 = 4.5f;
			StartCoroutine(RotateAlly(a2));
		}
	}

	private IEnumerator RotateEly(float a)
	{
		yield return new WaitForSeconds(a);
		_eliController.transform.eulerAngles = ElyGoTo_AB_0.eulerAngles;
	}

	private IEnumerator RotateAlly(float a)
	{
		yield return new WaitForSeconds(a);
		_assistantController.transform.eulerAngles = AllyGoTo_AB_0.eulerAngles;
	}
}

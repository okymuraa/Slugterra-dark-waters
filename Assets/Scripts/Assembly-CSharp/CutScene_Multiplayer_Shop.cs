using System;
using System.Collections;
using AppsMinistry.Core;
using DG.Tweening;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Scene;
using Slugterra.Scene.Multiplayer;
using Slugterra.UI.Shops;
using UnityEngine;

public class CutScene_Multiplayer_Shop : MonoBehaviour
{
	private bool TutorialMegaShopSceneComplete;

	private int _goldAdd;

	private bool _goldadd;

	public Transform btnSlugMessage;

	public Transform btnArmorMessage;

	public Transform btnProntoMessage;

	public Transform ScrollView;

	public GameObject MultiplayerLobby;

	public GameObject EquipStore;

	public GameObject AppStore;

	public GameObject Gifts;

	public GameObject BuyUpgradeRecovery;

	public GameObject BuyUpgrade;

	[SerializeField]
	private int AddGold;

	public Transform text_gold;

	public Transform text_gold2;

	public Transform[] ConsoleLeft;

	public Transform[] BtnLeft;

	public Transform[] ConsoleRight;

	public GameObject[] Arrows;

	public ShopItemsScrollController Scroll;

	public tk2dUIItem btnOpenAppStore;

	public tk2dUIItem btnExitAppStore;

	public tk2dUIItem btnMoneyAppStore;

	public tk2dUIItem btnPodpiskaAppStore;

	private bool btnPodpiskaClick;

	public tk2dUIItem btnSlugBuy;

	public tk2dUIItem btnSlugUpgrate;

	public tk2dUIItem btnArmorBuy;

	public tk2dUIItem btnArmorUpgrate;

	public tk2dUIItem btnProntoBuy;

	public tk2dUIItem btnProntoUpgrate;

	public GameObject EquipPanel;

	public tk2dUIItem btnSlugEquip;

	private GameObject TargetSlugEquip;

	public tk2dUIItem btnArmorEquip;

	private GameObject TargetArmorEquip;

	public tk2dUIItem btnProntoEquip;

	private GameObject TargetProntoEquip;

	public tk2dButton btnSlugKolb1;

	public tk2dButton btnSlugKolb2;

	public tk2dButton btnSlugKolb3;

	private tk2dUIItem btnSlugPanel;

	private tk2dUIItem btnArmorPanel;

	private tk2dUIItem btnProntoPanel;

	private tk2dUIItem btnEliPanel;

	public GameObject Reklama;

	private bool SlugTap;

	private bool ArmorTap;

	private bool ProntoTap;

	private bool EliTap;

	private tk2dButton[] btnMission;

	private tk2dUIItem[] btnUIItem;

	public GameObject hand2dFlip;

	public Vector3 offsetflip;

	private tk2dButton btmCurrent;

	public GameObject hand2d;

	public Vector3 offset;

	private int _buyAppStore;

	private int _ShowAppStore;

	public Transform AppMoneyRoot;

	private InAppItemController[] contentAppMoney;

	public Transform AppPodpiskaRoot;

	private SubscriptionItemController[] contentAppPodpiska;

	private tk2dUIScrollableArea AppMoneyScrollable;

	private tk2dUIScrollableArea AppPodpiskaScrollable;

	public float minY = 0.8f;

	public float maxY = 0.8f;

	private bool SlugPanelView;

	private Transform Arahnet;

	private int _buyArahnet;

	private int _ArahnetEquip;

	private Transform Geoshard;

	private int _buyGeoshard;

	private int _GeoshardEquip;

	private bool ArmorPanelView;

	private Transform ArmorLite;

	private int _buyArmorLite;

	private int _ArmorLiteEquip;

	private bool ProntoPanelView;

	private Transform Pronto;

	private int _buyPronto = 1;

	private int _ProntoEquip;

	private float startZLeft;

	private float startZAppStore;

	private float startZRight;

	public Renderer[] shadow;

	public Renderer[] shadow2;

	public float shadowSpeed = 0.25f;

	private Color black;

	private Color black2;

	private int indexShadow;

	private int indexShadow_2;

	private bool shadowFadeUp;

	private bool shadowFadeDown;

	private int s;

	private int i;

	private bool _startReady;

	public Transform[] HideElements;

	private bool TutorialMapSceneComplete3;

	private void Awake()
	{
		_goldAdd = PlayerPrefs.GetInt("bonusGoldMultiplayer");
		Debug.Log("CutScene_Multiplayer_Shop(ADD GOLD) = " + _goldAdd);
		if (PlayerPrefs.GetInt("Tutor_MultiplayerGiftTicket") == 0)
		{
			GiftTicket();
		}
	}

	private void Start()
	{
		if (text_gold == null)
		{
			text_gold = GameObject.Find("lblCountCoins").transform;
		}
		TutorialMapSceneComplete3 = true;
		if (PlayerPrefs.GetInt("Tutor_Multiplayer") == 0)
		{
			TutorialMapSceneComplete3 = false;
		}
		if (!TutorialMapSceneComplete3)
		{
			AfterStart3();
		}
	}

	private void AfterStart3()
	{
		btnMission = UnityEngine.Object.FindObjectsOfType<tk2dButton>();
		for (i = 0; i < btnMission.Length; i++)
		{
			if (btnMission[i].transform.parent.name == "btnTutor")
			{
				btmCurrent = btnMission[i];
				offset.x = 1.9f;
				offset.y = 0.5f;
				offset.z = -0.1f;
				StartCoroutine(ShowHand(btmCurrent.transform, offset, Vector3.forward * 96f, 0.5f));
			}
			else
			{
				btnMission[i].enabled = false;
			}
		}
		for (i = 0; i < btnMission.Length; i++)
		{
			if ((btnMission[i].targetObject != null && btnMission[i].targetObject.name == "PopupDailyBonus") || (btnMission[i].targetObject != null && btnMission[i].targetObject.name == "PopupConfirm") || (btnMission[i].targetObject != null && btnMission[i].targetObject.name == "PopupAdFeedback") || (btnMission[i].targetObject != null && btnMission[i].targetObject.name == "PopupFacebookGoals") || btnMission[i].transform.parent.name == "btnBack")
			{
				btnMission[i].enabled = true;
			}
		}
	}

	private IEnumerator AddMegaGoldPack(float weit)
	{
		yield return new WaitForSeconds(weit);
		if (_goldAdd == 0)
		{
			StartCoroutine(ScaleUp(text_gold, 3f, 0f));
			StartCoroutine(ScaleDown(text_gold, 1f, 0.75f));
			StartCoroutine(ScaleUp(text_gold2, 3f, 0f));
			StartCoroutine(ScaleDown(text_gold2, 1f, 0.75f));
			SetGold();
		}
	}

	private void SetGold()
	{
		MonoSingleton<AppController>.Instance.Model.Wallet.Gold += AddGold;
		_goldAdd = 1;
		PlayerPrefs.SetInt("bonusGoldMultiplayer", 1);
	}

	private void GiftTicket()
	{
		MonoSingleton<AppController>.Instance.Model.Wallet.OnGetTicketRewardSuccessEvent();
		PlayerPrefs.SetInt("Tutor_MultiplayerGiftTicket", 1);
	}

	private IEnumerator ShowHand(Transform obj, Vector3 offset, Vector3 rotation, float weit)
	{
		yield return new WaitForSeconds(weit);
		hand2d.transform.position = obj.position + offset;
		hand2d.transform.eulerAngles = rotation;
		hand2d.SetActive(true);
	}

	private void MoveHand(Transform obj, Vector3 offset, Vector3 rotation)
	{
		hand2d.transform.position = obj.position + offset;
		hand2d.transform.eulerAngles = rotation;
	}

	private IEnumerator ShowHandFlip(Transform obj, Vector3 offset, Vector3 rotation, float weit)
	{
		yield return new WaitForSeconds(weit);
		hand2dFlip.transform.position = obj.position + offset;
		hand2dFlip.transform.eulerAngles = rotation;
		hand2dFlip.SetActive(true);
	}

	private void MoveHandFlip(Transform obj, Vector3 offset, Vector3 rotation)
	{
		hand2dFlip.transform.position = obj.position + offset;
		hand2dFlip.transform.eulerAngles = rotation;
	}

	private IEnumerator HideHand(float weit)
	{
		yield return new WaitForSeconds(weit);
		hand2d.SetActive(false);
		hand2dFlip.SetActive(false);
	}

	private IEnumerator ScaleUp(Transform obj, float scale, float weit)
	{
		yield return new WaitForSeconds(weit);
		if (obj != null)
		{
			obj.DOScale(Vector3.one * scale, 0.25f);
		}
	}

	private IEnumerator ScaleDown(Transform obj, float scale, float weit)
	{
		yield return new WaitForSeconds(weit);
		if (obj != null)
		{
			obj.DOScale(Vector3.one * scale, 0.25f);
		}
	}

	public void OnTutor()
	{
		SceneLoader.LoadScene<TutorialMultiplayer>(true);
	}

	public void Temp()
	{
	}

	public void Tap(Type tabType)
	{
		if (ArmorTap && tabType == typeof(ArmorsTabController))
		{
			SelectArmorPanel();
		}
		if (EliTap && tabType == typeof(EliPreviewTabController))
		{
			SelectEliPanel();
		}
		if (SlugTap && tabType == typeof(SlugsTabController))
		{
			SelectSlugPanel();
		}
		if (ProntoTap && tabType == typeof(AssistantsTabController))
		{
			SelectProntoPanel();
		}
	}

	public void OnEquip()
	{
		if (SlugPanelView)
		{
			TargetSlugEquip.SendMessage("OnEquip");
			if (MonoSingleton<AppController>.Instance.Model.MissionSetting.DefenceSlug.Type == SlugType.Arachnet)
			{
				_ArahnetEquip = 1;
			}
			if (MonoSingleton<AppController>.Instance.Model.MissionSetting.SpecialSlug.Type == SlugType.Geoshard)
			{
				_GeoshardEquip = 1;
			}
			hand2d.SetActive(false);
			hand2dFlip.SetActive(false);
			btnSlugPanel.enabled = false;
			btnSlugPanel.transform.GetComponent<tk2dUIItem>().enabled = false;
			if (_ArahnetEquip == 1 && _GeoshardEquip == 1)
			{
				if (_ArmorLiteEquip == 0 || _buyArmorLite == 0)
				{
					StartCoroutine(Part_1_0(0.55f));
					return;
				}
				if (_ProntoEquip == 0 || _buyPronto == 0)
				{
					StartCoroutine(Part_2_0(0.55f));
					return;
				}
				StartCoroutine(Part_3_0(0.55f));
			}
		}
		if (ArmorPanelView)
		{
			TargetArmorEquip.SendMessage("OnEquip");
			if (_ArmorLiteEquip == 0)
			{
				_ArmorLiteEquip = 1;
			}
			hand2d.SetActive(false);
			hand2dFlip.SetActive(false);
			btnArmorPanel.enabled = false;
			btnArmorPanel.transform.GetComponent<tk2dUIItem>().enabled = false;
			if (_ProntoEquip == 0 || _buyPronto == 0)
			{
				StartCoroutine(Part_2_0(0.55f));
				return;
			}
			StartCoroutine(Part_3_0(0.55f));
		}
		if (ProntoPanelView)
		{
			TargetProntoEquip.SendMessage("OnEquip");
			if (_ProntoEquip == 0)
			{
				_ProntoEquip = 1;
			}
			hand2d.SetActive(false);
			hand2dFlip.SetActive(false);
			btnProntoPanel.enabled = false;
			StartCoroutine(Part_3_0(0.55f));
		}
	}

	private IEnumerator Part_OpenApp(float weit)
	{
		for (i = 0; i < ConsoleLeft.Length; i++)
		{
			ConsoleLeft[i].position += Vector3.forward * 3f;
		}
		for (i = 0; i < ConsoleRight.Length; i++)
		{
			ConsoleRight[i].position -= Vector3.forward * 2f;
		}
		yield return new WaitForSeconds(weit);
		btnOpenAppStore.enabled = true;
		btnExitAppStore.transform.gameObject.SetActive(false);
		offset.x = -0.964f;
		offset.y = -0.802f;
		offset.z = 0f;
		StartCoroutine(ShowHandFlip(btnOpenAppStore.transform, offset, Vector3.forward * 149f, 0f));
	}

	private IEnumerator Part_ViewMoneyApp(float weit)
	{
		for (i = 0; i < ConsoleLeft.Length; i++)
		{
			ConsoleLeft[i].position -= Vector3.forward * 1.5f;
		}
		for (i = 0; i < ConsoleRight.Length; i++)
		{
			ConsoleRight[i].position += Vector3.forward * 2f;
		}
		yield return new WaitForSeconds(weit);
		contentAppMoney = AppMoneyRoot.GetComponentsInChildren<InAppItemController>();
		AppMoneyRoot.DOLocalMoveY(3.2f, 1.5f);
		for (i = 0; i < contentAppMoney.Length; i++)
		{
			contentAppMoney[i].transform.DOScale(1.2f, 0.18f);
			yield return new WaitForSeconds(0.18f);
			contentAppMoney[i].transform.DOScale(1f, 0.4f);
		}
		AppMoneyScrollable.allowSwipeScrolling = true;
		yield return new WaitForSeconds(0.5f);
		offset.x = -2.21f;
		offset.y = -0.68f;
		offset.z = 0f;
		StartCoroutine(ShowHandFlip(btnPodpiskaAppStore.transform, offset, Vector3.forward * 150f, 0f));
		btnPodpiskaAppStore.enabled = true;
		btnPodpiskaClick = true;
	}

	private IEnumerator Part_ViewPodpiskaApp(float weit)
	{
		bool fix = true;
		yield return new WaitForSeconds(weit);
		if (fix)
		{
			contentAppPodpiska = AppPodpiskaRoot.GetComponentsInChildren<SubscriptionItemController>();
			for (i = 0; i < contentAppPodpiska.Length; i++)
			{
				contentAppPodpiska[i].transform.DOScaleX(1.2f, 0.24f);
				yield return new WaitForSeconds(0.24f);
				contentAppPodpiska[i].transform.DOScale(1f, 0.4f);
			}
			yield return new WaitForSeconds(0.5f);
		}
		else
		{
			AppPodpiskaRoot.DOLocalMoveY(8.32f, 2.5f);
			yield return new WaitForSeconds(2.5f);
		}
		AppPodpiskaScrollable.allowSwipeScrolling = true;
		offset.x = 0.53f;
		offset.y = -1.05f;
		offset.z = 0f;
		btnPodpiskaAppStore.enabled = true;
		btnExitAppStore.transform.gameObject.SetActive(true);
		btnExitAppStore.transform.DOScale(1f, 0.2f);
		btnExitAppStore.enabled = true;
	}

	private IEnumerator Part_CloseApp(float weit)
	{
		yield return new WaitForSeconds(weit);
		btnOpenAppStore.enabled = true;
		yield return new WaitForSeconds(1f);
		Scroll.enabled = true;
	}

	private IEnumerator Part_0_0(float weit)
	{
		yield return new WaitForSeconds(weit);
		if (btnSlugMessage.gameObject.activeSelf)
		{
			StartCoroutine(ScaleUp(btnSlugMessage, 2f, 0f));
			StartCoroutine(ScaleDown(btnSlugMessage, 1f, 0.4f));
			StartCoroutine(ScaleUp(btnSlugMessage, 2f, 0.7f));
			StartCoroutine(ScaleDown(btnSlugMessage, 1f, 1.1f));
			StartCoroutine(ScaleUp(btnSlugMessage, 2f, 1.4f));
			StartCoroutine(ScaleDown(btnSlugMessage, 1f, 1.8f));
		}
		yield return new WaitForSeconds(0.5f);
		btnSlugPanel.enabled = true;
		SlugTap = true;
		btnSlugPanel.transform.GetComponent<tk2dUIItem>().enabled = true;
		offset.x = 1.555f;
		offset.y = -0.53978f;
		offset.z = 0f;
	}

	public void SelectSlugPanel()
	{
		StartCoroutine(HideHand(0f));
		SlugTap = false;
		for (i = 0; i < ConsoleRight.Length; i++)
		{
			ConsoleRight[i].position -= Vector3.forward * 3f;
		}
		indexShadow = 2;
		StartCoroutine(Part_0_0_1(0.5f));
	}

	private IEnumerator Part_0_0_1(float weit)
	{
		yield return new WaitForSeconds(weit);
		Arahnet = GameObject.Find("prfArachnet_proto(Clone)").transform;
		Geoshard = GameObject.Find("prfGeoshard_proto(Clone)").transform;
		SlugPanelView = true;
	}

	public void OnButtonClick_A()
	{
		_buyArahnet = 1;
		SlugPanelView = false;
		hand2d.SetActive(false);
		hand2dFlip.SetActive(false);
		BuyUpgradeRecovery.SendMessage("OnButtonClick");
		StartCoroutine(Part_0_0_2(0.33f));
		OnEquip();
	}

	private IEnumerator Part_0_0_2(float weit)
	{
		yield return new WaitForSeconds(weit);
		SlugPanelView = true;
		if (btnSlugEquip.gameObject.activeSelf)
		{
			offset.x = 0.65f;
			offset.y = -0.5f;
			offset.z = 0f;
		}
		else
		{
			_ArahnetEquip = 1;
			PlayerPrefs.SetInt("arahnetEquip", 1);
		}
	}

	public void OnButtonClick_G()
	{
		hand2dFlip.SetActive(false);
		_buyGeoshard = 1;
		BuyUpgradeRecovery.SendMessage("OnButtonClick");
		OnEquip();
	}

	private IEnumerator Part_1_0(float weit)
	{
		yield return new WaitForSeconds(weit);
		if (btnArmorMessage.gameObject.activeSelf)
		{
			StartCoroutine(ScaleUp(btnArmorMessage, 2f, 0f));
			StartCoroutine(ScaleDown(btnArmorMessage, 1f, 0.4f));
			StartCoroutine(ScaleUp(btnArmorMessage, 2f, 0.7f));
			StartCoroutine(ScaleDown(btnArmorMessage, 1f, 1.1f));
			StartCoroutine(ScaleUp(btnArmorMessage, 2f, 1.4f));
			StartCoroutine(ScaleDown(btnArmorMessage, 1f, 1.8f));
		}
		yield return new WaitForSeconds(0.5f);
		btnArmorPanel.enabled = true;
		ArmorTap = true;
		btnArmorPanel.transform.GetComponent<tk2dUIItem>().enabled = true;
		offset.x = 1.555f;
		offset.y = -0.53978f;
		offset.z = 0f;
	}

	public void SelectArmorPanel()
	{
		StartCoroutine(HideHand(0f));
		SlugPanelView = false;
		ProntoPanelView = false;
		ArmorTap = false;
		indexShadow = 2;
		StartCoroutine(Part_1_0_1(0.5f));
	}

	private IEnumerator Part_1_0_1(float weit)
	{
		yield return new WaitForSeconds(weit);
		ArmorLite = GameObject.Find("EliArmor_ShopModel(Clone)").transform;
		ArmorPanelView = true;
	}

	public void OnButtonClick_Armor()
	{
		_buyArmorLite = 1;
		ArmorPanelView = false;
		hand2d.SetActive(false);
		hand2dFlip.SetActive(false);
		BuyUpgrade.SendMessage("OnButtonClick");
		StartCoroutine(Part_1_0_2(0.33f));
	}

	private IEnumerator Part_1_0_2(float weit)
	{
		yield return new WaitForSeconds(weit);
		ArmorPanelView = true;
		if (btnArmorEquip.gameObject.activeSelf)
		{
			offset.x = 0.65f;
			offset.y = -0.5f;
			offset.z = 0f;
		}
		else
		{
			_ArmorLiteEquip = 1;
		}
	}

	private IEnumerator Part_2_0(float weit)
	{
		yield return new WaitForSeconds(weit);
		if (btnProntoMessage.gameObject.activeSelf)
		{
			StartCoroutine(ScaleUp(btnProntoMessage, 2f, 0f));
			StartCoroutine(ScaleDown(btnProntoMessage, 1f, 0.4f));
			StartCoroutine(ScaleUp(btnProntoMessage, 2f, 0.7f));
			StartCoroutine(ScaleDown(btnProntoMessage, 1f, 1.1f));
			StartCoroutine(ScaleUp(btnProntoMessage, 2f, 1.4f));
			StartCoroutine(ScaleDown(btnProntoMessage, 1f, 1.8f));
		}
		yield return new WaitForSeconds(0.5f);
		btnProntoPanel.enabled = true;
		ProntoTap = true;
		btnProntoPanel.enabled = true;
		offset.x = 1.555f;
		offset.y = -0.53978f;
		offset.z = 0f;
	}

	public void SelectProntoPanel()
	{
		StartCoroutine(HideHand(0f));
		ArmorPanelView = false;
		SlugPanelView = false;
		ProntoTap = false;
		indexShadow = 2;
		StartCoroutine(Part_2_0_1(0.5f));
	}

	private IEnumerator Part_2_0_1(float weit)
	{
		yield return new WaitForSeconds(weit);
		Pronto = GameObject.Find("prf_Pronto_Assistant(Clone)").transform;
		ProntoPanelView = true;
	}

	private IEnumerator Part_3_0(float weit)
	{
		yield return new WaitForSeconds(weit);
		btnEliPanel.enabled = true;
		EliTap = true;
		btnEliPanel.enabled = true;
		offset.x = 1.714f;
		offset.y = 0.404f;
		offset.z = 0f;
	}

	public void SelectEliPanel()
	{
		StartCoroutine(HideHand(0f));
		ProntoPanelView = false;
		SlugPanelView = false;
		ProntoPanelView = false;
		EliTap = false;
		indexShadow = 2;
		ReadyStart();
	}

	public void ReadyStart()
	{
		btnSlugUpgrate.transform.localScale = Vector3.one;
		btnArmorUpgrate.transform.parent.parent.localScale = Vector3.one;
		btnProntoUpgrate.transform.parent.parent.localScale = Vector3.one;
		btnSlugEquip.sendMessageTarget = TargetSlugEquip;
		btnArmorEquip.sendMessageTarget = TargetArmorEquip;
		btnProntoEquip.sendMessageTarget = TargetProntoEquip;
		btnOpenAppStore.enabled = true;
		btnExitAppStore.enabled = true;
		btnMoneyAppStore.enabled = true;
		btnPodpiskaAppStore.enabled = true;
		if (Reklama != null)
		{
			Reklama.transform.localScale = Vector3.one;
		}
		EquipPanel.transform.localScale = Vector3.one;
		for (i = 0; i < ConsoleLeft.Length; i++)
		{
			ConsoleLeft[i].DOLocalMoveZ(startZLeft, 0f);
		}
		for (i = 0; i < ConsoleRight.Length; i++)
		{
			ConsoleRight[i].DOLocalMoveZ(startZRight, 0f);
		}
		AppStore.transform.DOLocalMoveZ(startZAppStore, 0f);
		for (i = 0; i < HideElements.Length; i++)
		{
			HideElements[i].position += Vector3.up * 10f;
		}
		hand2d.SetActive(false);
		hand2dFlip.SetActive(false);
		for (i = 0; i < btnMission.Length; i++)
		{
			btnMission[i].enabled = true;
		}
		for (i = 0; i < btnUIItem.Length; i++)
		{
			btnUIItem[i].enabled = true;
		}
		btnSlugBuy.sendMessageTarget = BuyUpgradeRecovery;
		btnSlugBuy.SendMessageOnClickMethodName = "OnButtonClick";
		btnSlugBuy.enabled = true;
		btnSlugUpgrate.enabled = true;
		btnArmorBuy.sendMessageTarget = BuyUpgrade;
		btnArmorBuy.SendMessageOnClickMethodName = "OnButtonClick";
		btnArmorBuy.enabled = true;
		btnArmorUpgrate.enabled = true;
		btnSlugKolb1.enabled = true;
		btnSlugKolb2.enabled = true;
		btnSlugKolb3.enabled = true;
		btnSlugEquip.sendMessageTarget = EquipPanel;
		btnSlugEquip.enabled = true;
		offset.x = -1.7943f;
		offset.y = 0.279f;
		offset.z = 0f;
	}

	public void OnBtnStartClick()
	{
		EquipStore.SendMessage("OnBtnStartClick");
		PlayerPrefs.SetInt("Tutor_CutMegaShopScene", 1);
	}

	private void ScrollOff()
	{
	}

	private void ScrollOn()
	{
	}

	private void LateUpdate()
	{
		if (_goldAdd == 0 && MultiplayerLobby.activeSelf && !_goldadd)
		{
			if (text_gold2 == null)
			{
				text_gold2 = GameObject.Find("lblCountCoins").transform;
			}
			_goldadd = true;
			StartCoroutine(AddMegaGoldPack(1f));
		}
	}
}

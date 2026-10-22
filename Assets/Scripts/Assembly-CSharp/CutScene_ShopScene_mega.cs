using System;
using System.Collections;
using AppsMinistry.Core;
using DG.Tweening;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.Game.UI;
using Slugterra.Scene.Game;
using Slugterra.UI.Shops;
using UnityEngine;

public class CutScene_ShopScene_mega : MonoBehaviour
{
	private bool TutorialMegaShopSceneComplete;

	private int _goldAdd;

	public Transform btnSlugMessage;

	public Transform btnArmorMessage;

	public Transform btnProntoMessage;

	public Transform ScrollView;

	public GameObject EquipStore;

	public GameObject AppStore;

	public GameObject Gifts;

	public GameObject BuyUpgradeRecovery;

	public GameObject BuyUpgrade;

	[SerializeField]
	private int AddGold;

	public Transform text_gold;

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

	public GameObject hand2d;

	public GameObject hand2dFlip;

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

	private Vector3 offset;

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

	private void Awake()
	{
		if (MonoSingleton<AppController>.Instance.Model.MissionSetting.Mission != typeof(QuietLawn_2_3) && MonoSingleton<AppController>.Instance.Model.MissionSetting.Mission != typeof(DailyMission_01) && MonoSingleton<AppController>.Instance.Model.MissionSetting.Mission != typeof(DailyMission_02) && MonoSingleton<AppController>.Instance.Model.MissionSetting.Mission != typeof(DailyMission_03) && MonoSingleton<AppController>.Instance.Model.MissionSetting.Mission != typeof(DailyMission_04))
		{
			gameObject.SetActive(false);
		}
		else if (TutorialMegaShopSceneComplete)
		{
		}
	}

	private void Start()
	{
		_goldAdd = PlayerPrefs.GetInt("bonusGoldnew");
		_buyAppStore = PlayerPrefs.GetInt("buyAppStore");
		_buyPronto = 1;
		if (MonoSingleton<AppController>.Instance.Model.MissionSetting.DefenceSlug.Type == SlugType.Arachnet)
		{
			_ArahnetEquip = 1;
		}
		for (i = 0; i < MonoSingleton<AppController>.Instance.Model.ShopModel.DefenceSlugs.Count; i++)
		{
			if (MonoSingleton<AppController>.Instance.Model.ShopModel.DefenceSlugs[i].IsBought && MonoSingleton<AppController>.Instance.Model.ShopModel.DefenceSlugs[i].Type == SlugType.Arachnet)
			{
				_buyArahnet = 1;
				break;
			}
		}
		if (MonoSingleton<AppController>.Instance.Model.MissionSetting.SpecialSlug.Type == SlugType.Geoshard)
		{
			_GeoshardEquip = 1;
		}
		for (i = 0; i < MonoSingleton<AppController>.Instance.Model.ShopModel.SpecialSlugs.Count; i++)
		{
			if (MonoSingleton<AppController>.Instance.Model.ShopModel.SpecialSlugs[i].IsBought && MonoSingleton<AppController>.Instance.Model.ShopModel.SpecialSlugs[i].Type == SlugType.Geoshard)
			{
				_buyGeoshard = 1;
				break;
			}
		}
		if (MonoSingleton<AppController>.Instance.Model.MissionSetting.Armor.Type == ArmorType.Light)
		{
			_ArmorLiteEquip = 1;
		}
		_ArmorLiteEquip = 1;
		for (i = 0; i < MonoSingleton<AppController>.Instance.Model.ShopModel.Armors.Count; i++)
		{
			if (MonoSingleton<AppController>.Instance.Model.ShopModel.Armors[i].IsBought && MonoSingleton<AppController>.Instance.Model.ShopModel.Armors[i].Type == ArmorType.Light)
			{
				_buyArmorLite = 1;
				break;
			}
		}
		_buyArmorLite = 1;
		if (MonoSingleton<AppController>.Instance.Model.MissionSetting.Assistant.Type == AllyType.Pronto)
		{
			_ProntoEquip = 1;
		}
		int num = PlayerPrefs.GetInt("Tutor_CutMegaShopScene");
		if (num == 1)
		{
			TutorialMegaShopSceneComplete = true;
		}
		else
		{
			TutorialMegaShopSceneComplete = false;
			if (MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(QuietLawn_2_3)).Status == LevelStatus.Completed)
			{
				TutorialMegaShopSceneComplete = true;
			}
		}
		hand2d.SetActive(false);
		hand2dFlip.SetActive(false);
		if (TutorialMegaShopSceneComplete)
		{
			return;
		}
		black = Color.black;
		black.a = 0f;
		black2 = black;
		startZLeft = ConsoleLeft[0].localPosition.z;
		startZAppStore = AppStore.transform.localPosition.z;
		startZRight = ConsoleRight[0].localPosition.z;
		if (Reklama != null)
		{
			Reklama.transform.localScale = Vector3.zero;
		}
		for (i = 0; i < ConsoleLeft.Length; i++)
		{
			ConsoleLeft[i].position -= Vector3.forward * 3f;
		}
		for (i = 0; i < HideElements.Length; i++)
		{
			HideElements[i].position -= Vector3.up * 10f;
		}
		if (text_gold == null)
		{
			text_gold = GameObject.Find("lblCountCoins").transform;
		}
		indexShadow = 1;
		StartCoroutine(AfterStart());
		if (_buyArahnet == 1 && _ArahnetEquip == 1 && _buyGeoshard == 1 && _GeoshardEquip == 1)
		{
			_buyAppStore = 1;
			PlayerPrefs.SetInt("buyAppStore", 1);
			if (_buyArmorLite == 1 && _ArmorLiteEquip == 1)
			{
				if (_ProntoEquip == 0)
				{
					StartCoroutine(Part_2_0(0.55f));
					return;
				}
				_startReady = true;
				indexShadow = 2;
				offset.x = -1.7943f;
				offset.y = 0.279f;
				offset.z = 0f;
				for (i = 0; i < HideElements.Length; i++)
				{
					HideElements[i].position += Vector3.up * 10f;
				}
				for (i = 0; i < ConsoleLeft.Length; i++)
				{
					ConsoleLeft[i].DOLocalMoveZ(startZLeft, 0f);
				}
				for (i = 0; i < ConsoleRight.Length; i++)
				{
					ConsoleRight[i].DOLocalMoveZ(startZRight, 0f);
				}
				AppStore.transform.DOLocalMoveZ(startZAppStore, 0f);
				if (Reklama != null)
				{
					Reklama.transform.localScale = Vector3.one;
				}
			}
			else
			{
				StartCoroutine(Part_1_0(0.55f));
			}
		}
		else if (_buyAppStore == 0)
		{
			StartCoroutine(Part_OpenApp(0.85f));
		}
		else
		{
			StartCoroutine(Part_0_0(0.55f));
		}
	}

	private IEnumerator AfterStart()
	{
		yield return new WaitForSeconds(0.01f);
		if (_startReady)
		{
			yield break;
		}
		btnMission = UnityEngine.Object.FindObjectsOfType<tk2dButton>();
		btnUIItem = UnityEngine.Object.FindObjectsOfType<tk2dUIItem>();
		TargetSlugEquip = btnSlugEquip.sendMessageTarget;
		TargetArmorEquip = btnArmorEquip.sendMessageTarget;
		TargetProntoEquip = btnProntoEquip.sendMessageTarget;
		btnSlugEquip.sendMessageTarget = gameObject;
		btnArmorEquip.sendMessageTarget = gameObject;
		btnProntoEquip.sendMessageTarget = gameObject;
		btnSlugUpgrate.transform.localScale = Vector3.zero;
		for (i = 0; i < btnMission.Length; i++)
		{
			if (!btnMission[i].enabled && btnMission[i].targetObject != null && btnMission[i].targetObject.name == "EquipStore")
			{
				btnMission[i].targetObject = gameObject;
				btnMission[i].messageName = "Temp";
				btnMission[i].gameObject.AddComponent<BoxCollider>();
				btnMission[i].GetComponent<BoxCollider>().size = Vector3.zero;
			}
		}
		if (_buyAppStore == 0)
		{
			Scroll.enabled = false;
		}
		for (i = 0; i < btnMission.Length; i++)
		{
		}
		for (i = 0; i < btnUIItem.Length; i++)
		{
			if (btnUIItem[i].transform.parent.name == "btnSlugs")
			{
				btnSlugPanel = btnUIItem[i];
			}
			if (btnUIItem[i].transform.parent.name == "btnCompanions")
			{
				btnProntoPanel = btnUIItem[i];
			}
			if (btnUIItem[i].transform.parent.name == "btnTotalView")
			{
				btnEliPanel = btnUIItem[i];
			}
			if (btnUIItem[i].transform.parent.name == "btnArmor")
			{
				btnArmorPanel = btnUIItem[i];
			}
			if (btnUIItem[i].transform.parent.name != "btnBack")
			{
				btnUIItem[i].enabled = false;
			}
			else
			{
				btnUIItem[i].transform.parent.position -= Vector3.forward;
			}
		}
		btnOpenAppStore.enabled = false;
		btnExitAppStore.enabled = false;
		AppMoneyScrollable = AppMoneyRoot.parent.GetComponent<tk2dUIScrollableArea>();
		AppPodpiskaScrollable = AppPodpiskaRoot.parent.GetComponent<tk2dUIScrollableArea>();
		btnMoneyAppStore.enabled = false;
		btnPodpiskaAppStore.enabled = false;
		btnSlugBuy.sendMessageTarget = gameObject;
		btnSlugBuy.enabled = false;
		btnSlugUpgrate.enabled = false;
		btnSlugKolb1.enabled = false;
		btnSlugKolb2.enabled = false;
		btnSlugKolb3.enabled = false;
		ScrollOff();
	}

	private IEnumerator AddMegaGoldPack(float weit)
	{
		yield return new WaitForSeconds(weit);
		if (_goldAdd == 0)
		{
			StartCoroutine(ScaleUp(text_gold, 3f, 0f));
			StartCoroutine(ScaleDown(text_gold, 1f, 0.75f));
			SetGold();
		}
	}

	private void SetGold()
	{
		MonoSingleton<AppController>.Instance.Model.Wallet.Gold += AddGold;
		_goldAdd = 1;
		PlayerPrefs.SetInt("bonusGoldnew", 1);
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
		obj.DOScale(Vector3.one * scale, 0.25f);
	}

	private IEnumerator ScaleDown(Transform obj, float scale, float weit)
	{
		yield return new WaitForSeconds(weit);
		obj.DOScale(Vector3.one * scale, 0.25f);
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
		StartCoroutine(AddMegaGoldPack(0.5f));
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
		StartCoroutine(ShowHand(btnExitAppStore.transform, offset, Vector3.forward * 12f, 0f));
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
		StartCoroutine(ShowHand(btnSlugPanel.transform, offset, Vector3.forward * 35f, 0f));
	}

	public void SelectSlugPanel()
	{
		StartCoroutine(HideHand(0f));
		StartCoroutine(AddMegaGoldPack(0.5f));
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
			StartCoroutine(ShowHand(btnSlugEquip.transform, offset, Vector3.zero, 0f));
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
		StartCoroutine(ShowHand(btnArmorPanel.transform, offset, Vector3.forward * 35f, 0f));
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
			StartCoroutine(ShowHand(btnArmorEquip.transform, offset, Vector3.zero, 0f));
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
		StartCoroutine(ShowHand(btnProntoPanel.transform, offset, Vector3.forward * 35f, 0f));
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
		StartCoroutine(ShowHand(btnEliPanel.transform, offset, Vector3.forward * 98f, 0f));
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
		if (_buyAppStore == 0)
		{
			if (_ShowAppStore == 0 && AppStore.activeSelf)
			{
				_ShowAppStore = 1;
				StartCoroutine(HideHand(0f));
				StartCoroutine(Part_ViewMoneyApp(0.5f));
				AppMoneyScrollable.allowSwipeScrolling = false;
			}
			if (_ShowAppStore == 1 && !AppStore.activeSelf)
			{
				_ShowAppStore = 0;
				AppMoneyRoot.DOLocalMoveY(0f, 0f);
				PlayerPrefs.SetInt("buyAppStore", 1);
				AppMoneyScrollable.allowSwipeScrolling = true;
				AppPodpiskaScrollable.allowSwipeScrolling = true;
				btnOpenAppStore.enabled = false;
				btnPodpiskaAppStore.enabled = true;
				btnExitAppStore.transform.gameObject.SetActive(true);
				btnExitAppStore.enabled = true;
				_buyAppStore = 1;
				StopAllCoroutines();
				StartCoroutine(Part_CloseApp(0.5f));
				StartCoroutine(HideHand(0f));
				StartCoroutine(Part_0_0(0.55f));
			}
			if (btnPodpiskaClick && AppPodpiskaRoot.parent.gameObject.activeSelf)
			{
				btnPodpiskaClick = false;
				StartCoroutine(HideHand(0f));
				StartCoroutine(Part_ViewPodpiskaApp(0.5f));
			}
		}
		else if (AppStore.activeSelf || Gifts.activeSelf)
		{
			if (hand2d.GetComponent<MeshRenderer>().enabled)
			{
				hand2d.GetComponent<MeshRenderer>().enabled = false;
			}
			if (hand2dFlip.GetComponent<MeshRenderer>().enabled)
			{
				hand2dFlip.GetComponent<MeshRenderer>().enabled = false;
			}
		}
		else
		{
			if (!hand2d.GetComponent<MeshRenderer>().enabled)
			{
				hand2d.GetComponent<MeshRenderer>().enabled = true;
			}
			if (!hand2dFlip.GetComponent<MeshRenderer>().enabled)
			{
				hand2dFlip.GetComponent<MeshRenderer>().enabled = true;
			}
		}
		if (SlugPanelView)
		{
			if (_buyArahnet == 0)
			{
				if (Vector3.Distance(ScrollView.position - Vector3.up * 0.7f, Arahnet.position) < minY)
				{
					hand2d.SetActive(false);
					offset.x = -1.5f;
					offset.y = -0.41f;
					offset.z = 0f;
					hand2dFlip.SetActive(true);
					MoveHandFlip(btnSlugBuy.transform, offset, Vector3.forward * 133f);
					btnSlugBuy.enabled = true;
					btnSlugBuy.SendMessageOnClickMethodName = "OnButtonClick_A";
				}
				else
				{
					btnSlugBuy.enabled = false;
					if (Vector3.Distance(ScrollView.position - Vector3.up * 0.7f, Arahnet.position) > maxY)
					{
						hand2d.SetActive(true);
						hand2dFlip.SetActive(false);
						if (ScrollView.position.y - Arahnet.position.y > 0f)
						{
							offset.x = 0.659f;
							offset.y = -0.8964f;
							offset.z = 0f;
							MoveHand(Arrows[0].transform, offset, Vector3.zero);
						}
						else
						{
							offset.x = 0.659f;
							offset.y = -0.8964f;
							offset.z = 0f;
							MoveHand(Arrows[1].transform, offset, Vector3.zero);
						}
					}
					else
					{
						hand2d.SetActive(false);
					}
				}
			}
			else if (_ArahnetEquip == 0)
			{
				if (Vector3.Distance(ScrollView.position - Vector3.up * 0.7f, Arahnet.position) < minY)
				{
					hand2d.SetActive(true);
					if (_ArahnetEquip == 0 && btnSlugEquip.gameObject.activeSelf)
					{
						offset.x = 1.5195f;
						offset.y = 0.47906f;
						offset.z = 0f;
						MoveHand(btnSlugEquip.transform, offset, Vector3.forward * 95f);
						EquipPanel.transform.localScale = Vector3.one;
					}
				}
				else if (Vector3.Distance(ScrollView.position - Vector3.up * 0.7f, Arahnet.position) > maxY)
				{
					hand2d.SetActive(true);
					if (ScrollView.position.y - Arahnet.position.y > 0f)
					{
						offset.x = 0.659f;
						offset.y = -0.8964f;
						offset.z = 0f;
						MoveHand(Arrows[0].transform, offset, Vector3.zero);
					}
					else
					{
						offset.x = 0.659f;
						offset.y = -0.8964f;
						offset.z = 0f;
						MoveHand(Arrows[1].transform, offset, Vector3.zero);
					}
				}
				else
				{
					hand2d.SetActive(false);
				}
			}
			else if (Arahnet != null)
			{
				if (Vector3.Distance(ScrollView.position - Vector3.up * 0.7f, Arahnet.position) < 4f)
				{
					EquipPanel.transform.localScale = Vector3.zero;
				}
				else if (Vector3.Distance(ScrollView.position - Vector3.up * 0.7f, Arahnet.position) < 5f)
				{
					EquipPanel.transform.localScale = Vector3.one;
				}
			}
			if (_buyArahnet == 1 && _ArahnetEquip == 1)
			{
				if (_buyGeoshard == 0)
				{
					if (Vector3.Distance(ScrollView.position - Vector3.up * 0.7f, Geoshard.position) < minY)
					{
						hand2d.SetActive(false);
						offset.x = -1.5f;
						offset.y = -0.41f;
						offset.z = 0f;
						hand2dFlip.SetActive(true);
						MoveHandFlip(btnSlugBuy.transform, offset, Vector3.forward * 133f);
						btnSlugBuy.enabled = true;
						btnSlugBuy.SendMessageOnClickMethodName = "OnButtonClick_G";
					}
					else
					{
						btnSlugBuy.enabled = false;
						if (Vector3.Distance(ScrollView.position - Vector3.up * 0.7f, Geoshard.position) > maxY)
						{
							hand2d.SetActive(true);
							hand2dFlip.SetActive(false);
							if (ScrollView.position.y - Geoshard.position.y > 0f)
							{
								offset.x = 0.659f;
								offset.y = -0.8964f;
								offset.z = 0f;
								MoveHand(Arrows[0].transform, offset, Vector3.zero);
							}
							else
							{
								offset.x = 0.659f;
								offset.y = -0.8964f;
								offset.z = 0f;
								MoveHand(Arrows[1].transform, offset, Vector3.zero);
							}
						}
						else
						{
							hand2d.SetActive(false);
						}
					}
				}
				else if (_GeoshardEquip == 0)
				{
					if (Vector3.Distance(ScrollView.position - Vector3.up * 0.7f, Geoshard.position) < minY)
					{
						hand2d.SetActive(true);
						if (_GeoshardEquip == 0 && btnSlugEquip.gameObject.activeSelf)
						{
							offset.x = 1.5195f;
							offset.y = 0.47906f;
							offset.z = 0f;
							MoveHand(btnSlugEquip.transform, offset, Vector3.forward * 95f);
							EquipPanel.transform.localScale = Vector3.one;
						}
					}
					else if (Vector3.Distance(ScrollView.position - Vector3.up * 0.7f, Geoshard.position) > maxY)
					{
						hand2d.SetActive(true);
						if (ScrollView.position.y - Geoshard.position.y > 0f)
						{
							offset.x = 0.659f;
							offset.y = -0.8964f;
							offset.z = 0f;
							MoveHand(Arrows[0].transform, offset, Vector3.zero);
						}
						else
						{
							offset.x = 0.659f;
							offset.y = -0.8964f;
							offset.z = 0f;
							MoveHand(Arrows[1].transform, offset, Vector3.zero);
						}
					}
					else
					{
						hand2d.SetActive(false);
					}
				}
				else if (Geoshard != null)
				{
					if (Vector3.Distance(ScrollView.position - Vector3.up * 0.7f, Geoshard.position) < 4f)
					{
						EquipPanel.transform.localScale = Vector3.zero;
					}
					else if (Vector3.Distance(ScrollView.position - Vector3.up * 0.7f, Geoshard.position) < 5f)
					{
						EquipPanel.transform.localScale = Vector3.one;
					}
				}
			}
		}
		if (ArmorPanelView)
		{
			if (_buyArmorLite == 0)
			{
				if (Vector3.Distance(ScrollView.position - Vector3.up * 0.7f, ArmorLite.position) < minY)
				{
					hand2d.SetActive(false);
					offset.x = -1.5f;
					offset.y = -0.41f;
					offset.z = 0f;
					hand2dFlip.SetActive(true);
					MoveHandFlip(btnArmorBuy.transform, offset, Vector3.forward * 133f);
					btnArmorBuy.enabled = true;
					btnArmorBuy.SendMessageOnClickMethodName = "OnButtonClick_Armor";
				}
				else
				{
					btnArmorBuy.enabled = false;
					if (Vector3.Distance(ScrollView.position - Vector3.up * 0.7f, ArmorLite.position) > maxY)
					{
						hand2d.SetActive(true);
						hand2dFlip.SetActive(false);
						if (ScrollView.position.y - ArmorLite.position.y > 0f)
						{
							offset.x = 0.659f;
							offset.y = -0.8964f;
							offset.z = 0f;
							MoveHand(Arrows[0].transform, offset, Vector3.zero);
						}
						else
						{
							offset.x = 0.659f;
							offset.y = -0.8964f;
							offset.z = 0f;
							MoveHand(Arrows[1].transform, offset, Vector3.zero);
						}
					}
					else
					{
						hand2d.SetActive(false);
					}
				}
			}
			else if (_ArmorLiteEquip == 0)
			{
				if (Vector3.Distance(ScrollView.position - Vector3.up * 0.7f, ArmorLite.position) < minY)
				{
					hand2d.SetActive(true);
					if (_ArmorLiteEquip == 0 && btnArmorEquip.gameObject.activeSelf)
					{
						offset.x = 1.5195f;
						offset.y = 0.47906f;
						offset.z = 0f;
						MoveHand(btnArmorEquip.transform, offset, Vector3.forward * 95f);
						EquipPanel.transform.localScale = Vector3.one;
					}
				}
				else if (Vector3.Distance(ScrollView.position - Vector3.up * 0.7f, ArmorLite.position) > maxY)
				{
					hand2d.SetActive(true);
					if (ScrollView.position.y - ArmorLite.position.y > 0f)
					{
						offset.x = 0.659f;
						offset.y = -0.8964f;
						offset.z = 0f;
						MoveHand(Arrows[0].transform, offset, Vector3.zero);
					}
					else
					{
						offset.x = 0.659f;
						offset.y = -0.8964f;
						offset.z = 0f;
						MoveHand(Arrows[1].transform, offset, Vector3.zero);
					}
				}
				else
				{
					hand2d.SetActive(false);
				}
			}
			else if (ArmorLite != null)
			{
				if (Vector3.Distance(ScrollView.position - Vector3.up * 0.7f, ArmorLite.position) < 4f)
				{
					EquipPanel.transform.localScale = Vector3.zero;
				}
				else
				{
					EquipPanel.transform.localScale = Vector3.one;
				}
			}
		}
		if (!ProntoPanelView)
		{
			return;
		}
		if (_buyPronto == 0)
		{
			if (Vector3.Distance(ScrollView.position - Vector3.up * 0.7f, Pronto.position) < minY)
			{
				hand2d.SetActive(true);
				offset.x = 0.15926f;
				offset.y = -0.74959f;
				offset.z = 0f;
				MoveHand(btnProntoBuy.transform, offset, Vector3.forward * 20f);
				btnProntoBuy.enabled = true;
				btnProntoBuy.SendMessageOnClickMethodName = "OnButtonClick_Pronto";
				return;
			}
			btnProntoBuy.enabled = false;
			if (Vector3.Distance(ScrollView.position - Vector3.up * 0.7f, Pronto.position) > maxY)
			{
				hand2d.SetActive(true);
				if (ScrollView.position.y - Pronto.position.y > 0f)
				{
					offset.x = 0.659f;
					offset.y = -0.8964f;
					offset.z = 0f;
					MoveHand(Arrows[0].transform, offset, Vector3.zero);
				}
				else
				{
					offset.x = 0.659f;
					offset.y = -0.8964f;
					offset.z = 0f;
					MoveHand(Arrows[1].transform, offset, Vector3.zero);
				}
			}
			else
			{
				hand2d.SetActive(false);
			}
		}
		else if (_ProntoEquip == 0)
		{
			if (Vector3.Distance(ScrollView.position - Vector3.up * 0.7f, Pronto.position) < minY)
			{
				hand2d.SetActive(true);
				if (_ProntoEquip == 0 && btnProntoEquip.gameObject.activeSelf)
				{
					offset.x = 1.5195f;
					offset.y = 0.47906f;
					offset.z = 0f;
					MoveHand(btnProntoEquip.transform, offset, Vector3.forward * 95f);
					EquipPanel.transform.localScale = Vector3.one;
				}
			}
			else if (Vector3.Distance(ScrollView.position - Vector3.up * 0.7f, Pronto.position) > maxY)
			{
				hand2d.SetActive(true);
				if (ScrollView.position.y - Pronto.position.y > 0f)
				{
					offset.x = 0.659f;
					offset.y = -0.8964f;
					offset.z = 0f;
					MoveHand(Arrows[0].transform, offset, Vector3.zero);
				}
				else
				{
					offset.x = 0.659f;
					offset.y = -0.8964f;
					offset.z = 0f;
					MoveHand(Arrows[1].transform, offset, Vector3.zero);
				}
			}
			else
			{
				hand2d.SetActive(false);
			}
		}
		else if (Pronto != null)
		{
			if (Vector3.Distance(ScrollView.position - Vector3.up * 0.7f, Pronto.position) < 4f)
			{
				EquipPanel.transform.localScale = Vector3.zero;
			}
			else
			{
				EquipPanel.transform.localScale = Vector3.one;
			}
		}
	}
}

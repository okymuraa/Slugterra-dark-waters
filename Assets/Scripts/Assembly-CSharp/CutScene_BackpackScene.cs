using System.Collections;
using AppsMinistry.Core;
using DG.Tweening;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.Game.UI;
using Slugterra.Scene.Game;
using Slugterra.UI.Popup;
using Slugterra.UI.Shops;
using UnityEngine;

public class CutScene_BackpackScene : MonoBehaviour
{
	private bool TutorialBackpackSceneComplete;

	private byte _goldAdd;

	private byte _openAppStore;

	public Transform[] ConsoleLeft;

	public Transform[] ConsoleRight;

	public GameObject[] Arrows;

	[SerializeField]
	private int AddGold;

	public Transform[] text_gold;

	public GameObject Gifts;

	public GameObject AppStore;

	public GameObject SlugPriviwe;

	public tk2dUIItem btnOpenAppStore;

	public GameObject btnOpenAppStoreTViOS;

	public GameObject Recomendation;

	private byte _ShowAppStore;

	private byte _buyAppStore;

	public tk2dUIItem btnExitAppStore;

	public tk2dUIItem btnMoneyAppStore;

	public tk2dUIItem btnAlmazAppStore;

	private bool btnAlmazClick;

	public tk2dUIItem btnPodpiskaAppStore;

	private bool btnPodpiskaClick;

	public Transform AppMoneyRoot;

	private Transform[] contentAppMoney;

	public Transform AppPodpiskaRoot;

	private SubscriptionItemController[] contentAppPodpiska;

	public Transform AppGemsRoot;

	private Transform[] contentAppGems;

	public tk2dUIScrollableArea ScrollableAreaRecommended;

	public ShopShadowConttroller ScrollableShadow;

	public RecommendedItemView[] RecommendedItem;

	public Transform AreaRecommendedRoot;

	public tk2dUIScrollableArea AppMoneyScrollable;

	public tk2dUIScrollableArea AppGemsScrollable;

	public tk2dUIScrollableArea AppPodpiskaScrollable;

	private tk2dButton[] btnMission;

	private tk2dUIItem[] btnUIItem;

	private tk2dUIToggleButton[] btnUIToggle;

	public tk2dButton btnExitSlugShow;

	public tk2dButton BtnStart;

	public tk2dButton BtnArsenal;

	private Transform btnBack;

	public GameObject hand2d;

	public GameObject hand2dFlip;

	private bool SelectItems;

	private byte _ShowInfurnus;

	private RecommendedItemView Infurnus;

	private int _buyInfurnus;

	private int _InfurnusEquip;

	private int _InfurnusUpgrade;

	private RecommendedItemView Arahnet;

	private int _buyArahnet;

	private int _ArahnetEquip;

	private int _ArahnetUpgrade;

	private RecommendedItemView Geoshard;

	private int _buyGeoshard;

	private int _GeoshardEquip;

	private int _GeoshardUpgrade;

	private RecommendedItemView Pronto;

	private int _buyPronto = 1;

	private int _ProntoEquip;

	private int _ProntoUpgrade;

	private Vector3 offset;

	public Renderer[] shadow;

	public Renderer[] shadow2;

	public float shadowSpeed = 0.25f;

	private Color black;

	private Color black2;

	private int indexShadow;

	private int indexShadow_2;

	private bool shadowFadeUp;

	private bool shadowFadeDown;

	private int i;

	private bool EventActive;

	private float timeShowHand;

	private void OnDisable()
	{
		if (EventActive)
		{
			BtnStart.ButtonPressedEvent -= OnBtnStartClick;
			btnExitSlugShow.ButtonUpEvent -= SlugShowBack;
			Infurnus.CurButton(0).ButtonPressedEvent -= OnChekEquip;
			Infurnus.CurButton(1).ButtonPressedEvent -= OnChekEquip;
			Infurnus.CurButton(2).ButtonPressedEvent -= OnChekEquip;
			Infurnus.CurButton(4).ButtonPressedEvent -= SlugShow;
			Arahnet.CurButton(0).ButtonPressedEvent -= OnChekEquip;
			Arahnet.CurButton(1).ButtonPressedEvent -= OnChekEquip;
			Arahnet.CurButton(2).ButtonPressedEvent -= OnChekEquip;
			Geoshard.CurButton(0).ButtonPressedEvent -= OnChekEquip;
			Geoshard.CurButton(1).ButtonPressedEvent -= OnChekEquip;
			Geoshard.CurButton(2).ButtonPressedEvent -= OnChekEquip;
			Pronto.CurButton(0).ButtonPressedEvent -= OnChekEquip;
			Pronto.CurButton(1).ButtonPressedEvent -= OnChekEquip;
			Pronto.CurButton(2).ButtonPressedEvent -= OnChekEquip;
		}
	}

	private void Awake()
	{
		if (MonoSingleton<AppController>.Instance.Model.MissionSetting.Mission != typeof(QuietLawn_2_3))
		{
			StopAllCoroutines();
			gameObject.SetActive(false);
		}
	}

	private void Start()
	{
		_goldAdd = (byte)PlayerPrefs.GetInt("bonusGoldBackpack");
		_buyAppStore = (byte)PlayerPrefs.GetInt("buyAppStoreBackpack");
		_ShowInfurnus = (byte)PlayerPrefs.GetInt("showInfurnusBackpack");
		OnChekEquipStart();
		int num = PlayerPrefs.GetInt("Tutor_CutBackpackSceneComplete");
		if (num == 1)
		{
			TutorialBackpackSceneComplete = true;
		}
		else
		{
			TutorialBackpackSceneComplete = false;
			if (MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(QuietLawn_2_3)).Status == LevelStatus.Completed || MonoSingleton<AppController>.Instance.Model.MissionSetting.AttackSlug.Type != SlugType.Infurnus || (MonoSingleton<AppController>.Instance.Model.MissionSetting.DefenceSlug.Type != SlugType.Arachnet && MonoSingleton<AppController>.Instance.Model.MissionSetting.DefenceSlug.Type != SlugType.None) || (MonoSingleton<AppController>.Instance.Model.MissionSetting.SpecialSlug.Type != SlugType.Geoshard && MonoSingleton<AppController>.Instance.Model.MissionSetting.SpecialSlug.Type != SlugType.None))
			{
				TutorialBackpackSceneComplete = true;
				StopAllCoroutines();
				gameObject.SetActive(false);
				return;
			}
		}
		hand2d.SetActive(false);
		hand2dFlip.SetActive(false);
		if (!TutorialBackpackSceneComplete)
		{
			black = Color.black;
			black.a = 0f;
			black2 = black;
			Recomendation.GetComponent<MoveAnimUI>().enabled = false;
			Recomendation.transform.localPosition = -Vector3.up;
			Recomendation.SetActive(false);
			BtnStart.enabled = false;
			BtnArsenal.enabled = false;
			btnOpenAppStore.enabled = false;
			if (_buyAppStore == 0)
			{
				StartCoroutine(AfterStart());
				StartCoroutine(Part_OpenApp(0.2f));
			}
			else if (_buyInfurnus == 1 && _InfurnusEquip == 1 && _InfurnusUpgrade == 1 && _buyArahnet == 1 && _ArahnetEquip == 1 && _ArahnetUpgrade == 1 && _buyGeoshard == 1 && _GeoshardEquip == 1 && _GeoshardUpgrade == 1 && _ProntoEquip == 1 && _ProntoUpgrade == 1)
			{
				StartCoroutine(Part_Start(0f));
			}
			else
			{
				StartCoroutine(AfterStart());
				StartCoroutine(Part_Item(0.2f));
			}
		}
		else if (MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(QuietLawn_2_3)).Status != LevelStatus.Completed)
		{
			StartCoroutine(Part_Start(0f));
		}
	}

	private IEnumerator AfterStart()
	{
		yield return new WaitForSeconds(0.1f);
		btnMission = Object.FindObjectsOfType<tk2dButton>();
		btnUIItem = Object.FindObjectsOfType<tk2dUIItem>();
		RecommendedItem = Object.FindObjectsOfType<RecommendedItemView>();
		for (i = 0; i < RecommendedItem.Length; i++)
		{
			if (RecommendedItem[i].SlugType == SlugType.Infurnus)
			{
				Infurnus = RecommendedItem[i];
				Infurnus.CurButton(0).ButtonPressedEvent += OnChekEquip;
				Infurnus.CurButton(1).ButtonPressedEvent += OnChekEquip;
				Infurnus.CurButton(2).ButtonPressedEvent += OnChekEquip;
				Infurnus.CurButton(4).ButtonPressedEvent += SlugShow;
			}
			if (RecommendedItem[i].SlugType == SlugType.Arachnet)
			{
				Arahnet = RecommendedItem[i];
				Arahnet.CurButton(0).ButtonPressedEvent += OnChekEquip;
				Arahnet.CurButton(1).ButtonPressedEvent += OnChekEquip;
				Arahnet.CurButton(2).ButtonPressedEvent += OnChekEquip;
			}
			if (RecommendedItem[i].SlugType == SlugType.Geoshard)
			{
				Geoshard = RecommendedItem[i];
				Geoshard.CurButton(0).ButtonPressedEvent += OnChekEquip;
				Geoshard.CurButton(1).ButtonPressedEvent += OnChekEquip;
				Geoshard.CurButton(2).ButtonPressedEvent += OnChekEquip;
			}
			if (RecommendedItem[i].AllyType == AllyType.Pronto)
			{
				Pronto = RecommendedItem[i];
				Pronto.CurButton(0).ButtonPressedEvent += OnChekEquip;
				Pronto.CurButton(1).ButtonPressedEvent += OnChekEquip;
				Pronto.CurButton(2).ButtonPressedEvent += OnChekEquip;
			}
			if ((double)RecommendedItem[i].transform.localPosition.x > 14.399999999999999)
			{
				RecommendedItem[i].gameObject.SetActive(false);
			}
		}
		BtnStart.ButtonPressedEvent += OnBtnStartClick;
		btnExitSlugShow.ButtonUpEvent += SlugShowBack;
		EventActive = true;
		for (i = 0; i < btnMission.Length; i++)
		{
			if (btnMission[i].transform.parent.name != "btnBack")
			{
				btnMission[i].enabled = false;
			}
		}
		for (i = 0; i < btnUIItem.Length; i++)
		{
			if (btnUIItem[i].transform.name != "ScrollableAreaRecommended")
			{
				btnUIItem[i].enabled = false;
			}
		}
		btnMoneyAppStore.enabled = false;
		btnPodpiskaAppStore.enabled = false;
		btnAlmazAppStore.enabled = false;
		btnOpenAppStore.enabled = false;
		ScrollableAreaRecommended.allowScrollWheel = false;
		ScrollableAreaRecommended.allowSwipeScrolling = false;
	}

	private IEnumerator AddMegaGoldPack(float weit)
	{
		yield return new WaitForSeconds(weit);
		if (_goldAdd == 0)
		{
			for (int i = 0; i < text_gold.Length; i++)
			{
				StartCoroutine(ScaleUp(text_gold[i], 1.8f, 0f));
				StartCoroutine(ScaleDown(text_gold[i], 1f, 0.75f));
			}
			SetGold();
		}
	}

	private void SetGold()
	{
		int addGold = AddGold;
		addGold -= (int)MonoSingleton<AppController>.Instance.Model.Wallet.Gold;
		if (addGold <= 0)
		{
			AddGold = 0;
		}
		else
		{
			AddGold = addGold;
		}
		MonoSingleton<AppController>.Instance.Model.Wallet.Gold += AddGold;
		_goldAdd = 1;
		PlayerPrefs.SetInt("bonusGoldBackpack", 1);
	}

	private IEnumerator ShowHand(Transform obj, Vector3 offset, Vector3 rotation, float weit)
	{
		yield return new WaitForSeconds(weit);
		if (obj.gameObject.activeSelf)
		{
			hand2d.transform.position = obj.position + offset;
			hand2d.transform.eulerAngles = rotation;
			hand2d.SetActive(true);
		}
	}

	private void MoveHand(Transform obj, Vector3 offset, Vector3 rotation)
	{
		hand2d.transform.position = obj.position + offset;
		hand2d.transform.eulerAngles = rotation;
	}

	private IEnumerator ShowHandFlip(Transform obj, Vector3 offset, Vector3 rotation, float weit)
	{
		yield return new WaitForSeconds(weit);
		if (obj.gameObject.activeSelf)
		{
			hand2dFlip.transform.position = obj.position + offset;
			hand2dFlip.transform.eulerAngles = rotation;
			hand2dFlip.SetActive(true);
		}
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
		obj.DOScale(Vector3.one * scale, 0.5f);
	}

	private IEnumerator ScaleDown(Transform obj, float scale, float weit)
	{
		yield return new WaitForSeconds(weit);
		obj.DOScale(Vector3.one * scale, 0.5f);
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
		ScrollableAreaRecommended.allowScrollWheel = true;
		ScrollableAreaRecommended.allowSwipeScrolling = true;
	}

	private IEnumerator Part_Item(float weit)
	{
		StartCoroutine(AddMegaGoldPack(0.5f));
		yield return new WaitForSeconds(weit);
		SelectItems = true;
	}

	private IEnumerator Part_OpenApp(float weit)
	{
		yield return new WaitForSeconds(weit);
		btnOpenAppStore.enabled = true;
		btnExitAppStore.enabled = false;
		offset.x = -0.964f;
		offset.y = -0.802f;
		offset.z = 0f;
		StartCoroutine(ShowHandFlip(btnOpenAppStore.transform, offset, Vector3.forward * 149f, 0f));
	}

	private IEnumerator Part_ViewMoneyApp(float weit)
	{
		ScrollableShadow.ActiveScrollTutorial(0, 1);
		yield return new WaitForSeconds(weit);
		AppMoneyScrollable.allowSwipeScrolling = false;
		AppMoneyScrollable.allowScrollWheel = false;
		ScrollableShadow.ActiveScrollTutorial(1, 10);
		btnMoneyAppStore.enabled = true;
		StartCoroutine(AddMegaGoldPack(0.5f));
		AdsItemController[] tempContentAppMoneyAds = AppMoneyRoot.GetComponentsInChildren<AdsItemController>();
		InAppItemController[] tempContentAppMoney = AppMoneyRoot.GetComponentsInChildren<InAppItemController>();
		contentAppMoney = new Transform[tempContentAppMoney.Length + tempContentAppMoneyAds.Length];
		int d = 0;
		for (int i = 0; i < contentAppMoney.Length; i++)
		{
			if (d < tempContentAppMoneyAds.Length)
			{
				contentAppMoney[i] = tempContentAppMoneyAds[d].transform;
			}
			else
			{
				contentAppMoney[i] = tempContentAppMoney[d - tempContentAppMoneyAds.Length].transform;
			}
			d++;
		}
		StartCoroutine(MoveLocalX(AppMoneyRoot, -9.85f, 14f));
		this.i = 0;
		while (this.i < contentAppMoney.Length)
		{
			contentAppMoney[this.i].transform.DOScale(0.9f, 0.18f);
			yield return new WaitForSeconds(0.2f);
			contentAppMoney[this.i].transform.DOScale(1f, 0.4f);
			this.i++;
		}
		yield return new WaitForSeconds(0.5f);
		AppMoneyScrollable.allowSwipeScrolling = true;
		AppMoneyScrollable.allowScrollWheel = true;
		offset.x = 1.54f;
		offset.y = -0.9f;
		offset.z = -1f;
		StartCoroutine(ShowHand(btnPodpiskaAppStore.transform, offset, Vector3.forward * 12f, 0f));
		btnMoneyAppStore.enabled = false;
		btnPodpiskaAppStore.enabled = true;
		btnPodpiskaClick = true;
	}

	private IEnumerator Part_ViewPodpiskaApp(float weit)
	{
		bool fix = true;
		ScrollableShadow.ActiveScrollTutorial(0, 1);
		yield return new WaitForSeconds(weit);
		AppPodpiskaScrollable.allowSwipeScrolling = false;
		AppPodpiskaScrollable.allowScrollWheel = false;
		if (fix)
		{
			contentAppPodpiska = AppPodpiskaRoot.GetComponentsInChildren<SubscriptionItemController>();
			for (i = 0; i < contentAppPodpiska.Length; i++)
			{
				contentAppPodpiska[i].transform.DOScale(0.9f, 0.24f);
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
		yield return new WaitForSeconds(0.5f);
		AppPodpiskaScrollable.allowSwipeScrolling = true;
		AppPodpiskaScrollable.allowScrollWheel = true;
		offset.x = 1.54f;
		offset.y = -0.9f;
		offset.z = -1f;
		StartCoroutine(ShowHand(btnAlmazAppStore.transform, offset, Vector3.forward * 12f, 0f));
		btnPodpiskaAppStore.enabled = false;
		btnAlmazAppStore.enabled = true;
		btnAlmazClick = true;
	}

	private IEnumerator Part_ViewGemApp(float weit)
	{
		ScrollableShadow.ActiveScrollTutorial(0, 1);
		yield return new WaitForSeconds(weit);
		AppGemsScrollable.allowSwipeScrolling = false;
		AppGemsScrollable.allowScrollWheel = false;
		ScrollableShadow.ActiveScrollTutorial(1, 10);
		AdsItemController[] tempContentAppGemsAds = AppGemsRoot.GetComponentsInChildren<AdsItemController>();
		InAppItemController[] tempContentGemsMoney = AppGemsRoot.GetComponentsInChildren<InAppItemController>();
		contentAppGems = new Transform[tempContentGemsMoney.Length + tempContentAppGemsAds.Length];
		int d = 0;
		for (int i = 0; i < contentAppGems.Length; i++)
		{
			if (d < tempContentAppGemsAds.Length)
			{
				contentAppGems[i] = tempContentAppGemsAds[d].transform;
			}
			else
			{
				contentAppGems[i] = tempContentGemsMoney[d - tempContentAppGemsAds.Length].transform;
			}
			d++;
		}
		StartCoroutine(MoveLocalX(AppGemsRoot, -7.21f, 14f));
		this.i = 0;
		while (this.i < contentAppGems.Length)
		{
			contentAppGems[this.i].transform.DOScale(0.9f, 0.18f);
			yield return new WaitForSeconds(0.2f);
			contentAppGems[this.i].transform.DOScale(1f, 0.4f);
			this.i++;
		}
		yield return new WaitForSeconds(0.5f);
		AppGemsScrollable.allowSwipeScrolling = true;
		AppGemsScrollable.allowScrollWheel = true;
		offset.x = -1.13f;
		offset.y = -0.78f;
		offset.z = -1f;
		StartCoroutine(ShowHandFlip(btnExitAppStore.transform, offset, Vector3.forward * 150f, 0f));
		btnAlmazAppStore.enabled = true;
		btnMoneyAppStore.enabled = true;
		btnPodpiskaAppStore.enabled = true;
		btnExitAppStore.enabled = true;
	}

	private IEnumerator Part_Start(float weit)
	{
		yield return new WaitForSeconds(weit);
		offset.x = -1.845f;
		offset.y = 0.7f;
		offset.z = 0f;
		MoveHandFlip(BtnStart.transform, offset, Vector3.forward * 82f);
		hand2dFlip.SetActive(true);
	}

	private void OnChekEquipStart()
	{
		_buyInfurnus = 1;
		if (MonoSingleton<AppController>.Instance.Model.MissionSetting.AttackSlug.Type == SlugType.Infurnus)
		{
			_InfurnusEquip = 1;
			if (MonoSingleton<AppController>.Instance.Model.MissionSetting.AttackSlug.Level > 0)
			{
				_InfurnusUpgrade = 1;
			}
		}
		for (i = 0; i < MonoSingleton<AppController>.Instance.Model.ShopModel.DefenceSlugs.Count; i++)
		{
			if (MonoSingleton<AppController>.Instance.Model.ShopModel.DefenceSlugs[i].IsBought && MonoSingleton<AppController>.Instance.Model.ShopModel.DefenceSlugs[i].Type == SlugType.Infurnus)
			{
				_buyInfurnus = 1;
				break;
			}
		}
		if (MonoSingleton<AppController>.Instance.Model.MissionSetting.DefenceSlug.Type == SlugType.Arachnet)
		{
			_ArahnetEquip = 1;
			if (MonoSingleton<AppController>.Instance.Model.MissionSetting.DefenceSlug.Level >= 0)
			{
				_ArahnetUpgrade = 1;
			}
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
			if (MonoSingleton<AppController>.Instance.Model.MissionSetting.SpecialSlug.Level >= 0)
			{
				_GeoshardUpgrade = 1;
			}
		}
		for (i = 0; i < MonoSingleton<AppController>.Instance.Model.ShopModel.SpecialSlugs.Count; i++)
		{
			if (MonoSingleton<AppController>.Instance.Model.ShopModel.SpecialSlugs[i].IsBought && MonoSingleton<AppController>.Instance.Model.ShopModel.SpecialSlugs[i].Type == SlugType.Geoshard)
			{
				_buyGeoshard = 1;
				break;
			}
		}
		_buyPronto = 1;
		if (MonoSingleton<AppController>.Instance.Model.MissionSetting.Assistant.Type == AllyType.Pronto)
		{
			_ProntoEquip = 1;
			if (MonoSingleton<AppController>.Instance.Model.MissionSetting.Assistant.Level > 0)
			{
				_ProntoUpgrade = 1;
			}
		}
	}

	private void OnChekEquip(tk2dButton t)
	{
		_buyInfurnus = 1;
		if (MonoSingleton<AppController>.Instance.Model.MissionSetting.AttackSlug.Type != SlugType.None)
		{
			_InfurnusEquip = 1;
			if (MonoSingleton<AppController>.Instance.Model.MissionSetting.AttackSlug.Level > 0)
			{
				_InfurnusUpgrade = 1;
			}
		}
		if (MonoSingleton<AppController>.Instance.Model.MissionSetting.DefenceSlug.Type == SlugType.Arachnet)
		{
			_ArahnetEquip = 1;
			if (MonoSingleton<AppController>.Instance.Model.MissionSetting.DefenceSlug.Level >= 0)
			{
				_ArahnetUpgrade = 1;
			}
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
			if (MonoSingleton<AppController>.Instance.Model.MissionSetting.SpecialSlug.Level >= 0)
			{
				_GeoshardUpgrade = 1;
			}
		}
		for (i = 0; i < MonoSingleton<AppController>.Instance.Model.ShopModel.SpecialSlugs.Count; i++)
		{
			if (MonoSingleton<AppController>.Instance.Model.ShopModel.SpecialSlugs[i].IsBought && MonoSingleton<AppController>.Instance.Model.ShopModel.SpecialSlugs[i].Type == SlugType.Geoshard)
			{
				_buyGeoshard = 1;
				break;
			}
		}
		_buyPronto = 1;
		if (MonoSingleton<AppController>.Instance.Model.MissionSetting.Assistant.Type == AllyType.Pronto)
		{
			_ProntoEquip = 1;
			if (MonoSingleton<AppController>.Instance.Model.MissionSetting.Assistant.Level > 0)
			{
				_ProntoUpgrade = 1;
			}
		}
		if (_buyInfurnus == 1 && _InfurnusEquip == 1 && _InfurnusUpgrade == 1 && _buyArahnet == 1 && _ArahnetEquip == 1 && _ArahnetUpgrade == 1 && _buyGeoshard == 1 && _GeoshardEquip == 1 && _GeoshardUpgrade == 1 && _ProntoEquip == 1 && _ProntoUpgrade == 1)
		{
			ReadyStart();
		}
		StartCoroutine(HideHand(0f));
	}

	public void SlugShow(tk2dButton t)
	{
		if (_ShowInfurnus == 0)
		{
			StartCoroutine(HideHand(0f));
			offset.x = 1.38f;
			offset.y = -0.47906f;
			offset.z = 0f;
			StartCoroutine(ShowHand(btnExitSlugShow.transform, offset, Vector3.forward * 35f, 2f));
		}
	}

	public void SlugShowBack(tk2dButton t)
	{
		if (_ShowInfurnus == 0)
		{
			StartCoroutine(HideHand(0f));
			PlayerPrefs.SetInt("showInfurnusBackpack", 1);
			Recomendation.SetActive(false);
			ScrollableAreaRecommended.allowScrollWheel = false;
			ScrollableAreaRecommended.allowSwipeScrolling = false;
		}
		_ShowInfurnus = 1;
	}

	public void OnBtnStartClick(tk2dButton temp)
	{
		hand2d.SetActive(false);
		hand2dFlip.SetActive(false);
		PlayerPrefs.SetInt("Tutor_CutBackpackSceneComplete", 1);
	}

	public void ReadyStart()
	{
		hand2d.SetActive(false);
		hand2dFlip.SetActive(false);
		PlayerPrefs.SetInt("Tutor_CutBackpackSceneComplete", 1);
		StartCoroutine(Part_Start(0f));
		for (i = 0; i < btnMission.Length; i++)
		{
			btnMission[i].enabled = true;
		}
		for (i = 0; i < btnUIItem.Length; i++)
		{
			btnUIItem[i].enabled = true;
		}
		ScrollableAreaRecommended.allowScrollWheel = true;
		ScrollableAreaRecommended.allowSwipeScrolling = true;
		AppMoneyScrollable.allowSwipeScrolling = true;
		AppMoneyScrollable.allowScrollWheel = true;
		AppGemsScrollable.allowSwipeScrolling = true;
		AppGemsScrollable.allowScrollWheel = true;
		AppPodpiskaScrollable.allowSwipeScrolling = true;
		AppPodpiskaScrollable.allowScrollWheel = true;
		btnMoneyAppStore.enabled = true;
		btnPodpiskaAppStore.enabled = true;
		btnAlmazAppStore.enabled = true;
		btnOpenAppStore.enabled = true;
	}

	private void LateUpdate()
	{
		if (_buyAppStore == 0)
		{
			if (_ShowAppStore == 0 && AppStore.activeSelf)
			{
				_ShowAppStore = 1;
				StartCoroutine(HideHand(0f));
				StartCoroutine(Part_ViewMoneyApp(1f));
			}
			if (_ShowAppStore == 1 && !AppStore.activeSelf)
			{
				_ShowAppStore = 0;
				AppMoneyRoot.DOLocalMoveY(0f, 0f);
				PlayerPrefs.SetInt("buyAppStoreBackpack", 1);
				_buyAppStore = 1;
				StopAllCoroutines();
				AppMoneyScrollable.allowSwipeScrolling = true;
				AppMoneyScrollable.allowScrollWheel = true;
				AppGemsScrollable.allowSwipeScrolling = true;
				AppGemsScrollable.allowScrollWheel = true;
				AppPodpiskaScrollable.allowSwipeScrolling = true;
				AppPodpiskaScrollable.allowScrollWheel = true;
				btnOpenAppStore.enabled = false;
				btnAlmazAppStore.enabled = true;
				btnMoneyAppStore.enabled = true;
				btnPodpiskaAppStore.enabled = true;
				btnExitAppStore.enabled = true;
				StartCoroutine(HideHand(0f));
				StartCoroutine(Part_Item(0.55f));
			}
			if (btnPodpiskaClick && AppPodpiskaRoot.parent.gameObject.activeSelf)
			{
				btnPodpiskaClick = false;
				StartCoroutine(HideHand(0f));
				StartCoroutine(Part_ViewPodpiskaApp(0.5f));
			}
			if (btnAlmazClick && AppGemsRoot.parent.gameObject.activeSelf)
			{
				btnAlmazClick = false;
				StartCoroutine(HideHand(0f));
				StartCoroutine(Part_ViewGemApp(0.5f));
			}
		}
		if (SelectItems)
		{
			if (_ShowInfurnus == 0)
			{
				if (!Infurnus.IsInformation)
				{
					hand2d.SetActive(false);
					offset.x = -1.085f;
					offset.y = -0.465f;
					offset.z = 0f;
					MoveHandFlip(Infurnus.CurUIItem(0).transform, offset, Vector3.forward * 133f);
					hand2dFlip.SetActive(true);
					Infurnus.CurUIItem(0).enabled = true;
				}
				else if (SlugPriviwe.activeSelf)
				{
					hand2dFlip.SetActive(false);
					offset.x = 1.5195f;
					offset.y = 0.47906f;
					offset.z = 0f;
					Infurnus.CurUIItem(0).enabled = false;
					MoveHand(Infurnus.CurButton(4).transform, offset, Vector3.forward * 95f);
					hand2d.SetActive(true);
				}
				AreaRecommendedRoot.transform.localPosition = Vector3.MoveTowards(AreaRecommendedRoot.transform.localPosition, Vector3.left * 0.92f, Time.deltaTime * 8f);
			}
			else
			{
				if (_buyInfurnus == 0)
				{
					hand2d.SetActive(false);
					offset.x = -1.5f;
					offset.y = -0.41f;
					offset.z = 0f;
					MoveHandFlip(Infurnus.CurButton(1).transform, offset, Vector3.forward * 133f);
					hand2dFlip.SetActive(true);
					Infurnus.CurButton(1).enabled = true;
					AreaRecommendedRoot.transform.localPosition = Vector3.MoveTowards(AreaRecommendedRoot.transform.localPosition, Vector3.left * 0.92f, Time.deltaTime * 8f);
				}
				else if (_InfurnusEquip == 0 || _InfurnusUpgrade == 0)
				{
					hand2dFlip.SetActive(false);
					offset.x = 1.5195f;
					offset.y = 0.47906f;
					offset.z = 0f;
					MoveHand(Infurnus.CurButton(0).transform, offset, Vector3.forward * 95f);
					hand2d.SetActive(true);
					Infurnus.CurButton(0).enabled = true;
					Infurnus.CurButton(2).enabled = true;
					AreaRecommendedRoot.transform.localPosition = Vector3.MoveTowards(AreaRecommendedRoot.transform.localPosition, Vector3.left * 0.92f, Time.deltaTime * 8f);
				}
				if (_buyInfurnus == 1 && _InfurnusEquip == 1 && _InfurnusUpgrade == 1 && _buyInfurnus == 1 && _InfurnusEquip == 1 && _InfurnusUpgrade == 1)
				{
					if (_buyArahnet == 0)
					{
						hand2d.SetActive(false);
						offset.x = -1.5f;
						offset.y = -0.41f;
						offset.z = 0f;
						MoveHandFlip(Arahnet.CurButton(1).transform, offset, Vector3.forward * 133f);
						hand2dFlip.SetActive(true);
						Arahnet.CurButton(1).enabled = true;
						AreaRecommendedRoot.transform.localPosition = Vector3.MoveTowards(AreaRecommendedRoot.transform.localPosition, Vector3.left * 1.92f, Time.deltaTime * 8f);
					}
					else if (_ArahnetEquip == 0 || _ArahnetUpgrade == 0)
					{
						hand2dFlip.SetActive(false);
						offset.x = 1.5195f;
						offset.y = 0.47906f;
						offset.z = 0f;
						MoveHand(Arahnet.CurButton(0).transform, offset, Vector3.forward * 95f);
						hand2d.SetActive(true);
						Arahnet.CurButton(0).enabled = true;
						Arahnet.CurButton(2).enabled = true;
						AreaRecommendedRoot.transform.localPosition = Vector3.MoveTowards(AreaRecommendedRoot.transform.localPosition, Vector3.left * 1.92f, Time.deltaTime * 8f);
					}
				}
				if (_buyInfurnus == 1 && _InfurnusEquip == 1 && _InfurnusUpgrade == 1 && _buyArahnet == 1 && _ArahnetEquip == 1 && _ArahnetUpgrade == 1)
				{
					if (_buyGeoshard == 0)
					{
						hand2d.SetActive(false);
						offset.x = -1.5f;
						offset.y = -0.41f;
						offset.z = 0f;
						hand2dFlip.SetActive(true);
						MoveHandFlip(Geoshard.CurButton(1).transform, offset, Vector3.forward * 133f);
						Geoshard.CurButton(1).enabled = true;
						AreaRecommendedRoot.transform.localPosition = Vector3.MoveTowards(AreaRecommendedRoot.transform.localPosition, Vector3.left * 2.88f, Time.deltaTime * 8f);
					}
					else if (_GeoshardEquip == 0 || _GeoshardUpgrade == 0)
					{
						hand2dFlip.SetActive(false);
						offset.x = 1.5195f;
						offset.y = 0.47906f;
						offset.z = 0f;
						MoveHand(Geoshard.CurButton(0).transform, offset, Vector3.forward * 95f);
						hand2d.SetActive(true);
						Geoshard.CurButton(0).enabled = true;
						Geoshard.CurButton(2).enabled = true;
						AreaRecommendedRoot.transform.localPosition = Vector3.MoveTowards(AreaRecommendedRoot.transform.localPosition, Vector3.left * 2.88f, Time.deltaTime * 8f);
					}
				}
				if (_buyInfurnus == 1 && _InfurnusEquip == 1 && _InfurnusUpgrade == 1 && _buyArahnet == 1 && _ArahnetEquip == 1 && _ArahnetUpgrade == 1 && _buyGeoshard == 1 && _GeoshardEquip == 1 && _GeoshardUpgrade == 1 && _buyPronto == 1 && (_ProntoEquip == 0 || _ProntoUpgrade == 0))
				{
					hand2d.SetActive(false);
					offset.x = -1.5f;
					offset.y = -0.41f;
					offset.z = 0f;
					MoveHandFlip(Pronto.CurButton(2).transform, offset, Vector3.forward * 133f);
					hand2dFlip.SetActive(true);
					Pronto.CurButton(0).enabled = true;
					Pronto.CurButton(2).enabled = true;
					AreaRecommendedRoot.transform.localPosition = Vector3.MoveTowards(AreaRecommendedRoot.transform.localPosition, Vector3.left * 5.6f, Time.deltaTime * 8f);
				}
				if (_buyInfurnus == 1 && _InfurnusEquip == 1 && _InfurnusUpgrade == 1 && _buyArahnet == 1 && _ArahnetEquip == 1 && _ArahnetUpgrade == 1 && _buyGeoshard == 1 && _GeoshardEquip == 1 && _GeoshardUpgrade == 1 && _ProntoEquip == 1 && _ProntoUpgrade == 1)
				{
					SelectItems = false;
					ReadyStart();
				}
			}
		}
		if (_buyAppStore != 1)
		{
			return;
		}
		if (AppStore.activeSelf || Gifts.activeSelf || (!SlugPriviwe.activeSelf && Infurnus != null && !Infurnus.IsInformation) || (!SlugPriviwe.activeSelf && _ShowInfurnus == 1))
		{
			if (hand2d.GetComponent<MeshRenderer>().enabled)
			{
				hand2d.GetComponent<MeshRenderer>().enabled = false;
			}
			if (hand2dFlip.GetComponent<MeshRenderer>().enabled)
			{
				hand2dFlip.GetComponent<MeshRenderer>().enabled = false;
			}
			if (!SlugPriviwe.activeSelf)
			{
				timeShowHand = 1f;
			}
			return;
		}
		if (timeShowHand > 0f)
		{
			timeShowHand -= Time.deltaTime;
			if (timeShowHand < 0f)
			{
				timeShowHand = 0f;
			}
		}
		if (timeShowHand == 0f)
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
	}
}

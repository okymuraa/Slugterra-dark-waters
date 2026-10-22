using System.Collections;
using System.Collections.Generic;
using AppsMinistry.Core;
using DG.Tweening;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Scene.Game;
using Slugterra.UI.Popup;
using Slugterra.UI.Shops;
using UnityEngine;

public class CutScene_BackpackScene_Arena : MonoBehaviour
{
	private bool TutorialBackpackSceneComplete;

	private byte _goldAdd;

	[SerializeField]
	private int AddGold_med;

	[SerializeField]
	private int AddGold_Food;

	private int AddGold;

	public Transform[] text_gold;

	public GameObject Gifts;

	public GameObject AppStore;

	public GameObject Recomendation;

	public tk2dUIScrollableArea ScrollableAreaRevals;

	public tk2dUIScrollableArea ScrollableAreaRecommended;

	public ArenaItemController[] RevalsItem;

	public RecommendedItemController[] RecommendedItem;

	public Transform AreaRecommendedRoot;

	public Transform AreaRevalsRoot;

	private tk2dButton[] btnMission;

	private tk2dUIItem[] btnUIItem;

	public tk2dButton BtnStart;

	public tk2dButton BtnNext;

	private Transform btnBack;

	public GameObject hand2d;

	public GameObject hand2dFlip;

	private bool SelectRevals;

	private bool SelectItems;

	private ArenaItemView Revals;

	private RecommendedItemView MedKit;

	private int _buyMedKit;

	private RecommendedItemView SlugFood;

	private int _buySlugFood;

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

	private float dist;

	private void OnEnable()
	{
		if (MonoSingleton<AppController>.Instance.Model.MissionSetting.Mission != typeof(ScorchedSparksTutorial_1_1))
		{
			StopAllCoroutines();
			gameObject.SetActive(false);
		}
		else
		{
			AreaRevalsRoot.transform.localPosition = Vector3.left * 2.88f;
			AreaRecommendedRoot.transform.localPosition = Vector3.zero;
		}
	}

	private void OnDisable()
	{
		if (EventActive)
		{
			BtnStart.ButtonPressedEvent -= OnBtnStartClick;
			BtnNext.ButtonPressedEvent -= OnBtnNextClick;
			if (Revals != null)
			{
				Revals.CurButton(0).ButtonPressedEvent -= RavalEquip;
			}
			if (MedKit != null)
			{
				MedKit.CurButton(1).ButtonPressedEvent -= MedKidTap;
			}
			if (SlugFood != null)
			{
				SlugFood.CurButton(1).ButtonPressedEvent -= SlugFoodTap;
			}
		}
	}

	private void Awake()
	{
		if (MonoSingleton<AppController>.Instance.Model.MissionSetting.Mission != typeof(ScorchedSparksTutorial_1_1))
		{
			StopAllCoroutines();
			gameObject.SetActive(false);
		}
	}

	private void Start()
	{
		_goldAdd = (byte)PlayerPrefs.GetInt("bonusGoldBackpack_arena");
		_buyMedKit = (byte)PlayerPrefs.GetInt("bonusGoldBackpack_arena_medkid");
		_buySlugFood = (byte)PlayerPrefs.GetInt("bonusGoldBackpack_arena_slugfood");
		Recomendation.GetComponent<MoveAnimUI>().enabled = false;
		Recomendation.transform.localPosition = -Vector3.up;
		Recomendation.SetActive(false);
		AddGold_med = (int)MonoSingleton<GameConfiguration>.Instance.ConsumableConfigurationManager.GetConsumablePrice(ConsumableType.First, 5);
		AddGold_Food = (int)MonoSingleton<GameConfiguration>.Instance.ConsumableConfigurationManager.GetConsumablePrice(ConsumableType.First, 5);
		int num = PlayerPrefs.GetInt("Tutor_CutBackpackSceneComplete_arena");
		if (num == 1)
		{
			TutorialBackpackSceneComplete = true;
		}
		else
		{
			TutorialBackpackSceneComplete = false;
		}
		SelectRevals = true;
		StartCoroutine(SelectRevalsKord());
		hand2d.SetActive(false);
		hand2dFlip.SetActive(false);
		if (!TutorialBackpackSceneComplete)
		{
			black = Color.black;
			black.a = 0f;
			black2 = black;
			if (_buyMedKit == 1 && _buySlugFood == 1)
			{
				StartCoroutine(Part_Start());
			}
		}
	}

	public void StartScene(List<ArenaItemController> items)
	{
		RevalsItem = items.ToArray();
		if (MonoSingleton<AppController>.Instance.Model.MissionSetting.Mission != typeof(ScorchedSparksTutorial_1_1))
		{
			return;
		}
		for (i = 0; i < RevalsItem.Length; i++)
		{
			if (RevalsItem[i].View.ItemValue == EnemyType.BossKord)
			{
				Revals = RevalsItem[i].View;
				Revals.ItemViewType = ArenaItemViewType.NeedEquip;
				Revals.UpdateForTutor();
			}
			else
			{
				RevalsItem[i].View.CurButton(0).enabled = false;
			}
		}
	}

	private IEnumerator SelectRevalsKord()
	{
		yield return new WaitForSeconds(0.1f);
		for (i = 0; i < RevalsItem.Length; i++)
		{
			if (RevalsItem[i].View.ItemValue == EnemyType.BossKord)
			{
				Revals = RevalsItem[i].View;
				Revals.CurButtonUi(0).enabled = false;
				Revals.CurButton(0).ButtonPressedEvent += RavalEquip;
				if (Revals.ItemViewType == ArenaItemViewType.Acquired)
				{
					ScrollableAreaRecommended.allowSwipeScrolling = true;
					ScrollableAreaRecommended.allowScrollWheel = true;
					BtnNext.enabled = true;
				}
			}
		}
		BtnStart.ButtonPressedEvent += OnBtnStartClick;
		BtnNext.ButtonPressedEvent += OnBtnNextClick;
		EventActive = true;
	}

	public void StartBackpack(List<RecommendedItemController> items)
	{
		RecommendedItem = items.ToArray();
		if (RecommendedItem.Length > 0)
		{
			RecommendedItem[0].View.CurUIItem(0).enabled = false;
		}
	}

	private IEnumerator AfterNext()
	{
		yield return new WaitForSeconds(0.1f);
		StartCoroutine(MoveLocalX(AreaRecommendedRoot, 0f, 6f));
		btnMission = Object.FindObjectsOfType<tk2dButton>();
		btnUIItem = Object.FindObjectsOfType<tk2dUIItem>();
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
		for (i = 1; i < RecommendedItem.Length; i++)
		{
			if (RecommendedItem[i].View.ConsumableType == ConsumableType.First)
			{
				MedKit = RecommendedItem[i].View;
				MedKit.CurButton(1).ButtonPressedEvent += MedKidTap;
				if (MedKit.CurrentViewType == AnnounceItemViewType.Acquired || (_goldAdd == 1 && _buyMedKit == 0 && MonoSingleton<AppController>.Instance.Model.Wallet.Gold < (float)AddGold_med))
				{
					_buyMedKit = 1;
				}
				else
				{
					MedKit.CurUIItem(0).enabled = false;
				}
			}
			if (RecommendedItem[i].View.ConsumableType == ConsumableType.SlugFood)
			{
				SlugFood = RecommendedItem[i].View;
				SlugFood.CurButton(1).ButtonPressedEvent += SlugFoodTap;
				if (SlugFood.CurrentViewType == AnnounceItemViewType.Acquired || (_goldAdd == 1 && _buyMedKit == 0 && _buySlugFood == 0 && MonoSingleton<AppController>.Instance.Model.Wallet.Gold < (float)AddGold_Food) || (_buyMedKit == 1 && _buySlugFood == 0 && MonoSingleton<AppController>.Instance.Model.Wallet.Gold < (float)AddGold_Food))
				{
					_buySlugFood = 1;
				}
				else
				{
					SlugFood.CurUIItem(0).enabled = false;
				}
			}
		}
		RecommendedItem[0].View.ShowFakeGUISlug(SlugType.Infurnus, AnnounceItemViewType.Acquired, 0);
		RecommendedItem[0].View.CurUIItem(0).enabled = false;
		OnChekEquip();
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
		if (_buyMedKit == 1)
		{
			AddGold_med = 0;
		}
		if (_buySlugFood == 1)
		{
			AddGold_Food = 0;
		}
		AddGold = AddGold_med + AddGold_Food;
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
		PlayerPrefs.SetInt("bonusGoldBackpack_arena", 1);
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
	}

	private IEnumerator Part_Item(float weit)
	{
		StartCoroutine(AddMegaGoldPack(0.5f));
		yield return new WaitForSeconds(weit);
		SelectItems = true;
	}

	private IEnumerator Part_Start()
	{
		yield return new WaitForSeconds(1f);
		offset.x = -1.845f;
		offset.y = 0.7f;
		offset.z = 0f;
		MoveHandFlip(BtnStart.transform, offset, Vector3.forward * 82f);
		hand2dFlip.SetActive(true);
	}

	private void RavalEquip(tk2dButton t)
	{
		StartCoroutine(HideHand(0f));
		BtnNext.enabled = true;
	}

	private void MedKidTap(tk2dButton t)
	{
		_buyMedKit = 1;
		PlayerPrefs.SetInt("bonusGoldBackpack_arena_medkid", 1);
		OnChekEquip();
	}

	private void SlugFoodTap(tk2dButton t)
	{
		_buySlugFood = 1;
		PlayerPrefs.SetInt("bonusGoldBackpack_arena_slugfood", 1);
		OnChekEquip();
	}

	private void OnChekEquip()
	{
		if (_buyMedKit == 1 && _buySlugFood == 0 && MonoSingleton<AppController>.Instance.Model.Wallet.Gold < (float)AddGold_Food)
		{
			_buySlugFood = 1;
		}
		if (_buyMedKit == 1 && _buySlugFood == 1)
		{
			ReadyStart();
		}
		StartCoroutine(HideHand(0f));
	}

	public void OnBtnStartClick(tk2dButton temp)
	{
		hand2d.SetActive(false);
		hand2dFlip.SetActive(false);
		Recomendation.SetActive(false);
		PlayerPrefs.SetInt("Tutor_CutBackpackSceneComplete_arena", 1);
	}

	public void OnBtnNextClick(tk2dButton temp)
	{
		SelectRevals = false;
		hand2d.SetActive(false);
		hand2dFlip.SetActive(false);
		Recomendation.SetActive(false);
		StartCoroutine(AfterNext());
		StartCoroutine(Part_Item(0.2f));
	}

	public void ReadyStart()
	{
		hand2d.SetActive(false);
		hand2dFlip.SetActive(false);
		PlayerPrefs.SetInt("Tutor_CutBackpackSceneComplete_arena", 1);
		StartCoroutine(MoveLocalX(AreaRecommendedRoot, 0f, 6f));
		if (Revals != null)
		{
			Revals.transform.Find("btnInfo").GetComponent<tk2dUIItem>().enabled = true;
		}
		if (MedKit != null)
		{
			MedKit.transform.Find("btnInfo").GetComponent<tk2dUIItem>().enabled = true;
		}
		if (SlugFood != null)
		{
			SlugFood.transform.Find("btnInfo").GetComponent<tk2dUIItem>().enabled = true;
		}
		StartCoroutine(Part_Start());
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
		if (RecommendedItem.Length > 0)
		{
			RecommendedItem[0].View.CurUIItem(0).enabled = false;
		}
	}

	private void Update()
	{
		if (SelectRevals)
		{
			dist = Vector3.Distance(AreaRevalsRoot.transform.localPosition, Vector3.left * 2.88f);
		}
		if (SelectRevals && dist > 4f)
		{
			ScrollableAreaRevals.enabled = false;
			ScrollableAreaRevals.allowSwipeScrolling = false;
			ScrollableAreaRevals.allowScrollWheel = false;
			AreaRevalsRoot.transform.localPosition = Vector3.MoveTowards(AreaRevalsRoot.transform.localPosition, Vector3.left * 2.88f, Time.deltaTime * 8f);
		}
		else if (dist < 1f)
		{
			ScrollableAreaRevals.enabled = true;
			ScrollableAreaRevals.allowSwipeScrolling = true;
			ScrollableAreaRevals.allowScrollWheel = true;
		}
		else
		{
			AreaRevalsRoot.transform.localPosition = Vector3.MoveTowards(AreaRevalsRoot.transform.localPosition, Vector3.left * 2.88f, Time.deltaTime * 8f);
		}
	}

	private void LateUpdate()
	{
		if (SelectRevals && Revals != null)
		{
			if (Revals.ItemViewType == ArenaItemViewType.NeedEquip)
			{
				BtnNext.enabled = false;
				hand2d.SetActive(false);
				offset.x = -1.5f;
				offset.y = -0.41f;
				offset.z = 0f;
				MoveHandFlip(Revals.CurButton(0).transform, offset, Vector3.forward * 133f);
				hand2dFlip.SetActive(true);
				Revals.CurButton(0).enabled = true;
			}
			else
			{
				BtnNext.enabled = true;
				hand2d.SetActive(false);
				offset.x = -1.845f;
				offset.y = 0.7f;
				offset.z = 0f;
				MoveHandFlip(BtnNext.transform, offset, Vector3.forward * 82f);
				hand2dFlip.SetActive(true);
				Revals.CurButton(0).enabled = true;
			}
		}
		if (SelectItems && !SelectRevals && MedKit != null && SlugFood != null)
		{
			if (_buyMedKit == 0)
			{
				hand2d.SetActive(false);
				offset.x = -1.5f;
				offset.y = -0.41f;
				offset.z = 0f;
				MoveHandFlip(MedKit.CurButton(1).transform, offset, Vector3.forward * 133f);
				hand2dFlip.SetActive(true);
				MedKit.CurButton(1).enabled = true;
				AreaRecommendedRoot.transform.localPosition = Vector3.MoveTowards(AreaRecommendedRoot.transform.localPosition, Vector3.zero, Time.deltaTime * 8f);
			}
			if (_buyMedKit == 1 && _buySlugFood == 0)
			{
				hand2d.SetActive(false);
				offset.x = -1.5f;
				offset.y = -0.41f;
				offset.z = 0f;
				MoveHandFlip(SlugFood.CurButton(1).transform, offset, Vector3.forward * 133f);
				hand2dFlip.SetActive(true);
				SlugFood.CurButton(1).enabled = true;
				AreaRecommendedRoot.transform.localPosition = Vector3.MoveTowards(AreaRecommendedRoot.transform.localPosition, Vector3.zero, Time.deltaTime * 8f);
			}
			if (_buyMedKit == 1 && _buySlugFood == 1)
			{
				SelectItems = false;
				ReadyStart();
			}
		}
		if (SelectItems)
		{
			return;
		}
		if (AppStore.activeSelf || Gifts.activeSelf)
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
	}
}

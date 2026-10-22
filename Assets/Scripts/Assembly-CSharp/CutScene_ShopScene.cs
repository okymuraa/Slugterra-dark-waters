using System;
using System.Collections;
using AppsMinistry.Core;
using DG.Tweening;
using Slugterra.App;
using Slugterra.Scene.Game;
using Slugterra.UI.Shops;
using UnityEngine;

public class CutScene_ShopScene : MonoBehaviour
{
	public static bool TutorialShopSceneComplete;

	private int _goldAdd;

	private int useFood;

	private int useHealth;

	public GameObject EquipStore;

	public tk2dUIItem BtnStart;

	public tk2dUIItem btnBoosts;

	public tk2dUIItem OnBuySmallPack;

	public tk2dUIItem OnBuyLargePack;

	public GameObject AppStore;

	[SerializeField]
	private int AddGold;

	public Transform text_gold;

	public Transform[] ConsoleLeft;

	public Transform[] BtnLeft;

	public Transform[] ConsoleRight;

	public GameObject[] Arrows;

	public GameObject Ico_Health;

	public GameObject Ico_Food;

	private tk2dButton[] btnMission;

	private tk2dUIItem[] btnUIItem;

	private Transform btnBack;

	public GameObject hand2d;

	public GameObject hand2dFlip;

	public Transform ScrollView;

	public float minY = 0.8f;

	public float maxY = 0.8f;

	private Vector3 offset;

	private GameObject prfConsumeItem_Health;

	private bool ItemsHealthUse;

	private GameObject prfConsumeItem_Food;

	private bool ItemsFoodUse;

	private bool btnBoostsOn;

	private bool QuestHealth;

	private bool QuestFood;

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

	private bool BoostsTap;

	private void OnDisable()
	{
		OnBuySmallPack.OnClick -= OnBuySmallPackClick;
		BtnStart.OnClick -= OnBtnStartClick;
	}

	private void Awake()
	{
		if (MonoSingleton<AppController>.Instance.Model.MissionSetting.Mission != typeof(ScorchedSparksTutorial_1_1))
		{
			gameObject.SetActive(false);
		}
		else if (TutorialShopSceneComplete)
		{
		}
	}

	private void Start()
	{
		_goldAdd = PlayerPrefs.GetInt("bonusGold");
		useFood = PlayerPrefs.GetInt("useFood");
		useHealth = PlayerPrefs.GetInt("useHealth");
		hand2d.SetActive(false);
		hand2dFlip.SetActive(false);
		if (!TutorialShopSceneComplete)
		{
			black = Color.black;
			black.a = 0f;
			black2 = black;
			AppStore.transform.position -= Vector3.forward;
			for (i = 0; i < ConsoleLeft.Length; i++)
			{
				ConsoleLeft[i].position -= Vector3.forward * 3f;
			}
			text_gold = GameObject.Find("lblCountCoins").transform;
			BtnStart.OnClick += OnBtnStartClick;
			StartCoroutine("AfterStart");
			if (useFood != 0 && useHealth != 0)
			{
				indexShadow = 1;
				BoostsTap = true;
				StartCoroutine(Part_fix(0.5f));
			}
			else
			{
				indexShadow = 1;
				StartCoroutine(Part_0_0(0.5f));
			}
		}
	}

	private IEnumerator AfterStart()
	{
		yield return new WaitForSeconds(0.1f);
		btnMission = UnityEngine.Object.FindObjectsOfType<tk2dButton>();
		btnUIItem = UnityEngine.Object.FindObjectsOfType<tk2dUIItem>();
		for (i = 0; i < btnMission.Length; i++)
		{
			if (btnMission[i].transform.parent.name != "btnBack")
			{
				btnMission[i].enabled = false;
				if ((bool)btnMission[i].GetComponent<tk2dUIItem>())
				{
					btnMission[i].GetComponent<tk2dUIItem>().enabled = false;
				}
			}
			else
			{
				btnBack = btnMission[i].transform.parent;
				btnBack.transform.position -= Vector3.forward;
			}
		}
		OnBuyLargePack.enabled = false;
		for (i = 0; i < btnUIItem.Length; i++)
		{
			if (btnUIItem[i].transform.parent.name != "btnBack")
			{
				btnUIItem[i].enabled = false;
			}
		}
		ScrollOff();
	}

	private IEnumerator ShowHand(Transform obj, Vector3 offset, Vector3 rotation, float weit)
	{
		yield return new WaitForSeconds(weit);
		hand2d.transform.position = obj.position + offset;
		hand2d.transform.eulerAngles = rotation;
		hand2d.SetActive(true);
	}

	private IEnumerator ShowHandFlip(Transform obj, Vector3 offset, Vector3 rotation, float weit)
	{
		yield return new WaitForSeconds(weit);
		hand2dFlip.transform.position = obj.position + offset;
		hand2dFlip.transform.eulerAngles = rotation;
		hand2dFlip.SetActive(true);
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

	private IEnumerator Part_0_0(float weit)
	{
		yield return new WaitForSeconds(weit);
		StartCoroutine(ScaleUp(BtnLeft[0], 1.2f, 0f));
		StartCoroutine(ScaleDown(BtnLeft[0], 1f, 0.5f));
		StartCoroutine(ScaleUp(BtnLeft[1], 1.2f, 0.3f));
		StartCoroutine(ScaleDown(BtnLeft[1], 1f, 0.8f));
		StartCoroutine(ScaleUp(BtnLeft[2], 1.2f, 0.6f));
		StartCoroutine(ScaleDown(BtnLeft[2], 1f, 1.1f));
		StartCoroutine(ScaleUp(BtnLeft[3], 1.2f, 0.9f));
		StartCoroutine(ScaleDown(BtnLeft[3], 1f, 1.4f));
		StartCoroutine(ScaleUp(BtnLeft[4], 1.2f, 1.2f));
		StartCoroutine(ScaleDown(BtnLeft[4], 1f, 1.7f));
		StartCoroutine(ScaleUp(btnBoosts.transform, 1.2f, 1.5f));
		StartCoroutine(ScaleUp(btnBoosts.transform, 1f, 2f));
		StartCoroutine(Part_0_0_1(2.1f));
	}

	private IEnumerator Part_0_0_1(float weit)
	{
		yield return new WaitForSeconds(weit);
		btnBoosts.enabled = true;
		offset.x = 1.555f;
		offset.y = -0.53978f;
		offset.z = 0f;
		StartCoroutine(ShowHand(btnBoosts.transform, offset, Vector3.forward * 35f, 0.1f));
	}

	private IEnumerator Part_0_1_0(float weit)
	{
		yield return new WaitForSeconds(weit);
		if (_goldAdd == 0)
		{
			StartCoroutine(ScaleUp(text_gold, 3f, 0f));
			StartCoroutine(ScaleDown(text_gold, 1f, 0.75f));
			SetGold();
		}
		indexShadow_2 = 1;
	}

	private IEnumerator Part_0_1_0_0(float weit)
	{
		yield return new WaitForSeconds(weit);
		for (i = 0; i < Arrows.Length; i++)
		{
			Arrows[i].SetActive(false);
		}
	}

	private IEnumerator Part_fix(float weit)
	{
		yield return new WaitForSeconds(weit);
		QuestFood = true;
		StartCoroutine(HideHand(0f));
		offset.x = 1.12f;
		offset.y = -0.93f;
		offset.z = 0f;
		ReadyStart();
		StartCoroutine(ShowHand(BtnStart.transform, offset, Vector3.forward * 23f, 0.1f));
	}

	public void Tap(Type tabType)
	{
		if (!BoostsTap && tabType == typeof(BoostsTabController))
		{
			OnBtnBoostsClick();
		}
	}

	public void OnBtnBoostsClick()
	{
		BoostsTap = true;
		btnBoosts.enabled = false;
		OnBuySmallPack.OnClick += OnBuySmallPackClick;
		StartCoroutine(HideHand(0f));
		prfConsumeItem_Health = GameObject.Find("prfConsumeItem_Health(Clone)");
		prfConsumeItem_Food = GameObject.Find("prfConsumeItem_SlugFood(Clone)");
		for (i = 0; i < ConsoleRight.Length; i++)
		{
			ConsoleRight[i].position -= Vector3.forward * 3f;
		}
		if (useHealth == 0)
		{
			QuestHealth = false;
		}
		else
		{
			QuestHealth = true;
		}
		indexShadow = 2;
		btnBoostsOn = true;
		StartCoroutine(Part_0_1_0(0.1f));
		if (useHealth == 0)
		{
			StartCoroutine(Part_0_1_0_0(0f));
			return;
		}
		OnBuySmallPack.enabled = false;
		StartCoroutine(HideHand(0f));
		offset.x = 0.659f;
		offset.y = -0.8964f;
		offset.z = 0f;
		StartCoroutine(ShowHand(Arrows[0].transform, offset, Vector3.zero, 0.3f));
		for (i = 0; i < Arrows.Length; i++)
		{
			Arrows[i].SetActive(true);
		}
	}

	public void OnBuySmallPackClick()
	{
		if (!QuestFood && QuestHealth)
		{
			OnBuySmallPack.enabled = false;
			QuestFood = true;
			StartCoroutine(HideHand(0f));
			useFood = 1;
			PlayerPrefs.SetInt("useFood", 1);
			StartCoroutine(ScaleUp(Ico_Food.transform, 1.2f, 0f));
			StartCoroutine(ScaleDown(Ico_Food.transform, 1f, 0.5f));
			indexShadow_2 = 2;
			offset.x = 1.12f;
			offset.y = -0.93f;
			offset.z = 0f;
			ReadyStart();
			StartCoroutine(ShowHand(BtnStart.transform, offset, Vector3.forward * 23f, 0.5f));
		}
		if (!QuestHealth && !QuestFood)
		{
			OnBuySmallPack.enabled = false;
			QuestHealth = true;
			useHealth = 1;
			PlayerPrefs.SetInt("useHealth", 1);
			StartCoroutine(ScaleUp(Ico_Health.transform, 1.2f, 0f));
			StartCoroutine(ScaleDown(Ico_Health.transform, 1f, 0.5f));
			StartCoroutine(HideHand(0f));
			offset.x = 0.659f;
			offset.y = -0.8964f;
			offset.z = 0f;
			StartCoroutine(ShowHand(Arrows[0].transform, offset, Vector3.zero, 0.3f));
			for (i = 0; i < Arrows.Length; i++)
			{
				Arrows[i].SetActive(true);
			}
		}
	}

	public void OnBtnStartClick()
	{
		ScrollOn();
		StartCoroutine(HideHand(0f));
	}

	private void ScrollOff()
	{
	}

	private void ScrollOn()
	{
	}

	private void SetGold()
	{
		MonoSingleton<AppController>.Instance.Model.Wallet.Gold += AddGold;
		_goldAdd = 1;
		PlayerPrefs.SetInt("bonusGold", 1);
	}

	public void ReadyStart()
	{
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
		OnBuyLargePack.enabled = true;
		OnBuySmallPack.enabled = true;
		offset.x = 1.12f;
		offset.y = -0.93f;
		offset.z = 0f;
	}

	private void LateUpdate()
	{
		if (!btnBoostsOn)
		{
			return;
		}
		if (AppStore.activeSelf)
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
		if (!QuestHealth)
		{
			if (Vector3.Distance(ScrollView.position - Vector3.up * 0.7f, prfConsumeItem_Health.transform.position) < minY)
			{
				if (!ItemsHealthUse)
				{
					hand2d.SetActive(false);
					ItemsHealthUse = true;
					OnBuySmallPack.enabled = true;
					offset.x = -1.5f;
					offset.y = -0.41f;
					offset.z = 0f;
					StartCoroutine(ShowHandFlip(OnBuySmallPack.transform, offset, Vector3.forward * 133f, 0f));
				}
			}
			else
			{
				if (Vector3.Distance(ScrollView.position - Vector3.up * 0.7f, prfConsumeItem_Health.transform.position) > maxY)
				{
					hand2d.SetActive(true);
					hand2dFlip.SetActive(false);
					if (ScrollView.position.y - prfConsumeItem_Health.transform.position.y > 0f)
					{
						offset.x = 0.659f;
						offset.y = -0.8964f;
						offset.z = 0f;
						StartCoroutine(ShowHand(Arrows[0].transform, offset, Vector3.zero, 0f));
					}
					else
					{
						offset.x = 0.659f;
						offset.y = -0.8964f;
						offset.z = 0f;
						StartCoroutine(ShowHand(Arrows[1].transform, offset, Vector3.zero, 0f));
					}
				}
				else
				{
					hand2d.SetActive(false);
				}
				if (ItemsHealthUse)
				{
					ItemsHealthUse = false;
					OnBuySmallPack.enabled = false;
				}
			}
		}
		if (!QuestHealth || QuestFood)
		{
			return;
		}
		if (Vector3.Distance(ScrollView.position - Vector3.up * 0.7f, prfConsumeItem_Food.transform.position) < minY)
		{
			if (!ItemsFoodUse)
			{
				hand2d.SetActive(false);
				ItemsFoodUse = true;
				OnBuySmallPack.enabled = true;
				for (i = 0; i < Arrows.Length; i++)
				{
					Arrows[i].SetActive(false);
				}
				offset.x = -1.5f;
				offset.y = -0.41f;
				offset.z = 0f;
				StartCoroutine(ShowHandFlip(OnBuySmallPack.transform, offset, Vector3.forward * 133f, 0f));
			}
			return;
		}
		if (Vector3.Distance(ScrollView.position - Vector3.up * 0.7f, prfConsumeItem_Food.transform.position) > maxY)
		{
			hand2d.SetActive(true);
			hand2dFlip.SetActive(false);
			if (ScrollView.position.y - prfConsumeItem_Food.transform.position.y > 0f)
			{
				offset.x = 0.659f;
				offset.y = -0.8964f;
				offset.z = 0f;
				StartCoroutine(ShowHand(Arrows[0].transform, offset, Vector3.zero, 0f));
			}
			else
			{
				offset.x = 0.659f;
				offset.y = -0.8964f;
				offset.z = 0f;
				StartCoroutine(ShowHand(Arrows[1].transform, offset, Vector3.zero, 0f));
			}
		}
		else
		{
			hand2d.SetActive(false);
		}
		if (ItemsFoodUse)
		{
			ItemsFoodUse = false;
			OnBuySmallPack.enabled = false;
			StartCoroutine(HideHand(0f));
		}
	}
}

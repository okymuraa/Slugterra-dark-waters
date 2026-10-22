using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Slugterra.UI.Survival;
using UnityEngine;

public class CutScene_BackpackScene_Survival : MonoBehaviour
{
	private bool TutorialSurvivalSceneComplete;

	public GameObject Gifts;

	public GameObject AppStore;

	public tk2dUIScrollableArea ScrollableAreaLevelTutor;

	public SurvivalMapItemView[] LevelTutorItem;

	public Transform AreaLevelTutorRoot;

	private tk2dButton[] btnMission;

	private tk2dUIItem[] btnUIItem;

	public tk2dButton BtnStart;

	private Transform btnBack;

	public GameObject hand2d;

	public GameObject hand2dFlip;

	private bool SelectLevelTutor;

	private SurvivalMapItemView LevelTutor;

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

	private int r;

	private bool EventActive;

	private bool SelectMap;

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
		if (EventActive)
		{
			BtnStart.ButtonPressedEvent -= OnBtnStartClick;
			if (LevelTutor != null)
			{
				LevelTutor.CurButton(0).ButtonPressedEvent -= RavalEquip;
			}
		}
	}

	private void Awake()
	{
	}

	private void Start()
	{
		int num = PlayerPrefs.GetInt("Tutor_CutSurvivalComplete");
		num = 0;
		if (num == 1)
		{
			TutorialSurvivalSceneComplete = true;
		}
		else
		{
			TutorialSurvivalSceneComplete = false;
		}
		SelectLevelTutor = true;
		hand2d.SetActive(false);
		hand2dFlip.SetActive(false);
		if (!TutorialSurvivalSceneComplete)
		{
			black = Color.black;
			black.a = 0f;
			black2 = black;
		}
	}

	public void StartScene(List<SurvivalMapItemView> items)
	{
		LevelTutorItem = items.ToArray();
		for (i = 0; i < LevelTutorItem.Length; i++)
		{
			if (LevelTutorItem[i].PlayLevel())
			{
				TutorialSurvivalSceneComplete = true;
				return;
			}
		}
		r = Random.Range(0, 3);
		for (i = 0; i < LevelTutorItem.Length; i++)
		{
			if (i == r)
			{
				LevelTutor = LevelTutorItem[i];
			}
			else
			{
				LevelTutorItem[i].CurButton(0).enabled = false;
			}
		}
		if (!TutorialSurvivalSceneComplete)
		{
			StartCoroutine(SelectLevelTutorKord());
		}
	}

	private IEnumerator SelectLevelTutorKord()
	{
		yield return new WaitForSeconds(0.1f);
		for (i = 0; i < LevelTutorItem.Length; i++)
		{
			if (i == r)
			{
				LevelTutor = LevelTutorItem[i];
				LevelTutor.CurButton(0).ButtonPressedEvent += RavalEquip;
			}
		}
		BtnStart.ButtonPressedEvent += OnBtnStartClick;
		EventActive = true;
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
		SelectMap = true;
		StartCoroutine(HideHand(0f));
		BtnStart.enabled = true;
	}

	public void OnBtnStartClick(tk2dButton temp)
	{
		SelectLevelTutor = false;
		hand2d.SetActive(false);
		hand2dFlip.SetActive(false);
		PlayerPrefs.SetInt("Tutor_CutSurvivalComplete", 1);
	}

	public void ReadyStart()
	{
		hand2d.SetActive(false);
		hand2dFlip.SetActive(false);
		PlayerPrefs.SetInt("Tutor_CutSurvivalComplete", 1);
		StartCoroutine(Part_Start());
		for (i = 0; i < btnMission.Length; i++)
		{
			btnMission[i].enabled = true;
		}
		for (i = 0; i < btnUIItem.Length; i++)
		{
			btnUIItem[i].enabled = true;
		}
	}

	private void LateUpdate()
	{
		if (SelectLevelTutor && LevelTutor != null)
		{
			if (!SelectMap)
			{
				BtnStart.enabled = false;
				if (r == 2)
				{
					hand2d.SetActive(false);
				}
				else
				{
					hand2dFlip.SetActive(false);
				}
				if (r == 2)
				{
					offset.x = -1.5f;
					offset.y = -0.41f;
					offset.z = 0f;
					MoveHandFlip(LevelTutor.CurButton(0).transform, offset, Vector3.forward * 133f);
					hand2dFlip.SetActive(true);
				}
				else
				{
					offset.x = 1.5195f;
					offset.y = 0.47906f;
					offset.z = 0f;
					MoveHand(LevelTutor.CurButton(0).transform, offset, Vector3.forward * 95f);
					hand2d.SetActive(true);
				}
				LevelTutor.CurButton(0).enabled = true;
			}
			else
			{
				BtnStart.enabled = true;
				hand2d.SetActive(false);
				offset.x = -1.845f;
				offset.y = 0.7f;
				offset.z = 0f;
				MoveHandFlip(BtnStart.transform, offset, Vector3.forward * 82f);
				hand2dFlip.SetActive(true);
				LevelTutor.CurButton(0).enabled = true;
			}
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

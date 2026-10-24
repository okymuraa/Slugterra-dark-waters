using System.Collections.Generic;
using AppsMinistry.Core;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.Game.UI;
using Slugterra.Domain.Shops;
using Slugterra.Game.Units.Ally;
using Slugterra.Game.Units.Enemy;
using Slugterra.Scene.Game;
using Slugterra.UI.Game;
using UnityEngine;

public class CutScene_2_1_Tutor : MonoBehaviour
{
	[SerializeField]
	private TutorHelpInput Help;

	private TutorHelpInput[] HelpArray;

	private int i;

	public GameObject PopupConfirm;

	public GameObject InAppStore;

	private Collider[] AllTerrain;

	private List<GameObject> terraain = new List<GameObject>();

	private List<GameObject> probs = new List<GameObject>();

	private List<GameObject> touch = new List<GameObject>();

	[SerializeField]
	private SlugButtonView _ButtonTemp0;

	[SerializeField]
	private GameObject _ButtonTemp0_cd;

	public CapsuleCollider TouchBot;

	private EliController _eliController;

	public Transform pointShoot;

	[SerializeField]
	private EnemyController targetBot;

	public EnemyController[] tempBot;

	public GameObject[] SlugFireHelp;

	private float timer1;

	private float timer2;

	private bool needUseBoost;

	private bool tutorStart;

	private List<ConsumableItem> items = new List<ConsumableItem>();

	public GameObject HiLightHealth;

	private bool getsgug;

	private bool selectsgug;

	private bool fireReagy;

	private bool cd;

	private bool nextboost;

	private bool tapboost;

	private void Start()
	{
		_eliController = Object.FindObjectOfType<EliController>();
		_ButtonTemp0.OnUseEvent += SelectSlug;
		_ButtonTemp0.OnEndEvent += FireSlug;
		if (MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(QuietLawn_2_1)).Status == LevelStatus.Completed)
		{
			gameObject.SetActive(false);
		}
		HelpArray = Help.GetComponents<TutorHelpInput>();
	}

	private void OnDisable()
	{
		_ButtonTemp0.OnUseEvent -= SelectSlug;
		_ButtonTemp0.OnEndEvent -= FireSlug;
	}

	private void SelectSlug()
	{
		selectsgug = true;
	}

	private void FireSlug()
	{
		selectsgug = false;
	}

	private void CanMove(bool type)
	{
		for (int i = 0; i < terraain.Count; i++)
		{
			if (type)
			{
				if (terraain[i] != null)
				{
					terraain[i].layer = LayerMask.NameToLayer("Terrain");
				}
			}
			else if (terraain[i] != null)
			{
				terraain[i].layer = LayerMask.NameToLayer("Default");
			}
		}
		for (int j = 0; j < probs.Count; j++)
		{
			if (type)
			{
				if (probs[j] != null)
				{
					probs[j].layer = LayerMask.NameToLayer("PropLayer");
				}
			}
			else if (probs[j] != null)
			{
				probs[j].layer = LayerMask.NameToLayer("UnitsConstraints");
			}
		}
		for (int k = 0; k < touch.Count; k++)
		{
			if (type)
			{
				if (touch[k] != null)
				{
					touch[k].layer = LayerMask.NameToLayer("TouchLayer");
				}
			}
			else if (touch[k] != null)
			{
				touch[k].layer = LayerMask.NameToLayer("CoverLayer");
			}
		}
	}

	private void FixedUpdate()
	{
		timer1 += Time.deltaTime;
		timer2 += Time.deltaTime;
		if (_ButtonTemp0_cd.activeSelf)
		{
			cd = true;
		}
		else if (cd)
		{
			cd = false;
		}
		if (timer1 > 0.25f)
		{
			if (_eliController != null && _eliController.NextSlug != null && _eliController.NextSlug.Type != SlugType.Flopper)
			{
				selectsgug = true;
			}
			if (targetBot != null)
			{
				if (!selectsgug && !cd && Vector3.Magnitude(targetBot.transform.position - _eliController.transform.position) < 16f)
				{
					if (!getsgug)
					{
						getsgug = true;
						for (i = 0; i < HelpArray.Length; i++)
						{
							HelpArray[i].ShowHelp("selectslug", null, Vector3.zero);
						}
					}
				}
				else if (getsgug)
				{
					getsgug = false;
					for (i = 0; i < HelpArray.Length; i++)
					{
						HelpArray[i].DisableHelp("selectslug");
					}
				}
				if (selectsgug && !cd && Vector3.Magnitude(targetBot.transform.position - _eliController.transform.position) < 16f && targetBot != null && targetBot.AttackTarget == null)
				{
					if (!fireReagy)
					{
						fireReagy = true;
						for (i = 0; i < SlugFireHelp.Length; i++)
						{
							if (SlugFireHelp[i] != null)
							{
								SlugFireHelp[i].SetActive(true);
							}
						}
					}
				}
				else if (fireReagy)
				{
					fireReagy = false;
					for (i = 0; i < SlugFireHelp.Length; i++)
					{
						if (SlugFireHelp[i] != null)
						{
							SlugFireHelp[i].SetActive(false);
						}
					}
				}
			}
			else if (getsgug)
			{
				getsgug = false;
				for (i = 0; i < HelpArray.Length; i++)
				{
					HelpArray[i].DisableHelp("selectslug");
				}
			}
			timer1 = 0f;
		}
		if (!(timer2 > 20f))
		{
			return;
		}
		if (!needUseBoost && _eliController.Model.Health < _eliController.Model.MaxHealth * 0.4f)
		{
			items = MonoSingleton<AppController>.Instance.Model.ShopModel.Consumables;
			foreach (ConsumableItem item in items)
			{
				if (item.Type == ConsumableType.First && item.Count > 0)
				{
					needUseBoost = true;
					for (i = 0; i < HelpArray.Length; i++)
					{
						HelpArray[i].DelHelp("selectslug");
					}
					for (i = 0; i < HelpArray.Length; i++)
					{
						HelpArray[i].ShowHelp("useboost", null, Vector3.zero);
					}
					return;
				}
			}
		}
		if (needUseBoost && _eliController.Model.Health > _eliController.Model.MaxHealth * 0.4f)
		{
			needUseBoost = false;
			for (i = 0; i < HelpArray.Length; i++)
			{
				HelpArray[i].DisableHelp("useboost");
			}
			timer2 = 0f;
		}
	}

	private void LateUpdate()
	{
		if (PopupConfirm.activeSelf || InAppStore.activeSelf || Time.timeScale == 0f)
		{
			for (i = 0; i < HelpArray.Length; i++)
			{
				HelpArray[i].HideLayer();
			}
		}
		else
		{
			for (i = 0; i < HelpArray.Length; i++)
			{
				HelpArray[i].ShowLayer();
			}
		}
	}
}

using System.Collections;
using AppsMinistry.Core;
using AppsMinistry.Scene;
using Slugterra.Game.Mission;
using Slugterra.Game.Units.Enemy;
using Slugterra.Game.Units.Mechanisms;
using Slugterra.UI.Game;
using UnityEngine;

namespace Slugterra.Scene.Game
{
	[SceneName("3_1_DarkPeriphery", "", false, false)]
	public class DarkPeriphery_3_1 : SlugterraSceneController
	{
		[SerializeField]
		private GameObject _trailer_1;

		[SerializeField]
		private GameObject _trailer_2;

		[SerializeField]
		private GameObject _trailer_3;

		[SerializeField]
		private GameObject _arrow_1;

		[SerializeField]
		private GameObject _arrow_2;

		[SerializeField]
		private GameObject _arrow_3;

		[SerializeField]
		private ObjectiveTarget _objective_Win_AB_1;

		[SerializeField]
		private ObjectiveTarget _objective_Win_AB_2;

		[SerializeField]
		private ObjectiveTarget _objective_Win_AB_3;

		[SerializeField]
		private ObjectiveTarget _objective_AB_2_Start;

		[SerializeField]
		private ObjectiveTarget _objective_AB_3_Start;

		[SerializeField]
		private ObjectiveTarget _objective_Loose_AB_1;

		[SerializeField]
		private ObjectiveTarget _objective_Loose_AB_2;

		[SerializeField]
		private ObjectiveTarget _objective_Loose_AB_3;

		[SerializeField]
		private GameObject _sentinels_AB_4;

		[SerializeField]
		private ScrapHeapController Mob_1;

		[SerializeField]
		private ScrapHeapController AllyMob_1;

		[SerializeField]
		private ScrapHeapController Mob_2;

		[SerializeField]
		private ScrapHeapController AllyMob_2;

		[SerializeField]
		private ScrapHeapController Mob_3;

		[SerializeField]
		private ScrapHeapController AllyMob_3;

		[SerializeField]
		private ScrapHeapController Mob_4;

		[SerializeField]
		private ScrapHeapController AllyMob_4;

		[SerializeField]
		private ScrapHeapController Mob_5;

		[SerializeField]
		private ScrapHeapController AllyMob_5;

		[SerializeField]
		private ScrapHeapController Mob_6;

		[SerializeField]
		private ScrapHeapController AllyMob_6;

		[SerializeField]
		private ObjectiveTarget _enabled_AB1;

		[SerializeField]
		private ObjectiveTarget _enabled_AB2;

		[SerializeField]
		private ObjectiveTarget _enabled_AB3;

		[SerializeField]
		private ObjectiveTarget _activ_Enabled_AB1_2;

		[SerializeField]
		private ObjectiveTarget _activ_Enabled_AB2_2;

		[SerializeField]
		private ObjectiveTarget _activ_Enabled_AB3;

		public GameObject _AB1_2;

		public GameObject _holder_AB12;

		public GameObject _AB2_2;

		public GameObject _holder_AB23;

		public GameObject _AB3;

		public GameObject _holder_AB34;

		public GameObject _CutScene_On_AB_3_End;

		public float _timer_CutScene_On_AB_3_End = 1f;

		private InGameView _inGameView;

		private CountdownWarningMessageView _countdownMessage;

		private void Start()
		{
			_inGameView = Object.FindObjectOfType<InGameView>();
			_countdownMessage = _inGameView.CountdownWarningMessage;
			ItemBlinking[] componentsInChildren = _trailer_1.GetComponentsInChildren<ItemBlinking>();
			foreach (ItemBlinking itemBlinking in componentsInChildren)
			{
				itemBlinking.Play();
			}
			DonDestroyMonoSingleton<GameServiceContext>.Instance.ReportAchievment("CgkIjZj4j6kZEAIQEA");
		}

		private void OnEnable()
		{
			_objective_Win_AB_1.OnObjectiveCompleteEvent += OnObjective_AB_1_Complete;
			_objective_Win_AB_2.OnObjectiveCompleteEvent += OnObjective_AB_2_Complete;
			_objective_Win_AB_3.OnObjectiveCompleteEvent += OnObjective_AB_3_Complete;
			_objective_AB_3_Start.OnObjectiveSetActiveEvent += EnableCutSceneAI_AB_3;
			_enabled_AB1.OnObjectiveCompleteEvent += EnableCutSceneAI_AB_1;
			_enabled_AB2.OnObjectiveCompleteEvent += EnableCutSceneAI_AB_2;
			_enabled_AB3.OnObjectiveCompleteEvent += EnableCutSceneAI_AB_3;
			_activ_Enabled_AB3.OnObjectiveCompleteEvent += ActiveEnabled_AB3;
			_activ_Enabled_AB2_2.OnObjectiveCompleteEvent += ActiveEnabled_AB2_2;
			_activ_Enabled_AB1_2.OnObjectiveCompleteEvent += ActiveEnabled_AB1_2;
		}

		private void OnDisable()
		{
			_objective_Win_AB_1.OnObjectiveCompleteEvent -= OnObjective_AB_1_Complete;
			_objective_Win_AB_2.OnObjectiveCompleteEvent -= OnObjective_AB_2_Complete;
			_objective_Win_AB_3.OnObjectiveCompleteEvent -= OnObjective_AB_3_Complete;
			_objective_AB_2_Start.OnObjectiveSetActiveEvent -= EnableCutSceneAI_AB_2;
			_objective_AB_3_Start.OnObjectiveSetActiveEvent -= EnableCutSceneAI_AB_3;
			_enabled_AB1.OnObjectiveCompleteEvent -= EnableCutSceneAI_AB_1;
			_enabled_AB2.OnObjectiveCompleteEvent -= EnableCutSceneAI_AB_2;
			_enabled_AB3.OnObjectiveCompleteEvent -= EnableCutSceneAI_AB_3;
			_activ_Enabled_AB3.OnObjectiveCompleteEvent -= ActiveEnabled_AB3;
			_activ_Enabled_AB2_2.OnObjectiveCompleteEvent -= ActiveEnabled_AB2_2;
			_activ_Enabled_AB1_2.OnObjectiveCompleteEvent -= ActiveEnabled_AB1_2;
		}

		public override void OnRespawn()
		{
			base.OnRespawn();
			WagonController[] array = Object.FindObjectsOfType<WagonController>();
			WagonController[] array2 = array;
			foreach (WagonController wagonController in array2)
			{
				if (wagonController.Model.IsDead())
				{
					wagonController.StateMachine.ChangeState<RespawnState>(new object[0]);
					wagonController.Model.Health = wagonController.Model.MaxHealth * 0.49f;
					NotDieObjectiveTarget component = wagonController.GetComponent<NotDieObjectiveTarget>();
					if (component != null)
					{
						Object.Destroy(component);
					}
					component = wagonController.gameObject.AddComponent<NotDieObjectiveTarget>();
					component.SetTarget(wagonController);
				}
			}
		}

		private void OnObjective_AB_1_Complete()
		{
			_trailer_3.layer = LayerMask.NameToLayer("PassiveLayer");
			_holder_AB12.SetActive(false);
			_trailer_1.layer = 0;
			_arrow_1.SetActive(false);
			_arrow_2.SetActive(true);
			ItemBlinking[] componentsInChildren = _trailer_1.GetComponentsInChildren<ItemBlinking>();
			foreach (ItemBlinking itemBlinking in componentsInChildren)
			{
				itemBlinking.Stop();
			}
			ItemBlinking[] componentsInChildren2 = _trailer_2.GetComponentsInChildren<ItemBlinking>();
			foreach (ItemBlinking itemBlinking2 in componentsInChildren2)
			{
				itemBlinking2.Play();
			}
		}

		private void OnObjective_AB_2_Complete()
		{
			_trailer_3.layer = LayerMask.NameToLayer("AllyLayer");
			_holder_AB23.SetActive(false);
			_trailer_2.layer = 0;
			_arrow_2.SetActive(false);
			_arrow_3.SetActive(true);
			ItemBlinking[] componentsInChildren = _trailer_2.GetComponentsInChildren<ItemBlinking>();
			foreach (ItemBlinking itemBlinking in componentsInChildren)
			{
				itemBlinking.Stop();
			}
			ItemBlinking[] componentsInChildren2 = _trailer_3.GetComponentsInChildren<ItemBlinking>();
			foreach (ItemBlinking itemBlinking2 in componentsInChildren2)
			{
				itemBlinking2.Play();
			}
		}

		private void OnObjective_AB_3_Complete()
		{
			StartCoroutine(CutScene_On_AB_3_End_Delay(_timer_CutScene_On_AB_3_End));
			_trailer_2.layer = 0;
			_arrow_3.SetActive(false);
			_sentinels_AB_4.SetActive(true);
			ItemBlinking[] componentsInChildren = _trailer_3.GetComponentsInChildren<ItemBlinking>();
			foreach (ItemBlinking itemBlinking in componentsInChildren)
			{
				itemBlinking.Stop();
			}
		}

		private void EnableCutSceneAI_AB_1()
		{
			if (Mob_1 != null)
			{
				Mob_1.enabled = true;
			}
			if (AllyMob_1 != null)
			{
				AllyMob_1.enabled = true;
			}
			if (Mob_2 != null)
			{
				Mob_2.enabled = true;
			}
			if (AllyMob_2 != null)
			{
				AllyMob_2.enabled = true;
			}
		}

		private void EnableCutSceneAI_AB_2()
		{
			if (Mob_3 != null)
			{
				Mob_3.enabled = true;
			}
			if (AllyMob_3 != null)
			{
				AllyMob_3.enabled = true;
			}
			if (Mob_4 != null)
			{
				Mob_4.enabled = true;
			}
			if (AllyMob_4 != null)
			{
				AllyMob_4.enabled = true;
			}
		}

		private void EnableCutSceneAI_AB_3()
		{
			if (Mob_5 != null)
			{
				Mob_5.enabled = true;
			}
			if (AllyMob_5 != null)
			{
				AllyMob_5.enabled = true;
			}
			if (Mob_6 != null)
			{
				Mob_6.enabled = true;
			}
			if (AllyMob_6 != null)
			{
				AllyMob_6.enabled = true;
			}
		}

		private void ActiveEnabled_AB1_2()
		{
			_AB1_2.SetActive(true);
		}

		private void ActiveEnabled_AB2_2()
		{
			_AB2_2.SetActive(true);
		}

		private void ActiveEnabled_AB3()
		{
			_AB3.SetActive(true);
		}

		public void MessageStart()
		{
			StartCoroutine(MessageShowDeffend());
		}

		private IEnumerator MessageShowDeffend()
		{
			_countdownMessage.Show(true);
			_countdownMessage.Description = MonoSingleton<Localization>.Instance["3_1_additional_text_01"];
			yield return new WaitForSeconds(5f);
			_countdownMessage.Show(false);
		}

		private IEnumerator CutScene_On_AB_3_End_Delay(float a)
		{
			yield return new WaitForSeconds(a);
			_CutScene_On_AB_3_End.SetActive(true);
		}
	}
}

using AppsMinistry.Core;
using AppsMinistry.Scene;
using Slugterra.Configuration;
using Slugterra.Game.Mission;
using Slugterra.Game.Units.Player;
using UnityEngine;

namespace Slugterra.Scene.Game
{
	[SceneName("2_1_QuietLawn", "", false, false)]
	public class QuietLawn_2_1 : SlugterraSceneController
	{
		[SerializeField]
		private TutorHelpInput Help;

		[SerializeField]
		private GameObject Arrow;

		[SerializeField]
		private GameObject Fence;

		[SerializeField]
		private tk2dUIItem _infurnus;

		[SerializeField]
		private tk2dUIItem _ButtonTemp1;

		[SerializeField]
		private tk2dUIItem _ButtonTemp2;

		[SerializeField]
		private ObjectiveTarget _objective_7_1;

		[SerializeField]
		private ObjectiveTarget _objective_7_3;

		[SerializeField]
		private GameObject _wagon1;

		[SerializeField]
		private GameObject _arrowWagon1;

		[SerializeField]
		private GameObject _wagon3;

		[SerializeField]
		private GameObject _arrowWagon3;

		[SerializeField]
		private BoxCollider _wagonTouchLayer1;

		[SerializeField]
		private BoxCollider _wagonTouchLayer3;

		private bool forFence;

		private BoxCollider[] bColliders;

		private void Start()
		{
			forFence = true;
			_infurnus.OnClick += ArrowTurnOn;
			_ButtonTemp1.OnClick += ArrowTurnOn;
			_ButtonTemp2.OnClick += ArrowTurnOn;
			bColliders = _wagon3.GetComponents<BoxCollider>();
			BoxCollider[] array = bColliders;
			foreach (BoxCollider boxCollider in array)
			{
				boxCollider.enabled = false;
			}
			DonDestroyMonoSingleton<GameServiceContext>.Instance.ReportAchievment("CgkIjZj4j6kZEAIQCw");
		}

		private void Update()
		{
			if (forFence)
			{
				PlayerController eli = UnityEngine.Object.FindObjectOfType<PlayerController>();
				if (eli != null && eli.NextSlug != null && eli.NextSlug.Type != SlugType.Flopper)
				{
					ArrowTurnOn();
				}
			}
			if (Fence == null && forFence)
			{
				if (Arrow != null)
				{
					Arrow.SetActive(false);
				}
				if (_infurnus != null)
				{
					_infurnus.OnClick -= ArrowTurnOn;
				}
				if (_ButtonTemp1 != null)
				{
					_ButtonTemp1.OnClick -= ArrowTurnOn;
				}
				if (_ButtonTemp2 != null)
				{
					_ButtonTemp2.OnClick -= ArrowTurnOn;
				}
				forFence = false;
			}
		}

		private void ArrowTurnOn()
		{
			Arrow.SetActive(true);
			Help.DisableHelp("selectslug");
		}

		private void OnEnable()
		{
			_objective_7_1.OnObjectiveSetActiveEvent += _arrow_1;
			_objective_7_3.OnObjectiveSetActiveEvent += _arrow_3;
		}

		private void OnDisable()
		{
			_objective_7_1.OnObjectiveSetActiveEvent -= _arrow_1;
			_objective_7_3.OnObjectiveSetActiveEvent -= _arrow_3;
		}

		private void _arrow_1()
		{
			if (!(_wagonTouchLayer1 == null))
			{
				_arrowWagon1.SetActive(true);
				_wagonTouchLayer1.enabled = true;
				BoxCollider[] componentsInChildren = _wagon1.GetComponentsInChildren<BoxCollider>();
				foreach (BoxCollider boxCollider in componentsInChildren)
				{
					boxCollider.enabled = true;
				}
			}
		}

		private void _arrow_3()
		{
			if (!(_wagonTouchLayer3 == null))
			{
				_arrowWagon3.SetActive(true);
				_wagonTouchLayer3.enabled = true;
				BoxCollider[] array = bColliders;
				foreach (BoxCollider boxCollider in array)
				{
					boxCollider.enabled = true;
				}
			}
		}
	}
}

using System.Collections;
using AppsMinistry.Core;
using AppsMinistry.Scene;
using Slugterra.Game.Mission;
using UnityEngine;

namespace Slugterra.Scene.Game
{
	[SceneName("4_3_Deadweed", "", false, false)]
	public class Deadweed_4_3 : SlugterraSceneController
	{
		[SerializeField]
		private ObjectiveTarget _destroyStation_1;

		[SerializeField]
		private ObjectiveTarget _destroyStation_2;

		[SerializeField]
		private ObjectiveTarget _destroyStation_3;

		[SerializeField]
		private ComeBackObjectiveTarget CombatZone_1;

		[SerializeField]
		private ComeBackObjectiveTarget CombatZone_2;

		[SerializeField]
		private ComeBackObjectiveTarget CombatZone_3;

		[SerializeField]
		private GameObject _station_1;

		[SerializeField]
		private GameObject _station_2;

		[SerializeField]
		private GameObject _station_3;

		[SerializeField]
		private GameObject _arrow_1;

		[SerializeField]
		private GameObject _arrow_2;

		[SerializeField]
		private GameObject _arrow_3;

		private void Start()
		{
			DonDestroyMonoSingleton<GameServiceContext>.Instance.ReportAchievment("CgkIjZj4j6kZEAIQFA");
		}

		private void OnEnable()
		{
			_destroyStation_1.OnObjectiveSetActiveEvent += OnObjective_AB_1_Complete;
			_destroyStation_2.OnObjectiveSetActiveEvent += OnObjective_AB_2_Complete;
			_destroyStation_3.OnObjectiveSetActiveEvent += OnObjective_AB_3_Complete;
		}

		private void OnDisable()
		{
			_destroyStation_1.OnObjectiveSetActiveEvent -= OnObjective_AB_1_Complete;
			_destroyStation_2.OnObjectiveSetActiveEvent -= OnObjective_AB_2_Complete;
			_destroyStation_3.OnObjectiveSetActiveEvent -= OnObjective_AB_3_Complete;
		}

		private void OnObjective_AB_1_Complete()
		{
			if (!(_station_1 == null))
			{
				ItemBlinking[] componentsInChildren = _station_1.GetComponentsInChildren<ItemBlinking>();
				foreach (ItemBlinking itemBlinking in componentsInChildren)
				{
					itemBlinking.Play();
				}
				_station_1.GetComponent<MeshCollider>().enabled = true;
				_arrow_1.SetActive(true);
			}
		}

		private void OnObjective_AB_2_Complete()
		{
			if (!(_station_2 == null))
			{
				ItemBlinking[] componentsInChildren = _station_2.GetComponentsInChildren<ItemBlinking>();
				foreach (ItemBlinking itemBlinking in componentsInChildren)
				{
					itemBlinking.Play();
				}
				_station_2.GetComponent<MeshCollider>().enabled = true;
				_arrow_2.SetActive(true);
			}
		}

		private void OnObjective_AB_3_Complete()
		{
			if (!(_station_3 == null))
			{
				ItemBlinking[] componentsInChildren = _station_3.GetComponentsInChildren<ItemBlinking>();
				foreach (ItemBlinking itemBlinking in componentsInChildren)
				{
					itemBlinking.Play();
				}
				_arrow_3.SetActive(true);
				StartCoroutine(PumpStation3Active());
			}
		}

		private IEnumerator PumpStation3Active()
		{
			yield return new WaitForSeconds(2f);
			_station_3.GetComponent<MeshCollider>().enabled = true;
		}
	}
}

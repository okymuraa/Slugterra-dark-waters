using AppsMinistry.Core;
using AppsMinistry.Scene;
using Slugterra.Game.Mission;
using UnityEngine;

namespace Slugterra.Scene.Game
{
	[SceneName("3_3_DarkPeriphery", "", false, false)]
	public class DarkPeriphery_3_3 : SlugterraSceneController
	{
		[SerializeField]
		private ObjectiveTarget _objective_Win_AB_1;

		[SerializeField]
		private ObjectiveTarget _objective_Win_AB_2;

		[SerializeField]
		private ObjectiveTarget _objective_Win_AB_3;

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
			DonDestroyMonoSingleton<GameServiceContext>.Instance.ReportAchievment("CgkIjZj4j6kZEAIQEg");
		}

		private void OnEnable()
		{
			_objective_Win_AB_1.OnObjectiveCompleteEvent += OnObjective_AB_1_Complete;
			_objective_Win_AB_2.OnObjectiveCompleteEvent += OnObjective_AB_2_Complete;
			_objective_Win_AB_3.OnObjectiveCompleteEvent += OnObjective_AB_3_Complete;
		}

		private void OnDisable()
		{
			_objective_Win_AB_1.OnObjectiveCompleteEvent -= OnObjective_AB_1_Complete;
			_objective_Win_AB_2.OnObjectiveCompleteEvent -= OnObjective_AB_2_Complete;
			_objective_Win_AB_3.OnObjectiveCompleteEvent -= OnObjective_AB_3_Complete;
		}

		private void OnObjective_AB_1_Complete()
		{
			ItemBlinking[] componentsInChildren = _station_1.GetComponentsInChildren<ItemBlinking>();
			foreach (ItemBlinking itemBlinking in componentsInChildren)
			{
				itemBlinking.Play();
			}
			_station_1.GetComponent<CapsuleCollider>().enabled = true;
			_arrow_1.SetActive(true);
		}

		private void OnObjective_AB_2_Complete()
		{
			ItemBlinking[] componentsInChildren = _station_2.GetComponentsInChildren<ItemBlinking>();
			foreach (ItemBlinking itemBlinking in componentsInChildren)
			{
				itemBlinking.Play();
			}
			_station_2.GetComponent<CapsuleCollider>().enabled = true;
			_arrow_2.SetActive(true);
		}

		private void OnObjective_AB_3_Complete()
		{
			ItemBlinking[] componentsInChildren = _station_3.GetComponentsInChildren<ItemBlinking>();
			foreach (ItemBlinking itemBlinking in componentsInChildren)
			{
				itemBlinking.Play();
			}
			_station_3.GetComponent<CapsuleCollider>().enabled = true;
			_arrow_3.SetActive(true);
		}
	}
}

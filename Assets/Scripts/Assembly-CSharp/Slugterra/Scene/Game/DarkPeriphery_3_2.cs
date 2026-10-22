using AppsMinistry.Core;
using AppsMinistry.Scene;
using Slugterra.Game.Mission;
using Slugterra.Game.Units.Mechanisms.Spline;
using UnityEngine;

namespace Slugterra.Scene.Game
{
	[SceneName("3_2_DarkPeriphery", "", false, false)]
	public class DarkPeriphery_3_2 : SlugterraSceneController
	{
		public TeamController _trailer;

		[SerializeField]
		private ObjectiveTarget _objective_Complete1;

		[SerializeField]
		private ObjectiveTarget _objective_Complete2;

		[SerializeField]
		private ObjectiveTarget _objective_Complete3;

		[SerializeField]
		private ObjectiveTarget _objective_Complete4;

		[SerializeField]
		private ObjectiveTarget _objective_Complete5;

		private void Start()
		{
			DonDestroyMonoSingleton<GameServiceContext>.Instance.ReportAchievment("CgkIjZj4j6kZEAIQEQ");
		}

		private void OnEnable()
		{
			_objective_Complete1.OnObjectiveCompleteEvent += Objective_Complete;
			_objective_Complete2.OnObjectiveCompleteEvent += Objective_Complete;
			_objective_Complete3.OnObjectiveCompleteEvent += Objective_Complete;
			_objective_Complete4.OnObjectiveCompleteEvent += Objective_Complete;
			_objective_Complete5.OnObjectiveCompleteEvent += Objective_Complete;
		}

		private void OnDisable()
		{
			_objective_Complete1.OnObjectiveCompleteEvent -= Objective_Complete;
			_objective_Complete2.OnObjectiveCompleteEvent -= Objective_Complete;
			_objective_Complete3.OnObjectiveCompleteEvent -= Objective_Complete;
			_objective_Complete4.OnObjectiveCompleteEvent -= Objective_Complete;
			_objective_Complete5.OnObjectiveCompleteEvent -= Objective_Complete;
		}

		public override void OnRespawn()
		{
			base.OnRespawn();
			TeamController teamController = Object.FindObjectOfType<TeamController>();
			if (teamController != null && teamController.Model.IsDead())
			{
				teamController.StateMachine.ChangeState<RespawnState>(new object[0]);
				teamController.Model.Health = teamController.Model.MaxHealth * 0.49f;
				NotDieObjectiveTarget component = teamController.GetComponent<NotDieObjectiveTarget>();
				if (component != null)
				{
					Object.Destroy(component);
				}
				component = teamController.gameObject.AddComponent<NotDieObjectiveTarget>();
				component.SetTarget(teamController);
			}
		}

		private void Objective_Complete()
		{
			_trailer.SetSpeed(0.3f, 0.9f, 1.9f);
			_trailer.OnMove();
		}
	}
}

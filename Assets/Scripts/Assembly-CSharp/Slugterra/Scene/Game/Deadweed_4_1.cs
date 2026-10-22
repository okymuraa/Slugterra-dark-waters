using AppsMinistry.Core;
using AppsMinistry.Scene;
using Slugterra.Game.Units.Ally.Assistant;
using Slugterra.Game.Units.Enemy.Deadweed;
using UnityEngine;

namespace Slugterra.Scene.Game
{
	[SceneName("4_1_Deadweed", "", false, false)]
	public class Deadweed_4_1 : SlugterraSceneController
	{
		private DeadweedController[] ghosts;

		private float viewArea;

		private AssistantController _assistantController;

		[SerializeField]
		private GameObject playerTriggerDoorClose;

		[SerializeField]
		private GameObject allyTriggerDoorClose;

		private void Start()
		{
			playerTriggerDoorClose.SetActive(false);
			allyTriggerDoorClose.SetActive(false);
			ghosts = (DeadweedController[])Object.FindObjectsOfType(typeof(DeadweedController));
			viewArea = ghosts[0].GetComponentInChildren<SphereCollider>().radius;
			_assistantController = Object.FindObjectOfType<AssistantController>();
			if (_assistantController != null)
			{
				if (!(_assistantController.StateMachine.CurrentState is DeathState))
				{
					allyTriggerDoorClose.SetActive(true);
				}
			}
			else
			{
				playerTriggerDoorClose.SetActive(true);
			}
			DonDestroyMonoSingleton<GameServiceContext>.Instance.ReportAchievment("CgkIjZj4j6kZEAIQEw");
		}

		public void EnableMobs()
		{
			DeadweedController[] array = ghosts;
			foreach (DeadweedController deadweedController in array)
			{
				if (deadweedController != null)
				{
					deadweedController.ViewArea.SetRadius(viewArea);
					deadweedController.CanAttack = true;
					deadweedController.DamageSystem.enabled = true;
				}
			}
		}

		public void DisableMobs()
		{
			DeadweedController[] array = ghosts;
			foreach (DeadweedController deadweedController in array)
			{
				if (deadweedController != null)
				{
					deadweedController.ViewArea.SetRadius(0f);
					deadweedController.CanAttack = false;
					deadweedController.DamageSystem.enabled = false;
					deadweedController.DamageSystem.Clear();
					deadweedController.AttackTarget = null;
				}
			}
		}
	}
}

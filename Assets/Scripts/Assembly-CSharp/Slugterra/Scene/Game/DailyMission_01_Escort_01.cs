using AppsMinistry.Scene;
using Slugterra.Game.Mission;
using Slugterra.Game.Units.Ally.Assistant;
using UnityEngine;

namespace Slugterra.Scene.Game
{
	[SceneName("0_DailyMission_01_Escort_01", "", false, false)]
	internal class DailyMission_01_Escort_01 : SlugterraSceneController
	{
		[SerializeField]
		private NotDieObjectiveTarget objective;

		public GameObject CutScene;

		protected override void OnLoaded()
		{
			base.OnLoaded();
			GameObject gameObject = GameObject.Find("DailyTemp_Camera_DestroyAfterRun");
			if (gameObject != null)
			{
				Object.Destroy(gameObject);
			}
			FollowerController[] followers = GetFollowers();
			CutScene.SendMessage("FindAfterRes", followers);
			FollowerController[] array = followers;
			foreach (FollowerController target in array)
			{
				objective.SetTarget(target);
			}
			objective.Reset();
		}

		public override void OnRespawn()
		{
			base.OnRespawn();
			FollowerController[] followers = GetFollowers();
			CutScene.SendMessage("FindAfterRes", followers);
			FollowerController[] array = followers;
			foreach (FollowerController target in array)
			{
				objective.SetTarget(target);
			}
			objective.Reset();
		}
	}
}

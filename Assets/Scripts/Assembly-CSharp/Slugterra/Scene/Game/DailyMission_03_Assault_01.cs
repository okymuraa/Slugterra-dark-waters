using AppsMinistry.Scene;
using UnityEngine;

namespace Slugterra.Scene.Game
{
	[SceneName("0_DailyMission_03_Assault_01", "", false, false)]
	internal class DailyMission_03_Assault_01 : SlugterraSceneController
	{
		protected override void OnLoaded()
		{
			base.OnLoaded();
			GameObject gameObject = GameObject.Find("DailyTemp_Camera_DestroyAfterRun");
			if (gameObject != null)
			{
				Object.Destroy(gameObject);
			}
		}
	}
}

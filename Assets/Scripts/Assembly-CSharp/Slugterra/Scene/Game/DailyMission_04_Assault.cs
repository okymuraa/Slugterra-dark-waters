using AppsMinistry.Scene;
using UnityEngine;

namespace Slugterra.Scene.Game
{
	[SceneName("0_DailyMission_04_Assault", "", false, false)]
	internal class DailyMission_04_Assault : SlugterraSceneController
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

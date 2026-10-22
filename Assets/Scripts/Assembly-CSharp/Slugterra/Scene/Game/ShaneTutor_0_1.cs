using AppsMinistry.Core;
using AppsMinistry.Scene;
using UnityEngine;

namespace Slugterra.Scene.Game
{
	[SceneName("0_1_ShaneTutor", "", false, false)]
	public class ShaneTutor_0_1 : SlugterraSceneController
	{
		protected override void OnLoaded()
		{
			base.OnLoaded();
			string key = 2 + "." + 0 + "." + 2275;
			PlayerPrefs.SetInt(key, 1);
			PlayerPrefs.SetInt("LoadBundlesEnable", 1);
			DonDestroyMonoSingleton<GameServiceContext>.Instance.ReportAchievment("CgkIjZj4j6kZEAIQAQ");
		}
	}
}

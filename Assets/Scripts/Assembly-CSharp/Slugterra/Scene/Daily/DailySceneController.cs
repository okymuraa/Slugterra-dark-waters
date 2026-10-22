using UnityEngine;

namespace Slugterra.Scene.Daily
{
	internal class DailySceneController : SlugterraSceneController
	{
		protected override void OnLoaded()
		{
			base.OnLoaded();
			SceneLoader.LoadAdditiveScene();
			Object.Destroy(this);
		}
	}
}

using AppsMinistry.Scene;
using Slugterra.Game;

namespace Slugterra.Scene.Loading
{
	[LoadingScene]
	[SceneName("BaseLoadingScene", "", false, false)]
	public class LoadingSceneController : SlugterraSceneController
	{
		protected override void OnLoaded()
		{
			base.OnLoaded();
			GameFactory.Clear();
			SceneLoader.LoadNextScene();
		}
	}
}

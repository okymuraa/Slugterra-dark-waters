using AppsMinistry.Scene;

namespace Slugterra.Scene
{
	[BundleScene("base", true)]
	[SceneName("MapScene", "", false, false)]
	public class MapSceneController : SlugterraSceneController
	{
		protected override void OnBack()
		{
			SceneLoader.LoadScene<MainSceneController>();
		}
	}
}

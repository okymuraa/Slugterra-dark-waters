using AppsMinistry.Scene;

namespace Slugterra.Scene
{
	[SceneName("MultiplayerMapScene", "", false, false)]
	[BundleScene("base", true)]
	public class MultiplayerSceneController : SlugterraSceneController
	{
		protected override void OnBack()
		{
			SceneLoader.LoadScene<MainSceneController>();
		}
	}
}

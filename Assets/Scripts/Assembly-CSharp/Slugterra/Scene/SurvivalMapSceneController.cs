using AppsMinistry.Scene;

namespace Slugterra.Scene
{
	[BundleScene("base", true)]
	[SceneName("SurvivalMapScene", "", false, false)]
	public class SurvivalMapSceneController : SlugterraSceneController
	{
		protected override void OnBack()
		{
			SceneLoader.LoadScene<MainSceneController>();
		}
	}
}

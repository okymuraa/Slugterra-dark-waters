using AppsMinistry.Scene;

namespace Slugterra.Scene
{
	[BundleScene("base", true)]
	[SceneName("BackpackScene", "", false, false)]
	public class BackpackSceneController : SlugterraSceneController
	{
		protected override void OnBack()
		{
			SceneLoader.LoadPrevScene();
		}
	}
}

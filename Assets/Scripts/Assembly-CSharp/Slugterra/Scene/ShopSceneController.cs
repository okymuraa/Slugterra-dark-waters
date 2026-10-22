using AppsMinistry.Scene;

namespace Slugterra.Scene
{
	[SceneName("ShopScene", "", false, false)]
	[BundleScene("base", true)]
	public class ShopSceneController : SlugterraSceneController
	{
		protected override void OnBack()
		{
			SceneLoader.LoadPrevScene();
		}
	}
}

using AppsMinistry.Scene;

namespace Slugterra.Scene
{
	[BundleScene("base", true)]
	[SceneName("SettingsScene", "", false, false)]
	public class SettingsSceneController : SlugterraSceneController
	{
		protected override void OnLoaded()
		{
			AppsMinistrySdkBinding.Instance.AMRichMediaShow(string.Empty);
		}

		protected override void OnBack()
		{
			SceneLoader.LoadPrevScene();
		}
	}
}

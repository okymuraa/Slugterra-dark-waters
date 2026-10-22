using AppsMinistry.Core;
using AppsMinistry.Scene;

namespace Slugterra.Scene.Game
{
	[SceneName("5_1_GhoulDep", "", false, false)]
	public class GhoulDep_5_1 : SlugterraSceneController
	{
		private void Start()
		{
			DonDestroyMonoSingleton<GameServiceContext>.Instance.ReportAchievment("CgkIjZj4j6kZEAIQFg");
		}
	}
}

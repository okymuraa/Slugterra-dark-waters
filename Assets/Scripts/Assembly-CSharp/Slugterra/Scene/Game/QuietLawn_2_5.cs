using AppsMinistry.Core;
using AppsMinistry.Scene;

namespace Slugterra.Scene.Game
{
	[SceneName("2_5_QuietLawn", "", false, false)]
	public class QuietLawn_2_5 : SlugterraSceneController
	{
		private void Start()
		{
			DonDestroyMonoSingleton<GameServiceContext>.Instance.ReportAchievment("CgkIjZj4j6kZEAIQDw");
		}
	}
}

using AppsMinistry.Core;
using AppsMinistry.Scene;

namespace Slugterra.Scene.Game
{
	[SceneName("4_2_Deadweed", "", false, false)]
	public class Deadweed_4_2 : SlugterraSceneController
	{
		private void Start()
		{
			DonDestroyMonoSingleton<GameServiceContext>.Instance.ReportAchievment("CgkIjZj4j6kZEAIQFQ");
		}
	}
}

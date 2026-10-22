using AppsMinistry.Core;
using AppsMinistry.Scene;

namespace Slugterra.Scene.Game
{
	[SceneName("6_3_Train", "", false, false)]
	public class Train_6_3 : SlugterraSceneController
	{
		private void Start()
		{
			DonDestroyMonoSingleton<GameServiceContext>.Instance.ReportAchievment("CgkIjZj4j6kZEAIQGw");
		}
	}
}

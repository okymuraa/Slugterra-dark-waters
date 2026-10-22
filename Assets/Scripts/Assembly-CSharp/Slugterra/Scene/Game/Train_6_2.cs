using AppsMinistry.Core;
using AppsMinistry.Scene;

namespace Slugterra.Scene.Game
{
	[SceneName("6_2_Train", "", false, false)]
	public class Train_6_2 : SlugterraSceneController
	{
		private void Start()
		{
			DonDestroyMonoSingleton<GameServiceContext>.Instance.ReportAchievment("CgkIjZj4j6kZEAIQGg");
		}
	}
}

using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.App;

namespace Slugterra.UI.Game
{
	public class StatisticsController : UIController<StatisticsView>
	{
		protected override void _OnEnable()
		{
			View.ApplyModel(MonoSingleton<AppController>.Instance.GetCurrentGameLevel());
		}
	}
}

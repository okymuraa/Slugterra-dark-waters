using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.App;

namespace Slugterra.UI.Multiplayer
{
	public class MultiplayerStatisticsController : UIController<MultiplayerStatisticsView>
	{
		protected override void _OnEnable()
		{
			View.ApplyModel(MonoSingleton<AppController>.Instance.GetCurrentGameLevel());
		}
	}
}

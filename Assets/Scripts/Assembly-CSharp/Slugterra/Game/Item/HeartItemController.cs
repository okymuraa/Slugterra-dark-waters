using AppsMinistry.Core;
using Slugterra.Configuration;
using Slugterra.Domain.Game.Units;

namespace Slugterra.Game.Item
{
	public class HeartItemController : ItemController
	{
		private float _heart;

		protected override void OnStart()
		{
			GameItemsConfig gameItemsConfig = MonoSingleton<GameConfiguration>.Instance.GameItemsConfig;
			_heart = gameItemsConfig.HeartConfig.Heart;
		}

		protected override void OnPickUp(Unit unit)
		{
			unit.AddHealth(_heart);
		}
	}
}

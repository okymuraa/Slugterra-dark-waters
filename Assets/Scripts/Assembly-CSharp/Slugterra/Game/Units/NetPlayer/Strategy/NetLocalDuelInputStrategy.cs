using Slugterra.Game.Units.Player;
using Slugterra.Game.Units.Player.Strategy;

namespace Slugterra.Game.Units.NetPlayer.Strategy
{
	public class NetLocalDuelInputStrategy : DuelSplineInputStrategy
	{
		public NetLocalDuelInputStrategy(PlayerController controller)
			: base(controller)
		{
		}
	}
}

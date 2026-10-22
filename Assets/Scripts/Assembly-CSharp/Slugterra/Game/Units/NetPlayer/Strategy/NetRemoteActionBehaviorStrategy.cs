using Slugterra.Game.Units.Player;
using Slugterra.Game.Units.Player.Strategy;

namespace Slugterra.Game.Units.NetPlayer.Strategy
{
	public class NetRemoteActionBehaviorStrategy : BehaviorStrategy
	{
		public NetRemoteActionBehaviorStrategy(PlayerController controller)
			: base(controller)
		{
		}
	}
}

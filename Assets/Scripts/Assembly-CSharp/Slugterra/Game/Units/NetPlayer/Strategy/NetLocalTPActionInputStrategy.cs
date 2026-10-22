using Slugterra.Game.Units.Player;
using Slugterra.Game.Units.Player.Strategy;

namespace Slugterra.Game.Units.NetPlayer.Strategy
{
	public class NetLocalTPActionInputStrategy : TPActionInputStrategy
	{
		public NetLocalTPActionInputStrategy(PlayerController controller)
			: base(controller)
		{
		}
	}
}

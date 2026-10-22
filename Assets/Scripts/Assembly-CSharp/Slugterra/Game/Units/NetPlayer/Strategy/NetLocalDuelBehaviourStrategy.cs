using AppsMinistry.Core.Command;
using Slugterra.Game.Units.Player;
using Slugterra.Game.Units.Player.Strategy;

namespace Slugterra.Game.Units.NetPlayer.Strategy
{
	public class NetLocalDuelBehaviourStrategy : DuelBehaviourStrategy
	{
		public NetLocalDuelBehaviourStrategy(PlayerController controller)
			: base(controller)
		{
		}

		protected override void FireDuel()
		{
			MonoCommand.ExecuteOn<NetDuelFireCommand>(Controller.GameObject, new object[2] { Controller, Controller.NextSlug });
			Controller.UpdateNextFire();
		}

		protected override void FireDefault()
		{
			NetFireCommand netFireCommand = MonoCommand.ExecuteOn<NetFireCommand>(Controller.GameObject, new object[4] { Controller, Controller.NextSlug, false, 2f });
			netFireCommand.AsyncToken.AddResponder(new Responder<FireCommand>(OnFireCommandFinish));
			Controller.UpdateNextFire();
		}
	}
}

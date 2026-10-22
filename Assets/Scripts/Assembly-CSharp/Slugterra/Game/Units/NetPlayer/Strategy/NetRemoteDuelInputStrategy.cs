using AppsMinistry.Core.Command;
using Slugterra.Configuration;
using Slugterra.Domain.Game.Units;
using Slugterra.Domain.Game.Units.Slugs;
using Slugterra.Game.Units.Player;
using Slugterra.Game.Units.Player.Strategy;

namespace Slugterra.Game.Units.NetPlayer.Strategy
{
	public class NetRemoteDuelInputStrategy : BaseSplineInputStrategy
	{
		public NetRemoteDuelInputStrategy(PlayerController controller)
			: base(controller)
		{
		}

		public override void OnUseSlug(SlugType slugType)
		{
			Slug slug = (Controller.Model as Humanoid).GetSlug(slugType);
			MonoCommand.ExecuteOn<NetDuelFireCommand>(Controller.GameObject, new object[2] { Controller, slug });
		}
	}
}

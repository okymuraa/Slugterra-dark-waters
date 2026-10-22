using AppsMinistry.Core.Command;
using Slugterra.Configuration;
using Slugterra.Domain.Game.Units;
using Slugterra.Domain.Game.Units.Slugs;
using Slugterra.Game.Units.Player;
using Slugterra.Game.Units.Player.Strategy;
using UnityEngine;

namespace Slugterra.Game.Units.NetPlayer.Strategy
{
	public class NetRemoteTPActionInputStrategy : TPActionInputStrategy
	{
		public NetRemoteTPActionInputStrategy(PlayerController controller)
			: base(controller)
		{
		}

		public override void OnMove(Vector2 axises)
		{
		}

		public override void OnMoveEnd()
		{
		}

		public override void OnRotate(Vector2 axises)
		{
		}

		public override void OnUseSlug(SlugType slugType)
		{
			Slug slug = (Controller.Model as Humanoid).GetSlug(slugType);
			MonoCommand.ExecuteOn<NetFireCommand>(Controller.GameObject, new object[2] { Controller, slug });
		}
	}
}

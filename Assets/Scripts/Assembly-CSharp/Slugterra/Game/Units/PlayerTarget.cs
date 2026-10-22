using System;
using AppsMinistry.Core.Command;
using Slugterra.Configuration;
using Slugterra.Game.Units.Player;

namespace Slugterra.Game.Units
{
	public class PlayerTarget : ITarget
	{
		public void Apply(Type typeOfBoost, ConsumableProperties config)
		{
			PlayerController playerController = FindTarget(config) as PlayerController;
			if (playerController != null)
			{
				MonoCommand.ExecuteOn(typeOfBoost, playerController.GameObject, playerController, config);
			}
		}

		public object FindTarget(ConsumableProperties config)
		{
			return PlayerManager.LocalPlayer;
		}
	}
}

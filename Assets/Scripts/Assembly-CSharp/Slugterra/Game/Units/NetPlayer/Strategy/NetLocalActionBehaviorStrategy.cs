using AppsMinistry.Core.Command;
using Slugterra.Configuration;
using Slugterra.Domain.Game.Units;
using Slugterra.Game.Units.Player;
using Slugterra.Game.Units.Player.Strategy;
using UnityEngine;

namespace Slugterra.Game.Units.NetPlayer.Strategy
{
	public class NetLocalActionBehaviorStrategy : ActionBehaviorStrategy
	{
		private NetFireCommand _fireCommand;

		public NetLocalActionBehaviorStrategy(PlayerController controller)
			: base(controller)
		{
		}

		protected override void Attack()
		{
			if (Controller.NextSlug != null && Controller.CanAttack && Vector3.SqrMagnitude(Controller.AttackTarget.position - Controller.Transform.position) < Mathf.Pow((Controller.Model as Humanoid).Blaster.Range, 2f))
			{
				bool flag = true;
				if (Controller.NextSlug.Type == SlugType.Crystalid)
				{
					flag = false;
				}
				_fireCommand = MonoCommand.ExecuteOn<NetFireCommand>(Controller.GameObject, new object[3] { Controller, Controller.NextSlug, flag });
				Controller.UpdateNextFire();
			}
		}
	}
}

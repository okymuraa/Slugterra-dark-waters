using UnityEngine;

namespace Slugterra.Game.Units.Slugs.Network
{
	public class NetLocalDuelSlugStrategy : DuelAllySlugStrategy
	{
		public NetLocalDuelSlugStrategy(SlugController controller, int[] layers, Transform target)
			: base(controller, layers, target)
		{
		}

		protected override void AutoFigting(Transform enemy)
		{
			if (PhotonNetwork.isMasterClient)
			{
				base.AutoFigting(enemy);
			}
		}

		protected override void ManualFigting(Transform enemy)
		{
			AutoFigting(enemy);
		}

		protected override Transform DetectCollision()
		{
			return (!PhotonNetwork.isMasterClient) ? null : base.DetectCollision();
		}
	}
}

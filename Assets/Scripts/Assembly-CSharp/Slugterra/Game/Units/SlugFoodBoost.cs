using Slugterra.Domain.Game.Units;
using UnityEngine;

namespace Slugterra.Game.Units
{
	internal class SlugFoodBoost : BoostEffect
	{
		protected override void OnStart(object[] args)
		{
			base.OnStart(args);
			Humanoid humanoid = controller.Model as Humanoid;
			if (humanoid != null)
			{
				humanoid.Clear(Time.time);
			}
			Finish();
		}
	}
}

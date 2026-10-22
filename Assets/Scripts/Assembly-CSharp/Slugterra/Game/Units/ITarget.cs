using System;
using Slugterra.Configuration;

namespace Slugterra.Game.Units
{
	public interface ITarget
	{
		void Apply(Type typeOfBoost, ConsumableProperties config);

		object FindTarget(ConsumableProperties config);
	}
}

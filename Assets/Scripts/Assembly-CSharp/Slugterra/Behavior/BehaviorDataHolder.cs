using System.Collections.Generic;
using Slugterra.Behavior.Data;

namespace Slugterra.Behavior
{
	public class BehaviorDataHolder
	{
		public Unit Me = new Unit();

		public Enemy Enemy = new Enemy();

		public List<IBehavior> Privates;
	}
}

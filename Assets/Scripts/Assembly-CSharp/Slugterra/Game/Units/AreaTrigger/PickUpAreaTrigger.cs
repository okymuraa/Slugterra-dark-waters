using UnityEngine;

namespace Slugterra.Game.Units.AreaTrigger
{
	public class PickUpAreaTrigger : AreaTrigger
	{
		public Transform Pop()
		{
			if (Items.Count == 0)
			{
				return null;
			}
			Transform transform = Items[Items.Count - 1];
			Items.Remove(transform);
			return transform;
		}
	}
}

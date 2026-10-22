using System.Collections.Generic;
using AppsMinistry.UI;
using Slugterra.Configuration;
using Slugterra.Domain.Shops;
using UnityEngine;

namespace Slugterra.UI.Game
{
	internal class InventoryBarController : UIController<InventoryBarView>
	{
		[SerializeField]
		private InventoryItemController _speedBoost;

		[SerializeField]
		private InventoryItemController _slugfoodBoost;

		[SerializeField]
		private InventoryItemController _defenciveBoost;

		[SerializeField]
		private InventoryItemController _medicineBoost;

		public void ApplyModel(List<ConsumableItem> items)
		{
			foreach (ConsumableItem item in items)
			{
				switch (item.Type)
				{
				case ConsumableType.SpeedBoost:
					_speedBoost.ApplyModel(item);
					break;
				case ConsumableType.SlugFood:
					_slugfoodBoost.ApplyModel(item);
					break;
				case ConsumableType.ArmorBoost:
					_defenciveBoost.ApplyModel(item);
					break;
				case ConsumableType.First:
					_medicineBoost.ApplyModel(item);
					break;
				}
			}
		}
	}
}

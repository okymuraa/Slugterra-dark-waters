using System.Collections.Generic;
using AppsMinistry.UI.View.ScrollView;
using AppsMinistry.Utils;
using Slugterra.Domain.Shops;
using UnityEngine;

namespace Slugterra.UI.Mission
{
	public class ArmorScrollViewController : ScrollViewController<ArmorItemController>
	{
		private List<ArmorItem> _items;

		protected override int Count
		{
			get
			{
				if (_items == null)
				{
					return 0;
				}
				return _items.Count;
			}
		}

		protected override void OnItemChanged(ArmorItemController item, int index)
		{
			item.ApplyModel(_items[index]);
		}

		protected override void OnStart()
		{
			base.OnStart();
			switch (ScreenUtil.GetAspectRation(Camera.main.pixelWidth, Camera.main.pixelHeight))
			{
			case AspectRation.e4x3:
				SetVisibleHeight(7.1f);
				break;
			case AspectRation.e3x2:
				SetVisibleHeight(6.35f);
				break;
			case AspectRation.e16x9:
				SetVisibleHeight(5.2f);
				break;
			}
		}

		public void ApplyModel(List<ArmorItem> items)
		{
			_items = items;
			Rebuild();
		}
	}
}

using AppsMinistry.UI.View.ScrollView;
using AppsMinistry.Utils;
using UnityEngine;

namespace Slugterra.UI.Mission
{
	public class ConsumableScrollViewController : ScrollViewController<ConsumableLineController>
	{
		protected override int Count
		{
			get
			{
				return 0;
			}
		}

		protected override void OnItemChanged(ConsumableLineController line, int index)
		{
			if (Count == 0)
			{
				return;
			}
			for (int i = 0; i < 3; i++)
			{
				int num = index * 3 + i;
				ConsumableItemController item = line.GetItem(i);
				item.gameObject.SetActive(true);
				if (num > Count * 3)
				{
					item.gameObject.SetActive(false);
				}
			}
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
	}
}

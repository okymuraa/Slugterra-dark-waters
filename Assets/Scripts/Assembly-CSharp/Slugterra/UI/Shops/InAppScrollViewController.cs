using AppsMinistry.UI.View.ScrollView;
using AppsMinistry.Utils;
using Slugterra.Domain.UI.Shop;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class InAppScrollViewController : ScrollViewController<InAppLineController>
	{
		private InAppsTab _model;

		protected override int Count
		{
			get
			{
				if (_model == null)
				{
					return 0;
				}
				int num = _model.Items.Count / 3;
				if (num * 3 < _model.Items.Count)
				{
					num++;
				}
				return num;
			}
		}

		protected override void OnItemChanged(InAppLineController line, int index)
		{
			if (Count == 0)
			{
				return;
			}
			for (int i = 0; i < 3; i++)
			{
				int num = index * 3 + i;
				InAppItemController item = line.GetItem(i);
				item.gameObject.SetActive(true);
				if (num < _model.Items.Count)
				{
					item.ApplyModel(_model.Items[num]);
				}
				else
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

		public void ApplyModel(InAppsTab model)
		{
			_model = model;
			Rebuild();
		}
	}
}

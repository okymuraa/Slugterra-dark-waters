using System;
using AppsMinistry.UI;

namespace Slugterra.UI.Popup
{
	public class PopupNewDailyMissionView : PopupView
	{
		public Action closeEvent;

		protected override void OnCreate()
		{
			base.OnCreate();
		}

		protected override void OnApplyModel(object model)
		{
			base.OnApplyModel(model);
		}

		public override void Close()
		{
			if (closeEvent != null)
			{
				closeEvent();
			}
		}
	}
}

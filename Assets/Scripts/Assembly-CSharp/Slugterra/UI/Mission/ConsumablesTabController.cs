using UnityEngine;

namespace Slugterra.UI.Mission
{
	public class ConsumablesTabController : MissionTabController
	{
		[SerializeField]
		private ConsumableScrollViewController _scrollView;

		public override void ApplyModel(object model)
		{
			base.ApplyModel(model);
		}
	}
}

using Slugterra.Domain.UI.Shop;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class InAppsTabController : ShopTabController
	{
		[SerializeField]
		private InAppScrollViewController _scrollView;

		protected new InAppsTabView View
		{
			get
			{
				return base.View as InAppsTabView;
			}
		}

		protected override void OnStart()
		{
			base.OnStart();
		}

		public override void ApplyModel(object model)
		{
			base.ApplyModel(model);
			if (model is InAppsTab)
			{
				_scrollView.ApplyModel(model as InAppsTab);
			}
		}
	}
}

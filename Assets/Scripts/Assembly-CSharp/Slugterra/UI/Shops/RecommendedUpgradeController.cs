using AppsMinistry.UI;

namespace Slugterra.UI.Shops
{
	public class RecommendedUpgradeController : UIController<RecommendedUpgradeView>
	{
		public void SetRecommendedUpgrade(RecommendationDataContainer data)
		{
			View.ApplyModel(data);
		}

		protected override void _OnDisable()
		{
			View.Deactivate();
			base._OnDisable();
		}
	}
}

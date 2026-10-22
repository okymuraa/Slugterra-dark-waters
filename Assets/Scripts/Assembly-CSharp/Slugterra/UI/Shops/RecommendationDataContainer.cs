namespace Slugterra.UI.Shops
{
	public struct RecommendationDataContainer
	{
		public int ItemLevel;

		public int RecommendedLevel;

		public bool IsRequired;

		public RecommendationDataContainer(int itemLvl, int recommendedLvl, bool isRequired)
		{
			ItemLevel = itemLvl;
			RecommendedLevel = recommendedLvl;
			IsRequired = isRequired;
		}
	}
}

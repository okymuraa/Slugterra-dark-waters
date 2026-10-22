using System;

namespace AppsMinistry.InAppPurchases
{
	public class InAppPurchase
	{
		public enum InAppCategory
		{
			Coins = 0,
			Gems = 1,
			Blasters = 2,
			Subscription = 3,
			Tickets = 4
		}

		public string ProductID;

		public string Description;

		public InAppCategory Category;

		public string Price;

		public string FormattedPrice;

		public string CurrencyCode;

		public string CurrencySymbol;

		public string StoreSpriteName;

		public InAppType InAppType;

		public InAppMarket InAppMarket;

		public int GoldValue = -1;

		public int GemsValue = -1;

		public int TicketValue = -1;

		public bool IsBestSeller;

		public int Sale;

		public string TitleLocalizationKey;

		public Action<int> OnSuccessBuy;

		public InAppPurchase(string productID, InAppType inAppType, InAppCategory category, InAppMarket inAppMarket, Action<int> onSuccessBuy, string titleLocalizationKey = "")
		{
			ProductID = productID;
			InAppType = inAppType;
			Category = category;
			InAppMarket = inAppMarket;
			TitleLocalizationKey = titleLocalizationKey;
			OnSuccessBuy = onSuccessBuy;
		}
	}
}

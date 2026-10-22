using System;

namespace AppsMinistry.InAppPurchases
{
	[Flags]
	public enum InAppMarket
	{
		AppStore = 1,
		GoogleMarket = 2,
		AmazonMarket = 4
	}
}

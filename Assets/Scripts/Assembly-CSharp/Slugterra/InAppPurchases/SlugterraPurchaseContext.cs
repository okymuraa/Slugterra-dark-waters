using System.Collections.Generic;
using AppsMinistry.Core;
using AppsMinistry.InAppPurchases;

namespace Slugterra.InAppPurchases
{
	[AppSingleton]
	public class SlugterraPurchaseContext : PurchaseContext<SlugterraPurchaseContext>
	{
		protected override List<InAppPurchase> GetInApps()
		{
			return InApps.ALL_INAPPS;
		}
	}
}

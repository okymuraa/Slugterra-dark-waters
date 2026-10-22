using System;
using Slugterra.Domain.Shops;

namespace Slugterra.InAppPurchases
{
	public class SubscriptionContextSlug : SubscriptionContext
	{
		public SubscriptionContextSlug(SubscriptionItem sItem, string nameSub, DateTime dateStart, int periodSub)
			: base(sItem, nameSub, dateStart, periodSub)
		{
		}
	}
}

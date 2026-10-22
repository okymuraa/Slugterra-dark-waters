using System;
using Slugterra.Domain.Shops;

namespace Slugterra.InAppPurchases
{
	public abstract class SubscriptionContext
	{
		public SubscriptionItem sItem;

		public string nameSub;

		public DateTime dateEnd;

		public DateTime dateStart;

		public int periodSub;

		public SubscriptionContext(SubscriptionItem sItem, string nameSub, DateTime dateStart, int periodSub)
		{
			this.sItem = sItem;
			this.nameSub = nameSub;
			this.dateStart = dateStart;
			dateEnd = dateStart.AddSeconds(periodSub);
			this.periodSub = periodSub;
		}
	}
}

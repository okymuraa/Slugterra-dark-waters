namespace Slugterra.Domain.Shops
{
	public class SubscriptionItemMultiplier : SubscriptionItem
	{
		public SubscriptionItemMultiplier(string sku, int days)
			: base(sku, days)
		{
		}

		public SubscriptionItemMultiplier(string type, string bondType, string description, string sku, int days)
			: base(type, bondType, description, sku, days)
		{
		}
	}
}

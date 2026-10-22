namespace Slugterra.Domain.Shops
{
	public class SubscriptionItemGold : SubscriptionItem
	{
		private string _goldDaily;

		public string GoldDaily
		{
			get
			{
				return _goldDaily;
			}
		}

		public SubscriptionItemGold(string type, string bondType, string description, int goldDaily, string sku, int days)
			: base(type, bondType, description, sku, days)
		{
			_goldDaily = string.Format("{0}", goldDaily);
		}

		public override string ToString()
		{
			return string.Format("[SubscriptionItem: GoldDaily={0}, DurationDays={1}, SKU={2}, IsValid={3}, ExpirationTime={4}]", GoldDaily, DurationDays, SKU, State, ExpirationTime, IsBestSeller, IsSale);
		}
	}
}

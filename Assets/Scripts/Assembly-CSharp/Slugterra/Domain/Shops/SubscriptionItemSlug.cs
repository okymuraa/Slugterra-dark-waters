using System;

namespace Slugterra.Domain.Shops
{
	public class SubscriptionItemSlug : SubscriptionItem
	{
		private string _slugType;

		public string SlugType
		{
			get
			{
				return _slugType;
			}
		}

		public SubscriptionItemSlug(string slugType, string sku, int days)
			: base(sku, days)
		{
			_slugType = slugType;
		}

		public SubscriptionItemSlug(string slugType, string title, string sku, SubscriptionItemState state, DateTime expirationTime, bool isSale = false, bool isBestSeller = false)
			: base(title, sku, state, expirationTime, isSale, isBestSeller)
		{
			_slugType = slugType;
		}
	}
}

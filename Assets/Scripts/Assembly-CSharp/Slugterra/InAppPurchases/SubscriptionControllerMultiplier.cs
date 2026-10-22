using Slugterra.Domain.Shops;

namespace Slugterra.InAppPurchases
{
	public class SubscriptionControllerMultiplier : SubscriptionController
	{
		private SubscriptionItemMultiplier _subscriptionItem;

		public override SubscriptionItem Item
		{
			get
			{
				return _subscriptionItem;
			}
		}

		public override void SetSubsciptionItem(SubscriptionItem subItem)
		{
			_subscriptionItem = (SubscriptionItemMultiplier)subItem;
		}

		public override void OnApplicationPause(bool paused)
		{
		}

		public override void Init()
		{
		}

		public override void OnSubscriptionPurchased(int validThru)
		{
		}

		protected override void ResolveSubscription()
		{
			base.ResolveSubscription();
		}
	}
}

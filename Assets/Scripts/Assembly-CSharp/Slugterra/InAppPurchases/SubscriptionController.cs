using System;
using System.Diagnostics;
using System.Threading;
using Slugterra.Domain.Shops;

namespace Slugterra.InAppPurchases
{
	public abstract class SubscriptionController
	{
		public const string SUBSCRIPTION_NAME_MASK = "nameSub";

		public const string SUBSCRIPTION_STARTDATE_MASK = "dateStartStamp";

		public const string SUBSCRIPTION_DURATION_MASK = "countPay";

		public const string SUBSCRIPTION_PERIOD_MASK = "periodAction";

		public const string SUBSCRIPTION_WORKSDAY_MASK = "donePay";

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnResolveSubscription__BackingField;

		public abstract SubscriptionItem Item { get; }

		public event Action OnResolveSubscription
		{
			add
			{
				Action action = OnResolveSubscription__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnResolveSubscription__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnResolveSubscription__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnResolveSubscription__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public abstract void SetSubsciptionItem(SubscriptionItem subItem);

		public abstract void OnApplicationPause(bool paused);

		public abstract void Init();

		public abstract void OnSubscriptionPurchased(int validThru);

		protected virtual void ResolveSubscription()
		{
			if (OnResolveSubscription__BackingField != null)
			{
				OnResolveSubscription__BackingField();
			}
		}
	}
}

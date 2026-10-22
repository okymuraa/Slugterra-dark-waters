using System;
using Slugterra.Domain.Shops;

namespace Slugterra.InAppPurchases
{
	public class SubscriptionContextGold : SubscriptionContext
	{
		public int countPay;

		public int donePay;

		public int periodPay;

		public SubscriptionContextGold(SubscriptionItem sItem, string nameSub, DateTime dateStart, int periodSub, int countPay, int donePay)
			: base(sItem, nameSub, dateStart, periodSub)
		{
			this.countPay = countPay;
			this.donePay = donePay;
			if (countPay > 0)
			{
				periodPay = periodSub / countPay;
			}
			else
			{
				periodPay = 0;
			}
		}

		public bool IsSubscriptionActive()
		{
			return donePay < countPay;
		}
	}
}

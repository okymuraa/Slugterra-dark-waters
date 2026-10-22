using System.Collections.Generic;
using UnityEngine;

namespace AppsMinistry.InAppPurchases
{
	public interface IPurchaseContext
	{
		GameObject GameObject { get; }

		List<string> GetProductsIds(InAppMarket market);
	}
}

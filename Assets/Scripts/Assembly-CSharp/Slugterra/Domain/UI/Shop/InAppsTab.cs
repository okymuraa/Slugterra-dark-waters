using System.Collections.Generic;
using AppsMinistry.Domain;
using AppsMinistry.InAppPurchases;

namespace Slugterra.Domain.UI.Shop
{
	public class InAppsTab : DomainObject
	{
		private List<InAppPurchase> _items;

		public List<InAppPurchase> Items
		{
			get
			{
				return _items;
			}
		}

		public InAppsTab()
		{
			_items = new List<InAppPurchase>();
		}
	}
}

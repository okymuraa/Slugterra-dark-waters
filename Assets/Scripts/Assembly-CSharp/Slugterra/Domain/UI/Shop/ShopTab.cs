using AppsMinistry.Domain;

namespace Slugterra.Domain.UI.Shop
{
	public class ShopTab : DomainObject
	{
		private string _header;

		public string Header
		{
			get
			{
				return _header;
			}
		}

		public ShopTab(string header)
		{
			_header = header;
		}
	}
}

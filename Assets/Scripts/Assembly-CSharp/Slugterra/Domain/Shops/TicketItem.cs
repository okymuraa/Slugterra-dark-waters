using AppsMinistry.Domain;

namespace Slugterra.Domain.Shops
{
	public class TicketItem : DomainObject
	{
		private string _tickets;

		private string _sku;

		private string _title;

		private bool _isBestSeller;

		private bool _isSale;

		public string Tickets
		{
			get
			{
				return _tickets;
			}
		}

		public string SKU
		{
			get
			{
				return _sku;
			}
		}

		public string Title
		{
			get
			{
				return _title;
			}
		}

		public bool IsBestSeller
		{
			get
			{
				return _isBestSeller;
			}
		}

		public bool IsSale
		{
			get
			{
				return _isSale;
			}
		}

		public TicketItem(string title, string tickets, string money, bool isSale = false, bool isBestSeller = false)
		{
			_title = title;
			_tickets = tickets;
			_sku = money;
			_isSale = isSale;
			_isBestSeller = isBestSeller;
		}

		public TicketItem(string title, float tickets, float money, bool isSale = false, bool isBestSeller = false)
		{
			_title = title;
			_tickets = string.Format("+{0}", tickets);
			_sku = string.Format("{0}", money);
			_isSale = isSale;
			_isBestSeller = isBestSeller;
		}
	}
}

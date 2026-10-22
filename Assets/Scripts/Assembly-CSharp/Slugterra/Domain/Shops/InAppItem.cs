using AppsMinistry.Domain;

namespace Slugterra.Domain.Shops
{
	public class InAppItem : DomainObject
	{
		private string _gold;

		private string _money;

		private string _title;

		private bool _isBestSeller;

		private int _isSale;

		public string Gold
		{
			get
			{
				return _gold;
			}
		}

		public string Currency
		{
			get
			{
				return _money;
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

		public int IsSale
		{
			get
			{
				return _isSale;
			}
		}

		public InAppItem(string title, string gold, string money, int isSale = 0, bool isBestSeller = false)
		{
			_title = title;
			_gold = gold;
			_money = money;
			_isSale = isSale;
			_isBestSeller = isBestSeller;
		}

		public InAppItem(string title, float gold, float money, int isSale = 0, bool isBestSeller = false)
		{
			_title = title;
			_gold = string.Format("+{0}", gold);
			_money = string.Format("{0}", money);
			_isSale = isSale;
			_isBestSeller = isBestSeller;
		}
	}
}

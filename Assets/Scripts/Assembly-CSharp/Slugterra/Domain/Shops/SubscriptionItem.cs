using System;
using AppsMinistry.Domain;

namespace Slugterra.Domain.Shops
{
	public abstract class SubscriptionItem : DomainObject
	{
		protected int _durationDays;

		protected string _sku;

		protected string _type;

		protected string _bondType;

		protected string _desription;

		protected SubscriptionItemState _state;

		protected DateTime _expirationTime = DateTime.MinValue;

		protected bool _isBestSeller;

		protected bool _isSale;

		public int DurationDays
		{
			get
			{
				return _durationDays;
			}
			set
			{
				_durationDays = value;
			}
		}

		public string SKU
		{
			get
			{
				return _sku;
			}
		}

		public string Type
		{
			get
			{
				return _type;
			}
		}

		public string BondType
		{
			get
			{
				return _bondType;
			}
		}

		public string Description
		{
			get
			{
				return _desription;
			}
		}

		public SubscriptionItemState State
		{
			get
			{
				return _state;
			}
			set
			{
				if (_state != value)
				{
					_state = value;
					switch (value)
					{
					case SubscriptionItemState.AVAILABLE:
						_expirationTime = DateTime.MinValue;
						break;
					}
					CommitAndNotify();
				}
			}
		}

		public DateTime ExpirationTime
		{
			get
			{
				return _expirationTime;
			}
			set
			{
				if (value.CompareTo(_expirationTime) > 0)
				{
					_expirationTime = value;
				}
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

		public SubscriptionItem(string sku, int days)
		{
			_sku = sku;
			_durationDays = days;
		}

		public SubscriptionItem(string type, string bondType, string description, string sku, int days)
		{
			_type = type;
			_bondType = bondType;
			_desription = description;
			_sku = sku;
			_durationDays = days;
		}

		public SubscriptionItem(string title, string sku, SubscriptionItemState state, DateTime expirationTime, bool isSale = false, bool isBestSeller = false)
		{
			_sku = sku;
			_state = state;
			_expirationTime = expirationTime;
			_isSale = isSale;
			_isBestSeller = isBestSeller;
		}

		public override string ToString()
		{
			return string.Format("[SubscriptionItem: DurationDays={0}, SKU={1}, IsValid={2}, ExpirationTime={3}]", DurationDays, SKU, State, ExpirationTime, IsBestSeller, IsSale);
		}
	}
}

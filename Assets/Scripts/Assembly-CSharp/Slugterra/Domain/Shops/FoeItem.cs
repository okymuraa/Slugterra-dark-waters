using AppsMinistry.Domain;

namespace Slugterra.Domain.Shops
{
	public class FoeItem : DomainObject
	{
		private string _type;

		private int _level;

		private bool _isLocked;

		private bool _isBought;

		public string Type
		{
			get
			{
				return _type;
			}
			set
			{
				_type = value;
			}
		}

		public int Level
		{
			get
			{
				return _level;
			}
			set
			{
				_level = value;
				CommitAndNotify();
			}
		}

		public bool IsLocked
		{
			get
			{
				return _isLocked;
			}
			set
			{
				_isLocked = value;
				CommitAndNotify();
			}
		}

		public bool IsBought
		{
			get
			{
				return _isBought;
			}
			set
			{
				_isBought = value;
				CommitAndNotify();
			}
		}

		public FoeItem()
		{
		}

		public FoeItem(string type, int level, bool isLocked, bool isBought)
		{
			_type = type;
			_level = level;
			_isLocked = isLocked;
			_isBought = isBought;
		}
	}
}

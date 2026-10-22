using AppsMinistry.Domain;
using Newtonsoft.Json;
using Slugterra.Configuration;

namespace Slugterra.Domain.Shops
{
	public class ArmorItem : DomainObject
	{
		private ArmorType _type;

		private bool _isBackpack;

		private int _level;

		private bool _isLocked;

		private bool _isJustUnlocked;

		private bool _isBought;

		private bool _isOldBought;

		private int _recommendedLevel = -1;

		private bool _isRequired;

		public ArmorType Type
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

		public bool IsBackpack
		{
			get
			{
				return _isBackpack;
			}
			set
			{
				_isBackpack = value;
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
				if (_isLocked && !value)
				{
					_isJustUnlocked = true;
				}
				_isLocked = value;
				CommitAndNotify();
			}
		}

		public bool IsJustUnlocked
		{
			get
			{
				return _isJustUnlocked;
			}
			set
			{
				_isJustUnlocked = value;
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
				if (value)
				{
					_isJustUnlocked = false;
				}
				_isBought = value;
				CommitAndNotify();
			}
		}

		public bool IsOldBought
		{
			get
			{
				return _isOldBought;
			}
			set
			{
				_isOldBought = value;
			}
		}

		[JsonIgnore]
		public int RecommendedLevel
		{
			get
			{
				return _recommendedLevel;
			}
			set
			{
				_recommendedLevel = value;
			}
		}

		[JsonIgnore]
		public bool IsRequired
		{
			get
			{
				return _isRequired;
			}
			set
			{
				_isRequired = value;
			}
		}

		public ArmorItem()
		{
		}

		public ArmorItem(ArmorType type, int level, bool isLocked, bool isBought)
		{
			_type = type;
			_level = level;
			_isLocked = isLocked;
			_isBought = isBought;
		}
	}
}

using AppsMinistry.Domain;
using Newtonsoft.Json;
using Slugterra.Configuration;
using UnityEngine;

namespace Slugterra.Domain.Shops
{
	public class BlasterItem : DomainObject
	{
		private BlasterType _type;

		private bool _isBackpack;

		private int _level;

		private bool _isLocked;

		private bool _isJustUnlocked;

		private bool _isBought;

		private float _energy;

		private int _recommendedLevel = -1;

		private bool _isRequired;

		public BlasterType Type
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

		public float Energy
		{
			get
			{
				return _energy;
			}
			set
			{
				_energy = Mathf.Clamp(value, 0f, float.MaxValue);
				CommitAndNotify();
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

		public BlasterItem()
		{
		}

		public BlasterItem(BlasterType type, int level = 0, bool isLocked = false, bool isBought = true, float energy = 0f)
		{
			_type = type;
			_level = level;
			_isLocked = isLocked;
			_isBought = isBought;
			_energy = energy;
		}
	}
}

using System.Collections.Generic;
using AppsMinistry.Domain;
using Slugterra.Configuration;

namespace Slugterra.Domain.Shops
{
	public class PackItem : DomainObject
	{
		private PackType _type;

		private List<string> _levels = new List<string>();

		public PackType Type
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

		public List<string> Levels
		{
			get
			{
				return _levels;
			}
			set
			{
				if (value != null)
				{
					_levels = value;
					CommitAndNotify();
				}
			}
		}

		public PackItem()
		{
		}

		public PackItem(PackType type)
		{
			_type = type;
		}

		public bool IsBoughtForLevel(string levelName)
		{
			return _levels.Contains(levelName);
		}

		public void AddLevel(string levelName)
		{
			if (!string.IsNullOrEmpty(levelName) && !_levels.Contains(levelName))
			{
				_levels.Add(levelName);
				CommitAndNotify();
			}
		}
	}
}

using AppsMinistry.Domain;
using Slugterra.Configuration;

namespace Slugterra.Domain.Shops
{
	public class ConsumableItem : DomainObject
	{
		private ConsumableType _type;

		private int _count;

		private bool isMultiplayer;

		private const int netCountLimit = 15;

		private int _curNetCount;

		public ConsumableType Type
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

		public int Count
		{
			get
			{
				if (isMultiplayer)
				{
					return _curNetCount;
				}
				return _count;
			}
			set
			{
				_count = value;
				CommitAndNotify();
			}
		}

		public ConsumableItem()
		{
		}

		public ConsumableItem(ConsumableType type, int count)
		{
			_type = type;
			Count = count;
		}

		public void UseBoost()
		{
			_count--;
			_curNetCount--;
			CommitAndNotify();
		}

		public void SetMultiplayerGame(bool value)
		{
			isMultiplayer = value;
		}

		public void UpdateNetCount()
		{
			_curNetCount = _count;
			if (_curNetCount > 15)
			{
				_curNetCount = 15;
			}
		}
	}
}

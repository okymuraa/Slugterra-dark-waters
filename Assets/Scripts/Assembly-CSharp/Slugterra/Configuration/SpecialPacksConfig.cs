using System.Collections.Generic;
using AppsMinistry.Domain.Observer;

namespace Slugterra.Configuration
{
	public class SpecialPacksConfig : Observable
	{
		public string Id;

		public int ShowId;

		public string Name;

		public int Price;

		public List<SpecialPacksItem> Items = new List<SpecialPacksItem>();

		private bool _isBought;

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
	}
}

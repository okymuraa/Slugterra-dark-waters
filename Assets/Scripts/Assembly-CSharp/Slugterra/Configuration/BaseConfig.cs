using System.Collections.Generic;

namespace Slugterra.Configuration
{
	public class BaseConfig
	{
		public string title = string.Empty;

		public string description = string.Empty;

		public bool isUnLocked;

		public bool isShowInStore;

		public List<string> userData = new List<string>();
	}
}

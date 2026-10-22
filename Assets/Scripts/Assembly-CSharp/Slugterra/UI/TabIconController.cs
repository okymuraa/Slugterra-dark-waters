using AppsMinistry.UI;

namespace Slugterra.UI
{
	public class TabIconController : UIController<TabIconView>
	{
		private bool _isActive;

		public bool Active
		{
			set
			{
				if (_isActive != value)
				{
					_isActive = value;
					View.ApplyModel(_isActive);
				}
			}
		}
	}
}
